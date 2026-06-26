using Microsoft.AspNetCore.Http;

namespace HD_RedesSociales.Modelos
{
    public class mdl_Encabezado
    {
        public string? Folio { get; set; }
        public string? Linea { get; set; }
        public string? Firma { get; set; }   // NUEVO
        //public string? Imagen { get; set; }
        public IFormFile? Archivo { get; set; }
        public string? ImagenBase64 { get; set; }
        public string? Tipo_Publicacion { get; set; }
        public decimal? Precio_Lista { get; set; }
        public decimal? Precio_Especial { get; set; }
        public string? Beneficios { get; set; }
        public string? Vigencias { get; set; }
        public string? Restricciones { get; set; }
        public string? Escenografia { get; set; }
        public TimeSpan? Hora { get; set; }        
        public string? Red_Social { get; set; }

        public string? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}