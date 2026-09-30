using ClosedXML.Excel;
using HD.AccesoDatos;
using HD.Fiscal.Modelos;

namespace HD.Fiscal.Reportes
{
    public class XLS_ConciliacionIngresos_Invoice
    {
        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_Conciliacion_Ingresos_Invoice> detalle, string? titulo)
        {
            try
            {
                string sheetname = "CONC. DE ING. INVOICE";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, titulo, 17);

                    sheet.Cell(renglon, 1).Value = "ORIGEN";
                    sheet.Cell(renglon, 2).Value = "DOCUMENTO";
                    sheet.Cell(renglon, 3).Value = "CUST. ORD. NO.";
                    sheet.Cell(renglon, 4).Value = "FECHA";
                    sheet.Cell(renglon, 5).Value = "RO. NUMBER";
                    sheet.Cell(renglon, 6).Value = "SPECIAL INST.";
                    sheet.Cell(renglon, 7).Value = "SERIE FISCAL";
                    sheet.Cell(renglon, 8).Value = "FOLIO FISCAL";
                    sheet.Cell(renglon, 9).Value = "BATCH";
                    sheet.Cell(renglon, 10).Value = "CUENTA";
                    sheet.Cell(renglon, 11).Value = "UUID";
                    sheet.Cell(renglon, 12).Value = "RFC";
                    sheet.Cell(renglon, 13).Value = "TIPO DE COMPROBANTE";
                    sheet.Cell(renglon, 14).Value = "CONDICION DE PAGO";
                    sheet.Cell(renglon, 15).Value = "CANCELADO";
                    sheet.Cell(renglon, 16).Value = "FECHA CANCELACION";
                    sheet.Cell(renglon, 17).Value = "IMPORTE";


                    // Estilo para los encabezados de la tabla
                    var rango = sheet.Range(renglon, 1, renglon, 17);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    float totalImporte = 0;

                    // Llenar la tabla con los datos
                    foreach (var det in detalle)
                    {
                        sheet.Cell(renglon, 1).Value = det.origen?.ToUpper();
                        sheet.Cell(renglon, 2).Value = det.documento;
                        sheet.Cell(renglon, 3).Value = det.cust_ord_no;
                        sheet.Cell(renglon, 4).Value = det.fecha;
                        sheet.Cell(renglon, 5).Value = det.ro_number;
                        sheet.Cell(renglon, 6).Value = det.special_inst;
                        sheet.Cell(renglon, 7).Value = det.serie_fiscal;
                        sheet.Cell(renglon, 8).Value = det.folio_fiscal;
                        sheet.Cell(renglon, 9).Value = det.batch;
                        sheet.Cell(renglon, 10).Value = det.cuenta;
                        sheet.Cell(renglon, 11).Value = det.uuid;
                        sheet.Cell(renglon, 12).Value = det.rfc;
                        sheet.Cell(renglon, 13).Value = det.tipoComprobante;
                        sheet.Cell(renglon, 14).Value = det.condicionPago;
                        sheet.Cell(renglon, 15).Value = det.cancelado ? "SI" : "NO";
                        sheet.Cell(renglon, 16).Value = det.fechacancelacion;
                        sheet.Cell(renglon, 17).Value = det.importe;
                        totalImporte += det.importe;
                        renglon++;
                    }

                    sheet.Cell(renglon, 16).Value = "TOTAL:";
                    sheet.Cell(renglon, 17).Value = totalImporte;

                    var totalRange = sheet.Range(renglon, 1, renglon, 17);
                    totalRange.Style.Font.Bold = true;
                    totalRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    totalRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    //sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(17).Style.NumberFormat.Format = "#,##0.00";

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
