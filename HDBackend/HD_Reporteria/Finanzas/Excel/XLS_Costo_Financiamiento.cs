using ClosedXML.Excel;
using HD.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD_Finanzas.Modelos.CostoFinanciamiento;
using HD_Cobranza;

namespace HD_Reporteria.Finanzas.Excel
{
    public class XLS_Costo_Financiamiento
    {
        public static Task<DocResult> CrearExel(IEnumerable<mdl_Costo_Financiamiento> cif, mdl_Costo_Financiamiento_Filtrado vm)
        {
            try
            {
                string ruta = $"C:\\SMDH\\Procesados\\CostoIntegralDeFinanciamiento.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add("Inventario");
                    var cell = sheet.Range(1, 1, 1, 7);
                    cell.Value = "COSTO INTEGRAL DE FINANCIAMIENTO";
                    cell.Style.Font.Bold = true;
                    cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Style.Font.FontSize = 16;
                    cell.Merge();

                    sheet.Row(1).Height = 30;
                    sheet.Column(1).Width = 45;
                    sheet.Column(2).Width = 20;
                    sheet.Column(3).Width = 10;
                    sheet.Column(4).Width = 20;
                    sheet.Column(5).Width = 10;
                    sheet.Column(6).Width = 20;
                    sheet.Column(7).Width = 10;

                    int renglon = 2;


                    sheet.Cell(renglon, 1).Value = "Concepto";
                    cell = sheet.Range(renglon, 2, renglon, 3);
                    cell.Value = vm.tituloactual.ToUpper();
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Merge();

                    cell = sheet.Range(renglon, 4, renglon, 5);
                    cell.Value = vm.tituloanterior.ToUpper();
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Merge();

                    cell = sheet.Range(renglon, 6, renglon, 7);
                    cell.Value = "VARIACION";
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Merge();

                    sheet.Range(renglon, 1, renglon, 7).Style.Fill.BackgroundColor = XLColor.FromHtml("#BCD28A");
                    sheet.Range(renglon, 1, renglon, 7).Style.Font.Bold = true;


                    renglon++;

                    sheet.Cell(renglon, 2).Value = "Importe";
                    sheet.Cell(renglon, 3).Value = "%";
                    sheet.Cell(renglon, 4).Value = "Importe";
                    sheet.Cell(renglon, 5).Value = "%";
                    sheet.Cell(renglon, 6).Value = "Importe";
                    sheet.Cell(renglon, 7).Value = "%";
                    sheet.Range(renglon, 1, renglon, 7).Style.Fill.BackgroundColor = XLColor.FromHtml("#BCD28A");
                    sheet.Range(renglon, 1, renglon, 7).Style.Font.Bold = true;

                    renglon++;
                    var groupos = cif.GroupBy(x => x.grupo).ToList();
                    foreach (var grupo in groupos)
                    {
                        var registros = cif.Where(x => x.grupo.Equals(grupo.Key)).ToList();
                        double totalreal = registros.Sum(x => x.real);
                        double totalanterior = registros.Sum(x => x.anterior);

                        cell = sheet.Range(renglon, 1, renglon, 7);
                        cell.Value = grupo.Key.ToUpper();
                        cell.Style.Font.Bold = true;
                        cell.Merge();
                        sheet.Range(renglon, 1, renglon, 7).Style.Fill.BackgroundColor = XLColor.FromHtml("#DAE6BE");
                        sheet.Range(renglon, 1, renglon, 7).Style.Font.Bold = true;
                        renglon++;
                        foreach (var registro in registros)
                        {
                            sheet.Cell(renglon, 1).Value = registro.concepto;
                            sheet.Cell(renglon, 2).Value = registro.real;
                            sheet.Cell(renglon, 3).Value = registro.real / totalreal;
                            sheet.Cell(renglon, 4).Value = registro.anterior;
                            sheet.Cell(renglon, 5).Value = registro.anterior / totalanterior;
                            sheet.Cell(renglon, 6).Value = registro.anteriordiferencia;
                            sheet.Cell(renglon, 7).Value = registro.porcentajeanterior / 100;
                            renglon++;
                        }
                        if (!grupo.Key.Equals("RESULTADO INTEGRAL DE FINANCIAMIENTO"))
                        {
                            sheet.Cell(renglon, 1).Value = "TOTAL";
                            sheet.Cell(renglon, 2).Value = totalreal;
                            sheet.Cell(renglon, 3).Value = 1;
                            sheet.Cell(renglon, 4).Value = totalanterior;
                            sheet.Cell(renglon, 5).Value = 1;
                            sheet.Cell(renglon, 6).Value = totalreal - totalanterior;
                            sheet.Cell(renglon, 7).Value = (totalreal - totalanterior) / totalreal;
                            sheet.Range(renglon, 1, renglon, 7).Style.Font.Bold = true;
                        }
                        renglon++;
                    }

                    sheet.Column(2).Style.NumberFormat.Format = "#,##0";
                    sheet.Column(3).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(4).Style.NumberFormat.Format = "#,##0";
                    sheet.Column(5).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(6).Style.NumberFormat.Format = "#,##0";
                    sheet.Column(7).Style.NumberFormat.Format = "0.0 %";
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
                        filename = $"Costo Integral de Financiamiento"
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
