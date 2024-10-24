﻿using QuestPDF.Fluent;
using QuestPDF.Helpers;
using HD_Ventas.Modelos;
using ClosedXML.Excel;
using HD_Cobranza.GestionCobranza.Modelos;

namespace HD_Reporteria.Cobranza
{
    public class RPT_Listado_Clientes_Gestionar
    {
        public static string obtenernombre_mes(int numeromes)
        {
            switch (numeromes)
            {
                case 1:
                    return "ENERO";
                case 2:
                    return "FEBRERO";
                case 3:
                    return "MARZO";
                case 4:
                    return "ABRIL";
                case 5:
                    return "MAYO";
                case 6:
                    return "JUNIO";
                case 7:
                    return "JULIO";
                case 8:
                    return "AGOSTO";
                case 9:
                    return "SEPTIEMBRE";
                case 10:
                    return "OCTUBRE";
                case 11:
                    return "NOVIEMBRE";
                case 12:
                    return "DICIEMBRE";
                default:
                    return "";

            }
        }

        public static RPT_Result GenerarPDF(IEnumerable<mdl_Listado_Clientes_Gestionar> detalle)
        {
            try
            {
                string fontFamily = "Calibri";
                byte[] doc = Document.Create(document =>
                {
                    document.Page(page =>
                    {
                        page.Size(PageSizes.Letter.Portrait());



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
                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text("CLIENTES A GESTIONAR").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
                                    //+obtenernombre_mes(periodo) + " " + ejercicio
                                });
                            });


                        });

                        page.Content().PaddingTop(10).PaddingLeft(30).PaddingRight(30).Column(col1 =>
                        {

                            //col1.Item().LineHorizontal(0.5f);

                            //col1.Item().Row(row =>
                            //{
                            //    row.RelativeItem().AlignCenter().Text(txt =>
                            //    {
                            //        txt.Span("SCORECARD GENERAL").FontSize(12).Bold();
                            //    });
                            //});

                            col1.Item().PaddingVertical(10).Border(1).BorderColor("#275027").Table(tabla =>
                            {
                                tabla.ColumnsDefinition(Columns =>
                                {
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(1.6f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1.2f);
                                    Columns.RelativeColumn(0.8f);
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().BorderBottom(1).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("LINEA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderBottom(1).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("ESTADO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderBottom(1).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("SUCURSAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderBottom(1).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("CLIENTE").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderBottom(1).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("SALDO VENCIDO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderBottom(1).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("SALDO POR VENCER").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderBottom(1).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("RESPONSABLE").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderBottom(1).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("GESTION").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                foreach (var det in detalle)
                                {

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingLeft(4).PaddingRight(3).PaddingVertical(3).ShowEntire()
                                    .Text(det.linea?.ToUpper()).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                   .Text(det.estado?.ToUpper()).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                   .Text(det.sucursal?.ToUpper()).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                   .Text(det.RazonSocial?.ToUpper()).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                   .Text(det.SaldoVencido.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                   .Text(det.SaldoporVencer.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                   .Text(det.ResponsableCobranza?.ToUpper()).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                   .Text(det.gestion == "O" ? "PENDIENTE" :
                                          det.gestion == "M" ? "VOLVER A CONTACTAR" :
                                          det.gestion == "C" ? "CONVENIO" :
                                          det.gestion?.ToUpper()).FontSize(8).FontFamily(fontFamily);
                                }
                            });
                        });

                    });

                }).GeneratePdf();
                RPT_Result result = new RPT_Result();
                result.extension = "pdf";
                result.nombredocumento = "CONVENIOS REALIZADOS";
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
