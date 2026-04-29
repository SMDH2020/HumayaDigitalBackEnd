using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Modelos.EmbudoVentas
{
    public class mdl_Embudo_Ventas
    {
        public mdl_Embudo_Ventas_Permisos? permiso { get; set; }
        public List<mdl_Embudo_Ventas_Data>? data { get; set; }
        public List<mdl_Embudo_Ventas_Regiones>? regiones { get; set; }
        public List<mdl_Embudo_Ventas_Sucursales>? sucursales { get; set; }
        public List<mdl_Embudo_Ventas_Lineas>? lineas { get; set; }
        public List<mdl_Embudo_Ventas_Departamentos>? departamentos { get; set; }
        public List<mdl_Embudo_Ventas_Asesores>? asesores { get; set; }
    }
}
