using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;

namespace HD_Cobranza.Reportes
{
    public class XLSCob_ListadoPagare
    {
        public static Task<DocResult> GenerarExcel(IEnumerable<mdlListadoPC> detalle)
        {
            try
            {
                string sheetname = "LISTADO DE PAGARES";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, $"LISTADO DE PAGARES", 10);


                    sheet.Cell(renglon, 1).Value = "SUCURSAL";
                    sheet.Cell(renglon, 2).Value = "CLIENTE";
                    sheet.Cell(renglon, 3).Value = "DOCUMENTO";
                    sheet.Cell(renglon, 4).Value = "SERIE";
                    sheet.Cell(renglon, 5).Value = "VENCIMIENTO";
                    sheet.Cell(renglon, 6).Value = "IMPORTE";
                    sheet.Cell(renglon, 7).Value = "TASA";
                    sheet.Cell(renglon, 8).Value = "INTERES";
                    sheet.Cell(renglon, 9).Value = "TOTAL";
                    sheet.Cell(renglon, 10).Value = "SALDO";

                    // Estilo para los encabezados de la tabla
                    var rango = sheet.Range(renglon, 1, renglon, 10);
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
                        sheet.Cell(renglon, 1).Value = det.sucursal;
                        sheet.Cell(renglon, 2).Value = det.razon_social?.ToUpper();
                        sheet.Cell(renglon, 3).Value = det.documento;
                        sheet.Cell(renglon, 4).Value = det.serie_fiscal == "-" ? "-" : string.Concat(det.serie_fiscal, " - ", det.folio_fiscal);
                        sheet.Cell(renglon, 5).Value = det.vencimiento;
                        sheet.Cell(renglon, 6).Value = det.importefinanciar;
                        sheet.Cell(renglon, 7).Value = det.tasa;
                        sheet.Cell(renglon, 8).Value = det.interes;
                        sheet.Cell(renglon, 9).Value = det.totalpagar;
                        sheet.Cell(renglon, 10).Value = det.saldo;
                        renglon++;
                    }

                    sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(7).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(8).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(9).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(10).Style.NumberFormat.Format = "#,##0.00";

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
