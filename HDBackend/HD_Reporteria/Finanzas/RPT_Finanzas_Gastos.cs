using HD_Cobranza.Modelos;
using HD_Finanzas.Modelos.Gastos_Proyeccion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Reporteria.Finanzas
{
    public class RPT_Finanzas_Gastos
    {
        public static RPT_Result Generar(Fmdl_Gastos_PDF resumen)
        {
            try
            {
                string fontFamily = "Calibri";
                byte[] doc = Document.Create(document =>
                {
                    document.Page(page =>
                    {
                        page.Size(PageSizes.A4.Landscape());



                        page.Header().Height(120).Row(row =>
                        {

                            //row.ConstantItem(140).Border(1).Placeholder();
                            row.RelativeItem().PaddingTop(35).Height(50).Background("#477c2c").Row(row2 =>
                            {

                            });

                            row.ConstantColumn(0).Row(row1 =>
                            {
                                var rutaImagen = Path.Combine("C:\\Nube\\HumayaDigital\\HumayaDigitalBackEnd\\HDBackend\\HD_Reporteria\\Imagenes\\Logo.jpg");
                                byte[] imageData = System.IO.File.ReadAllBytes(rutaImagen);
                                row.ConstantItem(120).Image(imageData);

                                row.ConstantColumn(693).PaddingTop(35).Height(50).Background("#477c2c").Row(row2 =>
                                {
                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text("GASTOS DE OPERACION").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
                                });
                            });


                        });

                        page.Content().PaddingTop(10).PaddingLeft(30).PaddingRight(30).Column(col1 =>
                        {

                        //col1.Item().LineHorizontal(0.5f);

                        col1.Item().AlignRight().Row(row1 =>
                        {
                            row1.AutoItem().Column(txt1 =>
                            {
                                txt1.Item().Height(20).Text(txt2 =>
                                {
                                    txt2.Span(resumen.subtitulo).Bold().FontSize(12).FontFamily(fontFamily);
                                });
                            });
                        });

                        if (resumen.region.Count < 1)
                        {

                        }
                        else
                        {
                            col1.Item().AlignRight().Row(row1 =>
                            {
                                row1.AutoItem().Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Region: ").Bold().FontSize(10).FontFamily(fontFamily);
                                    });
                                });
                                row1.AutoItem().Column(txt1 =>
                                {
                                    txt1.Item().Height(15).AlignMiddle().Text(txt2 =>
                                    {
                                        if (resumen.region.Count > 1)
                                        {
                                            txt2.Span("TODO EL GRUPO").FontSize(8).FontFamily(fontFamily);
                                        }
                                        else
                                        {
                                            foreach (var reg in resumen.region)
                                            {
                                                txt2.Span(reg.adr).FontSize(8).FontFamily(fontFamily);
                                            }
                                        }
                                    });
                                });
                            });
                        }

                            if (resumen.sucursal.Count < 1)
                            {

                            }
                            else
                            {
                                col1.Item().AlignRight().Row(row1 =>
                                {
                                    row1.AutoItem().Column(txt1 =>
                                    {
                                        txt1.Item().Height(15).Text(txt2 =>
                                        {
                                            txt2.Span("Sucursal: ").Bold().FontSize(10).FontFamily(fontFamily);
                                        });
                                    });
                                    row1.AutoItem().Column(txt1 =>
                                    {
                                        txt1.Item().Height(15).AlignMiddle().Text(txt2 =>
                                        {
                                            int count = resumen.sucursal.Count;
                                            for (int i = 0; i < count; i++)
                                            {
                                                if (i < count - 1)
                                                {
                                                    txt2.Span(resumen.sucursal[i].sucursal + ", ").FontSize(8).FontFamily(fontFamily);
                                                }
                                                else
                                                {
                                                    txt2.Span(resumen.sucursal[i].sucursal).FontSize(8).FontFamily(fontFamily);
                                                }
                                            }
                                        });
                                    });
                                });
                            }

                            if (resumen.departamento.Count < 1)
                            {

                            }
                            else
                            {
                                col1.Item().AlignRight().Row(row1 =>
                                {
                                    row1.AutoItem().Column(txt1 =>
                                    {
                                        txt1.Item().Height(15).Text(txt2 =>
                                        {
                                            txt2.Span("Departamento: ").Bold().FontSize(10).FontFamily(fontFamily);
                                        });
                                    });
                                    row1.AutoItem().Column(txt1 =>
                                    {
                                        txt1.Item().Height(15).AlignMiddle().Text(txt2 =>
                                        {
                                            int count = resumen.departamento.Count;
                                            for (int i = 0; i < count; i++)
                                            {
                                                if (i < count - 1)
                                                {
                                                    txt2.Span(resumen.departamento[i].departamento + ", ").FontSize(8).FontFamily(fontFamily);
                                                }
                                                else
                                                {
                                                    txt2.Span(resumen.departamento[i].departamento).FontSize(8).FontFamily(fontFamily);
                                                }
                                            }
                                        });
                                    });
                                });
                            }



                            col1.Item().PaddingVertical(10).Border(1).BorderColor("#275027").Table(tabla =>
                            {
                                tabla.ColumnsDefinition(Columns =>
                                {
                                    Columns.RelativeColumn(2);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(1);

                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().ColumnSpan(1).Height(25).Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("").FontSize(12).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(4).Height(25).Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text(resumen.periodoactual).FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(3).Height(25).Background("#275027").AlignMiddle()
                                    .Padding(1).Text(resumen.periodoanterior).FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("CONCEPTO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("REAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("PROYECCION").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignRight().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("DIF").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("REAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("DIF").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                string tipoActual = "";

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

                                foreach (var mdl in resumen.data)
                                {
                                    if (tipoActual != mdl.tipo)
                                    {
                                        if (tipoActual != "")
                                        {
                                            tabla.Cell().BorderTop(1).BorderColor("#afb69d").PaddingLeft(15).Height(20).AlignMiddle()
                                                .Text("TOTAL GASTOS VARIABLES").FontSize(8).FontFamily(fontFamily).Bold();

                                            // Agregar celdas para mostrar el total de cada columna
                                            tabla.Cell().BorderTop(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                                .Text(totalReal.ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();

                                            tabla.Cell().BorderTop(1).BorderColor("#afb69d").AlignRight().Height(20).PaddingRight(10).AlignMiddle()
                                                .Text(totalProyeccion.ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();

                                            tabla.Cell().BorderTop(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                                .Text((totalDif*100/totalProyeccion).ToString("N2")).FontSize(8).FontColor("#ff2037").FontFamily(fontFamily).Bold();

                                            tabla.Cell().BorderTop(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                                .Text(totalDif.ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();

                                            tabla.Cell().BorderTop(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                                .Text(totalOldTotal.ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();

                                            tabla.Cell().BorderTop(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                                .Text((totalOldDif*100/totalOldTotal).ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();

                                            tabla.Cell().BorderTop(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                                .Text(totalOldDif.ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();
                                        }

                                        // Determinar el texto a mostrar según el tipo
                                        string tipoTexto = mdl.tipo == "V" ? "GASTOS VARIABLES" : "GASTOS FIJOS";

                                        // Agregar una fila para mostrar el tipo
                                        tabla.Cell().ColumnSpan(8).BorderBottom(1).BorderColor("#afb69d").Background("#cccccc").PaddingLeft(15).Height(20).AlignCenter().AlignMiddle()
                                           .Text(tipoTexto).FontSize(8).FontFamily(fontFamily).Bold(); // ColumnSpan para que ocupe todas las columnas                                                                                 
                                        tipoActual = mdl.tipo;

                                        // Reiniciar las sumas de cada columna para el nuevo tipo
                                        totalReal = 0;
                                        totalProyeccion = 0;
                                        totalPorcentaje = 0;
                                        totalDif = 0;
                                        totalOldTotal = 0;
                                        totalOldPorc = 0;
                                        totalOldDif = 0;

                                    }

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(15).Height(20).AlignMiddle()
                                    .Text(mdl.concepto).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                   .Text(mdl.total.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).PaddingRight(10).AlignMiddle()
                                   .Text((mdl.proyeccion).ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                   .Text(mdl.porc.ToString("N2")).FontSize(8).FontColor("#ff2037").FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                   .Text((mdl.dif).ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                   .Text(mdl.oldtotal.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                   .Text((mdl.oldporc).ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                   .Text(mdl.olddif.ToString("N2")).FontSize(8).FontFamily(fontFamily);

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
                                tabla.Cell().BorderTop(1).BorderColor("#afb69d").PaddingLeft(15).Height(20).AlignMiddle()
                                .Text("TOTAL GASTOS FIJOS").FontSize(8).FontFamily(fontFamily).Bold();

                                // Agregar celdas para mostrar el total de cada columna
                                tabla.Cell().BorderTop(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                    .Text(totalReal.ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();

                                tabla.Cell().BorderTop(1).BorderColor("#afb69d").AlignRight().Height(20).PaddingRight(10).AlignMiddle()
                                    .Text(totalProyeccion.ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();

                                tabla.Cell().BorderTop(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                    .Text((totalDif * 100 / totalProyeccion).ToString("N2")).FontSize(8).FontColor("#ff2037").FontFamily(fontFamily).Bold();

                                tabla.Cell().BorderTop(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                    .Text(totalDif.ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();

                                tabla.Cell().BorderTop(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                    .Text(totalOldTotal.ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();

                                tabla.Cell().BorderTop(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                    .Text((totalOldDif * 100 / totalOldTotal).ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();

                                tabla.Cell().BorderTop(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                    .Text(totalOldDif.ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();

                                //Totales Generales
                                tabla.Cell().BorderTop(1).BorderColor("#afb69d").Background("#cccccc").PaddingLeft(15).Height(20).AlignMiddle()
                                .Text("TOTAL GENERAL").FontSize(8).FontFamily(fontFamily).Bold();

                                tabla.Cell().BorderTop(1).BorderColor("#afb69d").Background("#cccccc").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                    .Text(totalRealGeneral.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderTop(1).BorderColor("#afb69d").Background("#cccccc").AlignRight().Height(20).PaddingRight(10).AlignMiddle()
                                    .Text(totalProyeccionGeneral.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderTop(1).BorderColor("#afb69d").Background("#cccccc").AlignRight().Height(20).AlignMiddle()
                                    .Text((totalDifGeneral*100/totalProyeccionGeneral).ToString("N2")).FontSize(8).FontColor("#ff2037").FontFamily(fontFamily);

                                tabla.Cell().BorderTop(1).BorderColor("#afb69d").Background("#cccccc").AlignRight().Height(20).AlignMiddle()
                                    .Text(totalDifGeneral.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderTop(1).BorderColor("#afb69d").Background("#cccccc").AlignRight().Height(20).AlignMiddle()
                                    .Text(totalOldTotalGeneral.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderTop(1).BorderColor("#afb69d").Background("#cccccc").AlignRight().Height(20).AlignMiddle()
                                    .Text((totalOldDifGeneral*100/totalOldTotalGeneral).ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderTop(1).BorderColor("#afb69d").Background("#cccccc").AlignRight().Height(20).AlignMiddle()
                                    .Text(totalOldDifGeneral.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                            });

                        });

                    });
                    
                }).GeneratePdf();
                RPT_Result result = new RPT_Result();
                result.extension = "pdf";
                result.nombredocumento = "GASTOS";
                result.documento = Convert.ToBase64String(doc);
                return result;


            }

            catch (Exception ex)
            {

                throw ex;
            }


        }
    }
}
