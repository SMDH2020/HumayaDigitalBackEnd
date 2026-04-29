using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;
using HD_Cobranza.Modelos.RecuperacionCartera;

namespace HD_Cobranza.Reportes
{
    public class XLSCob_Reporte_Recuperacion_Cartera_Mensual
    {
        public static Task<DocResult> GenerarExcel(mdlRecuperacionObjetivoView datos)
        {
            try
            {
                string sheetname = "RECUPERACION CARTERA MENSUAL";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, $"RECUPERACION DE CARTERA MENSUAL", 13);

                    //renglon += 1;

                    var rangosub = sheet.Range(renglon, 1, renglon, 13);
                    rangosub.Value = "TOTAL CARTERA";
                    rangosub.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rangosub.Style.Font.Bold = true;
                    rangosub.Style.Font.FontSize = 12;
                    rangosub.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rangosub.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    rangosub.Merge();

                    sheet.Row(renglon).Height = 20;
                    sheet.Row(renglon + 1).Height = 4;
                    sheet.Row(renglon + 2).Height = 2;

                    rangosub = sheet.Range(renglon + 1, 1, renglon + 1, 13);
                    rangosub.Style.Fill.BackgroundColor = XLColor.FromHtml("#275027");

                    rangosub = sheet.Range(renglon + 2, 1, renglon + 2, 13);
                    rangosub.Style.Fill.BackgroundColor = XLColor.FromHtml("#E9AE06");

                    renglon += 3;

                    sheet.Range(renglon, 1, renglon, 13).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE"); 

                    sheet.Range(renglon, 1, renglon, 13).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");  

                    sheet.Range(renglon, 2, renglon, 5).Merge().Value = "CARTERA";
                    sheet.Range(renglon, 2, renglon, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 2, renglon, 5).Style.Font.Bold = true;
                    sheet.Range(renglon, 2, renglon, 5).Style.Fill.BackgroundColor = XLColor.LightGray;
                    int rengloncarteratot = renglon;

                    sheet.Range(renglon, 7, renglon, 11).Merge().Value = "RECUPERACION DE CARTERA";
                    sheet.Range(renglon, 7, renglon, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 7, renglon, 11).Style.Font.Bold = true;
                    sheet.Range(renglon, 7, renglon, 11).Style.Fill.BackgroundColor = XLColor.LightGray;
                    int renglonrecuperaciontot = renglon;


                    renglon++;

                    sheet.Cell(renglon, 1).Value = "PERIODO";
                    sheet.Cell(renglon, 2).Value = "ACTIVA";
                    sheet.Cell(renglon, 3).Value = "POR VENCER";
                    sheet.Cell(renglon, 4).Value = "VENCIDA";
                    sheet.Cell(renglon, 5).Value = "TOTAL";
                    sheet.Cell(renglon, 6).Value = "OBJETIVO";
                    sheet.Cell(renglon, 7).Value = "MES";
                    sheet.Cell(renglon, 8).Value = "ACTIVA";
                    sheet.Cell(renglon, 9).Value = "POR VENCER";
                    sheet.Cell(renglon, 10).Value = "VENCIDA";
                    sheet.Cell(renglon, 11).Value = "TOTAL";
                    sheet.Cell(renglon, 12).Value = "OBJETIVO RECUPERADO";
                    sheet.Cell(renglon, 13).Value = "%";

                    // Estilo para los encabezados de la tabla
                    var rango = sheet.Range(renglon, 1, renglon, 13);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    // Llenar la tabla con los datos
                    foreach (var total in datos.total)
                    {
                        sheet.Cell(renglon, 1).Value = total.mes;
                        sheet.Cell(renglon, 2).Value = total.cartera_activa;
                        sheet.Cell(renglon, 3).Value = total.cartera_porvencer;
                        sheet.Cell(renglon, 4).Value = total.cartera_vencida;
                        sheet.Cell(renglon, 5).Value = total.total_cartera;
                        sheet.Cell(renglon, 6).Value = total.objetivo;
                        sheet.Cell(renglon, 7).Value = total.recuperacion_mes;
                        sheet.Cell(renglon, 8).Value = total.recuperacion_activa;
                        sheet.Cell(renglon, 9).Value = total.recuperacion_porvencer;
                        sheet.Cell(renglon, 10).Value = total.recuperacion_vencida;
                        sheet.Cell(renglon, 11).Value = total.total_recuperado;
                        sheet.Cell(renglon, 12).Value = total.recuperado;
                        sheet.Cell(renglon, 13).Value = (total.porc / 100);
                        renglon++;
                    }


                    sheet.Range(rengloncarteratot, 2, renglon - 1, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(rengloncarteratot, 2, renglon - 1, 5).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Range(renglonrecuperaciontot, 7, renglon - 1, 11).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(renglonrecuperaciontot, 7, renglon - 1, 11).Style.Border.OutsideBorderColor = XLColor.Black;

                    for (int i = 2; i <= 13; i++)
                    {
                        sheet.Column(i).Style.NumberFormat.Format = "#,##0.00";
                    }

                    sheet.Column(13).Style.NumberFormat.Format = "0.00%";

                    // ------------------------------------------------------------------------------------------------------------------------------------------

                    renglon += 1;

                    var rangosub2 = sheet.Range(renglon, 1, renglon, 13);
                    rangosub2.Value = "CARTERA DE OPERACION";
                    rangosub2.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rangosub2.Style.Font.Bold = true;
                    rangosub2.Style.Font.FontSize = 12;
                    rangosub2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rangosub2.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    rangosub2.Merge();

                    sheet.Row(renglon).Height = 20;
                    sheet.Row(renglon + 1).Height = 4;
                    sheet.Row(renglon + 2).Height = 2;

                    rangosub2 = sheet.Range(renglon + 1, 1, renglon + 1, 13);
                    rangosub2.Style.Fill.BackgroundColor = XLColor.FromHtml("#275027");

                    rangosub2 = sheet.Range(renglon + 2, 1, renglon + 2, 13);
                    rangosub2.Style.Fill.BackgroundColor = XLColor.FromHtml("#E9AE06");

                    renglon += 3;

                    sheet.Range(renglon, 1, renglon, 13).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");

                    sheet.Range(renglon, 1, renglon, 13).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");

                    sheet.Range(renglon, 2, renglon, 5).Merge().Value = "CARTERA";
                    sheet.Range(renglon, 2, renglon, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 2, renglon, 5).Style.Font.Bold = true;
                    sheet.Range(renglon, 2, renglon, 5).Style.Fill.BackgroundColor = XLColor.LightGray;
                    int rengloncarteraop = renglon;

                    sheet.Range(renglon, 7, renglon, 11).Merge().Value = "RECUPERACION DE CARTERA";
                    sheet.Range(renglon, 7, renglon, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 7, renglon, 11).Style.Font.Bold = true;
                    sheet.Range(renglon, 7, renglon, 11).Style.Fill.BackgroundColor = XLColor.LightGray;
                    int renglonrecuperacionop = renglon;

                    renglon++;

                    sheet.Cell(renglon, 1).Value = "PERIODO";
                    sheet.Cell(renglon, 2).Value = "ACTIVA";
                    sheet.Cell(renglon, 3).Value = "POR VENCER";
                    sheet.Cell(renglon, 4).Value = "VENCIDA";
                    sheet.Cell(renglon, 5).Value = "TOTAL";
                    sheet.Cell(renglon, 6).Value = "OBJETIVO";
                    sheet.Cell(renglon, 7).Value = "MES";
                    sheet.Cell(renglon, 8).Value = "ACTIVA";
                    sheet.Cell(renglon, 9).Value = "POR VENCER";
                    sheet.Cell(renglon, 10).Value = "VENCIDA";
                    sheet.Cell(renglon, 11).Value = "TOTAL";
                    sheet.Cell(renglon, 12).Value = "OBJETIVO RECUPERADO";
                    sheet.Cell(renglon, 13).Value = "%";

                    // Estilo para los encabezados de la tabla
                    var rango2 = sheet.Range(renglon, 1, renglon, 13);
                    rango2.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango2.Style.Font.Bold = true;
                    rango2.Style.Font.FontSize = 12;
                    rango2.RangeUsed().SetAutoFilter();
                    rango2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango2.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    // Llenar la tabla con los datos
                    foreach (var operacion in datos.operacion)
                    {
                        sheet.Cell(renglon, 1).Value = operacion.mes;
                        sheet.Cell(renglon, 2).Value = operacion.cartera_activa;
                        sheet.Cell(renglon, 3).Value = operacion.cartera_porvencer;
                        sheet.Cell(renglon, 4).Value = operacion.cartera_vencida;
                        sheet.Cell(renglon, 5).Value = operacion.total_cartera;
                        sheet.Cell(renglon, 6).Value = operacion.objetivo;
                        sheet.Cell(renglon, 7).Value = operacion.recuperacion_mes;
                        sheet.Cell(renglon, 8).Value = operacion.recuperacion_activa;
                        sheet.Cell(renglon, 9).Value = operacion.recuperacion_porvencer;
                        sheet.Cell(renglon, 10).Value = operacion.recuperacion_vencida;
                        sheet.Cell(renglon, 11).Value = operacion.total_recuperado;
                        sheet.Cell(renglon, 12).Value = operacion.recuperado;
                        sheet.Cell(renglon, 13).Value = (operacion.porc / 100);
                        renglon++;
                    }


                    sheet.Range(rengloncarteraop, 2, renglon - 1, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(rengloncarteraop, 2, renglon - 1, 5).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Range(renglonrecuperacionop, 7, renglon - 1, 11).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(renglonrecuperacionop, 7, renglon - 1, 11).Style.Border.OutsideBorderColor = XLColor.Black;

                    for (int i = 2; i <= 13; i++)
                    {
                        sheet.Column(i).Style.NumberFormat.Format = "#,##0.00";
                    }

                    sheet.Column(13).Style.NumberFormat.Format = "0.00%";

                    // ----------------------------------------------------------------------------------------------------------------------------
                    
                    renglon += 1;

                    var rangosub3 = sheet.Range(renglon, 1, renglon, 13);
                    rangosub3.Value = "CARTERA REVOLVENTE";
                    rangosub3.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rangosub3.Style.Font.Bold = true;
                    rangosub3.Style.Font.FontSize = 12;
                    rangosub3.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rangosub3.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    rangosub3.Merge();

                    sheet.Row(renglon).Height = 20;
                    sheet.Row(renglon + 1).Height = 4;
                    sheet.Row(renglon + 2).Height = 2;

                    rangosub3 = sheet.Range(renglon + 1, 1, renglon + 1, 13);
                    rangosub3.Style.Fill.BackgroundColor = XLColor.FromHtml("#275027");

                    rangosub3 = sheet.Range(renglon + 2, 1, renglon + 2, 13);
                    rangosub3.Style.Fill.BackgroundColor = XLColor.FromHtml("#E9AE06");

                    renglon += 3;

                    sheet.Range(renglon, 1, renglon, 13).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");

                    sheet.Range(renglon, 1, renglon, 13).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");

                    sheet.Range(renglon, 2, renglon, 5).Merge().Value = "CARTERA";
                    sheet.Range(renglon, 2, renglon, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 2, renglon, 5).Style.Font.Bold = true;
                    sheet.Range(renglon, 2, renglon, 5).Style.Fill.BackgroundColor = XLColor.LightGray;
                    int rengloncarterarev = renglon;

                    sheet.Range(renglon, 7, renglon, 11).Merge().Value = "RECUPERACION DE CARTERA";
                    sheet.Range(renglon, 7, renglon, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 7, renglon, 11).Style.Font.Bold = true;
                    sheet.Range(renglon, 7, renglon, 11).Style.Fill.BackgroundColor = XLColor.LightGray;
                    int renglonrecuperacionrev = renglon;

                    renglon++;

                    sheet.Cell(renglon, 1).Value = "PERIODO";
                    sheet.Cell(renglon, 2).Value = "ACTIVA";
                    sheet.Cell(renglon, 3).Value = "POR VENCER";
                    sheet.Cell(renglon, 4).Value = "VENCIDA";
                    sheet.Cell(renglon, 5).Value = "TOTAL";
                    sheet.Cell(renglon, 6).Value = "OBJETIVO";
                    sheet.Cell(renglon, 7).Value = "MES";
                    sheet.Cell(renglon, 8).Value = "ACTIVA";
                    sheet.Cell(renglon, 9).Value = "POR VENCER";
                    sheet.Cell(renglon, 10).Value = "VENCIDA";
                    sheet.Cell(renglon, 11).Value = "TOTAL";
                    sheet.Cell(renglon, 12).Value = "OBJETIVO RECUPERADO";
                    sheet.Cell(renglon, 13).Value = "%";

                    // Estilo para los encabezados de la tabla
                    var rango3 = sheet.Range(renglon, 1, renglon, 13);
                    rango3.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango3.Style.Font.Bold = true;
                    rango3.Style.Font.FontSize = 12;
                    rango3.RangeUsed().SetAutoFilter();
                    rango3.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango3.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    // Llenar la tabla con los datos
                    foreach (var revolvente in datos.revolvente)
                    {
                        sheet.Cell(renglon, 1).Value = revolvente.mes;
                        sheet.Cell(renglon, 2).Value = revolvente.cartera_activa;
                        sheet.Cell(renglon, 3).Value = revolvente.cartera_porvencer;
                        sheet.Cell(renglon, 4).Value = revolvente.cartera_vencida;
                        sheet.Cell(renglon, 5).Value = revolvente.total_cartera;
                        sheet.Cell(renglon, 6).Value = revolvente.objetivo;
                        sheet.Cell(renglon, 7).Value = revolvente.recuperacion_mes;
                        sheet.Cell(renglon, 8).Value = revolvente.recuperacion_activa;
                        sheet.Cell(renglon, 9).Value = revolvente.recuperacion_porvencer;
                        sheet.Cell(renglon, 10).Value = revolvente.recuperacion_vencida;
                        sheet.Cell(renglon, 11).Value = revolvente.total_recuperado;
                        sheet.Cell(renglon, 12).Value = revolvente.recuperado;
                        sheet.Cell(renglon, 13).Value = (revolvente.porc / 100);
                        renglon++;
                    }


                    sheet.Range(rengloncarterarev, 2, renglon - 1, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(rengloncarterarev, 2, renglon - 1, 5).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Range(renglonrecuperacionrev, 7, renglon - 1, 11).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(renglonrecuperacionrev, 7, renglon - 1, 11).Style.Border.OutsideBorderColor = XLColor.Black;

                    for (int i = 2; i <= 13; i++)
                    {
                        sheet.Column(i).Style.NumberFormat.Format = "#,##0.00";
                    }

                    sheet.Column(13).Style.NumberFormat.Format = "0.00%";

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
