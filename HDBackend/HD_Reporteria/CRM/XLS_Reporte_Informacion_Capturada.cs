using ClosedXML.Excel;
using HD.AccesoDatos;
using HD.Clientes.Modelos.CRM.Reportes;
using HD_Ventas;
using HD_Ventas.Reportes;

namespace HD_Reporteria.CRM
{
    public class XLS_Reporte_Informacion_Capturada
    {
        private static string TextoTipoPersona(string v) =>
            v == "F" ? "Física" : v == "M" ? "Moral" : (v ?? "—");

        private static string TextoValidado(int v) => v == 1 ? "Sí" : "No";

        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_Reporte_Informacion_CapturadaCRM> detalle)
        {
            try
            {
                string sheetname = "INFORMACION CAPTURADA CRM";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, "REPORTE DE INFORMACIÓN CAPTURADA - CRM", 7);

                    sheet.Cell(renglon, 1).Value = "ASESOR";
                    sheet.Cell(renglon, 2).Value = "RFC";
                    sheet.Cell(renglon, 3).Value = "RAZON SOCIAL";
                    sheet.Cell(renglon, 4).Value = "TIPO DE PERSONA";
                    sheet.Cell(renglon, 5).Value = "ETIQUETA";
                    sheet.Cell(renglon, 6).Value = "SUCURSAL";
                    sheet.Cell(renglon, 7).Value = "VALIDADO";

                    var rango = sheet.Range(renglon, 1, renglon, 7);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    foreach (var det in detalle)
                    {
                        sheet.Cell(renglon, 1).Value = det.vendedor;
                        sheet.Cell(renglon, 2).Value = det.rfc;
                        sheet.Cell(renglon, 3).Value = det.razon_social;
                        sheet.Cell(renglon, 4).Value = TextoTipoPersona(det.tipo_persona);
                        sheet.Cell(renglon, 5).Value = det.etiqueta_texto;
                        sheet.Cell(renglon, 6).Value = det.sucursal;
                        sheet.Cell(renglon, 7).Value = TextoValidado(det.validado);
                        renglon++;
                    }

                    sheet.Range(2, 4, renglon - 1, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    sheet.Columns().AdjustToContents();
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
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { errores = ex.Message });
            }
        }
    }
}