using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Modelos.Juridico
{
    public class mdl_Clientes_Juridico
    {
        public int idsucursal { get; set; }
        public int idcliente { get; set; }
        public string? razon_social { get; set; }
        public string? vencimiento { get; set; }
        public int dias_vencidos { get; set; }
        public double saldo_vencido { get; set; }
        public double saldo_porvencer { get; set; }
        public double saldo_total { get; set; }
        public double recuperado { get; set; }
        public string? fecha_recuperacion { get; set; }
        public string? estatus { get; set; }
        public string? tiene_comentario { get; set; }

    }
}
