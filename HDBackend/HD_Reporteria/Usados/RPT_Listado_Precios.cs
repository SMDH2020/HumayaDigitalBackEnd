using QuestPDF.Fluent;
using QuestPDF.Helpers;
using ClosedXML.Excel;
using Usados.Consultas.Usados;
using HD_Reporteria.Cobranza;
using static ClosedXML.Excel.XLPredefinedFormat;
using SkiaSharp;

namespace HD_Reporteria.Usados
{
    public class RPT_Listado_Precios
    {
        public static RPT_Result GenerarPDF(IEnumerable<mdl_Inventario> detalle)
        {
            try
            {
                var detalleOrdenado = detalle
                .OrderBy(det => det.estatus == "L" ? 0 : det.estatus == "A" ? 1 : 2)
                .ThenBy(det => det.sucursal)
                .ThenBy(det => det.HP)
                .ToList();
                string fontFamily = "Calibri";
                byte[] doc = Document.Create(document =>
                {
                    document.Page(page =>
                    {
                        page.Size(PageSizes.A4.Landscape());

                        var registrosListos = detalleOrdenado.Where(x => x.estatus.Contains("L"));
                        var registrosAcondicionando = detalleOrdenado.Where(x => x.estatus.Contains("A"));
                        var registrosTrilladoras = detalleOrdenado
                            .Where(x => x.modelo_descripcion.Contains("trilladora", StringComparison.OrdinalIgnoreCase));
                        var registrosTractores = detalleOrdenado
                            .Where(x => x.modelo_descripcion.Contains("tr-", StringComparison.OrdinalIgnoreCase)
                                     || x.modelo_descripcion.Contains("tr", StringComparison.OrdinalIgnoreCase)
                                            && !x.modelo_descripcion.Contains("trilladora", StringComparison.OrdinalIgnoreCase)
                                            && !x.modelo_descripcion.Contains("trasn", StringComparison.OrdinalIgnoreCase)
                                            && !x.modelo_descripcion.Contains("trans", StringComparison.OrdinalIgnoreCase)
                                     || x.modelo_descripcion.Contains("tractor", StringComparison.OrdinalIgnoreCase));
                        var registrosImplementos = detalleOrdenado.Except(registrosTractores)
                                                    .Except(registrosTrilladoras);

                        bool cambioTitulo = false;

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
                                    string titulo;

                                    if (cambioTitulo)
                                    {
                                        titulo = "INVENTARIO DE MAQUINARIA USADA ACONDICIONANDO";
                                        
                                    } else
                                    {
                                        titulo = "INVENTARIO DE MAQUINARIA USADA";
                                    }

                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text(titulo).FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
                                    //+obtenernombre_mes(periodo) + " " + ejercicio
                                });
                            });


                        });

