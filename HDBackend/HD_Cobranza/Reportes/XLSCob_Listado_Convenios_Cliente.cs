using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Cobranza.Reportes;
using HD_Cobranza.Modelos;
using DocumentFormat.OpenXml.Bibliography;
using HD_Cobranza.GestionCobranza.Modelos;

namespace HD_Cobranza.Reportes
{
    public class XLSCob_Listado_Convenios_Cliente
    {
        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_Listado_Convenios_Cliente> detalle)
        {
            try
            {
                string sheetname = "CONVENIOS REALIZADOS";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, $"BITACORA DE CONVENIOS REALIZADOS A {detalle.FirstOrDefault().razon_social}", 6);

                    sheet.Cell(renglon, 1).Value = "SUCURSAL";
                    sheet.Cell(renglon, 2).Value = "RAZON SOCIAL";
                    sheet.Cell(renglon, 3).Value = "SALDO";
                    sheet.Cell(renglon, 4).Value = "MONTO DE CONVENIO";
                    sheet.Cell(renglon, 5).Value = "VENCIMIENTO DE CONVENIO";
                    sheet.Cell(renglon, 6).Value = "RESPONSABLE";

                    // Estilo para los encabezados de la tabla
                    var rango = sheet.Range(renglon, 1, renglon, 6);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    // Llenar la tabla con los datos
                    foreach (var det in detalle)
                    {
                        sheet.Cell(renglon, 1).Value = det.sucursal;
                        sheet.Cell(renglon, 2).Value = det.razon_social?.ToUpper();
                        sheet.Cell(renglon, 3).Value = det.saldo;
                        sheet.Cell(renglon, 4).Value = det.monto;
                        sheet.Cell(renglon, 5).Value = det.fecha_convenio;
                        sheet.Cell(renglon, 6).Value = det.NombreCompleto?.ToUpper();
                        renglon++;
                    }

                    sheet.Column(3).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(4).Style.NumberFormat.Format = "#,##0.00";

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
