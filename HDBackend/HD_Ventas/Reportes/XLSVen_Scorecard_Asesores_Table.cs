using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Ventas.Reportes;
using HD_Ventas.Modelos;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;

namespace HD_Ventas.Reportes
{
    public class XLSVen_Scorecard_Asesores_Table
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
        public static Task<DocResult> GenerarExcel(IEnumerable<mdlCarga_Scorecard_porVendedor_Dash_TablaAsesor> scorecard, int ejercicio, int mes_actual, int ejercicio_inicio, int periodo_inicio)
        {
            try
            {
                string sheetname = "SCORECARD";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, $"SCORECARD GENERAL POR ASESOR", 31);

                    //renglon += 1;

                    sheet.Range(renglon, 1, renglon, 25).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");

                    sheet.Range(renglon, 2, renglon, 4).Merge().Value = "TRACTORES";
                    sheet.Range(renglon, 2, renglon, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 2, renglon, 4).Style.Font.Bold = true;
                    sheet.Range(renglon, 2, renglon, 4).Style.Fill.BackgroundColor = XLColor.LightGray;

                    sheet.Range(renglon, 5, renglon, 7).Merge().Value = "IMPLEMENTOS";
                    sheet.Range(renglon, 5, renglon, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 5, renglon, 7).Style.Font.Bold = true;
                    sheet.Range(renglon, 5, renglon, 7).Style.Fill.BackgroundColor = XLColor.LightGray;

                    sheet.Range(renglon, 8, renglon, 10).Merge().Value = "JARDINEROS";
                    sheet.Range(renglon, 8, renglon, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 8, renglon, 10).Style.Font.Bold = true;
                    sheet.Range(renglon, 8, renglon, 10).Style.Fill.BackgroundColor = XLColor.LightGray;

                    sheet.Range(renglon, 11, renglon, 13).Merge().Value = "AUTOGUIADOS";
                    sheet.Range(renglon, 11, renglon, 13).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 11, renglon, 13).Style.Font.Bold = true;
                    sheet.Range(renglon, 11, renglon, 13).Style.Fill.BackgroundColor = XLColor.LightGray;
                    int rengloncarteratot = renglon;

