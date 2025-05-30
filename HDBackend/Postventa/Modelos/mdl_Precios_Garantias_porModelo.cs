
namespace Postventa.Modelos
{
    public class mdl_Precios_Garantias_porModelo
    {
        public int idprecio {  get; set; }
        public string modelo {  get; set; }
        public float precio_original {  get; set; }
        public float precio_ajustado { get; set; }
        public string? inicio_vigencia { get; set; }
        public string? vigencia { get; set; }
        public int estatus { get; set; }
    }
}
