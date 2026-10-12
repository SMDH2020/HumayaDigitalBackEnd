using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Modelos.PaqueteServicios
{
    public class mdl_Poliza_Mantenimiento_Listado
    {
        public int idpoliza { get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public int idcliente { get; set; }
        public string? cliente { get; set; }
        public string? serie { get; set; }
        public string? periodo { get; set; }
        public double num_factura { get; set; }
        public string tipo { get; set; }
        public double mano_obra { get; set; }
        public double refacciones { get; set; }
        public double km { get; set; }
        public double facturacion { get; set; }
        public int orden_trabajo { get; set; }
        public int ejercicio { get; set; }
        public int mes { get; set; }
        public int idvendedor { get; set; }
        public string? vendedor { get; set; }

    }
}
