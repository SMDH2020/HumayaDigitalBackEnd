-- ============================================================================
-- HumayaDigital_GestionProyectosTI -- Permitir editar una funcionalidad ya
-- registrada en una version de alcance, mientras ninguno de los dos
-- aprobadores (SOLICITANTE / JEFATURA_TI) haya registrado todavia su
-- aprobacion. Script preparado para REVISION Y EJECUCION MANUAL. No se ha
-- ejecutado contra ningun servidor.
--
-- Motivo: hoy no hay forma de ver (ni editar) Descripcion, ReglasNegocio o
-- CriteriosAceptacion de una funcionalidad una vez capturada -- la tabla de
-- la pantalla de alcance nunca mostro esas columnas. Este script agrega
-- sp_TI_Funcionalidades_Editar (NUEVO), hermano de sp_TI_Funcionalidades_Agregar
-- (14_Alcance_AgregarFuncionalidad.sql): misma regla de bloqueo por
-- aprobacion, mismo criterio de "aprobacion registrada" (EXISTS en
-- dbo.TI_AprobacionesAlcance con EstadoAprobacion = 'APROBADA'), sin crear
-- ninguna tabla ni columna.
--
-- Reglas que valida el SP (nunca basta con deshabilitar el formulario en
-- pantalla):
--   - La funcionalidad debe existir y estar activa.
--   - La version a la que pertenece debe estar en BORRADOR o
--     PENDIENTE_APROBACION; cualquier otro estado rechaza la edicion.
--   - No debe existir ninguna fila APROBADA en TI_AprobacionesAlcance para
--     esa version.
--
-- Campos editables: Codigo, Nombre, Descripcion, ReglasNegocio,
-- CriteriosAceptacion, Complejidad, DiasEstimados -- los mismos que ya se
-- capturan al agregar. Orden, Activa, CreadaPorUsuarioId y FechaCreacion no
-- se tocan.
-- ============================================================================

CREATE OR ALTER PROCEDURE dbo.sp_TI_Funcionalidades_Editar
    @FuncionalidadId BIGINT,
    @Codigo NVARCHAR(30) = NULL,
    @Nombre NVARCHAR(200),
    @Descripcion NVARCHAR(MAX),
    @ReglasNegocio NVARCHAR(MAX) = NULL,
    @CriteriosAceptacion NVARCHAR(MAX),
    @Complejidad NVARCHAR(20) = NULL,
    @DiasEstimados DECIMAL(6, 2) = NULL,
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @VersionAlcanceId BIGINT;
        SELECT @VersionAlcanceId = VersionAlcanceId FROM dbo.TI_Funcionalidades WHERE FuncionalidadId = @FuncionalidadId AND Activa = 1;

        IF (@VersionAlcanceId IS NULL)
        BEGIN
            RAISERROR('La funcionalidad indicada no existe o no esta activa', 16, 1);
            RETURN;
        END

        DECLARE @LockResult INT;
        EXEC @LockResult = sp_getapplock @Resource = @VersionAlcanceId, @LockMode = 'Exclusive', @LockOwner = 'Transaction', @LockTimeout = 5000;
        IF (@LockResult < 0)
        BEGIN
            RAISERROR('No se pudo asegurar la operacion de forma exclusiva, intente de nuevo', 16, 1);
            RETURN;
        END

        DECLARE @EstadoVersionActual NVARCHAR(30);
        SELECT @EstadoVersionActual = EstadoVersion FROM dbo.TI_VersionesAlcance WHERE VersionAlcanceId = @VersionAlcanceId;

        IF (@EstadoVersionActual NOT IN ('BORRADOR', 'PENDIENTE_APROBACION'))
        BEGIN
            RAISERROR('La version ya no admite editar funcionalidades', 16, 1);
            RETURN;
        END
        IF (@DiasEstimados IS NOT NULL AND @DiasEstimados < 0)
        BEGIN
            RAISERROR('Los dias estimados no pueden ser negativos', 16, 1);
            RETURN;
        END

        IF EXISTS (SELECT 1 FROM dbo.TI_AprobacionesAlcance WHERE VersionAlcanceId = @VersionAlcanceId AND EstadoAprobacion = 'APROBADA')
        BEGIN
            RAISERROR('El alcance ya cuenta con una aprobacion y no se pueden editar funcionalidades', 16, 1);
            RETURN;
        END

        UPDATE dbo.TI_Funcionalidades
           SET Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, ReglasNegocio = @ReglasNegocio,
               CriteriosAceptacion = @CriteriosAceptacion, Complejidad = @Complejidad, DiasEstimados = @DiasEstimados
         WHERE FuncionalidadId = @FuncionalidadId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
