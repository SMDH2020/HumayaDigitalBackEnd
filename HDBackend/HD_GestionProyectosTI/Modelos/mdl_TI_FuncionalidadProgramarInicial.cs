using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_TI_FuncionalidadProgramarInicial
    {
        [Range(1, long.MaxValue, ErrorMessage = "El FuncionalidadId es un valor requerido")]
        public long FuncionalidadId { get; set; }

        [Range(1, 12, ErrorMessage = "El mes programado debe estar entre 1 y 12")]
        public byte MesProgramado { get; set; }

        [Range(2000, 2100, ErrorMessage = "El anio programado no es valido")]
        public short AnioProgramado { get; set; }
    }
}
