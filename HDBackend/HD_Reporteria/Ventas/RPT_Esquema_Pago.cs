using HD_Reporteria.Cobranza;
using HD_Ventas.Modelos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usados.Consultas.Usados;

namespace HD_Reporteria.Ventas
{
    public class RPT_Esquema_Pago
    {
        public static RPT_Result GenerarPDF(mdl_Modelos_Esquema_Linea_PDF_View detalle)
        {
            try
            {
                var detalleOrdenado = detalle.modelos
                .ToList();
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
                                    row2.RelativeItem().Padding(10).PaddingLeft(10).Text("ESQUEMA DE PAGOS DE MODELOS").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
                                });
                            });


                        });

                        page.Content().PaddingTop(10).PaddingLeft(10).PaddingRight(10).Column(col1 =>
                        {

                            //col1.Item().LineHorizontal(0.5f);

                            System.DateTime fecha = System.DateTime.Now;
                            string fechaActual = fecha.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("es-ES"));

                                col1.Item().EnsureSpace(100).Column(column =>
                                {

                                    column.Item().Row(row =>
                                    {
                                        row.RelativeItem().PaddingTop(5).AlignCenter()
                                            .Text(string.IsNullOrWhiteSpace(detalle.esquema) ? "PRECIO DE LISTA" : detalle.esquema)
                                            .FontSize(12).Bold().FontFamily(fontFamily);
                                    });

                                    col1.Item().PaddingBottom(10).PaddingTop(20).PaddingHorizontal(30).Border(0.5f).BorderColor("#477c2c").Table(tabla =>
                                    {
                                        tabla.ColumnsDefinition(Columns =>
                                        {
                                            Columns.RelativeColumn(1.4f);
                                            Columns.RelativeColumn(0.8f);
                                            Columns.RelativeColumn(2);
                                            Columns.RelativeColumn(0.8f);

                                        });

                                        tabla.Header(header =>
                                        {
                                            header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("LINEA").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("MODELO").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("DESCRIPCION").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("PRECIO NORMAL").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        });

                                        int index = 0;

                                        foreach (var det in detalle.modelos)
                                        {
                                            string rowBackground = (index % 2 == 0) ? "#FFFFFF" : "#F0F0F0";


                                            tabla.Cell().Background(rowBackground).BorderColor("#afb69d").MinHeight(20).AlignLeft().AlignMiddle().PaddingLeft(5).PaddingRight(3).PaddingVertical(3).ShowEntire()
                                           .Text(det.linea).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().Background(rowBackground).BorderColor("#afb69d").MinHeight(20).AlignLeft().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                           .Text(det.modelo).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().Background(rowBackground).BorderColor("#afb69d").MinHeight(20).AlignLeft().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                           .Text(det.descripcion.ToString()).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().Background(rowBackground).BorderColor("#afb69d").MinHeight(20).AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                           .Text(det.precio_lista.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                            index++;
                                        }
                                    });
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
                result.nombredocumento = "ESQUEMA DE PAGO DE MODELOS";
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
