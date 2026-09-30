using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;

namespace HD_Cobranza.Reportes
{
    public class XLSCob_ReporteRecuperacionCartera_Detalle
    {
        public static Task<DocResult> CrearReporteRecuperacionCartera(IEnumerable<mdlReporteRecuperacionCartera_Obtener> lista)
        {
            try
            {
                string sheetname = "REPORTE DE RECUPERACION DE CARTERA";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add("COBRANZA");
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, $"REPORTE DE RECUPERACION DE CARTERA", 9);

                    sheet.Cell(renglon, 1).Value = "SUCURSAL";
                    sheet.Cell(renglon, 2).Value = "CODIGO DE CLIENTE";
                    sheet.Cell(renglon, 3).Value = "RAZON SOCIAL";
                    sheet.Cell(renglon, 4).Value = "FACTURA";
                    sheet.Cell(renglon, 5).Value = "IMPORTE";
                    sheet.Cell(renglon, 6).Value = "PAGO";
                    sheet.Cell(renglon, 7).Value = "FECHA";
                    sheet.Cell(renglon, 8).Value = "FECHA DE PAGO";
                    sheet.Cell(renglon, 9).Value = "DIAS";

                    var rango = sheet.Range(renglon, 1, renglon, 9);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    foreach (mdlReporteRecuperacionCartera_Obtener cartera in lista)
                    {
                        sheet.Cell(renglon, 1).Value = cartera.sucursal;
                        sheet.Cell(renglon, 2).Value = cartera.codigocliente;
                        sheet.Cell(renglon, 3).Value = cartera.razonsocial;
                        sheet.Cell(renglon, 4).Value = cartera.factura;
                        sheet.Cell(renglon, 5).Value = cartera.importe;
                        sheet.Cell(renglon, 6).Value = cartera.pago;
                        sheet.Cell(renglon, 7).Value = cartera.fecha;
                        sheet.Cell(renglon, 8).Value = cartera.fechapago;
                        sheet.Cell(renglon, 9).Value = cartera.dias;
                        renglon++;
                    }

                    sheet.Column(5).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";

                    rango = sheet.Range(renglon, 1, renglon, 9);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#e5e6e6");
                    rango.Style.Font.Bold = true;

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
