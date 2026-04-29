using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;

namespace HD_Cobranza.Reportes
{
    public class XLSCob_TotalCartera_Detalle_Cliente
    {
        public static Task<DocResult> CrearExcel(IEnumerable<mdlResumenCartera_Clientes> lista)
        {
            try
            {
                string sheetname = "Facturas por vencer de cliente";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Arial";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, $"RESUMEN DE CARTERA DETALLE POR CLIENTE", 7);

                    sheet.Cell(renglon, 1).Value = "Sucursal";
                    sheet.Cell(renglon, 2).Value = "Documento";
                    sheet.Cell(renglon, 3).Value = "Vencimiento";
                    sheet.Cell(renglon, 4).Value = "Dias";
                    sheet.Cell(renglon, 5).Value = "Importe";
                    sheet.Cell(renglon, 6).Value = "Intereses";
                    sheet.Cell(renglon, 7).Value = "Total";


                    var rango = sheet.Range(renglon, 1, renglon, 7);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;


                    var cliente = lista.GroupBy(item => item.linea).ToList();

                    foreach (var mdl in cliente)
                    {
                        sheet.Cell(renglon, 1).Value = mdl.Key;
                        rango = sheet.Range(renglon, 1, renglon, 7);
                        rango.Style.Fill.BackgroundColor = XLColor.FromArgb(218, 230, 190);
                        rango.Style.Font.Bold = true;
                        rango.Style.Font.FontSize = 10;
                        //rango.RangeUsed().SetAutoFilter();
                        rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        renglon++;
                        foreach (mdlResumenCartera_Clientes activos in lista.Where(item => item.linea == mdl.Key))
                        {
                            sheet.Cell(renglon, 1).Value = activos.sucursal;
                            sheet.Cell(renglon, 2).Value = activos.documento;
                            sheet.Cell(renglon, 3).Value = activos.vencimiento;
                            sheet.Cell(renglon, 4).Value = activos.diasvencido;
                            sheet.Cell(renglon, 5).Value = activos.saldo;
                            sheet.Cell(renglon, 6).Value = activos.interesbase;
                            sheet.Cell(renglon, 7).Value = activos.importe;
                            renglon++;
                        }

                    }


                    rango = sheet.Range(renglon - 1, 1, renglon - 1, 7);
                    rango.Style.Font.Bold = true;
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#e5e6e6");

                    //sheet.Column(2).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(3).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(4).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(5).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(7).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(8).Style.NumberFormat.Format = "0.0 %";
                    //sheet.Column(9).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(10).Style.NumberFormat.Format = "0.0 %";
                    //sheet.Column(11).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(12).Style.NumberFormat.Format = "0.0 %";
                    //sheet.Column(13).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(14).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(15).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(16).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(17).Style.NumberFormat.Format = "#,##0.00";

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
                        filename = "RESUMEN DE CARTERA DETALLE POR CLIENTE"
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

