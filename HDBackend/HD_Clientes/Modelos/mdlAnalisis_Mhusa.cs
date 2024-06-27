using HD.Clientes.Modelos.SC_Analisis.Modal;

namespace HD.Clientes.Modelos
{
    public class mdlAnalisis_Mhusa
    {
        public mdldatos_notificacion? mdldatos { get; set; }
        public mdlSCAnalisis_Pedido_Estado? estado { get; set; }
        public IEnumerable<mdlSolicitudCredito_Enviar>? mdlSolicitud { get; set; }
        public IEnumerable<mdlSCAnalisis_Documentacion>? documentacion { get; set; }
    }

    public class mdldatos_notificacion
    {
        public string? folio { get; set; }
        public string? asunto { get; set; }
        public string? cliente { get; set; }
        public string? asesor { get; set; }
        public string? comentarios { get; set; }
    }
}
