
namespace HD_Ventas.Modelos
{
    public class mdl_Editar_Modelo
    {
        public int idmodelo {  get; set; }
        public int idlinea { get; set; }
        public string modelo { get; set; }
        public string descripcion_mdl { get; set; }
        public float precio_lista { get; set; }
        public int usuario { get; set; }
        public string caracteristicas { get; set; }
        public string imagenes { get; set; }
    }
}
