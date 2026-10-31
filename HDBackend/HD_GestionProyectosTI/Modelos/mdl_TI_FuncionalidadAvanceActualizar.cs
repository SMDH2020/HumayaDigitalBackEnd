using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    // Input para actualizar manualmente el % de avance de una funcionalidad
    // desde la pantalla de Backlog Mensual (accion directa de Jefatura de
    // TI, sin aprobacion).
    public class mdl_TI_FuncionalidadAvanceActualizar
    {
        [Range(1, long.MaxValue, ErrorMessage = "La funcionalidad es un valor requerido")]
        public long FuncionalidadId { get; set; }

        [Range(0, 100, ErrorMessage = "El avance debe estar entre 0 y 100")]
        public byte? AvanceManual { get; set; }
    }
}
