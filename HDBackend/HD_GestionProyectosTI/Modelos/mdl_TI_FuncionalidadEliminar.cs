using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_TI_FuncionalidadEliminar
    {
        [Range(1, long.MaxValue, ErrorMessage = "El FuncionalidadId es un valor requerido")]
        public long FuncionalidadId { get; set; }
    }
}
