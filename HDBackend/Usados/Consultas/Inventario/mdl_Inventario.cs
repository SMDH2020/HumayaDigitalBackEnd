using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usados.Consultas.Usados
{
    public class mdl_Inventario
    {
        public int idinventario { get; set; }
        public string? NE { get; set; }
        public string? Marca { get; set; }
        public string? modelo { get; set; }
        public string? fecha_recepcion { get; set; }
        public int ejercicio { get; set; }
        public int HP { get; set; }
        public int sucursal { get; set; }
        public string? serie { get; set; }
        public double horas { get; set; }
        public double precio { get; set; }
        public double Costo { get; set; }
        public double OT { get; set; }
        public double utilidad { get; set; }
        public double margen { get; set; }
        public double precio_lista { get; set; }
        public string? estatus { get; set; }
        public string? promocion { get; set; }
        public string? vigencia { get; set; }
        public string? tiene_promocion { get; set; }
    }
}
