using ClosedXML.Excel;
using HD.Clientes.Modelos.CRM.IndicadoresVisitas;
using HD_Ventas;
using HD_Ventas.Reportes;
using System.Globalization;

namespace HD_Reporteria.CRM
{
    public class XLS_Reporte_IndicadoresVisitas
    {
        private const string VerdeFondo = "#EAF3DE";
        private const string VerdeTexto = "#3B6D11";
        private const string AmbarFondo = "#FDF3E3";
        private const string AmbarTexto = "#A76B0B";
        private const string RojoFondo = "#FDECEB";
        private const string RojoTexto = "#C0392B";
        private const string GrisTexto = "#9E9E9E";
        private const string EncabezadoTabla = "#EBECEE";
        private const string BandaEstado = "#DDE4D5";
        private const string BandaSucursal = "#F1F3EE";

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

                    string titulo = "REPORTE INDICADOR DE VISITAS - " + NombreMes(periodo, ci).ToUpper() + " " + ejercicio;
                    int renglon = XLSEncabezado.Encabezado(ref sheet, titulo, totalColumnas);

                    int filaGrupo = renglon;
                    int filaSub = renglon + 1;

                    // Encabezados fijos
                    sheet.Range(filaGrupo, 1, filaSub, 1).Merge().Value = "ASESOR";
                    sheet.Range(filaGrupo, 2, filaSub, 2).Merge().Value = "OBJETIVO MENSUAL";

                    // Un grupo de 3 columnas por semana
                    for (int i = 0; i < semanas.Count; i++)
                    {
                        int col = 3 + (i * 3);
                        var s = semanas[i];
                        string etiqueta = "SEMANA " + (i + 1) + Environment.NewLine +
                                          s.fecha_inicio.ToString("dd", ci) + " - " +
                                          s.fecha_fin.ToString("dd", ci) + " de " + NombreMes(s.fecha_fin.Month, ci);

                        sheet.Range(filaGrupo, col, filaGrupo, col + 2).Merge().Value = etiqueta;
                        sheet.Cell(filaSub, col).Value = "Objetivo";
                        sheet.Cell(filaSub, col + 1).Value = "Visitas";
                        sheet.Cell(filaSub, col + 2).Value = "Cumpl.";
                    }

                    // Grupo de totales del mes
                    int colTotal = 3 + (semanas.Count * 3);
                    sheet.Range(filaGrupo, colTotal, filaGrupo, colTotal + 2).Merge().Value = "TOTAL DEL MES";
                    sheet.Cell(filaSub, colTotal).Value = "Realizadas";
                    sheet.Cell(filaSub, colTotal + 1).Value = "Faltan";
                    sheet.Cell(filaSub, colTotal + 2).Value = "Cumpl.";

                    var rangoEncabezado = sheet.Range(filaGrupo, 1, filaSub, totalColumnas);
                    rangoEncabezado.Style.Fill.BackgroundColor = XLColor.FromHtml(EncabezadoTabla);
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
                        renglon = EscribirBanda(sheet, renglon, totalColumnas, grupoEstado.Key.ToUpper(), BandaEstado, 11, 0);

                        foreach (var grupoSucursal in grupoEstado.GroupBy(x => x.sucursal ?? ""))
                        {
                            renglon = EscribirBanda(sheet, renglon, totalColumnas, grupoSucursal.Key.ToUpper(), BandaSucursal, 10, 1);

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
                                        AplicarSemaforo(sheet.Cell(renglon, col + 2), null);
                                        continue;
                                    }

                                    realizadasMes += registro.realizadas;

                                    sheet.Cell(renglon, col).Value = registro.objetivo_semanal;
                                    sheet.Cell(renglon, col).Style.NumberFormat.Format = "0";
                                    sheet.Cell(renglon, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                    sheet.Cell(renglon, col + 1).Value = registro.realizadas;
                                    sheet.Cell(renglon, col + 1).Style.NumberFormat.Format = "0";
                                    sheet.Cell(renglon, col + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                    AplicarSemaforo(sheet.Cell(renglon, col + 2), registro.cumplimiento_vp);
                                }

                                // Totales del mes
                                sheet.Cell(renglon, colTotal).Value = realizadasMes;
                                sheet.Cell(renglon, colTotal).Style.NumberFormat.Format = "0";
                                sheet.Cell(renglon, colTotal).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                var celdaFaltan = sheet.Cell(renglon, colTotal + 1);
                                if (objetivoMensual <= 0)
                                {
                                    celdaFaltan.Value = "—";
                                    celdaFaltan.Style.Font.FontColor = XLColor.FromHtml(GrisTexto);
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
                                AplicarSemaforo(sheet.Cell(renglon, colTotal + 2), cumplimientoMes);

                                renglon++;
                            }
                        }
                    }

                    // Bordes de todo el cuerpo
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

        /// <summary>
        /// Escribe una fila banda (Estado o Sucursal) combinada a todo lo ancho de la tabla.
        /// </summary>
        private static int EscribirBanda(IXLWorksheet sheet, int renglon, int totalColumnas, string texto, string fondo, double tamanio, int sangria)
        {
            var banda = sheet.Range(renglon, 1, renglon, totalColumnas);
            banda.Merge();
            banda.Value = texto;
            banda.Style.Fill.BackgroundColor = XLColor.FromHtml(fondo);
            banda.Style.Font.Bold = true;
            banda.Style.Font.FontSize = tamanio;
            banda.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            banda.Style.Alignment.Indent = sangria;
            sheet.Row(renglon).Height = 18;
            return renglon + 1;
        }

        /// <summary>
        /// Pinta el semaforo de cumplimiento. porcentaje null = sin objetivo capturado (N/A).
        /// </summary>
        private static void AplicarSemaforo(IXLCell celda, decimal? porcentaje)
        {
            celda.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            if (porcentaje == null)
            {
                celda.Value = "N/A";
                celda.Style.Font.FontColor = XLColor.FromHtml(GrisTexto);
                return;
            }

            decimal valor = porcentaje.Value;
            celda.Value = valor;
            celda.Style.NumberFormat.Format = "0\"%\"";

            if (valor >= 100)
            {
                celda.Style.Fill.BackgroundColor = XLColor.FromHtml(VerdeFondo);
                celda.Style.Font.FontColor = XLColor.FromHtml(VerdeTexto);
            }
            else if (valor > 80)
            {
                celda.Style.Fill.BackgroundColor = XLColor.FromHtml(AmbarFondo);
                celda.Style.Font.FontColor = XLColor.FromHtml(AmbarTexto);
            }
            else
            {
                celda.Style.Fill.BackgroundColor = XLColor.FromHtml(RojoFondo);
                celda.Style.Font.FontColor = XLColor.FromHtml(RojoTexto);
            }
        }

        private static string NombreMes(int mes, CultureInfo ci)
        {
            if (mes < 1 || mes > 12) return "";
            string nombre = ci.DateTimeFormat.GetMonthName(mes);
            return char.ToUpper(nombre[0]) + nombre.Substring(1);
        }
    }
}
