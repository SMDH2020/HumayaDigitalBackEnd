using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos
{
    public class mdl_Agregar_Precio_Garantia
    {

        [Required(ErrorMessage = "El modelo es un valor requerido")]
        [RegularExpression(@"^[. , - # $ % ñ Ñ a-zA-Z0-9]+$", ErrorMessage = "El campo modelo debe contener solo letras y numeros")]
        [StringLength(20, ErrorMessage = "El modelo no debe exceder los 20 caracteres")]
        public string modelo { get; set; }


        [Required(ErrorMessage = "El precio original es un valor requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El campo precio de lista esta fuera de rango")]
        public float precio_original { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El campo precio de lista esta fuera de rango")]
        public float precio_ajustado { get; set; }
        public string? inicio_vigencia { get; set; }
        public string? vigencia { get; set; }
        public int usuario { get; set; }
    }
}
