using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Finanzas.Modelos.RotacionInventario;
using HD_Ventas;
using HD_Ventas.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Reporteria.Finanzas
{
    public class RPT_Rotacion_Inventario_XLS
    {
        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_Rotacion_Inventario> detalle, string? titulo)
        {
            try
            {
                string sheetname = "ROTACION INVENTARIO";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, titulo, 14);


                    sheet.Cell(renglon, 1).Value = "LINEA";
                    sheet.Cell(renglon, 2).Value = "MINIMO";
                    sheet.Cell(renglon, 3).Value = "MAXIMO";
                    sheet.Cell(renglon, 4).Value = "VENTA";
                    sheet.Cell(renglon, 5).Value = "COSTO";
                    sheet.Cell(renglon, 6).Value = "OPTIMO MAX";
                    sheet.Cell(renglon, 7).Value = "OPTIMO MIN";
                    sheet.Cell(renglon, 8).Value = "INVENTARIO";
                    sheet.Cell(renglon, 9).Value = "DIREFENCIA MAX";
                    sheet.Cell(renglon, 10).Value = "DIFERENCIA MIN";
                    sheet.Cell(renglon, 11).Value = "ROTACION";
                    sheet.Cell(renglon, 12).Value = "INVENTARIO MES";
                    sheet.Cell(renglon, 13).Value = "DIREFENCIA MAX MES";
                    sheet.Cell(renglon, 14).Value = "DIFERENCIA MIN MES";


                    // Estilo para los encabezados de la tabla
                    var rango = sheet.Range(renglon, 1, renglon, 14);
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
                        sheet.Cell(renglon, 1).Value = det.linea;
                        sheet.Cell(renglon, 2).Value = det.minimo;
                        sheet.Cell(renglon, 3).Value = det.maximo;
                        sheet.Cell(renglon, 4).Value = det.venta;
                        sheet.Cell(renglon, 5).Value = det.costo;
                        sheet.Cell(renglon, 6).Value = det.opt_maximo;
                        sheet.Cell(renglon, 7).Value = det.opt_minimo;
                        sheet.Cell(renglon, 8).Value = det.inventario;
                        sheet.Cell(renglon, 9).Value = det.dif_maxima;
                        sheet.Cell(renglon, 10).Value = det.dif_minima;
                        sheet.Cell(renglon, 11).Value = det.rotacion;
                        sheet.Cell(renglon, 12).Value = det.inventariomes;
                        sheet.Cell(renglon, 13).Value = det.dif_maxima_mes;
                        sheet.Cell(renglon, 14).Value = det.dif_minima_mes;

                        // 👉 Estilo especial para TOTALES y ROTACION TOTAL
                        if (det.linea == "TOTALES" || det.linea == "ROTACION TOTAL")
                        {
                            var fila = sheet.Range(renglon, 1, renglon, 14);
                            fila.Style.Font.Bold = true;
                            fila.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                        }

                        renglon++;
                    }

                    sheet.Column(4).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(5).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(7).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(8).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(9).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(10).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(11).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(12).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(13).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(14).Style.NumberFormat.Format = "#,##0.00";


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
