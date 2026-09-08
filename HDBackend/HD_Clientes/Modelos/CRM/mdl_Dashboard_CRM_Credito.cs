using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM
{
    public class mdl_Dashboard_CRM_Credito
    {
        public string? tipo { get; set; }
        public int idfactura { get; set; }
        public string? documento { get; set; }
        public int idsucursal { get; set; }
        public int idcliente_HD { get; set; }
        public int idcliente { get; set; }
        public string? descripcion { get; set; }
        public string? serie_fiscal { get; set; }
        public string? folio_fiscal { get; set; }
        public string? fecha { get; set; }
        public string? vencimiento { get; set; }
        public double importefactura { get; set; }
        public double importepagado { get; set; }
        public double saldo { get; set; }
        public string? pagare { get; set; }
        public double interes_pactado { get; set; }
        public float interes_moratorio { get; set; }
        public double saldo_total { get; set; }
        public int diasvencido { get; set; }
        public float tasa { get; set; }
        public float tasa_moratoria { get; set; }
        public string folio_solicitud { get; set; }
    }
}
