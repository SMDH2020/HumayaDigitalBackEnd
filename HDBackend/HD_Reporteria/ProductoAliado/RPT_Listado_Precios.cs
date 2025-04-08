using HD_Reporteria.Cobranza;
using ProductoAliado.Modelos.Inventario;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Reporteria.ProductoAliado
{
    public class RPT_Listado_Precios
    {
        public static RPT_Result GenerarPDF(IEnumerable<mdl_Inventario_Producto_Aliado> detalle)
        {
            try
            {
                var detalleOrdenado = detalle
                    .OrderBy(det => det.estatus == "L" ? 0 : det.estatus == "A" ? 1 : 2)
                    .ThenBy(det => det.sucursal)
                    .ToList();

                var registrosAspersoras = detalleOrdenado
                    .Where(x => x.modelo_descripcion.Contains("aspersora", StringComparison.OrdinalIgnoreCase));

                var registrosRemolques = detalleOrdenado
                    .Where(x => x.modelo_descripcion.Contains("remolque", StringComparison.OrdinalIgnoreCase));

                var registrosMolinos = detalleOrdenado
                    .Where(x => x.modelo_descripcion.Contains("molino", StringComparison.OrdinalIgnoreCase));

                var registrosEquiposFertilizacion = detalleOrdenado.Except(registrosAspersoras)
                                                    .Except(registrosRemolques)
                                                    .Except(registrosMolinos);
                string fontFamily = "Calibri";
                byte[] doc = Document.Create(document =>
                {
                    document.Page(page =>
                    {
                        page.Size(PageSizes.Letter.Portrait());
                        var registrosListos = detalleOrdenado.Where(x => x.estatus.Contains("L"));
                        var registrosAcondicionando = detalleOrdenado.Where(x => x.estatus.Contains("A"));


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
                                    row2.RelativeItem().Padding(10).PaddingLeft(10).Text("INVENTARIO DE PRODUCTO ALIADO").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
                                    //+obtenernombre_mes(periodo) + " " + ejercicio
                                });
                            });


                        });

                        page.Content().PaddingTop(10).PaddingLeft(30).PaddingRight(30).Column(col1 =>
                        {

                        //col1.Item().LineHorizontal(0.5f);

                        System.DateTime fecha = System.DateTime.Now;
                        string fechaActual = fecha.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("es-ES"));
                            if (registrosAspersoras.Any())
                            {

                                col1.Item().Row(row =>
                                {
                                    row.RelativeItem().PaddingTop(-30).AlignCenter()
                                         .Text("ASPERSORAS")
                                        .FontSize(12).Bold().FontFamily(fontFamily);

                                });

                                col1.Item().PaddingTop(20).PaddingBottom(10).Border(0.5f).BorderColor("#477c2c").Table(tabla =>
                                {
                                    tabla.ColumnsDefinition(Columns =>
                                    {
                                        Columns.RelativeColumn(0.4f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.7f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(1.3f);
                                        Columns.RelativeColumn(0.9f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.9f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.6f);
                                        Columns.RelativeColumn(0.9f);
                                        Columns.RelativeColumn(1.1f);
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
                                        .Padding(1).Text("ANTIGUEDAD").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("SERIE").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("RECEPCIÓN").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("COSTO").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
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

                                    foreach (var det in registrosAspersoras)
                                    {
                                        string backgroundColor = (index % 2 == 0) ? "#FFFFFF" : "#f0f0f0";

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().Height(20).Padding(0)
                                        .SkiaSharpCanvas((canvas, size) =>
                                        {
                                            SKColor color;
                                            int circleRadius = 3;
                                            switch (det.estatus)
                                            {
                                                case "L":
                                                    color = SKColors.Green;
                                                    break;
                                                case "A":
                                                    color = SKColors.Yellow;
                                                    break;
                                                default:
                                                    color = SKColors.Transparent;
                                                    break;
                                            }

                                            using var paint = new SKPaint
                                            {
                                                Color = color,
                                                Style = SKPaintStyle.Fill
                                            };

                                            canvas.DrawCircle(size.Width / 2, size.Height / 2, circleRadius, paint);
                                        });

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingLeft(4).PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.nombre_sucursal?.ToUpper()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.NE).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.Marca).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.modelo_descripcion).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.antiguedad.ToString() + (det.antiguedad == 1 ? " MES" : " MESES")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.serie?.ToUpper()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.fecha_recepcion).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.Costo.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.utilidad.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.margen.ToString("N1")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.precio_lista.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                       .Text(!string.IsNullOrEmpty(det.promocion) && !string.IsNullOrEmpty(det.vigencia)
                                        ? det.promocion.ToUpper() + " Vigencia al " + det.vigencia
                                        : !string.IsNullOrEmpty(det.promocion)
                                            ? det.promocion.ToUpper()
                                            : !string.IsNullOrEmpty(det.vigencia)
                                                ? "Vigencia al " + det.vigencia
                                                : "").FontSize(7).FontFamily(fontFamily);

                                        index++;
                                    }

                                    tabla.Cell().ColumnSpan(8).Background("#DAE6BE").BorderColor("#afb69d").AlignRight().AlignMiddle().Padding(3)
                                         .Text("TOTAL:").FontSize(7).Bold().FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignRight().AlignMiddle().Padding(3)
                                        .Text(registrosAspersoras.Sum(r => r.Costo).ToString("N1")).FontSize(7).Bold().FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignRight().AlignMiddle().Padding(3)
                                        .Text(registrosAspersoras.Sum(r => r.utilidad).ToString("N1")).FontSize(7).Bold().FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignCenter().AlignMiddle().Padding(3)
                                        .Text(((registrosAspersoras.Sum(r => r.utilidad)) / (registrosAspersoras.Sum(r => r.precio_lista)) * 100).ToString("N2"))
                                        .FontSize(7).Bold().FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignRight().AlignMiddle().Padding(3)
                                        .Text(registrosAspersoras.Sum(r => r.precio_lista).ToString("N1")).FontSize(7).Bold().FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignCenter().AlignMiddle().Padding(3)
                                        .Text("").FontSize(7).FontFamily(fontFamily);
                                });
                            }
                            //if (registrosAspersoras.Any() && registrosRemolques.Any())
                            //{
                            //    col1.Item().PageBreak();
                            //}
                            if (registrosRemolques.Any())
                            {
                                col1.Item().Row(row =>
                                {
                                    row.RelativeItem().AlignCenter()
                                        .Text("REMOLQUES")
                                        .FontSize(12).Bold().FontFamily(fontFamily);

                                });

                                col1.Item().PaddingTop(10).PaddingBottom(10).Border(0.5f).BorderColor("#477c2c").Table(tabla =>
                                {
                                    tabla.ColumnsDefinition(Columns =>
                                    {
                                        Columns.RelativeColumn(0.4f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.7f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(1.3f);
                                        Columns.RelativeColumn(0.9f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.9f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.6f);
                                        Columns.RelativeColumn(0.9f);
                                        Columns.RelativeColumn(1.1f);
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
                                        .Padding(1).Text("ANTIGUEDAD").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("SERIE").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("RECEPCIÓN").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("COSTO").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
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
                                    // Ahora iteramos sobre los productos de esa promoción
                                    foreach (var det in registrosRemolques)
                                    {

                                        string backgroundColor = (index % 2 == 0) ? "#FFFFFF" : "#f0f0f0";

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().Height(20).Padding(0)
                                        .SkiaSharpCanvas((canvas, size) =>
                                        {
                                            SKColor color;
                                            int circleRadius = 3;
                                            switch (det.estatus)
                                            {
                                                case "L":
                                                    color = SKColors.Green;
                                                    break;
                                                case "A":
                                                    color = SKColors.Yellow;
                                                    break;
                                                default:
                                                    color = SKColors.Transparent;
                                                    break;
                                            }

                                            using var paint = new SKPaint
                                            {
                                                Color = color,
                                                Style = SKPaintStyle.Fill
                                            };

                                            canvas.DrawCircle(size.Width / 2, size.Height / 2, circleRadius, paint);
                                        });

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingLeft(4).PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.nombre_sucursal?.ToUpper()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.NE).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.Marca).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.modelo_descripcion).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.antiguedad.ToString() + (det.antiguedad == 1 ? " MES" : " MESES")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.serie?.ToUpper()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.fecha_recepcion).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.Costo.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.utilidad.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.margen.ToString("N1")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.precio_lista.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                      .Text(!string.IsNullOrEmpty(det.promocion) && !string.IsNullOrEmpty(det.vigencia)
                                       ? det.promocion.ToUpper() + " Vigencia al " + det.vigencia
                                       : !string.IsNullOrEmpty(det.promocion)
                                           ? det.promocion.ToUpper()
                                           : !string.IsNullOrEmpty(det.vigencia)
                                               ? "Vigencia al " + det.vigencia
                                               : "").FontSize(7).FontFamily(fontFamily);


                                        index++;
                                    }
                                    tabla.Cell().ColumnSpan(8).Background("#DAE6BE").BorderColor("#afb69d").AlignRight().AlignMiddle().Padding(3)
                                        .Text("TOTAL:").FontSize(7).Bold().FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignRight().AlignMiddle().Padding(3)
                                        .Text(registrosRemolques.Sum(r => r.Costo).ToString("N1")).FontSize(7).Bold().FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignRight().AlignMiddle().Padding(3)
                                        .Text(registrosRemolques.Sum(r => r.utilidad).ToString("N1")).FontSize(7).Bold().FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignCenter().AlignMiddle().Padding(3)
                                        .Text(((registrosRemolques.Sum(r => r.utilidad)) / (registrosRemolques.Sum(r => r.precio_lista)) * 100).ToString("N2"))
                                        .FontSize(7).Bold().FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignRight().AlignMiddle().Padding(3)
                                        .Text(registrosRemolques.Sum(r => r.precio_lista).ToString("N1")).FontSize(7).Bold().FontFamily(fontFamily);

                                    // Promoción/Vigencia: vacío en total
                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignCenter().AlignMiddle().Padding(3)
                                        .Text("").FontSize(7).FontFamily(fontFamily);
                                });
                                }

                            //if (registrosAspersoras.Any() && registrosMolinos.Any() || registrosRemolques.Any() && registrosMolinos.Any())
                            //{
                            //    col1.Item().PageBreak();
                            //}

                            if (registrosMolinos.Any())
                            {
                                col1.Item().Row(row =>
                                {
                                    row.RelativeItem().AlignCenter()
                                        .Text("MOLINOS")
                                        .FontSize(12).Bold().FontFamily(fontFamily);

                                });

                                col1.Item().PaddingTop(10).PaddingBottom(10).Border(0.5f).BorderColor("#477c2c").Table(tabla =>
                                {
                                    tabla.ColumnsDefinition(Columns =>
                                    {
                                        Columns.RelativeColumn(0.4f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.7f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(1.3f);
                                        Columns.RelativeColumn(0.9f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.9f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.6f);
                                        Columns.RelativeColumn(0.9f);
                                        Columns.RelativeColumn(1.1f);
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
                                        .Padding(1).Text("ANTIGUEDAD").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("SERIE").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("RECEPCIÓN").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("COSTO").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
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
                                    // Ahora iteramos sobre los productos de esa promoción
                                    foreach (var det in registrosMolinos)
                                    {

                                        string backgroundColor = (index % 2 == 0) ? "#FFFFFF" : "#f0f0f0";

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().Height(20).Padding(0)
                                        .SkiaSharpCanvas((canvas, size) =>
                                        {
                                            SKColor color;
                                            int circleRadius = 3;
                                            switch (det.estatus)
                                            {
                                                case "L":
                                                    color = SKColors.Green;
                                                    break;
                                                case "A":
                                                    color = SKColors.Yellow;
                                                    break;
                                                default:
                                                    color = SKColors.Transparent;
                                                    break;
                                            }

                                            using var paint = new SKPaint
                                            {
                                                Color = color,
                                                Style = SKPaintStyle.Fill
                                            };

                                            canvas.DrawCircle(size.Width / 2, size.Height / 2, circleRadius, paint);
                                        });

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingLeft(4).PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.nombre_sucursal?.ToUpper()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.NE).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.Marca).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.modelo_descripcion).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.antiguedad.ToString() + (det.antiguedad == 1 ? " MES" : " MESES")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.serie?.ToUpper()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.fecha_recepcion).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.Costo.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.utilidad.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.margen.ToString("N1")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.precio_lista.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                      .Text(!string.IsNullOrEmpty(det.promocion) && !string.IsNullOrEmpty(det.vigencia)
                                       ? det.promocion.ToUpper() + " Vigencia al " + det.vigencia
                                       : !string.IsNullOrEmpty(det.promocion)
                                           ? det.promocion.ToUpper()
                                           : !string.IsNullOrEmpty(det.vigencia)
                                               ? "Vigencia al " + det.vigencia
                                               : "").FontSize(7).FontFamily(fontFamily);


                                        index++;
                                    }

                                    tabla.Cell().ColumnSpan(8).Background("#DAE6BE").BorderColor("#afb69d").AlignRight().AlignMiddle().Padding(3)
                                        .Text("TOTAL:").FontSize(7).Bold().FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignRight().AlignMiddle().Padding(3)
                                        .Text(registrosMolinos.Sum(r => r.Costo).ToString("N1")).FontSize(7).Bold().FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignRight().AlignMiddle().Padding(3)
                                        .Text(registrosMolinos.Sum(r => r.utilidad).ToString("N1")).FontSize(7).Bold().FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignCenter().AlignMiddle().Padding(3)
                                        .Text(((registrosMolinos.Sum(r => r.utilidad)) / (registrosMolinos.Sum(r => r.precio_lista)) * 100).ToString("N2"))
                                        .FontSize(7).Bold().FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignRight().AlignMiddle().Padding(3)
                                        .Text(registrosMolinos.Sum(r => r.precio_lista).ToString("N1")).FontSize(7).Bold().FontFamily(fontFamily);

                                    // Promoción/Vigencia: vacío en total
                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignCenter().AlignMiddle().Padding(3)
                                        .Text("").FontSize(7).FontFamily(fontFamily);
                                });
                            }

                            //if (registrosAspersoras.Any() && registrosEquiposFertilizacion.Any() || registrosRemolques.Any() && registrosEquiposFertilizacion.Any() || registrosMolinos.Any() && registrosEquiposFertilizacion.Any())
                            //{
                            //    col1.Item().PageBreak();
                            //}

                            if (registrosEquiposFertilizacion.Any())
                            {
                                col1.Item().Row(row =>
                                {
                                    row.RelativeItem().AlignCenter()
                                        .Text("EQUIPOS DE FERTILIZACION")
                                        .FontSize(12).Bold().FontFamily(fontFamily);

                                });

                                col1.Item().PaddingTop(10).PaddingBottom(10).Border(0.5f).BorderColor("#477c2c").Table(tabla =>
                                {
                                    tabla.ColumnsDefinition(Columns =>
                                    {
                                        Columns.RelativeColumn(0.4f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.7f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(1.3f);
                                        Columns.RelativeColumn(0.9f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.9f);
                                        Columns.RelativeColumn(0.8f);
                                        Columns.RelativeColumn(0.6f);
                                        Columns.RelativeColumn(0.9f);
                                        Columns.RelativeColumn(1.1f);
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
                                        .Padding(1).Text("ANTIGUEDAD").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("SERIE").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("RECEPCIÓN").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().BorderBottom(0.5f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("COSTO").FontSize(7).Bold().FontFamily(fontFamily).FontColor("#fff");
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
                                    // Ahora iteramos sobre los productos de esa promoción
                                    foreach (var det in registrosEquiposFertilizacion)
                                    {

                                        string backgroundColor = (index % 2 == 0) ? "#FFFFFF" : "#f0f0f0";

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().Height(20).Padding(0)
                                        .SkiaSharpCanvas((canvas, size) =>
                                        {
                                            SKColor color;
                                            int circleRadius = 3;
                                            switch (det.estatus)
                                            {
                                                case "L":
                                                    color = SKColors.Green;
                                                    break;
                                                case "A":
                                                    color = SKColors.Yellow;
                                                    break;
                                                default:
                                                    color = SKColors.Transparent;
                                                    break;
                                            }

                                            using var paint = new SKPaint
                                            {
                                                Color = color,
                                                Style = SKPaintStyle.Fill
                                            };

                                            canvas.DrawCircle(size.Width / 2, size.Height / 2, circleRadius, paint);
                                        });

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingLeft(4).PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.nombre_sucursal?.ToUpper()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.NE).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.Marca).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.modelo_descripcion).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.antiguedad.ToString() + (det.antiguedad == 1 ? " MES" : " MESES")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignLeft().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.serie?.ToUpper()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.fecha_recepcion).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.Costo.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignRight().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.utilidad.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.margen.ToString("N1")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingLeft(3).PaddingVertical(3).ShowEntire()
                                        .Text(det.precio_lista.ToString("N2")).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background(backgroundColor).BorderColor("#afb69d").AlignCenter().AlignMiddle().PaddingRight(3).PaddingVertical(3).ShowEntire()
                                      .Text(!string.IsNullOrEmpty(det.promocion) && !string.IsNullOrEmpty(det.vigencia)
                                       ? det.promocion.ToUpper() + " Vigencia al " + det.vigencia
                                       : !string.IsNullOrEmpty(det.promocion)
                                           ? det.promocion.ToUpper()
                                           : !string.IsNullOrEmpty(det.vigencia)
                                               ? "Vigencia al " + det.vigencia
                                               : "").FontSize(7).FontFamily(fontFamily);


                                        index++;
                                    }

                                    tabla.Cell().ColumnSpan(8).Background("#DAE6BE").BorderColor("#afb69d").AlignRight().AlignMiddle().Padding(3)
                                        .Text("TOTAL:").FontSize(7).Bold().FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignRight().AlignMiddle().Padding(3)
                                        .Text(registrosEquiposFertilizacion.Sum(r => r.Costo).ToString("N1")).FontSize(7).Bold().FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignRight().AlignMiddle().Padding(3)
                                        .Text(registrosEquiposFertilizacion.Sum(r => r.utilidad).ToString("N1")).FontSize(7).Bold().FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignCenter().AlignMiddle().Padding(3)
                                        .Text(((registrosEquiposFertilizacion.Sum(r => r.utilidad)) / (registrosEquiposFertilizacion.Sum(r => r.precio_lista)) * 100).ToString("N2"))
                                        .FontSize(7).Bold().FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignRight().AlignMiddle().Padding(3)
                                        .Text(registrosEquiposFertilizacion.Sum(r => r.precio_lista).ToString("N1")).FontSize(7).Bold().FontFamily(fontFamily);

                                    //tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignCenter().AlignMiddle().Padding(3)
                                    //    .Text("").FontSize(7).FontFamily(fontFamily);

                                    // TOTAL DE TODA LA TABLA
                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignCenter().AlignMiddle().Padding(3)
                                        .Text("").FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().ColumnSpan(8).Background("#DAE6BE").BorderColor("#afb69d").AlignRight().AlignMiddle().Padding(3)
                                         .Text("TOTALES:").FontSize(7).Bold().FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignRight().AlignMiddle().Padding(3)
                                        .Text(detalleOrdenado.Sum(r => r.Costo).ToString("N0")).Bold().FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignRight().AlignMiddle().Padding(3)
                                        .Text(detalleOrdenado.Sum(r => r.utilidad).ToString("N0")).FontSize(7).Bold().FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignCenter().AlignMiddle().Padding(3)
                                        .Text(((detalleOrdenado.Sum(r => r.utilidad)) / (detalleOrdenado.Sum(r => r.precio_lista)) * 100).ToString("N2"))
                                        .FontSize(7).Bold().FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignRight().AlignMiddle().Padding(3)
                                        .Text(detalleOrdenado.Sum(r => r.precio_lista).ToString("N0")).FontSize(7).Bold().FontFamily(fontFamily);

                                    // Promoción/Vigencia: vacío en total
                                    tabla.Cell().Background("#DAE6BE").BorderColor("#afb69d").AlignCenter().AlignMiddle().Padding(3)
                                        .Text("").FontSize(7).FontFamily(fontFamily);
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
