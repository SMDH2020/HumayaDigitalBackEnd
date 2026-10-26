using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_TI_SolicitudEliminar
    {
        [Range(1, long.MaxValue)]
        public long SolicitudId { get; set; }
    }
}
