-- ==========================================================
-- SCRIPT 2: Procedimientos y Funciones
-- ==========================================================

-- 1. Procedimiento para registrar un nuevo Apiario
CREATE PROCEDURE sp_RegistrarApiario
    @Longitud DECIMAL(9,6),
    @Latitud DECIMAL(9,6),
    @Zona VARCHAR(50)
AS
BEGIN
    INSERT INTO Apiarios (Longitud, Latitud, Zona, Cant_Colmenas)
    VALUES (@Longitud, @Latitud, @Zona, 0);
END;
GO

-- 2. Procedimiento para registrar una visita completa a un apiario
CREATE PROCEDURE sp_RegistrarVisitaCompleta
    @Fecha DATE,
    @Temporada VARCHAR(50),
    @Observaciones VARCHAR(255),
    @CI_Apicultor INT,
    @ID_Apiario INT,
    @H_Salida TIME,
    @H_Llegada TIME,
    @Clima VARCHAR(50)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ID_Visita INT;
        
        INSERT INTO Visitas (Fecha, Temporada, Observaciones, CI_Apicultor)
        VALUES (@Fecha, @Temporada, @Observaciones, @CI_Apicultor);
        
        SET @ID_Visita = SCOPE_IDENTITY();
        
        INSERT INTO Revisa (ID_Visita, ID_Apiario, H_Salida, H_Llegada, Clima)
        VALUES (@ID_Visita, @ID_Apiario, @H_Salida, @H_Llegada, @Clima);
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- 3. Procedimiento para cambiar el estado de una colmena
CREATE PROCEDURE sp_ActualizarEstadoColmena
    @ID_Colmena INT,
    @NuevoEstado VARCHAR(50)
AS
BEGIN
    UPDATE Colmenas
    SET Estado = @NuevoEstado
    WHERE ID_Colmena = @ID_Colmena;
END;
GO

-- 4. Procedimiento para aplicar tratamiento a colmena
CREATE PROCEDURE sp_AplicarTratamientoColmena
    @ID_Colmena INT,
    @ID_Tratamiento INT
AS
BEGIN
    -- Verifica si ya se aplicó ese tratamiento
    IF NOT EXISTS (SELECT 1 FROM Aplica WHERE ID_Colmena = @ID_Colmena AND ID_Tratamiento = @ID_Tratamiento)
    BEGIN
        INSERT INTO Aplica (ID_Colmena, ID_Tratamiento)
        VALUES (@ID_Colmena, @ID_Tratamiento);
    END
    ELSE
    BEGIN
        RAISERROR('El tratamiento ya fue aplicado a esta colmena', 16, 1);
    END
END;
GO

-- 5. Función escalar para obtener la cantidad de colmenas de un apiario
CREATE FUNCTION fn_CantidadColmenasApiario (@ID_Apiario INT)
RETURNS INT
AS
BEGIN
    DECLARE @Cantidad INT;
    SELECT @Cantidad = Cant_Colmenas
    FROM Apiarios
    WHERE ID_Apiario = @ID_Apiario;
    
    RETURN ISNULL(@Cantidad, 0);
END;
GO

-- 6. Función escalar para obtener la producción total de un apiario (en Kg)
CREATE FUNCTION fn_ProduccionTotalApiario (@ID_Apiario INT)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @TotalKg DECIMAL(10,2);
    
    SELECT @TotalKg = SUM(b.Cantidad_Miel)
    FROM Realiza r
    INNER JOIN Genera g ON r.ID_Cosecha = g.ID_Cosecha
    INNER JOIN Barriles b ON g.ID_Barril = b.ID_Barril
    WHERE r.ID_Apiario = @ID_Apiario;
    
    RETURN ISNULL(@TotalKg, 0);
END;
GO

-- 7. Función de tabla en línea para obtener las colmenas de un estado específico
CREATE FUNCTION fn_ColmenasPorEstado (@Estado VARCHAR(50))
RETURNS TABLE
AS
RETURN
(
    SELECT ID_Colmena, Cant_Bastidores, Fortaleza_Abejas
    FROM Colmenas
    WHERE Estado = @Estado
);
GO

-- 8. Función de tabla en línea para listar herramientas usadas en una visita
CREATE FUNCTION fn_HerramientasPorVisita (@ID_Visita INT)
RETURNS TABLE
AS
RETURN
(
    SELECT h.ID_Herramienta, h.Nombre, h.Tipo, p.Cantidad
    FROM Planifica p
    INNER JOIN Herramientas h ON p.ID_Herramienta = h.ID_Herramienta
    WHERE p.ID_Visita = @ID_Visita
);
GO
