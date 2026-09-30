using ClosedXML.Excel;
using HD.AccesoDatos;
using HD.Clientes.Modelos.CRM.Parque_Maquinaria;
using HD_Ventas;
using HD_Ventas.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Reporteria.CRM
{
    public class XLS_Listado_Parque_Maquinaria
    {
        private static string TextoCategoria(string v)
        {
            if (string.IsNullOrWhiteSpace(v))
                return "-";

            switch (v.Trim().ToUpperInvariant())
            {
                case "JD":
                    return "JOHN DEERE";
                case "OTRA_MARCA":
                    return "OTRAS MARCAS";
                default:
                    return v;
            }
        }

        private static string TextoAnio(int? v) => v.HasValue ? v.Value.ToString() : "-";

        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_Listado_Parque_MaquinariaCRM> detalle)
        {
            try
            {
                string sheetname = "PARQUE DE MAQUINARIA CRM";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, "REPORTE DE PARQUE DE MAQUINARIA - CRM", 8);

                    sheet.Cell(renglon, 1).Value = "RAZON SOCIAL";
                    sheet.Cell(renglon, 2).Value = "CATEGORIA";
                    sheet.Cell(renglon, 3).Value = "TIPO";
                    sheet.Cell(renglon, 4).Value = "MARCA";
                    sheet.Cell(renglon, 5).Value = "MODELO";
                    sheet.Cell(renglon, 6).Value = "SERIE";
                    sheet.Cell(renglon, 7).Value = "AÑO";
                    sheet.Cell(renglon, 8).Value = "COMENTARIOS";

                    var rango = sheet.Range(renglon, 1, renglon, 8);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    foreach (var det in detalle)
                    {
                        sheet.Cell(renglon, 1).Value = det.razon_social;
                        sheet.Cell(renglon, 2).Value = TextoCategoria(det.categoria);
                        sheet.Cell(renglon, 3).Value = det.tipo;
                        sheet.Cell(renglon, 4).Value = det.marca;
                        sheet.Cell(renglon, 5).Value = det.modelo;
                        sheet.Cell(renglon, 6).Value = det.serie;
                        sheet.Cell(renglon, 7).Value = TextoAnio(det.anio);
                        sheet.Cell(renglon, 8).Value = det.comentarios;
                        renglon++;
                    }

                    sheet.Range(2, 2, renglon - 1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(2, 7, renglon - 1, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

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
