-- Detalle de solo lectura de una funcionalidad por id, usado por el modal de
-- "ver detalle" en Backlog Mensual (el listado de BacklogTI/Mensual no trae
-- Descripcion/ReglasNegocio/CriteriosAceptacion/Complejidad).
CREATE OR ALTER PROCEDURE dbo.sp_TI_Funcionalidades_Obtener
    @FuncionalidadId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        f.FuncionalidadId,
        f.Codigo,
        f.Nombre,
        f.Descripcion,
        f.ReglasNegocio,
        f.CriteriosAceptacion,
        f.Complejidad,
        f.DiasEstimados
    FROM dbo.TI_Funcionalidades f
    WHERE f.FuncionalidadId = @FuncionalidadId;
END
