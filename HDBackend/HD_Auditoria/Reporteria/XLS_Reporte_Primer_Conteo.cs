using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Auditoria.Modelos.Reporteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Reporteria
{
    public class XLS_Reporte_Primer_Conteo
    {
        public static Task<DocResult> GenerarExcel(mdl_Reporte_Primer_Conteo_View view, string? folio)
        {
            try
            {
                string sheetname = "REP. PRIMER CONT. " + folio;
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, "REPORTE PRIMER CONTEO " + folio, 19);

                    sheet.Cell(renglon, 1).Value = "FAMILIA";
                    sheet.Cell(renglon, 2).Value = "SKU";
                    sheet.Cell(renglon, 3).Value = "DESCRIPCION";
                    sheet.Cell(renglon, 4).Value = "UBICACIÓN";
                    sheet.Cell(renglon, 5).Value = "UBICACION CORRECTA";
                    sheet.Cell(renglon, 6).Value = "COSTO UNITARIO";
                    sheet.Cell(renglon, 7).Value = "EXISTENCIA";
                    sheet.Cell(renglon, 8).Value = "UM";
                    sheet.Cell(renglon, 9).Value = "EXISTENCIA ($)";
                    sheet.Cell(renglon, 10).Value = "CONTEO FISICO";
                    sheet.Cell(renglon, 11).Value = "CONTEO FISICO ($)";
                    sheet.Cell(renglon, 12).Value = "DIFERENCIAS";
                    sheet.Cell(renglon, 13).Value = "TIPO DIFERENCIAS";
                    sheet.Cell(renglon, 14).Value = "DIFERENCIAS ($)";
                    sheet.Cell(renglon, 15).Value = "DIFERENCIAS (%)";
                    sheet.Cell(renglon, 16).Value = "JUSTIFICADAS";
                    sheet.Cell(renglon, 17).Value = "JUSTIFICADAS ($)";
                    sheet.Cell(renglon, 18).Value = "NO JUSTIFICADAS";
                    sheet.Cell(renglon, 19).Value = "NO JUSTIFICADAS ($)";

                    // Estilo para los encabezados de la tabla
                    var rango = sheet.Range(renglon, 1, renglon, 19);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    float totalImporte = 0;
                    double totalExistencia = 0;
                    double totalImporteExistencia = 0;
                    double totalConteo = 0;
                    double totalImporteConteo = 0;
                    double totalDiferencias = 0;
                    double totalImporteDiferencias = 0;
                    double totalJustificadas = 0;
                    double totalImporteJustificadas = 0;
                    double totalNoJustificadas = 0;
                    double totalImporteNoJustificadas = 0;

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
                            sheet.Cell(renglon, 1).Value = det.familia;
                            sheet.Cell(renglon, 2).Value = det.sku;
                            sheet.Cell(renglon, 3).Value = det.descripcion;
                            sheet.Cell(renglon, 4).Value = det.posicion;
                            sheet.Cell(renglon, 5).Value = det.ubicacion_correcta;
                            sheet.Cell(renglon, 6).Value = det.precio_unitario;
                            sheet.Cell(renglon, 7).Value = det.existencia;
                            sheet.Cell(renglon, 8).Value = det.unidad_medida;
                            sheet.Cell(renglon, 9).Value = det.importe_existencia;
                            sheet.Cell(renglon, 10).Value = det.conteo;
                            sheet.Cell(renglon, 11).Value = det.importe_cont_fisico;
                            sheet.Cell(renglon, 12).Value = det.diferencias;
                            sheet.Cell(renglon, 13).Value =
                                det.tipo_diferencia == "F" ? "FALTANTE" :
                                det.tipo_diferencia == "S" ? "SOBRANTE" :
                                "CORRECTO";
                            sheet.Cell(renglon, 14).Value = det.importe_dif;
                            sheet.Cell(renglon, 15).Value = det.porc_dif;
                            sheet.Cell(renglon, 16).Value = det.justificadas;
                            sheet.Cell(renglon, 17).Value = det.importe_justificadas;
                            sheet.Cell(renglon, 18).Value = det.no_justificadas;
                            sheet.Cell(renglon, 19).Value = det.importe_no_justificadas;
                            skusProcesados.Add(det.sku);

                        } else
                        {
                            sheet.Cell(renglon, 1).Value = det.familia;
                            sheet.Cell(renglon, 2).Value = det.sku;
                            sheet.Cell(renglon, 3).Value = det.descripcion;
                            sheet.Cell(renglon, 4).Value = det.posicion;
                            //sheet.Cell(renglon, 5).Value = det.ubicacion_correcta;
                            //sheet.Cell(renglon, 6).Value = det.precio_unitario;
                            //sheet.Cell(renglon, 7).Value = det.existencia;
                            //sheet.Cell(renglon, 8).Value = det.unidad_medida;
                            //sheet.Cell(renglon, 9).Value = det.importe_existencia;
                            sheet.Cell(renglon, 10).Value = det.conteo;
                            sheet.Cell(renglon, 11).Value = det.importe_cont_fisico;
                        }
                            renglon++;

                        totalExistencia += Convert.ToDouble(det.existencia);
                        totalImporteExistencia += Convert.ToDouble(det.importe_existencia);

                        totalConteo += Convert.ToDouble(det.conteo);
                        totalImporteConteo += Convert.ToDouble(det.importe_cont_fisico);

                        totalDiferencias += Convert.ToDouble(det.diferencias);
                        totalImporteDiferencias += Convert.ToDouble(det.importe_dif);

                        totalJustificadas += Convert.ToDouble(det.justificadas);
                        totalImporteJustificadas += Convert.ToDouble(det.importe_justificadas);

                        totalNoJustificadas += Convert.ToDouble(det.no_justificadas);
                        totalImporteNoJustificadas += Convert.ToDouble(det.importe_no_justificadas);
                    }

                    // =======================
                    // FILA TOTAL
                    // =======================

                    sheet.Cell(renglon, 1).Value = "TOTALES";

                    sheet.Cell(renglon, 7).Value = totalExistencia;
                    sheet.Cell(renglon, 9).Value = totalImporteExistencia;

                    sheet.Cell(renglon, 10).Value = totalConteo;
                    sheet.Cell(renglon, 11).Value = totalImporteConteo;

                    sheet.Cell(renglon, 12).Value = totalDiferencias;
                    sheet.Cell(renglon, 14).Value = totalImporteDiferencias;

                    sheet.Cell(renglon, 16).Value = totalJustificadas;
                    sheet.Cell(renglon, 17).Value = totalImporteJustificadas;

                    sheet.Cell(renglon, 18).Value = totalNoJustificadas;
                    sheet.Cell(renglon, 19).Value = totalImporteNoJustificadas;

                    var totalRange = sheet.Range(renglon, 1, renglon, 19);

                    totalRange.Style.Font.Bold = true;
                    totalRange.Style.Fill.BackgroundColor = XLColor.LightGray;

                    renglon++;

                    //sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(6).Style.NumberFormat.Format = "$ #,##0.00";
                    sheet.Column(9).Style.NumberFormat.Format = "$ #,##0.00";
                    sheet.Column(11).Style.NumberFormat.Format = "$ #,##0.00";
                    sheet.Column(14).Style.NumberFormat.Format = "$ #,##0.00";
                    sheet.Column(17).Style.NumberFormat.Format = "$ #,##0.00";
                    sheet.Column(19).Style.NumberFormat.Format = "$ #,##0.00";

                    renglon += 2;

                    // =======================
                    // TABLA RESUMEN
                    // =======================

                    int resumenInicio = renglon;

                    sheet.Range(renglon, 5, renglon, 8).Merge();

                    sheet.Cell(renglon, 5).Value = "GENERALES";

                    sheet.Cell(renglon, 5).Style.Font.Bold = true;
                    sheet.Cell(renglon, 5).Style.Font.FontColor = XLColor.White;

                    sheet.Cell(renglon, 5).Style.Fill.BackgroundColor =
                        XLColor.FromHtml("#275027");

                    sheet.Cell(renglon, 5).Style.Alignment.Horizontal =
                        XLAlignmentHorizontalValues.Center;

                    sheet.Cell(renglon, 5).Style.Alignment.Vertical =
                        XLAlignmentVerticalValues.Center;

                    renglon++;

                    // FALTANTE
                    sheet.Cell(renglon, 5).Value = view.resumen.conteo_faltante;
                    sheet.Cell(renglon, 6).Value = "FALTANTE";
                    sheet.Cell(renglon, 7).Value = view.resumen.importe_faltante;
                    sheet.Cell(renglon, 8).Value = view.resumen.porc_faltante / 100;

                    sheet.Cell(renglon, 7).Style.NumberFormat.Format = "$ #,##0.00";
                    sheet.Cell(renglon, 8).Style.NumberFormat.Format = "0.00%";

                    renglon++;

                    // SOBRANTE
                    sheet.Cell(renglon, 5).Value = view.resumen.conteo_sobrante;
                    sheet.Cell(renglon, 6).Value = "SOBRANTE";
                    sheet.Cell(renglon, 7).Value = view.resumen.importe_sobrante;
                    sheet.Cell(renglon, 8).Value = view.resumen.porc_sobrante / 100;

                    sheet.Cell(renglon, 7).Style.NumberFormat.Format = "$ #,##0.00";
                    sheet.Cell(renglon, 8).Style.NumberFormat.Format = "0.00%";

                    renglon++;

                    // ESTILO TABLA
                    var resumenRange = sheet.Range(resumenInicio, 5, renglon - 1, 8);

                    resumenRange.Style.Border.OutsideBorder =
                        XLBorderStyleValues.Thin;

                    resumenRange.Style.Border.InsideBorder =
                        XLBorderStyleValues.Thin;

                    resumenRange.Style.Border.OutsideBorderColor =
                        XLColor.FromHtml("#367C2B");

                    resumenRange.Style.Alignment.Horizontal =
                        XLAlignmentHorizontalValues.Center;

                    resumenRange.Style.Alignment.Vertical =
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
