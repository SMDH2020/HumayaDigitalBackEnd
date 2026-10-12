namespace HD_Cobranza.GestionCobranza.Modelos
{
    public class mdl_Listado_Clientes_Gestionar
    {
        public int idcliente {  get; set; }
        public string? RazonSocial {  get; set; }
        public int idSucursal {  get; set; }
        public string? sucursal {  get; set; }
        public int IDEstado { get; set; }
        public string? estado { get; set; }
        public string? registro { get; set; }
        public string? telefono { get; set; }
        public string? correo { get; set; }
        public int idResponsableCobranza { get; set; }
        public string? ResponsableCobranza {  get; set; }
        public float SaldoVencido { get; set; }
        public float SaldoporVencer {  get; set; }
        public string? linea {  get; set; }
        public string? gestion {  get; set; }
    }
}
