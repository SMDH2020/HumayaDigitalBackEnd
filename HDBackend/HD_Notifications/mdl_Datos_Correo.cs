using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Notifications
{
    public class mdl_Datos_Correo
    {
        /// <summary>Opcional. Va en la "píldora" de la derecha del encabezado (FOLIO xxx).</summary>
        public string? Folio { get; set; }

        /// <summary>Texto pequeño sobre el título (ej. "Seguimiento de actividades").</summary>
        public string Seccion { get; set; } = "";

        /// <summary>Título principal (ej. "Nuevo ticket de soporte").</summary>
        public string Titulo { get; set; } = "";

        /// <summary>Opcional. Badge bajo el título (ej. "Ticket creado").</summary>
        public string? Accion { get; set; }

        /// <summary>Opcional. Colores del badge de acción. Si es null usa el estilo neutro.</summary>
        public EstiloAccion? EstiloAccion { get; set; }

        /// <summary>Pares nombre → contenido, en el orden en que se muestran.</summary>
        public List<KeyValuePair<string, string?>> Cuerpo { get; set; } = new();

        /// <summary>Opcional. Se muestra en el recuadro amarillo.</summary>
        public string? Comentario { get; set; }

        /// <summary>Opcional. Texto final antes del pie (ej. "Ingresa al sistema...").</summary>
        public string? Instrucciones { get; set; }
    }

    /// <summary>Colores (hex) del badge de acción.</summary>
    public record EstiloAccion(string Fondo, string Texto, string Borde)
    {
        public static readonly EstiloAccion Neutro = new("#eef0ec", "#1f2937", "#d5d9d0");
        public static readonly EstiloAccion Verde = new("#e6f4e4", "#1e4a19", "#b7dbb2");
        public static readonly EstiloAccion Ambar = new("#fff6d6", "#7a5a00", "#fedb05");
        public static readonly EstiloAccion Rojo = new("#fde8e8", "#8a1c1c", "#f1b4b4");
    }
}
