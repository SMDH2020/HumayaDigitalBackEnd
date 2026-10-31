using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_TI_CambioProgramacionCrear
    {
        [Range(1, long.MaxValue, ErrorMessage = "El FuncionalidadId es un valor requerido")]
        public long FuncionalidadId { get; set; }

        [Range(1, 12, ErrorMessage = "El mes propuesto debe estar entre 1 y 12")]
        public byte MesPropuesto { get; set; }

        [Range(2000, 2100, ErrorMessage = "El anio propuesto no es valido")]
        public short AnioPropuesto { get; set; }

        [Required(ErrorMessage = "El motivo es un valor requerido")]
        public string Motivo { get; set; } = "";
    }
}
