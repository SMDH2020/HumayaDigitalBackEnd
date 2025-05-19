using QuestPDF.Fluent;
using QuestPDF.Helpers;
using ClosedXML.Excel;
using HD_Ventas.Modelos;
using System.Globalization;
using Newtonsoft.Json;
using HD.Clientes.Consultas.Clientes;
using System.ComponentModel;
using DocumentFormat.OpenXml.Spreadsheet;

namespace HD_Reporteria.Cotizaciones
{
    public class RPT_Cotizacion
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

        public static string obtenerLinea(string linea)
        {
            switch (linea)
            {
                case "O":
                    return "OPERACION";
                case "R":
                    return "REVOLVENTE";
                case "E":
                    return "ESPECIAL";
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

        public static RPT_Result GenerarPDF(IEnumerable<mdl_Cotizacion_Imprimir> detalle)
        {
            try
            {
                string fontFamily = "Calibri";
                var modelos = JsonConvert.DeserializeObject<List<mdl_Detalle_Cotizacion_Imprimir>>(detalle.FirstOrDefault().detalle);
                double sumaPrecioLista = modelos.Sum(m => m.precio_lista);
                double sumaDescuento = modelos.Sum(m => m.descuento);
                double sumaPrecioTotal = modelos.Sum(m => m.precio_total);

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
                                    row2.RelativeItem().Padding(10).PaddingLeft(10).Text("COTIZACION DE EQUIPO").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
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
                                row.RelativeItem().Text(txt => {
                                    txt.Span("Folio: ").FontSize(9).Bold();
                                    txt.Span(detalle.First().folio).FontSize(9);
                                });
                            });

                            col1.Item().PaddingBottom(5).Row(row =>
                            {
                                row.RelativeItem().Text(txt => {
                                    txt.Span("Asesor de venta: ").FontSize(9).Bold();
                                    txt.Span(detalle.First().asesorventa).FontSize(9);
                                });
                            });


                            col1.Item().Border(1).Padding(5).BorderColor(QuestPDF.Helpers.Colors.Black).Column(innerCol =>
                            {
                                innerCol.Item().Row(row =>
                                {
                                    row.RelativeItem().Text(txt => {
                                        txt.Span("Apreciable: ").FontSize(9).Bold();
                                        txt.Span(detalle.First().razon_social).FontSize(9);
                                    });
                                });

                                innerCol.Item().Row(row =>
                                {
                                    row.RelativeItem().Text(txt => {
                                        txt.Span("Dirección: ").FontSize(9).Bold();
                                        txt.Span(detalle.First().direccion).FontSize(9);
                                    });
                                });

                                innerCol.Item().Row(row =>
                                {
                                    row.RelativeItem().Text(txt => { 
                                        txt.Span("Sucursal: ").FontSize(9).Bold(); 
                                        txt.Span(detalle.First().sucursal).FontSize(9);
                                    });
                                    row.RelativeItem().AlignRight().Text(txt => {
                                        txt.Span("Informacion al: ").FontSize(9).Bold();
                                        txt.Span(fechaActual).FontSize(9);
                                    });
                                });
                            });

                            col1.Item().Row(row =>
                            {
                                row.RelativeItem().PaddingTop(5).AlignLeft().Text(txt => {
                                    txt.Span("De acuerdo a su amable solicitud tenemos el gusto de cotizar a usted lo siguiente, con vigencia al: ").FontSize(9);
                                    txt.Span(detalle.First().vigencia).FontSize(9).Bold();
                                });
                                //row.RelativeItem().AlignRight().Text("Vigencia al: " + detalle.First().vigencia).FontSize(9);

                            });


                            col1.Item().PaddingVertical(10).Border(1).BorderColor("#477c2c").Table(tabla =>
                            {
                                tabla.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(0.1f);
                                    columns.RelativeColumn(1.5f); 
                                    columns.RelativeColumn(1); 
                                });

                                // Header
                                tabla.Header(header =>
                                {
                                    header.Cell().ColumnSpan(1).BorderBottom(1).BorderColor("#fedb05")
                                       .Background("#477c2c").Height(20).AlignMiddle()
                                       .Padding(4).Text("").FontSize(9).Bold().FontFamily(fontFamily).FontColor("#fff");

                                    header.Cell().ColumnSpan(1).BorderBottom(1).BorderColor("#fedb05")
                                        .Background("#477c2c").Height(20).AlignMiddle()
                                        .Padding(4).Text("MODELO / DESCRIPCION").FontSize(9).Bold().FontFamily(fontFamily).FontColor("#fff");

                                    header.Cell().ColumnSpan(1).BorderBottom(1).BorderColor("#fedb05")
                                        .Background("#477c2c").AlignRight().Height(20).AlignMiddle()
                                        .Padding(4).Text("IMPORTES").FontSize(9).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                int c = 1;
                                // Filas con datos
                                foreach (var modelo in modelos)
                                {
                                    tabla.Cell().BorderBottom(1).BorderRight(1).BorderColor("#477c2c").Padding(5).Column(column =>
                                    {
                                        column.Item().Text(c.ToString()).FontSize(9).AlignCenter();
                                    });
                                    tabla.Cell().BorderBottom(1).BorderColor("#477c2c").Padding(5).Column(column =>
                                    {
                                        column.Item().Text(modelo.modelo + " / " + modelo.descripcion).Bold().FontSize(9).FontFamily(fontFamily);
                                        column.Item().PaddingVertical(0.5f).Text("Características del equipo: ").FontSize(9).FontFamily(fontFamily);
                                        column.Item().PaddingTop(2).Column(inner =>
                                        {
                                            foreach (var carac in modelo.caracteristicas_json)
                                            {
                                                inner.Item().Text("• " + carac.descripcion).FontSize(8);
                                            }
                                        });
                                    });

                                    tabla.Cell().BorderBottom(1).BorderColor("#477c2c").AlignBottom().Padding(5).Column(column =>
                                    {
                                        column.Item().AlignRight().Text(txt => {
                                            txt.Span("Precio de lista: ").FontSize(9).Bold();
                                            txt.Span(modelo.precio_lista.ToString("N0")).FontSize(9);
                                        });
                                        column.Item().AlignRight().Text(txt => {
                                            txt.Span("Descuento: ").FontSize(9).Bold();
                                            txt.Span(modelo.descuento.ToString("N0")).FontSize(9);
                                        });
                                        column.Item().AlignRight().Text(txt => {
                                            txt.Span("Precio total: ").FontSize(9).Bold();
                                            txt.Span(modelo.precio_total.ToString("N0")).FontSize(9);
                                        });
                                        column.Item().AlignRight().Text(txt => {
                                            txt.Span("Moneda: ").FontSize(9).Bold();
                                            txt.Span(modelo.moneda).FontSize(9);
                                        });
                                    });

                                    c++;
                                }
                            });
                            col1.Item().BorderBottom(1).Padding(5).BorderColor(QuestPDF.Helpers.Colors.Black).Column(innerCol =>
                            {
                                innerCol.Item().Row(row =>
                                {
                                    row.RelativeItem().AlignRight().Text(txt => {
                                        txt.Span("Subtotal: ").FontSize(9).Bold();
                                        txt.Span(sumaPrecioLista.ToString("N0")).FontSize(9);
                                    });
                                });

                                innerCol.Item().Row(row =>
                                {
                                    row.RelativeItem().AlignRight().Text(txt => {
                                        txt.Span("Descuento: ").FontSize(9).Bold();
                                        txt.Span(sumaDescuento.ToString("N0")).FontSize(9);
                                    });
                                });

                                innerCol.Item().Row(row =>
                                {
                                    row.RelativeItem().AlignRight().Text(txt => {
                                        txt.Span("Total: ").FontSize(9).Bold();
                                        txt.Span(sumaPrecioTotal.ToString("N0")).FontSize(9);
                                    });
                                });
                            });

                            col1.Item().Row(row =>
                            {
                                row.RelativeItem().Text("Estos precios son netos y de contado quedan sujetos a cambio sin previo aviso, prevaleciendo los que estén en vigor al momento de facturar.").FontSize(9);
                            });

                            col1.Item().PaddingTop(100).Row(row =>
                            {
                                // Firma 1
                                //row.RelativeItem().AlignCenter().Column(col =>
                                //{
                                //    col.Item().Width(150).LineHorizontal(1);
                                //    col.Item().Text("ATENTAMENTE").FontSize(9).AlignCenter();
                                //    col.Item().Text("IVÁN LÓPEZ").FontSize(9).AlignCenter();
                                //});

                                // Firma 2
                                row.RelativeItem().AlignCenter().Column(col =>
                                {
                                    col.Item().Width(150).LineHorizontal(1);
                                    col.Item().Text("ATENDIÓ").FontSize(9).AlignCenter();
                                    col.Item().Text(detalle.First().asesorventa.ToUpper()).FontSize(9).AlignCenter();
                                });

                                // Firma 3
                                row.RelativeItem().PaddingTop(1).AlignCenter().Column(col =>
                                {
                                    //col.Item().Width(150).LineHorizontal(1);
                                    col.Item().Text("VALIDO HASTA").FontSize(9).AlignCenter();
                                    col.Item().Text(detalle.First().vigencia).FontSize(9).AlignCenter();

                                });
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
                result.nombredocumento = "COTIZACION";
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
