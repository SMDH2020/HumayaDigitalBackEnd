using ClosedXML.Excel;
using HD.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD_Finanzas.Modelos.AntiguedadInventario;
using HD_Cobranza;

namespace HD_Reporteria.Finanzas.Excel
{
    public class XLS_Antiguedad_Inventario
    {
        public static Task<DocResult> CrearExel(mdl_Inventario_Antiguedad_View result, mdl_vInventario vm, string usuario)
        {
            try
            {
                string ruta = $"C:\\SMDH\\Procesados\\PROYECCIONVENTASVSREAL{usuario}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add("Inventario");


                    sheet.Column(1).Width = 12;
                    sheet.Column(2).Width = 28;
                    sheet.Column(3).Width = 22;
                    sheet.Column(4).Width = 50;
                    sheet.Column(5).Width = 13;
                    sheet.Column(6).Width = 13;
                    sheet.Column(7).Width = 13;
                    sheet.Column(8).Width = 13;

                    var cell = sheet.Range(1, 1, 1, 8);
                    cell.Value = "ANTIGUEDAD DEL INVENTARIO";
                    cell.Style.Font.Bold = true;
                    cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Style.Font.FontSize = 16;
                    cell.Merge();

                    cell = sheet.Range(2, 1, 2, 8);
                    cell.Value = $"ACTUALIZADO EL {result.InvAntiguedadInfo.fecha} POR {result.InvAntiguedadInfo.usuario}";
                    cell.Style.Font.FontSize = 12;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Merge();


                    cell = sheet.Range(3, 1, 3, 8);
                    cell.Value = vm.arraysucursales.ToUpper();
                    cell.Style.Font.FontSize = 12;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Merge();

                    int row = 4;
                    sheet.Cell(row, 1).Value = "SUCURSAL";
                    sheet.Cell(row, 2).Value = "LINEA";
                    sheet.Cell(row, 3).Value = "MODELO";
                    sheet.Cell(row, 4).Value = "DESCRIPCIÓN";
                    sheet.Cell(row, 5).Value = "NUM. E";
                    sheet.Cell(row, 6).Value = "DIAS";
                    sheet.Cell(row, 7).Value = "MES";
                    sheet.Cell(row, 8).Value = "COSTO";
                    sheet.Row(row).AdjustToContents();
                    cell = sheet.Range(row, 1, row, 8);
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.FontSize = 12;
                    cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Style.Fill.BackgroundColor = XLColor.FromArgb(188, 210, 138);

                    var lineas = result.InvAntiguedad.GroupBy(x => x.familia).Select(x => x.Key).ToList();
                    row += 1;
                    foreach (string fam in lineas)
                    {
                        cell = sheet.Range(row, 1, row, 8);
                        cell.Value = fam;
                        cell.Merge();
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.FontSize = 16;
                        cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        cell.Style.Fill.BackgroundColor = XLColor.FromArgb(218, 230, 190);
                        sheet.Row(row).Height = 25;

                        row += 1;

                        var detalle = result.InvAntiguedad.Where(x => x.familia.Equals(fam)).ToList()
                            .OrderByDescending(x => x.antiguedaddias);
                        foreach (mdl_Inventario_Antiguedad item in detalle)
                        {
                            sheet.Cell(row, 1).Value = item.sucursal;
                            sheet.Cell(row, 2).Value = item.descfamilia;
                            sheet.Cell(row, 3).Value = item.modelo;
                            sheet.Cell(row, 4).Value = item.nombremodelo;
                            sheet.Cell(row, 5).Value = item.neconomico;
                            sheet.Cell(row, 6).Value = item.antiguedaddias;
                            sheet.Cell(row, 7).Value = item.nummonth;
                            sheet.Cell(row, 8).Value = item.costo;
                            row += 1;
                        }
                        cell = sheet.Range(row, 1, row, 5);
                        cell.Value = $"TOTAL {fam}";
                        cell.Merge();
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.FontSize = 12;
                        cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        cell.Style.Fill.BackgroundColor = XLColor.FromArgb(218, 230, 190);

                        cell = sheet.Range(row, 6, row, 6);
                        cell.Value = detalle.Average(x => x.antiguedaddias);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.FontSize = 12;
                        cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        cell.Style.Fill.BackgroundColor = XLColor.FromArgb(218, 230, 190);

                        cell = sheet.Range(row, 7, row, 7);
                        cell.Value = detalle.Average(x => x.nummonth);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.FontSize = 12;
                        cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        cell.Style.Fill.BackgroundColor = XLColor.FromArgb(218, 230, 190);

                        cell = sheet.Range(row, 8, row, 8);
                        cell.Value = Math.Round(detalle.Sum(x => x.costo), 0);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.FontSize = 12;
                        cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        cell.Style.Fill.BackgroundColor = XLColor.FromArgb(218, 230, 190);
                        row += 3;
                    }


                    cell = sheet.Range(row, 1, row, 5);
                    cell.Value = $"TOTAL INVENTARIO";
                    cell.Merge();
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.FontSize = 14;
                    cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    cell.Style.Fill.BackgroundColor = XLColor.FromArgb(218, 230, 190);

                    cell = sheet.Range(row, 6, row, 6);
                    cell.Value = result.InvAntiguedad.Average(x => x.antiguedaddias);
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.FontSize = 14;
                    cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Style.Fill.BackgroundColor = XLColor.FromArgb(218, 230, 190);

                    cell = sheet.Range(row, 7, row, 7);
                    cell.Value = result.InvAntiguedad.Average(x => x.nummonth);
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.FontSize = 14;
                    cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Style.Fill.BackgroundColor = XLColor.FromArgb(218, 230, 190);

                    cell = sheet.Range(row, 8, row, 8);
                    cell.Value = Math.Round(result.InvAntiguedad.Sum(x => x.costo), 0);
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.FontSize = 14;
                    cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Style.Fill.BackgroundColor = XLColor.FromArgb(218, 230, 190);

                    sheet.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Column(6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Column(7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Column(6).Style.NumberFormat.Format = "#,##0";
                    sheet.Column(7).Style.NumberFormat.Format = "#,##0";
                    sheet.Column(8).Style.NumberFormat.Format = "#,##0";
                    sheet.Rows().AdjustToContents();
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
                        filename = $"ANTIGUEDAD DEL INVENTARIO"
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
