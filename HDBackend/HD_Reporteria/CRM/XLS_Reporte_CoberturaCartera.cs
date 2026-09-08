using ClosedXML.Excel;
using HD.AccesoDatos;
using HD.Clientes.Modelos.CRM.Reportes;
using HD_Ventas;
using HD_Ventas.Reportes;

namespace HD_Reporteria.CRM
{
    public class XLS_Reporte_CoberturaCartera
    {
        private static readonly (string Label, Func<mdl_Reporte_Cobertura_Cartera, int> Asignados, Func<mdl_Reporte_Cobertura_Cartera, int> Visitados)[] Segmentos = new (string, Func<mdl_Reporte_Cobertura_Cartera, int>, Func<mdl_Reporte_Cobertura_Cartera, int>)[]
        {
            ("PEQUEÑO PRODUCTOR", a => a.pequeño_productor_asignados, a => a.pequeño_productor_visitados),
            ("MEDIANO PRODUCTOR", a => a.mediano_productor_asignados, a => a.mediano_productor_visitados),
            ("GRAN PRODUCTOR", a => a.gran_productor_asignados, a => a.gran_productor_visitados),
            ("ESTRATEGICO", a => a.estrategico_asignados, a => a.estrategico_visitados),
            ("CLAVE", a => a.clave_asignados, a => a.clave_visitados),
            ("SIN CULTIVOS", a => a.sin_cultivos_asignados, a => a.sin_cultivos_visitados),
            ("SIN CLASIFICAR", a => a.sin_clasificar_asignados, a => a.sin_clasificar_visitados),
        };

        private static string Porcentaje(int visitado, int asignado) =>
            asignado > 0 ? $"{Math.Min(100, Math.Round((double)visitado / asignado * 100))}%" : "—";

        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_Reporte_Cobertura_Cartera> detalle)
        {
            try
            {
                string sheetname = "COBERTURA DE CARTERA CRM";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int totalColumnas = 3 + Segmentos.Length + 3;
                    int renglon = XLSEncabezado.Encabezado(ref sheet, "REPORTE DE COBERTURA DE CARTERA - CRM", totalColumnas);

                    sheet.Cell(renglon, 1).Value = "VENDEDOR";
                    sheet.Cell(renglon, 2).Value = "SUCURSAL";
                    sheet.Cell(renglon, 3).Value = "DETALLE";
                    int col = 4;
                    foreach (var s in Segmentos)
                        sheet.Cell(renglon, col++).Value = s.Label;
                    sheet.Cell(renglon, col++).Value = "TOTAL";
                    sheet.Cell(renglon, col++).Value = "SIN VISITAR";
                    sheet.Cell(renglon, col++).Value = "% COBERTURA";

                    var rango = sheet.Range(renglon, 1, renglon, col - 1);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    foreach (var a in detalle)
                    {
                        int sinVisitar = a.clientes_asignados - a.clientes_visitados_total;
                        string coberturaAsesor = Porcentaje(a.clientes_visitados_total, a.clientes_asignados);

                        col = 1;
                        sheet.Cell(renglon, col++).Value = a.asesor;
                        sheet.Cell(renglon, col++).Value = a.sucursal;
                        sheet.Cell(renglon, col++).Value = "Cartera asignada";
                        foreach (var s in Segmentos)
                            sheet.Cell(renglon, col++).Value = s.Asignados(a);
                        sheet.Cell(renglon, col++).Value = a.clientes_asignados;
                        sheet.Cell(renglon, col++).Value = sinVisitar;
                        sheet.Cell(renglon, col++).Value = coberturaAsesor;
                        renglon++;

                        col = 3;
                        sheet.Cell(renglon, col++).Value = "Visitada";
                        foreach (var s in Segmentos)
                            sheet.Cell(renglon, col++).Value = s.Visitados(a);
                        sheet.Cell(renglon, col++).Value = a.clientes_visitados_total;
                        renglon++;

                        col = 3;
                        sheet.Cell(renglon, col++).Value = "%";
                        foreach (var s in Segmentos)
                            sheet.Cell(renglon, col++).Value = Porcentaje(s.Visitados(a), s.Asignados(a));
                        sheet.Cell(renglon, col++).Value = coberturaAsesor;
                        renglon++;
                    }

                    sheet.Range(2, 4, renglon - 1, totalColumnas).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

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