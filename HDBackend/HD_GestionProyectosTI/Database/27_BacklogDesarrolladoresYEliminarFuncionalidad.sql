-- ─────────────────────────────────────────────────────────────────────────
-- Backlog Mensual: dos funcionalidades nuevas.
--
-- 1) Asignar uno o varios desarrolladores (rol TI "DESARROLLADOR") a una
--    funcionalidad -- quien atendera la peticion. Catalogo nuevo
--    (TI_FuncionalidadDesarrolladores, tabla puente N:N) + rol nuevo en
--    TI_Roles. La asignacion de ESE rol a los usuarios (quienes son
--    desarrolladores) se hace igual que ya se hace hoy para JEFATURA_TI/
--    GERENCIA_PLANEACION -- insertando en TI_UsuarioRoles, no hay pantalla
--    de administracion de roles en este modulo todavia.
--
-- 2) Eliminar una funcionalidad por completo (no solo quitarla del backlog
--    del mes, que ya hace PriorizacionTI/RegresarABacklog poniendo
--    Mes/Anio en NULL) -- baja logica (Activa = 0), igual criterio que ya
--    usa todo el modulo para "ocultar sin borrar". Desaparece de inmediato
--    de todos los listados activos (bandeja, alcance, backlog, buscar).
-- ─────────────────────────────────────────────────────────────────────────

-- ----------------------------------------------------------------------------
-- 1) Rol "DESARROLLADOR" + tabla puente TI_FuncionalidadDesarrolladores
-- ----------------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.TI_Roles WHERE Codigo = 'DESARROLLADOR')
BEGIN
    INSERT INTO dbo.TI_Roles (Codigo, Nombre, Activo) VALUES ('DESARROLLADOR', 'Desarrollador', 1);
END
GO

IF OBJECT_ID('dbo.TI_FuncionalidadDesarrolladores', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.TI_FuncionalidadDesarrolladores (
        FuncionalidadId       BIGINT   NOT NULL,
        UsuarioId             INT      NOT NULL,
        AsignadoPorUsuarioId  INT      NOT NULL,
        FechaAsignacion       DATETIME2 NOT NULL CONSTRAINT DF_TI_FuncionalidadDesarrolladores_Fecha DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT PK_TI_FuncionalidadDesarrolladores PRIMARY KEY (FuncionalidadId, UsuarioId),
        CONSTRAINT FK_TI_FuncionalidadDesarrolladores_Funcionalidad FOREIGN KEY (FuncionalidadId) REFERENCES dbo.TI_Funcionalidades (FuncionalidadId)
    );
END
GO

-- ----------------------------------------------------------------------------
-- 2) Asignar desarrolladores (reemplaza el conjunto completo de la funcionalidad)
-- ----------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_TI_Funcionalidades_AsignarDesarrolladores
    @FuncionalidadId BIGINT,
    @UsuarioIds      NVARCHAR(MAX) = NULL,  -- ids separados por coma; NULL o vacio = quitar a todos
    @usuarioId       INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM dbo.TI_Funcionalidades WHERE FuncionalidadId = @FuncionalidadId AND Activa = 1)
        BEGIN
            RAISERROR('La funcionalidad indicada no existe o no esta activa', 16, 1);
            RETURN;
        END

        DELETE FROM dbo.TI_FuncionalidadDesarrolladores WHERE FuncionalidadId = @FuncionalidadId;

        IF (@UsuarioIds IS NOT NULL AND LTRIM(RTRIM(@UsuarioIds)) <> '')
        BEGIN
            INSERT INTO dbo.TI_FuncionalidadDesarrolladores (FuncionalidadId, UsuarioId, AsignadoPorUsuarioId)
            SELECT @FuncionalidadId, CAST(value AS INT), @usuarioId
            FROM STRING_SPLIT(@UsuarioIds, ',')
            WHERE LTRIM(RTRIM(value)) <> '';
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- ----------------------------------------------------------------------------
-- 3) Eliminar funcionalidad (baja logica)
-- ----------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_TI_Funcionalidades_Eliminar
    @FuncionalidadId BIGINT,
    @usuarioId       INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @SolicitudId BIGINT;
        SELECT @SolicitudId = v.SolicitudId
        FROM dbo.TI_Funcionalidades f
        INNER JOIN dbo.TI_VersionesAlcance v ON v.VersionAlcanceId = f.VersionAlcanceId
        WHERE f.FuncionalidadId = @FuncionalidadId AND f.Activa = 1;

        IF (@SolicitudId IS NULL)
        BEGIN
            RAISERROR('La funcionalidad indicada no existe o ya fue eliminada', 16, 1);
            RETURN;
        END

        UPDATE dbo.TI_Funcionalidades SET Activa = 0 WHERE FuncionalidadId = @FuncionalidadId;

        INSERT INTO dbo.TI_AuditoriaEventos (SolicitudId, UsuarioId, Entidad, EntidadId, Accion, FechaEvento, ValorAnterior, ValorNuevo, Origen)
        VALUES (@SolicitudId, @usuarioId, 'TI_Funcionalidades', CAST(@FuncionalidadId AS NVARCHAR(200)), 'FUNCIONALIDAD_ELIMINADA', SYSUTCDATETIME(), 'Activa=1', 'Activa=0', 'API');

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- ----------------------------------------------------------------------------
-- 4) Listado del backlog mensual: ahora tambien trae los desarrolladores
--    asignados a cada funcionalidad -- un solo campo "id:Nombre;id:Nombre"
--    (DesarrolladoresAsignados) para que el frontend pueda mostrar los
--    nombres Y pre-marcar el selector de checkboxes con los ids actuales
--    sin pedir nada aparte.
-- ----------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_TI_Backlog_Mensual_Listado
    @Mes  TINYINT,
    @Anio SMALLINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT f.FuncionalidadId, f.Codigo, f.Nombre, f.DiasEstimados, f.AvanceManual,
           f.MesProgramado, f.AnioProgramado,
           s.SolicitudId, s.Folio, s.Titulo, s.AreaSolicitante,
           dev.DesarrolladoresAsignados
    FROM dbo.TI_Funcionalidades f
    INNER JOIN dbo.TI_VersionesAlcance v ON v.VersionAlcanceId = f.VersionAlcanceId AND v.EstadoVersion = 'APROBADA'
    INNER JOIN dbo.TI_Solicitudes s ON s.SolicitudId = v.SolicitudId
    OUTER APPLY (
        SELECT STRING_AGG(CONCAT(fd.UsuarioId, ':', e.Nombre, ' ', e.ApellidoPaterno), ';')
                 WITHIN GROUP (ORDER BY e.Nombre) AS DesarrolladoresAsignados
        FROM dbo.TI_FuncionalidadDesarrolladores fd
        LEFT JOIN AppMH.dbo.Empleados e ON e.IDEmpleado = fd.UsuarioId AND e.Estatus = 1
        WHERE fd.FuncionalidadId = f.FuncionalidadId
    ) dev
    WHERE f.Activa = 1 AND f.MesProgramado = @Mes AND f.AnioProgramado = @Anio
    ORDER BY s.AreaSolicitante, f.Orden;
END
GO
