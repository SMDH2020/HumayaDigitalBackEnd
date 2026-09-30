using System.ComponentModel.DataAnnotations;

namespace Postventa.Modelos.OrdenesAbiertas
{
    public class mdl_OrdenTrabajoAbiertaSucursal_Detalle
    {
        public int v_ot { get; set; }
        public string cliente { get; set; }
        public int v_antiguedad { get; set; }
        public decimal v_manobra { get; set; }
        public decimal v_refacciones { get; set; }
        public decimal v_trabajoforaneo { get; set; }
        public decimal v_Kilometraje { get; set; }
        public string v_nip { get; set; }
        public string v_descfalla { get; set; }
        public string v_tipo { get; set; }
    }
}