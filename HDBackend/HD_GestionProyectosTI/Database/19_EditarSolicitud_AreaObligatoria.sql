-- ============================================================================
-- HumayaDigital_GestionProyectosTI -- Ajustes al registro de solicitudes:
-- 1) Area solicitante pasa a ser obligatoria (antes era opcional, texto
--    libre). El front ahora manda uno de un catalogo fijo de areas via
--    dropdown; aqui solo se valida que no venga vacia -- el catalogo de
--    valores permitidos vive en el frontend, no en una tabla nueva.
-- 2) Se agrega sp_TI_Solicitudes_Editar, de solo escritura de los datos
--    base de la solicitud (titulo, descripciones, tipo, area). Bloqueado
--    en cuanto exista una version de alcance APROBADA para la solicitud --
--    a partir de ahi la solicitud ya no se debe modificar desde esta
--    pantalla, solo consultar.
--
-- Script preparado para REVISION Y EJECUCION MANUAL. No se ha ejecutado
-- contra ningun servidor.
-- ============================================================================

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

-- Edita los datos base de una solicitud ya registrada. Bloqueada en cuanto
-- la solicitud tenga una version de alcance APROBADA (sin importar si ya
-- hay versiones posteriores en otro estado) -- a partir de ahi el detalle
-- se consulta de manera informativa en el frontend, no se vuelve a abrir
-- el formulario de edicion.
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
