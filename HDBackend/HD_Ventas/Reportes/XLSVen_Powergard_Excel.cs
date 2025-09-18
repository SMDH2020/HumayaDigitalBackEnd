using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Ventas.Modelos;
using HD_Ventas.Modelos.PaqueteServicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Reportes
{
    public class XLSVen_Powergard_Excel
    {
        public static string obtenernombre_mes(int numeromes)
        {
            switch (numeromes)
            {
                case 1:
                    return "ENERO";
                case 2:
                    return "FEBRERO";
                case 3:
                    return "MARZO";
                case 4:
                    return "ABRIL";
                case 5:
                    return "MAYO";
                case 6:
                    return "JUNIO";
                case 7:
                    return "JULIO";
                case 8:
                    return "AGOSTO";
                case 9:
                    return "SEPTIEMBRE";
                case 10:
                    return "OCTUBRE";
                case 11:
                    return "NOVIEMBRE";
                case 12:
                    return "DICIEMBRE";
                default:
                    return "";

            }
        }
        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_Powergard_Listado> scorecard, int ejercicio, int mes_actual, int ejercicio_inicio, int periodo_inicio)
        {
            try
            {
                string sheetname = "Powergard";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, $"POWERGARD", 10);

                    //renglon += 1;

                    sheet.Range(renglon, 1, renglon, 10).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");

                    //sheet.Range(renglon, 2, renglon, 4).Merge().Value = obtenernombre_mes(mes_actual);
                    //sheet.Range(renglon, 2, renglon, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    //sheet.Range(renglon, 2, renglon, 4).Style.Font.Bold = true;
                    //sheet.Range(renglon, 2, renglon, 4).Style.Fill.BackgroundColor = XLColor.LightGray;
                    //int rengloncarteratot = renglon;

                    //sheet.Range(renglon, 5, renglon, 7).Merge().Value = obtenernombre_mes(periodo_inicio) + " " + ejercicio_inicio + " A " + obtenernombre_mes(mes_actual) + " " + ejercicio;
                    //sheet.Range(renglon, 5, renglon, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    //sheet.Range(renglon, 5, renglon, 7).Style.Font.Bold = true;
                    //sheet.Range(renglon, 5, renglon, 7).Style.Fill.BackgroundColor = XLColor.LightGray;
                    //int renglonrecuperaciontot = renglon;


                    renglon++;

                    sheet.Cell(renglon, 1).Value = "SUCURSAL";
                    sheet.Cell(renglon, 2).Value = "NOMBRE DEL CLIENTE";
                    sheet.Cell(renglon, 3).Value = "SERIE";
                    sheet.Cell(renglon, 4).Value = "FACTURACION";
                    sheet.Cell(renglon, 5).Value = "COSTO";
                    sheet.Cell(renglon, 6).Value = "INTERNA/EXTERNA";
                    sheet.Cell(renglon, 7).Value = "FECHA FACTURACION";
                    sheet.Cell(renglon, 8).Value = "# OT";
                    sheet.Cell(renglon, 9).Value = "VENDEDORN";
                    sheet.Cell(renglon, 10).Value = "COBERTURA";


                    // Estilo para los encabezados de la tabla
                    var rango = sheet.Range(renglon, 1, renglon, 10);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    // Llenar la tabla con los datos
                    foreach (var sco in scorecard)
                    {
                        sheet.Cell(renglon, 1).Value = sco.sucursal;
                        sheet.Cell(renglon, 2).Value = sco.cliente;
                        sheet.Cell(renglon, 3).Value = sco.serie;
                        sheet.Cell(renglon, 4).Value = sco.facturacion;
                        sheet.Cell(renglon, 5).Value = sco.costo;
                        sheet.Cell(renglon, 6).Value = sco.tipo == "I" ? "Interno" : sco.tipo == "E" ? "Externa" : "";
                        sheet.Cell(renglon, 7).Value = sco.fecha_facturacion.Substring(0, 10);
                        sheet.Cell(renglon, 8).Value = sco.num_ot;
                        sheet.Cell(renglon, 9).Value = sco.vendedor;
                        sheet.Cell(renglon, 10).Value = sco.cobertura;

                        renglon++;
                    }

                    //float totalImporteProyectado = scorecard.Sum(sco => sco.importe_proyectado);
                    //float totalImporte = scorecard.Sum(sco => sco.importe);
                    //float totalImporteProyectadoAcumulado = scorecard.Sum(sco => sco.importe_proyectado_acumulado);
                    //float totalImporteAcumulado = scorecard.Sum(sco => sco.importe_acumulado);

                    //sheet.Cell(renglon, 1).Value = "IMPORTE TOTAL";
                    //sheet.Cell(renglon, 2).Value = totalImporteAcumulado;
                    //sheet.Cell(renglon, 3).Value = totalImporte;
                    //sheet.Cell(renglon, 4).Value = "";
                    //sheet.Cell(renglon, 5).Value = totalImporteProyectadoAcumulado;
                    //sheet.Cell(renglon, 6).Value = totalImporteAcumulado;
                    //sheet.Cell(renglon, 7).Value = "";

                    //sheet.Row(renglon).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(4).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(5).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(7).Style.NumberFormat.Format = "0.0 %";

                    rango = sheet.Range(renglon, 1, renglon, 10);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#e5e6e6");
                    rango.Style.Font.Bold = true;

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
