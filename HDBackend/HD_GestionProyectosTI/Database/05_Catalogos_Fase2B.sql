-- ============================================================================
-- HumayaDigital_GestionProyectosTI -- Catalogos de solo lectura para Fase 2B
-- Script preparado para REVISION Y EJECUCION MANUAL. No se ha ejecutado
-- contra ningun servidor.
--
-- Motivo: la pantalla de evaluacion de prioridad necesita mostrar el
-- Nombre/Peso de los 6 criterios activos (TI_CriteriosPrioridad), y la
-- pantalla de decision PRIORIZAR necesita un selector de TI_Prioridades.
-- Ningun SP de Fase 2A/2B expone estas 2 tablas al cliente -- son de solo
-- lectura, no agregan reglas de negocio nuevas ni tocan ninguna tabla.
--
-- VERIFICACION DE COLUMNAS (contra el resto del repositorio, sin acceso a
-- la base y sin ejecutar nada): no existe en este repositorio ningun script
-- de creacion de dbo.TI_CriteriosPrioridad. Las UNICAS columnas con uso real
-- confirmado en el repo son CriterioPrioridadId, Peso y Activo
-- (sp_TI_EvaluacionesPrioridad_Crear, en 02_StoredProcedures_Fase2B.sql:
-- "SELECT CriterioPrioridadId, Peso FROM dbo.TI_CriteriosPrioridad ... WHERE
-- Activo = 1"). Codigo/Nombre/Descripcion/Orden que se habian incluido
-- antes NO tienen ninguna referencia real en el repo -- se quitaron de este
-- SP para no inventar nombres de columna. El frontend ya tiene previsto
-- este caso (muestra "Criterio {id}" cuando no hay nombre disponible). Si
-- en desarrollo se confirma que existen columnas adicionales (Nombre,
-- Descripcion, Orden u otras), hay que agregarlas aqui con su nombre real
-- antes de usarlas.
-- ============================================================================

CREATE OR ALTER PROCEDURE dbo.sp_TI_Catalogos_CriteriosPrioridad_Listado
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CriterioPrioridadId, Peso
    FROM dbo.TI_CriteriosPrioridad
    WHERE Activo = 1
    ORDER BY CriterioPrioridadId;
END
GO

-- Columnas confirmadas por uso real en 01_Schema_Y_StoredProcedures_Fase2A.sql
-- (sp_TI_Solicitudes_Listado/Obtener: "p.Codigo AS PrioridadCodigo, p.Nombre
-- AS Prioridad" via JOIN contra dbo.TI_Prioridades) -- PrioridadId, Codigo y
-- Nombre si existen.
CREATE OR ALTER PROCEDURE dbo.sp_TI_Catalogos_Prioridades_Listado
AS
BEGIN
    SET NOCOUNT ON;
    SELECT PrioridadId, Codigo, Nombre
    FROM dbo.TI_Prioridades
    ORDER BY PrioridadId;
END
GO
