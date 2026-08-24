namespace Postventa.Modelos.OrdenesAbiertas
{
    public class mdl_DashboardOTAbiertas_Resumen
    {
        public int Ordenes_Abiertas { get; set; }
        public int green { get; set; }
        public int yellow { get; set; }
        public int red { get; set; }
        public int max_antiguedad { get; set; }
    }

    public class mdl_DashboardOTAbiertas_Detalle
    {
        public int idestado { get; set; }
        public int idsucursal { get; set; }
        public string sucursal { get; set; }
        public int ordenes_Abiertas { get; set; }
        public int green { get; set; }
        public int yellow { get; set; }
        public int red { get; set; }
        public int max_antiguedad { get; set; }
        public decimal mano_obra { get; set; }
        public decimal refacciones { get; set; }
        public decimal trabajos_foraneos { get; set; }
        public decimal kilometraje { get; set; }
        public decimal total { get; set; }
    }
    public class mdl_DashboardOTAbiertas_DetalleTipo
    {
        public int idsucursal { get; set; }
        public string v_tipo { get; set; }
        public decimal mano_obra { get; set; }
        public decimal refacciones { get; set; }
        public decimal trabajos_foraneos { get; set; }
        public decimal kilometraje { get; set; }
        public decimal total { get; set; }
    }
}