                        page.Content().PaddingTop(10).PaddingLeft(30).PaddingRight(30).Column(col1 =>
                        {

                            //col1.Item().LineHorizontal(0.5f);

                            System.DateTime fecha = System.DateTime.Now;
                            string fechaActual = fecha.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("es-ES"));

                            if (registrosTractores.Any())
                            {

                                var productosPorPromocion = registrosTractores
                               .GroupBy(p => new { p.promocion, p.vigencia })
                               .OrderBy(g => g.Key.promocion)
                               .ToList();

                                col1.Item().PaddingTop(10).Row(row =>
                                {
                                    row.RelativeItem().PaddingLeft(10).AlignLeft()
                                        .Text("Tractores")
                                        .FontSize(12).Bold().FontFamily(fontFamily);
                                });

                                col1.Item().PaddingVertical(10).PaddingHorizontal(10).Border(0.5f).BorderColor("#477c2c").Table(tabla =>
                                {
                                    tabla.ColumnsDefinition(Columns =>
                                    {
                                        Columns.RelativeColumn(0.4f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.7f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.7f);
                                        Columns.RelativeColumn(0.5f);
                                        Columns.RelativeColumn(1.2f);
                                        Columns.RelativeColumn(0.5f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.9f);
                                        Columns.RelativeColumn(0.9f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.6f);
                                        Columns.RelativeColumn(0.9f);
                                        Columns.RelativeColumn(0.8f);
                                    });

                                    tabla.Header(header =>
                                    {
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("SUCURSAL").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("N. ECON.").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("MARCA").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("MODELO").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("EJERCICIO").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("HP").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("SERIE").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("HORAS").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("RECEPCIÓN").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("COSTO").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("OT").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("COSTO TOTAL").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("UTILIDAD").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("MARGEN").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("PRECIO LISTA").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("PROMOCION").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    });

                                    int index = 0;

                                    foreach (var det in registrosTractores)
                                    {
                                        string rowBackground = (index % 2 == 0) ? "#FFFFFF" : "#F0F0F0";

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().Height(20).Padding(0)
                                        .SkiaSharpCanvas((canvas, size) =>
                                        {
                                            // Determinar el color del círculo
                                            SKColor color;
                                            int circleRadius = 3; // Radio del círculo
                                            float spacing = 10; // Espacio entre los círculos

                                            switch (det.estatus)
                                            {
                                                case "L":
                                                    color = SKColors.Green; // Verde
                                                    break;
                                                case "A":
                                                    color = SKColors.Yellow; // Amarillo
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
                                            // Dibujar un solo círculo
                                            canvas.DrawCircle(size.Width / 2, size.Height / 2, circleRadius, paint);
                                        });

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingLeft(4).PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.nombre_sucursal?.ToUpper()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.NE).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.Marca == "JD" ? "JOHN DEERE" : det.Marca).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.modelo_descripcion).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.ejercicio.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.HP.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.serie?.ToUpper()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.horas.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.fecha_recepcion).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.Costo.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.OT.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.costo_total.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.utilidad.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.margen.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text((0).ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                      .Text(!string.IsNullOrEmpty(det.promocion) && !string.IsNullOrEmpty(det.vigencia)
                                       ? det.promocion.ToUpper() + " Vigencia al " + det.vigencia
                                       : !string.IsNullOrEmpty(det.promocion)
                                           ? det.promocion.ToUpper()
                                           : !string.IsNullOrEmpty(det.vigencia)
                                               ? "Vigencia al " + det.vigencia
                                               : "").FontSize(7).FontFamily(fontFamily);

                                        index++;

                                    }
                                });
                            }

                            if (registrosTractores.Any() && registrosTrilladoras.Any())
                            {
                                col1.Item().PageBreak();
                            }

