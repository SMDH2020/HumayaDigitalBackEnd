using HD_Cobranza.Modelos.NewFolder;

namespace HD_Cobranza.Modelos.Dashboard.Dash_Indicadores
{
    public class mdl_Dashboard_View
    {
        public IEnumerable<mdl_Dashboard_Header> header { get; set; }
        public IEnumerable<mdl_Dashboard_TotalCartera> total { get; set; }
        public IEnumerable<mdl_Dashboard_RecuperacionCartera> recuperacion { get; set; }
        public IEnumerable<mdl_Dashboard_GestionCobranza> gestion { get; set; }
        public IEnumerable<mdl_Dashboard_PedidosFacturados> pedidos { get; set; }
        public IEnumerable<mdl_Dashboard_ClientesJuridico> juridico { get; set; }
        public IEnumerable<mdl_Dashboard_MensajesAutomaticos> mensajes { get; set; }
        public IEnumerable<mdl_Dashboard_ProyeccionRecuperar_View> proyeccion { get; set; }
        public IEnumerable<mdl_Dashboard_ProyeccionesRecuperar>? listado { get; set; }
        public string? tipo_cartera { get; set; }
        public string? columnas { get; set; }

    }
}
