using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Modelos.Programar_Inventario
{
    public class mdl_Listado_Inventario
    {
        public string? folio { get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public string? fecha_inicio { get; set; }
        public string? fecha_fin { get; set; }
        public int anio { get; set; }
        public int mes { get; set; }
        public int dia { get; set; }
        public int id_auditor_ppal { get; set; }
        public string? auditor_ppal { get; set; }
        public bool cargado_inventario { get; set; }
        public bool cargado_transito { get; set; }
        public bool cargado_surtida { get; set; }
        public string? estatus { get; set; }

        public double diferencias { get; set; }
        public double confiabilidad { get; set; }
        public double avance { get; set; }

    }
}
