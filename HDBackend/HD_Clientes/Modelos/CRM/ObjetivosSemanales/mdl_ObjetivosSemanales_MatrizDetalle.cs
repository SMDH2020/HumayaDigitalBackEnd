using System.ComponentModel.DataAnnotations;

namespace HD.Clientes.Modelos.CRM.ObjetivosSemanales
{
    /// <summary>
    /// Renglon de la matriz que captura el front. Se serializa a JSON en el AD:
    /// el SP lo desarma con OPENJSON WITH (idlinea INT, objetivo DECIMAL(18,2)).
    /// </summary>
    public class mdl_ObjetivosSemanales_MatrizDetalle
    {
        [Range(1, int.MaxValue, ErrorMessage = "La Linea es un valor requerido")]
        public int idlinea { get; set; }

        [Range(typeof(decimal), "0", "9999999", ErrorMessage = "El Objetivo debe ser un valor mayor o igual a cero")]
        public decimal objetivo { get; set; } = 0;
    }
}
