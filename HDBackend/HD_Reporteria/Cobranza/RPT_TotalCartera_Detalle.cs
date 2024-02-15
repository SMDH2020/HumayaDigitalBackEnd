using HD_Cobranza.Modelos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace HD_Reporteria.Cobranza
{
    public class RPT_TotalCartera_Detalle
    {
        public static RPT_Result Generar(IEnumerable<mdlCob_TotalCartera_Detalle> resumen)
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
                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text("RESUMEN DE CARTERA DETALLE").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
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
                                    Columns.RelativeColumn(0.5f);
                                    Columns.RelativeColumn(1.5f);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(0.3f);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(0.3f);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(0.3f);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(0.3f);

                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Text("IDCLIENTE").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                     .Text("RAZON SOCIAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Text("TOTAL CARTERA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Text("SALDO A FAVOR").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Text("TOTAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                   .Text("JURIDICO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Text("CARTERA ACTIVA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Text("POR VENCER").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Text("VENCIDA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                   .Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                foreach (var mdl in resumen)
                                {

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignLeft().Height(20).AlignMiddle().PaddingLeft(3)
                                   .Text(mdl.idcliente).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignLeft().Height(30).AlignMiddle()
                                   .Text(mdl.razonsocial).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                   .Text((mdl.totalcartera + mdl.juridico).ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                   .Text(mdl.saldoafavor.ToString("N2")).FontSize(8).FontColor("#ff2037").FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                   .Text((mdl.total + mdl.juridico).ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                   .Text(mdl.juridico.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                   .Text((mdl.juridico / (mdl.totalcartera + mdl.juridico) * 100).ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                   .Text(mdl.activo.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                   .Text(((mdl.activo / (mdl.totalcartera + mdl.juridico)) * 100).ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                   .Text(mdl.porvencer.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                    .Text(((mdl.porvencer / (mdl.totalcartera + mdl.juridico)) * 100).ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                    .Text(mdl.vencido.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                    .Text(((mdl.vencido / (mdl.totalcartera + mdl.juridico)) * 100).ToString("N2")).FontSize(8).FontFamily(fontFamily);
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

                                row.ConstantColumn(693).PaddingTop(35).Height(50).Background("#477c2c").Row(row2 =>
                                {
                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text("RESUMEN DE CARTERA DETALLE VENCIDO").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
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
                                    Columns.RelativeColumn(2);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);


                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Text("ID CLIENTE").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Text("RAZON SOCIAL").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Text("DE 1 A 15").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Text("MAS DE 15").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Text("MAS DE 30").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Text("MAS DE 60").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Text("MAS DE 90").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                foreach (var mdl in resumen)
                                {
                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignLeft().Height(20).AlignMiddle().PaddingLeft(20)
                                   .Text(mdl.idcliente).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignLeft().Height(20).AlignMiddle().PaddingLeft(20)
                                   .Text(mdl.razonsocial).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                   .Text(mdl.de1a15.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                   .Text(mdl.mas15.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                   .Text(mdl.mas30.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle()
                                   .Text(mdl.mas60.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(20)
                                   .Text(mdl.mas90.ToString("N2")).FontSize(8).FontFamily(fontFamily);


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

