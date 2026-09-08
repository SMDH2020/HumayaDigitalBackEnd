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


                    sheet.Cell(renglon, 1).Value = "";
                    sheet.Range("B4:E4").Merge();
                    sheet.Range("F4:H4").Merge();

                    sheet.Cell(renglon, 2).Value = lista.periodoactual;
                    sheet.Cell(renglon, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Cell(renglon, 2).Style.Font.Bold = true;

                    var rango2 = sheet.Range(renglon, 1, renglon, 27);
                    rango2 = sheet.Range(renglon, 1, renglon, 5);
                    rango2.Style.Font.FontSize = 12;
                    rango2.Style.Fill.BackgroundColor = XLColor.FromHtml("#ebecee");

                    sheet.Cell(renglon, 6).Value = lista.periodoanterior;
                    sheet.Cell(renglon, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Cell(renglon, 6).Style.Font.Bold = true;


                    var rango3 = sheet.Range(renglon, 1, renglon, 27);
                    rango3.Style.Font.FontSize = 12;
                    rango3 = sheet.Range(renglon, 6, renglon, 8);
                    rango3.Style.Fill.BackgroundColor = XLColor.FromHtml("#ebecee");

                    renglon++;

                    sheet.Cell(renglon, 1).Value = "CONCEPTO";
                    sheet.Cell(renglon, 2).Value = "REAL";
                    sheet.Cell(renglon, 3).Value = "PROYECCION";
                    sheet.Cell(renglon, 4).Value = "%";
                    sheet.Cell(renglon, 5).Value = "DIF";
                    sheet.Cell(renglon, 6).Value = "REAL";
                    sheet.Cell(renglon, 7).Value = "%";
                    sheet.Cell(renglon, 8).Value = "DIF";

                    var rango = sheet.Range(renglon, 1, renglon, 8);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    string tipoActual = "";
                    int contador = 0;

                    decimal totalReal = 0;
                    decimal totalProyeccion = 0;
                    decimal totalPorcentaje = 0;
                    decimal totalDif = 0;
                    decimal totalOldTotal = 0;
                    decimal totalOldPorc = 0;
                    decimal totalOldDif = 0;


                    decimal totalRealGeneral = 0;
                    decimal totalProyeccionGeneral = 0;
                    decimal totalPorcentajeGeneral = 0;
                    decimal totalDifGeneral = 0;
                    decimal totalOldTotalGeneral = 0;
                    decimal totalOldPorcGeneral = 0;
                    decimal totalOldDifGeneral = 0;

                    foreach (var mdl in lista.data)
                    {
                        if (tipoActual != mdl.tipo)
                        {
                            if (tipoActual != "")
                            {
                                sheet.Cell(renglon, 1).Value = "TOTAL GASTOS VARIABLES";
                                sheet.Cell(renglon, 2).Value = totalReal;
                                sheet.Cell(renglon, 3).Value = totalProyeccion;
                                sheet.Cell(renglon, 4).Value = (totalProyeccion != 0 ? (totalDif/totalProyeccion) : "0.00");
                                sheet.Cell(renglon, 5).Value = totalDif;
                                sheet.Cell(renglon, 6).Value = totalOldTotal;
                                sheet.Cell(renglon, 7).Value = (totalOldTotal != 0 ? (totalOldDif / totalOldTotal) : "0.00");
                                sheet.Cell(renglon, 8).Value = totalOldDif;

                                var rangoTotal = sheet.Range(renglon, 1, renglon, 8);
                                //rangoTitulo.Merge();
                                rangoTotal.Style.Font.Bold = true;
                                rangoTotal.Style.Fill.BackgroundColor = XLColor.FromHtml("#cccccc");
                                renglon++;
                            }
                            string tipoGasto = mdl.tipo == "V" ? "GASTOS VARIABLES" : "GASTOS FIJOS";

                            sheet.Cell(renglon, 1).Value = tipoGasto;
                            var rangoTitulo = sheet.Range(renglon, 1, renglon, 8);
                            //rangoTitulo.Merge();
                            rangoTitulo.Style.Font.Bold = true;
                            rangoTitulo.Style.Font.FontSize = 12;
                            rangoTitulo.Style.Fill.BackgroundColor = XLColor.FromHtml("#cccccc"); 
                            tipoActual = mdl.tipo;
                            renglon++;

                            totalReal = 0;
                            totalProyeccion = 0;
                            totalPorcentaje = 0;
                            totalDif = 0;
                            totalOldTotal = 0;
                            totalOldPorc = 0;
                            totalOldDif = 0;
                        }

                        sheet.Cell(renglon, 1).Value = mdl.concepto;
                        sheet.Cell(renglon, 2).Value = mdl.total;
                        sheet.Cell(renglon, 3).Value = mdl.proyeccion;
                        sheet.Cell(renglon, 4).Value = mdl.porc/100;
                        sheet.Cell(renglon, 5).Value = mdl.dif;
                        sheet.Cell(renglon, 6).Value = mdl.oldtotal;
                        sheet.Cell(renglon, 7).Value = mdl.oldporc/100;
                        sheet.Cell(renglon, 8).Value = mdl.olddif;
                        renglon++;

                        totalReal += (decimal)mdl.total;
                        totalProyeccion += (decimal)mdl.proyeccion;
                        totalPorcentaje += (decimal)mdl.porc;
                        totalDif += (decimal)mdl.dif;
                        totalOldTotal += (decimal)mdl.oldtotal;
                        totalOldPorc += (decimal)mdl.oldporc;
                        totalOldDif += (decimal)mdl.olddif;

                        totalRealGeneral += (decimal)mdl.total;
                        totalProyeccionGeneral += (decimal)mdl.proyeccion;
                        totalPorcentajeGeneral += (decimal)mdl.porc;
                        totalDifGeneral += (decimal)mdl.dif;
                        totalOldTotalGeneral += (decimal)mdl.oldtotal;
                        totalOldPorcGeneral += (decimal)mdl.oldporc;
                        totalOldDifGeneral += (decimal)mdl.olddif;
                    }
                    sheet.Cell(renglon, 1).Value = "TOTAL GASTOS FIJOS";
                    sheet.Cell(renglon, 2).Value = totalReal;
                    sheet.Cell(renglon, 3).Value = totalProyeccion;
                    sheet.Cell(renglon, 4).Value = (totalProyeccion != 0 ? (totalDif / totalProyeccion) : "0.00");
                    sheet.Cell(renglon, 5).Value = totalDif;
                    sheet.Cell(renglon, 6).Value = totalOldTotal;
                    sheet.Cell(renglon, 7).Value = (totalOldTotal != 0 ? (totalOldDif / totalOldTotal) : "0.00");
                    sheet.Cell(renglon, 8).Value = totalOldDif;

                    var rangoTotal2 = sheet.Range(renglon, 1, renglon, 8);
                    //rangoTitulo.Merge();
                    rangoTotal2.Style.Font.Bold = true;
                    rangoTotal2.Style.Fill.BackgroundColor = XLColor.FromHtml("#cccccc");
                    renglon++;

                    sheet.Cell(renglon, 1).Value = "TOTAL GENERAL";
                    sheet.Cell(renglon, 2).Value = totalRealGeneral;
                    sheet.Cell(renglon, 3).Value = totalProyeccionGeneral;
                    sheet.Cell(renglon, 4).Value = (totalProyeccionGeneral != 0 ? (totalDifGeneral / totalProyeccionGeneral) : "0.00");
                    sheet.Cell(renglon, 5).Value = totalDifGeneral;
                    sheet.Cell(renglon, 6).Value = totalOldTotalGeneral;
                    sheet.Cell(renglon, 7).Value = (totalOldTotalGeneral != 0 ? (totalOldDifGeneral / totalOldTotalGeneral) : "0.00")   ;
                    sheet.Cell(renglon, 8).Value = totalOldDifGeneral;

                    var rangoTotal3 = sheet.Range(renglon, 1, renglon, 8);
                    rangoTotal3.Style.Font.Bold = true;
                    rangoTotal3.Style.Fill.BackgroundColor = XLColor.FromHtml("#cccccc");
                    renglon++;
                    renglon++;


                    sheet.Column(2).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(3).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(4).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(5).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(7).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(8).Style.NumberFormat.Format = "#,##0.00";



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

                    if (lista.departamento.Count < 1)
                    {

                    }
                    else
                    {
                        int count = lista.departamento.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (lista.departamento.Count > 1)
                            {
                                string departamentoConcatenadas = string.Join(", ", lista.departamento.Select(d => d.departamento));
                                sheet.Cell(renglon, 1).Value = "Departamento: " + departamentoConcatenadas;
                            }
                            else
                            {
                                sheet.Cell(renglon, 1).Value = "Departamento: " + lista.departamento[i].departamento;
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
