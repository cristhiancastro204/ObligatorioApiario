-- ======================================================================
-- SCRIPT 1: Creación de Tablas, Restricciones, Índices, Triggers y Datos
-- ======================================================================

-- 1. CREACIÓN DE TIPOS DE DATOS
CREATE TYPE TipoNombre FROM VARCHAR(100) NOT NULL;
CREATE TYPE TipoObservacion FROM VARCHAR(255);
GO

-- 2. CREACIÓN DE TABLAS Y RESTRICCIONES
CREATE TABLE Apicultor (
    CI_Apicultor INT PRIMARY KEY,
    Nombre TipoNombre,
    Cargo VARCHAR(20),
    CONSTRAINT CK_Apicultor_Cargo CHECK (Cargo IN ('Administrador de Apiario', 'Apicultor de Campo'))
);

CREATE TABLE Apicultor_Telefono (
    CI_Apicultor INT,
    Telefono INT,
    CONSTRAINT PK_Apicultor_Telefono PRIMARY KEY (CI_Apicultor, Telefono),
    CONSTRAINT FK_ApTel_Apicultor FOREIGN KEY (CI_Apicultor) REFERENCES Apicultor(CI_Apicultor)
);

CREATE TABLE Barriles (
    ID_Barril INT IDENTITY(1,1) PRIMARY KEY,
    Cantidad_Miel DECIMAL(10,2) NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    CI_Apicultor INT,
    CONSTRAINT FK_Barriles_Apicultor FOREIGN KEY (CI_Apicultor) REFERENCES Apicultor(CI_Apicultor)
);

CREATE TABLE Herramientas (
    ID_Herramienta INT IDENTITY(1,1) PRIMARY KEY,
    Nombre TipoNombre,
    Descripcion VARCHAR(255),
    Tipo VARCHAR(50)
);

CREATE TABLE Visitas (
    ID_Visita INT IDENTITY(1,1) PRIMARY KEY,
    Fecha DATE NOT NULL,
    Temporada VARCHAR(50),
    Observaciones TipoObservacion,
    CI_Apicultor INT,
    CONSTRAINT FK_Visitas_Apicultor FOREIGN KEY (CI_Apicultor) REFERENCES Apicultor(CI_Apicultor)
);

CREATE TABLE Cosecha (
    ID_Cosecha INT IDENTITY(1,1) PRIMARY KEY,
    Observaciones TipoObservacion,
    Estado VARCHAR(50) NOT NULL
);

CREATE TABLE Apiarios (
    ID_Apiario INT IDENTITY(1,1) PRIMARY KEY,
    Longitud DECIMAL(9,6) NOT NULL,
    Latitud DECIMAL(9,6) NOT NULL,
    Zona VARCHAR(50),
    Cant_Colmenas INT NOT NULL DEFAULT 0
);

CREATE TABLE Colmenas (
    ID_Colmena INT IDENTITY(1,1) PRIMARY KEY,
    Estado VARCHAR(50) NOT NULL,
    Cant_Bastidores INT,
    Cant_Abejas_Estimada INT,
    Fortaleza_Abejas VARCHAR(50),
    CONSTRAINT CHK_Fortaleza_Abejas CHECK (Fortaleza_Abejas IN ('Fuerte', 'Media', 'Débil'))
);

CREATE TABLE Tratamientos (
    ID_Tratamiento INT IDENTITY(1,1) PRIMARY KEY,
    Fecha_Inicio DATE NOT NULL,
    Cant_Dosis INT NOT NULL,
    Observaciones TipoObservacion
);

CREATE TABLE Reinas (
    ID_Reina INT IDENTITY(1,1) PRIMARY KEY,
    Estado VARCHAR(50) NOT NULL,
    Nivel_Prod VARCHAR(50)
);

