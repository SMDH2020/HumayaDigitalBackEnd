using QuestPDF.Fluent;
using QuestPDF.Helpers;
using HD_Ventas.Modelos;
using HD_Reporteria.Cobranza;

namespace HD_Reporteria.Ventas
{
    public class RPT_Scorecard_General_Dash
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
        public static RPT_Result GenerarPDF(IEnumerable<mdlCarga_Scorecard_porVendedor_Dash> scorecard, int ejercicio, int mes_actual, int ejercicio_inicio, int periodo_inicio)
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
                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text("SCORECARD").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
                                });
                            });


                        });

                        page.Content().PaddingTop(10).PaddingLeft(30).PaddingRight(30).Column(col1 =>
                        {

                            //col1.Item().LineHorizontal(0.5f);

                            col1.Item().Row(row =>
                            {
                                row.RelativeItem().AlignCenter().Text(txt =>
                                {
                                    txt.Span("SCORECARD GENERAL").FontSize(12).Bold();
                                });
                            });

                            col1.Item().PaddingVertical(10).Border(1).BorderColor("#275027").Table(tabla =>
                            {
                                tabla.ColumnsDefinition(Columns =>
                                {
                                    Columns.RelativeColumn(1.5f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(3).BorderLeft(0.6f).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text(obtenernombre_mes(mes_actual)).FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(3).BorderLeft(0.6f).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text(obtenernombre_mes(periodo_inicio) + " " + ejercicio_inicio + " A " + obtenernombre_mes(mes_actual) + " " + ejercicio).FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("LINEA").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("OBJETIVO").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("REAL").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ALCANCE").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("OBJETIVO").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("REAL").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ALCANCE").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");

                                double porcentaje = 0;
                                double porcentajeacumulado = 0;

                                foreach (var sco in scorecard)
                                {

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignLeft().Height(20).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                    .Text(sco.linea).FontSize(10).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(sco.objetivo.ToString()).FontSize(10).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                  .Text(sco.unidades_vendidas.ToString()).FontSize(10).FontFamily(fontFamily);

                                    if (sco.unidades_vendidas != 0)
                                    {
                                        porcentaje = (double)sco.unidades_vendidas / (double)sco.objetivo * 100;
                                        if (porcentaje > 100)
                                            porcentaje = 100;
                                    }
                                    else
                                        porcentaje = 0;

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(Math.Round(porcentaje, 2).ToString() + " %").FontSize(10).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(sco.objetivo_acumulado.ToString()).FontSize(10).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(sco.unidades_vendidas_acumulado.ToString()).FontSize(10).FontFamily(fontFamily);

                                    if (sco.unidades_vendidas_acumulado != 0)
                                    {
                                        porcentajeacumulado = (double)sco.unidades_vendidas_acumulado / (double)sco.objetivo_acumulado * 100;
                                        if (porcentajeacumulado > 100)
                                            porcentajeacumulado = 100;
                                    }
                                    else
                                        porcentajeacumulado = 0;

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(Math.Round(porcentajeacumulado).ToString() + " %").FontSize(10).FontFamily(fontFamily);
                                }

                                float totalImporteProyectado = scorecard.Sum(sco => sco.importe_proyectado);
                                float totalImporte = scorecard.Sum(sco => sco.importe);
                                float totalImporteProyectadoAcumulado = scorecard.Sum(sco => sco.importe_proyectado_acumulado);
                                float totalImporteAcumulado = scorecard.Sum(sco => sco.importe_acumulado);

                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Background("#e5e6e6").AlignLeft().Height(20).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                    .Text("IMPORTE TOTAL").Bold().FontSize(10).FontFamily(fontFamily);
                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Background("#e5e6e6").AlignRight().Height(20).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                    .Text(totalImporteProyectado.ToString("N2")).Bold().FontSize(10).FontFamily(fontFamily);
                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Background("#e5e6e6").AlignRight().Height(20).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                    .Text(totalImporte.ToString("N2")).Bold().FontSize(10).FontFamily(fontFamily);
                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Background("#e5e6e6").AlignLeft().Height(20).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                    .Text("").FontSize(10).FontFamily(fontFamily);
                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Background("#e5e6e6").AlignRight().Height(20).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                    .Text(totalImporteProyectadoAcumulado.ToString("N2")).Bold().FontSize(10).FontFamily(fontFamily);
                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Background("#e5e6e6").AlignRight().Height(20).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                    .Text(totalImporteAcumulado.ToString("N2")).Bold().FontSize(10).FontFamily(fontFamily);
                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Background("#e5e6e6").AlignLeft().Height(20).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                    .Text("").FontSize(10).FontFamily(fontFamily);
                            });
                        });

                    });

                }).GeneratePdf();
                RPT_Result result = new RPT_Result();
                result.extension = "pdf";
                result.nombredocumento = "SCORECARD";
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
