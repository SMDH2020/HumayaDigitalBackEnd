using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using HD.AccesoDatos;
using HD_Buro.Modelos;
using HD_Cobranza;
using HD_Cobranza.Reportes;

namespace HD_Reporteria.Buro_Credito
{
    public class XLS_Reporte_Buro_Credito
    {
        public static Task<DocResult> CrearExcel(IEnumerable<mdl_Buro_Credito_Reporte> lista, string periodo, int ejercicio)
        {
            try
            {
                string sheetname = "Buro de Credito " + periodo + " " + ejercicio.ToString();
                sheetname = sheetname.ToUpper();
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, sheetname, 12);
                    sheet.Cell(renglon, 3).Value = "Crédito MHUSA";
                    sheet.Range(renglon, 3, renglon, 6).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Cell(renglon, 7).Value = "Crédito Revolvente";
                    sheet.Range(renglon, 7, renglon, 10).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    sheet.Cell(renglon, 1).Value = "Razón Social";
                    sheet.Range(renglon, 1, renglon + 1, 1).Merge();
                    sheet.Cell(renglon, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    sheet.Cell(renglon, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    sheet.Cell(renglon, 2).Value = "RFC";
                    sheet.Range(renglon, 2, renglon + 1, 2).Merge();
                    sheet.Cell(renglon, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    sheet.Cell(renglon, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Encabezado 11 y 12 con fusión de dos filas
                    sheet.Cell(renglon, 11).Value = "Está Registrado";
                    sheet.Range(renglon, 11, renglon + 1, 11).Merge();
                    sheet.Cell(renglon, 11).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    sheet.Cell(renglon, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    sheet.Cell(renglon, 12).Value = "Tiene Domicilio";
                    sheet.Range(renglon, 12, renglon + 1, 12).Merge();
                    sheet.Cell(renglon, 12).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    sheet.Cell(renglon, 12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    sheet.Cell(renglon + 1, 3).Value = "Facturas";
                    sheet.Cell(renglon + 1, 4).Value = "Vencidas";
                    sheet.Cell(renglon + 1, 5).Value = "Por Vencer";
                    sheet.Cell(renglon + 1, 6).Value = "Saldo";
                    sheet.Cell(renglon + 1, 7).Value = "Facturas";
                    sheet.Cell(renglon + 1, 8).Value = "Vencidas";
                    sheet.Cell(renglon + 1, 9).Value = "Por Vencer";
                    sheet.Cell(renglon + 1, 10).Value = "Saldo";

                    var rango = sheet.Range(renglon, 1, renglon, 12);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    sheet.Columns(1, 12).AdjustToContents();
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;
                    double totalOperacion = 0;
                    double totalRevolvente = 0;

                    foreach (mdl_Buro_Credito_Reporte mdl in lista)
                    {
                        totalOperacion = (mdl.op_vencido + mdl.op_porvencer);
                        totalRevolvente = (mdl.rev_vencido + mdl.rev_porvencer);
                        sheet.Cell(renglon + 1, 1).Value = mdl.razonsocial; // Razon social
                        sheet.Cell(renglon + 1, 2).Value = mdl.rfc; // RFC
                        sheet.Cell(renglon + 1, 3).Value = mdl.op_fac_vencidas; // facturas operacion
                        sheet.Cell(renglon + 1, 4).Value = mdl.op_vencido; // monto vencido operacion
                        sheet.Cell(renglon + 1, 5).Value = mdl.op_porvencer; // monto por vencer operacion
                        sheet.Cell(renglon + 1, 6).Value = totalOperacion; // saldo operacion
                        sheet.Cell(renglon + 1, 7).Value = mdl.rev_fac_vencidas; // facturas en revolvente
                        sheet.Cell(renglon + 1, 8).Value = mdl.rev_vencido; // monto vencdio revolvente
                        sheet.Cell(renglon + 1, 9).Value = mdl.rev_porvencer; // monto por vencer revolvente
                        sheet.Cell(renglon + 1, 10).Value = totalRevolvente; // saldo revolvente
                        sheet.Cell(renglon + 1, 11).Value = mdl.registrado ? "Sí" : "No"; // está registrado
                        sheet.Cell(renglon + 1, 12).Value = mdl.domicilio ? "Sí" : "No"; // tiene domicilio
                        if (mdl.registrado && !mdl.domicilio)
                        {
                            // Fondo amarillo tenue si registrado es "Sí" y domicilio es "No"
                            sheet.Range(renglon + 1, 1, renglon + 1, 12).Style.Fill.BackgroundColor = XLColor.LightYellow;
                        }
                        else if (!mdl.registrado && !mdl.domicilio)
                        {
                            // Fondo rojo tenue si registrado es "No" y domicilio es "No"
                            sheet.Range(renglon + 1, 1, renglon + 1, 12).Style.Fill.BackgroundColor = XLColor.MistyRose;
                        }
                        renglon++;
                    }
                    sheet.Column(4).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(5).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(8).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(9).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(10).Style.NumberFormat.Format = "#,##0.00";
                    renglon -= 1;
                    var group = lista.GroupBy(item => item.rfc);
                    //var suma = lista.Sum(item => item.saldo);
                    //sheet.Cell(renglon, 109).Value = "TS";//SEccion TS
                    //sheet.Cell(renglon, 110).Value = group.Count(); ;//Numero de compañias
                    //sheet.Cell(renglon, 111).Value = suma;//Cantidad


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
