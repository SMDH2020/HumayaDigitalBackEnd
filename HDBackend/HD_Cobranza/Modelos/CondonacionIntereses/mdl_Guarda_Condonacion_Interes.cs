using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Modelos.CondonacionIntereses
{
    public class mdl_Guarda_Condonacion_Interes
    {
        public int idcliente { get; set; }
        public int usuario { get; set; }
        public double Saldo { get; set; }
        public double Inormal_saldo { get; set; }
        public double Inormal_porcentaje { get; set; }
        public double Inormal_descuento { get; set; }
        public double Inormal_pagado { get; set; }
        public double Imoratorio_saldo { get; set; }
        public double Imoratorio_porcentaje { get; set; }
        public double Imoratorio_descuento { get; set; }
        public double Imoratorio_pagado { get; set; }
        public string? Comentarios { get; set; }
        public string facturas { get; set; }
        public string? Folio { get; set; }
        public string? Estatus { get; set; }
        public int? Autoriza { get; set; }
    }
}
