using ClosedXML.Excel;
using System.Globalization;

namespace HD_Reporteria.CRM
{
    /// <summary>
    /// Estilos y celdas compartidas por los reportes de Indicadores del CRM
    /// (Visitas y Cotizaciones): semaforo de cumplimiento, filas banda y nombres de mes.
    /// </summary>
    public class XLS_IndicadoresEstilos
    {
        public const string VerdeFondo = "#EAF3DE";
        public const string VerdeTexto = "#3B6D11";
        public const string AmbarFondo = "#FDF3E3";
        public const string AmbarTexto = "#A76B0B";
        public const string RojoFondo = "#FDECEB";
        public const string RojoTexto = "#C0392B";
        public const string GrisTexto = "#9E9E9E";

        public const string EncabezadoTabla = "#EBECEE";
        public const string EncabezadoSemana = "#E9AE06";
        public const string EncabezadoLineaA = "#275027";
        public const string EncabezadoLineaB = "#3C3C3C";
        public const string BandaEstado = "#DDE4D5";
        public const string BandaSucursal = "#F1F3EE";

        /// <summary>
        /// Pinta el semaforo de cumplimiento en la celda.
        /// porcentaje null = sin objetivo capturado: se escribe N/A en gris, sin relleno.
        /// </summary>
        public static void AplicarSemaforo(IXLCell celda, decimal? porcentaje)
        {
            celda.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            if (porcentaje == null)
            {
                celda.Value = "N/A";
                celda.Style.Font.FontColor = XLColor.FromHtml(GrisTexto);
                return;
            }

            decimal valor = porcentaje.Value;
            celda.Value = valor;
            celda.Style.NumberFormat.Format = "0\"%\"";

            if (valor >= 100)
            {
                celda.Style.Fill.BackgroundColor = XLColor.FromHtml(VerdeFondo);
                celda.Style.Font.FontColor = XLColor.FromHtml(VerdeTexto);
            }
            else if (valor > 80)
            {
                celda.Style.Fill.BackgroundColor = XLColor.FromHtml(AmbarFondo);
                celda.Style.Font.FontColor = XLColor.FromHtml(AmbarTexto);
            }
            else
            {
                celda.Style.Fill.BackgroundColor = XLColor.FromHtml(RojoFondo);
                celda.Style.Font.FontColor = XLColor.FromHtml(RojoTexto);
            }
        }

        /// <summary>
        /// Escribe una fila banda (Estado o Sucursal) combinada a todo lo ancho de la tabla.
        /// Devuelve el siguiente renglon disponible.
        /// </summary>
        public static int EscribirBanda(IXLWorksheet sheet, int renglon, int totalColumnas, string texto, string fondo, double tamanio, int sangria)
        {
            var banda = sheet.Range(renglon, 1, renglon, totalColumnas);
            banda.Merge();
            banda.Value = texto;
            banda.Style.Fill.BackgroundColor = XLColor.FromHtml(fondo);
            banda.Style.Font.Bold = true;
            banda.Style.Font.FontSize = tamanio;
            banda.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            banda.Style.Alignment.Indent = sangria;
            sheet.Row(renglon).Height = 18;
            return renglon + 1;
        }

        /// <summary>
        /// Nombre del mes en espanol con la primera letra en mayuscula.
        /// </summary>
        public static string NombreMes(int mes, CultureInfo ci)
        {
            if (mes < 1 || mes > 12) return "";
            string nombre = ci.DateTimeFormat.GetMonthName(mes);
            return char.ToUpper(nombre[0]) + nombre.Substring(1);
        }

        /// <summary>
        /// Etiqueta de encabezado de semana: SEMANA n + rango de fechas.
        /// </summary>
        public static string EtiquetaSemana(int numero, DateTime inicio, DateTime fin, CultureInfo ci)
        {
            return "SEMANA " + numero + Environment.NewLine +
                   inicio.ToString("dd", ci) + " - " + fin.ToString("dd", ci) + " de " + NombreMes(fin.Month, ci);
        }
    }
}
