using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Reportes
{
    public class XLSFacturacionVencidas
    {
        public static Task<DocResult> CrearExcelTotalCartera(IEnumerable<mdlCob_Total_Cartera_Detalle> lista)
        {
            try
            {
                string sheetname = "TOTAL CARTERA DETALLE";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Arial";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, $"TOTAL CARTERA DETALLE", 9);

                    sheet.Cell(renglon, 1).Value = "ESTADO";
                    sheet.Cell(renglon, 2).Value = "LINEA";
                    sheet.Cell(renglon, 3).Value = "SUCURSAL";
                    sheet.Cell(renglon, 4).Value = "CLIENTE";
                    sheet.Cell(renglon, 5).Value = "DOCUMENTO";
                    sheet.Cell(renglon, 6).Value = "VENCIMIENTO";
                    sheet.Cell(renglon, 7).Value = "DIAS VENCIDO";
                    sheet.Cell(renglon, 8).Value = "IMPORTE";
                    sheet.Cell(renglon, 9).Value = "INTERESES MORATORIOS";
                    sheet.Cell(renglon, 10).Value = "TOTAL";

                    var rango = sheet.Range(renglon, 1, renglon, 10);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    foreach (mdlCob_Total_Cartera_Detalle activos in lista)
                    {
                        sheet.Cell(renglon, 1).Value = activos.estatus;
                        sheet.Cell(renglon, 2).Value = activos.linea;
                        sheet.Cell(renglon, 3).Value = activos.sucursal;
                        sheet.Cell(renglon, 4).Value = string.Concat(activos.idcliente, " - ", activos.razonsocial);
                        sheet.Cell(renglon, 5).Value = activos.documento;
                        sheet.Cell(renglon, 6).Value = activos.vencimiento;
                        sheet.Cell(renglon, 7).Value = activos.diasvencido;
                        sheet.Cell(renglon, 8).Value = activos.saldo;
                        sheet.Cell(renglon, 9).Value = activos.interesbase;
                        sheet.Cell(renglon, 10).Value = activos.importe;
                        renglon++;
                    }
                    sheet.Cell(renglon, 8).FormulaA1 = $"SUBTOTAL(9,G5:G{renglon - 1})";
                    sheet.Cell(renglon, 9).FormulaA1 = $"SUBTOTAL(9,H5:H{renglon - 1})";
                    sheet.Cell(renglon, 10).FormulaA1 = $"SUBTOTAL(9,I5:I{renglon - 1})";

                    sheet.Column(8).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(9).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(10).Style.NumberFormat.Format = "#,##0.00";

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
