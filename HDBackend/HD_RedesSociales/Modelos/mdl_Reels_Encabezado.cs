using System;

namespace HD_RedesSociales.Modelos
{
    public class mdl_Reels_Encabezado
    {
        public string? Folio { get; set; }
        public string? Modo { get; set; }
        public string? Tema { get; set; }
        public string? Informacion_Empresa { get; set; }
        public string? Avatar_Id { get; set; }
        public string? Aspect_Ratio { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public TimeSpan? Hora { get; set; }
        public string? Red_Social { get; set; }
        public string? Video_Url { get; set; }
        public bool Cargar { get; set; } = true;

        public string? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}