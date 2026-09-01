namespace HD.Clientes.Modelos.Pedido_Impresion
{
    public class mdl_pedido_impresion
    {
        public mdl_Pedido_Solicitante_View? solicitante { get; set; }
        public List<mdl_Pedido_Unidades_View>? unidades { get; set; }
        public mdl_Pedido_Condiciones_View? condiciones { get; set; }
        public List<mdl_Pedido_Financiamiento_View>? financiamiento { get; set; }
        public mdl_Pedido_Firmas_View? firmas { get; set; }
    }
}
