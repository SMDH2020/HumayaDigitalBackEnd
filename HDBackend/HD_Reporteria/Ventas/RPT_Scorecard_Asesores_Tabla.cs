using QuestPDF.Fluent;
using QuestPDF.Helpers;
using HD_Ventas.Modelos;
using HD_Reporteria.Cobranza;

namespace HD_Reporteria.Ventas
{
    public class RPT_Scorecard_Asesores_Tabla
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
        public static RPT_Result GenerarPDF(IEnumerable<mdlCarga_Scorecard_porVendedor_Dash_TablaAsesor> scorecard, int ejercicio, int mes_actual, int ejercicio_inicio, int periodo_inicio)
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
                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text("SCORECARD").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
                                });
                            });


                        });

                        page.Content().PaddingTop(10).PaddingLeft(20).PaddingRight(20).Column(col1 =>
                        {

                            //col1.Item().LineHorizontal(0.5f);

                            col1.Item().Row(row =>
                            {
                                row.RelativeItem().AlignCenter().Text(txt =>
                                {
                                    txt.Span("SCORECARD GENERAL").FontSize(12).Bold();
                                });
                            });

                            DateTime fecha = DateTime.Now;
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
                                    Columns.RelativeColumn(1.6f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f);
                                    Columns.RelativeColumn(0.46f); // Garantias Objetivo
                                    Columns.RelativeColumn(0.46f); // Garantias Real
                                    Columns.RelativeColumn(0.46f); // Garantias Alcance
                                    Columns.RelativeColumn(0.46f); // Polizas Objetivo
                                    Columns.RelativeColumn(0.46f); // Polizas Real
                                    Columns.RelativeColumn(0.46f); // Polizas Alcance
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(3).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("TRACTORES").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(3).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("IMPLEMENTOS").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(3).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                   .Padding(1).Text("JARDINEROS").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(3).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("AUTOGUIADOS").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(3).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                   .Padding(1).Text("DRONES").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(3).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("P. ALIADO").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(3).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                   .Padding(1).Text("TRACTORES S.").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(3).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("TRILLADORAS S.").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(3).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                   .Padding(1).Text("GARANTIAS").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    header.Cell().ColumnSpan(3).BorderLeft(0.6f).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("POLIZAS").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                });

                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().Height(20).AlignMiddle()
                                    .Padding(1).Text("ASESOR").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("OBJETIVO").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("REAL").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ALCANCE").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("OBJETIVO").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("REAL").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ALCANCE").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("OBJETIVO").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("REAL").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ALCANCE").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("OBJETIVO").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("REAL").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ALCANCE").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("OBJETIVO").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("REAL").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ALCANCE").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("OBJETIVO").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("REAL").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ALCANCE").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("OBJETIVO").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("REAL").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ALCANCE").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("OBJETIVO").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("REAL").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ALCANCE").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("OBJETIVO").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("REAL").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ALCANCE").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("OBJETIVO").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("REAL").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");
                                tabla.Cell().BorderHorizontal(1).BorderColor("#fedb05").Background("#477c2c").AlignCenter().AlignMiddle()
                                .Padding(1).Text("ALCANCE").FontSize(6).Bold().FontFamily(fontFamily).FontColor("#fff");

                                double porcentaje = 0;
                                double porcentajeacumulado = 0;

                                var groupedByAdr = scorecard.GroupBy(x => x.adr);

                                foreach (var adrGroup in groupedByAdr)
                                {

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignLeft().Height(20).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                    .Text(adrGroup.Key).FontSize(7).FontFamily(fontFamily);

                                    var totalObjetivoAdrAutoguiados = adrGroup.Sum(x => x.Objetivo_Autoguiados);
                                    var totalRealAdrAutoguiados = adrGroup.Sum(x => x.Real_Autoguiados);
                                    float totalPorcentajeAdrAutoguiados = totalObjetivoAdrAutoguiados > 0
                                        ? ((float)totalRealAdrAutoguiados / totalObjetivoAdrAutoguiados) * 100
                                        : (totalRealAdrAutoguiados > 0 ? 100 : 0);

                                    var totalObjetivoAdrDrones = adrGroup.Sum(x => x.Objetivo_Drones);
                                    var totalRealAdrDrones = adrGroup.Sum(x => x.Real_Drones);
                                    float totalPorcentajeAdrDrones = totalObjetivoAdrDrones > 0
                                        ? ((float)totalRealAdrDrones / totalObjetivoAdrDrones) * 100
                                        : (totalRealAdrDrones > 0 ? 100 : 0);

                                    var totalObjetivoAdrImplementos = adrGroup.Sum(x => x.Objetivo_Implementos);
                                    var totalRealAdrImplementos = adrGroup.Sum(x => x.Real_Implementos);
                                    float totalPorcentajeAdrImplementos = totalObjetivoAdrImplementos > 0
                                        ? ((float)totalRealAdrImplementos / totalObjetivoAdrImplementos) * 100
                                        : (totalRealAdrImplementos > 0 ? 100 : 0);

                                    var totalObjetivoAdrJardineros = adrGroup.Sum(x => x.Objetivo_Jardineros);
                                    var totalRealAdrJardineros = adrGroup.Sum(x => x.Real_Jardineros);
                                    float totalPorcentajeAdrJardineros = totalObjetivoAdrJardineros > 0
                                        ? ((float)totalRealAdrJardineros / totalObjetivoAdrJardineros) * 100
                                        : (totalRealAdrJardineros > 0 ? 100 : 0);

                                    var totalObjetivoAdrPA = adrGroup.Sum(x => x.Objetivo_PA);
                                    var totalRealAdrPA = adrGroup.Sum(x => x.Real_PA);
                                    float totalPorcentajeAdrPA = totalObjetivoAdrPA > 0
                                        ? ((float)totalRealAdrPA / totalObjetivoAdrPA) * 100
                                        : (totalRealAdrPA > 0 ? 100 : 0);

                                    int totalObjetivoAdrTractores = adrGroup.Sum(x => x.Objetivo_Tractores);
                                    int totalRealAdrTractores = adrGroup.Sum(x => x.Real_Tractores);
                                    float totalPorcentajeAdrTractores = totalObjetivoAdrTractores > 0
                                        ? ((float)totalRealAdrTractores / totalObjetivoAdrTractores) * 100
                                        : (totalRealAdrTractores > totalObjetivoAdrTractores ? 100 : 0);

                                    var totalObjetivoAdrTracUsa = adrGroup.Sum(x => x.Objetivo_TracUsa);
                                    var totalRealAdrTracUsa = adrGroup.Sum(x => x.Real_TracUsa);
                                    float totalPorcentajeAdrTracUsa = totalObjetivoAdrTracUsa > 0
                                        ? ((float)totalRealAdrTracUsa / totalObjetivoAdrTracUsa) * 100
                                        : (totalRealAdrTracUsa > 0 ? 100 : 0);

                                    var totalObjetivoAdrTriUsa = adrGroup.Sum(x => x.Objetivo_TriUsa);
                                    var totalRealAdrTriUsa = adrGroup.Sum(x => x.Real_TriUsa);
                                    float totalPorcentajeAdrTriUsa = totalObjetivoAdrTriUsa > 0
                                        ? ((float)totalRealAdrTriUsa / totalObjetivoAdrTriUsa) * 100
                                        : (totalRealAdrTriUsa > 0 ? 100 : 0);

                                    var totalObjetivoAdrGarantias = adrGroup.Sum(x => x.Objetivo_Garantia);
                                    var totalRealAdrGarantias = adrGroup.Sum(x => x.Real_Garantia);
                                    float totalPorcentajeAdrGarantias = totalObjetivoAdrGarantias > 0
                                        ? ((float)totalRealAdrGarantias / totalObjetivoAdrGarantias) * 100
                                        : (totalRealAdrGarantias > 0 ? 100 : 0);

                                    var totalObjetivoAdrPolizas = adrGroup.Sum(x => x.Objetivo_Poliza);
                                    var totalRealAdrPolizas = adrGroup.Sum(x => x.Real_Poliza);
                                    float totalPorcentajeAdrPolizas = totalObjetivoAdrPolizas > 0
                                        ? ((float)totalRealAdrPolizas / totalObjetivoAdrPolizas) * 100
                                        : (totalRealAdrPolizas > 0 ? 100 : 0);


                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                       .Text(totalObjetivoAdrTractores.ToString()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                      .Text(totalRealAdrTractores.ToString()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                       .Text(Math.Round(totalPorcentajeAdrTractores, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                           .Text(totalObjetivoAdrImplementos.ToString()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                      .Text(totalRealAdrImplementos.ToString()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                       .Text(Math.Round(totalPorcentajeAdrImplementos, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                           .Text(totalObjetivoAdrJardineros.ToString()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                      .Text(totalRealAdrJardineros.ToString()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                       .Text(Math.Round(totalPorcentajeAdrJardineros, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                       .Text(totalObjetivoAdrAutoguiados.ToString()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                      .Text(totalRealAdrAutoguiados.ToString()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                       .Text(Math.Round(totalPorcentajeAdrAutoguiados, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                               .Text(totalObjetivoAdrDrones.ToString()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                      .Text(totalRealAdrDrones.ToString()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                       .Text(Math.Round(totalPorcentajeAdrDrones, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                               .Text(totalObjetivoAdrPA.ToString()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                      .Text(totalRealAdrPA.ToString()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                       .Text(Math.Round(totalPorcentajeAdrPA, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                               .Text(totalObjetivoAdrTracUsa.ToString()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                      .Text(totalRealAdrTracUsa.ToString()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                       .Text(Math.Round(totalPorcentajeAdrTracUsa, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                               .Text(totalObjetivoAdrTriUsa.ToString()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                      .Text(totalRealAdrTriUsa.ToString()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                       .Text(Math.Round(totalPorcentajeAdrTriUsa, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                               .Text(totalObjetivoAdrGarantias.ToString()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                      .Text(totalRealAdrGarantias.ToString()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                       .Text(Math.Round(totalPorcentajeAdrGarantias, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                               .Text(totalObjetivoAdrPolizas.ToString()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                      .Text(totalRealAdrPolizas.ToString()).FontSize(7).FontFamily(fontFamily);

                                    tabla.Cell().Background("#DAE6BE").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                       .Text(Math.Round(totalPorcentajeAdrPolizas, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);


                                    var groupedBySucursal = adrGroup.GroupBy(x => x.sucursal);

                                    foreach (var sucursalGroup in groupedBySucursal)
                                    {
                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignLeft().Height(20).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                        .Text(sucursalGroup.Key).FontSize(7).FontFamily(fontFamily);

                                        var totalObjetivosucursalAutoguiados = sucursalGroup.Sum(x => x.Objetivo_Autoguiados);
                                        var totalRealsucursalAutoguiados = sucursalGroup.Sum(x => x.Real_Autoguiados);
                                        float totalPorcentajesucursalAutoguiados = totalObjetivosucursalAutoguiados > 0
                                            ? ((float)totalRealsucursalAutoguiados / totalObjetivosucursalAutoguiados) * 100
                                            : (totalRealsucursalAutoguiados > 0 ? 100 : 0);

                                        var totalObjetivosucursalDrones = sucursalGroup.Sum(x => x.Objetivo_Drones);
                                        var totalRealsucursalDrones = sucursalGroup.Sum(x => x.Real_Drones);
                                        float totalPorcentajesucursalDrones = totalObjetivosucursalDrones > 0
                                            ? ((float)totalRealsucursalDrones / totalObjetivosucursalDrones) * 100
                                            : (totalRealsucursalDrones > 0 ? 100 : 0);

                                        var totalObjetivosucursalImplementos = sucursalGroup.Sum(x => x.Objetivo_Implementos);
                                        var totalRealsucursalImplementos = sucursalGroup.Sum(x => x.Real_Implementos);
                                        float totalPorcentajesucursalImplementos = totalObjetivosucursalImplementos > 0
                                            ? ((float)totalRealsucursalImplementos / totalObjetivosucursalImplementos) * 100
                                            : (totalRealsucursalImplementos > 0 ? 100 : 0);

                                        var totalObjetivosucursalJardineros = sucursalGroup.Sum(x => x.Objetivo_Jardineros);
                                        var totalRealsucursalJardineros = sucursalGroup.Sum(x => x.Real_Jardineros);
                                        float totalPorcentajesucursalJardineros = totalObjetivosucursalJardineros > 0
                                            ? ((float)totalRealsucursalJardineros / totalObjetivosucursalJardineros) * 100
                                            : (totalRealsucursalJardineros > 0 ? 100 : 0);

                                        var totalObjetivosucursalPA = sucursalGroup.Sum(x => x.Objetivo_PA);
                                        var totalRealsucursalPA = sucursalGroup.Sum(x => x.Real_PA);
                                        float totalPorcentajesucursalPA = totalObjetivosucursalPA > 0
                                            ? ((float)totalRealsucursalPA / totalObjetivosucursalPA) * 100
                                            : (totalRealsucursalPA > 0 ? 100 : 0);

                                        int totalObjetivosucursalTractores = sucursalGroup.Sum(x => x.Objetivo_Tractores);
                                        int totalRealsucursalTractores = sucursalGroup.Sum(x => x.Real_Tractores);
                                        float totalPorcentajesucursalTractores = totalObjetivosucursalTractores > 0
                                            ? ((float)totalRealsucursalTractores / totalObjetivosucursalTractores) * 100
                                            : (totalRealsucursalTractores > totalObjetivosucursalTractores ? 100 : 0);

                                        var totalObjetivosucursalTracUsa = sucursalGroup.Sum(x => x.Objetivo_TracUsa);
                                        var totalRealsucursalTracUsa = sucursalGroup.Sum(x => x.Real_TracUsa);
                                        float totalPorcentajesucursalTracUsa = totalObjetivosucursalTracUsa > 0
                                            ? ((float)totalRealsucursalTracUsa / totalObjetivosucursalTracUsa) * 100
                                            : (totalRealsucursalTracUsa > 0 ? 100 : 0);

                                        var totalObjetivosucursalTriUsa = sucursalGroup.Sum(x => x.Objetivo_TriUsa);
                                        var totalRealsucursalTriUsa = sucursalGroup.Sum(x => x.Real_TriUsa);
                                        float totalPorcentajesucursalTriUsa = totalObjetivosucursalTriUsa > 0
                                            ? ((float)totalRealsucursalTriUsa / totalObjetivosucursalTriUsa) * 100
                                            : (totalRealsucursalTriUsa > 0 ? 100 : 0);

                                        var totalObjetivosucursalGarantias = sucursalGroup.Sum(x => x.Objetivo_Garantia);
                                        var totalRealsucursalGarantias = sucursalGroup.Sum(x => x.Real_Garantia);
                                        float totalPorcentajesucursalGarantias = totalObjetivosucursalGarantias > 0
                                            ? ((float)totalRealsucursalGarantias / totalObjetivosucursalGarantias) * 100
                                            : (totalRealsucursalGarantias > 0 ? 100 : 0);

                                        var totalObjetivosucursalPolizas = sucursalGroup.Sum(x => x.Objetivo_Poliza);
                                        var totalRealsucursalPolizas = sucursalGroup.Sum(x => x.Real_Poliza);
                                        float totalPorcentajesucursalPolizas = totalObjetivosucursalPolizas > 0
                                            ? ((float)totalRealsucursalPolizas / totalObjetivosucursalPolizas) * 100
                                            : (totalRealsucursalPolizas > 0 ? 100 : 0);


                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                           .Text(totalObjetivosucursalTractores.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                          .Text(totalRealsucursalTractores.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                           .Text(Math.Round(totalPorcentajesucursalTractores, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                               .Text(totalObjetivosucursalImplementos.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                          .Text(totalRealsucursalImplementos.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                           .Text(Math.Round(totalPorcentajesucursalImplementos, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                               .Text(totalObjetivosucursalJardineros.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                          .Text(totalRealsucursalJardineros.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                           .Text(Math.Round(totalPorcentajesucursalJardineros, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                           .Text(totalObjetivosucursalAutoguiados.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                          .Text(totalRealsucursalAutoguiados.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                           .Text(Math.Round(totalPorcentajesucursalAutoguiados, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                                   .Text(totalObjetivosucursalDrones.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                          .Text(totalRealsucursalDrones.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                           .Text(Math.Round(totalPorcentajesucursalDrones, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                                   .Text(totalObjetivosucursalPA.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                          .Text(totalRealsucursalPA.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                           .Text(Math.Round(totalPorcentajesucursalPA, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                                   .Text(totalObjetivosucursalTracUsa.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                          .Text(totalRealsucursalTracUsa.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                           .Text(Math.Round(totalPorcentajesucursalTracUsa, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                                   .Text(totalObjetivosucursalTriUsa.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                          .Text(totalRealsucursalTriUsa.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                           .Text(Math.Round(totalPorcentajesucursalTriUsa, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                                   .Text(totalObjetivosucursalGarantias.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                          .Text(totalRealsucursalGarantias.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                           .Text(Math.Round(totalPorcentajesucursalGarantias, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                                   .Text(totalObjetivosucursalPolizas.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                          .Text(totalRealsucursalPolizas.ToString()).FontSize(7).FontFamily(fontFamily);

                                        tabla.Cell().Background("#e3e3e3").BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                           .Text(Math.Round(totalPorcentajesucursalPolizas, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                        foreach (var sco in sucursalGroup)
                                        {
                                            float totalPorcentajeAsesorAutoguiados = sco.Objetivo_Autoguiados > 0
                                                ? ((float)sco.Real_Autoguiados / sco.Objetivo_Autoguiados) * 100
                                                : (sco.Real_Autoguiados > 0 ? 100 : 0);

                                            float totalPorcentajeAsesorDrones = sco.Objetivo_Drones > 0
                                                ? ((float)sco.Real_Drones / sco.Objetivo_Drones) * 100
                                                : (sco.Real_Drones > 0 ? 100 : 0);

                                            float totalPorcentajeAsesorImplementos = sco.Objetivo_Implementos > 0
                                                ? ((float)sco.Real_Implementos / sco.Objetivo_Implementos) * 100
                                                : (sco.Real_Implementos > 0 ? 100 : 0);

                                            float totalPorcentajeAsesorJardineros = sco.Objetivo_Jardineros > 0
                                                ? ((float)sco.Real_Jardineros / sco.Objetivo_Jardineros) * 100
                                                : (sco.Real_Jardineros > 0 ? 100 : 0);

                                            float totalPorcentajeAsesorPA = sco.Objetivo_PA > 0
                                                ? ((float)sco.Real_PA / sco.Objetivo_PA) * 100
                                                : (sco.Real_PA > 0 ? 100 : 0);

                                            float totalPorcentajeAsesorTractores = sco.Objetivo_Tractores > 0
                                                ? ((float)sco.Real_Tractores / sco.Objetivo_Tractores) * 100
                                                : (sco.Real_Tractores > 0 ? 100 : 0);

                                            float totalPorcentajeAsesorTracUsa = sco.Objetivo_TracUsa > 0
                                                ? ((float)sco.Real_TracUsa / sco.Objetivo_TracUsa) * 100
                                                : (sco.Real_TracUsa > 0 ? 100 : 0);

                                            float totalPorcentajeAsesorTriUsa = sco.Objetivo_TriUsa > 0
                                                ? ((float)sco.Real_TriUsa / sco.Objetivo_TriUsa) * 100
                                                : (sco.Real_TriUsa > 0 ? 100 : 0);

                                            float totalPorcentajeAsesorGarantias = sco.Objetivo_Garantia > 0
                                                ? ((float)sco.Real_Garantia / sco.Objetivo_Garantia) * 100
                                                : (sco.Real_Garantia > 0 ? 100 : 0);

                                            float totalPorcentajeAsesorPolizas = sco.Objetivo_Poliza > 0
                                                ? ((float)sco.Real_Poliza / sco.Objetivo_Poliza) * 100
                                                : (sco.Real_Poliza > 0 ? 100 : 0);



                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignLeft().Height(20).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                            .Text(sco.asesor).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                           .Text(sco.Objetivo_Tractores.ToString()).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                              .Text(sco.Real_Tractores.ToString()).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                               .Text(Math.Round(totalPorcentajeAsesorTractores, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                                   .Text(sco.Objetivo_Implementos.ToString()).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                              .Text(sco.Real_Implementos.ToString()).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                               .Text(Math.Round(totalPorcentajeAsesorImplementos, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                                   .Text(sco.Objetivo_Jardineros.ToString()).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                              .Text(sco.Real_Jardineros.ToString()).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                               .Text(Math.Round(totalPorcentajeAsesorJardineros, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                               .Text(sco.Objetivo_Autoguiados.ToString()).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                              .Text(sco.Real_Autoguiados.ToString()).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                               .Text(Math.Round(totalPorcentajeAsesorAutoguiados, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                                       .Text(sco.Objetivo_Drones.ToString()).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                              .Text(sco.Real_Drones.ToString()).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                               .Text(Math.Round(totalPorcentajeAsesorDrones, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                                       .Text(sco.Objetivo_PA.ToString()).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                              .Text(sco.Real_PA.ToString()).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                               .Text(Math.Round(totalPorcentajeAsesorPA, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                                       .Text(sco.Objetivo_TracUsa.ToString()).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                              .Text(sco.Real_TracUsa.ToString()).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                               .Text(Math.Round(totalPorcentajeAsesorTracUsa, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                                       .Text(sco.Objetivo_TriUsa.ToString()).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                              .Text(sco.Real_TriUsa.ToString()).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                               .Text(Math.Round(totalPorcentajeAsesorTriUsa, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                                       .Text(sco.Objetivo_Garantia.ToString()).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                              .Text(sco.Real_Garantia.ToString()).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                               .Text(Math.Round(totalPorcentajeAsesorGarantias, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                                       .Text(sco.Objetivo_Poliza.ToString()).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                              .Text(sco.Real_Poliza.ToString()).FontSize(7).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                               .Text(Math.Round(totalPorcentajeAsesorPolizas, 2).ToString() + " %").FontSize(7).FontFamily(fontFamily);
                                        }

                                    }
                                }



                                //foreach (var sco in scorecard)
                                //{

                                //    

                                //    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                //   .Text(sco.objetivo.ToString()).FontSize(10).FontFamily(fontFamily);

                                //    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                //  .Text(sco.unidades_vendidas.ToString()).FontSize(10).FontFamily(fontFamily);

                                //    if (sco.unidades_vendidas != 0)
                                //    {
                                //        porcentaje = (double)sco.unidades_vendidas / (double)sco.objetivo * 100;
                                //        if (porcentaje > 100)
                                //            porcentaje = 100;
                                //    }
                                //    else
                                //        porcentaje = 0;

                                //    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                //   .Text(Math.Round(porcentaje, 2).ToString() + " %").FontSize(10).FontFamily(fontFamily);

                                //    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                //   .Text(sco.objetivo_acumulado.ToString()).FontSize(10).FontFamily(fontFamily);

                                //    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                //   .Text(sco.unidades_vendidas_acumulado.ToString()).FontSize(10).FontFamily(fontFamily);

                                //    if (sco.unidades_vendidas_acumulado != 0)
                                //    {
                                //        porcentajeacumulado = (double)sco.unidades_vendidas_acumulado / (double)sco.objetivo_acumulado * 100;
                                //        if (porcentajeacumulado > 100)
                                //            porcentajeacumulado = 100;
                                //    }
                                //    else
                                //        porcentajeacumulado = 0;

                                //    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").AlignCenter().Height(20).AlignMiddle().PaddingRight(3)
                                //   .Text(Math.Round(porcentajeacumulado).ToString() + " %").FontSize(10).FontFamily(fontFamily);
                                //}

                                //float totalImporteProyectado = scorecard.Sum(sco => sco.importe_proyectado);
                                //float totalImporte = scorecard.Sum(sco => sco.importe);
                                //float totalImporteProyectadoAcumulado = scorecard.Sum(sco => sco.importe_proyectado_acumulado);
                                //float totalImporteAcumulado = scorecard.Sum(sco => sco.importe_acumulado);

                                //tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Background("#e5e6e6").AlignLeft().Height(20).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                //    .Text("IMPORTE TOTAL").Bold().FontSize(10).FontFamily(fontFamily);
                                //tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Background("#e5e6e6").AlignRight().Height(20).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                //    .Text(totalImporteProyectado.ToString("N2")).Bold().FontSize(10).FontFamily(fontFamily);
                                //tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Background("#e5e6e6").AlignRight().Height(20).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                //    .Text(totalImporte.ToString("N2")).Bold().FontSize(10).FontFamily(fontFamily);
                                //tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Background("#e5e6e6").AlignLeft().Height(20).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                //    .Text("").FontSize(10).FontFamily(fontFamily);
                                //tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Background("#e5e6e6").AlignRight().Height(20).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                //    .Text(totalImporteProyectadoAcumulado.ToString("N2")).Bold().FontSize(10).FontFamily(fontFamily);
                                //tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Background("#e5e6e6").AlignRight().Height(20).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                //    .Text(totalImporteAcumulado.ToString("N2")).Bold().FontSize(10).FontFamily(fontFamily);
                                //tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Background("#e5e6e6").AlignLeft().Height(20).AlignMiddle().PaddingLeft(4).PaddingRight(3)
                                //    .Text("").FontSize(10).FontFamily(fontFamily);
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