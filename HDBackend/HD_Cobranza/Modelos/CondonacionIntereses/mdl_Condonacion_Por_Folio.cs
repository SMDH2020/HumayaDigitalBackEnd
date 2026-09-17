using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Modelos.CondonacionIntereses
{
    public class mdl_Condonacion_Por_Folio
    {
        public string Folio { get; set; } = string.Empty;
        public int idcliente { get; set; }
        public string? razon_social { get; set; }
        public double Saldo { get; set; }
        public double Inormal_saldo { get; set; }
        public double Inormal_porcentaje { get; set; }
        public double Inormal_descuento { get; set; }
        public double Inormal_pagado { get; set; }
        public double Imoratorio_saldo { get; set; }
        public double Imoratorio_porcentaje { get; set; }
        public double Imoratorio_descuento { get; set; }
        public double Imoratorio_pagado { get; set; }
        public string Estatus { get; set; } = string.Empty;
        public int? idautoriza { get; set; }
        public string? autoriza { get; set; }
        public string? Comentarios { get; set; }
        public string? ComentarioAutoriza { get; set; }
        public int idcreador { get; set; }
        public string? creador { get; set; }
        public DateTime createdate { get; set; }
        public int? updateuser { get; set; }
        public DateTime? updatedate { get; set; }
    }
}
