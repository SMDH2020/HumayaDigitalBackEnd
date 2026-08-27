using ClosedXML.Excel;
using HD.Clientes.Modelos.CRM.IndicadoresCotizaciones;
using HD_Ventas;
using HD_Ventas.Reportes;
using System.Globalization;

namespace HD_Reporteria.CRM
{
    public class XLS_Reporte_IndicadoresCotizaciones
    {
        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_IndicadoresCotizaciones_ReporteCotizaciones> detalle, int ejercicio, int periodo)
        {
            try
            {
                CultureInfo ci = new CultureInfo("es-MX");
                var datos = detalle == null ? new List<mdl_IndicadoresCotizaciones_ReporteCotizaciones>() : detalle.ToList();

                // Semanas y lineas en el orden en que las devuelve el SP.
                var semanas = datos.GroupBy(x => x.idsemana)
                                   .Select(g => new
                                   {
                                       idsemana = g.Key,
                                       fecha_inicio = g.First().fecha_inicio,
                                       fecha_fin = g.First().fecha_fin
                                   })
                                   .OrderBy(x => x.fecha_inicio)
                                   .ToList();

                var lineas = datos.GroupBy(x => x.idlinea)
                                  .Select(g => new { idlinea = g.Key, linea = g.First().linea ?? "" })
                                  .ToList();

                int columnasPorSemana = lineas.Count * 3;
                int totalColumnas = 2 + (semanas.Count * columnasPorSemana) + 3;

                string sheetname = "INDICADOR DE COTIZACIONES";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";

                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    string titulo = "REPORTE INDICADOR DE COTIZACIONES - " + XLS_IndicadoresEstilos.NombreMes(periodo, ci).ToUpper() + " " + ejercicio;
                    int renglon = XLSEncabezado.Encabezado(ref sheet, titulo, totalColumnas);

                    int filaSemana = renglon;
                    int filaLinea = renglon + 1;
                    int filaSub = renglon + 2;

                    // Asesor y Objetivo mensual: combinados en los tres renglones de encabezado.
                    sheet.Range(filaSemana, 1, filaSub, 1).Merge().Value = "ASESOR DE VENTAS";
                    sheet.Range(filaSemana, 2, filaSub, 2).Merge().Value = "OBJETIVO MENSUAL";

                    for (int s = 0; s < semanas.Count; s++)
                    {
                        int colInicioSemana = 3 + (s * columnasPorSemana);
                        var sem = semanas[s];

                        var rangoSemana = sheet.Range(filaSemana, colInicioSemana, filaSemana, colInicioSemana + columnasPorSemana - 1);
                        rangoSemana.Merge();
                        rangoSemana.Value = XLS_IndicadoresEstilos.EtiquetaSemana(s + 1, sem.fecha_inicio, sem.fecha_fin, ci);
                        rangoSemana.Style.Fill.BackgroundColor = XLColor.FromHtml(XLS_IndicadoresEstilos.EncabezadoSemana);
                        rangoSemana.Style.Font.FontColor = XLColor.White;

                        for (int l = 0; l < lineas.Count; l++)
                        {
                            int col = colInicioSemana + (l * 3);

                            var rangoLinea = sheet.Range(filaLinea, col, filaLinea, col + 2);
                            rangoLinea.Merge();
                            rangoLinea.Value = lineas[l].linea;
                            rangoLinea.Style.Fill.BackgroundColor = XLColor.FromHtml(
                                l % 2 == 0 ? XLS_IndicadoresEstilos.EncabezadoLineaB : XLS_IndicadoresEstilos.EncabezadoLineaA);
                            rangoLinea.Style.Font.FontColor = XLColor.White;

                            sheet.Cell(filaSub, col).Value = "Objetivo";
                            sheet.Cell(filaSub, col + 1).Value = "Real";
                            sheet.Cell(filaSub, col + 2).Value = "%";
                        }
                    }

                    // Totales del mes
                    int colTotal = 3 + (semanas.Count * columnasPorSemana);
                    var rangoTotal = sheet.Range(filaSemana, colTotal, filaLinea, colTotal + 2);
                    rangoTotal.Merge();
                    rangoTotal.Value = "TOTAL DEL MES";
                    rangoTotal.Style.Fill.BackgroundColor = XLColor.FromHtml(XLS_IndicadoresEstilos.EncabezadoLineaA);
                    rangoTotal.Style.Font.FontColor = XLColor.White;
                    sheet.Cell(filaSub, colTotal).Value = "Real";
                    sheet.Cell(filaSub, colTotal + 1).Value = "Faltan";
                    sheet.Cell(filaSub, colTotal + 2).Value = "Cumpl.";

                    // Estilo comun de los tres renglones de encabezado
                    var rangoEncabezado = sheet.Range(filaSemana, 1, filaSub, totalColumnas);
                    rangoEncabezado.Style.Font.Bold = true;
                    rangoEncabezado.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rangoEncabezado.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    rangoEncabezado.Style.Alignment.WrapText = true;
                    rangoEncabezado.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    rangoEncabezado.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    sheet.Range(filaSub, 1, filaSub, totalColumnas).Style.Fill.BackgroundColor = XLColor.FromHtml(XLS_IndicadoresEstilos.EncabezadoTabla);
                    sheet.Range(filaSemana, 1, filaSub, 2).Style.Fill.BackgroundColor = XLColor.FromHtml(XLS_IndicadoresEstilos.EncabezadoTabla);
                    sheet.Range(filaSemana, 1, filaSub, 2).Style.Font.FontColor = XLColor.Black;

                    sheet.Row(filaSemana).Height = 32;
                    sheet.Row(filaLinea).Height = 26;

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

                                // Indice por semana + linea para no recorrer la lista en cada celda.
                                var mapa = new Dictionary<string, mdl_IndicadoresCotizaciones_ReporteCotizaciones>();
                                foreach (var f in filas)
                                {
                                    string clave = f.idsemana + "-" + f.idlinea;
                                    if (!mapa.ContainsKey(clave)) mapa.Add(clave, f);
                                }

                                sheet.Cell(renglon, 1).Value = grupoVendedor.Key.vendedor;
                                sheet.Cell(renglon, 1).Style.Alignment.Indent = 2;

                                sheet.Cell(renglon, 2).Value = objetivoMensual;
                                sheet.Cell(renglon, 2).Style.NumberFormat.Format = "0";
                                sheet.Cell(renglon, 2).Style.Font.Bold = true;
                                sheet.Cell(renglon, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                int realMes = 0;

                                for (int s = 0; s < semanas.Count; s++)
                                {
                                    int colInicioSemana = 3 + (s * columnasPorSemana);

                                    for (int l = 0; l < lineas.Count; l++)
                                    {
                                        int col = colInicioSemana + (l * 3);
                                        string clave = semanas[s].idsemana + "-" + lineas[l].idlinea;

                                        if (!mapa.ContainsKey(clave))
                                        {
                                            XLS_IndicadoresEstilos.AplicarSemaforo(sheet.Cell(renglon, col + 2), null);
                                            continue;
                                        }

                                        var registro = mapa[clave];
                                        realMes += registro.real;

                                        sheet.Cell(renglon, col).Value = registro.objetivo;
                                        sheet.Cell(renglon, col).Style.NumberFormat.Format = "0";
                                        sheet.Cell(renglon, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                        sheet.Cell(renglon, col + 1).Value = registro.real;
                                        sheet.Cell(renglon, col + 1).Style.NumberFormat.Format = "0";
                                        sheet.Cell(renglon, col + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                        XLS_IndicadoresEstilos.AplicarSemaforo(sheet.Cell(renglon, col + 2), registro.cumplimiento_vp);
                                    }
                                }

                                sheet.Cell(renglon, colTotal).Value = realMes;
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
                                    decimal faltan = objetivoMensual - realMes;
                                    celdaFaltan.Value = faltan < 0 ? 0 : faltan;
                                    celdaFaltan.Style.NumberFormat.Format = "0";
                                }
                                celdaFaltan.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                decimal? cumplimientoMes = objetivoMensual > 0
                                    ? Math.Round((realMes / objetivoMensual) * 100, 0, MidpointRounding.AwayFromZero)
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
                    sheet.Column(1).Width = 34;
                    sheet.Column(2).Width = 12;
                    for (int col = 3; col <= totalColumnas; col++)
                        sheet.Column(col).Width = 9;

                    // Asesor y Objetivo mensual fijos a la izquierda, encabezados fijos arriba.
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
