using HD_Reporteria.Cobranza;
using ProductoAliado.Modelos.Inventario;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Reporteria.ProductoAliado
{
    public class RPT_Listado_Precios_Corto
    {
        public static RPT_Result GenerarPDF(IEnumerable<mdl_Inventario_Producto_Aliado> detalle)
        {
            try
            {
                var detalleOrdenado = detalle
                    .OrderBy(det => det.estatus == "L" ? 0 : det.estatus == "A" ? 1 : 2)
                    .ThenBy(det => det.sucursal)
                    .ToList();
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
                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text("LISTADO DE PRECIOS").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
                                    //+obtenernombre_mes(periodo) + " " + ejercicio
                                });
                            });


                        });

                        page.Content().PaddingTop(10).PaddingLeft(30).PaddingRight(30).Column(col1 =>
                        {

                            //col1.Item().LineHorizontal(0.5f);

                            System.DateTime fecha = System.DateTime.Now;
                            string fechaActual = fecha.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("es-ES"));

                            col1.Item().Row(row =>
                            {
                                row.RelativeItem().AlignRight().Text(txt =>
                                {
                                    txt.Span("INFORMACION AL: ").Bold().FontSize(8);
                                    txt.Span(fechaActual).FontSize(8);
                                });
                            });

                            col1.Item().PaddingVertical(10).Border(0.5f).BorderColor("#477c2c").Table(tabla =>
                            {
                                tabla.ColumnsDefinition(Columns =>
                                {
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(0.9f);
                                    Columns.RelativeColumn(0.9f);
                                    Columns.RelativeColumn(0.5f);
                                    Columns.RelativeColumn(0.5f);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(0.6f);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(0.6f);
                                    Columns.RelativeColumn(1.6f);
                                    Columns.RelativeColumn(0.6f);
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("TRACTOR").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("MARCA").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("AÑO").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("HP").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("SUCURSAL").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("N. E.").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("HORAS").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("PRECIO").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("PROMOCIÓN").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("VIGENCIA").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                foreach (var det in detalleOrdenado)
                                {

                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#afb69d").AlignCenter().AlignMiddle().Height(20).Padding(0)
                                    .SkiaSharpCanvas((canvas, size) =>
                                    {
                                        // Determinar el color del círculo
                                        SKColor color;
                                        int circleRadius = 3; // Radio del círculo
                                        float spacing = 10; // Espacio entre los círculos

                                        switch (det.estatus)
                                        {
                                            case "L":
                                                color = SKColors.Green; // Verde
                                                break;
                                            case "A":
                                                color = SKColors.Yellow; // Amarillo
                                                break;
                                            default:
                                                color = SKColors.Transparent; // Sin color
                                                break;
                                        }

                                        // Crear el pincel para dibujar el círculo
                                        using var paint = new SKPaint
                                        {
                                            Color = color,
                                            Style = SKPaintStyle.Fill
                                        };

                                        // Dibujar círculos
                                        // Dibujar un solo círculo
                                        canvas.DrawCircle(size.Width / 2, size.Height / 2, circleRadius, paint);
                                    });

                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                   .Text(det.modelo).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                   .Text(det.Marca).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                   .Text(det.ejercicio.ToString()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                  .Text(det.HP.ToString()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingLeft(4).PaddingRight(3).PaddingVertical(3).ShowEntire()
                                    .Text(det.nombre_sucursal?.ToUpper()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                   .Text(det.NE).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                   .Text(det.horas.ToString()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                   .Text(det.precio_lista.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                   .Text(det.promocion).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                   .Text(det.vigencia).FontSize(7).FontFamily(fontFamily);
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
                result.nombredocumento = "LISTADO DE PRECIOS";
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
