using ClosedXML.Excel;
using HD.AccesoDatos;
using HD.Clientes.Modelos.CRM.Reportes;
using HD_Ventas;
using HD_Ventas.Reportes;

namespace HD_Reporteria.CRM
{
    public class XLS_Reporte_Geolocalizacion
    {
        private static string TextoGeo(int v) => v == 1 ? "Sí" : "No";

        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_Reporte_GeolocalizacionCRM> detalle)
        {
            try
            {
                string sheetname = "GEOLOCALIZACION CRM";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, "REPORTE DE GEOLOCALIZACIÓN - CRM", 8);

                    sheet.Cell(renglon, 1).Value = "ASESOR";
                    sheet.Cell(renglon, 2).Value = "CLIENTE";
                    sheet.Cell(renglon, 3).Value = "SUCURSAL";
                    sheet.Cell(renglon, 4).Value = "LOCALIDAD";
                    sheet.Cell(renglon, 5).Value = "MUNICIPIO";
                    sheet.Cell(renglon, 6).Value = "ESTADO";
                    sheet.Cell(renglon, 7).Value = "GOOGLE MAPS";
                    sheet.Cell(renglon, 8).Value = "GEOLOCALIZACION";

                    var rango = sheet.Range(renglon, 1, renglon, 8);
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
                        sheet.Cell(renglon, 2).Value = det.razon_social;
                        sheet.Cell(renglon, 3).Value = det.sucursal;
                        sheet.Cell(renglon, 4).Value = det.localidad;
                        sheet.Cell(renglon, 5).Value = det.municipio;
                        sheet.Cell(renglon, 6).Value = det.estado;
                        sheet.Cell(renglon, 7).Value = det.ubicacion;
                        sheet.Cell(renglon, 8).Value = TextoGeo(det.geolocalizacion);
                        renglon++;
                    }

                    sheet.Range(2, 3, renglon - 1, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

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