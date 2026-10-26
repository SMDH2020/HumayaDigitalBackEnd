-- ============================================================================
-- HumayaDigital_GestionProyectosTI -- Permitir agregar funcionalidades a una
-- version de alcance YA CREADA, mientras ninguno de los dos aprobadores
-- (SOLICITANTE / JEFATURA_TI) haya registrado todavia su aprobacion.
-- Script preparado para REVISION Y EJECUCION MANUAL. No se ha ejecutado
-- contra ningun servidor.
--
-- Contexto: hoy sp_TI_VersionesAlcance_Crear es la UNICA via para registrar
-- funcionalidades -- recibe la lista completa al crear la version y, de
-- hecho, rechaza crear una nueva version si ya existe otra en curso
-- (BORRADOR/PENDIENTE_APROBACION) para la misma solicitud. No existia
-- ningun SP ni endpoint para agregar una funcionalidad a una version ya
-- persistida. Este script agrega exactamente eso: sp_TI_Funcionalidades_Agregar
-- (NUEVO).
--
-- Identificacion de "aprobacion registrada" (sin inventar columnas ni
-- propiedades): se usa dbo.TI_AprobacionesAlcance, la misma tabla que ya
-- resuelve sp_TI_AprobacionesAlcance_Registrar. Mientras la version esta en
-- BORRADOR no existe ninguna fila ahi (se crean las 2, en PENDIENTE, hasta
-- que se llama a sp_TI_VersionesAlcance_EnviarAprobacion). Una vez en
-- PENDIENTE_APROBACION, cada lado resuelve su propia fila de forma
-- independiente -- por eso "ya existe una aprobacion registrada" se
-- identifica de forma confiable como:
--   EXISTS (SELECT 1 FROM TI_AprobacionesAlcance
--            WHERE VersionAlcanceId = @VersionAlcanceId AND EstadoAprobacion = 'APROBADA')
-- Esto cubre exactamente la regla pedida: el bloqueo aplica en cuanto
-- CUALQUIERA de los dos aprobo, sin importar si el otro lado sigue
-- PENDIENTE (un RECHAZADA ya cierra la version a estado RECHAZADA, que
-- tambien queda bloqueado por la validacion de EstadoVersion de abajo).
--
-- Reglas que valida el SP (nunca basta con ocultar el boton en pantalla):
--   - La version debe existir.
--   - EstadoVersion debe ser BORRADOR o PENDIENTE_APROBACION; cualquier otro
--     estado (APROBADA, RECHAZADA, SUSTITUIDA) rechaza la operacion.
--   - No debe existir ninguna fila APROBADA en TI_AprobacionesAlcance para
--     esa version.
--   - Las demas funcionalidades de la version NO se tocan ni se reordenan.
--
-- No se crea ninguna tabla ni columna. No se modifica el mecanismo de
-- aprobacion dual ni el historial de cambios (TI_AuditoriaEventos no se usa
-- aqui porque sp_TI_VersionesAlcance_Crear tampoco audita el alta inicial
-- de funcionalidades -- se mantiene el mismo criterio).
-- ============================================================================

CREATE OR ALTER PROCEDURE dbo.sp_TI_Funcionalidades_Agregar
    @VersionAlcanceId BIGINT,
    @Codigo NVARCHAR(30) = NULL,
    @Nombre NVARCHAR(200),
    @Descripcion NVARCHAR(MAX),
    @ReglasNegocio NVARCHAR(MAX) = NULL,
    @CriteriosAceptacion NVARCHAR(MAX),
    @PrioridadInterna TINYINT = 3,
    @Complejidad NVARCHAR(20) = NULL,
    @Orden INT,
    @DiasEstimados DECIMAL(6, 2) = NULL,
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @LockResult INT;
        EXEC @LockResult = sp_getapplock @Resource = @VersionAlcanceId, @LockMode = 'Exclusive', @LockOwner = 'Transaction', @LockTimeout = 5000;
        IF (@LockResult < 0)
        BEGIN
            RAISERROR('No se pudo asegurar la operacion de forma exclusiva, intente de nuevo', 16, 1);
            RETURN;
        END

        DECLARE @EstadoVersionActual NVARCHAR(30);
        SELECT @EstadoVersionActual = EstadoVersion FROM dbo.TI_VersionesAlcance WHERE VersionAlcanceId = @VersionAlcanceId;

        IF (@EstadoVersionActual IS NULL)
        BEGIN
            RAISERROR('La version de alcance indicada no existe', 16, 1);
            RETURN;
        END
        IF (@EstadoVersionActual NOT IN ('BORRADOR', 'PENDIENTE_APROBACION'))
        BEGIN
            RAISERROR('La version ya no admite agregar funcionalidades', 16, 1);
            RETURN;
        END
        IF (@DiasEstimados IS NOT NULL AND @DiasEstimados < 0)
        BEGIN
            RAISERROR('Los dias estimados no pueden ser negativos', 16, 1);
            RETURN;
        END

        IF EXISTS (SELECT 1 FROM dbo.TI_AprobacionesAlcance WHERE VersionAlcanceId = @VersionAlcanceId AND EstadoAprobacion = 'APROBADA')
        BEGIN
            RAISERROR('El alcance ya cuenta con una aprobacion y no se pueden agregar nuevas funcionalidades', 16, 1);
            RETURN;
        END

        INSERT INTO dbo.TI_Funcionalidades (VersionAlcanceId, Codigo, Nombre, Descripcion, ReglasNegocio, CriteriosAceptacion, PrioridadInterna, Complejidad, Orden, DiasEstimados, Activa, CreadaPorUsuarioId, FechaCreacion)
        VALUES (@VersionAlcanceId, @Codigo, @Nombre, @Descripcion, @ReglasNegocio, @CriteriosAceptacion, @PrioridadInterna, @Complejidad, @Orden, @DiasEstimados, 1, @usuarioId, SYSUTCDATETIME());

        DECLARE @FuncionalidadId BIGINT = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
        SELECT @FuncionalidadId AS FuncionalidadId;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
