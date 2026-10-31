using QuestPDF.Fluent;
using QuestPDF.Helpers;
using HD_Cobranza.Modelos;
using QuestPDF.Drawing;
using SkiaSharp;
using HD_Cobranza.Modelos.RecuperacionCartera;
using QuestPDF.Infrastructure;
using System.Text;
using HD_Cobranza.Modelos.ReporteRecuperacionCompleta;
using static ClosedXML.Excel.XLPredefinedFormat;

namespace HD_Reporteria.Cobranza
{
    public class RPT_Recuperacion_Completa
    {
        // Pinta el valor (arriba) y el porcentaje (abajo, mas chico y gris) dentro de la misma celda.
        // truncarMiles: si es true divide entre 1000 y trunca (igual que en la tabla de TOTAL CARTERA).
        private static void CeldaValorPorcentaje(
            IContainer celda,
            double valor,
            double? porcentaje,
            string fontFamily,
            bool truncarMiles = false)
        {
            string textoValor = truncarMiles
                ? Math.Truncate(valor / 1000).ToString("N0")
                : valor.ToString("N2");

            celda.Column(col =>
            {
                col.Item().AlignRight().PaddingRight(3).Text(textoValor)
                    .FontSize(8).FontFamily(fontFamily);

                if (porcentaje.HasValue)
                {
                    col.Item().AlignRight().PaddingRight(3).Text($"{porcentaje.Value:N2}%")
                        .FontSize(6).FontColor(Colors.Grey.Darken1).FontFamily(fontFamily);
                }
            });
        }

        public static RPT_Result GenerarPDF(mdl_Recuperacion_Completa_View datos)
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

                            System.DateTime fecha = System.DateTime.Now;
                            string fechaActual = fecha.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("es-ES"));

                            col1.Item().Row(row =>
                            {
                                row.RelativeItem().AlignRight().Text(txt =>
                                {
                                    txt.Span("INFORMACION AL: ").Bold().FontSize(8);
                                    txt.Span(fechaActual).FontSize(8);
                                });
                            });

