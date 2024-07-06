namespace HD_Ventas.Modelos.SolicitudesCerradas
{
    public class mdlSolicitudesFacturadasResult
    {
        public mdlSolicitudesFacturada_Resumen resumen { get; set; }
        public IEnumerable<mdlSolicitudesFacturadas_Sucursal> sucursal { get; set; }
    }
}
