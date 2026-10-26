-- ─────────────────────────────────────────────────────────────────────────
-- Limpieza de datos de prueba de Gestión de Proyectos de TI.
--
-- SOLO borra tablas TRANSACCIONALES (solicitudes y todo lo que cuelga de
-- ellas). NO toca catálogos/configuración -- borrarlos rompería el módulo:
--   dbo.TI_TiposSolicitud, dbo.TI_EstadosSolicitud, dbo.TI_Prioridades,
--   dbo.TI_Roles, dbo.TI_UsuarioRoles, dbo.TI_CriteriosPrioridad
--
-- Orden de borrado: hijos antes que padres (respeta FKs). Todo en una sola
-- transacción -- si algo falla, no queda a medias.
--
-- Folio = columna calculada a partir de SolicitudId (ver
-- 01_Schema_Y_StoredProcedures_Fase2A.sql, sp_TI_Solicitudes_Crear) -- por
-- eso se reinicia el IDENTITY de TI_Solicitudes al final, para que la
-- primera solicitud real vuelva a generar folio ...-0000001.
--
-- ADVERTENCIA: ejecutar una sola vez, antes de capturar datos reales.
-- Revisar el nombre de la base de datos conectada antes de correrlo.
-- ─────────────────────────────────────────────────────────────────────────

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRANSACTION;

    -- Tablas de uso activo hoy ------------------------------------------------
    DELETE FROM dbo.TI_AuditoriaEventos;
    DELETE FROM dbo.TI_CambiosProgramacion;
    DELETE FROM dbo.TI_SolicitudEstadosHistorial;
    DELETE FROM dbo.TI_SolicitudParticipantes;
    DELETE FROM dbo.TI_Funcionalidades;
    DELETE FROM dbo.TI_VersionesAlcance;
    DELETE FROM dbo.TI_Solicitudes;

    -- Tablas de un diseño anterior ya no usado por las pantallas actuales
    -- (EstimacionTI.js, PriorizacionTI.js -- sin referencias en el código
    -- activo). Se incluyen por si llegaron a tener datos de prueba sueltos;
    -- el IF OBJECT_ID evita error si alguna ya no existe en tu base.
    IF OBJECT_ID('dbo.TI_EvaluacionPrioridadDetalle', 'U') IS NOT NULL
        DELETE FROM dbo.TI_EvaluacionPrioridadDetalle;
    IF OBJECT_ID('dbo.TI_EvaluacionesPrioridad', 'U') IS NOT NULL
        DELETE FROM dbo.TI_EvaluacionesPrioridad;
    IF OBJECT_ID('dbo.TI_DecisionesPrioridad', 'U') IS NOT NULL
        DELETE FROM dbo.TI_DecisionesPrioridad;
    IF OBJECT_ID('dbo.TI_AprobacionesPrioridad', 'U') IS NOT NULL
        DELETE FROM dbo.TI_AprobacionesPrioridad;
    IF OBJECT_ID('dbo.TI_AprobacionesAlcance', 'U') IS NOT NULL
        DELETE FROM dbo.TI_AprobacionesAlcance;
    IF OBJECT_ID('dbo.TI_EstimacionDetalle', 'U') IS NOT NULL
        DELETE FROM dbo.TI_EstimacionDetalle;
    IF OBJECT_ID('dbo.TI_Estimaciones', 'U') IS NOT NULL
        DELETE FROM dbo.TI_Estimaciones;

    -- Reinicia el folio: la siguiente solicitud real sera SolicitudId = 1.
    DBCC CHECKIDENT ('dbo.TI_Solicitudes', RESEED, 0);

COMMIT TRANSACTION;
