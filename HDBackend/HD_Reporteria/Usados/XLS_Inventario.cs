using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Cobranza.Reportes;
using Usados.Modelos;
using DocumentFormat.OpenXml.Bibliography;
using HD_Cobranza.Modelos.Dashboard;
using HD_Ventas;
using Usados.Consultas.Usados;

namespace HD_Reporteria.Usados
{
    public class XLS_Inventario
    {
        public static string obtenersucursal(int idsucursal)
        {
            switch (idsucursal)
            {
                case 1:
                    return "NAVOLATO";
                case 2:
                    return "TEPIC";
                case 11:
                    return "CAIMANERO";
                case 12:
                    return "SAN JOSE";
                case 21:
                    return "ELDORADO";
                case 22:
                    return "SANTIAGO I.";
                case 31:
                    return "COSTA RICA";
                case 32:
                    return "TECUALA";
                case 41:
                    return "LA CRUZ";
                case 42:
                    return "LAS VARAS";
                case 51:
                    return "EL ROSARIO";
                case 52:
                    return "SAN VICENTE";
                case 61:
                    return "VILLA UNION";
                default:
                    return "";

            }
        }
        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_Inventario> detalle)
        {
            try
            {
                var detalleOrdenado = detalle
                .OrderBy(det => det.estatus == "L" ? 0 : det.estatus == "A" ? 1 : 2)
                .ThenBy(det => det.sucursal)
                .ThenBy(det => det.HP)
                .ToList();
                var registrosListos = detalleOrdenado.Where(x => x.estatus.Contains("L"));
                var registrosAcondicionando = detalleOrdenado.Where(x => x.estatus.Contains("A"));
                var registrosTrilladoras = detalleOrdenado
                    .Where(x => x.modelo_descripcion.Contains("trilladora", StringComparison.OrdinalIgnoreCase));
                var registrosTractores = detalleOrdenado
                    .Where(x => x.modelo_descripcion.Contains("tr-", StringComparison.OrdinalIgnoreCase)
                             || x.modelo_descripcion.Contains("tr", StringComparison.OrdinalIgnoreCase)
                                    && !x.modelo_descripcion.Contains("trilladora", StringComparison.OrdinalIgnoreCase)
                                    && !x.modelo_descripcion.Contains("trasn", StringComparison.OrdinalIgnoreCase)
                                    && !x.modelo_descripcion.Contains("trans", StringComparison.OrdinalIgnoreCase)
                             || x.modelo_descripcion.Contains("tractor", StringComparison.OrdinalIgnoreCase));
                var registrosCabezales = detalleOrdenado
                    .Where(x => x.modelo_descripcion.Contains("cabezal", StringComparison.OrdinalIgnoreCase));
                var registrosImplementos = detalleOrdenado.Except(registrosTractores)
                                            .Except(registrosTrilladoras)
                                            .Except(registrosCabezales);
                string sheetname = "INVENTARIO DE SEMINUEVOS";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, "INVENTARIO DE SEMINUEVOS", 17);

                    sheet.Cell(renglon, 1).Value = "SUCURSAL";
                    sheet.Cell(renglon, 2).Value = "N. ECON.";
                    sheet.Cell(renglon, 3).Value = "MARCA";
                    sheet.Cell(renglon, 4).Value = "MODELO";
                    sheet.Cell(renglon, 5).Value = "AÑO";
                    sheet.Cell(renglon, 6).Value = "HP";
                    sheet.Cell(renglon, 7).Value = "SERIE";
                    sheet.Cell(renglon, 8).Value = "HORAS";
                    sheet.Cell(renglon, 9).Value = "RECEPCIÓN";
                    sheet.Cell(renglon, 10).Value = "COSTO";
                    sheet.Cell(renglon, 11).Value = "OT";
                    sheet.Cell(renglon, 12).Value = "COSTO TOTAL";
                    sheet.Cell(renglon, 13).Value = "UTILIDAD";
                    sheet.Cell(renglon, 14).Value = "MARGEN";
                    sheet.Cell(renglon, 15).Value = "PRECIO DE LISTA";
                    sheet.Cell(renglon, 16).Value = "PROMOCION";
                    sheet.Cell(renglon, 17).Value = "ESTADO";

                    // Estilo para los encabezados de la tabla
                    var rango = sheet.Range(renglon, 1, renglon, 17);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    sheet.Cell(renglon, 1).Value = "TRACTORES";
                    sheet.Range(renglon, 1, renglon, 17).Merge();
                    sheet.Range(renglon, 1, renglon, 17).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#dfdfdf"));
                    sheet.Range(renglon, 1, renglon, 17).Style.Font.Bold = true;

                    renglon++;

                    foreach (var det in registrosTractores)
                    {
                        sheet.Cell(renglon, 1).Value = obtenersucursal(det.sucursal);
                        sheet.Cell(renglon, 2).Value = det.NE;
                        sheet.Cell(renglon, 3).Value = det.Marca;
                        sheet.Cell(renglon, 4).Value = det.modelo_descripcion;
                        sheet.Cell(renglon, 5).Value = det.ejercicio;
                        sheet.Cell(renglon, 6).Value = det.HP;
                        sheet.Cell(renglon, 7).Value = det.serie;
                        sheet.Cell(renglon, 8).Value = det.horas;
                        sheet.Cell(renglon, 9).Value = det.fecha_recepcion;
                        sheet.Cell(renglon, 10).Value = det.Costo;
                        sheet.Cell(renglon, 11).Value = det.OT;
                        sheet.Cell(renglon, 12).Value = det.costo_total;
                        sheet.Cell(renglon, 13).Value = det.utilidad;
                        sheet.Cell(renglon, 14).Value = det.margen;
                        sheet.Cell(renglon, 15).Value = det.precio_lista;
                        sheet.Cell(renglon, 16).Value = det.promocion?.ToUpper() + " VIGENCIA: " + det.vigencia ;
                        sheet.Cell(renglon, 17).Value = det.estatus == "A" ? "ACONDICIONANDO" : "LISTO PARA LA VENTA";
                        renglon++;
                    }

                    sheet.Cell(renglon, 9).Value = "TOTAL:";
                    sheet.Cell(renglon, 10).Value = registrosTractores.Sum(det => det.Costo);
                    sheet.Cell(renglon, 11).Value = registrosTractores.Sum(det => det.OT);
                    sheet.Cell(renglon, 12).Value = registrosTractores.Sum(det => det.costo_total);
                    sheet.Cell(renglon, 13).Value = registrosTractores.Sum(det => det.utilidad);
                    sheet.Cell(renglon, 14).Value = (registrosTractores.Sum(det => det.utilidad) / registrosTractores.Sum(det => det.precio_lista)) * 100;
                    sheet.Cell(renglon, 15).Value = registrosTractores.Sum(det => det.precio_lista);

                    renglon++;


                    sheet.Cell(renglon, 1).Value = "TRILLADORAS";
                    sheet.Range(renglon, 1, renglon, 17).Merge();
                    sheet.Range(renglon, 1, renglon, 17).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#dfdfdf"));
                    sheet.Range(renglon, 1, renglon, 17).Style.Font.Bold = true;

                    renglon++;

                    foreach (var det in registrosTrilladoras)
                    {
                        sheet.Cell(renglon, 1).Value = obtenersucursal(det.sucursal);
                        sheet.Cell(renglon, 2).Value = det.NE;
                        sheet.Cell(renglon, 3).Value = det.Marca;
                        sheet.Cell(renglon, 4).Value = det.modelo_descripcion;
                        sheet.Cell(renglon, 5).Value = det.ejercicio;
                        sheet.Cell(renglon, 6).Value = det.HP;
                        sheet.Cell(renglon, 7).Value = det.serie;
                        sheet.Cell(renglon, 8).Value = det.horas;
                        sheet.Cell(renglon, 9).Value = det.fecha_recepcion;
                        sheet.Cell(renglon, 10).Value = det.Costo;
                        sheet.Cell(renglon, 11).Value = det.OT;
                        sheet.Cell(renglon, 12).Value = det.costo_total;
                        sheet.Cell(renglon, 13).Value = det.utilidad;
                        sheet.Cell(renglon, 14).Value = det.margen;
                        sheet.Cell(renglon, 15).Value = det.precio_lista;
                        sheet.Cell(renglon, 16).Value = det.promocion?.ToUpper() + " VIGENCIA: " + det.vigencia;
                        sheet.Cell(renglon, 17).Value = det.estatus == "A" ? "ACONDICIONANDO" : "LISTO PARA LA VENTA";
                        renglon++;
                    }

                    sheet.Cell(renglon, 9).Value = "TOTAL:";
                    sheet.Cell(renglon, 10).Value = registrosTrilladoras.Sum(det => det.Costo);
                    sheet.Cell(renglon, 11).Value = registrosTrilladoras.Sum(det => det.OT);
                    sheet.Cell(renglon, 12).Value = registrosTrilladoras.Sum(det => det.costo_total);
                    sheet.Cell(renglon, 13).Value = registrosTrilladoras.Sum(det => det.utilidad);
                    sheet.Cell(renglon, 14).Value = (registrosTrilladoras.Sum(det => det.utilidad) / registrosTrilladoras.Sum(det => det.precio_lista)) * 100;
                    sheet.Cell(renglon, 15).Value = registrosTrilladoras.Sum(det => det.precio_lista);

                    renglon++;

                    sheet.Cell(renglon, 1).Value = "CABEZALES";
                    sheet.Range(renglon, 1, renglon, 17).Merge();
                    sheet.Range(renglon, 1, renglon, 17).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#dfdfdf"));
                    sheet.Range(renglon, 1, renglon, 17).Style.Font.Bold = true;

                    renglon++;

                    foreach (var det in registrosCabezales)
                    {
                        sheet.Cell(renglon, 1).Value = obtenersucursal(det.sucursal);
                        sheet.Cell(renglon, 2).Value = det.NE;
                        sheet.Cell(renglon, 3).Value = det.Marca;
                        sheet.Cell(renglon, 4).Value = det.modelo_descripcion;
                        sheet.Cell(renglon, 5).Value = det.ejercicio;
                        sheet.Cell(renglon, 6).Value = det.HP;
                        sheet.Cell(renglon, 7).Value = det.serie;
                        sheet.Cell(renglon, 8).Value = det.horas;
                        sheet.Cell(renglon, 9).Value = det.fecha_recepcion;
                        sheet.Cell(renglon, 10).Value = det.Costo;
                        sheet.Cell(renglon, 11).Value = det.OT;
                        sheet.Cell(renglon, 12).Value = det.costo_total;
                        sheet.Cell(renglon, 13).Value = det.utilidad;
                        sheet.Cell(renglon, 14).Value = det.margen;
                        sheet.Cell(renglon, 15).Value = det.precio_lista;
                        sheet.Cell(renglon, 16).Value = det.promocion?.ToUpper() + " VIGENCIA: " + det.vigencia;
                        sheet.Cell(renglon, 17).Value = det.estatus == "A" ? "ACONDICIONANDO" : "LISTO PARA LA VENTA";
                        renglon++;
                    }

                    sheet.Cell(renglon, 9).Value = "TOTAL:";
                    sheet.Cell(renglon, 10).Value = registrosCabezales.Sum(det => det.Costo);
                    sheet.Cell(renglon, 11).Value = registrosCabezales.Sum(det => det.OT);
                    sheet.Cell(renglon, 12).Value = registrosCabezales.Sum(det => det.costo_total);
                    sheet.Cell(renglon, 13).Value = registrosCabezales.Sum(det => det.utilidad);
                    sheet.Cell(renglon, 14).Value = (registrosCabezales.Sum(det => det.utilidad) / registrosCabezales.Sum(det => det.precio_lista)) * 100;
                    sheet.Cell(renglon, 15).Value = registrosCabezales.Sum(det => det.precio_lista);

                    renglon++;

                    sheet.Cell(renglon, 1).Value = "IMPLEMENTOS";
                    sheet.Range(renglon, 1, renglon, 17).Merge();
                    sheet.Range(renglon, 1, renglon, 17).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#dfdfdf"));
                    sheet.Range(renglon, 1, renglon, 17).Style.Font.Bold = true;

                    renglon++;

                    foreach (var det in registrosImplementos)
                    {
                        sheet.Cell(renglon, 1).Value = obtenersucursal(det.sucursal);
                        sheet.Cell(renglon, 2).Value = det.NE;
                        sheet.Cell(renglon, 3).Value = det.Marca;
                        sheet.Cell(renglon, 4).Value = det.modelo_descripcion;
                        sheet.Cell(renglon, 5).Value = det.ejercicio;
                        sheet.Cell(renglon, 6).Value = det.HP;
                        sheet.Cell(renglon, 7).Value = det.serie;
                        sheet.Cell(renglon, 8).Value = det.horas;
                        sheet.Cell(renglon, 9).Value = det.fecha_recepcion;
                        sheet.Cell(renglon, 10).Value = det.Costo;
                        sheet.Cell(renglon, 11).Value = det.OT;
                        sheet.Cell(renglon, 12).Value = det.costo_total;
                        sheet.Cell(renglon, 13).Value = det.utilidad;
                        sheet.Cell(renglon, 14).Value = det.margen;
                        sheet.Cell(renglon, 15).Value = det.precio_lista;
                        sheet.Cell(renglon, 16).Value = det.promocion?.ToUpper() + " VIGENCIA: " + det.vigencia;
                        sheet.Cell(renglon, 17).Value = det.estatus == "A" ? "ACONDICIONANDO" : "LISTO PARA LA VENTA";
                        renglon++;
                    }

                    sheet.Cell(renglon, 9).Value = "TOTAL:";
                    sheet.Cell(renglon, 10).Value = registrosImplementos.Sum(det => det.Costo);
                    sheet.Cell(renglon, 11).Value = registrosImplementos.Sum(det => det.OT);
                    sheet.Cell(renglon, 12).Value = registrosImplementos.Sum(det => det.costo_total);
                    sheet.Cell(renglon, 13).Value = registrosImplementos.Sum(det => det.utilidad);
                    sheet.Cell(renglon, 14).Value = (registrosImplementos.Sum(det => det.utilidad) / registrosImplementos.Sum(det => det.precio_lista)) * 100;
                    sheet.Cell(renglon, 15).Value = registrosImplementos.Sum(det => det.precio_lista);

                    renglon++;

                    sheet.Cell(renglon, 9).Value = "TOTAL GENERAL:";
                    sheet.Cell(renglon, 10).Value = detalleOrdenado.Sum(det => det.Costo);
                    sheet.Cell(renglon, 11).Value = detalleOrdenado.Sum(det => det.OT);
                    sheet.Cell(renglon, 12).Value = detalleOrdenado.Sum(det => det.costo_total);
                    sheet.Cell(renglon, 13).Value = detalleOrdenado.Sum(det => det.utilidad);
                    sheet.Cell(renglon, 14).Value = (detalleOrdenado.Sum(det => det.utilidad) / detalleOrdenado.Sum(det => det.precio_lista)) * 100;
                    sheet.Cell(renglon, 15).Value = detalleOrdenado.Sum(det => det.precio_lista);

                    sheet.Column(3).Style.DateFormat.Format = "dd/MM/yyyy";
                    sheet.Column(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Column(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Column(6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Column(8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Column(9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Column(10).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(11).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(12).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(13).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(14).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(15).Style.NumberFormat.Format = "#,##0.00";
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
