using HD_Cobranza.Modelos;
using HD_Cobranza.Modelos.ConvenioPago;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace HD_Reporteria.Cobranza
{
    public class RPT_ConvenioPago
    {
        public static RPT_Result Generar(mdlConvenio_Pago mdl)
        {
            try
            {
                string fontFamily = "Calibri";
                byte[] doc = Document.Create(document =>
                {
                    document.Page(page =>
                    {
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
                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text("Convenio de pago").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
                                });
                            });


                        });

                        page.Content().PaddingTop(3).PaddingLeft(30).PaddingRight(30).Column(col1 =>
                        {


                            col1.Item().PaddingBottom(5).AlignRight().Row(row1 =>
                            {
                                row1.AutoItem().Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        DateTime fechaActual = DateTime.Now;

                                        txt2.Span(fechaActual.ToString("dd 'de' MMMM 'del' yyyy")).FontSize(10).FontFamily("arial");
                                    });
                                });
                            });


                            col1.Item().AlignLeft().Row(row1 =>
                            {
                                row1.ConstantItem(20).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Yo: ").FontSize(10).FontFamily("arial");
                                    });
                                });
                                row1.AutoItem().Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.razon_social).FontSize(10).FontFamily("arial").Bold();
                                    });
                                });
                            });

                            //col1.Item().LineHorizontal(0.5f);


                            col1.Item().PaddingTop(20).PaddingBottom(20).Text($"Me comprometo a pagar la cantidad de ${mdl.monto.ToString("N2")} conveniado antes del {mdl.fecha_convenio.ToString("dd")} del mes de {mdl.fecha_convenio.ToString("MMMM")} del año {mdl.fecha_convenio.ToString("yyyy")} Humaya Jhon Deere, lo que corresponda a las siguientes facturas.").FontSize(10).FontFamily("arial");


                            col1.Item().Text("Detalle del convenio:").Bold();
                            col1.Item().PaddingVertical(10).Table(tabla =>
                            {
                                tabla.ColumnsDefinition(Columns =>
                                {
                                    Columns.RelativeColumn(0.7f);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(1.5f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(1);

                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("FOLIO FACTURA").FontSize(08).Bold().FontFamily(fontFamily);
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("FECHA").FontSize(08).Bold().FontFamily(fontFamily);
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("FECHA DE VENCIMIENTO").FontSize(08).Bold().FontFamily(fontFamily);
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("DIAS VENCIDOS").FontSize(08).Bold().FontFamily(fontFamily);
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("DESCRIPCION").FontSize(08).Bold().FontFamily(fontFamily);
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("MONTO FACTURA").FontSize(08).Bold().FontFamily(fontFamily);
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("SALDO VENCIDO").FontSize(08).Bold().FontFamily(fontFamily);
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("INTERES MORATORIO").FontSize(08).Bold().FontFamily(fontFamily);
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("SALDO CONVENIADO").FontSize(08).Bold().FontFamily(fontFamily);
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
                                    .Text($"{cantidad}").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignCenter()
                                   .Text("").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1)
                                   .Text("").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1)
                                   .Text("").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                   .Text("").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                   .Text($"{formattedTotal}").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                   .Text("").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1)
                                    .Text("").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1)
                                    .Text("").FontSize(8).FontFamily(fontFamily);
                                }
                            });

                            col1.Item().PaddingTop(20).Text("Es importante cumplir con nuestros compromisos para mantener una relación de confianza, por lo cual, lo invitamos a realizar el pago conveniado en la fecha acordada. ").FontSize(10).Bold().FontFamily("arial");

                            col1.Item().PaddingTop(20).AlignCenter().Row(row1 =>
                            {
                                row1.ConstantItem(200).AlignCenter().Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Cliente").FontSize(08).FontFamily(fontFamily);
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });


                                //row1.ConstantItem(200).AlignCenter().Column(txt1 =>
                                //{
                                //    txt1.Item().Height(15).Text(txt2 =>
                                //    {
                                //        txt2.Span("Responsa").FontSize(08).FontFamily(fontFamily);
                                //    });
                                //});
                            });

                            col1.Item().PaddingTop(15).AlignCenter().Row(row1 =>
                            {
                                row1.ConstantItem(200).PaddingRight(10).BorderBottom(1).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).AlignCenter().Text(txt2 =>
                                    {
                                        txt2.Span("SILVIA VAZQUEZ").FontSize(10).FontFamily(fontFamily);
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });


                                row1.ConstantItem(200).PaddingLeft(10).BorderBottom(1).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).AlignCenter().Text(txt2 =>
                                    {
                                        txt2.Span(mdl.razon_social).FontSize(10).FontFamily(fontFamily);
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });

                            });

                            col1.Item().PaddingTop(00).AlignCenter().Row(row1 =>
                            {
                                row1.ConstantItem(200).AlignCenter().Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Departamento de cobranza").FontSize(08).FontFamily(fontFamily);
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });


                                row1.ConstantItem(200).AlignCenter().Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Nombre y Firma").FontSize(08).FontFamily(fontFamily);
                                    });
                                });
                            });

                            col1.Item().PaddingTop(20).Text("Le recordamos que puede acudir a nuestra sucursal Humaya Jhon Deere o bien a su banco con su REFERENCIA UNICA DE CLIENTE XXXXX a realizar su deposito o transferencia para ponerse al corriente").FontSize(10).FontFamily("arial");
                        });

                        page.Footer().Height(60).PaddingLeft(30).PaddingRight(30).Column(col1 =>
                        {

                        });

                    });
                }).GeneratePdf();
                RPT_Result result = new RPT_Result();
                result.extension = "pdf";
                result.nombredocumento = "CONVENIO DE PAGO";
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
