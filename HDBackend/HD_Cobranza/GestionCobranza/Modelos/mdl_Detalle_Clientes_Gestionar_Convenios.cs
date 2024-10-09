namespace HD_Cobranza.GestionCobranza.Modelos
{
    public class mdl_Detalle_Clientes_Gestionar_Convenios
    {
        public string? folio { get; set; }
        public string? razon_social { get; set; }
        public double saldo { get; set; }
        public double monto { get; set; }

        public DateTime fecha_convenio { get; set; }

        public string? mediocontacto { get; set; }

        public int createuser { get; set; }

        public string? NombreCompleto { get; set; }
        public int ADR { get; set; }
        public int IDSucursal {  get; set; }
        public string? sucursal {  get; set; }
    }
}
