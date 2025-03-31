
namespace HD_Finanzas.Modelos.NivelInventario
{
    public class mdl_Nivel_Inventario
    {
        public short idadr { get; set; }
        public string adr { get; set; }
        public short idsucursal { get; set; }
        public string sucursal { get; set; }
        public string concepto { get; set; }
        public double invactual { get; set; }
        public double invanterior { get; set; }
        public int dias { get; set; }
        public int diasant { get; set; }
    }
}
