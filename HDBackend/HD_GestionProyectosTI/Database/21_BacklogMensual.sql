-- ============================================================================
-- HumayaDigital_GestionProyectosTI -- Backlog mensual (revision de desarrollo
-- del mes entre Jefatura de TI y Planeacion Estrategica). Script preparado
-- para REVISION Y EJECUCION MANUAL. No se ha ejecutado contra ningun
-- servidor.
--
-- Contenido:
--   1) ALTER TABLE TI_Funcionalidades ADD AvanceManual -- % de avance
--      (0-100) que Jefatura de TI actualiza a mano por funcionalidad.
--   2) sp_TI_Funcionalidades_ActualizarAvance -- escribe AvanceManual.
--   3) sp_TI_SolicitudesExtras_ObtenerOCrear -- devuelve (o crea la primera
--      vez) la solicitud administrativa "Solicitudes Extras {anio} - {area}"
--      que agrupa funcionalidades sin una solicitud real de origen. Se crea
--      con su version de alcance YA APROBADA automaticamente (excepcion
--      acordada: es un contenedor administrativo, no tiene un solicitante
--      real que pueda aprobar).
--   4) sp_TI_FuncionalidadesExtra_Agregar -- agrega una funcionalidad nueva
--      directo a esa solicitud, ya programada en el mes/anio indicado.
--   5) sp_TI_Backlog_Mensual_Listado -- funcionalidades programadas en un
--      mes/anio especifico (lo que se va a desarrollar ese mes).
--   6) sp_TI_Backlog_Buscar -- funcionalidades NO terminadas (AvanceManual
--      < 100 o NULL) que no estan en el mes/anio que se esta revisando, para
--      el modal de "Buscar" (agregar al backlog del mes algo que ya existia
--      en otro periodo, o que seguia en backlog sin programar).
--
-- SUPUESTO: la solicitud extra usa el primer TipoSolicitudId activo que
-- encuentre (ORDER BY Orden) -- es un contenedor administrativo, el tipo de
-- solicitud no tiene ningun efecto en su comportamiento. SolicitanteUsuarioId
-- y CreadaPorUsuarioId de esa solicitud quedan en quien la crea (Jefatura de
-- TI), porque no existe un solicitante real.
-- ============================================================================

-- ----------------------------------------------------------------------------
-- 1) Avance manual por funcionalidad
-- ----------------------------------------------------------------------------
-- EXEC(...) con SQL dinamico a proposito: si el ALTER TABLE y el CREATE
-- CONSTRAINT se compilan tal cual en este mismo batch, SQL Server resuelve
-- los nombres de columna contra el esquema ANTES de ejecutar nada ("Invalid
-- column name 'AvanceManual'"), porque la tabla ya existe (no aplica la
-- resolucion diferida que si tienen las stored procedures). Envolverlos en
-- SQL dinamico retrasa esa resolucion hasta el momento de ejecutar el EXEC,
-- cuando la columna ya fue agregada.
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.TI_Funcionalidades') AND name = 'AvanceManual')
BEGIN
    EXEC('ALTER TABLE dbo.TI_Funcionalidades ADD AvanceManual TINYINT NULL');
    EXEC('ALTER TABLE dbo.TI_Funcionalidades ADD CONSTRAINT CK_TI_Funcionalidades_AvanceManual CHECK (AvanceManual IS NULL OR AvanceManual BETWEEN 0 AND 100)');
END
GO

