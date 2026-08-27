using ClosedXML.Excel;
using HD.Clientes.Modelos.CRM.IndicadoresVisitas;
using HD_Ventas;
using HD_Ventas.Reportes;
using System.Globalization;

namespace HD_Reporteria.CRM
{
    public class XLS_Reporte_IndicadoresVisitas
    {
        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_IndicadoresVisitas_ReporteVisitas> detalle, int ejercicio, int periodo)
        {
            try
            {
                CultureInfo ci = new CultureInfo("es-MX");
                var datos = detalle == null ? new List<mdl_IndicadoresVisitas_ReporteVisitas>() : detalle.ToList();

                // Semanas en el orden en que las devuelve el SP (no se reordena el detalle).
                var semanas = datos.GroupBy(x => x.idsemana)
                                   .Select(g => new
                                   {
                                       idsemana = g.Key,
                                       fecha_inicio = g.First().fecha_inicio,
                                       fecha_fin = g.First().fecha_fin
                                   })
                                   .OrderBy(x => x.fecha_inicio)
                                   .ToList();

                int totalColumnas = 2 + (semanas.Count * 3) + 3;

                string sheetname = "INDICADOR DE VISITAS";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";

                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    string titulo = "REPORTE INDICADOR DE VISITAS - " + XLS_IndicadoresEstilos.NombreMes(periodo, ci).ToUpper() + " " + ejercicio;
                    int renglon = XLSEncabezado.Encabezado(ref sheet, titulo, totalColumnas);

                    int filaGrupo = renglon;
                    int filaSub = renglon + 1;

                    sheet.Range(filaGrupo, 1, filaSub, 1).Merge().Value = "ASESOR";
                    sheet.Range(filaGrupo, 2, filaSub, 2).Merge().Value = "OBJETIVO MENSUAL";

                    for (int i = 0; i < semanas.Count; i++)
                    {
                        int col = 3 + (i * 3);
                        var s = semanas[i];

                        sheet.Range(filaGrupo, col, filaGrupo, col + 2).Merge().Value =
                            XLS_IndicadoresEstilos.EtiquetaSemana(i + 1, s.fecha_inicio, s.fecha_fin, ci);
                        sheet.Cell(filaSub, col).Value = "Objetivo";
                        sheet.Cell(filaSub, col + 1).Value = "Visitas";
                        sheet.Cell(filaSub, col + 2).Value = "Cumpl.";
                    }

                    int colTotal = 3 + (semanas.Count * 3);
                    sheet.Range(filaGrupo, colTotal, filaGrupo, colTotal + 2).Merge().Value = "TOTAL DEL MES";
                    sheet.Cell(filaSub, colTotal).Value = "Realizadas";
                    sheet.Cell(filaSub, colTotal + 1).Value = "Faltan";
                    sheet.Cell(filaSub, colTotal + 2).Value = "Cumpl.";

                    var rangoEncabezado = sheet.Range(filaGrupo, 1, filaSub, totalColumnas);
                    rangoEncabezado.Style.Fill.BackgroundColor = XLColor.FromHtml(XLS_IndicadoresEstilos.EncabezadoTabla);
                    rangoEncabezado.Style.Font.Bold = true;
                    rangoEncabezado.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rangoEncabezado.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    rangoEncabezado.Style.Alignment.WrapText = true;
                    rangoEncabezado.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    rangoEncabezado.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    sheet.Row(filaGrupo).Height = 32;

                    renglon = filaSub + 1;

                    // GroupBy conserva el orden de aparicion: se respeta el orden del SP.
                    foreach (var grupoEstado in datos.GroupBy(x => x.estado ?? ""))
                    {
                        renglon = XLS_IndicadoresEstilos.EscribirBanda(sheet, renglon, totalColumnas, grupoEstado.Key.ToUpper(), XLS_IndicadoresEstilos.BandaEstado, 11, 0);

                        foreach (var grupoSucursal in grupoEstado.GroupBy(x => x.sucursal ?? ""))
                        {
                            renglon = XLS_IndicadoresEstilos.EscribirBanda(sheet, renglon, totalColumnas, grupoSucursal.Key.ToUpper(), XLS_IndicadoresEstilos.BandaSucursal, 10, 1);

                            foreach (var grupoVendedor in grupoSucursal.GroupBy(x => new { x.idvendedor, x.vendedor }))
                            {
                                var filas = grupoVendedor.ToList();
                                decimal objetivoMensual = filas.First().objetivo_mensual;

                                sheet.Cell(renglon, 1).Value = grupoVendedor.Key.vendedor;
                                sheet.Cell(renglon, 1).Style.Alignment.Indent = 2;

                                sheet.Cell(renglon, 2).Value = objetivoMensual;
                                sheet.Cell(renglon, 2).Style.NumberFormat.Format = "0";
                                sheet.Cell(renglon, 2).Style.Font.Bold = true;
                                sheet.Cell(renglon, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                int realizadasMes = 0;

                                for (int i = 0; i < semanas.Count; i++)
                                {
                                    int col = 3 + (i * 3);
                                    var registro = filas.FirstOrDefault(x => x.idsemana == semanas[i].idsemana);

                                    if (registro == null)
                                    {
                                        XLS_IndicadoresEstilos.AplicarSemaforo(sheet.Cell(renglon, col + 2), null);
                                        continue;
                                    }

                                    realizadasMes += registro.realizadas;

                                    sheet.Cell(renglon, col).Value = registro.objetivo_semanal;
                                    sheet.Cell(renglon, col).Style.NumberFormat.Format = "0";
                                    sheet.Cell(renglon, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                    sheet.Cell(renglon, col + 1).Value = registro.realizadas;
                                    sheet.Cell(renglon, col + 1).Style.NumberFormat.Format = "0";
                                    sheet.Cell(renglon, col + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                    XLS_IndicadoresEstilos.AplicarSemaforo(sheet.Cell(renglon, col + 2), registro.cumplimiento_vp);
                                }

                                sheet.Cell(renglon, colTotal).Value = realizadasMes;
                                sheet.Cell(renglon, colTotal).Style.NumberFormat.Format = "0";
                                sheet.Cell(renglon, colTotal).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                var celdaFaltan = sheet.Cell(renglon, colTotal + 1);
                                if (objetivoMensual <= 0)
                                {
                                    celdaFaltan.Value = "—";
                                    celdaFaltan.Style.Font.FontColor = XLColor.FromHtml(XLS_IndicadoresEstilos.GrisTexto);
                                }
                                else
                                {
                                    decimal faltan = objetivoMensual - realizadasMes;
                                    celdaFaltan.Value = faltan < 0 ? 0 : faltan;
                                    celdaFaltan.Style.NumberFormat.Format = "0";
                                }
                                celdaFaltan.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                decimal? cumplimientoMes = objetivoMensual > 0
                                    ? Math.Round((realizadasMes / objetivoMensual) * 100, 0, MidpointRounding.AwayFromZero)
                                    : (decimal?)null;
                                XLS_IndicadoresEstilos.AplicarSemaforo(sheet.Cell(renglon, colTotal + 2), cumplimientoMes);

                                renglon++;
                            }
                        }
                    }

                    if (renglon > filaSub + 1)
                    {
                        var cuerpo = sheet.Range(filaSub + 1, 1, renglon - 1, totalColumnas);
                        cuerpo.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        cuerpo.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        cuerpo.Style.Border.OutsideBorderColor = XLColor.FromHtml("#D9D9D9");
                        cuerpo.Style.Border.InsideBorderColor = XLColor.FromHtml("#D9D9D9");
                    }

                    // Anchos fijos: AdjustToContents rompe los encabezados combinados.
                    sheet.Column(1).Width = 38;
                    sheet.Column(2).Width = 12;
                    for (int col = 3; col <= totalColumnas; col++)
                        sheet.Column(col).Width = 10;

                    sheet.SheetView.Freeze(filaSub, 2);

                    workbook.SaveAs(ruta);
                }

                if (System.IO.File.Exists(ruta))
                {
                    byte[] docbytes = System.IO.File.ReadAllBytes(ruta);
                    string docBase64 = Convert.ToBase64String(docbytes);
                    System.IO.File.Delete(ruta);
                    return Task.FromResult(new DocResult { documento = docBase64, filename = sheetname });
                }
                throw new Exception("ERROR EN LA GENERACION DEL ARCHIVO, FAVOR DE COMUNICARSE CON EL ADMINISTRADOR DEL SISTEMA");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
