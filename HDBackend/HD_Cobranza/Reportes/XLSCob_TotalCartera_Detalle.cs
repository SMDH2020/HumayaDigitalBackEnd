using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;

namespace HD_Cobranza.Reportes
{
    public class XLSCob_TotalCartera_Detalle
    {
        public static Task<DocResult> CrearExcel(IEnumerable<mdlCob_TotalCartera_Detalle> list)
        {
            try
            {
                string sheetname = "Facturas por vencer";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Arial";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, $"RESUMEN DE CARTERA DETALLE", 13);

                    sheet.Cell(renglon, 1).Value = "IDCLIENTE";
                    sheet.Cell(renglon, 2).Value = "RAZON SOCIAL";
                    sheet.Cell(renglon, 3).Value = "TOTAL CARTERA";
                    sheet.Cell(renglon, 4).Value = "SALDO A FAVOR";
                    sheet.Cell(renglon, 5).Value = "TOTAL";
                    sheet.Cell(renglon, 6).Value = "JURIDICO";
                    sheet.Cell(renglon, 7).Value = "%";
                    sheet.Cell(renglon, 8).Value = "CARTERA ACTIVA";
                    sheet.Cell(renglon, 9).Value = "%";
                    sheet.Cell(renglon, 10).Value = "POR VENCER";
                    sheet.Cell(renglon, 11).Value = "%";
                    sheet.Cell(renglon, 12).Value = "VENCIDA";
                    sheet.Cell(renglon, 13).Value = "%";

                    var rango = sheet.Range(renglon, 1, renglon, 13);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    foreach (mdlCob_TotalCartera_Detalle activos in list)
                    { 
                        
                        double totalcartera = activos.totalcartera + activos.juridico;

                        sheet.Cell(renglon, 1).Value = activos.idcliente;
                        sheet.Cell(renglon, 2).Value = activos.razonsocial;
                        sheet.Cell(renglon, 3).Value = activos.totalcartera + activos.juridico;
                        sheet.Cell(renglon, 4).Value = activos.saldoafavor;
                        sheet.Cell(renglon, 5).Value = activos.total + activos.juridico;
                        sheet.Cell(renglon, 6).Value = activos.juridico;
                        sheet.Cell(renglon, 7).Value =  activos.juridico ==0 || (activos.totalcartera + activos.juridico)==0 ? 0: activos.juridico/totalcartera ;
                        sheet.Cell(renglon, 8).Value = activos.activo;
                        sheet.Cell(renglon, 9).Value = activos.activo ==0 || (activos.totalcartera + activos.juridico)==0 ? 0: activos.activo/totalcartera;
                        sheet.Cell(renglon, 10).Value = activos.porvencer;
                        sheet.Cell(renglon, 11).Value = activos.porvencer == 0 || (activos.totalcartera + activos.juridico) == 0 ? 0 : activos.porvencer / totalcartera;
                        sheet.Cell(renglon, 12).Value = activos.vencido;
                        sheet.Cell(renglon, 13).Value = activos.vencido == 0 || (activos.totalcartera + activos.juridico) == 0 ? 0 : activos.vencido/totalcartera;
                        renglon++;
                    }

                    sheet.Cell(renglon, 2).Value = "TOTALES";
                    sheet.Cell(renglon, 3).FormulaA1 = $"SUBTOTAL(9,C5:C{renglon - 1})";
                    sheet.Cell(renglon, 4).FormulaA1 = $"SUBTOTAL(9,D5:D{renglon - 1})";
                    sheet.Cell(renglon, 5).FormulaA1 = $"SUBTOTAL(9,E5:E{renglon - 1})";
                    sheet.Cell(renglon, 6).FormulaA1 = $"SUBTOTAL(9,F5:F{renglon - 1})";
                    sheet.Cell(renglon, 7).FormulaA1 = $"=C{renglon}/F{renglon}/100";
                    sheet.Cell(renglon, 8).FormulaA1 = $"SUBTOTAL(9,H5:H{renglon - 1})";
                    sheet.Cell(renglon, 9).FormulaA1 = $"=C{renglon}/H{renglon}/100";
                    sheet.Cell(renglon, 10).FormulaA1 = $"SUBTOTAL(9,J5:J{renglon - 1})";
                    sheet.Cell(renglon, 11).FormulaA1 = $"=C{renglon}/J{renglon}/100";
                    sheet.Cell(renglon, 12).FormulaA1 = $"SUBTOTAL(9,L5:L{renglon - 1})";
                    sheet.Cell(renglon, 13).FormulaA1 = $"=C{renglon}/L{renglon}/100";

                    sheet.Column(3).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(4).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(5).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(7).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(8).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(9).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(10).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(11).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(12).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(13).Style.NumberFormat.Format = "0.0 %";

                    rango = sheet.Range(renglon, 1, renglon, 13);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#e5e6e6");
                    rango.Style.Font.Bold = true;

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
                        filename = "RESUMEN DE CARTERA DETALLE"
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

