using ClosedXML.Excel;
using HD.AccesoDatos;
using HD.Clientes.Modelos.CRM.Proceso6Contactos;
using HD_Ventas;
using HD_Ventas.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Reportes
{
    public class XLSVen_Proceso_6Contactos_Listado
    {
        public static Task<DocResult> GenerarExcel(
            IEnumerable<mdl_Proceso_6Contactos_Listado> detalle, string? titulo)
        {
            try
            {
                string sheetname = "LISTADO PROCESO 6 CONTACTOS";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";

                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, titulo, 10);

                    sheet.Cell(renglon, 1).Value = "FOLIO";
                    //sheet.Cell(renglon, 2).Value = "ORDEN";
                    sheet.Cell(renglon, 2).Value = "CLIENTE";
                    sheet.Cell(renglon, 3).Value = "TELEFONO";
                    sheet.Cell(renglon, 4).Value = "LINEA";
                    sheet.Cell(renglon, 5).Value = "MODELO";
                    sheet.Cell(renglon, 6).Value = "FASE";
                    sheet.Cell(renglon, 7).Value = "FECHA DE ENTREGA";
                    sheet.Cell(renglon, 8).Value = "FECHA DE CONTACTO";
                    sheet.Cell(renglon, 9).Value = "REALIZADO";
                    sheet.Cell(renglon, 10).Value = "COMENTARIOS";

                    var rango = sheet.Range(renglon, 1, renglon, 10);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    foreach (var det in detalle)
                    {
                        sheet.Cell(renglon, 1).Value = det.folio;
                        //sheet.Cell(renglon, 2).Value = det.orden;
                        sheet.Cell(renglon, 2).Value = det.razon_social?.ToUpper();
                        sheet.Cell(renglon, 3).Value = det.telefono;
                        sheet.Cell(renglon, 4).Value = det.linea;
                        sheet.Cell(renglon, 5).Value = det.modelo;
                        sheet.Cell(renglon, 6).Value = det.fase;
                        sheet.Cell(renglon, 7).Value = det.fecha_entrega;
                        sheet.Cell(renglon, 8).Value = det.fecha_contacto;
                        sheet.Cell(renglon, 9).Value = det.realizado == true ? "SI" : "NO";
                        sheet.Cell(renglon, 10).Value = det.comentarios;
                        renglon++;
                    }

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
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { errores = ex.Message });
            }
        }
    }
}
