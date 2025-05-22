using HD_Reporteria.Cobranza;
using ProductoAliado.Modelos.Inventario;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using SkiaSharp;
using System;
using Usados.Consultas.Inventario;

namespace HD_Reporteria.Usados
{
    public  class RPT_Cotizacion_Usados
    {

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
        public static RPT_Result GenerarPDF(IEnumerable<mdl_Cotizacion_Usados_PDF> detalle)
        {
            try
            {
                var detalleOrdenado = detalle
                    .OrderBy(det => det.estatus == "L" ? 0 : det.estatus == "A" ? 1 : 2)
                    .ThenBy(det => det.sucursal)
                    .ToList();

                var detalleUnico = detalle
                .GroupBy(det => new { det.modelo_descripcion, det.Marca, det.ejercicio, det.HP, det.horas, det.estatus, det.idsucursal, det.sucursal, det.precio_lista, det.promocion, det.vigencia  })
                .Select(grp => grp.First());

                string fontFamily = "Calibri";
                byte[] doc = Document.Create(document =>
                {
                    document.Page(page =>
                    {
                        page.Size(PageSizes.Letter.Portrait());
                        var registrosListos = detalleOrdenado.Where(x => x.estatus.Contains("L"));
                        var registrosAcondicionando = detalleOrdenado.Where(x => x.estatus.Contains("A"));


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

                                row.ConstantColumn(450).PaddingTop(35).Height(50).Background("#477c2c").Row(row2 =>
                                {
                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text("COTIZACION DE EQUIPO SEMINUEVO").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
                                    //+obtenernombre_mes(periodo) + " " + ejercicio
                                });
                            });
                        });

                        page.Content().PaddingTop(10).PaddingLeft(30).PaddingRight(30).Column(col1 =>
                        {

                            //col1.Item().LineHorizontal(0.5f);

                            System.DateTime fecha = System.DateTime.Now;
                            string fechaActual = fecha.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("es-ES"));

                            col1.Item().PaddingBottom(2).Row(row =>
                            {
                                // Columna 1: "Cliente"
                                row.ConstantItem(340).Column(col =>
                                {
                                    col.Item().AlignLeft().PaddingTop(10).Height(30).Text(txt =>
                                    {
                                        //"(" + mdl.FirstOrDefault().idcliente + ") " +
                                        txt.Span("A QUIEN CORRESPONDA")
                                           .FontSize(12)
                                           .FontFamily("arial")
                                           .Bold();

                                        //col.Item().AlignLeft().Height(30).Text(txt =>
                                        //{
                                        //    txt.Span(CapitalizeWords(mdl.FirstOrDefault().direccion)).FontSize(10).FontFamily("arial");
                                        //});
                                    });
                                });

                                row.ConstantItem(190).Column(col =>
                                {
                                    col.Item().AlignLeft().Height(15).Text(txt =>
                                    {
                                        txt.Span("MAQUINARIA DEL HUMAYA").FontSize(10).FontFamily("arial").Bold();
                                    });

                                    col.Item().AlignLeft().Height(30).Text(txt =>
                                    {
                                        txt.Span("Carret. Navolato-Culiacan #1185 ote. San Pedro de Rosales, Navolato, Sinaloa. ")
                                           .FontSize(10)
                                           .FontFamily("arial");
                                    });
                                });
                            });

                            col1.Item().PaddingTop(5).PaddingRight(5).Row(row =>
                            {
                                row.RelativeItem().AlignRight().Text(txt =>
                                {
                                    txt.Span("Vigencia al: ").Bold().FontSize(10);
                                    txt.Span(detalleOrdenado.FirstOrDefault()?.vigencia?.ToString("dd/MM/yyyy") ?? "").FontSize(10);

                                });
                            });

                            col1.Item().PaddingTop(10).Background("#f1f1f1").Row(row =>
                            {
                                row.RelativeItem().AlignLeft().PaddingLeft(5).Text(txt =>
                                {
                                    txt.Span("Equipo: " + detalleOrdenado.FirstOrDefault().modelo_descripcion).FontSize(11).Bold();
                                });

                                row.RelativeItem().AlignRight().PaddingRight(5).Text(txt =>
                                {
                                    txt.Span("Precio: " + detalleOrdenado.FirstOrDefault().precio_lista.ToString("N2")).FontSize(11).Bold();
                                });
                            });

                            col1.Item().PaddingTop(10).Row(row =>
                            {
                                row.RelativeItem().AlignLeft().Text(txt =>
                                {
                                    txt.Span("INFORMACION TECNICA").FontSize(8).Bold();
                                });
                            });

                            col1.Item().PaddingVertical(4).Table(tabla =>
                            {
                            tabla.ColumnsDefinition(Columns =>
                            {
                                Columns.RelativeColumn(1.4f);
                                Columns.RelativeColumn(1);
                                Columns.RelativeColumn(1f);
                                Columns.RelativeColumn(1f);
                                Columns.RelativeColumn(1f);
                                Columns.RelativeColumn(1.5f);
                                Columns.RelativeColumn(1.4f);
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("MARCA").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                header.Cell().Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("AÑO").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                header.Cell().Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("HP").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                header.Cell().Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("HORAS").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                header.Cell().Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("SUCURSAL").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                header.Cell().Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ESTATUS").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                header.Cell().Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("PROMOCION").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                            });

                                var item = detalleOrdenado.FirstOrDefault(); // Solo un registro
                                if (item != null)
                                {
                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                    .Text(item.Marca).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                    .Text(item.ejercicio).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                    .Text($"{item.HP}").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                    .Text(item.horas).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                    .Text(item.sucursal).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                    .Text(item.estatus == "L" ? "LISTO PARA LA VENTA" : "ACONDICIONANDO").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                    .Text(item.promocion).FontSize(8).FontFamily(fontFamily);
                                }
                            });

                            col1.Item().PaddingTop(30).Row(row =>
                            {
                                row.RelativeItem().AlignLeft().Text(txt =>
                                {
                                    txt.Span("IMAGENES DEL EQUIPO:").FontSize(9).Bold();
                                });
                            });

                            col1.Item().AlignCenter().Column(col =>
                            {
                                var imagenes = detalleOrdenado
                                    .Where(item => EsBase64Valido(item.imagen))
                                    .Select(item => ObtenerImagenDesdeBase64(item.imagen))
                                    .Where(bytes => bytes != null)
                                    .ToList();

                                float anchoImagen = 180;
                                float altoImagen = 180;

                                for (int i = 0; i < imagenes.Count; i += 2)
                                {
                                    col.Item().AlignCenter().Container().AlignCenter().Row(row =>
                                    {
                                        row.Spacing(40);

                                        row.ConstantItem(anchoImagen).Height(altoImagen)
                                            .Container().AlignCenter().AlignMiddle().Image(imagenes[i]);

                                        if (i + 1 < imagenes.Count)
                                        {
                                            row.ConstantItem(anchoImagen).Height(altoImagen)
                                                .Container().AlignCenter().AlignMiddle().Image(imagenes[i + 1]);
                                        }
                                        else
                                        {
                                            // Agregar un espacio vacío para que la última imagen no quede desbalanceada
                                            row.ConstantItem(anchoImagen).Height(altoImagen)
                                                .Container();
                                        }
                                    });
                                }
                            });




                        });


                        //page.Footer().Height(60).PaddingLeft(30).PaddingRight(30).PaddingBottom(10).Row(row =>
                        //{
                        //    row.RelativeItem().AlignRight().PaddingTop(20).Text(txt =>
                        //    {
                        //        txt.Span("Pág. ").FontSize(10).FontFamily("arial");
                        //        txt.CurrentPageNumber().FontSize(10).Bold().FontFamily("arial");
                        //        txt.Span(" de ").FontSize(10).FontFamily("arial");
                        //        txt.TotalPages().FontSize(10).Bold().FontFamily("arial");
                        //    });
                        //});
                    });

                }).GeneratePdf();
                RPT_Result result = new RPT_Result();
                result.extension = "pdf";
                result.nombredocumento = "COTIZACION DE EQUIPO";
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
