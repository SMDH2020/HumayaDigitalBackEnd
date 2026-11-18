using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Modelos.AnalisisInventarioModelos
{
    public class mdl_Proyeccion_Pedidos_Detalle
    {
            [Range(1, int.MaxValue, ErrorMessage = "El idgeneral del modelo es un valor requerido.")]
            public int idgeneral { get; set; }

            // Cada fila trae su ejercicio: en un mismo envio pueden ir varios años (ej. 2026 y 2027)
            [Range(2024, 2100, ErrorMessage = "El ejercicio debe ser un año válido (2024 en adelante).")]
            public int ejercicio { get; set; }

            [Range(1, 12, ErrorMessage = "El periodo debe estar entre 1 y 12.")]
            public int periodo { get; set; }

            // 0 es un valor valido: es como el usuario quita un objetivo ya capturado.
            // Es nullable para detectar cuando el campo no viene en el JSON (si no, llegaria como 0).
            [Required(ErrorMessage = "La cantidad es un valor requerido.")]
            [Range(0, 9999, ErrorMessage = "La cantidad debe estar entre 0 y 9999.")]
            public int? cantidad { get; set; }
    }
}