                            col1.Item().PaddingVertical(10).Border(1).BorderColor("#477c2c").Table(tabla =>
                            {
                                tabla.ColumnsDefinition(Columns =>
                                {
                                    Columns.RelativeColumn(1.2f);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(0.9f);
                                    Columns.RelativeColumn(0.9f);
                                    Columns.RelativeColumn(0.9f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(0.6f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(0.6f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1.2f);
                                    Columns.RelativeColumn(0.8f);
                                    Columns.RelativeColumn(0.6f);
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(4).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("CARTERA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(3).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                   .Padding(1).Text("OBJETIVO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(9).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("RECUPERACION DE CARTERA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(3).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                   .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("PERIODO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ACTIVA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("POR VENCER").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("VENCIDA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("TOTAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("POR VENCER").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("VENCIDO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderRight(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("TOTAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("MES").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ACTIVA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("POR VENCER").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("VENCIDA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("TOTAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("   OBJETIVO\nRECUPERADO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");

                                foreach (var total in datos.total)
                                {

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignLeft().Height(28).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                    .Text(total.mes).FontSize(8).FontFamily(fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        total.cartera_activa, total.porc_cartera_activa, fontFamily, true);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        total.cartera_porvencer, total.porc_cartera_porvencer, fontFamily, true);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        total.cartera_vencida, total.porc_cartera_vencida, fontFamily, true);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        total.total_cartera, 100, fontFamily, true);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        total.objetivo_porvencer, total.porc_objetivo_porvencer, fontFamily, true);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        total.objetivo_vencido, total.porc_objetivo_vencido, fontFamily, true);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderRight(0.6f).BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        total.objetivo, 100, fontFamily, true);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        total.recuperacion_mes, total.porc_recuperacion_mes, fontFamily, true);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        total.recuperacion_activa, total.porc_recuperacion_activa, fontFamily, true);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        total.recuperacion_porvencer, total.porc_recuperacion_porvencer, fontFamily, true);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(28).AlignMiddle().PaddingRight(4).PaddingRight(3)
                                   .Text(total.porcporvencer.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(28).Padding(0)
                                    .SkiaSharpCanvas((canvas, size) =>
                                    {
                                        // Determinar el color del círculo
                                        SKColor color;
                                        int circleRadius = 3; // Radio del círculo
                                        float spacing = 10; // Espacio entre los círculos

                                        switch (total.indicadorporvencer)
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

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        total.recuperacion_vencida, total.porc_recuperacion_vencida, fontFamily, true);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(28).AlignMiddle().PaddingRight(4).PaddingRight(3)
                                   .Text(total.porcvencido.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(28).Padding(0)
                                    .SkiaSharpCanvas((canvas, size) =>
                                    {
                                        // Determinar el color del círculo
                                        SKColor color;
                                        int circleRadius = 3; // Radio del círculo
                                        float spacing = 10; // Espacio entre los círculos

                                        switch (total.indicadorvencido)
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

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        total.total_recuperado, 100, fontFamily, true);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        total.recuperado, null, fontFamily, true);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(28).AlignMiddle().PaddingRight(4).PaddingRight(3)
                                   .Text(total.porc.ToString("N2")).FontSize(8).FontFamily(fontFamily);


                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(28).Padding(0)
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

                            col1.Item().Row(row =>
                            {
                                row.RelativeItem().AlignRight().Text(txt =>
                                {
                                    txt.Span("INFORMACION AL: ").Bold().FontSize(8);
                                    txt.Span(fechaActual).FontSize(8);
                                });
                            });


                            col1.Item().PaddingVertical(10).Border(1).BorderColor("#477c2c").Table(tabla =>
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
                                    header.Cell().Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(4).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("CARTERA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                   .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(5).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("RECUPERACION DE CARTERA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(3).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                   .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("PERIODO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ACTIVA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("POR VENCER").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("VENCIDA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("TOTAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("OBJETIVO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("MES").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ACTIVA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("POR VENCER").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("VENCIDA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("TOTAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("   OBJETIVO\nRECUPERADO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");

                                foreach (var operacion in datos.operacion)
                                {

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignLeft().Height(28).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                    .Text(operacion.mes).FontSize(8).FontFamily(fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        operacion.cartera_activa, operacion.porc_cartera_activa, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        operacion.cartera_porvencer, operacion.porc_cartera_porvencer, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        operacion.cartera_vencida, operacion.porc_cartera_vencida, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        operacion.total_cartera, 100, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        operacion.objetivo, 100, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        operacion.recuperacion_mes, operacion.porc_recuperacion_mes, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        operacion.recuperacion_activa, operacion.porc_recuperacion_activa, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        operacion.recuperacion_porvencer, operacion.porc_recuperacion_porvencer, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        operacion.recuperacion_vencida, operacion.porc_recuperacion_vencida, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        operacion.total_recuperado, 100, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        operacion.recuperado, null, fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(28).AlignMiddle().PaddingRight(4).PaddingRight(3)
                                   .Text(operacion.porc.ToString("N2") + " %").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(28).Padding(0)
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

                            col1.Item().Row(row =>
                            {
                                row.RelativeItem().AlignRight().Text(txt =>
                                {
                                    txt.Span("INFORMACION AL: ").Bold().FontSize(8);
                                    txt.Span(fechaActual).FontSize(8);
                                });
                            });


                            col1.Item().PaddingVertical(10).Border(1).BorderColor("#477c2c").Table(tabla =>
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
                                    header.Cell().Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(4).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("CARTERA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                   .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(5).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("RECUPERACION DE CARTERA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(3).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                   .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("PERIODO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ACTIVA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("POR VENCER").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("VENCIDA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("TOTAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("OBJETIVO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("MES").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ACTIVA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("POR VENCER").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("VENCIDA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("TOTAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("   OBJETIVO\nRECUPERADO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");

                                foreach (var revolvente in datos.revolvente)
                                {

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignLeft().Height(28).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                    .Text(revolvente.mes).FontSize(8).FontFamily(fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        revolvente.cartera_activa, revolvente.porc_cartera_activa, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        revolvente.cartera_porvencer, revolvente.porc_cartera_porvencer, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        revolvente.cartera_vencida, revolvente.porc_cartera_vencida, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        revolvente.total_cartera, 100, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        revolvente.objetivo, 100, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        revolvente.recuperacion_mes, revolvente.porc_recuperacion_mes, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        revolvente.recuperacion_activa, revolvente.porc_recuperacion_activa, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        revolvente.recuperacion_porvencer, revolvente.porc_recuperacion_porvencer, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        revolvente.recuperacion_vencida, revolvente.porc_recuperacion_vencida, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        revolvente.total_recuperado, 100, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        revolvente.recuperado, null, fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(28).AlignMiddle().PaddingRight(4).PaddingRight(3)
                                   .Text(revolvente.porc.ToString("N2") + " %").FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(28).Padding(0)
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

                            col1.Item().PageBreak();

                            col1.Item().Row(row =>
                            {
                                row.RelativeItem().AlignCenter().Text(txt =>
                                {
                                    txt.Span("CARTERA ESPECIAL").FontSize(12).Bold();
                                });
                            });

                            col1.Item().Row(row =>
                            {
                                row.RelativeItem().AlignRight().Text(txt =>
                                {
                                    txt.Span("INFORMACION AL: ").Bold().FontSize(8);
                                    txt.Span(fechaActual).FontSize(8);
                                });
                            });

                            col1.Item().PaddingVertical(10).Border(1).BorderColor("#477c2c").Table(tabla =>
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
                                    header.Cell().Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(4).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("CARTERA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                   .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(5).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("RECUPERACION DE CARTERA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(3).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                   .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("PERIODO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ACTIVA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("POR VENCER").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("VENCIDA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("TOTAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("OBJETIVO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("MES").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ACTIVA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("POR VENCER").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("VENCIDA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("TOTAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("   OBJETIVO\nRECUPERADO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");

                                foreach (var especial in datos.especial)
                                {

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignLeft().Height(28).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                    .Text(especial.mes).FontSize(8).FontFamily(fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        especial.cartera_activa, especial.porc_cartera_activa, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        especial.cartera_porvencer, especial.porc_cartera_porvencer, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        especial.cartera_vencida, especial.porc_cartera_vencida, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        especial.total_cartera, 100, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        especial.objetivo, 100, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        especial.recuperacion_mes, especial.porc_recuperacion_mes, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        especial.recuperacion_activa, especial.porc_recuperacion_activa, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        especial.recuperacion_porvencer, especial.porc_recuperacion_porvencer, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        especial.recuperacion_vencida, especial.porc_recuperacion_vencida, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        especial.total_recuperado, 100, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        especial.recuperado, null, fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(28).AlignMiddle().PaddingRight(4).PaddingRight(3)
                                   .Text(especial.porc.ToString("N2") + " %").FontSize(8).FontFamily(fontFamily);


                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(28).Padding(0)
                                    .SkiaSharpCanvas((canvas, size) =>
                                    {
                                        // Determinar el color del círculo
                                        SKColor color;
                                        int circleRadius = 3; // Radio del círculo
                                        float spacing = 10; // Espacio entre los círculos

                                        switch (especial.indicador)
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
                                        if (especial.indicador == "RR")
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
                                    txt.Span("CARTERA JURIDICA").FontSize(12).Bold();
                                });
                            });

                            col1.Item().Row(row =>
                            {
                                row.RelativeItem().AlignRight().Text(txt =>
                                {
                                    txt.Span("INFORMACION AL: ").Bold().FontSize(8);
                                    txt.Span(fechaActual).FontSize(8);
                                });
                            });

                            col1.Item().PaddingVertical(10).Border(1).BorderColor("#477c2c").Table(tabla =>
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
                                    header.Cell().Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(4).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("CARTERA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                   .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(5).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("RECUPERACION DE CARTERA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(3).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                   .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("PERIODO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ACTIVA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("POR VENCER").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("VENCIDA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("TOTAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("OBJETIVO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("MES").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ACTIVA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("POR VENCER").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("VENCIDA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("TOTAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderLeft(0.6f).BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("   OBJETIVO\nRECUPERADO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("%").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");

                                foreach (var juridico in datos.juridico)
                                {

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignLeft().Height(28).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                    .Text(juridico.mes).FontSize(8).FontFamily(fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        juridico.cartera_activa, juridico.porc_cartera_activa, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        juridico.cartera_porvencer, juridico.porc_cartera_porvencer, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        juridico.cartera_vencida, juridico.porc_cartera_vencida, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        juridico.total_cartera, 100, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        juridico.objetivo, 100, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        juridico.recuperacion_mes, juridico.porc_recuperacion_mes, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        juridico.recuperacion_activa, juridico.porc_recuperacion_activa, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        juridico.recuperacion_porvencer, juridico.porc_recuperacion_porvencer, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        juridico.recuperacion_vencida, juridico.porc_recuperacion_vencida, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        juridico.total_recuperado, 100, fontFamily);

                                    CeldaValorPorcentaje(
                                        tabla.Cell().BorderLeft(0.6f).BorderBottom(1).BorderColor("#afb69d").Height(28).AlignMiddle(),
                                        juridico.recuperado, null, fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignRight().Height(28).AlignMiddle().PaddingRight(4).PaddingRight(3)
                                   .Text(juridico.porc.ToString("N2") + " %").FontSize(8).FontFamily(fontFamily);


                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(28).Padding(0)
                                    .SkiaSharpCanvas((canvas, size) =>
                                    {
                                        // Determinar el color del círculo
                                        SKColor color;
                                        int circleRadius = 3; // Radio del círculo
                                        float spacing = 10; // Espacio entre los círculos

                                        switch (juridico.indicador)
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
                                        if (juridico.indicador == "RR")
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