-- Accion directa de Jefatura de TI (igual que ProgramarInicial/RegresarABacklog
-- de 09_ProgramacionMensual.sql): sin aprobacion, el controller la gatea por rol.
CREATE OR ALTER PROCEDURE dbo.sp_TI_Funcionalidades_ActualizarAvance
    @FuncionalidadId BIGINT,
    @AvanceManual TINYINT = NULL,
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF (@AvanceManual IS NOT NULL AND @AvanceManual NOT BETWEEN 0 AND 100)
        BEGIN
            RAISERROR('El avance debe estar entre 0 y 100', 16, 1);
            RETURN;
        END

        DECLARE @SolicitudId BIGINT, @AvanceAnterior TINYINT;
        SELECT @SolicitudId = v.SolicitudId, @AvanceAnterior = f.AvanceManual
        FROM dbo.TI_Funcionalidades f
        INNER JOIN dbo.TI_VersionesAlcance v ON v.VersionAlcanceId = f.VersionAlcanceId
        WHERE f.FuncionalidadId = @FuncionalidadId AND f.Activa = 1;

        IF (@SolicitudId IS NULL)
        BEGIN
            RAISERROR('La funcionalidad indicada no existe o no esta activa', 16, 1);
            RETURN;
        END

        UPDATE dbo.TI_Funcionalidades SET AvanceManual = @AvanceManual WHERE FuncionalidadId = @FuncionalidadId;

        INSERT INTO dbo.TI_AuditoriaEventos (SolicitudId, UsuarioId, Entidad, EntidadId, Accion, FechaEvento, ValorAnterior, ValorNuevo, Origen)
        VALUES (@SolicitudId, @usuarioId, 'TI_Funcionalidades', CAST(@FuncionalidadId AS NVARCHAR(200)), 'ACTUALIZAR_AVANCE', SYSUTCDATETIME(),
                CAST(@AvanceAnterior AS NVARCHAR(10)), CAST(@AvanceManual AS NVARCHAR(10)), 'API');

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- ----------------------------------------------------------------------------
-- 2) Solicitud administrativa "Solicitudes Extras {anio} - {area}"
-- ----------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_TI_SolicitudesExtras_ObtenerOCrear
    @AreaSolicitante NVARCHAR(300),
    @Anio SMALLINT,
    @usuarioId INT,
    @SolicitudId BIGINT OUTPUT,
    @VersionAlcanceId BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Titulo NVARCHAR(400) = CONCAT('Solicitudes Extras ', @Anio, ' - ', @AreaSolicitante);

    SELECT @SolicitudId = s.SolicitudId
    FROM dbo.TI_Solicitudes s
    WHERE s.AreaSolicitante = @AreaSolicitante AND s.Titulo = @Titulo;

    IF (@SolicitudId IS NOT NULL)
    BEGIN
        SELECT @VersionAlcanceId = v.VersionAlcanceId FROM dbo.TI_VersionesAlcance v WHERE v.SolicitudId = @SolicitudId AND v.EstadoVersion = 'APROBADA';
        RETURN;
    END

    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @LockResult INT;
        EXEC @LockResult = sp_getapplock @Resource = @Titulo, @LockMode = 'Exclusive', @LockOwner = 'Transaction', @LockTimeout = 5000;
        IF (@LockResult < 0)
        BEGIN
            RAISERROR('No se pudo asegurar la operacion de forma exclusiva, intente de nuevo', 16, 1);
            RETURN;
        END

        -- Otra sesion pudo haberla creado mientras se esperaba el lock.
        SELECT @SolicitudId = s.SolicitudId FROM dbo.TI_Solicitudes s WHERE s.AreaSolicitante = @AreaSolicitante AND s.Titulo = @Titulo;
        IF (@SolicitudId IS NOT NULL)
        BEGIN
            SELECT @VersionAlcanceId = v.VersionAlcanceId FROM dbo.TI_VersionesAlcance v WHERE v.SolicitudId = @SolicitudId AND v.EstadoVersion = 'APROBADA';
            COMMIT TRANSACTION;
            RETURN;
        END

        DECLARE @TipoSolicitudId INT = (SELECT TOP 1 TipoSolicitudId FROM dbo.TI_TiposSolicitud WHERE Activo = 1 ORDER BY Orden);
        DECLARE @EstadoId INT = (SELECT EstadoSolicitudId FROM dbo.TI_EstadosSolicitud WHERE Codigo = 'EN_BACKLOG');

        INSERT INTO dbo.TI_Solicitudes (
            Titulo, DescripcionNecesidad, TipoSolicitudId, SolicitanteUsuarioId, CreadaPorUsuarioId, AreaSolicitante,
            EstadoSolicitudId, FechaCreacion, FechaEnvio, FechaUltimaActualizacion, EsConfidencial
        )
        VALUES (
            @Titulo,
            'Solicitud administrativa automatica: agrupa funcionalidades sin una solicitud real de origen (o de otros proyectos), para mantenerlas monitoreadas dentro del backlog mensual.',
            @TipoSolicitudId, @usuarioId, @usuarioId, @AreaSolicitante,
            @EstadoId, SYSUTCDATETIME(), SYSUTCDATETIME(), SYSUTCDATETIME(), 0
        );

        SET @SolicitudId = SCOPE_IDENTITY();

        INSERT INTO dbo.TI_VersionesAlcance (SolicitudId, NumeroVersion, VersionAnteriorId, Objetivo, AlcanceIncluido, EstadoVersion, CreadaPorUsuarioId, FechaCreacion, FechaResolucion, ObservacionesResolucion)
        VALUES (
            @SolicitudId, 1, NULL,
            'Contenedor administrativo de funcionalidades sin solicitud de origen.',
            'Funcionalidades agregadas directamente desde el backlog mensual, sin pasar por el flujo normal de definicion de alcance.',
            'APROBADA', @usuarioId, SYSUTCDATETIME(), SYSUTCDATETIME(), 'Aprobacion automatica: solicitud administrativa sin solicitante real'
        );

        SET @VersionAlcanceId = SCOPE_IDENTITY();

        INSERT INTO dbo.TI_AprobacionesAlcance (VersionAlcanceId, TipoAprobador, EstadoAprobacion, FechaSolicitud, FechaDecision, Comentario, AprobadorUsuarioId)
        VALUES
            (@VersionAlcanceId, 'SOLICITANTE', 'APROBADA', SYSUTCDATETIME(), SYSUTCDATETIME(), 'Aprobacion automatica: solicitud administrativa sin solicitante real', @usuarioId),
            (@VersionAlcanceId, 'JEFATURA_TI', 'APROBADA', SYSUTCDATETIME(), SYSUTCDATETIME(), 'Aprobacion automatica: solicitud administrativa sin solicitante real', @usuarioId);

        INSERT INTO dbo.TI_AuditoriaEventos (SolicitudId, UsuarioId, Entidad, EntidadId, Accion, FechaEvento, ValorAnterior, ValorNuevo, Origen)
        VALUES (@SolicitudId, @usuarioId, 'TI_Solicitudes', CAST(@SolicitudId AS NVARCHAR(200)), 'CREAR_SOLICITUD_EXTRA_AUTOMATICA', SYSUTCDATETIME(), NULL, @Titulo, 'API');

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- ----------------------------------------------------------------------------
-- 3) Agregar funcionalidad extra (sin solicitud real), ya programada
-- ----------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_TI_FuncionalidadesExtra_Agregar
    @AreaSolicitante NVARCHAR(300),
    @Anio SMALLINT,
    @Nombre NVARCHAR(200),
    @Descripcion NVARCHAR(MAX),
    @DiasEstimados DECIMAL(6, 2) = NULL,
    @MesProgramado TINYINT,
    @AnioProgramado SMALLINT,
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF (@AreaSolicitante IS NULL OR LTRIM(RTRIM(@AreaSolicitante)) = '')
        BEGIN
            RAISERROR('El area solicitante es un valor requerido', 16, 1);
            RETURN;
        END
        IF (@Nombre IS NULL OR LTRIM(RTRIM(@Nombre)) = '')
        BEGIN
            RAISERROR('El nombre de la funcionalidad es un valor requerido', 16, 1);
            RETURN;
        END
        IF (@MesProgramado IS NULL OR @MesProgramado NOT BETWEEN 1 AND 12)
        BEGIN
            RAISERROR('El mes programado debe estar entre 1 y 12', 16, 1);
            RETURN;
        END
        IF (@AnioProgramado IS NULL)
        BEGIN
            RAISERROR('El anio programado es obligatorio', 16, 1);
            RETURN;
        END
        IF (@DiasEstimados IS NOT NULL AND @DiasEstimados < 0)
        BEGIN
            RAISERROR('Los dias estimados no pueden ser negativos', 16, 1);
            RETURN;
        END

        DECLARE @SolicitudId BIGINT, @VersionAlcanceId BIGINT;
        EXEC dbo.sp_TI_SolicitudesExtras_ObtenerOCrear
            @AreaSolicitante = @AreaSolicitante, @Anio = @Anio, @usuarioId = @usuarioId,
            @SolicitudId = @SolicitudId OUTPUT, @VersionAlcanceId = @VersionAlcanceId OUTPUT;

        IF (@VersionAlcanceId IS NULL)
        BEGIN
            RAISERROR('No fue posible obtener la solicitud de funcionalidades extra', 16, 1);
            RETURN;
        END

        DECLARE @Orden INT = (SELECT ISNULL(MAX(Orden), 0) + 1 FROM dbo.TI_Funcionalidades WHERE VersionAlcanceId = @VersionAlcanceId);

        -- CriteriosAceptacion es NOT NULL en TI_Funcionalidades (confirmado
        -- en desarrollo: el INSERT normal de sp_TI_Funcionalidades_Agregar
        -- siempre lo recibe del formulario). Las funcionalidades extra no
        -- pasan por ese formulario completo a proposito (se acordo
        -- mantenerlo simple: solo area/nombre/descripcion/dias), asi que se
        -- rellena con un texto fijo en vez de agregar un campo mas a la UI.
        INSERT INTO dbo.TI_Funcionalidades (VersionAlcanceId, Codigo, Nombre, Descripcion, CriteriosAceptacion, Orden, DiasEstimados, Activa, CreadaPorUsuarioId, FechaCreacion, MesProgramado, AnioProgramado)
        VALUES (@VersionAlcanceId, NULL, @Nombre, @Descripcion, 'N/A - Funcionalidad extra agregada directo desde el Backlog Mensual, sin una solicitud de origen.', @Orden, @DiasEstimados, 1, @usuarioId, SYSUTCDATETIME(), @MesProgramado, @AnioProgramado);

        DECLARE @FuncionalidadId BIGINT = SCOPE_IDENTITY();

        INSERT INTO dbo.TI_AuditoriaEventos (SolicitudId, UsuarioId, Entidad, EntidadId, Accion, FechaEvento, ValorAnterior, ValorNuevo, Origen)
        VALUES (@SolicitudId, @usuarioId, 'TI_Funcionalidades', CAST(@FuncionalidadId AS NVARCHAR(200)), 'FUNCIONALIDAD_EXTRA_AGREGADA', SYSUTCDATETIME(), NULL,
                CONCAT('Mes=', @MesProgramado, ' Anio=', @AnioProgramado), 'API');

        COMMIT TRANSACTION;
        SELECT @FuncionalidadId AS FuncionalidadId, @SolicitudId AS SolicitudId;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- ----------------------------------------------------------------------------
