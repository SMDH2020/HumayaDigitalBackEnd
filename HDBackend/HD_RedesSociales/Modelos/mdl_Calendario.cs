    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace HD_RedesSociales.Modelos
    {
        public class mdl_Calendario
        {
            public string? Folio { get; set; }
            public string? Linea { get; set; }
            public string? Firma { get; set; }   
            public string? Tipo_Publicacion { get; set; }
            //public string? Imagen { get; set; }
            public decimal Precio_Lista { get; set; }
            public decimal? Precio_Especial { get; set; }
            public string? Beneficios { get; set; }
            public string? Vigencias { get; set; }
            public string? Restricciones { get; set; }
            public string? Escenografia { get; set; }
            public TimeSpan? Hora { get; set; }         
            public string? Red_Social { get; set; }
            public int? Consecutivo { get; set; }
            public DateTime? Fecha_Envio { get; set; }
            public string? Estatus { get; set; }
            public string? ImagenBase64 { get; set; }
            public bool Cargar { get; set; } = true;
        }   
    }