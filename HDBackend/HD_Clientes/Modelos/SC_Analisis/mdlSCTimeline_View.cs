namespace HD.Clientes.Modelos.SC_Analisis
{
    public class mdlSCTimeline_View
    {
        public mdlSCTimeline_estado? estado { get; set; }
        public IEnumerable<mdlSCTimeline_detalle>? detalle { get; set; }
    }
}
