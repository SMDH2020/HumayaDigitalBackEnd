using ClosedXML.Excel;
using DocumentFormat.OpenXml.Vml;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;
using HD_Cobranza.Modelos.RecuperacionCartera;
using HD_Cobranza.Modelos.ReporteRecuperacionCompleta;

namespace HD_Cobranza.Reportes
{
    public class XLSCob_Reporte_Recuperacion_Completa
    {
        public static Task<DocResult> GenerarExcel(mdl_Recuperacion_Completa_View datos)
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

                    int renglon = XLSEncabezado.Encabezado(ref sheet, $"RECUPERACION DE CARTERA MENSUAL", 17);

                    //renglon += 1;

                    var rangosub = sheet.Range(renglon, 1, renglon, 17);
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

                    rangosub = sheet.Range(renglon + 1, 1, renglon + 1, 17);
                    rangosub.Style.Fill.BackgroundColor = XLColor.FromHtml("#275027");

                    rangosub = sheet.Range(renglon + 2, 1, renglon + 2, 17);
                    rangosub.Style.Fill.BackgroundColor = XLColor.FromHtml("#E9AE06");

                    renglon += 3;

                    sheet.Range(renglon, 1, renglon, 17).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");

                    sheet.Range(renglon, 1, renglon, 17).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");

                    sheet.Range(renglon, 2, renglon, 5).Merge().Value = "CARTERA";
                    sheet.Range(renglon, 2, renglon, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 2, renglon, 5).Style.Font.Bold = true;
                    sheet.Range(renglon, 2, renglon, 5).Style.Fill.BackgroundColor = XLColor.LightGray;
                    int rengloncarteratot = renglon;

                    sheet.Range(renglon, 6, renglon, 8).Merge().Value = "OBJETIVO";
                    sheet.Range(renglon, 6, renglon, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 6, renglon, 8).Style.Font.Bold = true;
                    sheet.Range(renglon, 6, renglon, 8).Style.Fill.BackgroundColor = XLColor.LightGray;
                    int rengloncarteraobjetivo = renglon;

                    sheet.Range(renglon, 9, renglon, 15).Merge().Value = "RECUPERACION DE CARTERA";
                    sheet.Range(renglon, 9, renglon, 15).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 9, renglon, 15).Style.Font.Bold = true;
                    sheet.Range(renglon, 9, renglon, 15).Style.Fill.BackgroundColor = XLColor.LightGray;
                    int renglonrecuperaciontot = renglon;


                    renglon++;

                    sheet.Cell(renglon, 1).Value = "PERIODO";
                    sheet.Cell(renglon, 2).Value = "ACTIVA";
                    sheet.Cell(renglon, 3).Value = "POR VENCER";
                    sheet.Cell(renglon, 4).Value = "VENCIDA";
                    sheet.Cell(renglon, 5).Value = "TOTAL";
                    sheet.Cell(renglon, 6).Value = "POR VENCER";
                    sheet.Cell(renglon, 7).Value = "VENCIDO";
                    sheet.Cell(renglon, 8).Value = "TOTAL";
                    sheet.Cell(renglon, 9).Value = "MES";
                    sheet.Cell(renglon, 10).Value = "ACTIVA";
                    sheet.Cell(renglon, 11).Value = "POR VENCER";
                    sheet.Cell(renglon, 12).Value = "%";
                    sheet.Cell(renglon, 13).Value = "VENCIDA";
                    sheet.Cell(renglon, 14).Value = "%";
                    sheet.Cell(renglon, 15).Value = "TOTAL";
                    sheet.Cell(renglon, 16).Value = "OBJETIVO RECUPERADO";
                    sheet.Cell(renglon, 17).Value = "%";

