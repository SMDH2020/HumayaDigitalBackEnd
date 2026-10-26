-- ============================================================================
-- HumayaDigital_GestionProyectosTI -- Actualizacion aislada de Fase 2B
-- Script preparado para REVISION Y EJECUCION MANUAL. No se ejecuto contra
-- ningun servidor.
--
-- Proposito: permitir instalar SOLO la ampliacion del catalogo de
-- transiciones de dbo.sp_TI_Solicitudes_CambiarEstado en un ambiente donde
-- Fase 2A ya esta instalada, sin tener que volver a ejecutar el script
-- completo 01_Schema_Y_StoredProcedures_Fase2A.sql (que tambien recrearia
-- los otros 7 procedimientos de esa fase).
--
-- Contenido: el cuerpo completo es IDENTICO al que ya quedo en
-- 01_Schema_Y_StoredProcedures_Fase2A.sql (porque CREATE OR ALTER PROCEDURE
-- siempre requiere el cuerpo completo, no hay sintaxis de "solo agregar
-- rama"). La unica diferencia frente a la version original de Fase 2A son
-- las 6 ramas ELSE IF marcadas como "Ampliacion Fase 2B" mas abajo -- se
-- revise linea por linea contra el original y no se modifico ninguna otra
-- condicion, mensaje de error, ni el orden de las validaciones previas
-- (solicitud no encontrada, motivo obligatorio para RECHAZADA/CANCELADA).
--
-- IMPORTANTE sobre idempotencia:
-- CREATE OR ALTER PROCEDURE es idempotente por definicion (reemplaza la
-- definicion existente sin tocar datos). Este script no contiene ninguna
-- sentencia CREATE TABLE, ALTER TABLE ni INSERT de datos -- se puede
-- ejecutar cuantas veces sea necesario sin riesgo de duplicar objetos ni
-- filas. Lo mismo aplica, de hecho, a los 8 procedimientos completos de
-- 01_Schema_Y_StoredProcedures_Fase2A.sql (se revisaron los 297 renglones
-- del archivo: tampoco contiene CREATE TABLE ni INSERT de semilla, solo
-- CREATE OR ALTER PROCEDURE). Aun asi, para esta actualizacion puntual se
-- prefiere este script aislado en vez de volver a correr el archivo
-- completo de Fase 2A, para minimizar el area tocada en cada despliegue.
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
    -- endpoint generico (arriba). La SALIDA (-> TERMINADA o de vuelta a
    -- PENDIENTE_PRIORIZACION) NO se agrega aqui a proposito: solo la puede
    -- resolver sp_TI_Solicitudes_ResolverRevisionPrioridad, que valida la
    -- identidad real del solicitante -- si se agregara aqui, Jefatura de TI
    -- podria alcanzar TERMINADA directo por este mismo endpoint generico y
    -- saltarse esa aprobacion.
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
