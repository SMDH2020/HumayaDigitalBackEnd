using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarteraTransito.Modelos
{
    public class mdl_Cartera_Transito
    {
        public int id { get; set; }
        public int ejercicio { get; set; }
        public int periodo { get; set; }
        public int documento { get; set; }
        public string? modulo { get; set; }
        public string? origen { get; set; }
        public int idcliente { get; set; }
        public string? cliente { get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public double saldo { get; set; }
        public string? serie { get; set; }
        public string? folio { get; set; }
        public string? fecha { get; set; }
        public string? vencimiento { get; set; }
        public bool estatus { get; set; }

    }
}
