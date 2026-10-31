using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos
{
    public class MdlClientesDomicilioGuardar
    {
        public int orden { get; set; } = -1;      // -1 = nuevo
        public int idlocalidad { get; set; }
        public string direccion { get; set; } = "";
        public string tipodomicilio { get; set; } = "";  // 'C', 'T', etc.
        public bool principal { get; set; }
        public string? referencia1 { get; set; }
        public string? referencia2 { get; set; }
        public bool estatus { get; set; } = true;
        public string? ubicacion { get; set; }
    }
}
