namespace HD_Cobranza.Modelos
{
    public class mdlPedidos_Facturados
    {   
        public string? operacion {  get; set; }
        public string? folio {  get; set; }
        public string? asesor_ventas { get; set; }
        public string? responsable_cobranza { get; set; }
        public int idcliente { get; set; }
        public string? razon_social { get; set; }
        public string? idequip {  get; set; }
        public int idsucursal { get; set; }
        public string? sucursal {  get; set; }
        public string? serie_fiscal { get; set; }
        public string? folio_fiscal { get; set; }
        public string? documento_factura { get; set; }
        public string? documento_pagare { get; set; }
        public float tasa_anual {  get; set; }
        public int dias_financiamiento { get; set; }
        public DateTime? vencimiento { get; set; }
        public string? celular {  get; set; }
        public float importe_financiar { get; set; }
        public float interes_pactado { get; set; }
        public float total_pagar { get; set; }
    }
}
