using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Cobranza.Reportes;
using HD_Cobranza;
using HD_Finanzas.Modelos.Gastos_Proyeccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD_Finanzas.Modelos.Estado_Resultados;

namespace HD_Reporteria.Finanzas.Excel
{
    public class XLS_EstadoResultados
    {
        public static Task<DocResult> EstadoResultados(Fmdl_EstadoResultados_PDF lista)
        {
            try
            {
                string sheetname = "Estado de Resultados";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, $"Estado de Resultados", 11);

                    sheet.Cell(renglon, 1).Value = "";
                    sheet.Range("B4:G4").Merge();
                    sheet.Range("H4:K4").Merge();

                    sheet.Cell(renglon, 2).Value = lista.periodoactual;
                    sheet.Cell(renglon, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Cell(renglon, 2).Style.Font.Bold = true;

                    var rango2 = sheet.Range(renglon, 1, renglon, 27);
                    rango2 = sheet.Range(renglon, 1, renglon, 7);
                    rango2.Style.Font.FontSize = 12;
                    rango2.Style.Fill.BackgroundColor = XLColor.FromHtml("#ebecee");

                    sheet.Cell(renglon, 8).Value = lista.periodoanterior;
                    sheet.Cell(renglon, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Cell(renglon, 8).Style.Font.Bold = true;


                    var rango3 = sheet.Range(renglon, 1, renglon, 27);
                    rango3.Style.Font.FontSize = 12;
                    rango3 = sheet.Range(renglon, 8, renglon, 11);
                    rango3.Style.Fill.BackgroundColor = XLColor.FromHtml("#ebecee");

                    renglon++;

                    sheet.Cell(renglon, 1).Value = "CONCEPTO";
                    sheet.Cell(renglon, 2).Value = "REAL";
                    sheet.Cell(renglon, 3).Value = "%";
                    sheet.Cell(renglon, 4).Value = "PROYECCION";
                    sheet.Cell(renglon, 5).Value = "%";
                    sheet.Cell(renglon, 6).Value = "DESVIACION";
                    sheet.Cell(renglon, 7).Value = "%";
                    sheet.Cell(renglon, 8).Value = "REAL";
                    sheet.Cell(renglon, 9).Value = "%";
                    sheet.Cell(renglon, 10).Value = "DESVIACION";
                    sheet.Cell(renglon, 11).Value = "%";


                    var rango = sheet.Range(renglon, 1, renglon, 11);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    foreach (var mdl in lista.data)
                    {
                        sheet.Cell(renglon, 1).Value = mdl.depto;

                        sheet.Range(renglon, 1, renglon, 2).Style.Font.Bold = true;
                        rango = sheet.Range(renglon, 1, renglon, 11);
                        rango.Style.Font.FontSize = 12;
                        rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#cccccc");
                        renglon++;

                        for (int i = 0; i < mdl.data.Count; i++)
                        {
                            var ln = mdl.data[i];
                            var isLastRow = (i == mdl.data.Count - 1);
                            sheet.Cell(renglon, 1).Value = ln.concepto;
                            sheet.Cell(renglon, 2).Value = ln.importe;
                            sheet.Cell(renglon, 3).Value = ln.por/100;
                            sheet.Cell(renglon, 4).Value = ln.proyimporte;
                            sheet.Cell(renglon, 5).Value = ln.proypor/100;
                            sheet.Cell(renglon, 6).Value = ln.diffimporte;
                            sheet.Cell(renglon, 7).Value = ln.diffpor / 100;
                            sheet.Cell(renglon, 8).Value = ln.lastimporte;
                            sheet.Cell(renglon, 9).Value = ln.lastpor / 100;
                            sheet.Cell(renglon, 10).Value = ln.lastdiffimporte;
                            sheet.Cell(renglon, 11).Value = ln.lastdiffpor / 100;                                          
                            renglon++;
                        }

                    }


                    sheet.Column(2).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(3).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(4).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(5).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(7).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(8).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(9).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(10).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(11).Style.NumberFormat.Format = "0.0 %";

                    renglon++;
                    renglon++;

                    sheet.Cell(renglon, 1).Value = lista.subtitulo;
                    sheet.Range(renglon, 1, renglon, 2).Style.Font.Bold = true;
                    rango2 = sheet.Range(renglon, 1, renglon, 27);
                    rango2.Style.Font.FontSize = 12;
                    rango2.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFFFFF");
                    renglon++;

                    if (lista.region.Count < 1)
                    {

                    }
                    else
                    {
                        if (lista.region.Count > 1)
                        {
                            sheet.Cell(renglon, 1).Value = "Region: TODO EL GRUPO";
                        }
                        else
                        {
                            foreach (var reg in lista.region)
                            {
                                sheet.Cell(renglon, 1).Value = "Region: " + reg.adr;
                            }

                        };
                    };
                    sheet.Range(renglon, 1, renglon, 2).Style.Font.Bold = true;
                    rango2 = sheet.Range(renglon, 1, renglon, 27);
                    rango2.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFFFFF");
                    renglon++;

                    if (lista.sucursal.Count < 1)
                    {

                    }
                    else
                    {
                        int count = lista.sucursal.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (lista.sucursal.Count > 1)
                            {
                                string sucursalesConcatenadas = string.Join(", ", lista.sucursal.Select(s => s.sucursal));
                                sheet.Cell(renglon, 1).Value = "Sucursal: " + sucursalesConcatenadas;
                            }
                            else
                            {
                                sheet.Cell(renglon, 1).Value = "Sucursal: " + lista.sucursal[i].sucursal;
                            }

                        }

                    };
                    sheet.Range(renglon, 1, renglon, 2).Style.Font.Bold = true;
                    rango2 = sheet.Range(renglon, 1, renglon, 27);
                    rango2.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFFFFF");
                    renglon++;

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
