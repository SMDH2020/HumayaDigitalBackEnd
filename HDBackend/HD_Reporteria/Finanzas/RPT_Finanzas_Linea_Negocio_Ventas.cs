using HD_Finanzas.Modelos.Estado_Resultados;
using HD_Finanzas.Modelos.Linea_Negocio;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Reporteria.Finanzas
{
    public class RPT_Finanzas_Linea_Negocio_Ventas
    {
        public static RPT_Result Generar(Fmdl_Linea_Negocio_Ventas_PDF resumen)
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
                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text("RESULTADOS POR LINEA DE NEGOCIO").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
                                });
                            });


                        });

                        page.Content().PaddingLeft(30).PaddingRight(30).Column(col1 =>
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



                            col1.Item().PaddingVertical(10).Border(1).BorderColor("#275027").Table(tabla =>
                            {
                                tabla.ColumnsDefinition(Columns =>
                                {
                                    Columns.RelativeColumn(2);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);

                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().ColumnSpan(1).Height(25).Background("#3a3a3c").AlignCenter().AlignMiddle()
                                    .Padding(1).Text(resumen.data.ventastotales.seccion).FontSize(12).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(1).Height(25).Background("#3a3a3c").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("$ " + resumen.data.ventastotales.totalventas.ToString("N2")).FontSize(12).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(9).Height(25).Background("#3a3a3c").AlignMiddle()
                                    .Padding(1).Text("Expresado en miles de pesos").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().ColumnSpan(11).Height(20).Background("#4f7b58").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("LINEAS JOHN DEERE").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().ColumnSpan(1).Height(20).Background("#4f7b58").AlignMiddle()
                                    .Padding(1).Text("RESULTADOS POR LINEA DE NEGOCIO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(6).Height(20).Background("#9bb06f").AlignCenter().AlignMiddle()
                                    .Padding(1).Text(resumen.data.margenesbygrupo.lineasjd.ToString("N2")).FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(4).Height(20).Background("#9bb06f").AlignCenter().AlignMiddle()
                                    .Padding(1).Text(resumen.data.margenesbygrupo.mrglineasjd.ToString("N2") + " %").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("CONCEPTO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("MAQUINARIA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignRight().AlignMiddle()
                                    .Padding(1).Text("SOLUCIONES INTEGRALES").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("REFACCIONES").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("SERVICIO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("SUMA JD").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });


                                foreach (var mdl in resumen.data.estadoresultados)
                                {

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Background("#dae6be").PaddingLeft(15).Height(20).AlignMiddle()
                                        .Text(mdl.concepto).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                        .Text(mdl.maquinaria.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).PaddingRight(10).AlignMiddle()
                                        .Text(mdl.mrgmaquinaria.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text(mdl.ams.ToString("N2")).FontSize(8).FontColor("#ff2037").FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text((mdl.mrgams).ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text(mdl.refacciones.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text((mdl.mrgrefacciones).ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text(mdl.servicio.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text(mdl.mrgservicio.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text(mdl.sumajs.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text(mdl.mrgsumajd.ToString("N2")).FontSize(8).FontFamily(fontFamily);


                                }
                            });

                            //TABLA INDICADORES
                            col1.Item().PaddingVertical(10).Border(1).BorderColor("#275027").Table(tabla =>
                            {
                                tabla.ColumnsDefinition(Columns =>
                                {
                                    Columns.RelativeColumn(2);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);

                                });
                                tabla.Header(header =>
                                {
                                    header.Cell().ColumnSpan(11).Height(20).Background("#5c8fa4").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("INDICADORES").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                }); 
                                
                                //MARGENES
                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Background("#dae6be").PaddingLeft(15).Height(20).AlignMiddle()
                                        .Text("MARGEN").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                        .Text("").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).PaddingRight(10).AlignMiddle()
                                        .Text(resumen.data.indicadores.mrgmaquinaria.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text("").FontSize(8).FontColor("#ff2037").FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text(resumen.data.indicadores.mrgams.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text("").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text(resumen.data.indicadores.mrgrefacciones.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text("").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text(resumen.data.indicadores.mrgservicio).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text("").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text(resumen.data.indicadores.mrgsumajd.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                //VENTAS TOTALES
                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Background("#dae6be").PaddingLeft(15).Height(20).AlignMiddle()
                                    .Text("VENTAS/VENTAS TOTALES").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                    .Text("").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).PaddingRight(10).AlignMiddle()
                                    .Text(resumen.data.margenventasnetas.maquinaria.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                     tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                    .Text("").FontSize(8).FontColor("#ff2037").FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                    .Text(resumen.data.margenventasnetas.ams.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                    .Text("").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                    .Text(resumen.data.margenventasnetas.refacciones.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                    .Text("").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                    .Text(resumen.data.margenventasnetas.servicio.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                    .Text("").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                    .Text(resumen.data.margenventasnetas.jd.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                            });

                        });

                    });


                //PAGINA LINEAS ALIADAS
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
                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text("RESULTADOS POR LINEA DE NEGOCIO").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
                                });
                            });


                        });


                        page.Content().PaddingTop(10).PaddingLeft(30).PaddingRight(30).Column(col1 =>
                        {

                            //col1.Item().LineHorizontal(0.5f);



                            col1.Item().PaddingVertical(10).Border(1).BorderColor("#275027").Table(tabla =>
                            {
                                tabla.ColumnsDefinition(Columns =>
                                {
                                    Columns.RelativeColumn(2);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);

                                });
                                tabla.Header(header =>
                                {
                                    header.Cell().ColumnSpan(11).Height(20).Background("#4f7b58").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("LINEAS ALIADAS").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().ColumnSpan(1).Height(20).Background("#4f7b58").AlignMiddle()
                                    .Padding(1).Text("RESULTADOS POR LINEA DE NEGOCIO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(6).Height(20).Background("#9bb06f").AlignCenter().AlignMiddle()
                                    .Padding(1).Text(resumen.data.margenesbygrupo.otraslineas.ToString("N2")).FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(4).Height(20).Background("#9bb06f").AlignCenter().AlignMiddle()
                                    .Padding(1).Text(resumen.data.margenesbygrupo.mrgotraslineas.ToString("N2") + " %").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("CONCEPTO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("PROD. ALIADO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignRight().AlignMiddle()
                                    .Padding(1).Text("FERRETERIA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("RIEGO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("USADOS").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("SUMA O.L.").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });


                                foreach (var mdl in resumen.data.estadoresultados)
                                {

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Background("#dae6be").PaddingLeft(15).Height(20).AlignMiddle()
                                        .Text(mdl.concepto).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                        .Text(mdl.paliados.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).PaddingRight(10).AlignMiddle()
                                        .Text(mdl.mrgpaliados.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text(mdl.ferreteria.ToString("N2")).FontSize(8).FontColor("#ff2037").FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text((mdl.mrgferreteria).ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text(mdl.sriego.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text((mdl.mrgsriego).ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text(mdl.usados.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text(mdl.mrgusados.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text(mdl.sumaol.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text(mdl.mrgsumaol.ToString("N2")).FontSize(8).FontFamily(fontFamily);


                                }
                            });

                            //TABLA INDICADORES
                            col1.Item().PaddingVertical(10).Border(1).BorderColor("#275027").Table(tabla =>
                            {
                                tabla.ColumnsDefinition(Columns =>
                                {
                                    Columns.RelativeColumn(2);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);

                                });
                                tabla.Header(header =>
                                {
                                    header.Cell().ColumnSpan(11).Height(20).Background("#5c8fa4").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("INDICADORES").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                //MARGENES
                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Background("#dae6be").PaddingLeft(15).Height(20).AlignMiddle()
                                    .Text("MARGEN").FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                    .Text("").FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).PaddingRight(10).AlignMiddle()
                                    .Text(resumen.data.indicadores.mrgpaliados.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                    .Text("").FontSize(8).FontColor("#ff2037").FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                    .Text(resumen.data.indicadores.mrgferreteria.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                    .Text("").FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                    .Text(resumen.data.indicadores.mrgsriego.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                    .Text("").FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                    .Text(resumen.data.indicadores.mrgusados).FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                    .Text("").FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                    .Text(resumen.data.indicadores.mrgsumaol.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                //VENTAS TOTALES
                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Background("#dae6be").PaddingLeft(15).Height(20).AlignMiddle()
                                .Text("VENTAS/VENTAS TOTALES").FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                .Text("").FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).PaddingRight(10).AlignMiddle()
                                .Text(resumen.data.margenventasnetas.paliados.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                               .Text("").FontSize(8).FontColor("#ff2037").FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                .Text(resumen.data.margenventasnetas.ferreteria.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                .Text("").FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                .Text(resumen.data.margenventasnetas.sriego.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                .Text("").FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                .Text(resumen.data.margenventasnetas.usados.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                .Text("").FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                .Text(resumen.data.margenventasnetas.ol.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                            });

                        });
                    });

                    //PAGINA TOTALES
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
                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text("RESULTADOS POR LINEA DE NEGOCIO").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
                                });
                            });


                        });


                        page.Content().PaddingTop(10).PaddingLeft(30).PaddingRight(30).Column(col1 =>
                        {

                            //col1.Item().LineHorizontal(0.5f);



                            col1.Item().PaddingVertical(10).Border(1).BorderColor("#275027").Table(tabla =>
                            {
                                tabla.ColumnsDefinition(Columns =>
                                {
                                    Columns.RelativeColumn(2);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    
                                });
                                tabla.Header(header =>
                                {
                                    header.Cell().ColumnSpan(7).Height(20).Background("#4f7b58").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("TOTALES").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("CONCEPTO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("SUMA JD + O.L.").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignRight().AlignMiddle()
                                    .Padding(1).Text("STAFF + FINANZAS + ADMON").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("TOTAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");

                                });


                                foreach (var mdl in resumen.data.estadoresultados)
                                {

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Background("#dae6be").PaddingLeft(15).Height(20).AlignMiddle()
                                        .Text(mdl.concepto).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                        .Text(mdl.sumamh.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).PaddingRight(10).AlignMiddle()
                                        .Text(mdl.mrgsumamh.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text(mdl.gastos.ToString("N2")).FontSize(8).FontColor("#ff2037").FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text((mdl.mrggastos).ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text(mdl.sumatotal.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                        .Text((mdl.mrgsumatotal).ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                }
                            });

                            //TABLA INDICADORES
                            col1.Item().PaddingVertical(10).Border(1).BorderColor("#275027").Table(tabla =>
                            {
                                tabla.ColumnsDefinition(Columns =>
                                {
                                    Columns.RelativeColumn(2);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);

                                });
                                tabla.Header(header =>
                                {
                                    header.Cell().ColumnSpan(7).Height(20).Background("#5c8fa4").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("INDICADORES").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                //MARGENES
                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Background("#dae6be").PaddingLeft(15).Height(20).AlignMiddle()
                                    .Text("MARGEN").FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                    .Text("").FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).PaddingRight(10).AlignMiddle()
                                 .Text(resumen.data.indicadores.mrgsumatotal.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                               
                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                .Text("").FontSize(8).FontFamily(fontFamily);
                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                .Text("").FontSize(8).FontFamily(fontFamily);
                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                .Text("").FontSize(8).FontFamily(fontFamily);
                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                .Text("").FontSize(8).FontFamily(fontFamily);


                                //VENTAS TOTALES
                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Background("#dae6be").PaddingLeft(15).Height(20).AlignMiddle()
                                .Text("VENTAS/VENTAS TOTALES").FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                .Text("").FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                .Text(resumen.data.margenventasnetas.total).FontSize(8).FontFamily(fontFamily);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                .Text("").FontSize(8).FontFamily(fontFamily);
                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                .Text("").FontSize(8).FontFamily(fontFamily);
                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                .Text("").FontSize(8).FontFamily(fontFamily);
                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                .Text("").FontSize(8).FontFamily(fontFamily);
                            });

                        });
                    });

                }).GeneratePdf();
                RPT_Result result = new RPT_Result();
                result.extension = "pdf";
                result.nombredocumento = "RESULTADOS POR LINEA DE NEGOCIO";
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
