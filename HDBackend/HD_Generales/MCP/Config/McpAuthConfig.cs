namespace HD.Generales.MCP.Config
{
    public class McpAuthConfig
    {
        /// <summary>
        /// URL pública del servidor MCP. Ej: "https://mi-tunnel.trycloudflare.com"
        /// Si se deja vacío, se detecta automáticamente desde los headers del request.
        /// </summary>
        public string BaseUrl { get; set; }
        public string JwtSecret { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtAudiencia { get; set; }
        public int TokenHoras { get; set; }
        public int TempTokenMins { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPuerto { get; set; }
        public string SmtpUsuario { get; set; }
        public string SmtpPassword { get; set; }
        public string EmailRemitente { get; set; }
        public string EmailDisplayName { get; set; }
        public int AuthorizationCodeMinutes { get; set; }
    }
}
