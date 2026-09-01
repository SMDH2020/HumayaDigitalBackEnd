using System.ComponentModel.DataAnnotations;

namespace HD.Clientes.Modelos.CRM.ObjetivosSemanales
{
    /// <summary>
    /// Renglon del detalle que captura el front. Se serializa a JSON en el AD para el SP.
    /// </summary>
    public class mdl_ObjetivosSemanales_Detalle
    {
        [Range(1, int.MaxValue, ErrorMessage = "La Semana es un valor requerido")]
        public int idsemana { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El Vendedor es un valor requerido")]
        public int idvendedor { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La Linea es un valor requerido")]
        public int idlinea { get; set; }

        [Range(typeof(decimal), "0", "9999999", ErrorMessage = "El Objetivo debe ser un valor mayor o igual a cero")]
        public decimal objetivo { get; set; } = 0;
    }
}
