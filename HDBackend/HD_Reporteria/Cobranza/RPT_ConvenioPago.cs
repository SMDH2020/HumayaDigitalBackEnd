﻿using DocumentFormat.OpenXml.Spreadsheet;
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
                var rutaImagenQR = mdl.ADR == 2
                ? Path.Combine("C:\\Nube\\HumayaDigital\\HumayaDigitalBackEnd\\HDBackend\\HD_Reporteria\\Imagenes\\QRNayarit.png")
                : Path.Combine("C:\\Nube\\HumayaDigital\\HumayaDigitalBackEnd\\HDBackend\\HD_Reporteria\\Imagenes\\QRSinaloa.png");

                var telefono = mdl.ADR == 2 ? "Tel. (311) 341 4978" : "Tel. (667) 758 8200";
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
                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text("CONVENIO DE PAGO").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
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

                            col1.Item().PaddingTop(20).PaddingBottom(20).Text(txt =>
                            {
                                txt.Span("Me comprometo a pagar la cantidad de $").FontSize(10).FontFamily("arial");
                                txt.Span(mdl.monto.ToString("N2")).FontSize(10).Bold().FontFamily("arial");
                                txt.Span(" conveniado antes del ").FontSize(10).FontFamily("arial");
                                txt.Span(mdl.fecha_convenio.ToString("dd")).FontSize(10).Bold().FontFamily("arial");
                                txt.Span(" del mes de ").FontSize(10).FontFamily("arial");
                                txt.Span(mdl.fecha_convenio.ToString("MMMM")).FontSize(10).Bold().FontFamily("arial");
                                txt.Span(" del año ").FontSize(10).FontFamily("arial");
                                txt.Span(mdl.fecha_convenio.ToString("yyyy")).FontSize(10).Bold().FontFamily("arial");
                                txt.Span(" a ").FontSize(10).FontFamily("arial");
                                txt.Span("Humaya John Deere").FontSize(10).Bold().FontFamily("arial");
                                txt.Span(", lo que corresponda a las siguientes facturas.").FontSize(10).FontFamily("arial");
                            });

                            col1.Item().Text("Detalle del convenio:").Bold();
                            col1.Item().PaddingVertical(10).Table(tabla =>
                            {
                                tabla.ColumnsDefinition(Columns =>
                                {
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(0.9f);
                                    Columns.RelativeColumn(1.2f);
                                    Columns.RelativeColumn(0.9f);
                                    Columns.RelativeColumn(1.5f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.9f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1.2f);


                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("SERIE").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("FECHA").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("FECHA DE VENCIMIENTO").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("DIAS VENCIDOS").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("DESCRIPCION").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("MONTO FACTURA").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("ABONO").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("SALDO VENCIDO").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("INTERES MORATORIO").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                    .Padding(1).Text("SALDO CONVENIADO").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
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

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                    .Text($"{cantidad}").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                   .Text("").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                   .Text("").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                   .Text("").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignRight()
                                   .Text("").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignRight()
                                   .Text($"{formattedTotal}").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignRight()
                                   .Text("").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                    .Text("").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                    .Text("").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                    .Text("").FontSize(8).FontFamily(fontFamily);
                                }
                            });

                            col1.Item().PaddingTop(20).Text("En caso de incumplimiento de este convenio, la cuenta continuará generando intereses moratorios incrementando la cantidad del adeudo y afectando su historial crediticio").FontSize(10).FontFamily("arial");

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

                            col1.Item().PaddingTop(30).Text(txt =>
                            {
                                txt.Span("Le recordamos que puede acudir a nuestra sucursal ").FontSize(10).FontFamily("arial");
                                txt.Span("Humaya John Deere").FontSize(10).Bold().FontFamily("arial");
                                txt.Span(" o bien a su banco con su ").FontSize(10).FontFamily("arial");
                                txt.Span("REFERENCIA UNICA DE CLIENTE XXXXXXX").FontSize(10).Bold().FontFamily("arial");
                                txt.Span(" a realizar su depósito o transferencia para ponerse al corriente").FontSize(10).FontFamily("arial");
                            });
                        });

                        page.Footer().Height(100).PaddingLeft(30).PaddingRight(30).PaddingBottom(20).Row(row =>
        {
            row.ConstantColumn(0).Row(row1 =>
            {
                //var rutaImagen = Path.Combine("C:\\Nube\\HumayaDigital\\HumayaDigitalBackEnd\\HDBackend\\HD_Reporteria\\Imagenes\\QRSinaloa.png");
                byte[] imageData = System.IO.File.ReadAllBytes(rutaImagenQR);
                row.ConstantItem(80).BorderRight(1).Image(imageData);

                row.ConstantColumn(180).Row(row2 =>
                {
                    row2.RelativeItem().PaddingLeft(10).Column(col1 =>
                    {
                        col1.Item().Row(row3 =>
                        {
                            var rutaImagen = Path.Combine("C:\\Nube\\HumayaDigital\\HumayaDigitalBackEnd\\HDBackend\\HD_Reporteria\\Imagenes\\whatsapp.png");
                            byte[] imageData = System.IO.File.ReadAllBytes(rutaImagen);
                            row3.ConstantItem(15).PaddingTop(5).Image(imageData);
                            row3.RelativeItem().PaddingLeft(5).PaddingTop(5).Text(telefono).FontSize(10).FontFamily("arial");
                        });
                        col1.Item().PaddingTop(10).Text(txt => {
                            txt.Span("Tel. (667) 758 8200 ").FontSize(10).FontFamily("arial");
                            txt.Span("Ext. 8511").Bold().FontSize(10).FontFamily("arial");
                        });
                        col1.Item().PaddingTop(10).Text("www.humaya.com.mx").FontSize(10).FontFamily("arial");
                    });
                });
            });

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