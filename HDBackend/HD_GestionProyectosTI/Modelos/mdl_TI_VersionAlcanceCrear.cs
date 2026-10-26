using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_TI_VersionAlcanceCrear
    {
        [Range(1, long.MaxValue, ErrorMessage = "El identificador de la solicitud es un valor requerido")]
        public long SolicitudId { get; set; }

        [Required(ErrorMessage = "El objetivo es un valor requerido")]
        public string Objetivo { get; set; } = "";

        [Required(ErrorMessage = "El alcance incluido es un valor requerido")]
        public string AlcanceIncluido { get; set; } = "";

        public string? AlcanceExcluido { get; set; }
        public string? Supuestos { get; set; }
        public string? Dependencias { get; set; }

        [MinLength(1, ErrorMessage = "Debe incluir al menos una funcionalidad")]
        public List<mdl_TI_FuncionalidadCrear> Funcionalidades { get; set; } = new();

        // Lo llena el controller desde ISesion -- nunca viaja del cliente.
        public int UsuarioId { get; set; }
    }
}
