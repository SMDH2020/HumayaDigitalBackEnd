using System;

namespace HD_RedesSociales.Modelos
{
    public class mdl_Avatar
    {
        public string? Avatar_Id { get; set; }
        public string? Voice_Id { get; set; }
        public string? Nombre { get; set; }
        public string? Keyword { get; set; }
        public string? Foto { get; set; }
        public bool Activo { get; set; } = true;

        public string? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}