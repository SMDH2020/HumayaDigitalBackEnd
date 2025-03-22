using HD_Cobranza.GestionCobranza.Modelos;
using HD_Cobranza.Modelos.ReferenciasBancarias;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Reporteria.Cobranza
{
    public class RPT_ClientesRB_Referencia
    {
        public static RPT_Result GenerarReferenciaBancaria(mdl_Reporte_RB mdl)
        {
            try
            {
                string CapitalizeWords(string input)
                {
                    if (string.IsNullOrWhiteSpace(input))
                        return string.Empty;

                    return string.Join(" ", input.Split(' ')
                        .Where(w => !string.IsNullOrWhiteSpace(w))
                        .Select(w => char.ToUpper(w[0]) + w.Substring(1).ToLower()));
                }

                string fechaFormateada = DateTime.Now.ToString("dd/MM/yyyy");

                //string fechaFormateada = CambiarFormato(fechaposterior);
                string fontFamily = "Calibri";
                var rutaImagenQR = mdl.ADR == 2
                ? Path.Combine("C:\\Nube\\HumayaDigital\\HumayaDigitalBackEnd\\HDBackend\\HD_Reporteria\\Imagenes\\QRNayarit.png")
                : Path.Combine("C:\\Nube\\HumayaDigital\\HumayaDigitalBackEnd\\HDBackend\\HD_Reporteria\\Imagenes\\QRSinaloa.png");

                var telefono = mdl.ADR == 2 ? "Tel. (311) 341 4978" : "Tel. (667) 502 3527";

                var extension = mdl.ADR == 2 ? "Ext. 8511" : "Ext. 8111";
                byte[] doc = Document.Create(document =>
                {
                    document.Page(page =>
                    {
                        page.Header().Height(120).Row(row =>
                        {

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
                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text("REFERENCIA BANCARIA").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
                                });

                            });


                        });

                        page.Content().PaddingTop(3).PaddingLeft(30).PaddingRight(30).Column(col1 =>
                        {

                            col1.Item().PaddingBottom(2).Row(row =>
                            {
                                row.ConstantItem(340).Column(col =>
                                {
                                    col.Item().AlignLeft().PaddingTop(10).Height(30).Text(txt =>
                                    {
                                        txt.Span(mdl.razonsocial)
                                           .FontSize(12)
                                           .FontFamily("arial")
                                           .Bold();

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

                            col1.Item().Row(row =>
                            {
                                row.RelativeItem().AlignLeft().Text(txt =>
                                {
                                    txt.Span("Información al: ").Bold().FontSize(10);
                                    txt.Span(fechaFormateada).FontSize(10);
                                });
                            });

                            col1.Item().PaddingVertical(12).Table(tablaDatos =>
                            {
                                //var primeraReferencia = mdl.Select(x => x.referencia).First();

                                tablaDatos.ColumnsDefinition(Columns =>
                                {
                                    Columns.RelativeColumn(1f);
                                    Columns.RelativeColumn(1f);
                                    Columns.RelativeColumn(1f);
                                    Columns.RelativeColumn(1f);
                                });

                                tablaDatos.Header(header =>
                                {
                                    header.Cell().ColumnSpan(4).Background("#477c2c").AlignCenter().AlignMiddle().Padding(1).Text("NUMEROS DE CUENTA").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });



                                tablaDatos.Cell().BorderLeft(1).BorderBottom(1).BorderTop(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("BANCO").FontSize(10).FontFamily(fontFamily).Bold();
                                tablaDatos.Cell().BorderBottom(1).BorderTop(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("CUENTA").FontSize(10).FontFamily(fontFamily).Bold();
                                tablaDatos.Cell().BorderBottom(1).BorderTop(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("CONVENIO").FontSize(10).FontFamily(fontFamily).Bold();
                                tablaDatos.Cell().BorderRight(1).BorderBottom(1).BorderTop(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("CLABE").FontSize(10).FontFamily(fontFamily).Bold();

                                tablaDatos.Cell().BorderLeft(1).BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("BAJIO").FontSize(10).FontFamily(fontFamily);
                                tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("3487139").FontSize(10).FontFamily(fontFamily);
                                tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("2974").FontSize(10).FontFamily(fontFamily);
                                tablaDatos.Cell().BorderRight(1).BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("030730348713902015").FontSize(10).FontFamily(fontFamily);

                                tablaDatos.Cell().BorderLeft(1).BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("SANTANDER").FontSize(10).FontFamily(fontFamily);
                                tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("65500056527").FontSize(10).FontFamily(fontFamily);
                                tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("").FontSize(10).FontFamily(fontFamily);
                                tablaDatos.Cell().BorderRight(1).BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("014730655000565272").FontSize(10).FontFamily(fontFamily);

                                tablaDatos.Cell().BorderLeft(1).BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("BBVA").FontSize(10).FontFamily(fontFamily);
                                tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("0119696946").FontSize(10).FontFamily(fontFamily);
                                tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("2174774").FontSize(10).FontFamily(fontFamily);
                                tablaDatos.Cell().BorderRight(1).BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("012914002021747741").FontSize(10).FontFamily(fontFamily);

                                tablaDatos.Cell().BorderLeft(1).BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("BANORTE").FontSize(10).FontFamily(fontFamily);
                                tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("1219732793").FontSize(10).FontFamily(fontFamily);
                                tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("005516").FontSize(10).FontFamily(fontFamily);
                                tablaDatos.Cell().BorderRight(1).BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("072730012197327937").FontSize(10).FontFamily(fontFamily);

                                tablaDatos.Cell().BorderLeft(1).BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("HSBC").FontSize(10).FontFamily(fontFamily);
                                tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("4068669746").FontSize(10).FontFamily(fontFamily);
                                tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("").FontSize(10).FontFamily(fontFamily);
                                tablaDatos.Cell().BorderRight(1).BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("021730040686697463").FontSize(10).FontFamily(fontFamily);

                                tablaDatos.Cell().BorderLeft(1).BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("BANCOPPEL").FontSize(10).FontFamily(fontFamily);
                                tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("12000010160").FontSize(10).FontFamily(fontFamily);
                                tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("").FontSize(10).FontFamily(fontFamily);
                                tablaDatos.Cell().BorderRight(1).BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("137730120000101608").FontSize(10).FontFamily(fontFamily);

                                tablaDatos.Cell().BorderLeft(1).BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("BANAMEX").FontSize(10).FontFamily(fontFamily);
                                tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("92300500891").FontSize(10).FontFamily(fontFamily);
                                tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("").FontSize(10).FontFamily(fontFamily);
                                tablaDatos.Cell().BorderRight(1).BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("002730092305008915").FontSize(10).FontFamily(fontFamily);

                                tablaDatos.Cell().ColumnSpan(2).RowSpan(2).BorderLeft(1).BorderTop(1).BorderBottom(1).BorderColor("#D3D3D3").Background("#ECF3DB").Padding(1).AlignCenter().AlignMiddle().Text("NUMERO DE REFERENCIA").FontSize(12).FontFamily(fontFamily).Bold();
                                tablaDatos.Cell().ColumnSpan(2).BorderRight(1).BorderTop(1).BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(txt =>
                                {
                                    txt.Span("CONCEPTO DE PAGO: ").FontSize(10).FontFamily(fontFamily);
                                    txt.Span(mdl.referencia.ToString()).FontSize(10).FontFamily(fontFamily).Bold();
                                    txt.Span("\n"); // Salto de línea
                                    txt.Span("REFERENCIA DE PAGO: ").FontSize(10).FontFamily(fontFamily);
                                    txt.Span(mdl.referencia.ToString()).FontSize(10).FontFamily(fontFamily).Bold();
                                });

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
                                        col1.Item().PaddingTop(10).Text(txt =>
                                        {
                                            txt.Span("Tel. (667) 502 3527 ").FontSize(10).FontFamily("arial");
                                            txt.Span(extension).Bold().FontSize(10).FontFamily("arial");
                                        });
                                        col1.Item().PaddingTop(10).Text("www.humaya.com.mx").FontSize(10).FontFamily("arial");
                                    });
                                });
                            });
                            row.RelativeItem().AlignRight().PaddingTop(60).Text(txt =>
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
                result.nombredocumento = "REFERENCIAS BANCARIAS";
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
