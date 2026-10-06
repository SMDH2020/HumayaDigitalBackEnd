namespace HD.Notifications.Modelos
{
    /// <summary>
    /// Archivo adjunto de un correo enviado con NEEnviarM365.
    /// Si ContentId tiene valor, se adjunta como imagen en linea y el HTML
    /// la referencia con src="cid:{ContentId}".
    /// </summary>
    public class mdl_Correo_Adjunto
    {
        public string Nombre { get; set; } = string.Empty;
        public string ContentType { get; set; } = "application/octet-stream";
        public byte[] Contenido { get; set; } = Array.Empty<byte>();
        public string? ContentId { get; set; }
    }
}
