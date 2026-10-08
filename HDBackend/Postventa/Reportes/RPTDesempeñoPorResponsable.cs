using HD_Cobranza.Modelos;
using HD_Reporteria;
using Postventa.Modelos;
using Postventa.Modelos.Indicadores;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Postventa.Reportes
{
    public class RPTDesempeñoPorResponsable
    {
        /// <summary>
        /// Sobrecarga que recibe el filtro y arma el texto del periodo, para que el
        /// controlador no tenga que formatear nada.
        /// </summary>
        public static RPT_Result GenerarPDF(List<mdl_Indicadores_Detalle> detalle,
                                            int ejercicio_inicio, int ejercicio_fin,
                                            int periodo_inicio, int periodo_fin)
        {
            return GenerarPDF(detalle, ArmarPeriodo(ejercicio_inicio, ejercicio_fin, periodo_inicio, periodo_fin));
        }

        private static string ArmarPeriodo(int ejercicioInicio, int ejercicioFin, int periodoInicio, int periodoFin)
        {
            var cultura = new System.Globalization.CultureInfo("es-MX");

            string MesNombre(int mes)
            {
                if (mes < 1 || mes > 12) return "";
                string nombre = cultura.DateTimeFormat.GetMonthName(mes);
                return char.ToUpper(nombre[0], cultura) + nombre.Substring(1);
            }

            string mesIni = MesNombre(periodoInicio);
            string mesFin = MesNombre(periodoFin);

            // Ejercicios distintos: el año tiene que ir en los dos extremos
            if (ejercicioInicio != ejercicioFin)
                return periodoInicio == periodoFin
                    ? $"{mesIni} {ejercicioInicio} - {ejercicioFin}"
                    : $"{mesIni} {ejercicioInicio} - {mesFin} {ejercicioFin}";

            // Mismo ejercicio: el año va una sola vez al final
            return periodoInicio == periodoFin
                ? $"{mesIni} {ejercicioFin}"
                : $"{mesIni} - {mesFin} {ejercicioFin}";
        }

        public static RPT_Result GenerarPDF(List<mdl_Indicadores_Detalle> detalle, string periodo = "")
        {
            try
            {
                string fontFamily = "Calibri";

                detalle ??= new List<mdl_Indicadores_Detalle>();

                /* ------------------------------------------------------------------
                   Se agrupa por responsable: cada grupo imprime su renglon con los
                   totales y debajo los clientes que los componen. El orden es el
                   mismo del tablero: primero quien mas facturo via mensajeria.
                   ------------------------------------------------------------------ */
                var grupos = detalle
                    .GroupBy(d => new { d.idresponsable, d.Responsable })
                    .Select(g => new
                    {
                        Responsable = g.Key.Responsable,
                        // Si todos los clientes del responsable son de la misma
                        // sucursal se muestra; si trae varias, se dice asi.
                        Sucursal = g.Select(x => x.Sucursal).Distinct().Count() == 1
                                        ? g.First().Sucursal
                                        : "VARIAS",
                        FacturadosTotal = g.Sum(x => x.FacturadosTotal),
                        MontoTotal = g.Sum(x => x.MontoTotal),
                        MensajesEnviados = g.Sum(x => x.MensajesEnviados),
                        FacturadosMensajeria = g.Sum(x => x.FacturadosMensajeria),
                        MontoMensajeria = g.Sum(x => x.MontoMensajeria),
                        Clientes = g.OrderByDescending(x => x.MontoMensajeria)
                                    .ThenByDescending(x => x.MontoTotal)
                                    .ToList()
                    })
                    .OrderByDescending(g => g.MontoMensajeria)
                    .ThenByDescending(g => g.MontoTotal)
                    .ThenBy(g => g.Responsable)
                    .ToList();

                // Totales del pie. Se suman aqui para que cuadren siempre con lo que
                // la tabla acaba de imprimir, aun cuando el detalle llegue filtrado.
                int totFacturados = detalle.Sum(d => d.FacturadosTotal);
                decimal totMontoTotal = detalle.Sum(d => d.MontoTotal);
                int totMensajes = detalle.Sum(d => d.MensajesEnviados);
                int totFactMensajeria = detalle.Sum(d => d.FacturadosMensajeria);
                decimal totMontoMensajeria = detalle.Sum(d => d.MontoMensajeria);

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

                            var rutaImagen = Path.Combine("C:\\Nube\\HumayaDigital\\HumayaDigitalBackEnd\\HDBackend\\HD_Reporteria\\Imagenes\\Logo.jpg");
                            byte[] imageData = System.IO.File.ReadAllBytes(rutaImagen);
                            row.ConstantItem(120).Image(imageData);

                            row.ConstantItem(693).PaddingTop(35).Height(50).Background("#477c2c").Row(row2 =>
                            {
                                row2.RelativeItem().Padding(10).PaddingLeft(30).Text("INDICADORES DE POSVENTA").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
                            });
                        });

                        page.Content().PaddingTop(10).PaddingLeft(30).PaddingRight(30).Column(col1 =>
                        {
                            col1.Item().Row(row =>
                            {
                                row.RelativeItem().AlignCenter().Text(txt =>
                                {
                                    txt.Span("DESEMPEÑO POR RESPONSABLE").FontSize(12).Bold();
                                });
                            });

                            System.DateTime fecha = System.DateTime.Now;
                            string fechaActual = fecha.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("es-ES"));

                            col1.Item().Row(row =>
                            {
                                row.RelativeItem().AlignLeft().Text(txt =>
                                {
                                    if (!string.IsNullOrWhiteSpace(periodo))
                                    {
                                        txt.Span("PERIODO: ").Bold().FontSize(8);
                                        txt.Span(periodo).FontSize(8);
                                    }
                                });

                                row.RelativeItem().AlignRight().Text(txt =>
                                {
                                    txt.Span("INFORMACION AL: ").Bold().FontSize(8);
                                    txt.Span(fechaActual).FontSize(8);
                                });
                            });

                            col1.Item().PaddingVertical(10).Border(1).BorderColor("#477c2c").Table(tabla =>
                            {
                                tabla.ColumnsDefinition(Columns =>
                                {
                                    Columns.RelativeColumn(2.6f);   // RESPONSABLE / CLIENTE
                                    Columns.RelativeColumn(1.2f);   // SUCURSAL
                                    Columns.RelativeColumn(1f);     // FACTURACION TOTAL
                                    Columns.RelativeColumn(1.3f);   // IMPORTE FACTURADO
                                    Columns.RelativeColumn(1f);     // MENSAJES ENVIADOS
                                    Columns.RelativeColumn(1.2f);   // FACTURADO VIA MENSAJERIA
                                    Columns.RelativeColumn(1.3f);   // IMPORTE VIA MENSAJERIA
                                });

                                // Dentro de tabla.Header para que se repita en cada pagina:
                                // el detalle puede partirse en varias hojas.
                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#477c2c").AlignLeft().Height(26).AlignMiddle().PaddingLeft(4)
                                    .Text("RESPONSABLE").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#477c2c").AlignLeft().Height(26).AlignMiddle().PaddingLeft(4)
                                    .Text("SUCURSAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#477c2c").AlignCenter().Height(26).AlignMiddle().Padding(2)
                                    .Text("FACTURACION TOTAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#477c2c").AlignRight().Height(26).AlignMiddle().PaddingRight(4)
                                    .Text("IMPORTE FACTURADO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#477c2c").AlignCenter().Height(26).AlignMiddle().Padding(2)
                                    .Text("MENSAJES ENVIADOS").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#477c2c").AlignCenter().Height(26).AlignMiddle().Padding(2)
                                    .Text("FACTURADO VIA MENSAJERIA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#477c2c").AlignRight().Height(26).AlignMiddle().PaddingRight(4)
                                    .Text("IMPORTE VIA MENSAJERIA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                if (grupos.Count == 0)
                                {
                                    tabla.Cell().ColumnSpan(7).BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(28).AlignMiddle()
                                    .Text("SIN INFORMACION PARA EL PERIODO SELECCIONADO").FontSize(8).Italic().FontFamily(fontFamily).FontColor(Colors.Grey.Darken1);
                                }

                                foreach (var grupo in grupos)
                                {
                                    /* --- Renglon del RESPONSABLE con sus totales --- */
                                    tabla.Cell().BorderTop(1).BorderColor("#477c2c").Background("#eef1e9").AlignLeft().Height(22).AlignMiddle().PaddingLeft(4)
                                    .Text(grupo.Responsable).FontSize(8).Bold().FontFamily(fontFamily);

                                    tabla.Cell().BorderTop(1).BorderColor("#477c2c").Background("#eef1e9").AlignLeft().Height(22).AlignMiddle().PaddingLeft(4)
                                    .Text(grupo.Sucursal).FontSize(8).Bold().FontFamily(fontFamily);

                                    tabla.Cell().BorderTop(1).BorderColor("#477c2c").Background("#eef1e9").AlignCenter().Height(22).AlignMiddle().Padding(2)
                                    .Text(grupo.FacturadosTotal.ToString("N0")).FontSize(8).Bold().FontFamily(fontFamily);

                                    tabla.Cell().BorderTop(1).BorderColor("#477c2c").Background("#eef1e9").AlignRight().Height(22).AlignMiddle().PaddingRight(4)
                                    .Text("$ " + grupo.MontoTotal.ToString("N2")).FontSize(8).Bold().FontFamily(fontFamily);

                                    tabla.Cell().BorderTop(1).BorderColor("#477c2c").Background("#eef1e9").AlignCenter().Height(22).AlignMiddle().Padding(2)
                                    .Text(grupo.MensajesEnviados.ToString("N0")).FontSize(8).Bold().FontFamily(fontFamily);

                                    tabla.Cell().BorderTop(1).BorderColor("#477c2c").Background("#eef1e9").AlignCenter().Height(22).AlignMiddle().Padding(2)
                                    .Text(grupo.FacturadosMensajeria.ToString("N0")).FontSize(8).Bold().FontFamily(fontFamily);

                                    tabla.Cell().BorderTop(1).BorderColor("#477c2c").Background("#eef1e9").AlignRight().Height(22).AlignMiddle().PaddingRight(4)
                                    .Text("$ " + grupo.MontoMensajeria.ToString("N2")).FontSize(8).Bold().FontFamily(fontFamily);

                                    /* --- Clientes que componen ese total --- */
                                    foreach (var cliente in grupo.Clientes)
                                    {
                                        tabla.Cell().BorderBottom(1).BorderColor("#dfe3d8").AlignLeft().Height(20).AlignMiddle().PaddingLeft(18).PaddingRight(3)
                                        .Text(cliente.Cliente).FontSize(8).FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#dfe3d8").AlignLeft().Height(20).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                        .Text(cliente.Sucursal).FontSize(8).FontFamily(fontFamily).FontColor(Colors.Grey.Darken1);

                                        tabla.Cell().BorderBottom(1).BorderColor("#dfe3d8").AlignCenter().Height(20).AlignMiddle().Padding(2)
                                        .Text(cliente.FacturadosTotal.ToString("N0")).FontSize(8).FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#dfe3d8").AlignRight().Height(20).AlignMiddle().PaddingRight(4)
                                        .Text("$ " + cliente.MontoTotal.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#dfe3d8").AlignCenter().Height(20).AlignMiddle().Padding(2)
                                        .Text(cliente.MensajesEnviados.ToString("N0")).FontSize(8).FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#dfe3d8").AlignCenter().Height(20).AlignMiddle().Padding(2)
                                        .Text(cliente.FacturadosMensajeria.ToString("N0")).FontSize(8).FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#dfe3d8").AlignRight().Height(20).AlignMiddle().PaddingRight(4)
                                        .Text("$ " + cliente.MontoMensajeria.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                    }
                                }

                                /* --- Renglon de TOTALES --- */
                                tabla.Cell().ColumnSpan(2).BorderTop(1).BorderColor("#477c2c").Background("#477c2c").AlignLeft().Height(24).AlignMiddle().PaddingLeft(4)
                                .Text("TOTALES").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");

                                tabla.Cell().BorderTop(1).BorderColor("#477c2c").Background("#477c2c").AlignCenter().Height(24).AlignMiddle().Padding(2)
                                .Text(totFacturados.ToString("N0")).FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");

                                tabla.Cell().BorderTop(1).BorderColor("#477c2c").Background("#477c2c").AlignRight().Height(24).AlignMiddle().PaddingRight(4)
                                .Text("$ " + totMontoTotal.ToString("N2")).FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");

                                tabla.Cell().BorderTop(1).BorderColor("#477c2c").Background("#477c2c").AlignCenter().Height(24).AlignMiddle().Padding(2)
                                .Text(totMensajes.ToString("N0")).FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");

                                tabla.Cell().BorderTop(1).BorderColor("#477c2c").Background("#477c2c").AlignCenter().Height(24).AlignMiddle().Padding(2)
                                .Text(totFactMensajeria.ToString("N0")).FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");

                                tabla.Cell().BorderTop(1).BorderColor("#477c2c").Background("#477c2c").AlignRight().Height(24).AlignMiddle().PaddingRight(4)
                                .Text("$ " + totMontoMensajeria.ToString("N2")).FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                            });
                        });

                        page.Footer().Height(40).PaddingLeft(30).PaddingRight(30).PaddingBottom(20).Row(row =>
                        {
                            row.RelativeItem().AlignRight().PaddingTop(0).Text(txt =>
                            {
                                txt.Span("Pág. ").FontSize(10).FontFamily("arial");
                                txt.CurrentPageNumber().FontSize(10).Bold().FontFamily("arial");
                                txt.Span(" de ").FontSize(10).FontFamily("arial");
                                txt.TotalPages().FontSize(10).Bold().FontFamily("arial");
                            });
                        });
                    });

                }).GeneratePdf();

                RPT_Result result = new RPT_Result();
                result.extension = "pdf";
                result.nombredocumento = "DESEMPEÑO POR RESPONSABLE";
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