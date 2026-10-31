using HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados;

namespace HD.Clientes.Modelos.SC_Analisis.Modal
{
    public class mdlSCAnalisis_Documentacion_View
    {
        public IEnumerable<mdlSCAnalisis_Documentacion>? documentacion { get; set; }
        public mdlSCAnalisis_Pedido_Estado? estado { get; set; }
        public IEnumerable<mdl_Analisis_100_detalle> detalle{ get; set; }
    }
}
