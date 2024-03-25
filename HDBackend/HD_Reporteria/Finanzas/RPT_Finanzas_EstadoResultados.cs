using HD_Finanzas.Modelos.Estado_Resultados;
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
    public class RPT_Finanzas_EstadoResultados
    {
        public static RPT_Result Generar(Fmdl_EstadoResultados_PDF resumen)
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
                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text("ESTADO DE RESULTADOS").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
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
                                    header.Cell().ColumnSpan(1).Height(25).Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("").FontSize(12).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(6).Height(25).Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text(resumen.periodoactual).FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(4).Height(25).Background("#275027").AlignMiddle()
                                    .Padding(1).Text(resumen.periodoanterior).FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("CONCEPTO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("REAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignRight().AlignMiddle()
                                    .Padding(1).Text("PROYECCION").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("DESVIACION").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("REAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("DESVIACION").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
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

                                    tabla.Cell().ColumnSpan(11).BorderTop(1).BorderColor("#afb69d").Background("#ccc").PaddingLeft(15).Height(20).AlignCenter().AlignMiddle()
                                        .Text(mdl.depto).FontSize(8).FontFamily(fontFamily).Bold();

                                    for (int i = 0; i < mdl.data.Count; i++)
                                    {
                                        var ln = mdl.data[i];
                                        var isLastRow = (i == mdl.data.Count - 1); // Check if it's the last row

                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(15).Height(20).AlignMiddle()
                                            .Text(ln.concepto).FontSize(8).FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                            .Text(ln.importe.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).PaddingRight(10).AlignMiddle()
                                            .Text((ln.por).ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                            .Text(ln.proyimporte.ToString("N2")).FontSize(8).FontColor("#ff2037").FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                            .Text((ln.proypor).ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                            .Text(ln.diffimporte.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                            .Text((ln.diffpor).ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                            .Text(ln.lastimporte.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                            .Text(ln.lastpor.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                            .Text(ln.lastdiffimporte.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                            .Text(ln.lastdiffpor.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        if (isLastRow) // Apply bold to the last row
                                        {
                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(15).Height(20).AlignMiddle()
                                            .Text(ln.concepto).FontSize(8).FontFamily(fontFamily).Bold();

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                                .Text(ln.importe.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).PaddingRight(10).AlignMiddle()
                                                .Text((ln.por).ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                                .Text(ln.proyimporte.ToString("N2")).FontSize(8).FontColor("#ff2037").FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                                .Text((ln.proypor).ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                                .Text(ln.diffimporte.ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                                .Text((ln.diffpor).ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                                .Text(ln.lastimporte.ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                                .Text(ln.lastpor.ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                                .Text(ln.lastdiffimporte.ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                                .Text(ln.lastdiffpor.ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();

                                        }
                                    }

                                }
                            });

                        });

                    });

                }).GeneratePdf();
                RPT_Result result = new RPT_Result();
                result.extension = "pdf";
                result.nombredocumento = "ESTADO DE RESULTADOS";
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

