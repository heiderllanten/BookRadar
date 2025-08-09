CREATE TABLE HistorialBusquedas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Autor NVARCHAR(250) NOT NULL,
    Titulo NVARCHAR(500) NOT NULL,
    AnioPublicacion INT NULL,
    Editorial NVARCHAR(500) NULL,
    FechaConsulta DATETIME2 NOT NULL
);

CREATE INDEX IX_Historial_Autor_Fecha ON HistorialBusquedas(Autor, FechaConsulta);
