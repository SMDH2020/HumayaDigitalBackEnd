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
    public class XLSCob_TotalCartera_Detalle
    {
        public static Task<DocResult> CrearExcel()
        {
            try
            {
                string sheetname = "Facturas por vencer";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Arial";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, $"RESUMEN DE CARTERA DETALLE", 13);

                    sheet.Cell(renglon, 1).Value = "Sucursal";
                    sheet.Cell(renglon, 2).Value = "Más de 90";
                    sheet.Cell(renglon, 3).Value = "Más de 60";
                    sheet.Cell(renglon, 4).Value = "Más de 30";
                    sheet.Cell(renglon, 5).Value = "Más de 15";
                    sheet.Cell(renglon, 6).Value = "De 1 a 15";
                    sheet.Cell(renglon, 7).Value = "Total Vencido";
                    sheet.Cell(renglon, 8).Value = "Por vencer";
                    sheet.Cell(renglon, 9).Value = "Total Cartera";
                    sheet.Cell(renglon, 10).Value = "Saldo a Favor";
                    sheet.Cell(renglon, 11).Value = "Total";
                    sheet.Cell(renglon, 12).Value = "% Vencido";
                    sheet.Cell(renglon, 13).Value = "% Por Vencer";

                    var rango = sheet.Range(renglon, 1, renglon, 13);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;


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
                        filename = "RESUMEN DE CARTERA DETALLE"
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

