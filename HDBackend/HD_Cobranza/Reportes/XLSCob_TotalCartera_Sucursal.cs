using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Reportes
{
    public static class XLSCob_TotalCartera_Sucursal
    {
        public static Task<DocResult> CrearResumenPorSucursal(IEnumerable<mdlCob_TotalCarteraPorSucursal> lista)
        {
            try
            {
                string sheetname = "RESUMEN CARTERA POR SUCURSAL";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Arial";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, $"RESUMEN DE CARTERA POR SUCURSAL", 14);

                    sheet.Cell(renglon, 1).Value = "#";
                    sheet.Cell(renglon, 2).Value = "Sucursal";
                    sheet.Cell(renglon, 3).Value = "Más de 90";
                    sheet.Cell(renglon, 4).Value = "Más de 60";
                    sheet.Cell(renglon, 5).Value = "Más de 30";
                    sheet.Cell(renglon, 6).Value = "Más de 15";
                    sheet.Cell(renglon, 7).Value = "De 1 a 15";
                    sheet.Cell(renglon, 8).Value = "Total Vencido";
                    sheet.Cell(renglon, 9).Value = "Por Vencer";
                    sheet.Cell(renglon, 10).Value = "Total Cartera";
                    sheet.Cell(renglon, 11).Value = "Saldo a Favor";
                    sheet.Cell(renglon, 12).Value = "Total";
                    sheet.Cell(renglon, 13).Value = "% Vencido";
                    sheet.Cell(renglon, 14).Value = "% Por Vencer";

                    var rango = sheet.Range(renglon, 1, renglon, 14);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    foreach (mdlCob_TotalCarteraPorSucursal activos in lista.Where(x=> x.idsucursal!=13))
                    {
                        sheet.Cell(renglon, 1).Value = activos.idsucursal;
                        sheet.Cell(renglon, 2).Value = activos.sucursal;
                        sheet.Cell(renglon, 3).Value = activos.mas90;
                        sheet.Cell(renglon, 4).Value = activos.mas60;
                        sheet.Cell(renglon, 5).Value = activos.mas30;
                        sheet.Cell(renglon, 6).Value = activos.mas15;
                        sheet.Cell(renglon, 7).Value = activos.de1a15;
                        sheet.Cell(renglon, 8).Value = activos.vencido;
                        sheet.Cell(renglon, 9).Value = activos.porvencer;
                        sheet.Cell(renglon, 10).Value = activos.totalcartera;
                        sheet.Cell(renglon, 11).Value = activos.saldoafavor;
                        sheet.Cell(renglon, 12).Value = activos.total;
                        sheet.Cell(renglon, 13).Value = Math.Round(activos.vencido / activos.totalcartera, 2);
                        sheet.Cell(renglon, 14).Value = Math.Round(activos.porvencer / activos.totalcartera, 2);
                        renglon++;
                    }

                    sheet.Cell(renglon, 2).Value = "TOTALES";
                    sheet.Cell(renglon, 3).FormulaA1 = $"SUBTOTAL(9,C5:C{renglon - 1})";
                    sheet.Cell(renglon, 4).FormulaA1 = $"SUBTOTAL(9,D5:D{renglon - 1})";
                    sheet.Cell(renglon, 5).FormulaA1 = $"SUBTOTAL(9,E5:E{renglon - 1})";
                    sheet.Cell(renglon, 6).FormulaA1 = $"SUBTOTAL(9,F5:F{renglon - 1})";
                    sheet.Cell(renglon, 7).FormulaA1 = $"SUBTOTAL(9,G5:G{renglon - 1})";
                    sheet.Cell(renglon, 8).FormulaA1 = $"SUBTOTAL(9,H5:H{renglon - 1})";
                    sheet.Cell(renglon, 9).FormulaA1 = $"SUBTOTAL(9,I5:I{renglon - 1})";
                    sheet.Cell(renglon, 10).FormulaA1 = $"SUBTOTAL(9,J5:J{renglon - 1})";
                    sheet.Cell(renglon, 11).FormulaA1 = $"SUBTOTAL(9,K5:K{renglon - 1})";
                    sheet.Cell(renglon, 12).FormulaA1 = $"SUBTOTAL(9,L5:L{renglon - 1})";
                    sheet.Cell(renglon, 13).FormulaA1 = $"H{renglon}/J{renglon}";
                    sheet.Cell(renglon, 14).FormulaA1 = $"I{renglon}/J{renglon}";

                    sheet.Column(3).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(4).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(5).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(7).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(8).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(9).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(10).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(11).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(12).Style.NumberFormat.Format = "#,##0.00";

                    sheet.Column(13).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(14).Style.NumberFormat.Format = "0.0 %";

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
