using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos
{
    public class mdlSolicitud_Credito_Siniestros
    {
        public string folio { get; set; }

        public short registro { get; set; }

        public string siniestro { get; set; }

        public double ptotal { get; set; }

        public double pparcial { get; set; }

        public string ciclo { get; set; }

        public bool indemnizacion { get; set; }

        public double monto { get; set; }

        public bool estatus { get; set; }

        public string? usuario { get; set; } = "";
    }
}
