using HD.Clientes.Modelos.SC_Analisis.Modal;

namespace HD.Clientes.Modelos.SC_Analisis.JDF
{
    public class mdlJDFAnalisis_AsignarPromotorView
    {
        public IEnumerable<mdlJDFAnalisis_Asignarpromotor>? promotor { get; set; }
        public mdlSCAnalisis_Pedido_Estado? estado { get; set; }
    }
}
