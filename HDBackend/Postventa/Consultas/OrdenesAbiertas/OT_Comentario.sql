-- Comentarios de seguimiento de OT (motivo de retraso + tiempo estimado de cierre),
-- con catálogo de categorías editable desde base de datos.
-- Ejecutar contra 192.168.0.51 / HumayaDigital.

-- Catálogo de categorías — para agregar una nueva, solo INSERT aquí, no requiere tocar código.
CREATE TABLE Postventa.OT_Comentario_Categoria (
    Id      INT IDENTITY(1,1) PRIMARY KEY,
    Nombre  NVARCHAR(100) NOT NULL,
    Activo  BIT NOT NULL DEFAULT 1
);
GO

INSERT INTO Postventa.OT_Comentario_Categoria (Nombre) VALUES
    ('Falta de materiales'),
    ('Falta de personal'),
    ('Espera de autorización del cliente'),
    ('Refacción en tránsito'),
    ('Garantía en revisión'),
    ('Otro');
GO

-- Historial de comentarios de seguimiento por OT.
CREATE TABLE Postventa.OT_Comentario_Historial (
    Id                   INT IDENTITY(1,1) PRIMARY KEY,
    OrdenTrabajoId       INT NOT NULL,
    CategoriaId          INT NOT NULL REFERENCES Postventa.OT_Comentario_Categoria(Id),
    Comentario           NVARCHAR(MAX) NOT NULL,
    FechaEstimadaCierre  DATE NULL,
    FechaRegistro        DATETIME NOT NULL DEFAULT GETDATE(),
    UsuarioRegistro      NVARCHAR(100) NULL
);
GO

CREATE INDEX IX_OT_Comentario_Historial_OrdenTrabajoId ON Postventa.OT_Comentario_Historial (OrdenTrabajoId, FechaRegistro DESC);
GO

CREATE PROCEDURE Postventa.sp_OT_Comentario_Categorias_Listado
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Nombre
    FROM Postventa.OT_Comentario_Categoria
    WHERE Activo = 1
    ORDER BY Nombre;
END
GO

CREATE PROCEDURE Postventa.sp_OT_Comentario_Listado
    @OrdenTrabajoId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        h.Id,
        h.OrdenTrabajoId,
        h.CategoriaId,
        c.Nombre AS CategoriaNombre,
        h.Comentario,
        h.FechaEstimadaCierre,
        h.FechaRegistro,
        h.UsuarioRegistro
    FROM Postventa.OT_Comentario_Historial h
    INNER JOIN Postventa.OT_Comentario_Categoria c ON h.CategoriaId = c.Id
    WHERE h.OrdenTrabajoId = @OrdenTrabajoId
    ORDER BY h.FechaRegistro DESC;
END
GO

CREATE PROCEDURE Postventa.sp_OT_Comentario_Guardar
    @OrdenTrabajoId INT,
    @CategoriaId INT,
    @Comentario NVARCHAR(MAX),
    @FechaEstimadaCierre DATE = NULL,
    @UsuarioRegistro NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Postventa.OT_Comentario_Historial (
        OrdenTrabajoId, CategoriaId, Comentario, FechaEstimadaCierre, FechaRegistro, UsuarioRegistro
    )
    VALUES (
        @OrdenTrabajoId, @CategoriaId, @Comentario, @FechaEstimadaCierre, GETDATE(), @UsuarioRegistro
    );

    SELECT SCOPE_IDENTITY() AS NewId;
END
GO
