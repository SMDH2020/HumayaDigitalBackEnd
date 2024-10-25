using QuestPDF.Fluent;
using QuestPDF.Helpers;
using HD_Cobranza.Modelos;
using QuestPDF.Drawing;
using SkiaSharp;
using HD_Cobranza.Modelos.RecuperacionCartera;
using QuestPDF.Infrastructure;
using System.Text;

namespace HD_Reporteria.Cobranza
{

    public static class SkiaSharpHelpers
    {
        public static void SkiaSharpCanvas(this IContainer container, Action<SKCanvas, Size> drawOnCanvas)
        {
            container.Svg(size =>
            {
                using var stream = new MemoryStream();

                using (var canvas = SKSvgCanvas.Create(new SKRect(0, 0, size.Width, size.Height), stream))
                    drawOnCanvas(canvas, size);

                var svgData = stream.ToArray();
                return Encoding.UTF8.GetString(svgData);
            });
        }

        public static void SkiaSharpRasterized(this IContainer container, Action<SKCanvas, Size> drawOnCanvas)
        {
            container.Image(payload =>
            {
                using var bitmap = new SKBitmap(payload.ImageSize.Width, payload.ImageSize.Height);

                using (var canvas = new SKCanvas(bitmap))
                {
                    var scalingFactor = payload.Dpi / (float)DocumentSettings.DefaultRasterDpi;
                    canvas.Scale(scalingFactor);
                    drawOnCanvas(canvas, payload.AvailableSpace);
                }

                return bitmap.Encode(SKEncodedImageFormat.Png, 100).ToArray();
            });
        }
    }
    public class RPT_Recuperacion_Cartera_Mensual
    {
        public static RPT_Result GenerarPDF(mdlRecuperacionObjetivoView datos)
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
                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text("RECUPERACION DE CARTERA MENSUAL").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
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
                                    txt.Span("TOTAL CARTERA").FontSize(12).Bold();
                                });
                            });

                            col1.Item().PaddingVertical(10).Border(1).BorderColor("#275027").Table(tabla =>
                            {
                                tabla.ColumnsDefinition(Columns =>
                                {
                                    Columns.RelativeColumn(0.9f);
                                    Columns.RelativeColumn(1.1f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.6f);
                                    Columns.RelativeColumn(0.4f);
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(4).BorderLeft(0.6f).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("CARTERA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderLeft(0.6f).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                   .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(5).BorderLeft(0.6f).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("RECUPERACION DE CARTERA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(3).BorderLeft(0.6f).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                   .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("PERIODO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ACTIVA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("POR VENCER").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("VENCIDA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("TOTAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("OBJETIVO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("MES").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ACTIVA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("POR VENCER").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("VENCIDA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("TOTAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("   OBJETIVO\nRECUPERADO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");

                                foreach (var total in datos.total)
                                {

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignLeft().Height(20).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                    .Text(total.mes).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(total.cartera_activa.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(total.cartera_porvencer.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(total.cartera_vencida.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(total.total_cartera.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(total.objetivo.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(total.recuperacion_mes.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(total.recuperacion_activa.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(total.recuperacion_porvencer.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(total.recuperacion_vencida.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(total.total_recuperado.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(total.recuperado.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(4).PaddingRight(3)
                                   .Text(total.porc.ToString("N2") + " %").FontSize(8).FontFamily(fontFamily);


                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).Padding(0)
                                    .SkiaSharpCanvas((canvas, size) =>
                                    {
                                        // Determinar el color del círculo
                                        SKColor color;
                                        int circleRadius = 3; // Radio del círculo
                                        float spacing = 10; // Espacio entre los círculos

                                        switch (total.indicador)
                                        {
                                            case "V":
                                                color = SKColors.Green; // Verde
                                                break;
                                            case "A":
                                                color = SKColors.Yellow; // Amarillo
                                                break;
                                            case "R":
                                                color = SKColors.Red; // Rojo
                                                break;
                                            case "RR":
                                                // Para "RR", dibujar dos círculos rojos
                                                color = SKColors.Red;
                                                break;
                                            default:
                                                color = SKColors.Transparent; // Sin color
                                                break;
                                        }

                                        // Crear el pincel para dibujar el círculo
                                        using var paint = new SKPaint
                                        {
                                            Color = color,
                                            Style = SKPaintStyle.Fill
                                        };

                                        // Dibujar círculos
                                        if (total.indicador == "RR")
                                        {
                                            // Dibujar primer círculo
                                            canvas.DrawCircle(size.Width / 2 - spacing / 2, size.Height / 2, circleRadius, paint);
                                            // Dibujar segundo círculo
                                            canvas.DrawCircle(size.Width / 2 + spacing / 2, size.Height / 2, circleRadius, paint);
                                        }
                                        else
                                        {
                                            // Dibujar un solo círculo
                                            canvas.DrawCircle(size.Width / 2, size.Height / 2, circleRadius, paint);
                                        }
                                    });
                                }

                            });

                            col1.Item().PageBreak();


                            col1.Item().Row(row =>
                            {
                                row.RelativeItem().AlignCenter().Text(txt =>
                                {
                                    txt.Span("CARTERA DE OPERACION").FontSize(12).Bold();
                                });
                            });

                            col1.Item().PaddingVertical(10).Border(1).BorderColor("#275027").Table(tabla =>
                            {
                                tabla.ColumnsDefinition(Columns =>
                                {
                                    Columns.RelativeColumn(0.9f);
                                    Columns.RelativeColumn(1.1f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.6f);
                                    Columns.RelativeColumn(0.4f);
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(4).BorderLeft(0.6f).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("CARTERA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderLeft(0.6f).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                   .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(5).BorderLeft(0.6f).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("RECUPERACION DE CARTERA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(3).BorderLeft(0.6f).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                   .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("PERIODO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ACTIVA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("POR VENCER").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("VENCIDA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("TOTAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("OBJETIVO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("MES").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ACTIVA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("POR VENCER").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("VENCIDA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("TOTAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("   OBJETIVO\nRECUPERADO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");

                                foreach (var operacion in datos.operacion)
                                {

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignLeft().Height(20).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                    .Text(operacion.mes).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(operacion.cartera_activa.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(operacion.cartera_porvencer.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(operacion.cartera_vencida.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(operacion.total_cartera.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(operacion.objetivo.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(operacion.recuperacion_mes.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(operacion.recuperacion_activa.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(operacion.recuperacion_porvencer.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(operacion.recuperacion_vencida.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(operacion.total_recuperado.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(operacion.recuperado.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(4).PaddingRight(3)
                                   .Text(operacion.porc.ToString("N2") + " %").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).Padding(0)
                                   .SkiaSharpCanvas((canvas, size) =>
                                   {
                                       // Determinar el color del círculo
                                       SKColor color;
                                       int circleRadius = 3; // Radio del círculo
                                       float spacing = 10; // Espacio entre los círculos

                                       switch (operacion.indicador)
                                       {
                                           case "V":
                                               color = SKColors.Green; // Verde
                                               break;
                                           case "A":
                                               color = SKColors.Yellow; // Amarillo
                                               break;
                                           case "R":
                                               color = SKColors.Red; // Rojo
                                               break;
                                           case "RR":
                                               // Para "RR", dibujar dos círculos rojos
                                               color = SKColors.Red;
                                               break;
                                           default:
                                               color = SKColors.Transparent; // Sin color
                                               break;
                                       }

                                       // Crear el pincel para dibujar el círculo
                                       using var paint = new SKPaint
                                       {
                                           Color = color,
                                           Style = SKPaintStyle.Fill
                                       };

                                       // Dibujar círculos
                                       if (operacion.indicador == "RR")
                                       {
                                           // Dibujar primer círculo
                                           canvas.DrawCircle(size.Width / 2 - spacing / 2, size.Height / 2, circleRadius, paint);
                                           // Dibujar segundo círculo
                                           canvas.DrawCircle(size.Width / 2 + spacing / 2, size.Height / 2, circleRadius, paint);
                                       }
                                       else
                                       {
                                           // Dibujar un solo círculo
                                           canvas.DrawCircle(size.Width / 2, size.Height / 2, circleRadius, paint);
                                       }
                                   });
                                }
                            });

                            col1.Item().PageBreak();


                            col1.Item().Row(row =>
                            {
                                row.RelativeItem().AlignCenter().Text(txt =>
                                {
                                    txt.Span("CARTERA DE REVOLVENTE").FontSize(12).Bold();
                                });
                            });

                            col1.Item().PaddingVertical(10).Border(1).BorderColor("#275027").Table(tabla =>
                            {
                                tabla.ColumnsDefinition(Columns =>
                                {
                                    Columns.RelativeColumn(0.9f);
                                    Columns.RelativeColumn(1.1f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.6f);
                                    Columns.RelativeColumn(0.4f);
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(4).BorderLeft(0.6f).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("CARTERA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderLeft(0.6f).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                   .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(5).BorderLeft(0.6f).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("RECUPERACION DE CARTERA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(3).BorderLeft(0.6f).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                   .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("PERIODO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ACTIVA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("POR VENCER").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("VENCIDA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("TOTAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("OBJETIVO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("MES").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ACTIVA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("POR VENCER").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("VENCIDA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("TOTAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("   OBJETIVO\nRECUPERADO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#275027").AlignCenter().AlignMiddle()
                                .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");

                                foreach (var revolvente in datos.revolvente)
                                {

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignLeft().Height(20).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                    .Text(revolvente.mes).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(revolvente.cartera_activa.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(revolvente.cartera_porvencer.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(revolvente.cartera_vencida.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(revolvente.total_cartera.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(revolvente.objetivo.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(revolvente.recuperacion_mes.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(revolvente.recuperacion_activa.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(revolvente.recuperacion_porvencer.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(revolvente.recuperacion_vencida.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(revolvente.total_recuperado.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(3)
                                   .Text(revolvente.recuperado.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(20).AlignMiddle().PaddingRight(4).PaddingRight(3)
                                   .Text(revolvente.porc.ToString("N2") + " %").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).Padding(0)
                                   .SkiaSharpCanvas((canvas, size) =>
                                   {
                                       // Determinar el color del círculo
                                       SKColor color;
                                       int circleRadius = 3; // Radio del círculo
                                       float spacing = 10; // Espacio entre los círculos

                                       switch (revolvente.indicador)
                                       {
                                           case "V":
                                               color = SKColors.Green; // Verde
                                               break;
                                           case "A":
                                               color = SKColors.Yellow; // Amarillo
                                               break;
                                           case "R":
                                               color = SKColors.Red; // Rojo
                                               break;
                                           case "RR":
                                               // Para "RR", dibujar dos círculos rojos
                                               color = SKColors.Red;
                                               break;
                                           default:
                                               color = SKColors.Transparent; // Sin color
                                               break;
                                       }

                                       // Crear el pincel para dibujar el círculo
                                       using var paint = new SKPaint
                                       {
                                           Color = color,
                                           Style = SKPaintStyle.Fill
                                       };

                                       // Dibujar círculos
                                       if (revolvente.indicador == "RR")
                                       {
                                           // Dibujar primer círculo
                                           canvas.DrawCircle(size.Width / 2 - spacing / 2, size.Height / 2, circleRadius, paint);
                                           // Dibujar segundo círculo
                                           canvas.DrawCircle(size.Width / 2 + spacing / 2, size.Height / 2, circleRadius, paint);
                                       }
                                       else
                                       {
                                           // Dibujar un solo círculo
                                           canvas.DrawCircle(size.Width / 2, size.Height / 2, circleRadius, paint);
                                       }
                                   });

                                }
                            });
                        });

                        page.Footer().Height(40).PaddingLeft(30).PaddingRight(30).PaddingBottom(20).Row(row =>
                        {
                            row.RelativeItem().AlignRight().PaddingTop(0).Text(txt =>
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
                result.nombredocumento = "RESUMEN CARTERA POR SUCURSAL";
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
