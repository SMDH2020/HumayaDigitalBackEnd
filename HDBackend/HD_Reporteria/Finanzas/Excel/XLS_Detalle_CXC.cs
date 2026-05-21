using ClosedXML.Excel;
using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Finanzas.Modelos.AntiguedadInventario;
using HD_Finanzas.Modelos.RotacionInventario;
using HD_Ventas;
using HD_Ventas.Reportes;
using System;

namespace HD_Reporteria.Finanzas.Excel
{
    public class XLS_Detalle_CXC
    {
        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_RotacionCXC_Detalle> detalle, string? titulo)
        {
            try
            {
                string sheetname = "DETALLE CXC";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, titulo, 9);

                    sheet.Cell(renglon, 1).Value = "CLIENTE";
                    sheet.Cell(renglon, 2).Value = "SUCURSAL";
                    sheet.Cell(renglon, 3).Value = "DEPARTAMENTO";
                    sheet.Cell(renglon, 4).Value = "FECHA FACTURA";
                    sheet.Cell(renglon, 5).Value = "IMPORTE";
                    //sheet.Cell(renglon, 5).Value = "IVA";
                    //sheet.Cell(renglon, 6).Value = "ABONOS";
                    //sheet.Cell(renglon, 7).Value = "SALDO";
                    sheet.Cell(renglon, 6).Value = "SERIE";
                    sheet.Cell(renglon, 7).Value = "FOLIO";
                    //sheet.Cell(renglon, 8).Value = "DOCUMENTO INTERNO";
                    sheet.Cell(renglon, 8).Value = "DOCUMENTO FACTURA";
                    //sheet.Cell(renglon, 10).Value = "DIAS VENCIDOS";
                    sheet.Cell(renglon, 9).Value = "BATCH";

                    // Estilo para los encabezados de la tabla
                    var rango = sheet.Range(renglon, 1, renglon, 9);
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
                        sheet.Cell(renglon, 1).Value = det.cliente;
                        sheet.Cell(renglon, 2).Value = det.sucursal;
                        sheet.Cell(renglon, 3).Value = det.departamento;
                        sheet.Cell(renglon, 4).Value = det.fecha_factura;
                        sheet.Cell(renglon, 5).Value = det.importe;
                        //sheet.Cell(renglon, 5).Value = det.iva;
                        //sheet.Cell(renglon, 6).Value = det.abonos;
                        //sheet.Cell(renglon, 7).Value = det.saldo;
                        sheet.Cell(renglon, 6).Value = det.serie;
                        sheet.Cell(renglon, 7).Value = det.folio;
                        //sheet.Cell(renglon, 8).Value = det.documento_interno;
                        sheet.Cell(renglon, 8).Value = det.documento_factura;
                        //sheet.Cell(renglon, 10).Value = det.dias_vencido;
                        sheet.Cell(renglon, 9).Value = det.batch;
                        renglon++;
                    }

                    //sheet.Column(4).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(5).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(7).Style.NumberFormat.Format = "#,##0.00";

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
