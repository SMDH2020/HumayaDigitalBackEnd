using HD_Cobranza.Modelos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace HD_Reporteria.Cobranza
{
    public class RPT_TotalCartera_Detalle_Cliente
    {
        public static RPT_Result Generar(IEnumerable<mdlResumenCartera_Clientes> resumen)
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
                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text("RESUMEN CARTERA DETALLE").FontColor("#fff").FontSize(16).Bold().FontFamily(fontFamily);
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
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);

                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("SUCURSAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("DOCUMENTO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignRight().AlignMiddle()
                                    .Padding(1).Text("VENCIMIENTO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("DIAS").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("IMPORTE").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("INTERESES").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("TOTAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                foreach (var mdl in resumen)
                                {

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").PaddingLeft(20).AlignLeft().Height(20).AlignMiddle()
                                   .Text(mdl.sucursal).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).PaddingRight(10).AlignMiddle()
                                   .Text(mdl.documento).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                   .Text(mdl.vencimiento).FontSize(8).FontColor("#ff2037").FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                   .Text(mdl.diasvencido).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                   .Text(mdl.saldo.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                   .Text(mdl.interesbase.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                   .Text(mdl.importe.ToString("N2")).FontSize(8).FontFamily(fontFamily);

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