-- 4) Listado del backlog de un mes/anio especifico
-- ----------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_TI_Backlog_Mensual_Listado
    @Mes  TINYINT,
    @Anio SMALLINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT f.FuncionalidadId, f.Codigo, f.Nombre, f.DiasEstimados, f.AvanceManual,
           f.MesProgramado, f.AnioProgramado,
           s.SolicitudId, s.Folio, s.Titulo, s.AreaSolicitante
    FROM dbo.TI_Funcionalidades f
    INNER JOIN dbo.TI_VersionesAlcance v ON v.VersionAlcanceId = f.VersionAlcanceId AND v.EstadoVersion = 'APROBADA'
    INNER JOIN dbo.TI_Solicitudes s ON s.SolicitudId = v.SolicitudId
    WHERE f.Activa = 1 AND f.MesProgramado = @Mes AND f.AnioProgramado = @Anio
    ORDER BY s.AreaSolicitante, f.Orden;
END
GO

-- ----------------------------------------------------------------------------
-- 5) Buscar funcionalidades no terminadas fuera del mes/anio seleccionado
-- ----------------------------------------------------------------------------
-- CORRECCION: ya no basta con AvanceManual (es un % capturado a mano por
-- Jefatura de TI, no refleja si el SOLICITANTE ya aprobo la revision de esa
-- funcionalidad). Se agrega RevisionEstado con el mismo criterio que ya usa
-- sp_TI_Funcionalidades_ProgramacionVigente_ListadoPorSolicitud (ver
-- 13_RevisionPorFuncionalidad.sql), mirando el ULTIMO TI_CambiosProgramacion
-- de cada funcionalidad SIN importar la solicitud (aqui se busca entre
-- todas): se excluyen las que ya quedaron con RevisionEstado = 'APROBADA'
-- (el solicitante ya las aprobo), y se regresa el estado para que el
-- frontend pueda ofrecer "Enviar a Revision" solo cuando tenga sentido
-- (programada, y no ya pendiente de revision).
CREATE OR ALTER PROCEDURE dbo.sp_TI_Backlog_Buscar
    @Mes       TINYINT,
    @Anio      SMALLINT,
    @Busqueda  NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH UltimoCambio AS (
        SELECT c.*, ROW_NUMBER() OVER (PARTITION BY c.FuncionalidadId ORDER BY c.FechaSolicitud DESC) AS Orden_
        FROM dbo.TI_CambiosProgramacion c
    )
    SELECT f.FuncionalidadId, f.Codigo, f.Nombre, f.DiasEstimados, f.AvanceManual,
           f.MesProgramado, f.AnioProgramado,
           s.SolicitudId, s.Folio, s.Titulo, s.AreaSolicitante,
           uc.CambioProgramacionId AS RevisionCambioProgramacionId,
           CASE
               WHEN uc.EstadoCambio = 'PENDIENTE' THEN 'EN_REVISION'
               WHEN uc.EstadoCambio = 'APROBADO' AND uc.MesPropuesto = f.MesProgramado AND uc.AnioPropuesto = f.AnioProgramado THEN 'APROBADA'
               WHEN uc.EstadoCambio = 'RECHAZADO' AND uc.MesAnteriorProgramado = f.MesProgramado AND uc.AnioAnteriorProgramado = f.AnioProgramado THEN 'RECHAZADA'
               ELSE NULL
           END AS RevisionEstado
    FROM dbo.TI_Funcionalidades f
    INNER JOIN dbo.TI_VersionesAlcance v ON v.VersionAlcanceId = f.VersionAlcanceId AND v.EstadoVersion = 'APROBADA'
    INNER JOIN dbo.TI_Solicitudes s ON s.SolicitudId = v.SolicitudId
    LEFT JOIN UltimoCambio uc ON uc.FuncionalidadId = f.FuncionalidadId AND uc.Orden_ = 1
    WHERE f.Activa = 1
      AND (f.AvanceManual IS NULL OR f.AvanceManual < 100)
      AND NOT (f.MesProgramado = @Mes AND f.AnioProgramado = @Anio)
      AND NOT (uc.EstadoCambio = 'APROBADO' AND uc.MesPropuesto = f.MesProgramado AND uc.AnioPropuesto = f.AnioProgramado)
      AND (
            @Busqueda IS NULL OR LTRIM(RTRIM(@Busqueda)) = ''
            OR f.Nombre LIKE '%' + @Busqueda + '%'
            OR f.Codigo LIKE '%' + @Busqueda + '%'
            OR s.AreaSolicitante LIKE '%' + @Busqueda + '%'
          )
    ORDER BY s.AreaSolicitante, f.Nombre;
END
GO
