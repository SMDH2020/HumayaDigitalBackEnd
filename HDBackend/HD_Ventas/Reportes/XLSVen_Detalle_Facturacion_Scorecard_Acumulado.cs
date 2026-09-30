using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Ventas.Reportes;
using HD_Ventas.Modelos;
using DocumentFormat.OpenXml.Bibliography;

namespace HD_Ventas.Reportes
{
    public class XLSVen_Detalle_Facturacion_Scorecard_Acumulado
    {
        public static string obtenernombre_mes(int numeromes)
        {
            switch (numeromes)
            {
                case 1:
                    return "ENERO";
                case 2:
                    return "FEBRERO";
                case 3:
                    return "MARZO";
                case 4:
                    return "ABRIL";
                case 5:
                    return "MAYO";
                case 6:
                    return "JUNIO";
                case 7:
                    return "JULIO";
                case 8:
                    return "AGOSTO";
                case 9:
                    return "SEPTIEMBRE";
                case 10:
                    return "OCTUBRE";
                case 11:
                    return "NOVIEMBRE";
                case 12:
                    return "DICIEMBRE";
                default:
                    return "";

            }
        }
        public static Task<DocResult> GenerarExcel(IEnumerable<mdlDetalle_Facturacion_Scorecard> detalle, int ejercicioinicio, int periodoinicio, int ejerciciofin, int periodofin)
        {
            try
            {
                string sheetname = "FACTURACION DE MAQUINARIA";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, $"FACTURACION DE MAQUINARIA ACUMULADO {obtenernombre_mes(periodoinicio) + " " + ejercicioinicio + " - " + obtenernombre_mes(periodofin) + " " + ejerciciofin}", 8);

                    //renglon += 1;

                    //sheet.Range(renglon, 1, renglon, 13).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");

                    //sheet.Range(renglon, 2, renglon, 4).Merge().Value = obtenernombre_mes(mes_actual);
                    //sheet.Range(renglon, 2, renglon, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    //sheet.Range(renglon, 2, renglon, 4).Style.Font.Bold = true;
                    //sheet.Range(renglon, 2, renglon, 4).Style.Fill.BackgroundColor = XLColor.LightGray;
                    //int rengloncarteratot = renglon;

                    //sheet.Range(renglon, 5, renglon, 7).Merge().Value = obtenernombre_mes(periodo_inicio) + " " + ejercicio_inicio + " A " + obtenernombre_mes(mes_actual) + " " + ejercicio;
                    //sheet.Range(renglon, 5, renglon, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    //sheet.Range(renglon, 5, renglon, 7).Style.Font.Bold = true;
                    //sheet.Range(renglon, 5, renglon, 7).Style.Fill.BackgroundColor = XLColor.LightGray;
                    //int renglonrecuperaciontot = renglon;


                    //renglon++;

                    sheet.Cell(renglon, 1).Value = "LINEA";
                    sheet.Cell(renglon, 2).Value = "SUCURSAL";
                    sheet.Cell(renglon, 3).Value = "CLIENTE";
                    sheet.Cell(renglon, 4).Value = "VENDEDOR";
                    sheet.Cell(renglon, 5).Value = "FECHA";
                    sheet.Cell(renglon, 6).Value = "NO. ECONOMICO";
                    sheet.Cell(renglon, 7).Value = "MODELO";
                    sheet.Cell(renglon, 8).Value = "IMPORTE";

                    // Estilo para los encabezados de la tabla
                    var rango = sheet.Range(renglon, 1, renglon, 8);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    // Llenar la tabla con los datos
                    foreach (var det in detalle)
                    {
                        sheet.Cell(renglon, 1).Value = det.scorecard;
                        sheet.Cell(renglon, 2).Value = det.sucursal;
                        sheet.Cell(renglon, 3).Value = det.razonsocial?.ToUpper();
                        sheet.Cell(renglon, 4).Value = det.vendedor?.ToUpper();
                        sheet.Cell(renglon, 5).Value = det.fecha_venta;
                        sheet.Cell(renglon, 6).Value = det.neconomico;
                        sheet.Cell(renglon, 7).Value = det.modelo;
                        sheet.Cell(renglon, 8).Value = det.importe_venta;
                        sheet.Cell(renglon, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        sheet.Cell(renglon, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        if (det.cancelado == 1)
                        {
                            // Cambiar el color del texto a rojo para todas las celdas de la fila
                            for (int col = 1; col <= 8; col++)
                            {
                                sheet.Cell(renglon, col).Style.Font.FontColor = XLColor.Red;
                            }
                        }
                        renglon++;
                    }

                    sheet.Column(8).Style.NumberFormat.Format = "#,##0.00";

                    //sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(7).Style.NumberFormat.Format = "0.0 %";

                    //rango = sheet.Range(renglon, 1, renglon, 8);
                    //rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#e5e6e6");
                    //rango.Style.Font.Bold = true;

                    sheet.Columns().AdjustToContents();
                    workbook.SaveAs(ruta);

                }
                if (System.IO.File.Exists(ruta))
                {
                    byte[] docbytes = System.IO.File.ReadAllBytes(ruta);
                    string docBase64 = Convert.ToBase64String(docbytes);
                    System.IO.File.Delete(ruta);
                    DocResult doc = new DocResult
                    {
                        documento = docBase64,
                        filename = sheetname
                    };
                    return Task.FromResult(doc);
                }
                throw new Exception("ERROR EN LA GENERACION DEL ARCHIVO, FAVOR DE COMUNICARSE CON EL ADMINISTRADOR DEL SISTEMA");
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { errores = ex.Message });
            }
        }
    }
}
