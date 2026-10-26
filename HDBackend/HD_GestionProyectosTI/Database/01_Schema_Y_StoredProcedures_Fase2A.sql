-- ============================================================================
-- HumayaDigital_GestionProyectosTI -- Fase 2A (API nueva)
-- Script preparado para REVISION Y EJECUCION MANUAL. No se ejecuto contra el
-- servidor durante el desarrollo.
--
-- No crea tablas: las 29 tablas TI_* y las 2 vistas vw_TI_* ya existen y
-- estan validadas (ver Fase 1 / Fase 1.1), todas en el esquema dbo. Este
-- script crea los 8 procedimientos necesarios para esta fase, tambien en
-- dbo (mismo esquema de las tablas que consultan), con el prefijo sp_TI_
-- para distinguirlos del resto de los procedimientos de la base.
--
-- Alcance de Fase 2A: roles/permisos, catalogo de tipos de solicitud,
-- creacion de solicitudes, listado/detalle con restriccion por rol,
-- historial de estados, validacion de transiciones y auditoria.
-- NO incluye estimacion, priorizacion ni asignacion de desarrollo -- por eso
-- el catalogo de transiciones de dbo.sp_TI_Solicitudes_CambiarEstado solo cubre los
-- estados de registro/revision (ENVIADA, EN_REVISION, ESPERANDO_INFORMACION)
-- mas los cierres RECHAZADA/CANCELADA. Avanzar a DEFINICION_ALCANCE en
-- adelante queda para la fase que implemente esa parte del proceso.
--
-- SUPUESTO A CONFIRMAR: dbo.sp_TI_Solicitudes_Crear inserta la solicitud
-- directamente en estado ENVIADA (no en BORRADOR), porque el alcance de esta
-- fase no describe un paso separado de "guardar borrador" / "enviar". El
-- catalogo TI_EstadosSolicitud si contempla BORRADOR como primer estado
-- (Orden=10) y Solicitudes.FechaEnvio es nullable precisamente para permitir
-- ese flujo en dos pasos -- si la especificacion aprobada lo requiere, este
-- SP debe cambiar para insertar en BORRADOR y agregar una transicion
-- BORRADOR->ENVIADA disponible para el propio solicitante (hoy
-- SolicitudesTIController.PuedeCambiarEstadoAsync solo autoriza
-- JEFATURA_TI/ADMIN_MODULO, no al solicitante).
-- ============================================================================

-- ----------------------------------------------------------------------------
-- ROLES
-- ----------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_TI_Roles_Mio
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT r.RolId, r.Codigo, r.Nombre
    FROM dbo.TI_UsuarioRoles ur
    INNER JOIN dbo.TI_Roles r ON r.RolId = ur.RolId
    WHERE ur.UsuarioId = @usuarioId
      AND ur.Activo = 1
      AND r.Activo = 1
    ORDER BY r.Nombre;
END
GO

-- ----------------------------------------------------------------------------
-- CATALOGOS
-- ----------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_TI_Catalogos_TiposSolicitud_Listado
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TipoSolicitudId, Codigo, Nombre, Descripcion
    FROM dbo.TI_TiposSolicitud
    WHERE Activo = 1
    ORDER BY Orden;
END
GO

-- ----------------------------------------------------------------------------
-- SOLICITUDES
-- ----------------------------------------------------------------------------

