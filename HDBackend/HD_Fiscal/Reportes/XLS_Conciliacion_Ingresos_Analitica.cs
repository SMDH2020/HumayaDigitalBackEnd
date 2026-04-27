using HD.AccesoDatos;
using ClosedXML.Excel;

using HD.Fiscal.Modelos;

namespace HD.Fiscal.Reportes
{
    public class XLS_Conciliacion_Ingresos_Analitica
    {
        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_Conciliacion_Ingresos_Analitica> detalle, string? titulo)
        {
            try
            {
                string sheetname = "CONC. DE ING. ANALITICA";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, titulo, 25);

                    sheet.Cell(renglon, 1).Value = "ORIGEN";
                    sheet.Cell(renglon, 2).Value = "SUCURSAL";
                    sheet.Cell(renglon, 3).Value = "DEPARTAMENTO";
                    sheet.Cell(renglon, 4).Value = "CUENTA";
                    sheet.Cell(renglon, 5).Value = "GL. DESC";
                    sheet.Cell(renglon, 6).Value = "CARGOS";
                    sheet.Cell(renglon, 7).Value = "ABONOS";
                    sheet.Cell(renglon, 8).Value = "GL. MAIN";
                    sheet.Cell(renglon, 9).Value = "FECHA";
                    sheet.Cell(renglon, 10).Value = "BATCH";
                    sheet.Cell(renglon, 11).Value = "DOCUMENTO";
                    sheet.Cell(renglon, 12).Value = "SERIE FISCAL";
                    sheet.Cell(renglon, 13).Value = "FOLIO FISCAL";
                    sheet.Cell(renglon, 14).Value = "FECHA DE CANCELACION";
                    sheet.Cell(renglon, 15).Value = "UUID";
                    sheet.Cell(renglon, 16).Value = "ESTADO";
                    sheet.Cell(renglon, 17).Value = "TIPO DE COMPROBANTE";
                    sheet.Cell(renglon, 18).Value = "RFC";
                    sheet.Cell(renglon, 19).Value = "CONDICION DE PAGO";
                    sheet.Cell(renglon, 20).Value = "DESC.";
                    sheet.Cell(renglon, 21).Value = "REF.";
                    sheet.Cell(renglon, 22).Value = "USUARIO";
                    sheet.Cell(renglon, 23).Value = "ORIG. INVOICE NO.";
                    sheet.Cell(renglon, 24).Value = "DOCUMENTO DE REFACTURACION";
                    sheet.Cell(renglon, 25).Value = "EQUIP";


                    // Estilo para los encabezados de la tabla
                    var rango = sheet.Range(renglon, 1, renglon, 25);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    float totalCargos = 0;
                    float totalAbonos = 0;

                    // Llenar la tabla con los datos
                    foreach (var det in detalle)
                    {
                        sheet.Cell(renglon, 1).Value = det.origen?.ToUpper();
                        sheet.Cell(renglon, 2).Value = det.sucursal?.ToUpper();
                        sheet.Cell(renglon, 3).Value = det.departamento?.ToUpper();
                        sheet.Cell(renglon, 4).Value = det.cuenta;
                        sheet.Cell(renglon, 5).Value = det.v_gl_desc?.ToUpper();
                        sheet.Cell(renglon, 6).Value = det.v_cargos;
                        sheet.Cell(renglon, 7).Value = det.v_abonos;
                        sheet.Cell(renglon, 8).Value = det.v_gl_main;
                        sheet.Cell(renglon, 9).Value = det.v_fecha;
                        sheet.Cell(renglon, 10).Value = det.v_batch;
                        sheet.Cell(renglon, 11).Value = det.document_no;
                        sheet.Cell(renglon, 12).Value = det.serie;
                        sheet.Cell(renglon, 13).Value = det.folio;
                        sheet.Cell(renglon, 14).Value = det.fechacancelacion;
                        sheet.Cell(renglon, 15).Value = det.uuid;
                        sheet.Cell(renglon, 16).Value = det.estatus?.ToUpper();
                        sheet.Cell(renglon, 17).Value = det.tipoComprobante?.ToUpper();
                        sheet.Cell(renglon, 18).Value = det.rfc;
                        sheet.Cell(renglon, 19).Value = det.condicionPago;
                        sheet.Cell(renglon, 20).Value = det.v_desc?.ToUpper();
                        sheet.Cell(renglon, 21).Value = det.v_ref;
                        sheet.Cell(renglon, 22).Value = det.v_usuario;
                        sheet.Cell(renglon, 23).Value = det.orig_invoice_no;
                        sheet.Cell(renglon, 24).Value = det.document_refacturacion;
                        sheet.Cell(renglon, 25).Value = det.equip?.ToUpper();

                        totalCargos += det.v_cargos;
                        totalAbonos += det.v_abonos;
                        renglon++;
                    }

                    float diferencia = 0;
                    diferencia = totalAbonos - totalCargos;
                    sheet.Cell(renglon, 6).Value = totalCargos;
                    sheet.Cell(renglon, 7).Value = totalAbonos;
                    var totalRange = sheet.Range(renglon, 1, renglon, 25);
                    totalRange.Style.Font.Bold = true;
                    totalRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    totalRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    renglon++;
                    sheet.Cell(renglon, 6).Value = "DIFERENCIA:";
                    sheet.Cell(renglon, 7).Value = diferencia;

                    var diferenciaRange = sheet.Range(renglon, 1, renglon, 25);
                    diferenciaRange.Style.Font.Bold = true;
                    diferenciaRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    diferenciaRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(7).Style.NumberFormat.Format = "#,##0.00";

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
