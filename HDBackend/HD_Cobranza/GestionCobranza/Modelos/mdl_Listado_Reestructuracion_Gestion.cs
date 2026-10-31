using System.Data.SqlTypes;

namespace HD_Cobranza.GestionCobranza.Modelos
{
    public class mdl_Listado_Reestructuracion_Gestion
    {
        public int idgestion {  get; set; }
        public int idcliente { get; set; }
        public string cliente { get; set; }
        public int createuser { get; set; }
        public string empleado { get; set; }
        public string fecha {  get; set; }
        public string comentario { get; set; }
        public string documento {  get; set; }
        public string extension { get; set; }

    }
}
