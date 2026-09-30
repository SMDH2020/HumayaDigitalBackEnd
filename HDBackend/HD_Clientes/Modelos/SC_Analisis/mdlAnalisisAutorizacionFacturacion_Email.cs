using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.SC_Analisis
{
    public class mdlAnalisisAutorizacionFacturacion_Email
    {
        public string? vendedor { get; set; }
        public string? asunto { get; set; }
        public string? correo_vendedor { get; set; }
        public string? gerente_sucursal { get; set; }
        public string? correo_gerente_sucursal { get; set; }
        public string? responsable_credito { get; set; }
        public string? correo_responsable_credito { get; set; }
        public string? nombre_cajera { get; set; }
        public string? correo_cajera { get; set; }
        public string? proceso { get; set; }
        public string? comentarios { get; set; }
        public string? estatus { get; set; }
    }
}
