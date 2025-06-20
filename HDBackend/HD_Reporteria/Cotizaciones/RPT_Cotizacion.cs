using QuestPDF.Fluent;
using QuestPDF.Helpers;
using ClosedXML.Excel;
using HD_Ventas.Modelos;
using System.Globalization;
using Newtonsoft.Json;
using HD.Clientes.Consultas.Clientes;
using System.ComponentModel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using HD.Clientes.Consultas.Modelos;

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

        static bool EsBase64Valido(string base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String))
                return false;

            base64String = base64String.Trim();

            if (base64String.Contains(","))
            {
                base64String = base64String.Split(',')[1];
            }

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch
            {
                return false;
            }
        }

        static byte[] ObtenerImagenDesdeBase64(string base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String))
                return null;

            base64String = base64String.Trim();

            if (base64String.Contains(","))
            {
                base64String = base64String.Split(',')[1]; // Extrae la parte base64
            }

            try
            {
                return Convert.FromBase64String(base64String);
            }
            catch
            {
                return null;
            }
        }

        static string LimpiarBase64SiEsJson(string posibleJson)
        {
            try
            {
                // Intentar deserializar por si viene como JSON con "documento"
                var obj = JsonConvert.DeserializeObject<DocumentoImagen>(posibleJson);

                if (obj?.documento != null)
                {
                    var base64Completo = obj.documento;
                    int comaIndex = base64Completo.IndexOf(',');
                    return comaIndex != -1 ? base64Completo.Substring(comaIndex + 1) : base64Completo;
                }
            }
            catch
            {
                // No era JSON o deserialización falló, retornamos el original
            }

            return posibleJson; // Retorna tal cual si no era JSON
        }

        private class DocumentoImagen
        {
            public string documento { get; set; }
        }


        public static RPT_Result GenerarPDF(IEnumerable<mdl_Cotizacion_Imprimir> detalle)
        {
            try
            {
                string fontFamily = "Calibri";
                List<mdl_Detalle_Cotizacion_Imprimir> modelos = new();
                string jsonEscapado = detalle.FirstOrDefault()?.detalle;
                if (!string.IsNullOrWhiteSpace(jsonEscapado))
                {
                    modelos = JsonConvert.DeserializeObject<List<mdl_Detalle_Cotizacion_Imprimir>>(jsonEscapado);
                }
                double sumaPrecioLista = modelos.Sum(m => m.precio_lista);
                double sumaDescuento = modelos.Sum(m => m.descuento);
                double sumaPrecioTotal = modelos.Sum(m => m.precio_promocion);

                byte[] doc = QuestPDF.Fluent.Document.Create(document =>
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

                            //col1.Item().Row(row =>
                            //{
                            //    row.RelativeItem().Text(txt => {
                            //        txt.Span("Folio: ").FontSize(9).Bold();
                            //        txt.Span(detalle.First().folio).FontSize(9);
                            //    });
                            //});

                            //col1.Item().PaddingBottom(5).Row(row =>
                            //{
                            //    row.RelativeItem().Text(txt => {
                            //        txt.Span("Asesor de venta: ").FontSize(9).Bold();
                            //        txt.Span(detalle.First().asesorventa).FontSize(9);
                            //    });
                            //});


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

                                innerCol.Item().Row(row =>
                                {
                                    row.RelativeItem().Text(txt => {
                                        txt.Span("Folio: ").FontSize(9).Bold();
                                        txt.Span(detalle.First().folio).FontSize(9);
                                    });
                                    row.RelativeItem().AlignRight().Text(txt =>
                                    {
                                        txt.Span("Moneda: ").FontSize(9).Bold();
                                        txt.Span(modelos.FirstOrDefault().moneda).FontSize(9);
                                    });

                                });

                                innerCol.Item().Row(row =>
                                {
                                    row.RelativeItem().Text(txt => {
                                        txt.Span("Asesor de venta: ").FontSize(9).Bold();
                                        txt.Span(detalle.First().asesorventa.ToUpper()).FontSize(9);
                                    });

                                    row.RelativeItem().AlignRight().Text(txt => {
                                        txt.Span("Esquema de Pago: ").FontSize(9).Bold();
                                        txt.Span(modelos.FirstOrDefault().descripcion_promocion).FontSize(9);
                                    });
                                });
                                innerCol.Item().PaddingTop(5).Row(row =>
                                {
                                    row.RelativeItem().Text(txt => {
                                        txt.Span("De acuerdo a su amable solicitud tenemos el gusto de cotizar a usted lo siguiente, con vigencia al: ").FontSize(9);
                                        txt.Span(detalle.First().vigencia).FontSize(9).Bold();
                                    });
                                });
                            });

                            //col1.Item().Row(row =>
                            //{
                            //    row.RelativeItem().PaddingTop(5).AlignLeft().Text(txt => {
                            //        txt.Span("De acuerdo a su amable solicitud tenemos el gusto de cotizar a usted lo siguiente, con vigencia al: ").FontSize(9);
                            //        txt.Span(detalle.First().vigencia).FontSize(9).Bold();
                            //    });
                            //    //row.RelativeItem().AlignRight().Text("Vigencia al: " + detalle.First().vigencia).FontSize(9);

                            //});
                            col1.Item().PaddingVertical(10).Border(1).BorderColor("#477c2c").Table(tabla =>
                            {
                                tabla.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(0.1f); // Índice
                                    columns.RelativeColumn(1.9f); // Todo lo demás
                                });

                                // Header
                                tabla.Header(header =>
                                {
                                    header.Cell().BorderBottom(1).BorderColor("#fedb05")
                                        .Background("#477c2c").Height(20).AlignMiddle()
                                        .Padding(4).Text("#").FontSize(9).Bold().FontFamily(fontFamily).FontColor("#fff");

                                    header.Cell().BorderBottom(1).BorderColor("#fedb05")
                                        .Background("#477c2c").Height(20).AlignMiddle()
                                        .Padding(4).Text("MODELO / DESCRIPCION").FontSize(9).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                int c = 1;
                                foreach (var modelo in modelos)
                                {
                                    // Columna índice
                                    tabla.Cell().BorderBottom(1).BorderRight(1).BorderColor("#477c2c").Padding(5).AlignCenter().Text(c.ToString()).FontSize(9);

                                    // Columna de información
                                    tabla.Cell().BorderBottom(1).ShowEntire().BorderColor("#477c2c").Padding(5).Column(col =>
                                    {
                                        col.Item().Text(modelo.modelo + " / " + modelo.descripcion).Bold().FontSize(9).FontFamily(fontFamily);

                                        col.Item().PaddingTop(3).Text("Características del equipo:").FontSize(9).FontFamily(fontFamily);

                                        // Característica principal
                                        string raw = modelo.caracteristicas_json;
                                        int startIndex = raw.IndexOf("\"descripcion\":\"") + "\"descripcion\":\"".Length;
                                        int endIndex = raw.IndexOf("\"", startIndex);
                                        string descripcionSolo = (startIndex > -1 && endIndex > startIndex) ? raw.Substring(startIndex, endIndex - startIndex) : "";

                                        if (!string.IsNullOrEmpty(descripcionSolo))
                                        {
                                            col.Item().PaddingVertical(2).Text("• " + descripcionSolo).FontSize(8).Justify();
                                        }

                                        col.Item().PaddingTop(5).Row(row =>
                                        {
                                            var img = LimpiarBase64SiEsJson(modelo.imagen);
                                            var imagen = EsBase64Valido(img) ? ObtenerImagenDesdeBase64(img) : null;

                                            if (imagen != null)
                                            {
                                                row.RelativeItem().Width(150).Image(imagen);
                                            }

                                            // Segunda descripción si existe
                                            string descripcionSegunda = "";
                                            if (!string.IsNullOrEmpty(raw))
                                            {
                                                string patron = "\"descripcion\":\"";
                                                int firstIndex = raw.IndexOf(patron);
                                                if (firstIndex != -1)
                                                {
                                                    int secondIndex = raw.IndexOf(patron, firstIndex + patron.Length);
                                                    if (secondIndex != -1)
                                                    {
                                                        int startIndex2 = secondIndex + patron.Length;
                                                        int endIndex2 = raw.IndexOf("\"", startIndex2);
                                                        if (endIndex2 != -1 && endIndex2 > startIndex2)
                                                        {
                                                            descripcionSegunda = raw.Substring(startIndex2, endIndex2 - startIndex2);
                                                        }
                                                    }
                                                }
                                            }

                                            if (!string.IsNullOrEmpty(descripcionSegunda))
                                            {
                                                row.RelativeItem().Column(inner =>
                                                {
                                                    var lineas = descripcionSegunda.Replace("\\n", "\n").Split('\n');

                                                    inner.Item().Text("Puntos de valor del equipo:").FontSize(9).Bold();
                                                    inner.Item().PaddingTop(2).Text(txt =>
                                                    {
                                                        foreach (var linea in lineas)
                                                        {
                                                            if (!string.IsNullOrWhiteSpace(linea))
                                                                txt.Span("• " + linea.Trim()).FontSize(8);
                                                            txt.Span("\n");
                                                        }
                                                    });
                                                });
                                            }
                                            
                                            float anchoLabel = 60;
                                            float anchoValor = 60;

                                            row.RelativeItem().PaddingTop(5).AlignRight().AlignBottom().Column(precios =>
                                            {
                                                precios.Item().Row(row =>
                                                {
                                                    row.ConstantItem(anchoLabel).AlignLeft().Text("Subtotal:").FontSize(10).Bold();
                                                    row.ConstantItem(anchoValor).AlignRight().Text((modelo.precio_promocion != 0 ? modelo.precio_promocion : modelo.precio_lista).ToString("N0")).FontSize(10);
                                                });

                                                precios.Item().Row(row =>
                                                {
                                                    row.ConstantItem(anchoLabel).AlignLeft().Text("Descuento:").FontSize(10).Bold();
                                                    row.ConstantItem(anchoValor).AlignRight().Text(modelo.descuento.ToString("N0")).FontSize(10);
                                                });

                                                precios.Item().Row(row =>
                                                {
                                                    row.ConstantItem(anchoLabel).AlignLeft().Text("Total:").FontSize(10).Bold();
                                                    row.ConstantItem(anchoValor).AlignRight().Text(
                                                        ((modelo.precio_promocion != 0 ? modelo.precio_promocion : modelo.precio_lista) - modelo.descuento).ToString("N0")
                                                    ).FontSize(10);
                                                });

                                                //precios.Item().Row(row =>
                                                //{
                                                //    row.ConstantItem(anchoLabel).AlignLeft().Text("Moneda:").FontSize(10).Bold();
                                                //    row.ConstantItem(anchoValor).AlignRight().Text(modelo.moneda).FontSize(10);
                                                //});
                                            });
                                        });

                                        // Precios
                                        //float anchoLabel = 60;
                                        //float anchoValor = 60;

                                        //col.Item().PaddingTop(5).Column(precios =>
                                        //{
                                        //    precios.Item().Row(row =>
                                        //    {
                                        //        row.ConstantItem(anchoLabel).AlignLeft().Text("Subtotal:").FontSize(10).Bold();
                                        //        row.ConstantItem(anchoValor).AlignRight().Text((modelo.precio_promocion != 0 ? modelo.precio_promocion : modelo.precio_lista).ToString("N0")).FontSize(10);
                                        //    });

                                        //    precios.Item().Row(row =>
                                        //    {
                                        //        row.ConstantItem(anchoLabel).AlignLeft().Text("Descuento:").FontSize(10).Bold();
                                        //        row.ConstantItem(anchoValor).AlignRight().Text(modelo.descuento.ToString("N0")).FontSize(10);
                                        //    });

                                        //    precios.Item().Row(row =>
                                        //    {
                                        //        row.ConstantItem(anchoLabel).AlignLeft().Text("Total:").FontSize(10).Bold();
                                        //        row.ConstantItem(anchoValor).AlignRight().Text(
                                        //            ((modelo.precio_promocion != 0 ? modelo.precio_promocion : modelo.precio_lista) - modelo.descuento).ToString("N0")
                                        //        ).FontSize(10);
                                        //    });
                                        //});
                                    });

                                    c++;
                                }
                            });


                            //    string raw = modelo.caracteristicas_json;
                            //    string descripcionSegunda = "";

                            //    if (!string.IsNullOrEmpty(raw))
                            //    {
                            //        string patron = "\"descripcion\":\"";

                            //        int firstIndex = raw.IndexOf(patron);
                            //        if (firstIndex != -1)
                            //        {
                            //            int secondIndex = raw.IndexOf(patron, firstIndex + patron.Length);
                            //            if (secondIndex != -1)
                            //            {
                            //                int startIndex = secondIndex + patron.Length;
                            //                int endIndex = raw.IndexOf("\"", startIndex);

                            //                if (endIndex != -1 && endIndex > startIndex)
                            //                {
                            //                    descripcionSegunda = raw.Substring(startIndex, endIndex - startIndex);
                            //                }
                            //            }
                            //        }
                            //    }

                            //    col1.Item().PaddingTop(10).Row(row =>
                            //    {
                            //        var img = LimpiarBase64SiEsJson(modelo.imagen);
                            //        var imagen = EsBase64Valido(img)
                            //        ? ObtenerImagenDesdeBase64(img)
                            //        : null;

                            //        if (imagen != null)
                            //        {
                            //            row.RelativeItem(2).Column(col =>
                            //            {
                            //                // Texto arriba
                            //                col.Item().AlignLeft().Text(txt =>
                            //                {
                            //                    txt.Span("IMAGEN DEL EQUIPO:").FontSize(9).Bold();
                            //                });

                            //                // Imagen abajo
                            //                float anchoImagen = 180;
                            //                float altoImagen = 180;

                            //                col.Item().PaddingTop(5).AlignCenter().Height(altoImagen).Width(anchoImagen)
                            //                    .Image(imagen);
                            //            });
                            //        }

                            //        row.RelativeItem(2).Column(column =>
                            //        {
                            //            if (!string.IsNullOrEmpty(descripcionSegunda))
                            //            {
                            //                column.Item().Text("Puntos de valor del equipo:").FontSize(9).Bold();
                            //                column.Item().PaddingTop(2).Text("• " + descripcionSegunda).FontSize(8);
                            //            }
                            //        });


                            //    });
                            //}


                            if (modelos.Count > 1)
                            {
                                col1.Item().BorderBottom(1).AlignRight().Padding(5).ShowEntire().BorderColor(QuestPDF.Helpers.Colors.Black).Column(innerCol =>
                                {
                                    float anchoLabel = 60;
                                    float anchoValor = 60;

                                    innerCol.Item().Row(row =>
                                    {
                                        row.ConstantItem(anchoLabel).AlignLeft().Text("Subtotal:").FontSize(10).Bold();
                                        row.ConstantItem(anchoValor).AlignRight().Text((sumaPrecioLista).ToString("N0")).FontSize(10);
                                    });
                                    //if (sumaDescuento > 0)
                                    //{
                                    innerCol.Item().Row(row =>
                                    {
                                        row.ConstantItem(anchoLabel).AlignLeft().Text("Descuento:").FontSize(10).Bold();
                                        row.ConstantItem(anchoValor).AlignRight().Text((sumaDescuento).ToString("N0")).FontSize(10);
                                    });
                                    //}

                                    innerCol.Item().Row(row =>
                                    {
                                        row.ConstantItem(anchoLabel).AlignLeft().Text("Total:").FontSize(10).Bold();
                                        row.ConstantItem(anchoValor).AlignRight().Text((sumaPrecioTotal - sumaDescuento).ToString("N0")).FontSize(10);
                                    });
                                });
                            }

                            col1.Item().PaddingTop(10).Row(row =>
                            {
                                row.RelativeItem().Text("Estos precios son netos y de contado quedan sujetos a cambio sin previo aviso, prevaleciendo los que estén en vigor al momento de facturar.").FontSize(9);
                            });

                            col1.Item().PaddingTop(100).ShowEntire().Row(row =>
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
