-- ============================================================================
-- HumayaDigital_GestionProyectosTI -- Correccion puntual: permitir enviar a
-- revision de prioridad tambien desde EN_BACKLOG (no solo desde
-- PENDIENTE_PRIORIZACION). Script preparado para REVISION Y EJECUCION
-- MANUAL. No se ha ejecutado contra ningun servidor.
--
-- Motivo: una solicitud de prueba quedo en EN_BACKLOG (una funcionalidad
-- fue priorizada y luego regresada al backlog) y no tenia ninguna
-- transicion disponible hacia EN_REVISION_PRIORIDAD. Se agrega UNA sola
-- rama a la whitelist ya existente; no se modifico ninguna otra condicion,
-- mensaje de error ni el resto del procedimiento.
--
-- Si ya se ejecuto el script 11_Instalacion_PrioridadPorFuncionalidad.sql,
-- este script reemplaza la misma definicion con la rama EN_BACKLOG ya
-- incluida (CREATE OR ALTER PROCEDURE es idempotente, no requiere orden
-- especial ni revertir nada antes).
-- ============================================================================

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
    -- Revision de la priorizacion por funcionalidad (detalle de solicitud):
    -- la entrada a EN_REVISION_PRIORIDAD la dispara Jefatura de TI por este
    -- endpoint generico (arriba), desde PENDIENTE_PRIORIZACION o desde
    -- EN_BACKLOG (correccion de este script). La SALIDA (-> TERMINADA o de
    -- vuelta a PENDIENTE_PRIORIZACION) NO se agrega aqui a proposito: solo
    -- la puede resolver sp_TI_Solicitudes_ResolverRevisionPrioridad, que
    -- valida la identidad real del solicitante -- si se agregara aqui,
    -- Jefatura de TI podria alcanzar TERMINADA directo por este mismo
    -- endpoint generico y saltarse esa aprobacion.
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