                    // Estilo para los encabezados de la tabla
                    var rango = sheet.Range(renglon, 1, renglon, 17);
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
                        sheet.Cell(renglon, 6).Value = total.objetivo_porvencer;
                        sheet.Cell(renglon, 7).Value = total.objetivo_vencido;
                        sheet.Cell(renglon, 8).Value = total.objetivo;
                        sheet.Cell(renglon, 9).Value = total.recuperacion_mes;
                        sheet.Cell(renglon, 10).Value = total.recuperacion_activa;
                        sheet.Cell(renglon, 11).Value = total.recuperacion_porvencer;
                        sheet.Cell(renglon, 12).Value = (total.porcporvencer / 100);
                        sheet.Cell(renglon, 13).Value = total.recuperacion_vencida;
                        sheet.Cell(renglon, 14).Value = (total.porcvencido / 100);
                        sheet.Cell(renglon, 15).Value = total.total_recuperado;
                        sheet.Cell(renglon, 16).Value = total.recuperado;
                        sheet.Cell(renglon, 17).Value = (total.porc / 100);
                        renglon++;
                    }


                    sheet.Range(rengloncarteratot, 2, renglon - 1, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(rengloncarteratot, 2, renglon - 1, 5).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Range(rengloncarteratot, 6, renglon - 1, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(rengloncarteratot, 6, renglon - 1, 8).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Range(renglonrecuperaciontot, 9, renglon - 1, 15).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(renglonrecuperaciontot, 9, renglon - 1, 15).Style.Border.OutsideBorderColor = XLColor.Black;

                    for (int i = 2; i <= 17; i++)
                    {
                        sheet.Column(i).Style.NumberFormat.Format = "#,##0.00";
                    }

                    sheet.Column(12).Style.NumberFormat.Format = "0.00%";
                    sheet.Column(14).Style.NumberFormat.Format = "0.00%";
                    sheet.Column(17).Style.NumberFormat.Format = "0.00%";

                    // ------------------------------------------------------------------------------------------------------------------------------------------

                    renglon += 1;

                    var rangosub2 = sheet.Range(renglon, 1, renglon, 17);
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

                    rangosub2 = sheet.Range(renglon + 1, 1, renglon + 1, 17);
                    rangosub2.Style.Fill.BackgroundColor = XLColor.FromHtml("#275027");

                    rangosub2 = sheet.Range(renglon + 2, 1, renglon + 2, 17);
                    rangosub2.Style.Fill.BackgroundColor = XLColor.FromHtml("#E9AE06");

                    renglon += 3;

                    sheet.Range(renglon, 1, renglon, 17).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");

                    sheet.Range(renglon, 1, renglon, 17).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");

                    sheet.Range(renglon, 2, renglon, 5).Merge().Value = "CARTERA";
                    sheet.Range(renglon, 2, renglon, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 2, renglon, 5).Style.Font.Bold = true;
                    sheet.Range(renglon, 2, renglon, 5).Style.Fill.BackgroundColor = XLColor.LightGray;
                    int rengloncarteraop = renglon;

                    sheet.Range(renglon, 6, renglon, 8).Merge().Value = "OBJETIVO";
                    sheet.Range(renglon, 6, renglon, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 6, renglon, 8).Style.Font.Bold = true;
                    sheet.Range(renglon, 6, renglon, 8).Style.Fill.BackgroundColor = XLColor.LightGray;
                    int rengloncarteraobjetivoop = renglon;

                    sheet.Range(renglon, 9, renglon, 15).Merge().Value = "RECUPERACION DE CARTERA";
                    sheet.Range(renglon, 9, renglon, 15).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 9, renglon, 15).Style.Font.Bold = true;
                    sheet.Range(renglon, 9, renglon, 15).Style.Fill.BackgroundColor = XLColor.LightGray;
                    int renglonrecuperacionop = renglon;

                    renglon++;

                    sheet.Cell(renglon, 1).Value = "PERIODO";
                    sheet.Cell(renglon, 2).Value = "ACTIVA";
                    sheet.Cell(renglon, 3).Value = "POR VENCER";
                    sheet.Cell(renglon, 4).Value = "VENCIDA";
                    sheet.Cell(renglon, 5).Value = "TOTAL";
                    sheet.Cell(renglon, 6).Value = "POR VENCER";
                    sheet.Cell(renglon, 7).Value = "VENCIDO";
                    sheet.Cell(renglon, 8).Value = "TOTAL";
                    sheet.Cell(renglon, 9).Value = "MES";
                    sheet.Cell(renglon, 10).Value = "ACTIVA";
                    sheet.Cell(renglon, 11).Value = "POR VENCER";
                    sheet.Cell(renglon, 12).Value = "%";
                    sheet.Cell(renglon, 13).Value = "VENCIDA";
                    sheet.Cell(renglon, 14).Value = "%";
                    sheet.Cell(renglon, 15).Value = "TOTAL";
                    sheet.Cell(renglon, 16).Value = "OBJETIVO RECUPERADO";
                    sheet.Cell(renglon, 17).Value = "%";

                    // Estilo para los encabezados de la tabla
                    var rango2 = sheet.Range(renglon, 1, renglon, 17);
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
                        sheet.Cell(renglon, 6).Value = operacion.objetivo_porvencer;
                        sheet.Cell(renglon, 7).Value = operacion.objetivo_vencido;
                        sheet.Cell(renglon, 8).Value = operacion.objetivo;
                        sheet.Cell(renglon, 9).Value = operacion.recuperacion_mes;
                        sheet.Cell(renglon, 10).Value = operacion.recuperacion_activa;
                        sheet.Cell(renglon, 11).Value = operacion.recuperacion_porvencer;
                        sheet.Cell(renglon, 12).Value = (operacion.porcporvencer / 100);
                        sheet.Cell(renglon, 13).Value = operacion.recuperacion_vencida;
                        sheet.Cell(renglon, 14).Value = (operacion.porcvencido / 100);
                        sheet.Cell(renglon, 15).Value = operacion.total_recuperado;
                        sheet.Cell(renglon, 16).Value = operacion.recuperado;
                        sheet.Cell(renglon, 17).Value = (operacion.porc / 100);
                        renglon++;
                    }


                    sheet.Range(rengloncarteraop, 2, renglon - 1, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(rengloncarteraop, 2, renglon - 1, 5).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Range(rengloncarteraobjetivoop, 6, renglon - 1, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(rengloncarteraobjetivoop, 6, renglon - 1, 8).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Range(renglonrecuperacionop, 9, renglon - 1, 15).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(renglonrecuperacionop, 9, renglon - 1, 15).Style.Border.OutsideBorderColor = XLColor.Black;

                    for (int i = 2; i <= 17; i++)
                    {
                        sheet.Column(i).Style.NumberFormat.Format = "#,##0.00";
                    }

                    sheet.Column(12).Style.NumberFormat.Format = "0.00%";
                    sheet.Column(14).Style.NumberFormat.Format = "0.00%";
                    sheet.Column(17).Style.NumberFormat.Format = "0.00%";

                    // ----------------------------------------------------------------------------------------------------------------------------

                    renglon += 1;

                    var rangosub3 = sheet.Range(renglon, 1, renglon, 17);
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

                    rangosub3 = sheet.Range(renglon + 1, 1, renglon + 1, 17);
                    rangosub3.Style.Fill.BackgroundColor = XLColor.FromHtml("#275027");

                    rangosub3 = sheet.Range(renglon + 2, 1, renglon + 2, 17);
                    rangosub3.Style.Fill.BackgroundColor = XLColor.FromHtml("#E9AE06");

                    renglon += 3;

                    sheet.Range(renglon, 1, renglon, 17).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");

                    sheet.Range(renglon, 1, renglon, 17).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");

                    sheet.Range(renglon, 2, renglon, 5).Merge().Value = "CARTERA";
                    sheet.Range(renglon, 2, renglon, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 2, renglon, 5).Style.Font.Bold = true;
                    sheet.Range(renglon, 2, renglon, 5).Style.Fill.BackgroundColor = XLColor.LightGray;
                    int rengloncarterarev = renglon;

                    sheet.Range(renglon, 6, renglon, 8).Merge().Value = "OBJETIVO";
                    sheet.Range(renglon, 6, renglon, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 6, renglon, 8).Style.Font.Bold = true;
                    sheet.Range(renglon, 6, renglon, 8).Style.Fill.BackgroundColor = XLColor.LightGray;
                    int rengloncarteraobjetivorev = renglon;

                    sheet.Range(renglon, 9, renglon, 15).Merge().Value = "RECUPERACION DE CARTERA";
                    sheet.Range(renglon, 9, renglon, 15).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 9, renglon, 15).Style.Font.Bold = true;
                    sheet.Range(renglon, 9, renglon, 15).Style.Fill.BackgroundColor = XLColor.LightGray;
                    int renglonrecuperacionrev = renglon;

                    renglon++;

                    sheet.Cell(renglon, 1).Value = "PERIODO";
                    sheet.Cell(renglon, 2).Value = "ACTIVA";
                    sheet.Cell(renglon, 3).Value = "POR VENCER";
                    sheet.Cell(renglon, 4).Value = "VENCIDA";
                    sheet.Cell(renglon, 5).Value = "TOTAL";
                    sheet.Cell(renglon, 6).Value = "POR VENCER";
                    sheet.Cell(renglon, 7).Value = "VENCIDO";
                    sheet.Cell(renglon, 8).Value = "TOTAL";
                    sheet.Cell(renglon, 9).Value = "MES";
                    sheet.Cell(renglon, 10).Value = "ACTIVA";
                    sheet.Cell(renglon, 11).Value = "POR VENCER";
                    sheet.Cell(renglon, 12).Value = "%";
                    sheet.Cell(renglon, 13).Value = "VENCIDA";
                    sheet.Cell(renglon, 14).Value = "%";
                    sheet.Cell(renglon, 15).Value = "TOTAL";
                    sheet.Cell(renglon, 16).Value = "OBJETIVO RECUPERADO";
                    sheet.Cell(renglon, 17).Value = "%";

                    // Estilo para los encabezados de la tabla
                    var rango3 = sheet.Range(renglon, 1, renglon, 17);
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
                        sheet.Cell(renglon, 6).Value = revolvente.objetivo_porvencer;
                        sheet.Cell(renglon, 7).Value = revolvente.objetivo_vencido;
                        sheet.Cell(renglon, 8).Value = revolvente.objetivo;
                        sheet.Cell(renglon, 9).Value = revolvente.recuperacion_mes;
                        sheet.Cell(renglon, 10).Value = revolvente.recuperacion_activa;
                        sheet.Cell(renglon, 11).Value = revolvente.recuperacion_porvencer;
                        sheet.Cell(renglon, 12).Value = (revolvente.porcporvencer / 100);
                        sheet.Cell(renglon, 13).Value = revolvente.recuperacion_vencida;
                        sheet.Cell(renglon, 14).Value = (revolvente.porcvencido / 100);
                        sheet.Cell(renglon, 15).Value = revolvente.total_recuperado;
                        sheet.Cell(renglon, 16).Value = revolvente.recuperado;
                        sheet.Cell(renglon, 17).Value = (revolvente.porc / 100);
                        renglon++;
                    }


                    sheet.Range(rengloncarterarev, 2, renglon - 1, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(rengloncarterarev, 2, renglon - 1, 5).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Range(rengloncarteraobjetivorev, 6, renglon - 1, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(rengloncarteraobjetivorev, 6, renglon - 1, 8).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Range(renglonrecuperacionrev, 9, renglon - 1, 15).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(renglonrecuperacionrev, 9, renglon - 1, 15).Style.Border.OutsideBorderColor = XLColor.Black;

                    for (int i = 2; i <= 17; i++)
                    {
                        sheet.Column(i).Style.NumberFormat.Format = "#,##0.00";
                    }

                    sheet.Column(12).Style.NumberFormat.Format = "0.00%";
                    sheet.Column(14).Style.NumberFormat.Format = "0.00%";
                    sheet.Column(17).Style.NumberFormat.Format = "0.00%";

                    //---------------------------------------------------------------------------------------------------------------------------


                    renglon += 1;

                    var rangosub4 = sheet.Range(renglon, 1, renglon, 17);
                    rangosub4.Value = "CARTERA ESPECIAL";
                    rangosub4.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rangosub4.Style.Font.Bold = true;
                    rangosub4.Style.Font.FontSize = 12;
                    rangosub4.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rangosub4.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    rangosub4.Merge();

                    sheet.Row(renglon).Height = 20;
                    sheet.Row(renglon + 1).Height = 4;
                    sheet.Row(renglon + 2).Height = 2;

                    rangosub4 = sheet.Range(renglon + 1, 1, renglon + 1, 17);
                    rangosub4.Style.Fill.BackgroundColor = XLColor.FromHtml("#275027");

                    rangosub4 = sheet.Range(renglon + 2, 1, renglon + 2, 17);
                    rangosub4.Style.Fill.BackgroundColor = XLColor.FromHtml("#E9AE06");

                    renglon += 3;

                    sheet.Range(renglon, 1, renglon, 17).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");

                    sheet.Range(renglon, 1, renglon, 17).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");

                    sheet.Range(renglon, 2, renglon, 5).Merge().Value = "CARTERA";
                    sheet.Range(renglon, 2, renglon, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 2, renglon, 5).Style.Font.Bold = true;
                    sheet.Range(renglon, 2, renglon, 5).Style.Fill.BackgroundColor = XLColor.LightGray;
                    int rengloncarteraes = renglon;

                    sheet.Range(renglon, 6, renglon, 8).Merge().Value = "OBJETIVO";
                    sheet.Range(renglon, 6, renglon, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 6, renglon, 8).Style.Font.Bold = true;
                    sheet.Range(renglon, 6, renglon, 8).Style.Fill.BackgroundColor = XLColor.LightGray;
                    int rengloncarteraobjetivoes = renglon;

                    sheet.Range(renglon, 9, renglon, 15).Merge().Value = "RECUPERACION DE CARTERA";
                    sheet.Range(renglon, 9, renglon, 15).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 9, renglon, 15).Style.Font.Bold = true;
                    sheet.Range(renglon, 9, renglon, 15).Style.Fill.BackgroundColor = XLColor.LightGray;
                    int renglonrecuperaciones = renglon;

                    renglon++;

                    sheet.Cell(renglon, 1).Value = "PERIODO";
                    sheet.Cell(renglon, 2).Value = "ACTIVA";
                    sheet.Cell(renglon, 3).Value = "POR VENCER";
                    sheet.Cell(renglon, 4).Value = "VENCIDA";
                    sheet.Cell(renglon, 5).Value = "TOTAL";
                    sheet.Cell(renglon, 6).Value = "POR VENCER";
                    sheet.Cell(renglon, 7).Value = "VENCIDO";
                    sheet.Cell(renglon, 8).Value = "TOTAL";
                    sheet.Cell(renglon, 9).Value = "MES";
                    sheet.Cell(renglon, 10).Value = "ACTIVA";
                    sheet.Cell(renglon, 11).Value = "POR VENCER";
                    sheet.Cell(renglon, 12).Value = "%";
                    sheet.Cell(renglon, 13).Value = "VENCIDA";
                    sheet.Cell(renglon, 14).Value = "%";
                    sheet.Cell(renglon, 15).Value = "TOTAL";
                    sheet.Cell(renglon, 16).Value = "OBJETIVO RECUPERADO";
                    sheet.Cell(renglon, 17).Value = "%";

                    // Estilo para los encabezados de la tabla
                    var rango4 = sheet.Range(renglon, 1, renglon, 17);
                    rango4.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango4.Style.Font.Bold = true;
                    rango4.Style.Font.FontSize = 12;
                    rango4.RangeUsed().SetAutoFilter();
                    rango4.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango4.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    // Llenar la tabla con los datos
                    foreach (var especial in datos.especial)
                    {
                        sheet.Cell(renglon, 1).Value = especial.mes;
                        sheet.Cell(renglon, 2).Value = especial.cartera_activa;
                        sheet.Cell(renglon, 3).Value = especial.cartera_porvencer;
                        sheet.Cell(renglon, 4).Value = especial.cartera_vencida;
                        sheet.Cell(renglon, 5).Value = especial.total_cartera;
                        sheet.Cell(renglon, 6).Value = especial.objetivo_porvencer;
                        sheet.Cell(renglon, 7).Value = especial.objetivo_vencido;
                        sheet.Cell(renglon, 8).Value = especial.objetivo;
                        sheet.Cell(renglon, 9).Value = especial.recuperacion_mes;
                        sheet.Cell(renglon, 10).Value = especial.recuperacion_activa;
                        sheet.Cell(renglon, 11).Value = especial.recuperacion_porvencer;
                        sheet.Cell(renglon, 12).Value = (especial.porcporvencer / 100);
                        sheet.Cell(renglon, 13).Value = especial.recuperacion_vencida;
                        sheet.Cell(renglon, 14).Value = (especial.porcvencido / 100);
                        sheet.Cell(renglon, 15).Value = especial.total_recuperado;
                        sheet.Cell(renglon, 16).Value = especial.recuperado;
                        sheet.Cell(renglon, 17).Value = (especial.porc / 100);
                        renglon++;
                    }


                    sheet.Range(rengloncarteraes, 2, renglon - 1, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(rengloncarteraes, 2, renglon - 1, 5).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Range(rengloncarteraobjetivoes, 6, renglon - 1, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(rengloncarteraobjetivoes, 6, renglon - 1, 8).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Range(renglonrecuperaciones, 9, renglon - 1, 15).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(renglonrecuperaciones, 9, renglon - 1, 15).Style.Border.OutsideBorderColor = XLColor.Black;

                    for (int i = 2; i <= 17; i++)
                    {
                        sheet.Column(i).Style.NumberFormat.Format = "#,##0.00";
                    }

                    sheet.Column(12).Style.NumberFormat.Format = "0.00%";
                    sheet.Column(14).Style.NumberFormat.Format = "0.00%";
                    sheet.Column(17).Style.NumberFormat.Format = "0.00%";

                    //------------------------------------------------------------------------------------------------


                    renglon += 1;

                    var rangosub5 = sheet.Range(renglon, 1, renglon, 17);
                    rangosub5.Value = "CARTERA JURIDICA";
                    rangosub5.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rangosub5.Style.Font.Bold = true;
                    rangosub5.Style.Font.FontSize = 12;
                    rangosub5.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rangosub5.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    rangosub5.Merge();

                    sheet.Row(renglon).Height = 20;
                    sheet.Row(renglon + 1).Height = 4;
                    sheet.Row(renglon + 2).Height = 2;

                    rangosub5 = sheet.Range(renglon + 1, 1, renglon + 1, 17);
                    rangosub5.Style.Fill.BackgroundColor = XLColor.FromHtml("#275027");

                    rangosub5 = sheet.Range(renglon + 2, 1, renglon + 2, 17);
                    rangosub5.Style.Fill.BackgroundColor = XLColor.FromHtml("#E9AE06");

                    renglon += 3;

                    sheet.Range(renglon, 1, renglon, 17).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");

                    sheet.Range(renglon, 1, renglon, 17).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");

                    sheet.Range(renglon, 2, renglon, 5).Merge().Value = "CARTERA";
                    sheet.Range(renglon, 2, renglon, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 2, renglon, 5).Style.Font.Bold = true;
                    sheet.Range(renglon, 2, renglon, 5).Style.Fill.BackgroundColor = XLColor.LightGray;
                    int rengloncarteraju = renglon;

                    sheet.Range(renglon, 6, renglon, 8).Merge().Value = "OBJETIVO";
                    sheet.Range(renglon, 6, renglon, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 6, renglon, 8).Style.Font.Bold = true;
                    sheet.Range(renglon, 6, renglon, 8).Style.Fill.BackgroundColor = XLColor.LightGray;
                    int rengloncarteraobjetivoju = renglon;

                    sheet.Range(renglon, 9, renglon, 15).Merge().Value = "RECUPERACION DE CARTERA";
                    sheet.Range(renglon, 9, renglon, 15).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 9, renglon, 15).Style.Font.Bold = true;
                    sheet.Range(renglon, 9, renglon, 15).Style.Fill.BackgroundColor = XLColor.LightGray;
                    int renglonrecuperacionju = renglon;

                    renglon++;

                    sheet.Cell(renglon, 1).Value = "PERIODO";
                    sheet.Cell(renglon, 2).Value = "ACTIVA";
                    sheet.Cell(renglon, 3).Value = "POR VENCER";
                    sheet.Cell(renglon, 4).Value = "VENCIDA";
                    sheet.Cell(renglon, 5).Value = "TOTAL";
                    sheet.Cell(renglon, 6).Value = "POR VENCER";
                    sheet.Cell(renglon, 7).Value = "VENCIDO";
                    sheet.Cell(renglon, 8).Value = "TOTAL";
                    sheet.Cell(renglon, 9).Value = "MES";
                    sheet.Cell(renglon, 10).Value = "ACTIVA";
                    sheet.Cell(renglon, 11).Value = "POR VENCER";
                    sheet.Cell(renglon, 12).Value = "%";
                    sheet.Cell(renglon, 13).Value = "VENCIDA";
                    sheet.Cell(renglon, 14).Value = "%";
                    sheet.Cell(renglon, 15).Value = "TOTAL";
                    sheet.Cell(renglon, 16).Value = "OBJETIVO RECUPERADO";
                    sheet.Cell(renglon, 17).Value = "%";

                    // Estilo para los encabezados de la tabla
                    var rango5 = sheet.Range(renglon, 1, renglon, 17);
                    rango5.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango5.Style.Font.Bold = true;
                    rango5.Style.Font.FontSize = 12;
                    rango5.RangeUsed().SetAutoFilter();
                    rango5.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango5.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    // Llenar la tabla con los datos
                    foreach (var juridico in datos.juridico)
                    {
                        sheet.Cell(renglon, 1).Value = juridico.mes;
                        sheet.Cell(renglon, 2).Value = juridico.cartera_activa;
                        sheet.Cell(renglon, 3).Value = juridico.cartera_porvencer;
                        sheet.Cell(renglon, 4).Value = juridico.cartera_vencida;
                        sheet.Cell(renglon, 5).Value = juridico.total_cartera;
                        sheet.Cell(renglon, 6).Value = juridico.objetivo_porvencer;
                        sheet.Cell(renglon, 7).Value = juridico.objetivo_vencido;
                        sheet.Cell(renglon, 8).Value = juridico.objetivo;
                        sheet.Cell(renglon, 9).Value = juridico.recuperacion_mes;
                        sheet.Cell(renglon, 10).Value = juridico.recuperacion_activa;
                        sheet.Cell(renglon, 11).Value = juridico.recuperacion_porvencer;
                        sheet.Cell(renglon, 12).Value = (juridico.porcporvencer / 100);
                        sheet.Cell(renglon, 13).Value = juridico.recuperacion_vencida;
                        sheet.Cell(renglon, 14).Value = (juridico.porcvencido / 100);
                        sheet.Cell(renglon, 15).Value = juridico.total_recuperado;
                        sheet.Cell(renglon, 16).Value = juridico.recuperado;
                        sheet.Cell(renglon, 17).Value = (juridico.porc / 100);
                        renglon++;
                    }


                    sheet.Range(rengloncarteraju, 2, renglon - 1, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(rengloncarteraju, 2, renglon - 1, 5).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Range(rengloncarteraobjetivoju, 6, renglon - 1, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(rengloncarteraobjetivoju, 6, renglon - 1, 8).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Range(renglonrecuperacionju, 9, renglon - 1, 15).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(renglonrecuperacionju, 9, renglon - 1, 15).Style.Border.OutsideBorderColor = XLColor.Black;

                    for (int i = 2; i <= 17; i++)
                    {
                        sheet.Column(i).Style.NumberFormat.Format = "#,##0.00";
                    }

                    sheet.Column(12).Style.NumberFormat.Format = "0.00%";
                    sheet.Column(14).Style.NumberFormat.Format = "0.00%";
                    sheet.Column(17).Style.NumberFormat.Format = "0.00%";

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
