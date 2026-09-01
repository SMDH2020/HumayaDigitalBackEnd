using HD.Clientes.Modelos.SC_Analisis.JDF;

namespace HD.Clientes.Modelos.Facturar_Equipo
{
    public class mdl_datos_factura_unidad
    {
        public mdl_comentarios? datos_pedido { get; set; }
        public IEnumerable<mdl_sucursales_cliente>? sucursales { get; set; }
        public IEnumerable<mdl_documentos_facturados_EQUIP>? documentos { get; set; }
    }
}
