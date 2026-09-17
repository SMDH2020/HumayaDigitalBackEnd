using ClosedXML.Excel;
using HD.AccesoDatos;
using HD.Clientes.Modelos.CRM.Reportes;
using HD_Ventas;
using HD_Ventas.Reportes;

namespace HD_Reporteria.CRM
{
    public class XLS_Reporte_CoberturaVisitas
    {
        private static readonly string[] MESES = { "ENE", "FEB", "MAR", "ABR", "MAY", "JUN", "JUL", "AGO", "SEP", "OCT", "NOV", "DIC" };

        private static readonly Func<mdl_Reporte_Cobertura_Visitas_CRM, int>[] CamposMes = new Func<mdl_Reporte_Cobertura_Visitas_CRM, int>[]
        {
            a => a.enero, a => a.febrero, a => a.marzo, a => a.abril, a => a.mayo, a => a.junio,
            a => a.julio, a => a.agosto, a => a.septiembre, a => a.octubre, a => a.noviembre, a => a.diciembre,
        };

        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_Reporte_Cobertura_Visitas_CRM> detalle)
        {
            try
            {
                string sheetname = "COBERTURA DE VISITAS CRM";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int totalColumnas = 6 + MESES.Length + 1;
                    int renglon = XLSEncabezado.Encabezado(ref sheet, "REPORTE DE COBERTURA DE VISITAS - CRM", totalColumnas);

                    sheet.Cell(renglon, 1).Value = "ASESOR";
                    sheet.Cell(renglon, 2).Value = "CLIENTE";
                    sheet.Cell(renglon, 3).Value = "SUCURSAL";
                    sheet.Cell(renglon, 4).Value = "ESTADO";
                    sheet.Cell(renglon, 5).Value = "GIRO";
                    sheet.Cell(renglon, 6).Value = "LOCALIDAD";
                    int col = 7;
                    foreach (var m in MESES)
                        sheet.Cell(renglon, col++).Value = m;
                    sheet.Cell(renglon, col++).Value = "N° VISITAS";

                    var rango = sheet.Range(renglon, 1, renglon, col - 1);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    foreach (var det in detalle)
                    {
                        col = 1;
                        sheet.Cell(renglon, col++).Value = det.vendedor;
                        sheet.Cell(renglon, col++).Value = det.razon_social;
                        sheet.Cell(renglon, col++).Value = det.sucursal;
                        sheet.Cell(renglon, col++).Value = det.estado;
                        sheet.Cell(renglon, col++).Value = det.giros;
                        sheet.Cell(renglon, col++).Value = det.localidad;
                        foreach (var campo in CamposMes)
                            sheet.Cell(renglon, col++).Value = campo(det);
                        sheet.Cell(renglon, col++).Value = det.total_visitas;
                        renglon++;
                    }

                    sheet.Range(2, 7, renglon - 1, totalColumnas).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

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