-- Folio: dbo.TI_Solicitudes.Folio es una columna CALCULADA Y PERSISTIDA por
-- la propia tabla (formato AAAA-TI-NNNNNNN a partir de FechaCreacion y
-- SolicitudId) -- SQL Server la genera solo en cuanto se inserta la fila.
-- No se escribe aqui; se lee de vuelta despues del INSERT.
CREATE OR ALTER PROCEDURE dbo.sp_TI_Solicitudes_Crear
    @Titulo                   NVARCHAR(400),
    @DescripcionNecesidad     NVARCHAR(MAX),
    @ProblemaActual           NVARCHAR(MAX) = NULL,
    @BeneficioEsperado        NVARCHAR(MAX) = NULL,
    @TipoSolicitudId          INT,
    @AreaSolicitante          NVARCHAR(300),
    @EsConfidencial           BIT = 0,
    @SolicitanteUsuarioId     INT,
    @CreadaPorUsuarioId       INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.TI_TiposSolicitud WHERE TipoSolicitudId = @TipoSolicitudId AND Activo = 1)
    BEGIN
        RAISERROR('El tipo de solicitud indicado no existe o esta inactivo', 16, 1);
        RETURN;
    END

    -- Area solicitante obligatoria (ver 19_EditarSolicitud_AreaObligatoria.sql):
    -- el front ahora manda un valor de un catalogo fijo via dropdown.
    IF @AreaSolicitante IS NULL OR LTRIM(RTRIM(@AreaSolicitante)) = ''
    BEGIN
        RAISERROR('El area solicitante es un valor requerido', 16, 1);
        RETURN;
    END

    DECLARE @EstadoEnviada INT = (SELECT EstadoSolicitudId FROM dbo.TI_EstadosSolicitud WHERE Codigo = 'ENVIADA');

    INSERT INTO dbo.TI_Solicitudes (
        Titulo, DescripcionNecesidad, ProblemaActual, BeneficioEsperado,
        TipoSolicitudId, SolicitanteUsuarioId, CreadaPorUsuarioId, AreaSolicitante,
        EstadoSolicitudId, FechaCreacion, FechaEnvio, FechaUltimaActualizacion, EsConfidencial
    )
    VALUES (
        @Titulo, @DescripcionNecesidad, @ProblemaActual, @BeneficioEsperado,
        @TipoSolicitudId, @SolicitanteUsuarioId, @CreadaPorUsuarioId, @AreaSolicitante,
        @EstadoEnviada, SYSUTCDATETIME(), SYSUTCDATETIME(), SYSUTCDATETIME(), @EsConfidencial
    );

    DECLARE @SolicitudId BIGINT = SCOPE_IDENTITY();

    INSERT INTO dbo.TI_SolicitudEstadosHistorial (SolicitudId, EstadoAnteriorId, EstadoNuevoId, UsuarioCambioId, FechaCambio, Origen)
    VALUES (@SolicitudId, NULL, @EstadoEnviada, @CreadaPorUsuarioId, SYSUTCDATETIME(), 'API');

    INSERT INTO dbo.TI_AuditoriaEventos (SolicitudId, UsuarioId, Entidad, EntidadId, Accion, FechaEvento, ValorAnterior, ValorNuevo, Origen)
    VALUES (@SolicitudId, @CreadaPorUsuarioId, 'TI_Solicitudes', CAST(@SolicitudId AS NVARCHAR(200)), 'CREAR_SOLICITUD', SYSUTCDATETIME(), NULL, 'ENVIADA', 'API');

    SELECT SolicitudId, Folio FROM dbo.TI_Solicitudes WHERE SolicitudId = @SolicitudId;
END
GO

-- Edita los datos base de una solicitud ya registrada (ver
-- 19_EditarSolicitud_AreaObligatoria.sql). Bloqueada en cuanto la solicitud
-- tenga una version de alcance APROBADA -- a partir de ahi el detalle se
-- consulta de manera informativa en el frontend, no se vuelve a abrir el
-- formulario de edicion.
CREATE OR ALTER PROCEDURE dbo.sp_TI_Solicitudes_Editar
    @SolicitudId              BIGINT,
    @Titulo                   NVARCHAR(400),
    @DescripcionNecesidad     NVARCHAR(MAX),
    @ProblemaActual           NVARCHAR(MAX) = NULL,
    @BeneficioEsperado        NVARCHAR(MAX) = NULL,
    @TipoSolicitudId          INT,
    @AreaSolicitante          NVARCHAR(300),
    @UsuarioId                INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.TI_Solicitudes WHERE SolicitudId = @SolicitudId)
    BEGIN
        RAISERROR('La solicitud indicada no existe', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.TI_TiposSolicitud WHERE TipoSolicitudId = @TipoSolicitudId AND Activo = 1)
    BEGIN
        RAISERROR('El tipo de solicitud indicado no existe o esta inactivo', 16, 1);
        RETURN;
    END

    IF @AreaSolicitante IS NULL OR LTRIM(RTRIM(@AreaSolicitante)) = ''
    BEGIN
        RAISERROR('El area solicitante es un valor requerido', 16, 1);
        RETURN;
    END

    IF EXISTS (
        SELECT 1 FROM dbo.TI_VersionesAlcance
        WHERE SolicitudId = @SolicitudId AND EstadoVersion = 'APROBADA'
    )
    BEGIN
        RAISERROR('No se puede editar la solicitud: el alcance ya fue aprobado', 16, 1);
        RETURN;
    END

    UPDATE dbo.TI_Solicitudes
    SET Titulo = @Titulo,
        DescripcionNecesidad = @DescripcionNecesidad,
        ProblemaActual = @ProblemaActual,
        BeneficioEsperado = @BeneficioEsperado,
        TipoSolicitudId = @TipoSolicitudId,
        AreaSolicitante = @AreaSolicitante,
        FechaUltimaActualizacion = SYSUTCDATETIME()
    WHERE SolicitudId = @SolicitudId;

    INSERT INTO dbo.TI_AuditoriaEventos (SolicitudId, UsuarioId, Entidad, EntidadId, Accion, FechaEvento, ValorAnterior, ValorNuevo, Origen)
    VALUES (@SolicitudId, @UsuarioId, 'TI_Solicitudes', CAST(@SolicitudId AS NVARCHAR(200)), 'EDITAR_SOLICITUD', SYSUTCDATETIME(), NULL, NULL, 'API');
