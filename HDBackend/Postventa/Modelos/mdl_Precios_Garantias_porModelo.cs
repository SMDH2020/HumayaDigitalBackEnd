
namespace Postventa.Modelos
{
    public class mdl_Precios_Garantias_porModelo
    {
        public int idprecio { get; set; }
        public string modelo { get; set; }
        public float venta_temprana { get; set; }
        public float venta_tardia { get; set; }
        public float venta_fin_garantia { get; set; }
        public string? fecha_inicio { get; set; }
        public string? fecha_fin { get; set; }
    }
}
