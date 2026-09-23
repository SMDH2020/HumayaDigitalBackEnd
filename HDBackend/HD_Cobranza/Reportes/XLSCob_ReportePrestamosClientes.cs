using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Cobranza.Modelos.ReportePrestamosClientes;

namespace HD_Cobranza.Reportes
{
    public class XLSCob_ReportePrestamosClientes
    {
        public static Task<DocResult> CrearExcel(IEnumerable<mdlReportePrestamosClientes> lista)
        {
            try
            {
                string sheetname = "REPORTE DE PRESTAMOS CLIENTES";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add("PRESTAMOS");
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, $"REPORTE DE PRESTAMOS CLIENTES", 11);

                    sheet.Cell(renglon, 1).Value = "ORIGEN";
                    sheet.Cell(renglon, 2).Value = "DOCUMENTO";
                    sheet.Cell(renglon, 3).Value = "SERIE";
                    sheet.Cell(renglon, 4).Value = "RAZON SOCIAL";
                    sheet.Cell(renglon, 5).Value = "RFC";
                    sheet.Cell(renglon, 6).Value = "SUCURSAL";
                    sheet.Cell(renglon, 7).Value = "VENCIMIENTO";
                    sheet.Cell(renglon, 8).Value = "IMPORTE FACTURA";
                    sheet.Cell(renglon, 9).Value = "IMPORTE PAGADO";
                    sheet.Cell(renglon, 10).Value = "SALDO";
                    sheet.Cell(renglon, 11).Value = "FOLIO SOLICITUD";

                    var rango = sheet.Range(renglon, 1, renglon, 11);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    foreach (mdlReportePrestamosClientes prestamo in lista)
                    {
                        sheet.Cell(renglon, 1).Value = prestamo.origen_registro;
                        sheet.Cell(renglon, 2).Value = prestamo.documento;
                        sheet.Cell(renglon, 3).Value = SerieFolio(prestamo.serie, prestamo.folio);
                        sheet.Cell(renglon, 4).Value = prestamo.razon_social;
                        sheet.Cell(renglon, 5).Value = prestamo.rfc;
                        sheet.Cell(renglon, 6).Value = prestamo.sucursal;

                        // Las filas de SOLICITUD SIN FACTURACION no traen datos de
                        // facturacion: esas celdas se dejan vacias, no en cero.
                        if (prestamo.vencimiento.HasValue)
                            sheet.Cell(renglon, 7).Value = prestamo.vencimiento.Value;
                        if (prestamo.importe_factura.HasValue)
                            sheet.Cell(renglon, 8).Value = prestamo.importe_factura.Value;
                        if (prestamo.importe_pagado.HasValue)
                            sheet.Cell(renglon, 9).Value = prestamo.importe_pagado.Value;
                        if (prestamo.saldo.HasValue)
                            sheet.Cell(renglon, 10).Value = prestamo.saldo.Value;

                        sheet.Cell(renglon, 11).Value = prestamo.folio_solicitud;
                        renglon++;
                    }

                    renglon--;
                    if (renglon > 5)
                    {
                        renglon++;
                        sheet.Cell(renglon, 6).Value = "TOTALES";
                        sheet.Cell(renglon, 8).FormulaA1 = $"SUBTOTAL(9,H5:H{renglon - 1})";
                        sheet.Cell(renglon, 9).FormulaA1 = $"SUBTOTAL(9,I5:I{renglon - 1})";
                        sheet.Cell(renglon, 10).FormulaA1 = $"SUBTOTAL(9,J5:J{renglon - 1})";

                        rango = sheet.Range(renglon, 1, renglon, 11);
                        rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#e5e6e6");
                        rango.Style.Font.Bold = true;
                    }

                    sheet.Column(7).Style.DateFormat.Format = "dd/MM/yyyy";
                    sheet.Column(8).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(9).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(10).Style.NumberFormat.Format = "#,##0.00";

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

        // Serie y folio se muestran juntos: "[serie] - [folio]".
        // Si solo viene uno de los dos, se pinta ese sin el guion.
        private static string SerieFolio(string? serie, string? folio)
        {
            bool haySerie = !string.IsNullOrWhiteSpace(serie);
            bool hayFolio = !string.IsNullOrWhiteSpace(folio);

            if (haySerie && hayFolio) return serie.Trim() + " - " + folio.Trim();
            if (haySerie) return serie.Trim();
            if (hayFolio) return folio.Trim();
            return "";
        }
    }
}
