using ClosedXML.Excel;
using HD.AccesoDatos;
using HD.Clientes.Modelos.CRM.Reportes;
using HD.Clientes.Modelos.CRM.Visitas;
using HD_Ventas;
using HD_Ventas.Reportes; // quita este using si el modelo no está aquí

namespace HD_Reporteria.CRM
{
    public class XLS_Reporte_Visitas
    {
        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_Reporte_Visitas_ProgramadasCRM> detalle)
        {
            try
            {
                string sheetname = "VISITAS PROGRAMADAS CRM";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, "REPORTE DE VISITAS PROGRAMADAS - CRM", 8);

                    sheet.Cell(renglon, 1).Value = "ASESOR";
                    sheet.Cell(renglon, 2).Value = "CLIENTE";
                    sheet.Cell(renglon, 3).Value = "FECHA DE CREACION";
                    sheet.Cell(renglon, 4).Value = "FECHA DE VISITA";
                    sheet.Cell(renglon, 5).Value = "TIPO DE VISITA";
                    sheet.Cell(renglon, 6).Value = "LINEA";
                    sheet.Cell(renglon, 7).Value = "ESTATUS";
                    sheet.Cell(renglon, 8).Value = "COMENTARIOS";

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
                        sheet.Cell(renglon, 3).Value = string.IsNullOrEmpty(det.createdate) ? "—" : det.createdate;
                        sheet.Cell(renglon, 4).Value = string.IsNullOrEmpty(det.fecha_visita) ? "—" : det.fecha_visita;
                        sheet.Cell(renglon, 5).Value = det.visita;
                        sheet.Cell(renglon, 6).Value = det.linea;
                        sheet.Cell(renglon, 7).Value = det.estatus_texto;
                        sheet.Cell(renglon, 8).Value = det.comentario;
                        renglon++;
                    }

                    sheet.Range(2, 3, renglon - 1, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

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