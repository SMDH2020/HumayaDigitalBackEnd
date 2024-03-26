using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Cobranza.Reportes;
using HD_Cobranza;
using HD_Finanzas.Modelos.Estado_Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD_Cobranza.Modelos;
using HD_Finanzas.Modelos.Linea_Negocio;

namespace HD_Reporteria.Finanzas.Excel
{
    public class XLS_Linea_Negocio_Ventas
    {
        public static Task<DocResult> lineaNegocio(Fmdl_Linea_Negocio_Ventas_PDF lista)
        {
            try
            {
                string sheetname = "Lineas de Negocio";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, $"LINEAS DE NEGOCIO", 27);


                    sheet.Cell(renglon, 1).Value = lista.data.ventastotales.seccion;
                    sheet.Cell(renglon, 2).Value = "$" + lista.data.ventastotales.totalventas.ToString("N2");
                    sheet.Cell(renglon, 3).Value = "Expresado en miles de pesos";
                    sheet.Cell(renglon, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    sheet.Range(renglon, 1,renglon, 2).Style.Font.FontSize = 14;
                    sheet.Range(renglon, 1, renglon, 2).Style.Font.Bold = true;

                    var rango2 = sheet.Range(renglon, 1, renglon, 27);
                    rango2 = sheet.Range(renglon, 1, renglon, 27);
                    rango2.Style.Fill.BackgroundColor = XLColor.FromHtml("#3a3a3c");
                    rango2.Style.Font.FontColor = XLColor.White;
                    renglon++;

                    sheet.Cell(renglon, 1).Value = "CONCEPTO";
                    sheet.Cell(renglon, 2).Value = "MAQUINARIA";
                    sheet.Cell(renglon, 3).Value = "%";
                    sheet.Cell(renglon, 4).Value = "SOLUCIONES INTEGRALES";
                    sheet.Cell(renglon, 5).Value = "%";
                    sheet.Cell(renglon, 6).Value = "REFACCIONES";
                    sheet.Cell(renglon, 7).Value = "%";
                    sheet.Cell(renglon, 8).Value = "SERVICIO";
                    sheet.Cell(renglon, 9).Value = "%";
                    sheet.Cell(renglon, 10).Value = "SUMA JD";
                    sheet.Cell(renglon, 11).Value = "%";
                    sheet.Cell(renglon, 12).Value = "PROD. ALIADO";
                    sheet.Cell(renglon, 13).Value = "%";
                    sheet.Cell(renglon, 14).Value = "FERRETERIA";
                    sheet.Cell(renglon, 15).Value = "%";
                    sheet.Cell(renglon, 16).Value = "RIEGO";
                    sheet.Cell(renglon, 17).Value = "%";
                    sheet.Cell(renglon, 18).Value = "USADOS";
                    sheet.Cell(renglon, 19).Value = "%";
                    sheet.Cell(renglon, 20).Value = "SUMA O.L.";
                    sheet.Cell(renglon, 21).Value = "%";
                    sheet.Cell(renglon, 22).Value = "SUMA JD + O.L.";
                    sheet.Cell(renglon, 23).Value = "%";
                    sheet.Cell(renglon, 24).Value = "STAFF + FINANZAS + ADMON";
                    sheet.Cell(renglon, 25).Value = "%";
                    sheet.Cell(renglon, 26).Value = "TOTAL";
                    sheet.Cell(renglon, 27).Value = "%";


                    var rango = sheet.Range(renglon, 1, renglon, 27);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    foreach (var mdl in lista.data.estadoresultados)
                    {
                            sheet.Cell(renglon, 1).Value = mdl.concepto;
                            sheet.Cell(renglon, 2).Value = mdl.maquinaria;
                            sheet.Cell(renglon, 3).Value = mdl.mrgmaquinaria/100;
                            sheet.Cell(renglon, 4).Value = mdl.ams;
                            sheet.Cell(renglon, 5).Value = mdl.mrgams / 100;
                            sheet.Cell(renglon, 6).Value = mdl.refacciones;
                            sheet.Cell(renglon, 7).Value = mdl.mrgrefacciones / 100;
                            sheet.Cell(renglon, 8).Value = mdl.servicio;
                            sheet.Cell(renglon, 9).Value = mdl.mrgservicio / 100;
                            sheet.Cell(renglon, 10).Value = mdl.sumajs;
                            sheet.Cell(renglon, 11).Value = mdl.mrgsumajd / 100;
                            sheet.Cell(renglon, 12).Value = mdl.paliados;
                            sheet.Cell(renglon, 13).Value = mdl.mrgpaliados / 100;
                            sheet.Cell(renglon, 14).Value = mdl.ferreteria;
                            sheet.Cell(renglon, 15).Value = mdl.mrgferreteria / 100;
                            sheet.Cell(renglon, 16).Value = mdl.sriego;
                            sheet.Cell(renglon, 17).Value = mdl.mrgsriego / 100;
                            sheet.Cell(renglon, 18).Value = mdl.usados;
                            sheet.Cell(renglon, 19).Value = mdl.mrgusados / 100;
                            sheet.Cell(renglon, 20).Value = mdl.sumaol;
                            sheet.Cell(renglon, 21).Value = mdl.mrgsumaol / 100;
                            sheet.Cell(renglon, 22).Value = mdl.sumamh;
                            sheet.Cell(renglon, 23).Value = mdl.mrgsumamh / 100;
                            sheet.Cell(renglon, 24).Value = mdl.gastos;
                            sheet.Cell(renglon, 25).Value = mdl.mrggastos / 100;
                            sheet.Cell(renglon, 26).Value = mdl.sumatotal;
                            sheet.Cell(renglon, 27).Value = mdl.mrgsumatotal / 100;

                        renglon++;

                    }

                    renglon++;
                    renglon++;

                    sheet.Cell(renglon, 1).Value = "INDICADORES"; // Nueva tabla

                    rango = sheet.Range(renglon, 1, renglon, 27);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#5c8fa4");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.Style.Font.FontColor = XLColor.White;
                    renglon++;

                    sheet.Cell(renglon, 1).Value = "MARGEN";
                    sheet.Cell(renglon, 2).Value = "";
                    sheet.Cell(renglon, 3).Value = lista.data.indicadores.mrgmaquinaria/100;
                    sheet.Cell(renglon, 4).Value = "";
                    sheet.Cell(renglon, 5).Value = lista.data.indicadores.mrgams/100;
                    sheet.Cell(renglon, 6).Value = "";
                    sheet.Cell(renglon, 7).Value = lista.data.indicadores.mrgrefacciones / 100;
                    sheet.Cell(renglon, 8).Value = "";
                    sheet.Cell(renglon, 9).Value = lista.data.indicadores.mrgservicio / 100;
                    sheet.Cell(renglon, 10).Value = "";
                    sheet.Cell(renglon, 11).Value = lista.data.indicadores.mrgsumajd / 100  ;
                    sheet.Cell(renglon, 12).Value = "";
                    sheet.Cell(renglon, 13).Value = lista.data.indicadores.mrgpaliados/100;
                    sheet.Cell(renglon, 14).Value = "";
                    sheet.Cell(renglon, 15).Value = lista.data.indicadores.mrgferreteria / 100;
                    sheet.Cell(renglon, 16).Value = "";
                    sheet.Cell(renglon, 17).Value = lista.data.indicadores.mrgsriego / 100;
                    sheet.Cell(renglon, 18).Value = "";
                    sheet.Cell(renglon, 19).Value = lista.data.indicadores.mrgusados / 100;
                    sheet.Cell(renglon, 20).Value = "";
                    sheet.Cell(renglon, 21).Value = lista.data.indicadores.mrgsumaol / 100;
                    sheet.Cell(renglon, 22).Value = "";
                    sheet.Cell(renglon, 23).Value = lista.data.indicadores.mrgsumatotal / 100;
                    sheet.Cell(renglon, 24).Value = "";
                    renglon++;

                    sheet.Cell(renglon, 1).Value = "VENTAS/VENTAS TOTALES";
                    sheet.Cell(renglon, 2).Value = "";
                    sheet.Cell(renglon, 3).Value = lista.data.margenventasnetas.maquinaria / 100;
                    sheet.Cell(renglon, 4).Value = "";
                    sheet.Cell(renglon, 5).Value = lista.data.margenventasnetas.ams / 100;
                    sheet.Cell(renglon, 6).Value = "";
                    sheet.Cell(renglon, 7).Value = lista.data.margenventasnetas.refacciones / 100;
                    sheet.Cell(renglon, 8).Value = "";
                    sheet.Cell(renglon, 9).Value = lista.data.margenventasnetas.servicio / 100;
                    sheet.Cell(renglon, 10).Value = "";
                    sheet.Cell(renglon, 11).Value = lista.data.margenventasnetas.jd / 100;
                    sheet.Cell(renglon, 12).Value = "";
                    sheet.Cell(renglon, 13).Value = lista.data.margenventasnetas.paliados / 100;
                    sheet.Cell(renglon, 14).Value = "";
                    sheet.Cell(renglon, 15).Value = lista.data.margenventasnetas.ferreteria / 100;
                    sheet.Cell(renglon, 16).Value = "";
                    sheet.Cell(renglon, 17).Value = lista.data.margenventasnetas.sriego / 100;
                    sheet.Cell(renglon, 18).Value = "";
                    sheet.Cell(renglon, 19).Value = lista.data.margenventasnetas.usados / 100;
                    sheet.Cell(renglon, 20).Value = "";
                    sheet.Cell(renglon, 21).Value = lista.data.margenventasnetas.ol/100;
                    sheet.Cell(renglon, 22).Value = "";
                    sheet.Cell(renglon, 23).Value = lista.data.margenventasnetas.total/100;
                    sheet.Cell(renglon, 24).Value = "";
                    renglon++;
                    renglon++;

                    sheet.Column(2).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(3).Style.NumberFormat.Format = "0.00 %";
                    sheet.Column(4).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(5).Style.NumberFormat.Format = "0.00 %";
                    sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(7).Style.NumberFormat.Format = "0.00 %";
                    sheet.Column(8).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(9).Style.NumberFormat.Format = "0.00 %";
                    sheet.Column(10).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(11).Style.NumberFormat.Format = "0.00 %";
                    sheet.Column(12).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(13).Style.NumberFormat.Format = "0.00 %";
                    sheet.Column(14).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(15).Style.NumberFormat.Format = "0.00 %";
                    sheet.Column(16).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(17).Style.NumberFormat.Format = "0.00 %";
                    sheet.Column(18).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(19).Style.NumberFormat.Format = "0.00 %";
                    sheet.Column(20).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(21).Style.NumberFormat.Format = "0.00 %";
                    sheet.Column(22).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(23).Style.NumberFormat.Format = "0.00 %";
                    sheet.Column(24).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(25).Style.NumberFormat.Format = "0.00 %";
                    sheet.Column(26).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(27).Style.NumberFormat.Format = "0.00 %";

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
                        if(lista.region.Count > 1)
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
