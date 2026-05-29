using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Auditoria.Modelos.Reporteria;

namespace HD_Auditoria.Reporteria
{
    public class XLS_Reporte_Simplificado
    {
        public static Task<DocResult> GenerarExcel(mdl_Reporte_Simplificado_View view, string? folio)
        {
            try
            {
                string sheetname = "REP. SIMPLIFICADO " + folio;
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, "REPORTE SIMPLIFICADO " + folio, 10);

                    sheet.Cell(renglon, 1).Value = "FAMILIA";
                    sheet.Cell(renglon, 2).Value = "SKU";
                    sheet.Cell(renglon, 3).Value = "DESCRIPCION";
                    sheet.Cell(renglon, 4).Value = "POSICION";
                    sheet.Cell(renglon, 5).Value = "EXISTENCIA";
                    sheet.Cell(renglon, 6).Value = "CONTEO";
                    sheet.Cell(renglon, 7).Value = "DIFERENCIAS";
                    sheet.Cell(renglon, 8).Value = "TIPO DE DIFERENCIA";
                    sheet.Cell(renglon, 9).Value = "IMPORTE DIFERENCIAS";
                    sheet.Cell(renglon, 10).Value = "COMENTARIOS";


                    // Estilo para los encabezados de la tabla
                    var rango = sheet.Range(renglon, 1, renglon, 10);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    float totalImporte = 0;

                    HashSet<string> skusProcesados = new HashSet<string>();
                    view.detalle = view.detalle
                    .OrderBy(x => x.sku)
                    .ToList();
                    // Llenar la tabla con los datos
                    foreach (var det in view.detalle)
                    {
                        bool skuYaExiste = skusProcesados.Contains(det.sku);

                        if (!skuYaExiste)
                        {
                            // 🔥 REGISTRO COMPLETO
                            sheet.Cell(renglon, 1).Value = det.familia;
                            sheet.Cell(renglon, 2).Value = det.sku;
                            sheet.Cell(renglon, 3).Value = det.descripcion;
                            sheet.Cell(renglon, 4).Value = det.posicion;
                            sheet.Cell(renglon, 5).Value = det.existencia;
                            sheet.Cell(renglon, 6).Value = det.conteo;
                            sheet.Cell(renglon, 7).Value = det.diferencias;

                            sheet.Cell(renglon, 8).Value =
                                det.tipo_diferencia == "F" ? "FALTANTE" :
                                det.tipo_diferencia == "S" ? "SOBRANTE" :
                                "";

                            sheet.Cell(renglon, 9).Value = det.importe_dif;
                            sheet.Cell(renglon, 10).Value = det.comentario;

                            skusProcesados.Add(det.sku);
                        }
                        else
                        {
                            sheet.Cell(renglon, 1).Value = det.familia;
                            sheet.Cell(renglon, 2).Value = det.sku;
                            sheet.Cell(renglon, 3).Value = det.descripcion;
                            sheet.Cell(renglon, 4).Value = det.posicion;
                            sheet.Cell(renglon, 6).Value = det.conteo;
                        }

                        renglon++;
                    }

                    //sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(9).Style.NumberFormat.Format = "#,##0.00";

                    renglon += 2;

                    // =======================
                    // TABLA RESUMEN
                    // =======================

                    int resumenInicio = renglon;

                    sheet.Cell(renglon, 5).Value = "TOTAL DEL INVENTARIO";
                    sheet.Cell(renglon, 6).Value = view.resumen.importe_total_inventario;
                    sheet.Cell(renglon, 6).Style.NumberFormat.Format = "$ #,##0.00";
                    renglon++;

                    sheet.Cell(renglon, 5).Value = "FALTANTE";
                    sheet.Cell(renglon, 6).Value =
                        $"{view.resumen.importe_faltante:#,##0.00} / {view.resumen.porc_faltante:0.##}%";
                    renglon++;

                    sheet.Cell(renglon, 5).Value = "SOBRANTE";
                    sheet.Cell(renglon, 6).Value =
                        $"{view.resumen.importe_sobrante:#,##0.00} / {view.resumen.porc_sobrante:0.##}%";
                    renglon++;

                    sheet.Cell(renglon, 5).Value = "TOTAL NETO";
                    sheet.Cell(renglon, 6).Value =
                        $"{view.resumen.total_neto:#,##0.00} / {view.resumen.porc_total_neto:0.##}%";
                    renglon++;

                    sheet.Cell(renglon, 5).Value = "CONFIABILIDAD";
                    sheet.Cell(renglon, 6).Value = view.resumen.confiabilidad;
                    sheet.Cell(renglon, 6).Style.NumberFormat.Format = "0.00\"%\"";
                    renglon++;

                    sheet.Cell(renglon, 5).Value = "CONFIABILIDAD UBICACIÓN";
                    sheet.Cell(renglon, 6).Value = view.resumen.confiabilidad_ubi;
                    sheet.Cell(renglon, 6).Style.NumberFormat.Format = "0.00\"%\"";
                    renglon++;

                    var resumenRange = sheet.Range(resumenInicio, 5, renglon - 1, 6);

                    resumenRange.Style.Font.FontName = "Calibri";
                    resumenRange.Style.Font.FontSize = 11;

                    resumenRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    resumenRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    sheet.Range(resumenInicio, 5, renglon - 1, 5).Style.Font.Bold = true;

                    sheet.Range(resumenInicio, 5, renglon - 1, 5).Style.Fill.BackgroundColor =
                        XLColor.FromHtml("#D9EAD3"); // verde claro John Deere

                    sheet.Range(resumenInicio, 5, renglon - 1, 5).Style.Font.FontColor =
                        XLColor.FromHtml("#1F4E1F");
                    sheet.Range(resumenInicio, 6, renglon - 1, 6).Style.Alignment.Horizontal =
                        XLAlignmentHorizontalValues.Right;
                    resumenRange.Style.Border.OutsideBorderColor =
                        XLColor.FromHtml("#367C2B");
                    sheet.Range(resumenInicio, 5, renglon - 1, 6).Style.Alignment.Vertical =
                        XLAlignmentVerticalValues.Center;

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
