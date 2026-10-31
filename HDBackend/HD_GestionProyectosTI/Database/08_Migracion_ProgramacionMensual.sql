-- ============================================================================
-- HumayaDigital_GestionProyectosTI -- Migracion: programacion mensual por
-- funcionalidad (Bloque 2).
-- Script preparado para REVISION Y EJECUCION MANUAL. No se ha ejecutado
-- contra ningun servidor.
--
-- Cambios de persistencia autorizados explicitamente para este bloque:
--   1) dos columnas nuevas, NULLABLE, en dbo.TI_Funcionalidades
--      (MesProgramado, AnioProgramado).
--   2) una tabla nueva, dbo.TI_CambiosProgramacion, para el historial de
--      solicitudes de cambio de mes de una funcionalidad ya priorizada.
-- No se crean columnas ni tablas adicionales a las aqui listadas. No se
-- borra ningun dato existente.
--
-- Idempotente: cada ALTER/CREATE esta protegido con su propia verificacion
-- (sys.columns / sys.tables / sys.indexes) para poder ejecutarse varias
-- veces sin error ni efecto duplicado.
-- ============================================================================

-- ----------------------------------------------------------------------------
-- 1) dbo.TI_Funcionalidades.MesProgramado / AnioProgramado
-- ----------------------------------------------------------------------------

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.TI_Funcionalidades') AND name = 'MesProgramado'
)
BEGIN
    ALTER TABLE dbo.TI_Funcionalidades ADD MesProgramado TINYINT NULL;
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.TI_Funcionalidades') AND name = 'AnioProgramado'
)
BEGIN
    ALTER TABLE dbo.TI_Funcionalidades ADD AnioProgramado SMALLINT NULL;
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.check_constraints WHERE name = 'CK_TI_Funcionalidades_MesProgramado'
)
BEGIN
    ALTER TABLE dbo.TI_Funcionalidades
        ADD CONSTRAINT CK_TI_Funcionalidades_MesProgramado
        CHECK (MesProgramado IS NULL OR MesProgramado BETWEEN 1 AND 12);
END
GO

-- ----------------------------------------------------------------------------
-- 2) dbo.TI_CambiosProgramacion -- historial de cambios de mes/anio de una
--    funcionalidad ya priorizada (vigencia vs. historico, mismo espiritu
--    que TI_VersionesAlcance: la fila PENDIENTE no reemplaza la
--    programacion vigente hasta que se aprueba).
-- ----------------------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TI_CambiosProgramacion' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE dbo.TI_CambiosProgramacion (
        CambioProgramacionId      BIGINT IDENTITY(1,1) NOT NULL,
        FuncionalidadId           BIGINT NOT NULL,
        SolicitudId               BIGINT NOT NULL,
        MesAnteriorProgramado     TINYINT NULL,
        AnioAnteriorProgramado    SMALLINT NULL,
        MesPropuesto              TINYINT NOT NULL,
        AnioPropuesto             SMALLINT NOT NULL,
        SolicitadoPorUsuarioId    INT NOT NULL,
        FechaSolicitud            DATETIME2 NOT NULL CONSTRAINT DF_TI_CambiosProgramacion_FechaSolicitud DEFAULT (SYSUTCDATETIME()),
        Motivo                    NVARCHAR(2000) NOT NULL,
        EstadoCambio              NVARCHAR(20) NOT NULL CONSTRAINT DF_TI_CambiosProgramacion_EstadoCambio DEFAULT ('PENDIENTE'),
        DecididoPorUsuarioId      INT NULL,
        FechaDecision             DATETIME2 NULL,
        ComentarioResolucion      NVARCHAR(2000) NULL,

        CONSTRAINT PK_TI_CambiosProgramacion PRIMARY KEY CLUSTERED (CambioProgramacionId),

        CONSTRAINT FK_TI_CambiosProgramacion_Funcionalidad
            FOREIGN KEY (FuncionalidadId) REFERENCES dbo.TI_Funcionalidades (FuncionalidadId),
        CONSTRAINT FK_TI_CambiosProgramacion_Solicitud
            FOREIGN KEY (SolicitudId) REFERENCES dbo.TI_Solicitudes (SolicitudId),

        CONSTRAINT CK_TI_CambiosProgramacion_MesAnterior
            CHECK (MesAnteriorProgramado IS NULL OR MesAnteriorProgramado BETWEEN 1 AND 12),
        CONSTRAINT CK_TI_CambiosProgramacion_MesPropuesto
            CHECK (MesPropuesto BETWEEN 1 AND 12),
        CONSTRAINT CK_TI_CambiosProgramacion_EstadoCambio
            CHECK (EstadoCambio IN ('PENDIENTE', 'APROBADO', 'RECHAZADO'))
    );
END
GO

-- Indices de consulta (historial por funcionalidad / por solicitud).
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_TI_CambiosProgramacion_FuncionalidadId' AND object_id = OBJECT_ID('dbo.TI_CambiosProgramacion'))
BEGIN
    CREATE INDEX IX_TI_CambiosProgramacion_FuncionalidadId ON dbo.TI_CambiosProgramacion (FuncionalidadId, FechaSolicitud DESC);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_TI_CambiosProgramacion_SolicitudId' AND object_id = OBJECT_ID('dbo.TI_CambiosProgramacion'))
BEGIN
    CREATE INDEX IX_TI_CambiosProgramacion_SolicitudId ON dbo.TI_CambiosProgramacion (SolicitudId, FechaSolicitud DESC);
END
GO

-- Regla "no mas de un cambio pendiente por funcionalidad" reforzada tambien
-- a nivel de base de datos (ademas de la validacion en el SP): indice unico
-- filtrado, solo aplica sobre las filas EstadoCambio = 'PENDIENTE'.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_TI_CambiosProgramacion_PendientePorFuncionalidad' AND object_id = OBJECT_ID('dbo.TI_CambiosProgramacion'))
BEGIN
    CREATE UNIQUE INDEX UX_TI_CambiosProgramacion_PendientePorFuncionalidad
        ON dbo.TI_CambiosProgramacion (FuncionalidadId)
        WHERE EstadoCambio = 'PENDIENTE';
END
GO