END
GO

-- @verTodo lo resuelve el API a partir de los roles del usuario
-- (JEFATURA_TI / GERENCIA_PLANEACION / ADMIN_MODULO ven todo). El resto solo
-- ve lo que solicito, lo que creo, o donde figura como participante activo
-- en TI_SolicitudParticipantes.
CREATE OR ALTER PROCEDURE dbo.sp_TI_Solicitudes_Listado
    @usuarioId      INT,
    @verTodo        BIT,
    @estadoCodigo   NVARCHAR(100) = NULL,
    @tipoCodigo     NVARCHAR(80)  = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Nombre real del solicitante/creador (ver 17_NombresRealesUsuarios.sql):
    -- UsuarioId = AppMH.dbo.Empleados.IDEmpleado, mismo servidor SQL.
    SELECT
        s.SolicitudId, s.Folio, s.Titulo, s.DescripcionNecesidad, s.ProblemaActual, s.BeneficioEsperado,
        s.TipoSolicitudId, ts.Codigo AS TipoSolicitudCodigo, ts.Nombre AS TipoSolicitud,
        s.EstadoSolicitudId, es.Codigo AS EstadoCodigo, es.Nombre AS EstadoSolicitud, es.Etapa,
        s.PrioridadId, p.Codigo AS PrioridadCodigo, p.Nombre AS Prioridad,
        s.SolicitanteUsuarioId, s.CreadaPorUsuarioId, s.AreaSolicitante, s.EsConfidencial,
        s.MotivoRechazo, s.MotivoCancelacion,
        s.FechaCreacion, s.FechaEnvio, s.FechaUltimaActualizacion, s.FechaCierre,
        CONCAT(empSol.Nombre, ' ', empSol.ApellidoPaterno, ' ', empSol.ApellidoMaterno) AS SolicitanteNombre,
        CONCAT(empCrea.Nombre, ' ', empCrea.ApellidoPaterno, ' ', empCrea.ApellidoMaterno) AS CreadaPorNombre,
        empSol.Correo AS SolicitanteCorreo,
        agg.TotalDiasEstimados, agg.FuncionalidadesTotal, aprob.FuncionalidadesAprobadas
    FROM dbo.TI_Solicitudes s
    INNER JOIN dbo.TI_TiposSolicitud ts ON ts.TipoSolicitudId = s.TipoSolicitudId
    INNER JOIN dbo.TI_EstadosSolicitud es ON es.EstadoSolicitudId = s.EstadoSolicitudId
    LEFT JOIN dbo.TI_Prioridades p ON p.PrioridadId = s.PrioridadId
    LEFT JOIN AppMH.dbo.Empleados empSol ON empSol.IDEmpleado = s.SolicitanteUsuarioId AND empSol.Estatus = 1
    LEFT JOIN AppMH.dbo.Empleados empCrea ON empCrea.IDEmpleado = s.CreadaPorUsuarioId AND empCrea.Estatus = 1
    -- Avance por funcionalidades de la bandeja (ver 20_ListadoBandejaConAvance.sql):
    -- version de alcance APROBADA vigente de la solicitud, mismo criterio de
    -- "aprobada" que sp_TI_Funcionalidades_ProgramacionVigente_ListadoPorSolicitud.
    OUTER APPLY (
        SELECT TOP 1 va.VersionAlcanceId
        FROM dbo.TI_VersionesAlcance va
        WHERE va.SolicitudId = s.SolicitudId AND va.EstadoVersion = 'APROBADA'
        ORDER BY va.NumeroVersion DESC
    ) vig
    OUTER APPLY (
        SELECT SUM(f.DiasEstimados) AS TotalDiasEstimados, COUNT(*) AS FuncionalidadesTotal
        FROM dbo.TI_Funcionalidades f
        WHERE f.VersionAlcanceId = vig.VersionAlcanceId AND f.Activa = 1
    ) agg
    OUTER APPLY (
        SELECT COUNT(*) AS FuncionalidadesAprobadas
        FROM dbo.TI_Funcionalidades f
        LEFT JOIN (
            SELECT c.*, ROW_NUMBER() OVER (PARTITION BY c.FuncionalidadId ORDER BY c.FechaSolicitud DESC) AS Orden_
            FROM dbo.TI_CambiosProgramacion c
            WHERE c.SolicitudId = s.SolicitudId
        ) uc ON uc.FuncionalidadId = f.FuncionalidadId AND uc.Orden_ = 1
        WHERE f.VersionAlcanceId = vig.VersionAlcanceId AND f.Activa = 1
          AND uc.EstadoCambio = 'APROBADO' AND uc.MesPropuesto = f.MesProgramado AND uc.AnioPropuesto = f.AnioProgramado
    ) aprob
    WHERE (@estadoCodigo IS NULL OR es.Codigo = @estadoCodigo)
      AND (@tipoCodigo IS NULL OR ts.Codigo = @tipoCodigo)
      AND (
            @verTodo = 1
            OR s.SolicitanteUsuarioId = @usuarioId
            OR s.CreadaPorUsuarioId = @usuarioId
            OR EXISTS (
                SELECT 1 FROM dbo.TI_SolicitudParticipantes sp
                WHERE sp.SolicitudId = s.SolicitudId AND sp.UsuarioId = @usuarioId AND sp.Activo = 1
            )
          )
    ORDER BY s.FechaCreacion DESC;
