using System.ComponentModel.DataAnnotations;

namespace HD_Ventas.Modelos
{
    public class mdl_Categorias_Modelos
    {
        public int id_categoria {  get; set; }
        public string descripcion { get; set; }
        public int estatus { get; set; }
        public int usuario {  get; set; }
    }
}
