using HD.Clientes.Modelos.Pedido_Impresion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.Pagares
{
    public class mdl_Pagare_Impresion
    {
        public List<mdl_Pedido_Financiamiento_View>? financiamientocerodias { get; set; }

        public List<mdl_Pedido_Financiamiento_View>? financiamientomasdias { get; set; }

        public mdl_pagare_firmas? firmas { get; set; }

        public mdl_Pagare_Ubicacion_View? ubicacion { get; set;}

        public mdl_Pagare_Tasa? tasa { get; set; }

    }
}
