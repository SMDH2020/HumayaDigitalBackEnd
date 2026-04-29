using ClosedXML.Excel;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using DocumentFormat.OpenXml.Bibliography;
using HD.Clientes.Consultas.ClientesNoRegistrados;
using HD_Cobranza;
using HD_Cobranza.Reportes;
using HD.Clientes.Modelos.Especiales;

namespace HD_Reporteria.Credito
{
    public class XLSCre_Listado_Clientes_Especiales
    {
        public static Task<DocResult> GenerarExcel(IEnumerable<mdlClientesespecialesList> detalle)
        {
            try
            {
                string sheetname = "CLIENTES ESPECIALES";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, $"LISTADO DE CLIENTES ESPECIALES", 5);


                    sheet.Cell(renglon, 1).Value = "RAZON SOCIAL";
                    sheet.Cell(renglon, 2).Value = "COMENTARIOS";
                    sheet.Cell(renglon, 3).Value = "TIPO DE CLIENTE";
                    sheet.Cell(renglon, 4).Value = "ESTADO";
                    sheet.Cell(renglon, 5).Value = "EMPLEADO";

                    // Estilo para los encabezados de la tabla
                    var rango = sheet.Range(renglon, 1, renglon, 5);
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
                        sheet.Cell(renglon, 1).Value = det.razon_social?.ToUpper();
                        sheet.Cell(renglon, 2).Value = det.comentarios?.ToUpper();
                        sheet.Cell(renglon, 3).Value = det.tipo_cliente?.ToUpper();
                        sheet.Cell(renglon, 4).Value = det.estatus?.ToUpper();
                        sheet.Cell(renglon, 5).Value = det.empleado?.ToUpper();
                        renglon++;
                    }

                    //sheet.Column(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

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
