namespace HD.Notifications.Modelos
{
    /// <summary>
    /// Credenciales de la aplicacion registrada en Microsoft Entra ID que se usa
    /// para enviar correo por Microsoft Graph. Se llenan desde appsettings.json,
    /// seccion CorreoM365.
    /// </summary>
    public class mdl_Correo_M365
    {
        /// <summary>Directory (tenant) ID del registro de aplicacion.</summary>
        public string? TenantId { get; set; } = "";

        /// <summary>Application (client) ID del registro de aplicacion.</summary>
        public string? ClientId { get; set; } = "";

        /// <summary>Valor del secreto de cliente (no el Secret ID).</summary>
        public string? ClientSecret { get; set; } = "";

        /// <summary>Buzon desde el que se envia, en formato correo@dominio.</summary>
        public string? Remitente { get; set; } = "";

        /// <summary>Nombre visible del remitente. Opcional.</summary>
        public string? NombreRemitente { get; set; } = "";

        /// <summary>Guardar una copia en Elementos enviados del buzon remitente.</summary>
        public bool GuardarEnEnviados { get; set; } = false;
    }
}
