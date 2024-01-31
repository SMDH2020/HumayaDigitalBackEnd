using ClosedXML.Excel;

namespace HD_Cobranza.Reportes
{
    public class XLSEncabezado
    {
        public static int Encabezado(ref IXLWorksheet sheet, string Titulo, int Columnas)
        {
            try
            {
                var rango = sheet.Range(1, 1, 1, @Columnas);
                rango.Value = @Titulo;
                rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                rango.Style.Font.Bold = true;
                rango.Style.Font.FontSize = 14;
                rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                rango.Merge();

                sheet.Row(1).Height = 50;
                sheet.Row(2).Height = 8;
                sheet.Row(3).Height = 4;

                rango = sheet.Range(2, 1, 2, @Columnas);
                rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#275027");

                rango = sheet.Range(3, 1, 3, @Columnas);
                rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#E9AE06");
                return 4;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
