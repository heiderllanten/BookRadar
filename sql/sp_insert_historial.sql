CREATE PROCEDURE sp_InsertHistorial
    @Autor NVARCHAR(250),
    @Titulo NVARCHAR(500),
    @AnioPublicacion INT = NULL,
    @Editorial NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @now DATETIME2 = SYSUTCDATETIME();
    -- Evita guardar si existe el mismo autor+titulo en el último minuto
    IF EXISTS (
        SELECT 1 FROM HistorialBusquedas
        WHERE Autor = @Autor
          AND Titulo = @Titulo
          AND FechaConsulta >= DATEADD(MINUTE, -1, @now)
    )
    BEGIN
        RETURN; -- no insertar
    END

    INSERT INTO HistorialBusquedas (Autor, Titulo, AnioPublicacion, Editorial, FechaConsulta)
    VALUES (@Autor, @Titulo, @AnioPublicacion, @Editorial, @now);
END
