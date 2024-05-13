using HD.Clientes.Modelos.Pedido_Impresion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Reporteria.Pagares
{
    public class RPT_Pagare_Dos_Amortizaciones_Vencimiento
    {
        public static RPT_Result Generar(mdl_pedido_impresion mdl)
        {
            try
            {
                string fontFamily = "Calibri";
                byte[] doc = Document.Create(document =>
                {
                    document.Page(page =>
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
                                        row2.RelativeItem().Padding(10).PaddingLeft(130).Text("PAGARE").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
                                    });
                                });


                            });

                            page.Content().PaddingTop(3).PaddingLeft(30).PaddingRight(30).Column(col1 =>
                            {



                                //col1.Item().LineHorizontal(0.5f);


                                col1.Item().PaddingTop(20).PaddingBottom(20).Text(txt =>
                                {
                                    txt.Span("Por este PAGARE, por valor recibido, me(nos) obligo(amos) a pagar solidaria, mancomunada e incondicionalmente, a la orden de: MAQUINARIA DEL HUMAYA, S.A. DE C.V., en la dirección de sus oficinas en la ciudad de Navolato, Sinaloa, o en cualquier otra donde se me requiera el pago, según lo elija el tenedor de este pagaré, la cantidad principal de $ ").FontSize(10).FontFamily("arial");
                                    txt.Span("500,000 PESOS M.N.").FontSize(10).Bold().FontFamily("arial");
                                    txt.Span(" mediante las amortizaciones pactadas, por los montos y las fechas que a continuación se detallan:").FontSize(10).FontFamily("arial");
                                    //txt.Span("10 ").FontSize(10).Bold().FontFamily("arial"); 
                                    //txt.Span("del mes de ").FontSize(10).FontFamily("arial");
                                    //txt.Span("Enero ").FontSize(10).Bold().FontFamily("arial");
                                    //txt.Span("del año ").FontSize(10).FontFamily("arial");
                                    //txt.Span("2025 ").FontSize(10).Bold().FontFamily("arial");
                                    //txt.Span("a ").FontSize(10).FontFamily("arial");
                                    //txt.Span("Humaya John Deere").FontSize(10).Bold().FontFamily("arial");
                                    //txt.Span(", lo que corresponda a las siguientes facturas.").FontSize(10).FontFamily("arial");
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
                                        header.Cell().Background("#ccc").AlignCenter()
                                        .Padding(1).Text("SERIE").FontSize(08).Bold().FontFamily(fontFamily);
                                        header.Cell().Background("#ccc").AlignMiddle().AlignCenter()
                                        .Padding(1).Text("FECHA").FontSize(08).Bold().FontFamily(fontFamily);
                                        header.Cell().Background("#ccc").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("FECHA DE VENCIMIENTO").FontSize(08).Bold().FontFamily(fontFamily);
                                        header.Cell().Background("#ccc").AlignCenter()
                                        .Padding(1).Text("DIAS VENCIDOS").FontSize(08).Bold().FontFamily(fontFamily);
                                        header.Cell().Background("#ccc").AlignCenter()
                                        .Padding(1).Text("DESCRIPCION").FontSize(08).Bold().FontFamily(fontFamily);
                                        header.Cell().Background("#ccc").AlignCenter()
                                        .Padding(1).Text("MONTO FACTURA").FontSize(08).Bold().FontFamily(fontFamily);
                                        header.Cell().Background("#ccc").AlignCenter()
                                        .Padding(1).Text("ABONO").FontSize(08).Bold().FontFamily(fontFamily);
                                        header.Cell().Background("#ccc").AlignCenter()
                                        .Padding(1).Text("SALDO VENCIDO").FontSize(08).Bold().FontFamily(fontFamily);
                                        header.Cell().Background("#ccc").AlignCenter()
                                        .Padding(1).Text("INTERES MORATORIO").FontSize(08).Bold().FontFamily(fontFamily);
                                        header.Cell().Background("#ccc").AlignCenter()
                                        .Padding(1).Text("SALDO TOTAL").FontSize(08).Bold().FontFamily(fontFamily);
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

                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1)
                                       .Text($"{cantidad}").FontSize(8).FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignCenter()
                                       .Text("22-10-2024").FontSize(8).FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1)
                                       .Text("22-10-2025").FontSize(8).FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1)
                                       .Text($"{cantidad}").FontSize(8).FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                       .Text("Maquinaria Usada").FontSize(8).FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                       .Text($"{formattedDescuento}").FontSize(8).FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                        .Text($"{formattedDescuento}").FontSize(8).FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                       .Text($"{formattedTotal}").FontSize(8).FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1)
                                        .Text("R582698").FontSize(8).FontFamily(fontFamily);

                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1)
                                        .Text("R582698").FontSize(8).FontFamily(fontFamily);
                                    }
                                    tabla.Footer(footer =>
                                    {
                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("TOTAL").FontSize(8).FontFamily("arial");
                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("TOTAL").FontSize(8).FontFamily("arial");
                                        // Puedes agregar más celdas y contenido según tus necesidades
                                    });

                                });


                                col1.Item().PaddingTop(20).Text("El importe que ampara este pagaré causará intereses moratorios en forma mensual a partir de la fecha de vencimiento calculados a razón de la tasa fija del 33% (treinta y tres) por ciento anual sobre saldos insolutos.").FontSize(10).FontFamily("arial");

                                col1.Item().PaddingTop(20).Text("Los intereses se calcularán dividiendo la tasa anual aplicable entre 360 (Trescientos sesenta) y multiplicando el resultado obtenido por el número de días efectivamente transcurridos durante el periodo en que se devenguen los intereses.").FontSize(10).FontFamily("arial");

                                col1.Item().PaddingTop(20).Text("La falta de pago puntual de cualquier amortización o abono en su fecha de vencimiento causará el vencimiento anticipado del pagaré o abonos restantes, aun los no vencidos, los que serán exigibles de inmediato.").FontSize(10).FontFamily("arial");

                                col1.Item().PaddingTop(20).Text("Para todo lo relativo a la interpretación, ejecución y cumplimiento del presente pagaré, el otorgante se somete expresamente a la jurisdicción de los tribunales competentes de la ciudad de Culiacán, Sinaloa, renunciando expresamente a cualquier otro fuero que pudiese corresponderle por razón de su domicilio presente, futuro o por cualquier ubicación de sus bienes.").FontSize(10).FontFamily("arial");


                                col1.Item().PaddingTop(15).PaddingBottom(5).AlignRight().Row(row1 =>
                                {
                                    row1.AutoItem().Column(txt1 =>
                                    {
                                        txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                        {
                                            DateTime fechaActual = DateTime.Now;

                                            txt2.Span("NAVOLATO, SINALOA " + fechaActual.ToString("dd 'DE' MMMM 'DEL' yyyy").ToUpper()).FontSize(10).FontFamily("arial");
                                        });
                                    });
                                });


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


                                    row1.ConstantItem(200).AlignCenter().Column(txt1 =>
                                    {
                                        txt1.Item().Height(15).Text(txt2 =>
                                        {
                                            txt2.Span("Vendedor").FontSize(08).FontFamily(fontFamily);
                                        });
                                    });
                                });

                                col1.Item().PaddingTop(20).AlignCenter().Row(row1 =>
                                {
                                    row1.ConstantItem(200).PaddingRight(10).BorderBottom(1).Column(txt1 =>
                                    {
                                        txt1.Item().Height(15).AlignCenter().Text(txt2 =>
                                        {
                                            txt2.Span("Celina Godoy Valenzuela").FontSize(10).FontFamily(fontFamily);
                                            //txt2.Span("NAVOLATO").FontSize(10);
                                        });
                                    });


                                    row1.ConstantItem(200).PaddingLeft(10).BorderBottom(1).Column(txt1 =>
                                    {
                                        txt1.Item().Height(15).AlignCenter().Text(txt2 =>
                                        {
                                            txt2.Span("Asael Jimenez Terrazas").FontSize(10).FontFamily(fontFamily);
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
                                            txt2.Span("Nombre y Firma").FontSize(08).FontFamily(fontFamily);
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

                            });

                            page.Footer().Height(100).PaddingLeft(30).PaddingRight(30).PaddingBottom(20).Row(row =>
                            {



                            });

                        });

                    });

                }).GeneratePdf();
                RPT_Result result = new RPT_Result();
                result.extension = "pdf";
                result.nombredocumento = "PAGARE CON DOS O MAS AMORTIZACIONES CON INTERESES A PARTIR DE FECHA DE VENCIMIENTO";
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
