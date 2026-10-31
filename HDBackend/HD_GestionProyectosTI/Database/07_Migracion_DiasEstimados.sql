-- ============================================================================
-- HumayaDigital_GestionProyectosTI -- Migracion: estimacion en dias por
-- funcionalidad, dentro de la definicion de alcance.
-- Script preparado para REVISION Y EJECUCION MANUAL. No se ha ejecutado
-- contra ningun servidor.
--
-- Cambio de persistencia autorizado explicitamente para este bloque: una
-- sola columna nueva, NULLABLE, en dbo.TI_Funcionalidades. No crea tablas,
-- no toca TI_Estimaciones/TI_EstimacionDetalle (siguen existiendo intactas,
-- como historico de la estimacion en horas de la version anterior de este
-- flujo), no convierte ningun valor historico de horas a dias.
--
-- Tipo de dato: DECIMAL(6,2) -- mismo patron de precision ya usado en el
-- modulo para magnitudes fraccionarias (TI_Estimaciones.Horas*,
-- TI_CriteriosPrioridad.Peso usan DECIMAL(5,2)/DECIMAL(10,2)); se amplia a
-- 6 enteros para admitir estimaciones de alcance grandes sin desbordar.
-- Nullable: las funcionalidades ya existentes (de versiones historicas
-- creadas antes de este cambio) deben conservar DiasEstimados en NULL, no
-- en 0 -- 0 significaria "se estimo en cero dias", que es un dato distinto
-- a "nunca se capturo".
--
-- Idempotente: se verifica con sys.columns antes de alterar, para poder
-- ejecutarse varias veces sin error ni efecto duplicado.
-- ============================================================================

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.TI_Funcionalidades') AND name = 'DiasEstimados'
)
BEGIN
    ALTER TABLE dbo.TI_Funcionalidades ADD DiasEstimados DECIMAL(6, 2) NULL;
END
GO
