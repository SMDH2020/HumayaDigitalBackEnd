using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos
{
    public class mdlClientes_Datos_Persona_Fisica
    {
        public int idcliente { get; set; }

        public string nombre { get; set; }

        public string apellido_paterno { get; set; }

        public string apellido_materno { get; set; }

        public string curp { get; set; }

        public string sexo { get; set; }

        public string estado_civil { get; set; }

        public string regimen_conyugal { get; set; }

        public bool estatus { get; set; }

        public string? usuario { get; set; } = "";
    }
}
