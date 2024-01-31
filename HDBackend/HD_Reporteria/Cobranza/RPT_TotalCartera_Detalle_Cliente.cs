using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace HD_Reporteria.Cobranza
{
    public class RPT_TotalCartera_Detalle_Cliente
    {
        public static RPT_Result Generar()
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

                                row.ConstantColumn(450).PaddingTop(35).Height(50).Background("#477c2c").Row(row2 =>
                                {
                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text("RESUMEN DE CARTERA DETALLE POR CLIENTE").FontColor("#fff").FontSize(20).Bold().FontFamily("roboto");
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
                                    Columns.RelativeColumn(0.3f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
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
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("#").FontSize(8).Bold().FontFamily("roboto").FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("SUCURSAL").FontSize(8).Bold().FontFamily("roboto").FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("TOTAL CARTERA").FontSize(8).Bold().FontFamily("roboto").FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("SALDO A FAVOR").FontSize(8).Bold().FontFamily("roboto").FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("TOTAL").FontSize(8).Bold().FontFamily("roboto").FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("JURIDICO").FontSize(8).Bold().FontFamily("roboto").FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily("roboto").FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("CARTERA ACTIVA").FontSize(8).Bold().FontFamily("roboto").FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily("roboto").FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("POR VENCER").FontSize(8).Bold().FontFamily("roboto").FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily("roboto").FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("VENCIDA").FontSize(8).Bold().FontFamily("roboto").FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily("roboto").FontColor("#fff");
                                });

                                foreach (var item in Enumerable.Range(1, 3))
                                {
                                    var cantidad = Placeholders.Random.Next(1, 10);
                                    var precio = Placeholders.Random.Next(500, 50000);
                                    var descuento = Placeholders.Random.Next(50, 500);
                                    var total = (precio - descuento) * cantidad;
                                    var formattedPrecio = $"{precio:N0}";
                                    var formattedDescuento = $"{descuento:N0}";
                                    var formattedTotal = $"{total:N0}";

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignCenter()
                                    .Text("hola").FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignCenter()
                                   .Text("R6000").FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1)
                                   .Text($"{formattedPrecio}").FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1)
                                   .Text($"{formattedPrecio}").FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                   .Text($"{formattedPrecio}").FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                   .Text($"{formattedDescuento}").FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                   .Text($"{cantidad}").FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignCenter()
                                   .Text($"{formattedTotal}").FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1)
                                   .Text($"{cantidad}").FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1)
                                   .Text("R582698").FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                   .Text($"{formattedPrecio}").FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                   .Text($"{formattedDescuento}").FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                   .Text($"{formattedTotal}").FontSize(8).FontFamily("roboto");
                                }
                            });
                        });


                    });
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

                                row.ConstantColumn(450).PaddingTop(35).Height(50).Background("#477c2c").Row(row2 =>
                                {
                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text("RESUMEN DE CARTERA POR SUCURSAL").FontColor("#fff").FontSize(20).Bold().FontFamily("roboto");
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
                                    Columns.RelativeColumn(0.3f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.4f);
                                    Columns.RelativeColumn(1);


                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("#").FontSize(8).Bold().FontFamily("roboto").FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("SUCURSAL").FontSize(8).Bold().FontFamily("roboto").FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("DE 1 A 15").FontSize(8).Bold().FontFamily("roboto").FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("MAS DE 15").FontSize(8).Bold().FontFamily("roboto").FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("MAS DE 30").FontSize(8).Bold().FontFamily("roboto").FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("MAS DE 60").FontSize(8).Bold().FontFamily("roboto").FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("%").FontSize(8).Bold().FontFamily("roboto").FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("MAS DE 90").FontSize(8).Bold().FontFamily("roboto").FontColor("#fff");
                                });

                                foreach (var item in Enumerable.Range(1, 3))
                                {
                                    var cantidad = Placeholders.Random.Next(1, 10);
                                    var precio = Placeholders.Random.Next(500, 50000);
                                    var descuento = Placeholders.Random.Next(50, 500);
                                    var total = (precio - descuento) * cantidad;
                                    var formattedPrecio = $"{precio:N0}";
                                    var formattedDescuento = $"{descuento:N0}";
                                    var formattedTotal = $"{total:N0}";

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignCenter()
                                    .Text($"{cantidad}").FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignCenter()
                                   .Text("R6000").FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1)
                                   .Text($"{formattedPrecio}").FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1)
                                   .Text($"{formattedPrecio}").FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                   .Text($"{formattedPrecio}").FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                   .Text($"{formattedDescuento}").FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                   .Text($"{cantidad}").FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignCenter()
                                   .Text($"{formattedTotal}").FontSize(8).FontFamily("roboto");

                                }
                            });
                        });


                    });
                }).GeneratePdf();
                RPT_Result result = new RPT_Result();
                result.extension = "pdf";
                result.nombredocumento = "RESUMEN CARTERA DETALLE POR CLIENTE";
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
