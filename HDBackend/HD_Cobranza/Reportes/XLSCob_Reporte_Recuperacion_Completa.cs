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
        private static void SetValorConPorcentaje(IXLCell cell, double? valor, double? porcentaje)
        {
            cell.Style.Alignment.WrapText = true;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

            var rt = cell.CreateRichText();
            rt.AddText((valor ?? 0).ToString("#,##0.00")).SetFontSize(10);

            if (porcentaje.HasValue)
            {
                rt.AddNewLine();
                rt.AddText($"{porcentaje.Value:0.00} %")
                  .SetFontSize(8)
                  .SetFontColor(XLColor.Gray);
            }
        }

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

                    var rango = sheet.Range(renglon, 1, renglon, 17);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    foreach (var total in datos.total)
                    {
                        sheet.Cell(renglon, 1).Value = total.mes;

                        SetValorConPorcentaje(sheet.Cell(renglon, 2), total.cartera_activa, total.porc_cartera_activa);
                        SetValorConPorcentaje(sheet.Cell(renglon, 3), total.cartera_porvencer, total.porc_cartera_porvencer);
                        SetValorConPorcentaje(sheet.Cell(renglon, 4), total.cartera_vencida, total.porc_cartera_vencida);
                        SetValorConPorcentaje(sheet.Cell(renglon, 5), total.total_cartera, 100);

                        SetValorConPorcentaje(sheet.Cell(renglon, 6), total.objetivo_porvencer, total.porc_objetivo_porvencer);
                        SetValorConPorcentaje(sheet.Cell(renglon, 7), total.objetivo_vencido, total.porc_objetivo_vencido);
                        SetValorConPorcentaje(sheet.Cell(renglon, 8), total.objetivo, 100);

                        SetValorConPorcentaje(sheet.Cell(renglon, 9), total.recuperacion_mes, total.porc_recuperacion_mes);
                        SetValorConPorcentaje(sheet.Cell(renglon, 10), total.recuperacion_activa, total.porc_recuperacion_activa);
                        SetValorConPorcentaje(sheet.Cell(renglon, 11), total.recuperacion_porvencer, total.porc_recuperacion_porvencer);

                        sheet.Cell(renglon, 12).Value = (total.porcporvencer / 100);

                        SetValorConPorcentaje(sheet.Cell(renglon, 13), total.recuperacion_vencida, total.porc_recuperacion_vencida);

                        sheet.Cell(renglon, 14).Value = (total.porcvencido / 100);

                        SetValorConPorcentaje(sheet.Cell(renglon, 15), total.total_recuperado, 100);

                        sheet.Cell(renglon, 16).Value = total.recuperado;
                        sheet.Cell(renglon, 17).Value = (total.porc / 100);

                        sheet.Row(renglon).Height = 28;

                        renglon++;
                    }

                    sheet.Range(rengloncarteratot, 2, renglon - 1, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(rengloncarteratot, 2, renglon - 1, 5).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Range(rengloncarteratot, 6, renglon - 1, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(rengloncarteratot, 6, renglon - 1, 8).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Range(renglonrecuperaciontot, 9, renglon - 1, 15).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(renglonrecuperaciontot, 9, renglon - 1, 15).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Column(12).Style.NumberFormat.Format = "0.00%";
                    sheet.Column(14).Style.NumberFormat.Format = "0.00%";
                    sheet.Column(16).Style.NumberFormat.Format = "#,##0.00";
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

                    var rango2 = sheet.Range(renglon, 1, renglon, 17);
                    rango2.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango2.Style.Font.Bold = true;
                    rango2.Style.Font.FontSize = 12;
                    rango2.RangeUsed().SetAutoFilter();
                    rango2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango2.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    foreach (var operacion in datos.operacion)
                    {
                        sheet.Cell(renglon, 1).Value = operacion.mes;

                        SetValorConPorcentaje(sheet.Cell(renglon, 2), operacion.cartera_activa, operacion.porc_cartera_activa);
                        SetValorConPorcentaje(sheet.Cell(renglon, 3), operacion.cartera_porvencer, operacion.porc_cartera_porvencer);
                        SetValorConPorcentaje(sheet.Cell(renglon, 4), operacion.cartera_vencida, operacion.porc_cartera_vencida);
                        SetValorConPorcentaje(sheet.Cell(renglon, 5), operacion.total_cartera, 100);

                        SetValorConPorcentaje(sheet.Cell(renglon, 6), operacion.objetivo_porvencer, operacion.porc_objetivo_porvencer);
                        SetValorConPorcentaje(sheet.Cell(renglon, 7), operacion.objetivo_vencido, operacion.porc_objetivo_vencido);
                        SetValorConPorcentaje(sheet.Cell(renglon, 8), operacion.objetivo, 100);

                        SetValorConPorcentaje(sheet.Cell(renglon, 9), operacion.recuperacion_mes, operacion.porc_recuperacion_mes);
                        SetValorConPorcentaje(sheet.Cell(renglon, 10), operacion.recuperacion_activa, operacion.porc_recuperacion_activa);
                        SetValorConPorcentaje(sheet.Cell(renglon, 11), operacion.recuperacion_porvencer, operacion.porc_recuperacion_porvencer);

                        sheet.Cell(renglon, 12).Value = (operacion.porcporvencer / 100);

                        SetValorConPorcentaje(sheet.Cell(renglon, 13), operacion.recuperacion_vencida, operacion.porc_recuperacion_vencida);

                        sheet.Cell(renglon, 14).Value = (operacion.porcvencido / 100);

                        SetValorConPorcentaje(sheet.Cell(renglon, 15), operacion.total_recuperado, 100);

                        sheet.Cell(renglon, 16).Value = operacion.recuperado;
                        sheet.Cell(renglon, 17).Value = (operacion.porc / 100);

                        sheet.Row(renglon).Height = 28;

                        renglon++;
                    }

                    sheet.Range(rengloncarteraop, 2, renglon - 1, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(rengloncarteraop, 2, renglon - 1, 5).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Range(rengloncarteraobjetivoop, 6, renglon - 1, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(rengloncarteraobjetivoop, 6, renglon - 1, 8).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Range(renglonrecuperacionop, 9, renglon - 1, 15).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(renglonrecuperacionop, 9, renglon - 1, 15).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Column(12).Style.NumberFormat.Format = "0.00%";
                    sheet.Column(14).Style.NumberFormat.Format = "0.00%";
                    sheet.Column(16).Style.NumberFormat.Format = "#,##0.00";
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

                    var rango3 = sheet.Range(renglon, 1, renglon, 17);
                    rango3.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango3.Style.Font.Bold = true;
                    rango3.Style.Font.FontSize = 12;
                    rango3.RangeUsed().SetAutoFilter();
                    rango3.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango3.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    foreach (var revolvente in datos.revolvente)
                    {
                        sheet.Cell(renglon, 1).Value = revolvente.mes;

                        SetValorConPorcentaje(sheet.Cell(renglon, 2), revolvente.cartera_activa, revolvente.porc_cartera_activa);
                        SetValorConPorcentaje(sheet.Cell(renglon, 3), revolvente.cartera_porvencer, revolvente.porc_cartera_porvencer);
                        SetValorConPorcentaje(sheet.Cell(renglon, 4), revolvente.cartera_vencida, revolvente.porc_cartera_vencida);
                        SetValorConPorcentaje(sheet.Cell(renglon, 5), revolvente.total_cartera, 100);

                        SetValorConPorcentaje(sheet.Cell(renglon, 6), revolvente.objetivo_porvencer, revolvente.porc_objetivo_porvencer);
                        SetValorConPorcentaje(sheet.Cell(renglon, 7), revolvente.objetivo_vencido, revolvente.porc_objetivo_vencido);
                        SetValorConPorcentaje(sheet.Cell(renglon, 8), revolvente.objetivo, 100);

                        SetValorConPorcentaje(sheet.Cell(renglon, 9), revolvente.recuperacion_mes, revolvente.porc_recuperacion_mes);
                        SetValorConPorcentaje(sheet.Cell(renglon, 10), revolvente.recuperacion_activa, revolvente.porc_recuperacion_activa);
                        SetValorConPorcentaje(sheet.Cell(renglon, 11), revolvente.recuperacion_porvencer, revolvente.porc_recuperacion_porvencer);

                        sheet.Cell(renglon, 12).Value = (revolvente.porcporvencer / 100);

                        SetValorConPorcentaje(sheet.Cell(renglon, 13), revolvente.recuperacion_vencida, revolvente.porc_recuperacion_vencida);

                        sheet.Cell(renglon, 14).Value = (revolvente.porcvencido / 100);

                        SetValorConPorcentaje(sheet.Cell(renglon, 15), revolvente.total_recuperado, 100);

                        sheet.Cell(renglon, 16).Value = revolvente.recuperado;
                        sheet.Cell(renglon, 17).Value = (revolvente.porc / 100);

                        sheet.Row(renglon).Height = 28;

                        renglon++;
                    }

                    sheet.Range(rengloncarterarev, 2, renglon - 1, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(rengloncarterarev, 2, renglon - 1, 5).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Range(rengloncarteraobjetivorev, 6, renglon - 1, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(rengloncarteraobjetivorev, 6, renglon - 1, 8).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Range(renglonrecuperacionrev, 9, renglon - 1, 15).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(renglonrecuperacionrev, 9, renglon - 1, 15).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Column(12).Style.NumberFormat.Format = "0.00%";
                    sheet.Column(14).Style.NumberFormat.Format = "0.00%";
                    sheet.Column(16).Style.NumberFormat.Format = "#,##0.00";
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

                    var rango4 = sheet.Range(renglon, 1, renglon, 17);
                    rango4.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango4.Style.Font.Bold = true;
                    rango4.Style.Font.FontSize = 12;
                    rango4.RangeUsed().SetAutoFilter();
                    rango4.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango4.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    foreach (var especial in datos.especial)
                    {
                        sheet.Cell(renglon, 1).Value = especial.mes;

                        SetValorConPorcentaje(sheet.Cell(renglon, 2), especial.cartera_activa, especial.porc_cartera_activa);
                        SetValorConPorcentaje(sheet.Cell(renglon, 3), especial.cartera_porvencer, especial.porc_cartera_porvencer);
                        SetValorConPorcentaje(sheet.Cell(renglon, 4), especial.cartera_vencida, especial.porc_cartera_vencida);
                        SetValorConPorcentaje(sheet.Cell(renglon, 5), especial.total_cartera, 100);

                        SetValorConPorcentaje(sheet.Cell(renglon, 6), especial.objetivo_porvencer, especial.porc_objetivo_porvencer);
                        SetValorConPorcentaje(sheet.Cell(renglon, 7), especial.objetivo_vencido, especial.porc_objetivo_vencido);
                        SetValorConPorcentaje(sheet.Cell(renglon, 8), especial.objetivo, 100);

                        SetValorConPorcentaje(sheet.Cell(renglon, 9), especial.recuperacion_mes, especial.porc_recuperacion_mes);
                        SetValorConPorcentaje(sheet.Cell(renglon, 10), especial.recuperacion_activa, especial.porc_recuperacion_activa);
                        SetValorConPorcentaje(sheet.Cell(renglon, 11), especial.recuperacion_porvencer, especial.porc_recuperacion_porvencer);

                        sheet.Cell(renglon, 12).Value = (especial.porcporvencer / 100);

                        SetValorConPorcentaje(sheet.Cell(renglon, 13), especial.recuperacion_vencida, especial.porc_recuperacion_vencida);

                        sheet.Cell(renglon, 14).Value = (especial.porcvencido / 100);

                        SetValorConPorcentaje(sheet.Cell(renglon, 15), especial.total_recuperado, 100);

                        sheet.Cell(renglon, 16).Value = especial.recuperado;
                        sheet.Cell(renglon, 17).Value = (especial.porc / 100);

                        sheet.Row(renglon).Height = 28;

                        renglon++;
                    }

                    sheet.Range(rengloncarteraes, 2, renglon - 1, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(rengloncarteraes, 2, renglon - 1, 5).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Range(rengloncarteraobjetivoes, 6, renglon - 1, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(rengloncarteraobjetivoes, 6, renglon - 1, 8).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Range(renglonrecuperaciones, 9, renglon - 1, 15).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(renglonrecuperaciones, 9, renglon - 1, 15).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Column(12).Style.NumberFormat.Format = "0.00%";
                    sheet.Column(14).Style.NumberFormat.Format = "0.00%";
                    sheet.Column(16).Style.NumberFormat.Format = "#,##0.00";
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

                    var rango5 = sheet.Range(renglon, 1, renglon, 17);
                    rango5.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango5.Style.Font.Bold = true;
                    rango5.Style.Font.FontSize = 12;
                    rango5.RangeUsed().SetAutoFilter();
                    rango5.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango5.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    foreach (var juridico in datos.juridico)
                    {
                        sheet.Cell(renglon, 1).Value = juridico.mes;

                        SetValorConPorcentaje(sheet.Cell(renglon, 2), juridico.cartera_activa, juridico.porc_cartera_activa);
                        SetValorConPorcentaje(sheet.Cell(renglon, 3), juridico.cartera_porvencer, juridico.porc_cartera_porvencer);
                        SetValorConPorcentaje(sheet.Cell(renglon, 4), juridico.cartera_vencida, juridico.porc_cartera_vencida);
                        SetValorConPorcentaje(sheet.Cell(renglon, 5), juridico.total_cartera, 100);

                        SetValorConPorcentaje(sheet.Cell(renglon, 6), juridico.objetivo_porvencer, juridico.porc_objetivo_porvencer);
                        SetValorConPorcentaje(sheet.Cell(renglon, 7), juridico.objetivo_vencido, juridico.porc_objetivo_vencido);
                        SetValorConPorcentaje(sheet.Cell(renglon, 8), juridico.objetivo, 100);

                        SetValorConPorcentaje(sheet.Cell(renglon, 9), juridico.recuperacion_mes, juridico.porc_recuperacion_mes);
                        SetValorConPorcentaje(sheet.Cell(renglon, 10), juridico.recuperacion_activa, juridico.porc_recuperacion_activa);
                        SetValorConPorcentaje(sheet.Cell(renglon, 11), juridico.recuperacion_porvencer, juridico.porc_recuperacion_porvencer);

                        sheet.Cell(renglon, 12).Value = (juridico.porcporvencer / 100);

                        SetValorConPorcentaje(sheet.Cell(renglon, 13), juridico.recuperacion_vencida, juridico.porc_recuperacion_vencida);

                        sheet.Cell(renglon, 14).Value = (juridico.porcvencido / 100);

                        SetValorConPorcentaje(sheet.Cell(renglon, 15), juridico.total_recuperado, 100);

                        sheet.Cell(renglon, 16).Value = juridico.recuperado;
                        sheet.Cell(renglon, 17).Value = (juridico.porc / 100);

                        sheet.Row(renglon).Height = 28;

                        renglon++;
                    }

                    sheet.Range(rengloncarteraju, 2, renglon - 1, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(rengloncarteraju, 2, renglon - 1, 5).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Range(rengloncarteraobjetivoju, 6, renglon - 1, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(rengloncarteraobjetivoju, 6, renglon - 1, 8).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Range(renglonrecuperacionju, 9, renglon - 1, 15).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(renglonrecuperacionju, 9, renglon - 1, 15).Style.Border.OutsideBorderColor = XLColor.Black;

                    sheet.Column(12).Style.NumberFormat.Format = "0.00%";
                    sheet.Column(14).Style.NumberFormat.Format = "0.00%";
                    sheet.Column(16).Style.NumberFormat.Format = "#,##0.00";
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