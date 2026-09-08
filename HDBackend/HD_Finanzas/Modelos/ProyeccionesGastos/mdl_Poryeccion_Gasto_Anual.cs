namespace HD_Finanzas.Modelos.ProyeccionesGastos
{
    public class mdl_Poryeccion_Gasto_Anual
    {
        public int idadr { get; set; }
        public string adr { get; set; }
        public int idsucursal { get; set; }
        public string sucursal { get; set; }
        public string sucnom { get; set; }
        public int iddepartamento { get; set; }
        public string departamento { get; set; }
        public string depnom { get; set; }
        public string cuenta { get; set; }
        public string concepto { get; set; }
        public string tipo { get; set; }
        public double enero { get; set; }
        public double febrero { get; set; }
        public double marzo { get; set; }
        public double abril { get; set; }
        public double mayo { get; set; }
        public double junio { get; set; }
        public double julio { get; set; }
        public double agosto { get; set; }
        public double septiembre { get; set; }
        public double octubre { get; set; }
        public double noviembre { get; set; }
        public double diciembre { get; set; }
    }
}