CREATE TABLE Planifica (
    ID_Visita INT,
    ID_Herramienta INT,
    Cantidad INT,
    CONSTRAINT PK_Planifica PRIMARY KEY (ID_Visita, ID_Herramienta),
    CONSTRAINT FK_Planifica_Visita FOREIGN KEY (ID_Visita) REFERENCES Visitas(ID_Visita),
    CONSTRAINT FK_Planifica_Herramienta FOREIGN KEY (ID_Herramienta) REFERENCES Herramientas(ID_Herramienta)
);

CREATE TABLE Genera (
    ID_Barril INT,
    ID_Cosecha INT,
    CONSTRAINT PK_Genera PRIMARY KEY (ID_Barril, ID_Cosecha),
    CONSTRAINT FK_Genera_Barril FOREIGN KEY (ID_Barril) REFERENCES Barriles(ID_Barril),
    CONSTRAINT FK_Genera_Cosecha FOREIGN KEY (ID_Cosecha) REFERENCES Cosecha(ID_Cosecha)
);

CREATE TABLE Realiza (
    ID_Cosecha INT,
    ID_Apiario INT,
    Fecha_Cosecha DATE NOT NULL,
    Tipo_Miel VARCHAR(50),
    Cant_Barriles INT NOT NULL,
    CONSTRAINT PK_Realiza PRIMARY KEY (ID_Cosecha, ID_Apiario),
    CONSTRAINT FK_Realiza_Cosecha FOREIGN KEY (ID_Cosecha) REFERENCES Cosecha(ID_Cosecha),
    CONSTRAINT FK_Realiza_Apiario FOREIGN KEY (ID_Apiario) REFERENCES Apiarios(ID_Apiario)
);

CREATE TABLE Revisa (
    ID_Visita INT,
    ID_Apiario INT,
    H_Salida TIME,
    H_Llegada TIME,
    Clima VARCHAR(50),
    CONSTRAINT PK_Revisa PRIMARY KEY (ID_Visita, ID_Apiario),
    CONSTRAINT FK_Revisa_Visita FOREIGN KEY (ID_Visita) REFERENCES Visitas(ID_Visita),
    CONSTRAINT FK_Revisa_Apiario FOREIGN KEY (ID_Apiario) REFERENCES Apiarios(ID_Apiario)
);

CREATE TABLE Tiene (
    ID_Apiario INT,
    ID_Colmena INT,
    Fecha_Instalacion DATE,
    Sector VARCHAR(50),
    Fila VARCHAR(10),
    Columna VARCHAR(10),
    CONSTRAINT PK_Tiene PRIMARY KEY (ID_Apiario, ID_Colmena),
    CONSTRAINT FK_Tiene_Apiario FOREIGN KEY (ID_Apiario) REFERENCES Apiarios(ID_Apiario),
    CONSTRAINT FK_Tiene_Colmena FOREIGN KEY (ID_Colmena) REFERENCES Colmenas(ID_Colmena)
);

CREATE TABLE Aplica (
    ID_Colmena INT,
    ID_Tratamiento INT,
    CONSTRAINT PK_Aplica PRIMARY KEY (ID_Colmena, ID_Tratamiento),
    CONSTRAINT FK_Aplica_Colmena FOREIGN KEY (ID_Colmena) REFERENCES Colmenas(ID_Colmena),
    CONSTRAINT FK_Aplica_Tratamiento FOREIGN KEY (ID_Tratamiento) REFERENCES Tratamientos(ID_Tratamiento)
);

CREATE TABLE Asigna (
    ID_Colmena INT,
    ID_Reina INT,
    Fecha_Asignacion DATETIME,
    CONSTRAINT PK_Asigna PRIMARY KEY (ID_Colmena, ID_Reina),
    CONSTRAINT FK_Asigna_Colmena FOREIGN KEY (ID_Colmena) REFERENCES Colmenas(ID_Colmena),
    CONSTRAINT FK_Asigna_Reina FOREIGN KEY (ID_Reina) REFERENCES Reinas(ID_Reina)
);
GO