                    sheet.Range(renglon, 14, renglon, 16).Merge().Value = "DRONES";
                    sheet.Range(renglon, 14, renglon, 16).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 14, renglon, 16).Style.Font.Bold = true;
                    sheet.Range(renglon, 14, renglon, 16).Style.Fill.BackgroundColor = XLColor.LightGray;

                    sheet.Range(renglon, 17, renglon, 19).Merge().Value = "P. ALIADO";
                    sheet.Range(renglon, 17, renglon, 19).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 17, renglon, 19).Style.Font.Bold = true;
                    sheet.Range(renglon, 17, renglon, 19).Style.Fill.BackgroundColor = XLColor.LightGray;

                    sheet.Range(renglon, 20, renglon, 22).Merge().Value = "TRACTORES S.";
                    sheet.Range(renglon, 20, renglon, 22).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 20, renglon, 22).Style.Font.Bold = true;
                    sheet.Range(renglon, 20, renglon, 22).Style.Fill.BackgroundColor = XLColor.LightGray;

                    sheet.Range(renglon, 23, renglon, 25).Merge().Value = "TRILLADORAS S.";
                    sheet.Range(renglon, 23, renglon, 25).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 23, renglon, 25).Style.Font.Bold = true;
                    sheet.Range(renglon, 23, renglon, 25).Style.Fill.BackgroundColor = XLColor.LightGray;

                    sheet.Range(renglon, 26, renglon, 28).Merge().Value = "GARANTIAS";
                    sheet.Range(renglon, 26, renglon, 28).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 26, renglon, 28).Style.Font.Bold = true;
                    sheet.Range(renglon, 26, renglon, 28).Style.Fill.BackgroundColor = XLColor.LightGray;

                    sheet.Range(renglon, 29, renglon, 31).Merge().Value = "POLIZAS";
                    sheet.Range(renglon, 29, renglon, 31).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglon, 29, renglon, 31).Style.Font.Bold = true;
                    sheet.Range(renglon, 29, renglon, 31).Style.Fill.BackgroundColor = XLColor.LightGray;
                    int renglonrecuperaciontot = renglon;


                    renglon++;

                    sheet.Cell(renglon, 1).Value = "ASESOR";
                    sheet.Cell(renglon, 2).Value = "OBJETIVO";
                    sheet.Cell(renglon, 3).Value = "REAL";
                    sheet.Cell(renglon, 4).Value = "ALCANCE";
                    sheet.Cell(renglon, 5).Value = "OBJETIVO";
                    sheet.Cell(renglon, 6).Value = "REAL";
                    sheet.Cell(renglon, 7).Value = "ALCANCE";
                    sheet.Cell(renglon, 8).Value = "OBJETIVO";
                    sheet.Cell(renglon, 9).Value = "REAL";
                    sheet.Cell(renglon, 10).Value = "ALCANCE";
                    sheet.Cell(renglon, 11).Value = "OBJETIVO";
                    sheet.Cell(renglon, 12).Value = "REAL";
                    sheet.Cell(renglon, 13).Value = "ALCANCE";
                    sheet.Cell(renglon, 14).Value = "OBJETIVO";
                    sheet.Cell(renglon, 15).Value = "REAL";
                    sheet.Cell(renglon, 16).Value = "ALCANCE";
                    sheet.Cell(renglon, 17).Value = "OBJETIVO";
                    sheet.Cell(renglon, 18).Value = "REAL";
                    sheet.Cell(renglon, 19).Value = "ALCANCE";
                    sheet.Cell(renglon, 20).Value = "OBJETIVO";
                    sheet.Cell(renglon, 21).Value = "REAL";
                    sheet.Cell(renglon, 22).Value = "ALCANCE";
                    sheet.Cell(renglon, 23).Value = "OBJETIVO";
                    sheet.Cell(renglon, 24).Value = "REAL";
                    sheet.Cell(renglon, 25).Value = "ALCANCE";
                    sheet.Cell(renglon, 26).Value = "OBJETIVO";
                    sheet.Cell(renglon, 27).Value = "REAL";
                    sheet.Cell(renglon, 28).Value = "ALCANCE";
                    sheet.Cell(renglon, 29).Value = "OBJETIVO";
                    sheet.Cell(renglon, 30).Value = "REAL";
                    sheet.Cell(renglon, 31).Value = "ALCANCE";

                    // Estilo para los encabezados de la tabla
                    var rango = sheet.Range(renglon, 1, renglon, 31);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    var groupedByAdr = scorecard.GroupBy(x => x.adr);

                    foreach (var adrGroup in groupedByAdr)
                    {
                        sheet.Cell(renglon, 1).Value = adrGroup.Key;

                        var totalObjetivoAdrAutoguiados = adrGroup.Sum(x => x.Objetivo_Autoguiados);
                        var totalRealAdrAutoguiados = adrGroup.Sum(x => x.Real_Autoguiados);
                        var totalObjetivoAdrDrones = adrGroup.Sum(x => x.Objetivo_Drones);
                        var totalRealAdrDrones = adrGroup.Sum(x => x.Real_Drones);
                        var totalObjetivoAdrImplementos = adrGroup.Sum(x => x.Objetivo_Implementos);
                        var totalRealAdrImplementos = adrGroup.Sum(x => x.Real_Implementos);
                        var totalObjetivoAdrJardineros = adrGroup.Sum(x => x.Objetivo_Jardineros);
                        var totalRealAdrJardineros = adrGroup.Sum(x => x.Real_Jardineros);
                        var totalObjetivoAdrPA = adrGroup.Sum(x => x.Objetivo_PA);
                        var totalRealAdrPA = adrGroup.Sum(x => x.Real_PA);
                        var totalObjetivoAdrTractores = adrGroup.Sum(x => x.Objetivo_Tractores);
                        var totalRealAdrTractores = adrGroup.Sum(x => x.Real_Tractores);
                        var totalObjetivoAdrTracUsa = adrGroup.Sum(x => x.Objetivo_TracUsa);
                        var totalRealAdrTracUsa = adrGroup.Sum(x => x.Real_TracUsa);
                        var totalObjetivoAdrTriUsa = adrGroup.Sum(x => x.Objetivo_TriUsa);
                        var totalRealAdrTriUsa = adrGroup.Sum(x => x.Real_TriUsa);
                        var totalObjetivoAdrGarantias = adrGroup.Sum(x => x.Objetivo_Garantia);
                        var totalRealAdrGarantias = adrGroup.Sum(x => x.Real_Garantia);
                        var totalObjetivoAdrPolizas = adrGroup.Sum(x => x.Objetivo_Poliza);
                        var totalRealAdrPolizas = adrGroup.Sum(x => x.Real_Poliza);

                        sheet.Cell(renglon, 2).Value = totalObjetivoAdrTractores;
                        sheet.Cell(renglon, 3).Value = totalRealAdrTractores;
                        //float porcentajemensual = sco.objetivo > 0 ? sco.unidades_vendidas / sco.objetivo : 0;
                        sheet.Cell(renglon, 4).FormulaA1 = $"=IF(C{renglon} > B{renglon}, 1, IF(C{renglon} > 0, MIN(C{renglon}/B{renglon}, 1), 0))";
                        sheet.Cell(renglon, 6).Value = totalObjetivoAdrImplementos;
                        sheet.Cell(renglon, 5).Value = totalRealAdrImplementos;
                        //float porcentajeacumulado = sco.objetivo_acumulado > 0 ? sco.unidades_vendidas_acumulado / sco.objetivo_acumulado : 0;
                        sheet.Cell(renglon, 7).FormulaA1 = $"=IF(F{renglon} > E{renglon}, 1, IF(F{renglon} > 0, MIN(F{renglon}/E{renglon}, 1), 0))";
                        sheet.Cell(renglon, 8).Value = totalObjetivoAdrJardineros;
                        sheet.Cell(renglon, 9).Value = totalRealAdrJardineros;
                        sheet.Cell(renglon, 10).FormulaA1 = $"=IF(I{renglon} > H{renglon}, 1, IF(I{renglon} > 0, MIN(I{renglon}/H{renglon}, 1), 0))";
                        sheet.Cell(renglon, 11).Value = totalObjetivoAdrAutoguiados;
                        sheet.Cell(renglon, 12).Value = totalRealAdrAutoguiados;
                        sheet.Cell(renglon, 13).FormulaA1 = $"=IF(L{renglon} > K{renglon}, 1, IF(L{renglon} > 0, MIN(L{renglon}/K{renglon}, 1), 0))";
                        sheet.Cell(renglon, 14).Value = totalObjetivoAdrDrones;
                        sheet.Cell(renglon, 15).Value = totalRealAdrDrones;
                        sheet.Cell(renglon, 16).FormulaA1 = $"=IF(O{renglon} > N{renglon}, 1, IF(O{renglon} > 0, MIN(O{renglon}/N{renglon}, 1), 0))";
                        sheet.Cell(renglon, 17).Value = totalObjetivoAdrPA;
                        sheet.Cell(renglon, 18).Value = totalRealAdrPA;
                        sheet.Cell(renglon, 19).FormulaA1 = $"=IF(R{renglon} > Q{renglon}, 1, IF(R{renglon} > 0, MIN(R{renglon}/Q{renglon}, 1), 0))";
                        sheet.Cell(renglon, 20).Value = totalObjetivoAdrTracUsa;
                        sheet.Cell(renglon, 21).Value = totalRealAdrTracUsa;
                        sheet.Cell(renglon, 22).FormulaA1 = $"=IF(U{renglon} > T{renglon}, 1, IF(U{renglon} > 0, MIN(U{renglon}/T{renglon}, 1), 0))";
                        sheet.Cell(renglon, 23).Value = totalObjetivoAdrTriUsa;
                        sheet.Cell(renglon, 24).Value = totalRealAdrTriUsa;
                        sheet.Cell(renglon, 25).FormulaA1 = $"=IF(X{renglon} > W{renglon}, 1, IF(X{renglon} > 0, MIN(X{renglon}/W{renglon}, 1), 0))";
                        sheet.Cell(renglon, 26).Value = totalObjetivoAdrGarantias;
                        sheet.Cell(renglon, 27).Value = totalRealAdrGarantias;
                        sheet.Cell(renglon, 28).FormulaA1 = $"=IF(AA{renglon} > Z{renglon}, 1, IF(AA{renglon} > 0, MIN(AA{renglon}/Z{renglon}, 1), 0))";
                        sheet.Cell(renglon, 29).Value = totalObjetivoAdrPolizas;
                        sheet.Cell(renglon, 30).Value = totalRealAdrPolizas;
                        sheet.Cell(renglon, 31).FormulaA1 = $"=IF(AD{renglon} > AC{renglon}, 1, IF(AD{renglon} > 0, MIN(AD{renglon}/AC{renglon}, 1), 0))";
                        var rangoADR = sheet.Range(renglon, 1, renglon, 31);
                        rangoADR.Style.Fill.BackgroundColor = XLColor.FromHtml("#DAE6BE");
                        renglon++;

                        //var rango = sheet.Range(renglon, 1, renglon, 25);


                        var groupedBySucursal = adrGroup.GroupBy(x => x.sucursal);

                        foreach (var sucursalGroup in groupedBySucursal)
                        {
                            sheet.Cell(renglon, 1).Value = sucursalGroup.Key;

                            var totalObjetivoSucursalAutoguiados = sucursalGroup.Sum(x => x.Objetivo_Autoguiados);
                            var totalRealSucursalAutoguiados = sucursalGroup.Sum(x => x.Real_Autoguiados);
                            var totalObjetivoSucursalDrones = sucursalGroup.Sum(x => x.Objetivo_Drones);
                            var totalRealSucursalDrones = sucursalGroup.Sum(x => x.Real_Drones);
                            var totalObjetivoSucursalImplementos = sucursalGroup.Sum(x => x.Objetivo_Implementos);
                            var totalRealSucursalImplementos = sucursalGroup.Sum(x => x.Real_Implementos);
                            var totalObjetivoSucursalJardineros = sucursalGroup.Sum(x => x.Objetivo_Jardineros);
                            var totalRealSucursalJardineros = sucursalGroup.Sum(x => x.Real_Jardineros);
                            var totalObjetivoSucursalPA = sucursalGroup.Sum(x => x.Objetivo_PA);
                            var totalRealSucursalPA = sucursalGroup.Sum(x => x.Real_PA);
                            var totalObjetivoSucursalTractores = sucursalGroup.Sum(x => x.Objetivo_Tractores);
                            var totalRealSucursalTractores = sucursalGroup.Sum(x => x.Real_Tractores);
                            var totalObjetivoSucursalTracUsa = sucursalGroup.Sum(x => x.Objetivo_TracUsa);
                            var totalRealSucursalTracUsa = sucursalGroup.Sum(x => x.Real_TracUsa);
                            var totalObjetivoSucursalTriUsa = sucursalGroup.Sum(x => x.Objetivo_TriUsa);
                            var totalRealSucursalTriUsa = sucursalGroup.Sum(x => x.Real_TriUsa);
                            var totalObjetivoSucursalGarantias = sucursalGroup.Sum(x => x.Objetivo_Garantia);
                            var totalRealSucursalGarantias = sucursalGroup.Sum(x => x.Real_Garantia);
                            var totalObjetivoSucursalPolizas = sucursalGroup.Sum(x => x.Objetivo_Poliza);
                            var totalRealSucursalPolizas = sucursalGroup.Sum(x => x.Real_Poliza);

                            sheet.Cell(renglon, 2).Value = totalObjetivoSucursalTractores;
                            sheet.Cell(renglon, 3).Value = totalRealSucursalTractores;
                            //float porcentajemensual = sco.objetivo > 0 ? sco.unidades_vendidas / sco.objetivo : 0;
                            sheet.Cell(renglon, 4).FormulaA1 = $"=IF(C{renglon} > B{renglon}, 1, IF(C{renglon} > 0, MIN(C{renglon}/B{renglon}, 1), 0))";
                            sheet.Cell(renglon, 5).Value = totalObjetivoSucursalImplementos;
                            sheet.Cell(renglon, 6).Value = totalRealSucursalImplementos;
                            //float porcentajeacumulado = sco.objetivo_acumulado > 0 ? sco.unidades_vendidas_acumulado / sco.objetivo_acumulado : 0;
                            sheet.Cell(renglon, 7).FormulaA1 = $"=IF(F{renglon} > E{renglon}, 1, IF(F{renglon} > 0, MIN(F{renglon}/E{renglon}, 1), 0))";
                            sheet.Cell(renglon, 8).Value = totalObjetivoSucursalJardineros;
                            sheet.Cell(renglon, 9).Value = totalRealSucursalJardineros;
                            sheet.Cell(renglon, 10).FormulaA1 = $"=IF(I{renglon} > H{renglon}, 1, IF(I{renglon} > 0, MIN(I{renglon}/H{renglon}, 1), 0))";
                            sheet.Cell(renglon, 11).Value = totalObjetivoSucursalAutoguiados;
                            sheet.Cell(renglon, 12).Value = totalRealSucursalAutoguiados;
                            sheet.Cell(renglon, 13).FormulaA1 = $"=IF(L{renglon} > K{renglon}, 1, IF(L{renglon} > 0, MIN(L{renglon}/K{renglon}, 1), 0))";
                            sheet.Cell(renglon, 14).Value = totalObjetivoSucursalDrones;
                            sheet.Cell(renglon, 15).Value = totalRealSucursalDrones;
                            sheet.Cell(renglon, 16).FormulaA1 = $"=IF(O{renglon} > N{renglon}, 1, IF(O{renglon} > 0, MIN(O{renglon}/N{renglon}, 1), 0))";
                            sheet.Cell(renglon, 17).Value = totalObjetivoSucursalPA;
                            sheet.Cell(renglon, 18).Value = totalRealSucursalPA;
                            sheet.Cell(renglon, 19).FormulaA1 = $"=IF(R{renglon} > Q{renglon}, 1, IF(R{renglon} > 0, MIN(R{renglon}/Q{renglon}, 1), 0))";
                            sheet.Cell(renglon, 20).Value = totalObjetivoSucursalTracUsa;
                            sheet.Cell(renglon, 21).Value = totalRealSucursalTracUsa;
                            sheet.Cell(renglon, 22).FormulaA1 = $"=IF(U{renglon} > T{renglon}, 1, IF(U{renglon} > 0, MIN(U{renglon}/T{renglon}, 1), 0))";
                            sheet.Cell(renglon, 23).Value = totalObjetivoSucursalTriUsa;
                            sheet.Cell(renglon, 24).Value = totalRealSucursalTriUsa;
                            sheet.Cell(renglon, 25).FormulaA1 = $"=IF(X{renglon} > W{renglon}, 1, IF(X{renglon} > 0, MIN(X{renglon}/W{renglon}, 1), 0))";
                            sheet.Cell(renglon, 26).Value = totalObjetivoSucursalGarantias;
                            sheet.Cell(renglon, 27).Value = totalRealSucursalGarantias;
                            sheet.Cell(renglon, 28).FormulaA1 = $"=IF(AA{renglon} > Z{renglon}, 1, IF(AA{renglon} > 0, MIN(AA{renglon}/Z{renglon}, 1), 0))";
                            sheet.Cell(renglon, 29).Value = totalObjetivoSucursalPolizas;
                            sheet.Cell(renglon, 30).Value = totalRealSucursalPolizas;
                            sheet.Cell(renglon, 31).FormulaA1 = $"=IF(AD{renglon} > AC{renglon}, 1, IF(AD{renglon} > 0, MIN(AD{renglon}/AC{renglon}, 1), 0))";
                            var rangoSucursal = sheet.Range(renglon, 1, renglon, 31);
                            rangoSucursal.Style.Fill.BackgroundColor = XLColor.FromHtml("#e3e3e3");
                            renglon++;

                            foreach (var sco in sucursalGroup)
                            {
                                sheet.Cell(renglon, 1).Value = sco.asesor;
                                sheet.Cell(renglon, 2).Value = sco.Objetivo_Tractores;
                                sheet.Cell(renglon, 3).Value = sco.Real_Tractores;
                                //float porcentajemensual = sco.objetivo > 0 ? sco.unidades_vendidas / sco.objetivo : 0;
                                sheet.Cell(renglon, 4).FormulaA1 = $"=IF(C{renglon} > B{renglon}, 1, IF(C{renglon} > 0, MIN(C{renglon}/B{renglon}, 1), 0))";
                                sheet.Cell(renglon, 5).Value = sco.Objetivo_Implementos;
                                sheet.Cell(renglon, 6).Value = sco.Real_Implementos;
                                //float porcentajeacumulado = sco.objetivo_acumulado > 0 ? sco.unidades_vendidas_acumulado / sco.objetivo_acumulado : 0;
                                sheet.Cell(renglon, 7).FormulaA1 = $"=IF(F{renglon} > E{renglon}, 1, IF(F{renglon} > 0, MIN(F{renglon}/E{renglon}, 1), 0))";
                                sheet.Cell(renglon, 8).Value = sco.Objetivo_Jardineros;
                                sheet.Cell(renglon, 9).Value = sco.Real_Jardineros;
                                sheet.Cell(renglon, 10).FormulaA1 = $"=IF(I{renglon} > H{renglon}, 1, IF(I{renglon} > 0, MIN(I{renglon}/H{renglon}, 1), 0))";
                                sheet.Cell(renglon, 11).Value = sco.Objetivo_Autoguiados;
                                sheet.Cell(renglon, 12).Value = sco.Real_Autoguiados;
                                sheet.Cell(renglon, 13).FormulaA1 = $"=IF(L{renglon} > K{renglon}, 1, IF(L{renglon} > 0, MIN(L{renglon}/K{renglon}, 1), 0))";
                                sheet.Cell(renglon, 14).Value = sco.Objetivo_Drones;
                                sheet.Cell(renglon, 15).Value = sco.Real_Drones;
                                sheet.Cell(renglon, 16).FormulaA1 = $"=IF(O{renglon} > N{renglon}, 1, IF(O{renglon} > 0, MIN(O{renglon}/N{renglon}, 1), 0))";
                                sheet.Cell(renglon, 17).Value = sco.Objetivo_PA;
                                sheet.Cell(renglon, 18).Value = sco.Real_PA;
                                sheet.Cell(renglon, 19).FormulaA1 = $"=IF(R{renglon} > Q{renglon}, 1, IF(R{renglon} > 0, MIN(R{renglon}/Q{renglon}, 1), 0))";
                                sheet.Cell(renglon, 20).Value = sco.Objetivo_TracUsa;
                                sheet.Cell(renglon, 21).Value = sco.Real_TracUsa;
                                sheet.Cell(renglon, 22).FormulaA1 = $"=IF(U{renglon} > T{renglon}, 1, IF(U{renglon} > 0, MIN(U{renglon}/T{renglon}, 1), 0))";
                                sheet.Cell(renglon, 23).Value = sco.Objetivo_TriUsa;
                                sheet.Cell(renglon, 24).Value = sco.Real_TriUsa;
                                sheet.Cell(renglon, 25).FormulaA1 = $"=IF(X{renglon} > W{renglon}, 1, IF(X{renglon} > 0, MIN(X{renglon}/W{renglon}, 1), 0))";
                                sheet.Cell(renglon, 26).Value = sco.Objetivo_Garantia;
                                sheet.Cell(renglon, 27).Value = sco.Real_Garantia;
                                sheet.Cell(renglon, 28).FormulaA1 = $"=IF(AA{renglon} > Z{renglon}, 1, IF(AA{renglon} > 0, MIN(AA{renglon}/Z{renglon}, 1), 0))";
                                sheet.Cell(renglon, 29).Value = sco.Objetivo_Poliza;
                                sheet.Cell(renglon, 30).Value = sco.Real_Poliza;
                                sheet.Cell(renglon, 31).FormulaA1 = $"=IF(AD{renglon} > AC{renglon}, 1, IF(AD{renglon} > 0, MIN(AD{renglon}/AC{renglon}, 1), 0))";
                                renglon++;
                            }
                        }
                    }

                    // Llenar la tabla con los datos


                    //float totalImporteProyectado = scorecard.Sum(sco => sco.importe_proyectado);
                    //float totalImporte = scorecard.Sum(sco => sco.importe);
                    //float totalImporteProyectadoAcumulado = scorecard.Sum(sco => sco.importe_proyectado_acumulado);
                    //float totalImporteAcumulado = scorecard.Sum(sco => sco.importe_acumulado);

                    //sheet.Cell(renglon, 1).Value = "IMPORTE TOTAL";
                    //sheet.Cell(renglon, 2).Value = totalImporteAcumulado;
                    //sheet.Cell(renglon, 3).Value = totalImporte;
                    //sheet.Cell(renglon, 4).Value = "";
                    //sheet.Cell(renglon, 5).Value = totalImporteProyectadoAcumulado;
                    //sheet.Cell(renglon, 6).Value = totalImporteAcumulado;
                    //sheet.Cell(renglon, 7).Value = "";

                    //sheet.Row(renglon).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(4).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(7).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(10).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(13).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(16).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(19).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(22).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(25).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(28).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(31).Style.NumberFormat.Format = "0.0 %";
                    //sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(7).Style.NumberFormat.Format = "0.0 %";

                    //rango = sheet.Range(renglon, 1, renglon, 7);
                    //rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#e5e6e6");
                    //rango.Style.Font.Bold = true;

                    sheet.Columns().AdjustToContents();
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
                        filename = sheetname
                    };
                    return Task.FromResult(doc);
                }
                throw new Exception("ERROR EN LA GENERACION DEL ARCHIVO, FAVOR DE COMUNICARSE CON EL ADMINISTRADOR DEL SISTEMA");
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { errores = ex.Message });
            }
        }
    }
}