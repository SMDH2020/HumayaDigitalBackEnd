using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.RotacionInventario
{
    public class mdl_RotacionCXC_Detalle
    {
        public int idcliente { get; set; }
        public string? cliente { get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public int iddepartamento { get; set; }
        public string? departamento { get; set; }
        public double importe { get; set; }
        public double iva { get; set; }
        public double abonos { get; set; }
        public double saldo { get; set; }
        public string? serie { get; set; }
        public string? folio { get; set; }
        public int documento_interno { get; set; }
        public int documento_factura { get; set; }
        public string? fecha_factura { get; set; }

        public int dias_vencido { get; set; }
        public int batch { get; set; }


    }
}
