using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Modelos.Conteo_Piezas
{
    public class mdl_Editar_Nombre_Posicion_Extra
    {
        public int id_inv_fisico { get; set; }
        public string folio { get; set; }
        public string posicion_extra { get; set; }
        public string posicion_edit { get; set; }
        public int id_auditor { get; set; } = 0;
    }
}
