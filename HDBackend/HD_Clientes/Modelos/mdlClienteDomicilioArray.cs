using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos
{
    public class mdlClienteDomicilioArray
    {
        public int IdCliente { get; set; }
        public int usuario { get; set; }
        public List<MdlClientesDomicilioGuardar> Domicilios { get; set; } = new();
    }
}
