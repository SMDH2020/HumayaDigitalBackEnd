using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Mensajeria.Modelos
{
    public class mdl_Mensajeria_Indicadores_Detalle
    {
        public string Telefono { get; set; }
        public string Cliente { get; set; }
        public string Linea { get; set; }
        public string Seccion { get; set; }
        public int idresponsable { get; set; }
        public string Responsable { get; set; }
        public int IDSucursal { get; set; }
        public string Sucursal { get; set; }
        public int Recibidos { get; set; }
        public int Enviados { get; set; }
        public int pct_atencion { get; set; }
    }
}
