using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Cobranza;
using HD_Ventas.Reportes;
using Postventa.Modelos;
using Postventa.Modelos.Indicadores;

namespace Postventa.Reportes
{
    public class XLS_DesempeñoPorResponsable
    {
        /// <summary>
        /// Sobrecarga que recibe el filtro y arma el texto del periodo, para que el
        /// controlador no tenga que formatear nada.
        /// </summary>
        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_Indicadores_Detalle> detalle,
                                                   int ejercicio_inicio, int ejercicio_fin,
                                                   int periodo_inicio, int periodo_fin)
        {
            return GenerarExcel(detalle, ArmarPeriodo(ejercicio_inicio, ejercicio_fin, periodo_inicio, periodo_fin));
        }

        private static string ArmarPeriodo(int ejercicioInicio, int ejercicioFin, int periodoInicio, int periodoFin)
        {
            var cultura = new System.Globalization.CultureInfo("es-MX");

            string MesNombre(int mes)
            {
                if (mes < 1 || mes > 12) return "";
                string nombre = cultura.DateTimeFormat.GetMonthName(mes);
                return char.ToUpper(nombre[0], cultura) + nombre.Substring(1);
            }

            string mesIni = MesNombre(periodoInicio);
            string mesFin = MesNombre(periodoFin);

            if (ejercicioInicio != ejercicioFin)
                return periodoInicio == periodoFin
                    ? $"{mesIni} {ejercicioInicio} - {ejercicioFin}"
                    : $"{mesIni} {ejercicioInicio} - {mesFin} {ejercicioFin}";

            return periodoInicio == periodoFin
                ? $"{mesIni} {ejercicioFin}"
                : $"{mesIni} - {mesFin} {ejercicioFin}";
        }

        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_Indicadores_Detalle> detalle, string periodo = "")
        {
            try
            {
                var lista = (detalle ?? Enumerable.Empty<mdl_Indicadores_Detalle>()).ToList();

                /* ------------------------------------------------------------------
                   Mismo agrupado que el PDF: un renglon por responsable con sus
                   totales y debajo los clientes que los componen. Se agrupa por
                   idresponsable y no por nombre para no mezclar homonimos.
                   ------------------------------------------------------------------ */
                var grupos = lista
                    .GroupBy(d => new { d.idresponsable, d.Responsable })
                    .Select(g => new
                    {
                        Responsable = g.Key.Responsable,
                        Sucursal = g.Select(x => x.Sucursal).Distinct().Count() == 1
                                        ? g.First().Sucursal
                                        : "VARIAS",
                        FacturadosTotal = g.Sum(x => x.FacturadosTotal),
                        MontoTotal = g.Sum(x => x.MontoTotal),
                        MensajesEnviados = g.Sum(x => x.MensajesEnviados),
                        FacturadosMensajeria = g.Sum(x => x.FacturadosMensajeria),
                        MontoMensajeria = g.Sum(x => x.MontoMensajeria),
                        Clientes = g.OrderByDescending(x => x.MontoMensajeria)
                                    .ThenByDescending(x => x.MontoTotal)
                                    .ToList()
                    })
                    .OrderByDescending(g => g.MontoMensajeria)
                    .ThenByDescending(g => g.MontoTotal)
                    .ThenBy(g => g.Responsable)
                    .ToList();

                string sheetname = "DESEMPEÑO POR RESPONSABLE";
                string titulo = string.IsNullOrWhiteSpace(periodo) ? sheetname : $"{sheetname} - {periodo.ToUpper()}";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";

                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    // El renglon del responsable queda ARRIBA de sus clientes al agrupar
                    sheet.Outline.SummaryVLocation = XLOutlineSummaryVLocation.Top;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, titulo, 7);

                    sheet.Cell(renglon, 1).Value = "RESPONSABLE";
                    sheet.Cell(renglon, 2).Value = "SUCURSAL";
                    sheet.Cell(renglon, 3).Value = "FACTURACION TOTAL";
                    sheet.Cell(renglon, 4).Value = "IMPORTE FACTURADO";
                    sheet.Cell(renglon, 5).Value = "MENSAJES ENVIADOS";
                    sheet.Cell(renglon, 6).Value = "FACTURADO VIA MENSAJERIA";
                    sheet.Cell(renglon, 7).Value = "IMPORTE VIA MENSAJERIA";

                    // Estilo para los encabezados de la tabla
                    var rango = sheet.Range(renglon, 1, renglon, 7);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    rango.Style.Alignment.WrapText = true;

                    int renglonEncabezado = renglon;
                    sheet.SheetView.FreezeRows(renglonEncabezado);   // el encabezado queda fijo al desplazarse
                    renglon++;

                    // Llenar la tabla con los datos
                    foreach (var grupo in grupos)
                    {
                        sheet.Cell(renglon, 1).Value = grupo.Responsable?.ToUpper();
                        sheet.Cell(renglon, 2).Value = grupo.Sucursal?.ToUpper();
                        sheet.Cell(renglon, 3).Value = grupo.FacturadosTotal;
                        sheet.Cell(renglon, 4).Value = grupo.MontoTotal;
                        sheet.Cell(renglon, 5).Value = grupo.MensajesEnviados;
                        sheet.Cell(renglon, 6).Value = grupo.FacturadosMensajeria;
                        sheet.Cell(renglon, 7).Value = grupo.MontoMensajeria;

                        rango = sheet.Range(renglon, 1, renglon, 7);
                        rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EEF1E9");
                        rango.Style.Font.Bold = true;
                        renglon++;

                        int primerCliente = renglon;

                        foreach (var cliente in grupo.Clientes)
                        {
                            sheet.Cell(renglon, 1).Value = cliente.Cliente?.ToUpper();
                            sheet.Cell(renglon, 1).Style.Alignment.Indent = 2;
                            sheet.Cell(renglon, 2).Value = cliente.Sucursal?.ToUpper();
                            sheet.Cell(renglon, 3).Value = cliente.FacturadosTotal;
                            sheet.Cell(renglon, 4).Value = cliente.MontoTotal;
                            sheet.Cell(renglon, 5).Value = cliente.MensajesEnviados;
                            sheet.Cell(renglon, 6).Value = cliente.FacturadosMensajeria;
                            sheet.Cell(renglon, 7).Value = cliente.MontoMensajeria;
                            renglon++;
                        }

                        // Los clientes se agrupan para poder colapsarlos, igual que
                        // el desplegable del tablero.
                        if (renglon > primerCliente)
                            sheet.Rows(primerCliente, renglon - 1).Group();
                    }

                    // Renglon de totales
                    sheet.Cell(renglon, 1).Value = "TOTALES";
                    sheet.Cell(renglon, 3).Value = lista.Sum(d => d.FacturadosTotal);
                    sheet.Cell(renglon, 4).Value = lista.Sum(d => d.MontoTotal);
                    sheet.Cell(renglon, 5).Value = lista.Sum(d => d.MensajesEnviados);
                    sheet.Cell(renglon, 6).Value = lista.Sum(d => d.FacturadosMensajeria);
                    sheet.Cell(renglon, 7).Value = lista.Sum(d => d.MontoMensajeria);

                    rango = sheet.Range(renglon, 1, renglon, 7);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#e5e6e6");
                    rango.Style.Font.Bold = true;
                    rango.Style.Border.TopBorder = XLBorderStyleValues.Thin;

                    // Formatos: enteros centrados, importes a la derecha
                    sheet.Column(3).Style.NumberFormat.Format = "#,##0";
                    sheet.Column(5).Style.NumberFormat.Format = "#,##0";
                    sheet.Column(6).Style.NumberFormat.Format = "#,##0";
                    sheet.Column(4).Style.NumberFormat.Format = "$ #,##0.00";
                    sheet.Column(7).Style.NumberFormat.Format = "$ #,##0.00";

                    sheet.Range(renglonEncabezado + 1, 3, renglon, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglonEncabezado + 1, 5, renglon, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglonEncabezado + 1, 6, renglon, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    sheet.Columns().AdjustToContents();
                    sheet.Column(1).Width = 45;   // los nombres de cliente son largos
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