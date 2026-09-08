using HD.Clientes.Modelos.SC_Analisis.Modal;

namespace HD.Clientes.Modelos.Facturar_Equipo
{
    public class mdl_datos_view
    {
        public mdl_datos_pedido? datos_pedido { get; set; }
        public mdlSCAnalisis_Pedido_Estado? informacion { get; set; }
        //public mdl_comentarios? comentarios { get; set; }
        //public IEnumerable<mdl_sucursales_cliente>? sucursales { get; set; }
        public IEnumerable<mdlFacturaUnidadesSolicitadas>? unidades { get; set; }
        public IEnumerable<mdlPEdidoFinanciamiento>? financiamiento { get; set; }
    }
}
