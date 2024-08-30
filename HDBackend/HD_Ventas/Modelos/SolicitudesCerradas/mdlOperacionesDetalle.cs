namespace HD_Ventas.Modelos.SolicitudesCerradas
{
    public class mdlOperacionesDetalle
    {
        public int estatus { get; set; }
        public int idcliente { get; set; }
        public string? cliente { get; set; }
        public string? asesor { get; set; }
        public int idestado { get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public string? linea { get; set; }
        public string? modelo { get; set; }
        public string? linea_credito { get; set; }
        public string? creado { get; set; }
        public string? facturado { get; set; }
        public double importe { get; set; }
        public bool cerrado { get; set; }
        public bool credito { get; set; }
    }
}
