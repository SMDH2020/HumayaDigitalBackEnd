using HD_Finanzas.Modelos.Estado_Resultados;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace HD_Reporteria.Finanzas
{
    public class RPT_Finanzas_EstadoResultados
    {
        private static string GetIndicadorColor(string indicador)
        {
            return indicador switch
            {
                "V" => "#28a745",
                "A" => "#ffc107",
                "R" => "#dc3545",
                _ => ""
            };
        }
        // color rojo para negativos FontColor("#ff2037")
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

                            if (resumen.region.Count >= 1)
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
                                                txt2.Span("TODO EL GRUPO").FontSize(8).FontFamily(fontFamily);
                                            else
                                                foreach (var reg in resumen.region)
                                                    txt2.Span(reg.adr).FontSize(8).FontFamily(fontFamily);
                                        });
                                    });
                                });
                            }

                            if (resumen.sucursal.Count >= 1)
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
                                                    txt2.Span(resumen.sucursal[i].sucursal + ", ").FontSize(8).FontFamily(fontFamily);
                                                else
                                                    txt2.Span(resumen.sucursal[i].sucursal).FontSize(8).FontFamily(fontFamily);
                                            }
                                        });
                                    });
                                });
                            }

                            col1.Item().PaddingVertical(10).Border(1).BorderColor("#275027").Table(tabla =>
                            {
                                // ← Columna extra para el semáforo (0.3f) entre Real y %
                                tabla.ColumnsDefinition(Columns =>
                                {
                                    Columns.RelativeColumn(2);    // Concepto
                                    Columns.RelativeColumn(1);    // Real
                                    Columns.RelativeColumn(0.3f); // 🚦 Semáforo  ← NUEVA
                                    Columns.RelativeColumn(0.4f); // %
                                    Columns.RelativeColumn(1);    // Proyección
                                    Columns.RelativeColumn(0.4f); // %
                                    Columns.RelativeColumn(1);    // Desviación
                                    Columns.RelativeColumn(0.4f); // %
                                    Columns.RelativeColumn(1);    // Real anterior
                                    Columns.RelativeColumn(0.4f); // %
                                    Columns.RelativeColumn(1);    // Desviación anterior
                                    Columns.RelativeColumn(0.4f); // %
                                });

                                tabla.Header(header =>
                                {
                                    // Fila 1 — spans ajustados: periodo actual ahora cubre 7 cols (sumó la del semáforo)
                                    header.Cell().ColumnSpan(1).Height(25).Background("#275027").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("").FontSize(12).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(7).Height(25).Background("#275027").AlignCenter().AlignMiddle()
                                        .Padding(1).Text(resumen.periodoactual).FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(4).Height(25).Background("#275027").AlignMiddle()
                                        .Padding(1).Text(resumen.periodoanterior).FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");

                                    // Fila 2 — etiquetas de columna
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                        .Padding(1).Text("CONCEPTO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("REAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    // Encabezado semáforo — icono unicode de círculo
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("●").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
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

                                foreach (var mdl in resumen.data)
                                {
                                    // Fila de título de departamento — ahora 12 columnas
                                    tabla.Cell().ColumnSpan(12).BorderTop(1).BorderColor("#afb69d")
                                        .Background("#e8f0e8").PaddingLeft(15).Height(20).AlignMiddle()
                                        .Text(mdl.depto).FontColor("#1a2e1a").FontSize(8).FontFamily(fontFamily).Bold().LetterSpacing(0.6f);

                                    for (int i = 0; i < mdl.data.Count; i++)
                                    {
                                        var ln = mdl.data[i];
                                        string semaforoColor = GetIndicadorColor(ln.indicador);

                                        // Concepto
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d")
                                            .PaddingLeft(15).Height(20).AlignMiddle()
                                            .Text(ln.concepto).FontSize(8).FontFamily(fontFamily).FontColor("#333333");

                                        // Real
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d")
                                            .PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                            .Text(ln.importe.ToString("N2")).FontSize(8).FontFamily(fontFamily).FontColor("#333333");

                                        // 🚦 Semáforo — círculo unicode coloreado
                                        if (!string.IsNullOrEmpty(semaforoColor))
                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d")
                                                .AlignCenter().Height(20).AlignMiddle()
                                                .Text("●").FontSize(14).FontColor(semaforoColor).FontFamily(fontFamily);
                                        else
                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d")
                                                .Height(20).Element(e => { });

                                        // % Real
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d")
                                            .AlignRight().Height(20).PaddingRight(10).AlignMiddle()
                                            .Text(ln.por.ToString("N2")).FontSize(8).FontFamily(fontFamily).FontColor("#333333");

                                        // Proyección
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d")
                                            .AlignRight().Height(20).AlignMiddle()
                                            .Text(ln.proyimporte.ToString("N2")).FontSize(8).FontColor("#333333").FontFamily(fontFamily);

                                        // % Proyección
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d")
                                            .AlignRight().Height(20).AlignMiddle()
                                            .Text(ln.proypor.ToString("N2")).FontSize(8).FontFamily(fontFamily).FontColor("#333333");

                                        // Desviación
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d")
                                            .AlignRight().Height(20).AlignMiddle()
                                            .Text(ln.diffimporte.ToString("N2")).FontSize(8).FontFamily(fontFamily).FontColor("#333333");

                                        // % Desviación
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d")
                                            .AlignRight().Height(20).AlignMiddle()
                                            .Text(ln.diffpor.ToString("N2")).FontSize(8).FontFamily(fontFamily).FontColor("#333333");

                                        // Real anterior
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d")
                                            .AlignRight().Height(20).AlignMiddle()
                                            .Text(ln.lastimporte.ToString("N2")).FontSize(8).FontFamily(fontFamily).FontColor("#333333");

                                        // % Real anterior
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d")
                                            .AlignRight().Height(20).AlignMiddle()
                                            .Text(ln.lastpor.ToString("N2")).FontSize(8).FontFamily(fontFamily).FontColor("#333333");

                                        // Desviación anterior
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d")
                                            .AlignRight().Height(20).AlignMiddle()
                                            .Text(ln.lastdiffimporte.ToString("N2")).FontSize(8).FontFamily(fontFamily).FontColor("#333333");

                                        // % Desviación anterior
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d")
                                            .AlignRight().Height(20).AlignMiddle()
                                            .Text(ln.lastdiffpor.ToString("N2")).FontSize(8).FontFamily(fontFamily).FontColor("#333333");
                                    }
                                }
                            });
                        });
                    });
                }).GeneratePdf();

                return new RPT_Result
                {
                    extension = "pdf",
                    nombredocumento = "ESTADO DE RESULTADOS",
                    documento = Convert.ToBase64String(doc)
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}