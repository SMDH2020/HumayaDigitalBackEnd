using Enlace.Dapper.Reportes;
using HD_Cobranza.Modelos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Reporteria.Finanzas
{
    public class RPT_Balanze_General_PDF
    {
        public static RPT_Result Generar(BalacenGeneralResult resumen, int periodo, int ejercicio)
        {
            try
            {
                string fontFamily = "Calibri";
                byte[] doc = Document.Create(document =>
                {
                    document.Page(page =>
                    {
                        page.Size(PageSizes.A4.Portrait());



                        page.Header().Height(70).Row(row =>
                        {

                            //row.ConstantItem(140).Border(1).Placeholder();
                            row.RelativeItem().PaddingTop(20).Height(30).Background("#477c2c").Row(row2 =>
                            {

                            });

                            row.ConstantColumn(0).Row(row1 =>
                            {
                                var rutaImagen = Path.Combine("C:\\Nube\\HumayaDigital\\HumayaDigitalBackEnd\\HDBackend\\HD_Reporteria\\Imagenes\\Logo.jpg");
                                byte[] imageData = System.IO.File.ReadAllBytes(rutaImagen);
                                row.ConstantItem(70).Image(imageData);

                                row.ConstantColumn(450).PaddingTop(20).Height(30).Background("#477c2c").Row(row2 =>
                                {
                                    row2.RelativeItem().Padding(5).PaddingTop(3).PaddingLeft(60).Text("BALANCE GENERAL").FontColor("#fff").FontSize(12).Bold().FontFamily(fontFamily);
                                });
                            });


                        });

                        page.Content()
                            .PaddingTop(10)
                            .PaddingLeft(30)
                            .PaddingRight(30)
                            .Column(col =>
                            {
                                col.Item().Height(10);

                                // =====================================================
                                // CONTENEDOR GENERAL
                                // =====================================================
                                col.Item()
                                    .Border(1)
                                    .BorderColor("#CCCCCC")
                                    .Column(container =>
                                    {

                                        var culturaEs = new CultureInfo("es-ES");

                                        // Fecha actual construida a partir de periodo y ejercicio
                                        DateTime fechaActual = new DateTime(ejercicio, periodo, 1);
                                        DateTime fechaAnterior = fechaActual.AddYears(-1);

                                        // Mostrar solo las tres iniciales del mes
                                        string tituloActual = fechaActual.ToString("MMM yyyy", culturaEs).ToUpper();
                                        string tituloAnterior = fechaAnterior.ToString("MMM yyyy", culturaEs).ToUpper();

                                        // =============================================
                                        // ENCABEZADO GLOBAL
                                        // =============================================
                                        container.Item().Table(table =>
                                        {
                                            table.ColumnsDefinition(columns =>
                                            {
                                                columns.RelativeColumn(4);
                                                columns.RelativeColumn(2);
                                                columns.RelativeColumn(2);
                                                columns.RelativeColumn(2);
                                                columns.RelativeColumn(2);
                                                columns.RelativeColumn(2);
                                                columns.RelativeColumn(2);
                                            });

                                            table.Cell().Background("#CCCCCC").Padding(5)
                                                .Text("CONCEPTO")
                                                .FontFamily(fontFamily).FontSize(9).Bold();

                                            table.Cell().Background("#CCCCCC").Padding(5).AlignRight()
                                                .Text(tituloActual)
                                                .FontFamily(fontFamily).FontSize(9).Bold();

                                            table.Cell().Background("#CCCCCC").Padding(5).AlignRight()
                                                .Text("%")
                                                .FontFamily(fontFamily).FontSize(9).Bold();

                                            table.Cell().Background("#CCCCCC").Padding(5).AlignRight()
                                                .Text(tituloAnterior)
                                                .FontFamily(fontFamily).FontSize(9).Bold();

                                            table.Cell().Background("#CCCCCC").Padding(5).AlignRight()
                                                .Text("%")
                                                .FontFamily(fontFamily).FontSize(9).Bold();

                                            table.Cell().Background("#CCCCCC").Padding(5).AlignRight()
                                                .Text("VARIACION")
                                                .FontFamily(fontFamily).FontSize(9).Bold();

                                            table.Cell().Background("#CCCCCC").Padding(5).AlignRight()
                                                .Text("%")
                                                .FontFamily(fontFamily).FontSize(9).Bold();
                                        });


                                        // =============================================
                                        // TOTALES NIVEL 1
                                        // =============================================
                                        decimal totalNivel1_total = 0;
                                        decimal totalNivel1_mrgtotal = 0;
                                        decimal totalNivel1_totalanterior = 0;
                                        decimal totalNivel1_mrgtotalanterior = 0;
                                        decimal totalNivel1_variacion = 0;
                                        decimal totalNivel1_mrgvariacion = 0;

                                        // =============================================
                                        // AGRUPACIÓN NIVEL 2
                                        // =============================================
                                        var groupedNivel2 = resumen.balance
                                            .GroupBy(x => x.nivel2)
                                            .OrderBy(x => x.Key);

                                        foreach (var nivel2 in groupedNivel2)
                                        {
                                            // =====================================================
                                            // 🔴 BLOQUE COMPLETO NIVEL 2 (CONTROL DE PAGINACIÓN)
                                            // =====================================================
                                            container.Item()
                                                .EnsureSpace(200)
                                                .Column(nivel2Container =>
                                                {
                                                    // ---------- NIVEL 2 ----------
                                                    nivel2Container.Item()
                                                        .Background("#285026")
                                                        .Padding(4)
                                                        .Text(nivel2.Key)
                                                        .FontFamily(fontFamily)
                                                        .FontSize(12)
                                                        .Bold()
                                                        .FontColor(Colors.White);

                                                    //nivel2Container.Item().Height(5);

                                                    var groupedNivel3 = nivel2
                                                        .GroupBy(x => x.nivel3)
                                                        .OrderBy(x => x.Key);

                                                    decimal totalNivel2_total = 0;
                                                    decimal totalNivel2_mrgtotal = 0;
                                                    decimal totalNivel2_totalanterior = 0;
                                                    decimal totalNivel2_mrgtotalanterior = 0;
                                                    decimal totalNivel2_variacion = 0;
                                                    decimal totalNivel2_mrgvariacion = 0;

                                                    foreach (var nivel3 in groupedNivel3)
                                                    {
                                                        // ---------- NIVEL 3 ----------
                                                        nivel2Container.Item()
                                                            .Background("#e3dca5")
                                                            .Padding(5)
                                                            .Text(nivel3.Key)
                                                            .FontFamily(fontFamily)
                                                            .FontSize(10)
                                                            .Bold();

                                                        nivel2Container.Item().Height(4);

                                                        // ---------- NIVEL 4 ----------
                                                        nivel2Container.Item().Table(table =>
                                                        {
                                                            table.ColumnsDefinition(columns =>
                                                            {
                                                                columns.RelativeColumn(4);
                                                                columns.RelativeColumn(2);
                                                                columns.RelativeColumn(2);
                                                                columns.RelativeColumn(2);
                                                                columns.RelativeColumn(2);
                                                                columns.RelativeColumn(2);
                                                                columns.RelativeColumn(2);
                                                            });

                                                            decimal total_total = 0;
                                                            decimal total_mrgtotal = 0;
                                                            decimal total_totalanterior = 0;
                                                            decimal total_mrgtotalanterior = 0;
                                                            decimal total_variacion = 0;
                                                            decimal total_mrgvariacion = 0;

                                                            int rowIndex = 0;

                                                            foreach (var item in nivel3)
                                                            {
                                                                bool esPar = rowIndex % 2 == 1; // 0-based
                                                                string bgColor = esPar ? "#f0f0f0" : "#ffffff";

                                                                table.Cell().Background(bgColor).Padding(2)
                                                                    .Text(item.nivel4).FontSize(8);

                                                                table.Cell().Background(bgColor).Padding(2).AlignRight()
                                                                    .Text(((decimal)item.total).ToString("N0")).FontSize(8);

                                                                table.Cell().Background(bgColor).Padding(2).AlignRight()
                                                                    .Text(((decimal)item.mrgtotal).ToString("N1")).FontSize(8);

                                                                table.Cell().Background(bgColor).Padding(2).AlignRight()
                                                                    .Text(((decimal)item.totalanterior).ToString("N0")).FontSize(8);

                                                                table.Cell().Background(bgColor).Padding(2).AlignRight()
                                                                    .Text(((decimal)item.mrgtotalanterior).ToString("N1")).FontSize(8);

                                                                table.Cell().Background(bgColor).Padding(2).AlignRight()
                                                                    .Text(((decimal)item.variacion).ToString("N0")).FontSize(8);

                                                                table.Cell().Background(bgColor).Padding(2).AlignRight()
                                                                    .Text(((decimal)item.mrgvariacion).ToString("N1")).FontSize(8);

                                                                // ---------- SUMATORIAS ----------
                                                                total_total += (decimal)item.total;
                                                                total_mrgtotal += (decimal)item.mrgtotal;
                                                                total_totalanterior += (decimal)item.totalanterior;
                                                                total_mrgtotalanterior += (decimal)item.mrgtotalanterior;
                                                                total_variacion += (decimal)item.variacion;
                                                                total_mrgvariacion += (decimal)item.mrgvariacion;

                                                                rowIndex++;
                                                            }


                                                            if (nivel3.Count() > 1)
                                                            {
                                                                table.Cell().Background("#d9d9d9").Text($"TOTAL {nivel3.Key}").Bold().FontSize(8);
                                                                table.Cell().Background("#d9d9d9").AlignRight().Text(total_total.ToString("N0")).Bold().FontSize(8);
                                                                table.Cell().Background("#d9d9d9").AlignRight().Text(total_mrgtotal.ToString("N1")).Bold().FontSize(8);
                                                                table.Cell().Background("#d9d9d9").AlignRight().Text(total_totalanterior.ToString("N0")).Bold().FontSize(8);
                                                                table.Cell().Background("#d9d9d9").AlignRight().Text(total_mrgtotalanterior.ToString("N1")).Bold().FontSize(8);
                                                                table.Cell().Background("#d9d9d9").AlignRight().Text(total_variacion.ToString("N0")).Bold().FontSize(8);
                                                                table.Cell().Background("#d9d9d9").AlignRight().Text(total_mrgvariacion.ToString("N1")).Bold().FontSize(8);
                                                            }

                                                            totalNivel2_total += total_total;
                                                            totalNivel2_mrgtotal += total_mrgtotal;
                                                            totalNivel2_totalanterior += total_totalanterior;
                                                            totalNivel2_mrgtotalanterior += total_mrgtotalanterior;
                                                            totalNivel2_variacion += total_variacion;
                                                            totalNivel2_mrgvariacion += total_mrgvariacion;
                                                        });

                                                        nivel2Container.Item().Height(10);
                                                    }

                                                    // ---------- TOTAL NIVEL 2 ----------
                                                    nivel2Container.Item().Table(table =>
                                                    {
                                                        table.ColumnsDefinition(columns =>
                                                        {
                                                            columns.RelativeColumn(4);
                                                            columns.RelativeColumn(2);
                                                            columns.RelativeColumn(2);
                                                            columns.RelativeColumn(2);
                                                            columns.RelativeColumn(2);
                                                            columns.RelativeColumn(2);
                                                            columns.RelativeColumn(2);
                                                        });

                                                        table.Cell().Background("#d9d9d9").Padding(5).Text($"TOTAL {nivel2.Key}").Bold().FontSize(9);
                                                        table.Cell().Background("#d9d9d9").Padding(5).AlignRight().Text(totalNivel2_total.ToString("N0")).Bold().FontSize(9);
                                                        table.Cell().Background("#d9d9d9").Padding(5).AlignRight().Text(totalNivel2_mrgtotal.ToString("N1")).Bold().FontSize(9);
                                                        table.Cell().Background("#d9d9d9").Padding(5).AlignRight().Text(totalNivel2_totalanterior.ToString("N0")).Bold().FontSize(9);
                                                        table.Cell().Background("#d9d9d9").Padding(5).AlignRight().Text(totalNivel2_mrgtotalanterior.ToString("N1")).Bold().FontSize(9);
                                                        table.Cell().Background("#d9d9d9").Padding(5).AlignRight().Text(totalNivel2_variacion.ToString("N0")).Bold().FontSize(9);
                                                        table.Cell().Background("#d9d9d9").Padding(5).AlignRight().Text(totalNivel2_mrgvariacion.ToString("N1")).Bold().FontSize(9);
                                                    });

                                                    nivel2Container.Item().Height(15);

                                                    // ---------- ACUMULAR NIVEL 1 ----------
                                                    totalNivel1_total += totalNivel2_total;
                                                    totalNivel1_mrgtotal += totalNivel2_mrgtotal;
                                                    totalNivel1_totalanterior += totalNivel2_totalanterior;
                                                    totalNivel1_mrgtotalanterior += totalNivel2_mrgtotalanterior;
                                                    totalNivel1_variacion += totalNivel2_variacion;
                                                    totalNivel1_mrgvariacion += totalNivel2_mrgvariacion;
                                                });
                                        }

                                        container.Item().Height(20);
                                    });
                            });




                        //                        page.Content()
                        //.PaddingTop(10)
                        //.PaddingLeft(30)
                        //.PaddingRight(30)
                        //.Column(col =>
                        //{
                        //    col.Item().Height(10);

                        //    // =====================================================
                        //    // CONTENEDOR GENERAL (BORDE VERDE)
                        //    // =====================================================
                        //    col.Item()
                        //        .Border(1)
                        //        .BorderColor("#285026")
                        //        .Padding(8)
                        //        .Column(container =>
                        //        {
                        //            // =============================================
                        //            // ENCABEZADO GLOBAL DE COLUMNAS
                        //            // =============================================
                        //            container.Item().Table(table =>
                        //            {
                        //                table.ColumnsDefinition(columns =>
                        //                {
                        //                    columns.RelativeColumn(4); // Concepto
                        //                    columns.RelativeColumn(2);
                        //                    columns.RelativeColumn(2);
                        //                    columns.RelativeColumn(2);
                        //                    columns.RelativeColumn(2);
                        //                    columns.RelativeColumn(2);
                        //                    columns.RelativeColumn(2);
                        //                });

                        //                table.Cell().Background("#285026").Padding(5).Text("CONCEPTO")
                        //                    .FontFamily(fontFamily).FontSize(9).Bold().FontColor(Colors.White);

                        //                table.Cell().Background("#285026").Padding(5).AlignRight().Text("COLUMNA 1")
                        //                    .FontFamily(fontFamily).FontSize(9).Bold().FontColor(Colors.White);

                        //                table.Cell().Background("#285026").Padding(5).AlignRight().Text("COLUMNA 2")
                        //                    .FontFamily(fontFamily).FontSize(9).Bold().FontColor(Colors.White);

                        //                table.Cell().Background("#285026").Padding(5).AlignRight().Text("COLUMNA 3")
                        //                    .FontFamily(fontFamily).FontSize(9).Bold().FontColor(Colors.White);

                        //                table.Cell().Background("#285026").Padding(5).AlignRight().Text("COLUMNA 4")
                        //                    .FontFamily(fontFamily).FontSize(9).Bold().FontColor(Colors.White);

                        //                table.Cell().Background("#285026").Padding(5).AlignRight().Text("COLUMNA 5")
                        //                    .FontFamily(fontFamily).FontSize(9).Bold().FontColor(Colors.White);

                        //                table.Cell().Background("#285026").Padding(5).AlignRight().Text("COLUMNA 6")
                        //                    .FontFamily(fontFamily).FontSize(9).Bold().FontColor(Colors.White);
                        //            });

                        //            container.Item().Height(8);

                        //            // =============================================
                        //            // AGRUPACIÓN NIVEL 2
                        //            // =============================================
                        //            var groupedNivel2 = resumen.balance
                        //                .GroupBy(x => x.nivel2)
                        //                .OrderBy(x => x.Key);

                        //            foreach (var nivel2 in groupedNivel2)
                        //            {
                        //                // ---------- NIVEL 2 ----------
                        //                container.Item()
                        //                    .Background("#285026")
                        //                    .Padding(6)
                        //                    .Text(nivel2.Key)
                        //                    .FontFamily(fontFamily)
                        //                    .FontSize(12)
                        //                    .Bold()
                        //                    .FontColor(Colors.White);

                        //                container.Item().Height(5);

                        //                var groupedNivel3 = nivel2
                        //                    .GroupBy(x => x.nivel3)
                        //                    .OrderBy(x => x.Key);

                        //                foreach (var nivel3 in groupedNivel3)
                        //                {
                        //                    // ---------- NIVEL 3 ----------
                        //                    container.Item()
                        //                        .Background("#e3dca5")
                        //                        .Padding(5)
                        //                        .Text(nivel3.Key)
                        //                        .FontFamily(fontFamily)
                        //                        .FontSize(10)
                        //                        .Bold()
                        //                        .FontColor(Colors.Black);

                        //                    container.Item().Height(4);

                        //                    // ---------- NIVEL 4 (DATOS) ----------
                        //                    container.Item().Table(table =>
                        //                    {
                        //                        table.ColumnsDefinition(columns =>
                        //                        {
                        //                            columns.RelativeColumn(4);
                        //                            columns.RelativeColumn(2);
                        //                            columns.RelativeColumn(2);
                        //                            columns.RelativeColumn(2);
                        //                            columns.RelativeColumn(2);
                        //                            columns.RelativeColumn(2);
                        //                            columns.RelativeColumn(2);
                        //                        });

                        //                        foreach (var item in nivel3)
                        //                        {
                        //                            table.Cell().Padding(2).Text(item.nivel4)
                        //                                .FontFamily(fontFamily)
                        //                                .FontSize(9);

                        //                            table.Cell().Padding(2).AlignRight().Text(item.total.ToString("N2")).FontSize(9);
                        //                            table.Cell().Padding(2).AlignRight().Text(item.mrgtotal.ToString("N2")).FontSize(9);
                        //                            table.Cell().Padding(2).AlignRight().Text(item.totalanterior.ToString("N2")).FontSize(9);
                        //                            table.Cell().Padding(2).AlignRight().Text(item.mrgtotalanterior.ToString("N2")).FontSize(9);
                        //                            table.Cell().Padding(2).AlignRight().Text(item.variacion.ToString("N2")).FontSize(9);
                        //                            table.Cell().Padding(2).AlignRight().Text(item.mrgvariacion.ToString("N2")).FontSize(9);
                        //                        }
                        //                    });

                        //                    container.Item().Height(10);
                        //                }

                        //                container.Item().Height(15);
                        //            }
                        //        });
                        //});



                        //page.Content()
                        //            .PaddingTop(10)
                        //            .PaddingLeft(30)
                        //            .PaddingRight(30)
                        //            .Column(col =>
                        //            {

                        //                col.Item().Height(10);

                        //                // ====== AGRUPACIÓN NIVEL 2 ======
                        //                var groupedNivel2 = resumen.balance
                        //                    .GroupBy(x => x.nivel2)
                        //                    .OrderBy(x => x.Key);

                        //                foreach (var nivel2 in groupedNivel2)
                        //                {
                        //                    // ====== HEADER NIVEL 2 ======
                        //                    col.Item().Background("#006600").Padding(6).Text(nivel2.Key)
                        //                        .FontFamily(fontFamily)
                        //                        .FontSize(13)
                        //                        .Bold()
                        //                        .FontColor("#ffc000");

                        //                    col.Item().Height(5);

                        //                    var groupedNivel3 = nivel2
                        //                        .GroupBy(x => x.nivel3)
                        //                        .OrderBy(x => x.Key);

                        //                    foreach (var nivel3 in groupedNivel3)
                        //                    {
                        //                        // ====== HEADER NIVEL 3 ======
                        //                        col.Item().Background("#e3dca5").Padding(5).Text(nivel3.Key)
                        //                            .FontFamily(fontFamily)
                        //                            .FontSize(11)
                        //                            .Bold()
                        //                            .FontColor(Colors.Black);

                        //                        col.Item().Height(4);

                        //                        // ====== TABLA NIVEL 4 ======
                        //                        col.Item().Table(table =>
                        //                        {
                        //                            table.ColumnsDefinition(columns =>
                        //                            {
                        //                                columns.RelativeColumn(4); // Nivel 4
                        //                                columns.RelativeColumn(2); // Total
                        //                                columns.RelativeColumn(2); // MRG Total
                        //                                columns.RelativeColumn(2); // Total Anterior
                        //                                columns.RelativeColumn(2); // MRG Total Ant
                        //                                columns.RelativeColumn(2); // Variación
                        //                                columns.RelativeColumn(2); // MRG Variación
                        //                            });

                        //                            foreach (var item in nivel3)
                        //                            {
                        //                                table.Cell().Padding(2).Text(item.nivel4)
                        //                                    .FontFamily(fontFamily)
                        //                                    .FontSize(9);

                        //                                table.Cell().Padding(2).AlignRight().Text(item.total.ToString("N2"))
                        //                                    .FontSize(9);

                        //                                table.Cell().Padding(2).AlignRight().Text(item.mrgtotal.ToString("N2"))
                        //                                    .FontSize(9);

                        //                                table.Cell().Padding(2).AlignRight().Text(item.totalanterior.ToString("N2"))
                        //                                    .FontSize(9);

                        //                                table.Cell().Padding(2).AlignRight().Text(item.mrgtotalanterior.ToString("N2"))
                        //                                    .FontSize(9);

                        //                                table.Cell().Padding(2).AlignRight().Text(item.variacion.ToString("N2"))
                        //                                    .FontSize(9);

                        //                                table.Cell().Padding(2).AlignRight().Text(item.mrgvariacion.ToString("N2"))
                        //                                    .FontSize(9);
                        //                            }
                        //                        });

                        //                        // Espacio entre Nivel 3
                        //                        col.Item().Height(10);
                        //                    }

                        //                    // Espacio entre Nivel 2
                        //                    col.Item().Height(15);
                        //                }
                        //            });



                    });
                }).GeneratePdf();
                RPT_Result result = new RPT_Result();
                result.extension = "pdf";
                result.nombredocumento = "BALANCE GENERAL";
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
