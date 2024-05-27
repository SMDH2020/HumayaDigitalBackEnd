using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Buro.Modelos;
using HD_Cobranza;
using HD_Cobranza.Modelos;
using HD_Cobranza.Reportes;

namespace HD_Reporteria.Buro_Credito
{
    public class XLS_Reporte_Buro
    {
        public static Task<DocResult> CrearExcel(IEnumerable<mdlCarga_Reporte_Buro> lista)
        {
            try
            {
                string sheetname = "Facturas por vencer";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, "REPORTE BURO", 11);
                    sheet.Cell(renglon, 1).Value = "LINEA";
                    sheet.Cell(renglon, 2).Value = "SUCURSAL";
                    sheet.Cell(renglon, 3).Value = "IDCLIENTE";
                    sheet.Cell(renglon, 4).Value = "RAZON SOCIAL";
                    sheet.Cell(renglon, 5).Value = "RFC";
                    sheet.Cell(renglon, 6).Value = "TOTAL DE FACTURAS";
                    sheet.Cell(renglon, 7).Value = "FACTURAS VENCIDAS";
                    sheet.Cell(renglon, 8).Value = "FACTURAS POR VENCER";
                    sheet.Cell(renglon, 9).Value = "SALDO";
                    sheet.Cell(renglon, 10).Value = "REGISTRADO";
                    sheet.Cell(renglon, 11).Value = "CUENTA CON DOMICILIO";


                    var rango = sheet.Range(renglon, 1, renglon, 11);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;


                    foreach (mdlCarga_Reporte_Buro activos in lista)
                    {
                        sheet.Cell(renglon, 1).Value = activos.linea;
                        sheet.Cell(renglon, 2).Value = activos.sucursal;
                        sheet.Cell(renglon, 3).Value = activos.idcliente;
                        sheet.Cell(renglon, 4).Value = activos.razon_social;
                        sheet.Cell(renglon, 5).Value = activos.rfc;
                        sheet.Cell(renglon, 6).Value = activos.totalfacturas;
                        sheet.Cell(renglon, 7).Value = activos.vencidas;
                        sheet.Cell(renglon, 8).Value = activos.porvencer;
                        sheet.Cell(renglon, 9).Value = activos.saldo;
                        sheet.Cell(renglon, 10).Value = activos.registrado ? "SI" : "NO";
                        sheet.Cell(renglon, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        sheet.Cell(renglon, 11).Value = activos.tiene_domicilio ? "SI" : "NO";
                        sheet.Cell(renglon, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        renglon++;
                    }

                    sheet.Column(9).Style.NumberFormat.Format = "#,##0.00";

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
                        filename = "REPORTE BURO"
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
