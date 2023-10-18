using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos
{
    public class mdlSolicitud_Credito_Cultivos
    { 
        public int idsolicitud_cultivo {  get; set; }
        public string folio { get; set; }

        public int idcultivo { get; set; }

        public double hectareas { get; set; }

        public string ciclo { get; set; }

        public string tipo_riego { get; set; }

        public string temporal { get; set; }

        public double rendimiento { get; set; }

        public double precio { get; set; }

        public int mes_cosecha { get; set; }

        public bool estatus { get; set; }

        public int total_toneladas { get; set; }

        public string? usuario { get; set; } = "";
    }
}
