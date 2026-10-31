using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace HD_CentroMonitoreo.Modelos.Dashboard
{
    /// <summary>
    /// Parametros (query string) de GET CentroMonitoreo/Dashboard.
    /// El rango es inclusivo: del periodoinicio de ejercicioinicio al periodofin de ejerciciofin.
    /// sucursal y adr son ids separados por coma; 0 = todas.
    /// El usuario NO viaja en el request: el controller lo toma de la sesion y el SP
    /// filtra por las sucursales que ese usuario tiene permitidas.
    /// </summary>
    public class mdl_Dashboard_Filtro : IValidatableObject
    {
        private static readonly Regex ListaIds = new Regex(@"^\s*\d{1,9}(\s*,\s*\d{1,9})*\s*$", RegexOptions.Compiled);

        [Range(2000, 2999, ErrorMessage = "El ejercicio de inicio es un valor requerido (año de 4 dígitos)")]
        public int ejercicioinicio { get; set; }

        [Range(1, 12, ErrorMessage = "El periodo de inicio debe estar entre 1 y 12")]
        public int periodoinicio { get; set; }

        [Range(2000, 2999, ErrorMessage = "El ejercicio final es un valor requerido (año de 4 dígitos)")]
        public int ejerciciofin { get; set; }

        [Range(1, 12, ErrorMessage = "El periodo final debe estar entre 1 y 12")]
        public int periodofin { get; set; }

        /// <summary>Ids de sucursal separados por coma. 0 = todas.</summary>
        public string? sucursal { get; set; } = "0";

        /// <summary>Ids de region separados por coma. 0 = todas. Solo aplica cuando sucursal es 0.</summary>
        public string? adr { get; set; } = "0";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ejercicioinicio >= 2000 && ejerciciofin >= 2000 &&
                periodoinicio >= 1 && periodoinicio <= 12 && periodofin >= 1 && periodofin <= 12)
            {
                int inicio = ejercicioinicio * 12 + periodoinicio;
                int fin = ejerciciofin * 12 + periodofin;

                if (inicio > fin)
                {
                    yield return new ValidationResult(
                        "El periodo de inicio no puede ser posterior al periodo final",
                        new[] { nameof(periodoinicio), nameof(periodofin) });
                }
            }

            if (!string.IsNullOrWhiteSpace(sucursal) && !ListaIds.IsMatch(sucursal))
            {
                yield return new ValidationResult(
                    "Las sucursales deben ser ids numéricos separados por coma, o 0 para todas",
                    new[] { nameof(sucursal) });
            }

            if (!string.IsNullOrWhiteSpace(adr) && !ListaIds.IsMatch(adr))
            {
                yield return new ValidationResult(
                    "Las regiones deben ser ids numéricos separados por coma, o 0 para todas",
                    new[] { nameof(adr) });
            }
        }

        /// <summary>Lista normalizada de sucursales ("0" = todas, si viene vacia o incluye el 0).</summary>
        public string SucursalNormalizada() => Normalizar(sucursal);

        /// <summary>
        /// Lista normalizada de regiones. Solo se respeta cuando no hay sucursales
        /// especificas; si viene sucursal distinta de 0, la region se ignora ("0").
        /// </summary>
        public string AdrNormalizada() => SucursalNormalizada() == "0" ? Normalizar(adr) : "0";

        private static string Normalizar(string? lista)
        {
            if (string.IsNullOrWhiteSpace(lista)) return "0";

            var ids = lista.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                           .Select(x => int.Parse(x))
                           .Distinct()
                           .ToList();

            if (ids.Count == 0 || ids.Contains(0)) return "0";
            return string.Join(",", ids);
        }
    }
}
