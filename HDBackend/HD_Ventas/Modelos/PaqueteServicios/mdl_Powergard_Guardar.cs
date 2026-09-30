using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Modelos.PaqueteServicios
{
    public class mdl_Powergard_Guardar
    {
        public int idpowergard { get; set; }
        public int idsucursal { get; set; }
        public int idcliente { get; set; }
        public string? serie { get; set; }
        public double facturacion { get; set; }
        public double costo { get; set; }
        public string tipo { get; set; }
        public string? fecha_facturacion { get; set; }
        public int num_ot { get; set; }
        public int idvendedor { get; set; }
        public string? cobertura { get; set; }
        public string? contrato { get; set; }
        public string? usuario { get; set; }
    }
}
