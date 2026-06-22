# Descripción y Selección de Herramientas

Para el desarrollo del proyecto y la gestión de su ciclo de vida, se ha seleccionado un conjunto de herramientas alineadas con los requisitos del cliente, las restricciones de despliegue y la viabilidad técnica y operativa. A continuación, se describen y justifican las elecciones del equipo:

## 1. Entorno de Desarrollo Integrado (IDE)
* **Herramienta seleccionada**: Visual Studio 2022 / Visual Studio Code.
* **Descripción**: Entornos de desarrollo integrados optimizados para la compilación, depuración y pruebas de aplicaciones basadas en el ecosistema .NET.
* **Justificación**:
  * **Integración nativa**: Soporte completo para el lenguaje C# y el SDK de .NET 9.0, automatizando tareas como el andamiaje (*scaffolding*) de controladores y vistas Razor, y la administración de dependencias NuGet.
  * **Productividad**: Ofrecen herramientas avanzadas de diagnóstico de memoria, depuradores integrados de alta precisión y herramientas de refactorización que aceleran la fase de codificación y reducen la tasa de fallos.

## 2. Framework y Arquitectura de Software
* **Herramienta seleccionada**: ASP.NET Core MVC (.NET 9.0).
* **Descripción**: Framework web multiplataforma, de código abierto y alto rendimiento desarrollado por Microsoft, que implementa de forma nativa el patrón de diseño arquitectónico Modelo-Vista-Controlador (MVC).
* **Justificación**:
  * **Separación de responsabilidades**: Al dividir el sistema en Modelos (lógica y datos), Vistas (interfaz de usuario) y Controladores (flujo de control), se incrementa la mantenibilidad y facilita la división de tareas dentro del equipo de desarrollo.
  * **Seguridad y Rendimiento**: Cuenta con optimizaciones a nivel de compilador en .NET 9.0 para alta concurrencia y provee middleware de seguridad integrado contra ataques comunes (como XSS y XSRF/CSRF mediante tokens automáticos).

## 3. Mapeador Objeto-Relacional (ORM)
* **Herramienta seleccionada**: Entity Framework Core (EF Core 9).
* **Descripción**: Mapeador objeto-relacional (ORM) ligero y extensible para .NET que permite interactuar con bases de datos relacionales utilizando entidades y código C# tipado.
* **Justificación**:
  * **Enfoque Code-First**: Facilita la creación del esquema de la base de datos a partir de clases C#, gestionando los cambios en la estructura de forma organizada y reversible mediante migraciones (*migrations*).
  * **Independencia de la Base de Datos**: Abstrae las consultas SQL subyacentes, lo que permite cambiar el motor de base de datos sin alterar la lógica de negocio del controlador.

## 4. Motor de Base de Datos (DBMS)
* **Herramienta seleccionada**: SQLite (Desarrollo y Despliegue) / PostgreSQL (Producción).
* **Descripción**: SQLite es un motor de base de datos relacional autocontenido en un solo archivo que no requiere un servidor. PostgreSQL es un gestor de base de datos relacional clásico, robusto y altamente escalable.
* **Justificación**:
  * **Restricción de Despliegue (Hosting)**: La plataforma de alojamiento web elegida para albergar el proyecto final impone restricciones de compatibilidad de servicios externos. La aceptación nativa de SQLite y PostgreSQL en dicho entorno motivó su selección.
  * **Portabilidad y Costo Cero (SQLite)**: SQLite al almacenar los datos en un solo archivo físico dentro del proyecto web elimina la necesidad de configurar, mantener y abonar un servicio de base de datos en la nube independiente, lo que simplifica y economiza radicalmente el despliegue del proyecto.
  * **Escalabilidad Transparente**: Al estar estructurado sobre EF Core, si el volumen de colmenas y operaciones apícolas crece significativamente, migrar hacia **PostgreSQL** para mayor volumen de datos concurrente requiere una mínima modificación de configuración (cambiando únicamente la llamada al proveedor en el `Program.cs`).

## 5. Sistema de Control de Versiones y Repositorio
* **Herramienta seleccionada**: Git (VCS) y GitHub (Servicio de alojamiento).
* **Descripción**: Git es la herramienta de control de versiones distribuida y GitHub es la plataforma en la nube para almacenar repositorios y facilitar la colaboración.
* **Justificación**:
  * **Desarrollo Colaborativo**: Permite el trabajo en paralelo de los desarrolladores a través de ramas (*branches*), posibilitando la integración continua y el control sobre los cambios de código.
  * **Trazabilidad**: Permite auditar de forma exacta quién, cuándo y por qué modificó cada componente del sistema apícola.

## 6. Prototipado y Maquetación de Interfaces (UI/UX)
* **Herramienta seleccionada**: Stitch (stitch.withgoogle.com).
* **Descripción**: Herramienta de diseño y prototipado visual en la nube para la construcción ágil de interfaces de usuario.
* **Justificación**:
  * **Fidelidad y Estructura de UI**: Permite maquetar las pantallas del sistema web (como el Dashboard y el listado/detalles de tareas) antes de su codificación en Razor/HTML, facilitando la toma de decisiones estéticas y ergonómicas (como la elección de paletas de colores en tonos tierra y la correcta colocación de los widgets).
  * **Alineación del Equipo**: Ofrece una referencia visual compartida que minimiza la ambigüedad en el proceso de desarrollo de las vistas front-end.
