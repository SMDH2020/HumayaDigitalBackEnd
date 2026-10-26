using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_TI_FuncionalidadActualizarDias
    {
        [Range(1, long.MaxValue, ErrorMessage = "El FuncionalidadId es un valor requerido")]
        public long FuncionalidadId { get; set; }

        [Range(typeof(decimal), "0", "9999999", ErrorMessage = "Los dias estimados no pueden ser negativos")]
        public decimal? DiasEstimados { get; set; }
    }
}
