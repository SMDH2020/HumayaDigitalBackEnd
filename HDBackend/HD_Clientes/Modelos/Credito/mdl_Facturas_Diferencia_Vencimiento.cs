namespace HD.Clientes.Modelos.Credito
{
    public class mdl_Facturas_Diferencia_Vencimiento
    {
        public string? Folio { get; set; }
        public string? razon_social { get; set; }
        public string? folio_fiscal { get; set; }
        public int documento {  get; set; }
        public float importefinanciar { get; set; }
        public float interes { get; set; }
        public string? vencimiento_HD { get; set; }
        public string? vencimiento_EQUIP {  get; set; }

    }
}
