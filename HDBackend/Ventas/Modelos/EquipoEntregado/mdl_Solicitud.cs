using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas.Modelos.EquipoEntregado
{
    public class mdl_Solicitud
    {
        public string folio { get; set; }
        public int idcliente { get; set; }
        public string razon_social { get; set; }
        public DateTime? fecha_pagare { get; set; }
        public double importe { get; set; }
    }
}
