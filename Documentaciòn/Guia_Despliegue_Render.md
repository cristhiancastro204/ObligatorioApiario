# Guía de Despliegue en Render.com (ASP.NET Core + PostgreSQL)

Este documento detalla todos los pasos y configuraciones técnicas que se realizaron para poder subir el proyecto `ObligatorioApiario` (desarrollado en C# .NET 9) a la plataforma en la nube Render.com de forma gratuita.

## 1. Migración de Base de Datos (SQL Server a PostgreSQL)
Render.com no ofrece alojamiento gratuito para bases de datos SQL Server, pero sí para **PostgreSQL**. Por lo tanto, el primer paso fue adaptar el proyecto:

1. **Cambio de Paquetes NuGet:**
   - Se desinstaló el paquete `Microsoft.EntityFrameworkCore.SqlServer`.
   - Se instaló el paquete `Npgsql.EntityFrameworkCore.PostgreSQL` (versión 9.0.0).

2. **Re-generación de Migraciones:**
   - Como las migraciones previas contenían código específico de SQL Server (como los campos `IDENTITY`), se eliminó por completo la carpeta `Migrations`.
   - Se ejecutó el comando `dotnet ef migrations add InitialCreatePostgres` para generar una nueva migración limpia y compatible con PostgreSQL.

## 2. Configuración para Docker
Render.com requiere un archivo `Dockerfile` para saber cómo compilar y ejecutar aplicaciones .NET en sus servidores Linux.

1. **Creación del `Dockerfile`:**
   Se generó un archivo en la raíz del proyecto que utiliza las imágenes oficiales de Microsoft (`mcr.microsoft.com/dotnet/sdk:9.0` para compilar y `mcr.microsoft.com/dotnet/aspnet:9.0` para ejecutar). El contenedor expone el puerto `8080`.
2. **Creación de `.dockerignore`:**
   Para evitar que carpetas locales pesadas (`/bin`, `/obj`) se suban a la nube y causen conflictos durante el despliegue.

## 3. Adaptación del String de Conexión (El problema de parseo)
Render.com entrega la URL de la base de datos en formato de URI web (ej: `postgres://usuario:contraseña@servidor/bd`). Sin embargo, el conector `Npgsql` de Entity Framework espera el formato clásico de ADO.NET (`Host=...;Username=...;Password=...;Database=...`). 

Para evitar un error en tiempo de ejecución (`System.ArgumentException: Format of the initialization string does not conform to specification`), se modificó el archivo `Program.cs` para interceptar la URL y traducirla automáticamente:

```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrEmpty(connectionString))
{
    // Limpiamos comillas o espacios accidentales
    connectionString = connectionString.Trim(' ', '"', '\'');
    
    // Si detectamos que es la URL de Render, la traducimos:
    if (connectionString.StartsWith("postgres://") || connectionString.StartsWith("postgresql://"))
    {
        var uri = new Uri(connectionString);
        var userInfo = uri.UserInfo.Split(':');
        var password = userInfo.Length > 1 ? userInfo[1] : "";
        var port = uri.Port > 0 ? uri.Port : 5432;
        connectionString = $"Host={uri.Host};Port={port};Database={uri.LocalPath.TrimStart('/')};Username={userInfo[0]};Password={password};Ssl Mode=Prefer;Trust Server Certificate=true;";
    }
}
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
```

## 4. Pasos en la Plataforma Render.com
Con el código preparado y subido a GitHub, los pasos finales en la consola de Render fueron:

1. **Crear PostgreSQL:** Se creó una instancia de Base de Datos PostgreSQL (plan gratuito) y se copió su **Internal Database URL**.
2. **Crear Web Service:** Se conectó el repositorio de GitHub como un nuevo *Web Service* usando Docker.
3. **Variables de Entorno:** Se configuró una variable llamada `ConnectionStrings__DefaultConnection` (doble guión bajo) y se le asignó como valor la URL interna de la base de datos copiada en el primer paso.

*(Nota: En `Program.cs` está configurado `context.Database.Migrate()`, lo que garantiza que las tablas se creen automáticamente en la nube apenas el servidor arranque).*
