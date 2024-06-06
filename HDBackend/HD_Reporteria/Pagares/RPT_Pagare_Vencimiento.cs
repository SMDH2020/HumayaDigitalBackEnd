using HD.Clientes.Modelos.Pagares;
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
    public class RPT_Pagare_Vencimiento
    {

        public static string ConvertirNumeroALetras(decimal numero)
        {
            string[] unidades = { "Cero", "Uno", "Dos", "Tres", "Cuatro", "Cinco", "Seis", "Siete", "Ocho", "Nueve" };
            string[] especiales = { "", "Once", "Doce", "Trece", "Catorce", "Quince", "Dieciseis", "Diecisiete", "Dieciocho", "Diecinueve" };
            string[] decenas = { "", "", "Veinte", "Treinta", "Cuarenta", "Cincuenta", "Sesenta", "Setenta", "Ochenta", "Noventa" };
            string cien = numero / 100 == 1 ? "Cien" : "Ciento";
            string[] centenas = { "", cien, "Doscientos", "Trescientos", "Cuatrocientos", "Quinientos", "Seiscientos", "Setecientos", "Ochocientos", "Novecientos" };

            if (numero == 0)
                return "Cero";

            if (numero < 0)
                return "Menos " + ConvertirNumeroALetras(Math.Abs(numero));

            string letras = "";

            if ((int)(numero / 1000000) > 0)
            {
                if ((int)(numero / 1000000) == 1)
                {
                    letras += "Un Millón ";
                }
                else
                {
                    letras += ConvertirNumeroALetras(numero / 1000000) + " Millones ";
                }
                numero %= 1000000;
            }

            if ((int)(numero / 1000) > 0)
            {
                if ((int)(numero / 1000) == 1)
                {
                    letras += "Mil ";
                }
                else
                {
                    letras += ConvertirNumeroALetras(numero / 1000) + " Mil ";
                }
                numero %= 1000;

            }

            if ((int)(numero / 100) > 0)
            {
                letras += centenas[(int)(numero / 100)] + " ";
                numero %= 100;

            }

            if (numero >= 11 && numero <= 19)
                return letras + especiales[(int)numero - 10];

            if ((int)(numero / 10) > 0)
            {
                letras += decenas[(int)(numero / 10)] + " ";
                numero %= 10;
                if (numero > 0)
                    letras += "y ";
            }

            if ((int)numero > 0)
                letras += unidades[(int)numero] + " ";

            return letras.Trim();
        }

        public static RPT_Result Generar(mdl_Pagare_Impresion mdl)
        {
            try
            {
                string fontFamily = "Calibri";
                decimal sumaTotal = (decimal)mdl.financiamientocerodias.Sum(item => item.importefinanciar);
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
                                    row2.RelativeItem().Padding(10).PaddingLeft(130).Text("PAGARE").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
                                });
                            });


                        });

                        page.Content().PaddingTop(3).PaddingLeft(30).PaddingRight(30).Column(col1 =>
                        {



                            //col1.Item().LineHorizontal(0.5f);


                            col1.Item().PaddingTop(20).PaddingBottom(20).Text(txt =>
                            {
                                txt.Span("Por este PAGARE, por valor recibido, me(nos) obligo(amos) a pagar solidaria, mancomunada e incondicionalmente, a la orden de: MAQUINARIA DEL HUMAYA, S.A. DE C.V., en la dirección de sus oficinas en la ciudad de Navolato, Sinaloa, o en cualquier otra donde se me requiera el pago, según lo elija el tenedor de este pagaré, la cantidad principal de").FontSize(10).FontFamily("arial");
                                txt.Span(" $ " + sumaTotal.ToString("N2") + " (" + ConvertirNumeroALetras(sumaTotal) + " pesos " + ((int)((sumaTotal - (int)sumaTotal) * 100)).ToString("00") + "/100 M.N.) ").FontSize(10).Bold().FontFamily("arial");
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



                            col1.Item().PaddingVertical(10).Table(tabla =>
                            {
                                tabla.ColumnsDefinition(Columns =>
                                {
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#264f26").AlignCenter().Padding(1).Text("AMORTIZACION").FontColor("#fff").FontSize(08).Bold().FontFamily(fontFamily);
                                    header.Cell().Background("#264f26").AlignMiddle().AlignCenter().Padding(1).Text("VENCIMIENTO").FontColor("#fff").FontSize(08).Bold().FontFamily(fontFamily);
                                    header.Cell().Background("#264f26").AlignRight().AlignMiddle().Padding(1).PaddingRight(5).Text("IMPORTE A FINANCIAR").FontColor("#fff").FontSize(08).Bold().FontFamily(fontFamily);
                                });

                                foreach (var item in mdl.financiamientocerodias)
                                {
                                    // Colocar en las últimas tres columnas
                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignCenter().Text(item.docto).FontSize(8).FontFamily(fontFamily);
                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignCenter().Text(item.vencimiento).FontSize(8).FontFamily(fontFamily);
                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).PaddingRight(5).AlignRight().Text(item.importefinanciar.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                }
                            });


                            col1.Item().PaddingTop(10).Text("El importe que ampara este pagaré causará intereses moratorios en forma mensual a partir de la fecha de vencimiento, calculados a razón de la tasa fija del " + (mdl.tasa.tasa * 2) + "% por ciento anual sobre saldos insolutos.").FontSize(10).FontFamily("arial");

                            col1.Item().PaddingTop(10).Text("Los intereses se calcularán dividiendo la tasa anual aplicable entre 360 (Trescientos sesenta) y multiplicando el resultado obtenido por el número de días efectivamente transcurridos durante el periodo en que se devenguen los intereses.").FontSize(10).FontFamily("arial");

                            col1.Item().PaddingTop(10).Text("Para todo lo relativo a la interpretación, ejecución y cumplimiento del presente pagaré, el otorgante se somete expresamente a la jurisdicción de los tribunales competentes de la ciudad de Culiacán, Sinaloa, renunciando expresamente a cualquier otro fuero que pudiese corresponderle por razón de su domicilio presente, futuro o por cualquier ubicación de sus bienes.").FontSize(10).FontFamily("arial");


                            col1.Item().PaddingTop(15).PaddingBottom(5).AlignRight().Row(row1 =>
                            {
                                row1.AutoItem().Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        DateTime fechaActual = DateTime.Now;
                                        string ciudad = mdl.ubicacion.sucursal == "SANTIAGO I." ? "SANTIAGO IXCUINTLA" : mdl.ubicacion.sucursal;
                                        txt2.Span(ciudad + ", " + mdl.ubicacion?.estado + " " + fechaActual.ToString("dd 'DE' MMMM 'DEL' yyyy").ToUpper()).FontSize(10).FontFamily("arial");
                                    });
                                });
                            });


                            col1.Item().PaddingTop(20).AlignCenter().Row(row1 =>
                            {
                                row1.ConstantItem(200).AlignCenter().Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(" SUSCRIPTOR (ES)").FontSize(08).FontFamily(fontFamily);
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });


                                row1.ConstantItem(200).AlignCenter().Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("AVAL (ES)").FontSize(08).FontFamily(fontFamily);
                                    });
                                });
                            });

                            col1.Item().PaddingTop(20).AlignCenter().Row(row1 =>
                            {
                                row1.ConstantItem(200).PaddingRight(10).BorderBottom(1).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).AlignCenter().Text(txt2 =>
                                    {
                                        txt2.Span(mdl.firmas.suscriptor).FontSize(10).FontFamily(fontFamily);
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });


                                row1.ConstantItem(200).PaddingLeft(10).BorderBottom(1).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).AlignCenter().Text(txt2 =>
                                    {
                                        txt2.Span(mdl.firmas.aval).FontSize(10).FontFamily(fontFamily);
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });

                            });

                            col1.Item().PaddingTop(00).AlignCenter().Row(row1 =>
                            {
                                row1.ConstantItem(200).AlignCenter().PaddingRight(10).Column(txt1 =>
                                {
                                    txt1.Item().Text(txt2 =>
                                    {
                                        txt2.Span(mdl.firmas.direccion_suscriptor).FontSize(08).FontFamily(fontFamily);
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });


                                row1.ConstantItem(200).AlignCenter().PaddingLeft(10).Column(txt1 =>
                                {
                                    txt1.Item().Text(txt2 =>
                                    {
                                        txt2.Span(mdl.firmas.direccion_aval).FontSize(08).FontFamily(fontFamily);
                                    });
                                });
                            });

                        });

                        page.Footer().Height(100).PaddingLeft(30).PaddingRight(30).PaddingBottom(20).Row(row =>
                        {



                        });

                    });

                }).GeneratePdf();
                RPT_Result result = new RPT_Result();
                result.extension = "pdf";
                result.nombredocumento = "PAGARE CON UN SOLO PAGO CON INTERESES A PARTIR DE LA FECHA DE VENCIMIENTO";
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