-- 3. ÍNDICES
CREATE INDEX Indice_Estado_Colmena ON Colmenas (Estado);
CREATE INDEX Indice_Fortaleza_Colmena ON Colmenas (Fortaleza_Abejas);
CREATE INDEX Indice_Tiene_IdApiario ON Tiene (ID_Apiario);
CREATE INDEX Indice_FechaInicio_Tratamiento ON Tratamientos (Fecha_Inicio);
CREATE INDEX Indice_NivelProd_Reinas ON Reinas (Nivel_Prod);
GO

-- 4. DISPARADORES (TRIGGERS)
CREATE TRIGGER trg_AumentarCantColmenas
ON Tiene
AFTER INSERT
AS
BEGIN
    UPDATE a
    SET a.Cant_Colmenas = a.Cant_Colmenas + 1
    FROM Apiarios a
    INNER JOIN inserted i ON a.ID_Apiario = i.ID_Apiario;
END;
GO

CREATE TRIGGER trg_DisminuirCantColmenas
ON Tiene
AFTER DELETE
AS
BEGIN
    UPDATE a
    SET a.Cant_Colmenas = a.Cant_Colmenas - 1
    FROM Apiarios a
    INNER JOIN deleted d ON a.ID_Apiario = d.ID_Apiario;
END;
GO

