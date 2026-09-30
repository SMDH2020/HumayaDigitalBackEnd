using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Modelos.Programar_Inventario
{
    public class mdl_JustificacionInventario_Listado
    {
        public string? folio { get; set; }
        public int id_conteo { get; set; }
        public int id_just { get; set; }
        public string? modelo { get; set; }
        public string? descripcion { get; set; }
        public string? categoria { get; set; }
        public string? ubicacion { get; set; }
        public double cant_sistema { get; set; }
        public double cant_fisica { get; set; }
        public double diferencia { get; set; }
        public string? estatus_just { get; set; }
        public string? comentario_usuario { get; set; }
        public string? motivo_rechazo { get; set; }
        public double aceptadas { get; set; }
    }
}
