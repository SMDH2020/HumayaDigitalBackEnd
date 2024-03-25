using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;
using HD_Cobranza.Reportes;
using HD_Cobranza;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD_Finanzas.Modelos.Gastos_Proyeccion;

namespace HD_Reporteria.Finanzas.Excel
{
    public class XLS_Gastos
    {
        public static Task<DocResult> Gastos(Fmdl_Gastos_PDF lista)
        {
            try
            {
                string sheetname = "Gastos de Operación";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, $"Gastos de Operación por concepto", 8);

                    sheet.Cell(renglon, 1).Value = "CONCEPTO";
                    sheet.Cell(renglon, 2).Value = "REAL";
                    sheet.Cell(renglon, 3).Value = "PROYECCION";
                    sheet.Cell(renglon, 4).Value = "%";
                    sheet.Cell(renglon, 5).Value = "DIF";
                    sheet.Cell(renglon, 6).Value = "REAL";
                    sheet.Cell(renglon, 7).Value = "%";
                    sheet.Cell(renglon, 8).Value = "%";
                    sheet.Cell(renglon, 9).Value = "DIF";

                    var rango = sheet.Range(renglon, 1, renglon, 8);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    foreach (var mdl in lista.data)
                    {
                        sheet.Cell(renglon, 1).Value = mdl.concepto;
                        sheet.Cell(renglon, 2).Value = mdl.total;
                        sheet.Cell(renglon, 3).Value = mdl.proyeccion;
                        sheet.Cell(renglon, 4).Value = mdl.porc;
                        sheet.Cell(renglon, 5).Value = mdl.dif;
                        sheet.Cell(renglon, 6).Value = mdl.oldtotal;
                        sheet.Cell(renglon, 7).Value = mdl.oldporc;
                        sheet.Cell(renglon, 8).Value = mdl.olddif;
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
                    sheet.Cell(renglon, 14).FormulaA1 = $"SUBTOTAL(9,N5:N{renglon - 1})";
                    sheet.Cell(renglon, 15).FormulaA1 = $"SUBTOTAL(9,O5:O{renglon - 1})";
                    sheet.Cell(renglon, 16).FormulaA1 = $"SUBTOTAL(9,P5:P{renglon - 1})";
                    sheet.Cell(renglon, 17).FormulaA1 = $"SUBTOTAL(9,Q5:Q{renglon - 1})";
                    sheet.Cell(renglon, 18).FormulaA1 = $"SUBTOTAL(9,R5:R{renglon - 1})";

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
                    sheet.Column(14).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(15).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(16).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(17).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(18).Style.NumberFormat.Format = "#,##0.00";

                    rango = sheet.Range(renglon, 1, renglon, 18);
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