                            if (registrosTrilladoras.Any())
                            {

                                col1.Item().PaddingTop(10).Row(row =>
                                {
                                    row.RelativeItem().PaddingLeft(10).AlignLeft()
                                        .Text("Trilladoras")
                                        .FontSize(12).Bold().FontFamily(fontFamily);
                                });

                                col1.Item().PaddingVertical(10).PaddingHorizontal(10).Border(0.5f).BorderColor("#477c2c").Table(tabla =>
                                {
                                    tabla.ColumnsDefinition(Columns =>
                                    {
                                        Columns.RelativeColumn(0.4f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.7f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.7f);
                                        Columns.RelativeColumn(0.5f);
                                        Columns.RelativeColumn(1.2f);
                                        Columns.RelativeColumn(0.5f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.9f);
                                        Columns.RelativeColumn(0.9f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.6f);
                                        Columns.RelativeColumn(0.9f);
                                        Columns.RelativeColumn(0.8f);
                                    });

                                    tabla.Header(header =>
                                    {
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("SUCURSAL").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("N. ECON.").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("MARCA").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("MODELO").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("EJERCICIO").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("HP").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("SERIE").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("HORAS").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("RECEPCIÓN").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("COSTO").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("OT").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("COSTO TOTAL").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("UTILIDAD").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("MARGEN").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("PRECIO LISTA").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("PROMOCION").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    });

                                    int index = 0;

                                    foreach (var det in registrosTrilladoras)
                                    {
                                        string rowBackground = (index % 2 == 0) ? "#FFFFFF" : "#F0F0F0";

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().Height(20).Padding(0)
                                        .SkiaSharpCanvas((canvas, size) =>
                                        {
                                            // Determinar el color del círculo
                                            SKColor color;
                                            int circleRadius = 3; // Radio del círculo
                                            float spacing = 10; // Espacio entre los círculos

                                            switch (det.estatus)
                                            {
                                                case "L":
                                                    color = SKColors.Green; // Verde
                                                    break;
                                                case "A":
                                                    color = SKColors.Yellow; // Amarillo
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
                                            // Dibujar un solo círculo
                                            canvas.DrawCircle(size.Width / 2, size.Height / 2, circleRadius, paint);
                                        });

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingLeft(4).PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.nombre_sucursal?.ToUpper()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.NE).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.Marca == "JD" ? "JOHN DEERE" : det.Marca).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.modelo_descripcion).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.ejercicio.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.HP.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.serie?.ToUpper()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.horas.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.fecha_recepcion).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.Costo.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.OT.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.costo_total.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.utilidad.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.margen.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text((0).ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                      .Text(!string.IsNullOrEmpty(det.promocion) && !string.IsNullOrEmpty(det.vigencia)
                                       ? det.promocion.ToUpper() + " Vigencia al " + det.vigencia
                                       : !string.IsNullOrEmpty(det.promocion)
                                           ? det.promocion.ToUpper()
                                           : !string.IsNullOrEmpty(det.vigencia)
                                               ? "Vigencia al " + det.vigencia
                                               : "").FontSize(7).FontFamily(fontFamily);

                                        index++;

                                    }
                                });
                                }

                            if (registrosTractores.Any() && registrosImplementos.Any() || registrosTrilladoras.Any() && registrosImplementos.Any())
                            {
                                col1.Item().PageBreak();
                            }

                            if (registrosImplementos.Any())
                            {

                                col1.Item().PaddingTop(10).Row(row =>
                                {
                                    row.RelativeItem().PaddingLeft(10).AlignLeft()
                                        .Text("Cabezales e Implementos")
                                        .FontSize(12).Bold().FontFamily(fontFamily);
                                });

                                col1.Item().PaddingVertical(10).PaddingHorizontal(10).Border(0.5f).BorderColor("#477c2c").Table(tabla =>
                                {
                                    tabla.ColumnsDefinition(Columns =>
                                    {
                                        Columns.RelativeColumn(0.4f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.7f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.7f);
                                        Columns.RelativeColumn(0.5f);
                                        Columns.RelativeColumn(1.2f);
                                        Columns.RelativeColumn(0.5f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.9f);
                                        Columns.RelativeColumn(0.9f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.6f);
                                        Columns.RelativeColumn(0.9f);
                                        Columns.RelativeColumn(0.8f);
                                    });

                                    tabla.Header(header =>
                                    {
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("SUCURSAL").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("N. ECON.").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("MARCA").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("MODELO").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("EJERCICIO").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("HP").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("SERIE").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("HORAS").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("RECEPCIÓN").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("COSTO").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("OT").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("COSTO TOTAL").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("UTILIDAD").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("MARGEN").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("PRECIO LISTA").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("PROMOCION").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    });

                                    int index = 0;

                                    foreach (var det in registrosImplementos)
                                    {
                                        string rowBackground = (index % 2 == 0) ? "#FFFFFF" : "#F0F0F0";

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().Height(20).Padding(0)
                                        .SkiaSharpCanvas((canvas, size) =>
                                        {
                                            // Determinar el color del círculo
                                            SKColor color;
                                            int circleRadius = 3; // Radio del círculo
                                            float spacing = 10; // Espacio entre los círculos

                                            switch (det.estatus)
                                            {
                                                case "L":
                                                    color = SKColors.Green; // Verde
                                                    break;
                                                case "A":
                                                    color = SKColors.Yellow; // Amarillo
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
                                            // Dibujar un solo círculo
                                            canvas.DrawCircle(size.Width / 2, size.Height / 2, circleRadius, paint);
                                        });

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingLeft(4).PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.nombre_sucursal?.ToUpper()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.NE).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.Marca == "JD" ? "JOHN DEERE" : det.Marca).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.modelo_descripcion).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.ejercicio.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.HP.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.serie?.ToUpper()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.horas.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.fecha_recepcion).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.Costo.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.OT.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.costo_total.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.utilidad.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text(det.margen.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                       .Text((0).ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(rowBackground).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                      .Text(!string.IsNullOrEmpty(det.promocion) && !string.IsNullOrEmpty(det.vigencia)
                                       ? det.promocion.ToUpper() + " Vigencia al " + det.vigencia
                                       : !string.IsNullOrEmpty(det.promocion)
                                           ? det.promocion.ToUpper()
                                           : !string.IsNullOrEmpty(det.vigencia)
                                               ? "Vigencia al " + det.vigencia
                                               : "").FontSize(7).FontFamily(fontFamily);

                                        index++;

                                    }
                                });
                            }
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
                result.nombredocumento = "LISTADO DE PRECIOS";
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