END
GO

-- Detalle de una sola solicitud. La autorizacion (dueno/participante/rol
-- global) la aplica el controller comparando SolicitanteUsuarioId/
-- CreadaPorUsuarioId del resultado, o llamando a dbo.sp_TI_Solicitudes_EsParticipante.
CREATE OR ALTER PROCEDURE dbo.sp_TI_Solicitudes_Obtener
    @solicitudId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.SolicitudId, s.Folio, s.Titulo, s.DescripcionNecesidad, s.ProblemaActual, s.BeneficioEsperado,
        s.TipoSolicitudId, ts.Codigo AS TipoSolicitudCodigo, ts.Nombre AS TipoSolicitud,
        s.EstadoSolicitudId, es.Codigo AS EstadoCodigo, es.Nombre AS EstadoSolicitud, es.Etapa,
        s.PrioridadId, p.Codigo AS PrioridadCodigo, p.Nombre AS Prioridad,
        s.SolicitanteUsuarioId, s.CreadaPorUsuarioId, s.AreaSolicitante, s.EsConfidencial,
        s.MotivoRechazo, s.MotivoCancelacion,
        s.FechaCreacion, s.FechaEnvio, s.FechaUltimaActualizacion, s.FechaCierre,
        CONCAT(empSol.Nombre, ' ', empSol.ApellidoPaterno, ' ', empSol.ApellidoMaterno) AS SolicitanteNombre,
        CONCAT(empCrea.Nombre, ' ', empCrea.ApellidoPaterno, ' ', empCrea.ApellidoMaterno) AS CreadaPorNombre,
        empSol.Correo AS SolicitanteCorreo
    FROM dbo.TI_Solicitudes s
    INNER JOIN dbo.TI_TiposSolicitud ts ON ts.TipoSolicitudId = s.TipoSolicitudId
    INNER JOIN dbo.TI_EstadosSolicitud es ON es.EstadoSolicitudId = s.EstadoSolicitudId
    LEFT JOIN dbo.TI_Prioridades p ON p.PrioridadId = s.PrioridadId
    LEFT JOIN AppMH.dbo.Empleados empSol ON empSol.IDEmpleado = s.SolicitanteUsuarioId AND empSol.Estatus = 1
    LEFT JOIN AppMH.dbo.Empleados empCrea ON empCrea.IDEmpleado = s.CreadaPorUsuarioId AND empCrea.Estatus = 1
    WHERE s.SolicitudId = @solicitudId;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_TI_Solicitudes_EsParticipante
    @solicitudId BIGINT,
    @usuarioId   INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CAST(
        CASE WHEN EXISTS (
            SELECT 1 FROM dbo.TI_SolicitudParticipantes
            WHERE SolicitudId = @solicitudId AND UsuarioId = @usuarioId AND Activo = 1
        ) THEN 1 ELSE 0 END
    AS BIT);
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_TI_Solicitudes_Historial
    @solicitudId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        h.HistorialEstadoId, h.SolicitudId,
        h.EstadoAnteriorId, ea.Codigo AS EstadoAnteriorCodigo, ea.Nombre AS EstadoAnteriorNombre,
        h.EstadoNuevoId, en.Codigo AS EstadoNuevoCodigo, en.Nombre AS EstadoNuevoNombre,
        h.UsuarioCambioId, h.FechaCambio, h.Motivo, h.Origen,
        CONCAT(emp.Nombre, ' ', emp.ApellidoPaterno, ' ', emp.ApellidoMaterno) AS UsuarioCambioNombre
    FROM dbo.TI_SolicitudEstadosHistorial h
    LEFT JOIN dbo.TI_EstadosSolicitud ea ON ea.EstadoSolicitudId = h.EstadoAnteriorId
    INNER JOIN dbo.TI_EstadosSolicitud en ON en.EstadoSolicitudId = h.EstadoNuevoId
    LEFT JOIN AppMH.dbo.Empleados emp ON emp.IDEmpleado = h.UsuarioCambioId AND emp.Estatus = 1
    WHERE h.SolicitudId = @solicitudId
    ORDER BY h.FechaCambio ASC;
