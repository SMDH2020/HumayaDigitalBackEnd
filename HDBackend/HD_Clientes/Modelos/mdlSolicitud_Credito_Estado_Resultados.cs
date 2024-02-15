using System.ComponentModel.DataAnnotations;

namespace HD.Clientes.Modelos
{
    public class mdlSolicitud_Credito_Estado_Resultados
    {

        [Required(ErrorMessage = "El folio es un valor requerido")]
        [RegularExpression("^[SC0-9]+$", ErrorMessage = "El campo folio debe estar formado solo por caracteres numericos e iniciales SC")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El campo folio debe estar formado por 13 digitos")]
        public string? folio { get; set; }

        public double in_agricolas { get; set; }
        public double in_ganado { get; set; }
        public double in_leche { get; set; }
        public double in_maquilas { get; set; }
        public double in_procampo { get; set; }
        public double in_rentas { get; set; }
        public double in_sueldos { get; set; }
        public double in_otros { get; set; }
        public double eg_agricolas { get; set; }
        public double eg_ganaderos { get; set; }
        public double eg_maquilas { get; set; }
        public double eg_terrenos { get; set; }
        public double eg_refaccionarios { get; set; }
        public double eg_intereses { get; set; }
        public double eg_impuestos { get; set; }
        public double eg_familiares { get; set; }
        public double eg_otros { get; set; }
        public string? usuario { get; set; } = "";
    }
}
