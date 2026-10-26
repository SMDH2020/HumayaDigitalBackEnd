namespace HD_GestionProyectosTI.Modelos
{
    // Solo las columnas con uso real confirmado en el repositorio
    // (sp_TI_EvaluacionesPrioridad_Crear). No existe en el repo ningun
    // script de creacion de dbo.TI_CriteriosPrioridad que confirme Nombre/
    // Descripcion/Orden -- no se agregan hasta confirmarse en desarrollo.
    public class mdl_TI_CriterioPrioridad
    {
        public int CriterioPrioridadId { get; set; }
        public decimal Peso { get; set; }
    }
}
