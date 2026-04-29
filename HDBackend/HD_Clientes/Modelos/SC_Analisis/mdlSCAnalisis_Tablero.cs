namespace HD.Clientes.Modelos.SC_Analisis
{
    public class mdlSCAnalisis_Tablero
    {
        public string? folio { get; set; }
        public int idvendedor { get; set; }
        public string? vendedor { get; set; }
        public string? fecha { get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public string? razon_social { get; set; }
        public string? tipo_credito { get; set; }
        public string? linea_credito { get; set; }
        public string? estado { get; set; }
        public bool aprovo_gerencia { get; set; }
        public bool aprovo_cedito { get; set; }
        public double importe { get; set; }
    }
}
