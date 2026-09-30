namespace Postventa.Modelos
{
    public class mdl_Precios_Mantenimiento_porModelo
    { 
        public int id_precio {  get; set; }
        public string modelo { get; set; }
        public int id_paquete { get; set; }
        public float precio { get; set; }
    }
}
