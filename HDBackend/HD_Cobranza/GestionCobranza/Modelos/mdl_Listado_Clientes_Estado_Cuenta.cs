namespace HD_Cobranza.GestionCobranza.Modelos
{
    public class mdl_Listado_Clientes_Estado_Cuenta
    {
        public int idcliente { get; set; }
        public string? razonsocial { get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public int idestado { get; set; }
        public string? estado { get; set; }
        public string? registro { get; set; }
        public int idResponsableCobranza { get; set; }
        public string? ResponsableCobranza { get; set; }
        public float SaldoVencido { get; set; }
        public float SaldoporVencer { get; set; }
        public string? linea { get; set; }
    }
}