CREATE TRIGGER trg_ValidarReinaAsignada
ON Asigna
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM inserted i
        INNER JOIN Reinas r ON i.ID_Reina = r.ID_Reina
        WHERE r.Estado IN ('Muerta', 'Baja', 'Reina Vieja - Por sustituir')
    )
    BEGIN
        RAISERROR('No se puede asignar una reina que se encuentre dada de baja o en estado inactivo.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;
GO

CREATE TRIGGER trg_ValidarFechaVisita
ON Visitas
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (SELECT 1 FROM inserted WHERE Fecha > GETDATE())
    BEGIN
        RAISERROR('La fecha de la visita no puede ser posterior a la fecha actual.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;
GO

-- 5. JUEGO DE DATOS DE PRUEBA (VÁLIDOS)
INSERT INTO Apicultor (CI_Apicultor, Nombre, Cargo)
VALUES (41234567, 'Matias Verges', 'Administrador de Apiario');

INSERT INTO Herramientas (Nombre, Descripcion, Tipo)
VALUES
('Ahumador de acero', 'Ahumador estándar para calmar abejas', 'Manejo'),
('Palanca de alza', 'Herramienta para separar alzas y cuadros', 'Manejo'),
('Traje de apicultor', 'Overol completo con careta de protección', 'Seguridad'),
('Bastidor de madera (cuadro)', 'Insumo de repuesto con cera estampada', 'Insumo');

INSERT INTO Cosecha (Observaciones, Estado)
VALUES
('Cosecha de primavera alta en humedad', 'Completada'),
('Cosecha de fin de temporada', 'En Proceso'),
('Segunda zafra del año, excelente rendimiento', 'Completada');

INSERT INTO Apiarios (Longitud, Latitud, Zona, Cant_Colmenas)
VALUES
(-56.123456, -34.567890, 'Zona Norte - Canelones', 15),
(-55.987654, -33.123456, 'Zona Este - Maldonado', 20);

INSERT INTO Colmenas (Estado, Cant_Bastidores, Cant_Abejas_Estimada, Fortaleza_Abejas)
VALUES
('Activa', 10, 50000, 'Fuerte'),
('Activa', 9, 35000, 'Media'),
('En observation por posible peste', 8, 15000, 'Débil');

INSERT INTO Tratamientos (Fecha_Inicio, Cant_Dosis, Observaciones)
VALUES
('2026-03-10', 2, 'Tratamiento orgánico contra Varroa'),
('2026-05-15', 1, 'Suplemento nutricional de otoño');

INSERT INTO Reinas (Estado, Nivel_Prod)
VALUES
('Activa - Nueva', 'Alto'),
('Activa - Segunda Temporada', 'Medio'),
('Reina Vieja - Por sustituir', 'Bajo');

INSERT INTO Barriles (Cantidad_Miel, Precio, CI_Apicultor)
VALUES
(300.50, 1200.00, 41234567),
(280.00, 1100.00, 41234567),
(310.25, 1250.50, 41234567);

INSERT INTO Visitas (Fecha, Temporada, Observaciones, CI_Apicultor)
VALUES
('2026-03-12', 'Primavera', 'Revisión general de sanidad', 41234567),
('2026-05-20', 'Otoño', 'Preparación para el invierno', 41234567);

INSERT INTO Planifica (ID_Visita, ID_Herramienta, Cantidad)
VALUES
(1, 1, 2),
(1, 2, 3),
(2, 1, 1),
(1, 4, 20);

INSERT INTO Genera (ID_Barril, ID_Cosecha)
VALUES
(1, 1),
(2, 1),
(3, 3);

INSERT INTO Realiza (ID_Cosecha, ID_Apiario, Fecha_Cosecha, Tipo_Miel, Cant_Barriles)
VALUES
(1, 1, '2026-03-11', 'Eucaliptus', 2),
(3, 2, '2026-04-05', 'Pradera', 1);

INSERT INTO Revisa (ID_Visita, ID_Apiario, H_Salida, H_Llegada, Clima)
VALUES
(1, 1, '08:30:00', '12:00:00', 'Soleado'),
(2, 2, '14:00:00', '17:30:00', 'Nublado');

INSERT INTO Tiene (ID_Apiario, ID_Colmena, Fecha_Instalacion, Sector, Fila, Columna)
VALUES
(1, 1, '2025-10-01', 'Sector A', 'Fila 1', 'Col 3'),
(1, 2, '2025-10-01', 'Sector A', 'Fila 1', 'Col 4'),
(2, 3, '2025-11-15', 'Sector Sur', 'Fila 3', 'Col 1');

INSERT INTO Aplica (ID_Colmena, ID_Tratamiento)
VALUES
(3, 1), 
(1, 2);

INSERT INTO Asigna (ID_Colmena, ID_Reina, Fecha_Asignacion)
VALUES 
(1, 1, '2026-01-05 09:15:00'),
(2, 2, '2026-01-05 10:30:00');
GO

-- 6. JUEGO DE DATOS DE PRUEBA (INVÁLIDOS QUE DEBEN SER RECHAZADOS POR EL SISTEMA)
-- Se incluyen comentados para evitar errores al correr el script completo

/*
-- A) Falla por restricción de CHECK de Cargo (CK_Apicultor_Cargo)
INSERT INTO Apicultor (CI_Apicultor, Nombre, Cargo)
VALUES (12345678, 'Juan Perez', 'Gerente General');

-- B) Falla por restricción de CHECK de Fortaleza de abejas (CHK_Fortaleza_Abejas)
INSERT INTO Colmenas (Estado, Cant_Bastidores, Cant_Abejas_Estimada, Fortaleza_Abejas)
VALUES ('Activa', 10, 40000, 'Muy Fuerte'); -- Solo permite 'Fuerte', 'Media' o 'Débil'

-- C) Falla por restricción de Foreing Key (El Apicultor 99999999 no existe)
INSERT INTO Barriles (Cantidad_Miel, Precio, CI_Apicultor)
VALUES (300.00, 1000.00, 99999999);

-- D) Falla por disparador trg_ValidarFechaVisita (Trata de insertar una fecha en el futuro)
INSERT INTO Visitas (Fecha, Temporada, Observaciones, CI_Apicultor)
VALUES ('2099-01-01', 'Verano', 'Visita del futuro', 41234567);

-- E) Falla por disparador trg_ValidarReinaAsignada (Reina dada de baja)
INSERT INTO Asigna (ID_Colmena, ID_Reina, Fecha_Asignacion)
VALUES (3, 3, GETDATE()); -- La reina 3 está con estado 'Reina Vieja - Por sustituir'
*/
