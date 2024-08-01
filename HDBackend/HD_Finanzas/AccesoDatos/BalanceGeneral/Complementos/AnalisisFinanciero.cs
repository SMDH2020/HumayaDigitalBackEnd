using HD.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD_Finanzas.Modelos.Balance_General;
using ClosedXML.Excel;

namespace HD_Finanzas.AccesoDatos.BalanceGeneral.Complementos
{
    public static class DocBalanceGeneral
    {
        public static Task<DocResult> CrearExel(IEnumerable<Fmdl_BalanceGeneral> balance)
        {
            try
            {
                if (balance.Count() > 0)
                {
                    string ruta = $"C:\\SMDH\\Procesados\\BalanceGeneral.xlsx";
                    using (var worbook = new XLWorkbook())
                    {
                        var sheet = worbook.Worksheets.Add("Balance General");
                        sheet.Cell(1, 3).Value = "MEs";
                        sheet.Cell(1, 4).Value = "%";
                        sheet.Cell(1, 5).Value = "MEs";
                        sheet.Cell(1, 6).Value = "%";
                        sheet.Cell(1, 7).Value = "Variacion";
                        sheet.Cell(1, 8).Value = "%";
                        int renglon = 2;

                        var groupnivel1 = balance.GroupBy(x => x.nivel1).ToList();
                        foreach (Fmdl_BalanceGeneral nivel1 in groupnivel1)
                        {

                        }

                        foreach (Fmdl_BalanceGeneral bl in balance)
                        {
                            sheet.Cell(renglon, 1).Value = bl.nivel1;
                            sheet.Cell(renglon, 2).Value = bl.nivel2;
                            sheet.Cell(renglon, 3).Value = bl.nivel3;
                            sheet.Cell(renglon, 4).Value = bl.nivel4;
                            sheet.Cell(renglon, 5).Value = bl.total;
                            sheet.Cell(renglon, 5).Style.NumberFormat.Format = "#,##0";
                            sheet.Cell(renglon, 6).Value = bl.mrgtotal;
                            sheet.Cell(renglon, 6).Style.NumberFormat.Format = "#,##0.0";
                            sheet.Cell(renglon, 7).Value = bl.totalanterior;
                            sheet.Cell(renglon, 7).Style.NumberFormat.Format = "#,##0";
                            sheet.Cell(renglon, 8).Value = bl.mrgtotalanterior;
                            sheet.Cell(renglon, 8).Style.NumberFormat.Format = "#,##0.0";
                            sheet.Cell(renglon, 9).Value = bl.variacion;
                            sheet.Cell(renglon, 9).Style.NumberFormat.Format = "#,##0";
                            sheet.Cell(renglon, 10).Value = bl.mrgvariacion;
                            sheet.Cell(renglon, 10).Style.NumberFormat.Format = "#,##0.0";
                            renglon += 1;
                        }
                        sheet.Columns(1, 10).AdjustToContents();
                        worbook.SaveAs(ruta);
                        if (System.IO.File.Exists(ruta))
                        {
                            byte[] docbytes = System.IO.File.ReadAllBytes(ruta);
                            string docBase64 = Convert.ToBase64String(docbytes);
                            System.IO.File.Delete(ruta);

                            DocResult doc = new DocResult
                            {
                                documento = docBase64,
                                filename = "BalanceGeneral"
                            };

                            return Task.FromResult(doc);

                        }
                        throw new Exception("ERROR EN LA GENERACION DEL ARCHIVO, FAVOR DE COMUNICARSE CON EL ADMINISTRADOR DEL SISTEMA");
                    }
                }
                throw new Exception("No existe informacion para mostrar");
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { errores = ex.Message });
            }
        }

    }

    public static class DocBalanceGeneralConsolidado
    {


        public static Task<DocResult> CrearExcel(List<BalanceConsolidado> bgc, vmBalanceGeneral vm)
        {
            try
            {
                string ruta = $"C:\\SMDH\\Procesados\\PVE{vm.Ejercicio}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add("Balance General");

                    int columnas = 4 + (vm.periodo * 2);
                    int renglon = 2;

                    var cell = sheet.Range(1, 1, 1, columnas);
                    cell.Value = $"Balance General ({vm.Ejercicio})".ToUpper();
                    cell.Style.Font.Bold = true;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    cell.Style.Font.FontSize = 20;
                    cell.Merge();

                    int colum = 3;
                    foreach (var titles in bgc)
                    {
                        cell = sheet.Range(renglon, colum, renglon, colum + 1);
                        cell.Value = DocUtil.MonthName(titles.Periodo).ToUpper();
                        cell.Style.Font.Bold = true;
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        cell.Style.Font.FontSize = 14;
                        cell.Merge();

                        colum += 2;
                    }
                    cell = sheet.Range(renglon, colum, renglon, colum + 1);
                    cell.Value = "Promedio".ToUpper();
                    cell.Style.Font.Bold = true;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    cell.Style.Font.FontSize = 14;
                    cell.Merge();

                    renglon += 1;


                    var Piloto = bgc.Where(x => x.Periodo == 1).FirstOrDefault();
                    var PeriodoPiloto = Piloto.BalanceGeneral;

                    var nivel1 = PeriodoPiloto.GroupBy(x => x.nivel1).ToList();
                    foreach (var n1 in nivel1)
                    {
                        var nivel2 = PeriodoPiloto.Where(x => x.nivel1 == n1.Key).GroupBy(x => x.nivel2).ToList();
                        foreach (var n2 in nivel2)
                        {
                            cell = sheet.Range(renglon, 1, renglon, columnas);
                            cell.Value = n2.Key;
                            cell.Style.Font.Bold = true;
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            cell.Style.Font.FontSize = 14;
                            cell.Style.Fill.BackgroundColor = XLColor.FromArgb(0, 102, 0);
                            cell.Style.Font.FontColor = XLColor.FromArgb(255, 192, 0);
                            cell.Merge();
                            renglon += 1;

                            var nivel3 = PeriodoPiloto.Where(x => x.nivel1 == n1.Key && x.nivel2 == n2.Key).GroupBy(x => x.nivel3).ToList();
                            foreach (var n3 in nivel3)
                            {
                                sheet.Cell(renglon, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(227, 220, 165);
                                cell = sheet.Range(renglon, 2, renglon, columnas);
                                cell.Value = n3.Key;
                                cell.Style.Font.Bold = true;
                                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                cell.Style.Font.FontSize = 12;
                                cell.Style.Fill.BackgroundColor = XLColor.FromArgb(227, 220, 165);
                                cell.Merge();
                                renglon += 1;

                                var nivel4 = PeriodoPiloto.Where(x => x.nivel1 == n1.Key && x.nivel2 == n2.Key && x.nivel3 == n3.Key).ToList();
                                foreach (var n4 in nivel4)
                                {
                                    cell = sheet.Range(renglon, 2, renglon, 2);
                                    cell.Value = n4.nivel4;
                                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                    cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                    cell.Style.Font.FontSize = 12;
                                    cell.Merge();

                                    int columnaInicial = 2;
                                    foreach (var elements in bgc)
                                    {
                                        var item = elements.BalanceGeneral.Where(x => x.nivel2 == n2.Key && x.nivel3 == n3.Key && x.nivel4 == n4.nivel4).FirstOrDefault();
                                        sheet.Cell(renglon, columnaInicial + 1).Value = item.total;
                                        sheet.Cell(renglon, columnaInicial + 1).Style.NumberFormat.Format = "#,##0";
                                        sheet.Cell(renglon, columnaInicial + 2).Value = (item.mrgtotal / 100);
                                        sheet.Cell(renglon, columnaInicial + 2).Style.NumberFormat.Format = "0.0%";
                                        columnaInicial += 2;
                                    }
                                    sheet.Cell(renglon, columnaInicial + 1).SetFormulaA1(FormulaImportes(renglon, vm.periodo));
                                    sheet.Cell(renglon, columnaInicial + 1).Style.NumberFormat.Format = "#,##0";
                                    sheet.Cell(renglon, columnaInicial + 2).SetFormulaA1(FormulaProcentajes(renglon, vm.periodo));
                                    sheet.Cell(renglon, columnaInicial + 2).Style.NumberFormat.Format = "0.0%";
                                    renglon += 1;
                                }

                                if (nivel4.Count > 2)
                                {
                                    cell = sheet.Range(renglon, 2, renglon, 2);
                                    cell.Value = $"TOTAL {n3.Key}".ToUpper();
                                    cell.Style.Font.Bold = true;
                                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                    cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                    cell.Style.Font.FontSize = 12;
                                    cell.Merge();

                                    int columnan4 = 2;
                                    foreach (var elements in bgc)
                                    {
                                        double totalNivel4 = elements.BalanceGeneral.Where(x => x.nivel2 == n2.Key && x.nivel3 == n3.Key).Sum(x => x.total);
                                        double totalNivel1 = elements.BalanceGeneral.Where(x => x.nivel1 == n1.Key).Sum(x => x.total);
                                        sheet.Cell(renglon, columnan4 + 1).Value = totalNivel4;
                                        sheet.Cell(renglon, columnan4 + 1).Style.NumberFormat.Format = "#,##0";
                                        sheet.Cell(renglon, columnan4 + 1).Style.Font.Bold = true;
                                        sheet.Cell(renglon, columnan4 + 2).Value = totalNivel4 / totalNivel1;
                                        sheet.Cell(renglon, columnan4 + 2).Style.NumberFormat.Format = "0.0%";
                                        sheet.Cell(renglon, columnan4 + 2).Style.Font.Bold = true; ;
                                        columnan4 += 2;
                                    }
                                    sheet.Cell(renglon, columnan4 + 1).SetFormulaA1(FormulaImportes(renglon, vm.periodo));
                                    sheet.Cell(renglon, columnan4 + 1).Style.NumberFormat.Format = "#,##0";
                                    sheet.Cell(renglon, columnan4 + 1).Style.Font.Bold = true; ;
                                    sheet.Cell(renglon, columnan4 + 2).SetFormulaA1(FormulaProcentajes(renglon, vm.periodo));
                                    sheet.Cell(renglon, columnan4 + 2).Style.NumberFormat.Format = "0.0%";
                                    sheet.Cell(renglon, columnan4 + 2).Style.Font.Bold = true; ;
                                    renglon += 1;
                                }
                            }
                            cell = sheet.Range(renglon, 1, renglon, 2);
                            cell.Value = $"TOTAL {n2.Key}".ToUpper();
                            cell.Style.Font.Bold = true;
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            cell.Style.Font.FontSize = 14;
                            cell.Style.Fill.BackgroundColor = XLColor.FromArgb(0, 102, 0);
                            cell.Style.Font.FontColor = XLColor.FromArgb(255, 192, 0);
                            cell.Merge();

                            sheet.Cell(renglon, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(0, 102, 0);
                            int columnan2 = 2;
                            foreach (var elements in bgc)
                            {
                                double totalNivel = elements.BalanceGeneral.Where(x => x.nivel2 == n2.Key).Sum(x => x.total);
                                double totalNivel1 = elements.BalanceGeneral.Where(x => x.nivel1 == n1.Key).Sum(x => x.total);
                                sheet.Cell(renglon, columnan2 + 1).Value = totalNivel;
                                sheet.Cell(renglon, columnan2 + 1).Style.NumberFormat.Format = "#,##0";
                                sheet.Cell(renglon, columnan2 + 1).Style.Font.Bold = true;
                                sheet.Cell(renglon, columnan2 + 1).Style.Font.FontSize = 14;
                                sheet.Cell(renglon, columnan2 + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(0, 102, 0);
                                sheet.Cell(renglon, columnan2 + 1).Style.Font.FontColor = XLColor.FromArgb(255, 192, 0);
                                sheet.Cell(renglon, columnan2 + 2).Value = totalNivel / totalNivel1;
                                sheet.Cell(renglon, columnan2 + 2).Style.NumberFormat.Format = "0.0%";
                                sheet.Cell(renglon, columnan2 + 2).Style.Font.Bold = true;
                                sheet.Cell(renglon, columnan2 + 2).Style.Font.FontSize = 14;
                                sheet.Cell(renglon, columnan2 + 2).Style.Fill.BackgroundColor = XLColor.FromArgb(0, 102, 0);
                                sheet.Cell(renglon, columnan2 + 2).Style.Font.FontColor = XLColor.FromArgb(255, 192, 0);
                                columnan2 += 2;
                            }
                            sheet.Cell(renglon, columnan2 + 1).SetFormulaA1(FormulaImportes(renglon, vm.periodo));
                            sheet.Cell(renglon, columnan2 + 1).Style.NumberFormat.Format = "#,##0";
                            sheet.Cell(renglon, columnan2 + 1).Style.Font.Bold = true;
                            sheet.Cell(renglon, columnan2 + 1).Style.Font.FontSize = 14;
                            sheet.Cell(renglon, columnan2 + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(0, 102, 0);
                            sheet.Cell(renglon, columnan2 + 1).Style.Font.FontColor = XLColor.FromArgb(255, 192, 0);
                            sheet.Cell(renglon, columnan2 + 2).SetFormulaA1(FormulaProcentajes(renglon, vm.periodo));
                            sheet.Cell(renglon, columnan2 + 2).Style.NumberFormat.Format = "0.0%";
                            sheet.Cell(renglon, columnan2 + 2).Style.Font.Bold = true;
                            sheet.Cell(renglon, columnan2 + 2).Style.Font.FontSize = 14;
                            sheet.Cell(renglon, columnan2 + 2).Style.Fill.BackgroundColor = XLColor.FromArgb(0, 102, 0);
                            sheet.Cell(renglon, columnan2 + 2).Style.Font.FontColor = XLColor.FromArgb(255, 192, 0);
                            renglon += 2;
                        }

                        cell = sheet.Range(renglon, 2, renglon, 2);
                        cell.Value = $"TOTAL {n1.Key}".ToUpper();
                        cell.Style.Font.Bold = true;
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        cell.Style.Font.FontSize = 16;
                        cell.Style.Fill.BackgroundColor = XLColor.FromArgb(155, 176, 111);
                        cell.Merge();

                        sheet.Cell(renglon, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(155, 176, 111);
                        int columnan1 = 2;
                        foreach (var elements in bgc)
                        {
                            sheet.Cell(renglon, columnan1 + 1).Value = elements.BalanceGeneral.Where(x => x.nivel1 == n1.Key).Sum(x => x.total);
                            sheet.Cell(renglon, columnan1 + 1).Style.NumberFormat.Format = "#,##0";
                            sheet.Cell(renglon, columnan1 + 1).Style.Font.Bold = true;
                            sheet.Cell(renglon, columnan1 + 1).Style.Font.FontSize = 16;
                            sheet.Cell(renglon, columnan1 + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(155, 176, 111);
                            sheet.Cell(renglon, columnan1 + 2).Value = 1;
                            sheet.Cell(renglon, columnan1 + 2).Style.NumberFormat.Format = "0.0%";
                            sheet.Cell(renglon, columnan1 + 2).Style.Font.Bold = true;
                            sheet.Cell(renglon, columnan1 + 2).Style.Font.FontSize = 16;
                            sheet.Cell(renglon, columnan1 + 2).Style.Fill.BackgroundColor = XLColor.FromArgb(155, 176, 111);
                            columnan1 += 2;
                        }
                        sheet.Cell(renglon, columnan1 + 1).SetFormulaA1(FormulaImportes(renglon, vm.periodo));
                        sheet.Cell(renglon, columnan1 + 1).Style.NumberFormat.Format = "#,##0";
                        sheet.Cell(renglon, columnan1 + 1).Style.Font.Bold = true;
                        sheet.Cell(renglon, columnan1 + 1).Style.Font.FontSize = 16;
                        sheet.Cell(renglon, columnan1 + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(155, 176, 111);
                        sheet.Cell(renglon, columnan1 + 2).SetFormulaA1(FormulaProcentajes(renglon, vm.periodo));
                        sheet.Cell(renglon, columnan1 + 2).Style.NumberFormat.Format = "0.0%";
                        sheet.Cell(renglon, columnan1 + 2).Style.Font.Bold = true;
                        sheet.Cell(renglon, columnan1 + 2).Style.Font.FontSize = 16;
                        sheet.Cell(renglon, columnan1 + 2).Style.Fill.BackgroundColor = XLColor.FromArgb(155, 176, 111);
                        renglon += 2;
                    }



                    sheet.Columns().AdjustToContents();

                    sheet.Column(1).Width = 3;
                    workbook.SaveAs(ruta);

                }
                if (System.IO.File.Exists(ruta))
                {
                    byte[] docbytes = System.IO.File.ReadAllBytes(ruta);
                    string docBase64 = Convert.ToBase64String(docbytes);
                    System.IO.File.Delete(ruta);

                    DocResult doc = new DocResult
                    {
                        documento = docBase64,
                        filename = "Proyeccion Ventas"
                    };

                    return Task.FromResult(doc);

                }
                DocResult result = new DocResult();
                return Task.FromResult(result);

            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        static string FormulaImportes(int renglon, int periodos)
        {
            string result = "=(";
            int periodo = 1;
            while (periodo <= periodos)
            {
                result += $"+{cellName(periodo, renglon)}";
                periodo += 1;
            }
            return $"{result})/{periodos}";
        }
        static string FormulaProcentajes(int renglon, int periodos)
        {
            string result = "=(";
            int periodo = 1;
            while (periodo <= periodos)
            {
                result += $"+{cellNamePor(periodo, renglon)}";
                periodo += 1;
            }
            return $"{result})/{periodos}";
        }
        static string cellName(int Periodo, int Renglon)
        {
            string result = "";

            switch (Periodo)
            {
                case 1: result = $"C{Renglon}"; break;
                case 2: result = $"E{Renglon}"; break;
                case 3: result = $"G{Renglon}"; break;
                case 4: result = $"I{Renglon}"; break;
                case 5: result = $"K{Renglon}"; break;
                case 6: result = $"M{Renglon}"; break;
                case 7: result = $"O{Renglon}"; break;
                case 8: result = $"Q{Renglon}"; break;
                case 9: result = $"S{Renglon}"; break;
                case 10: result = $"U{Renglon}"; break;
                case 11: result = $"W{Renglon}"; break;
                case 12: result = $"Y{Renglon}"; break;
            }
            return result;
        }
        static string cellNamePor(int Periodo, int Renglon)
        {
            string result = "";

            switch (Periodo)
            {
                case 1: result = $"D{Renglon}"; break;
                case 2: result = $"F{Renglon}"; break;
                case 3: result = $"H{Renglon}"; break;
                case 4: result = $"J{Renglon}"; break;
                case 5: result = $"L{Renglon}"; break;
                case 6: result = $"N{Renglon}"; break;
                case 7: result = $"P{Renglon}"; break;
                case 8: result = $"R{Renglon}"; break;
                case 9: result = $"T{Renglon}"; break;
                case 10: result = $"V{Renglon}"; break;
                case 11: result = $"X{Renglon}"; break;
                case 12: result = $"Z{Renglon}"; break;
            }
            return result;
        }
    }
}
