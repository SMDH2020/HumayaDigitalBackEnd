using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.GestionCobranza.Modelos
{
    public class mdl_Reporte_Proyeccion_Recuperacion_Mensual_tipo_cartera
    {

        public int idcliente { get; set; }
        public int idcliente_HD { get; set; }
        public string? cliente { get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public string? documento { get; set; }
        public string? linea_credito { get; set; }
        public string? mes { get; set; }
        public string? fecha { get; set; }
        public string? vencimiento { get; set; }
        public float importe_factura { get; set; }
        public float interes_normal { get; set; }
        public float interes_moratorio { get; set; }
        public float importe_total { get; set; }
        public float pagado { get; set; }
        public float saldo_total { get; set; }
        public string? fecha_recuperacion { get; set; }
        public string? fecha_contacto { get; set; }
        public string? tiene_convenio { get; set; }
        public string? fecha_convenio { get; set; }
        public string? objecion { get; set; }
        public string? observaciones { get; set; }
        public float saldo { get; set; }
        public int idresponsable { get; set; }
        public string? responsable { get; set; }
    }
}
