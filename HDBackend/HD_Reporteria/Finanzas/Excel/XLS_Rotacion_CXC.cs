using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Finanzas.Modelos.AntiguedadInventario;
using HD_Finanzas.Modelos.RotacionInventario;
using HD_Ventas;
using HD_Ventas.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Reporteria.Finanzas.Excel
{
    public class XLS_Rotacion_CXC
    {
        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_RotacionCXC> detalle, string? titulo)
        {
            try
            {
                string sheetname = "ROTACION CXC";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, titulo, 8);

                    sheet.Cell(renglon, 1).Value = "LINEA";
                    sheet.Cell(renglon, 2).Value = "FACTURACION DE CREDITO";
                    sheet.Cell(renglon, 3).Value = "CARTERA INICIAL";
                    sheet.Cell(renglon, 4).Value = "CARTERA FINAL";
                    sheet.Cell(renglon, 5).Value = "ROTACION";
                    sheet.Cell(renglon, 6).Value = "GUIA";
                    sheet.Cell(renglon, 7).Value = "GUIA ANUAL";
                    sheet.Cell(renglon, 8).Value = "PERIODO PROMEDIO DE COBRO";


                    // Estilo para los encabezados de la tabla
                    var rango = sheet.Range(renglon, 1, renglon, 8);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    int total = detalle.Count();
                    int actual = 0;

                    // Llenar la tabla con los datos
                    foreach (var det in detalle)
                    {

                        actual++;
                        sheet.Cell(renglon, 1).Value = det.departamento;
                        sheet.Cell(renglon, 2).Value = det.credito;
                        sheet.Cell(renglon, 3).Value = det.saldo_inicial;
                        sheet.Cell(renglon, 4).Value = det.saldo_final;
                        sheet.Cell(renglon, 5).Value = det.rcxc;
                        sheet.Cell(renglon, 6).Value = det.guia;
                        sheet.Cell(renglon, 7).Value = det.guia_anual;
                        sheet.Cell(renglon, 8).Value = det.rcxc == 0 ? 0 : (365m / (decimal)det.rcxc);

                        if (actual == total)
                        {
                            var rangoUltimaFila = sheet.Range(renglon, 1, renglon, 8);

                            rangoUltimaFila.Style.Fill.BackgroundColor = XLColor.LightGray;
                            rangoUltimaFila.Style.Font.Bold = true; 
                        }
                        renglon++;
                    }

                    sheet.Column(2).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(3).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(4).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(8).Style.NumberFormat.Format = "#,##0.00";

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
