using HD.AccesoDatos;
using ClosedXML.Excel;
using HD.Fiscal.Modelos;

namespace HD.Fiscal.Reportes
{
    public class XLS_Conciliacion_Ingresos_Analitica
    {
        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_Conciliacion_Ingresos_Analitica> detalle, string? titulo, string origen)
        {
            try
            {
                bool esAnalitica = origen?.ToUpper() == "A";

                string sheetname = "CONC. DE ING. ANALITICA";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    // Calcular total de columnas según origen
                    int totalColumnas = esAnalitica ? 28 : 25;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, titulo, totalColumnas);

                    // ── Columnas fijas (1–7) ──────────────────────────────────────
                    sheet.Cell(renglon, 1).Value = "ORIGEN";
                    sheet.Cell(renglon, 2).Value = "SUCURSAL";
                    sheet.Cell(renglon, 3).Value = "DEPARTAMENTO";
                    sheet.Cell(renglon, 4).Value = "CUENTA";
                    sheet.Cell(renglon, 5).Value = "GL. DESC";
                    sheet.Cell(renglon, 6).Value = "CARGOS";
                    sheet.Cell(renglon, 7).Value = "ABONOS";

                    // ── Columna 8: TASA (solo si origen = 'A') ───────────────────
                    int colTasa = -1;
                    int colGlMain = 8;
                    int colFecha = 9;
                    int colBatch = 10;
                    int colDocumento = 11;
                    int colSerie = 12;
                    int colFolio = 13;
                    int colFechaCan = 14;
                    int colUUID = 15;
                    int colRelacion = -1;
                    int colFechaRelacion = -1;
                    int colEstado = 16;
                    int colTipoComp = 17;
                    int colRFC = 18;
                    int colCondPago = 19;
                    int colDesc = 20;
                    int colRef = 21;
                    int colUsuario = 22;
                    int colOrigInvoice = 23;
                    int colDocRefact = 24;
                    int colEquip = 25;

                    if (esAnalitica)
                    {
                        // Insertar TASA en col 8, desplazar todo +1
                        colTasa = 8;
                        colGlMain = 9;
                        colFecha = 10;
                        colBatch = 11;
                        colDocumento = 12;
                        colSerie = 13;
                        colFolio = 14;
                        colFechaCan = 15;
                        colUUID = 16;
                        // Insertar RELACION y FECHA DE RELACION después de UUID, desplazar +2
                        colRelacion = 17;
                        colFechaRelacion = 18;
                        colEstado = 19;
                        colTipoComp = 20;
                        colRFC = 21;
                        colCondPago = 22;
                        colDesc = 23;
                        colRef = 24;
                        colUsuario = 25;
                        colOrigInvoice = 26;
                        colDocRefact = 27;
                        colEquip = 28;
                    }

                    // ── Encabezados ───────────────────────────────────────────────
                    if (esAnalitica) sheet.Cell(renglon, colTasa).Value = "TASA";

                    sheet.Cell(renglon, colGlMain).Value = "GL. MAIN";
                    sheet.Cell(renglon, colFecha).Value = "FECHA";
                    sheet.Cell(renglon, colBatch).Value = "BATCH";
                    sheet.Cell(renglon, colDocumento).Value = "DOCUMENTO";
                    sheet.Cell(renglon, colSerie).Value = "SERIE FISCAL";
                    sheet.Cell(renglon, colFolio).Value = "FOLIO FISCAL";
                    sheet.Cell(renglon, colFechaCan).Value = "FECHA DE CANCELACION";
                    sheet.Cell(renglon, colUUID).Value = "UUID";

                    if (esAnalitica)
                    {
                        sheet.Cell(renglon, colRelacion).Value = "RELACION";
                        sheet.Cell(renglon, colFechaRelacion).Value = "FECHA DE RELACION";
                    }

                    sheet.Cell(renglon, colEstado).Value = "ESTADO";
                    sheet.Cell(renglon, colTipoComp).Value = "TIPO DE COMPROBANTE";
                    sheet.Cell(renglon, colRFC).Value = "RFC";
                    sheet.Cell(renglon, colCondPago).Value = "CONDICION DE PAGO";
                    sheet.Cell(renglon, colDesc).Value = "DESC.";
                    sheet.Cell(renglon, colRef).Value = "REF.";
                    sheet.Cell(renglon, colUsuario).Value = "USUARIO";
                    sheet.Cell(renglon, colOrigInvoice).Value = "ORIG. INVOICE NO.";
                    sheet.Cell(renglon, colDocRefact).Value = "DOCUMENTO DE REFACTURACION";
                    sheet.Cell(renglon, colEquip).Value = "EQUIP";

                    // ── Estilo encabezados ────────────────────────────────────────
                    var rango = sheet.Range(renglon, 1, renglon, totalColumnas);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    float totalCargos = 0;
                    float totalAbonos = 0;

                    // ── Datos ─────────────────────────────────────────────────────
                    foreach (var det in detalle)
                    {
                        sheet.Cell(renglon, 1).Value = det.origen?.ToUpper();
                        sheet.Cell(renglon, 2).Value = det.sucursal?.ToUpper();
                        sheet.Cell(renglon, 3).Value = det.departamento?.ToUpper();
                        sheet.Cell(renglon, 4).Value = det.cuenta;
                        sheet.Cell(renglon, 5).Value = det.v_gl_desc?.ToUpper();
                        sheet.Cell(renglon, 6).Value = det.v_cargos;
                        sheet.Cell(renglon, 7).Value = det.v_abonos;

                        if (esAnalitica) sheet.Cell(renglon, colTasa).Value = det.tasa;

                        sheet.Cell(renglon, colGlMain).Value = det.v_gl_main;
                        sheet.Cell(renglon, colFecha).Value = det.v_fecha;
                        sheet.Cell(renglon, colBatch).Value = det.v_batch;
                        sheet.Cell(renglon, colDocumento).Value = det.document_no;
                        sheet.Cell(renglon, colSerie).Value = det.serie;
                        sheet.Cell(renglon, colFolio).Value = det.folio;
                        sheet.Cell(renglon, colFechaCan).Value = det.fechacancelacion;
                        sheet.Cell(renglon, colUUID).Value = det.uuid;

                        if (esAnalitica)
                        {
                            sheet.Cell(renglon, colRelacion).Value = det.v_relacion;
                            sheet.Cell(renglon, colFechaRelacion).Value = det.v_fecha_relacion;
                        }

                        sheet.Cell(renglon, colEstado).Value = det.estatus?.ToUpper();
                        sheet.Cell(renglon, colTipoComp).Value = det.tipoComprobante?.ToUpper();
                        sheet.Cell(renglon, colRFC).Value = det.rfc;
                        sheet.Cell(renglon, colCondPago).Value = det.condicionPago;
                        sheet.Cell(renglon, colDesc).Value = det.v_desc?.ToUpper();
                        sheet.Cell(renglon, colRef).Value = det.v_ref;
                        sheet.Cell(renglon, colUsuario).Value = det.v_usuario;
                        sheet.Cell(renglon, colOrigInvoice).Value = det.orig_invoice_no;
                        sheet.Cell(renglon, colDocRefact).Value = det.document_refacturacion;
                        sheet.Cell(renglon, colEquip).Value = det.equip?.ToUpper();

                        totalCargos += det.v_cargos;
                        totalAbonos += det.v_abonos;
                        renglon++;
                    }

                    // ── Totales ───────────────────────────────────────────────────
                    float diferencia = totalAbonos - totalCargos;

                    sheet.Cell(renglon, 6).Value = totalCargos;
                    sheet.Cell(renglon, 7).Value = totalAbonos;
                    var totalRange = sheet.Range(renglon, 1, renglon, totalColumnas);
                    totalRange.Style.Font.Bold = true;
                    totalRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    totalRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    renglon++;
                    sheet.Cell(renglon, 6).Value = "DIFERENCIA:";
                    sheet.Cell(renglon, 7).Value = diferencia;
                    var diferenciaRange = sheet.Range(renglon, 1, renglon, totalColumnas);
                    diferenciaRange.Style.Font.Bold = true;
                    diferenciaRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    diferenciaRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(7).Style.NumberFormat.Format = "#,##0.00";

                    sheet.Columns().AdjustToContents();
                    workbook.SaveAs(ruta);
                }

                if (System.IO.File.Exists(ruta))
                {
                    byte[] docbytes = System.IO.File.ReadAllBytes(ruta);
                    string docBase64 = Convert.ToBase64String(docbytes);
                    System.IO.File.Delete(ruta);
                    return Task.FromResult(new DocResult
                    {
                        documento = docBase64,
                        filename = sheetname
                    });
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