END
GO

-- Transiciones validas en esta fase (registro/revision). RAISERROR >= 50000
-- para que el AD lo traduzca a 400 (regla del repo: THROW de negocio).
-- Motivo obligatorio para RECHAZADA/CANCELADA ya se valida tambien en el
-- controller; aqui se vuelve a exigir por si el SP se invoca desde otro
-- lado en el futuro.
CREATE OR ALTER PROCEDURE dbo.sp_TI_Solicitudes_CambiarEstado
    @SolicitudId      BIGINT,
    @EstadoNuevoCodigo NVARCHAR(100),
    @usuarioId        INT,
    @Motivo           NVARCHAR(4000) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @EstadoActualId INT, @EstadoActualCodigo NVARCHAR(100);
    SELECT @EstadoActualId = s.EstadoSolicitudId, @EstadoActualCodigo = es.Codigo
    FROM dbo.TI_Solicitudes s
    INNER JOIN dbo.TI_EstadosSolicitud es ON es.EstadoSolicitudId = s.EstadoSolicitudId
    WHERE s.SolicitudId = @SolicitudId;

    IF @EstadoActualId IS NULL
    BEGIN
        RAISERROR('Solicitud no encontrada', 16, 1);
        RETURN;
    END

    IF (@EstadoNuevoCodigo IN ('RECHAZADA', 'CANCELADA') AND (@Motivo IS NULL OR LTRIM(RTRIM(@Motivo)) = ''))
    BEGIN
        RAISERROR('El motivo es obligatorio para este cambio de estado', 16, 1);
        RETURN;
    END

    DECLARE @Permitido BIT = 0;
    -- Catalogo de transiciones habilitadas en Fase 2A. Ampliar aqui cuando
    -- se implemente la fase de definicion de alcance en adelante.
    IF (@EstadoActualCodigo = 'ENVIADA'                AND @EstadoNuevoCodigo IN ('EN_REVISION', 'CANCELADA'))
        SET @Permitido = 1;
    ELSE IF (@EstadoActualCodigo = 'EN_REVISION'        AND @EstadoNuevoCodigo IN ('ESPERANDO_INFORMACION', 'RECHAZADA', 'CANCELADA'))
        SET @Permitido = 1;
    ELSE IF (@EstadoActualCodigo = 'ESPERANDO_INFORMACION' AND @EstadoNuevoCodigo IN ('EN_REVISION', 'CANCELADA'))
        SET @Permitido = 1;
    -- Ampliacion Fase 2B (definicion de alcance, estimacion y priorizacion).
    -- No se modifico ninguna linea de arriba; solo se agregaron estas ramas.
    ELSE IF (@EstadoActualCodigo = 'EN_REVISION' AND @EstadoNuevoCodigo = 'DEFINICION_ALCANCE')
        SET @Permitido = 1;
    ELSE IF (@EstadoActualCodigo = 'DEFINICION_ALCANCE' AND @EstadoNuevoCodigo = 'PENDIENTE_AUTORIZACION_ALCANCE')
        SET @Permitido = 1;
    ELSE IF (@EstadoActualCodigo = 'PENDIENTE_AUTORIZACION_ALCANCE' AND @EstadoNuevoCodigo IN ('EVALUACION_TECNICA', 'PENDIENTE_PRIORIZACION', 'DEFINICION_ALCANCE'))
        SET @Permitido = 1;
    ELSE IF (@EstadoActualCodigo = 'EVALUACION_TECNICA' AND @EstadoNuevoCodigo IN ('PENDIENTE_PRIORIZACION', 'DEFINICION_ALCANCE'))
        SET @Permitido = 1;
    ELSE IF (@EstadoActualCodigo = 'PENDIENTE_PRIORIZACION' AND @EstadoNuevoCodigo IN ('PRIORIZADA', 'EN_BACKLOG', 'RECHAZADA', 'DEFINICION_ALCANCE', 'EN_REVISION_PRIORIDAD'))
        SET @Permitido = 1;
    -- Ver nota de seguridad en 03_Actualizacion_CambiarEstado_Fase2B.sql:
    -- la SALIDA de EN_REVISION_PRIORIDAD no se agrega aqui a proposito.
    ELSE IF (@EstadoActualCodigo = 'PRIORIZADA' AND @EstadoNuevoCodigo IN ('EN_BACKLOG', 'RECHAZADA', 'DEFINICION_ALCANCE'))
        SET @Permitido = 1;
    ELSE IF (@EstadoActualCodigo = 'EN_BACKLOG' AND @EstadoNuevoCodigo IN ('PRIORIZADA', 'RECHAZADA', 'DEFINICION_ALCANCE', 'EN_REVISION_PRIORIDAD'))
        SET @Permitido = 1;

    IF (@Permitido = 0)
    BEGIN
        RAISERROR('La transicion de estado solicitada no esta permitida', 16, 1);
        RETURN;
    END

    DECLARE @EstadoNuevoId INT = (SELECT EstadoSolicitudId FROM dbo.TI_EstadosSolicitud WHERE Codigo = @EstadoNuevoCodigo);

    UPDATE dbo.TI_Solicitudes
       SET EstadoSolicitudId = @EstadoNuevoId,
           FechaUltimaActualizacion = SYSUTCDATETIME(),
           MotivoRechazo = CASE WHEN @EstadoNuevoCodigo = 'RECHAZADA' THEN @Motivo ELSE MotivoRechazo END,
           MotivoCancelacion = CASE WHEN @EstadoNuevoCodigo = 'CANCELADA' THEN @Motivo ELSE MotivoCancelacion END,
           FechaCierre = CASE WHEN @EstadoNuevoCodigo IN ('RECHAZADA', 'CANCELADA') THEN SYSUTCDATETIME() ELSE FechaCierre END
     WHERE SolicitudId = @SolicitudId;

    INSERT INTO dbo.TI_SolicitudEstadosHistorial (SolicitudId, EstadoAnteriorId, EstadoNuevoId, UsuarioCambioId, FechaCambio, Motivo, Origen)
    VALUES (@SolicitudId, @EstadoActualId, @EstadoNuevoId, @usuarioId, SYSUTCDATETIME(), @Motivo, 'API');

    INSERT INTO dbo.TI_AuditoriaEventos (SolicitudId, UsuarioId, Entidad, EntidadId, Accion, FechaEvento, ValorAnterior, ValorNuevo, Origen)
    VALUES (@SolicitudId, @usuarioId, 'TI_Solicitudes', CAST(@SolicitudId AS NVARCHAR(200)), 'CAMBIAR_ESTADO', SYSUTCDATETIME(), @EstadoActualCodigo, @EstadoNuevoCodigo, 'API');
END
GO
