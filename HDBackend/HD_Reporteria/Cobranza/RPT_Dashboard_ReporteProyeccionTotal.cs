using QuestPDF.Fluent;
using QuestPDF.Helpers;
using HD_Ventas.Modelos;
using ClosedXML.Excel;
using HD_Cobranza.GestionCobranza.Modelos;
using HD_Cobranza.Modelos.Dashboard;

namespace HD_Reporteria.Cobranza
{
    public class RPT_Dashboard_ReporteProyeccionTotal
    {
        public static string obtenernombre_mes(int numeromes)
        {
            switch (numeromes)
            {
                case 1:
                    return "ENERO";
                case 2:
                    return "FEBRERO";
                case 3:
                    return "MARZO";
                case 4:
                    return "ABRIL";
                case 5:
                    return "MAYO";
                case 6:
                    return "JUNIO";
                case 7:
                    return "JULIO";
                case 8:
                    return "AGOSTO";
                case 9:
                    return "SEPTIEMBRE";
                case 10:
                    return "OCTUBRE";
                case 11:
                    return "NOVIEMBRE";
                case 12:
                    return "DICIEMBRE";
                default:
                    return "";

            }
        }

        public static string obtenerCartera(string cartera)
        {
            switch (cartera)
            {
                case "O":
                    return "DE OP.";
                case "R":
                    return "REV.";
                case "E":
                    return "ESP.";
                default:
                    return "";

            }
        }

        public static string FormatearMesAnio(string mes)
        {
            // Verifica que el formato sea correcto
            if (DateTime.TryParseExact(mes, "yyyy-MM", null, System.Globalization.DateTimeStyles.None, out DateTime fecha))
            {
                string nombreMes = obtenernombre_mes(fecha.Month);
                string anio = fecha.Year.ToString();
                return $"{nombreMes} {anio}";
            }
            else
            {
                throw new ArgumentException("El formato de la variable mes no es válido. Debe ser 'yyyy-MM'.");
            }
        }

        public static RPT_Result GenerarPDF(IEnumerable<mdl_Dashboard_Recuperacion_Mensual_Detalle> detalle, int ejercicio, int periodo, string mes, string sucursales, string adr)
        {
            try
            {
                string fontFamily = "Calibri";
                byte[] doc = Document.Create(document =>
                {
                    document.Page(page =>
                    {
                        page.Size(PageSizes.Letter.Portrait());



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

                                row.ConstantColumn(450).PaddingTop(30).Height(50).Background("#477c2c").Row(row2 =>
                                {
                                    row2.RelativeItem().Padding(5).PaddingTop(10).PaddingLeft(20).Text("PROYECCION DE RECUPERACION TOTAL " + FormatearMesAnio(mes)).FontColor("#fff").FontSize(14).Bold().FontFamily(fontFamily);
                                    //+obtenernombre_mes(periodo) + " " + ejercicio
                                });
                            });


                        });

                        page.Content().PaddingTop(10).PaddingLeft(30).PaddingRight(30).Column(col1 =>
                        {

                            //col1.Item().LineHorizontal(0.5f);

                            DateTime fecha = DateTime.Now;
                            string fechaActual = fecha.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("es-ES"));

                            col1.Item().Row(row =>
                            {
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
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(1.2f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.6f);
                                    Columns.RelativeColumn(1);
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().BorderBottom(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("SUCURSAL").FontSize(9).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderBottom(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("CLIENTE").FontSize(9).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderBottom(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("IMP. FACTURA").FontSize(9).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderBottom(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("PAGADO").FontSize(9).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderBottom(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("SALDO").FontSize(9).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                foreach (var det in detalle)
                                {

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignLeft().MaxHeight(60).AlignMiddle().PaddingLeft(4).PaddingRight(3).PaddingVertical(3).ShowEntire()
                                    .Text(det.sucursal?.ToUpper()).FontSize(9).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignLeft().MaxHeight(60).AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                   .Text(det.razonsocial?.ToUpper()).FontSize(9).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().MaxHeight(60).AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                   .Text(det.importe_factura.ToString("N2")).FontSize(9).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().MaxHeight(60).AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                   .Text(det.pagado.ToString("N2")).FontSize(9).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().MaxHeight(60).AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                   .Text(det.saldo.ToString("N2")).FontSize(9).FontFamily(fontFamily);
                                }
                            });
                        });

                        page.Footer().Height(60).PaddingLeft(30).PaddingRight(30).PaddingBottom(10).Row(row =>
                        {
                            row.RelativeItem().AlignRight().PaddingTop(20).Text(txt =>
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
                result.nombredocumento = "REPORTE PROYECCION TOTAL";
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
