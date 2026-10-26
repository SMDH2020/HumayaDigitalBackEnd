using System.ComponentModel.DataAnnotations;

namespace HD_Ventas.Modelos.ObjetivosModelos
{
    public class mdl_Objetivos_Modelos_Guardar_Detalle
    {
        [Range(1, int.MaxValue, ErrorMessage = "El idgeneral del modelo es un valor requerido.")]
        public int idgeneral { get; set; }

        [Range(1, 12, ErrorMessage = "El periodo debe estar entre 1 y 12.")]
        public int periodo { get; set; }

        // 0 es un valor valido: es como el usuario quita un objetivo ya capturado.
        // Es nullable para detectar cuando el campo no viene en el JSON (si no, llegaria como 0).
        [Required(ErrorMessage = "El objetivo es un valor requerido.")]
        [Range(0, 9999, ErrorMessage = "El objetivo debe estar entre 0 y 9999.")]
        public int? objetivo { get; set; }
    }
}
