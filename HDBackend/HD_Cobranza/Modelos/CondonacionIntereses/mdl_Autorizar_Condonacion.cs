using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Modelos.CondonacionIntereses
{
    public class mdl_Autorizar_Condonacion
    {
        public string Folio { get; set; }
        public int usuario { get; set; }               // quien aprueba/rechaza (se llena desde la sesión)
        public bool aprobado { get; set; }
        public double? Inormal_porcentaje { get; set; }
        public double? Imoratorio_porcentaje { get; set; }
        public string? ComentarioAutoriza { get; set; }

        // Se llenan con el SELECT final del stored
        public string? Estatus { get; set; }
        public int? Autoriza { get; set; }
        public double? Inormal_descuento { get; set; }
        public double? Imoratorio_descuento { get; set; }
        public int? updateuser { get; set; }
        public DateTime? updatedate { get; set; }
        public int? idcliente { get; set; }
        public string? razon_social { get; set; }
        public int? idcreador { get; set; }
        public string? correo_creador { get; set; }
    }
}
