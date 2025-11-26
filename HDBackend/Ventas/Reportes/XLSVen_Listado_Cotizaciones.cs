using ClosedXML.Excel;
using HD.AccesoDatos;
using Ventas.Reportes;
using Ventas.Modelos;
using DocumentFormat.OpenXml.Bibliography;
using Ventas.Modelos.CotizacionesVentas;

namespace Ventas.Reportes
{
    public class XLSVen_Listado_Cotizaciones
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



        public static string ObtenerNombreFase(string fase)
        {
            switch (fase.Substring(0,2))
            {
                case "PR": return "PROMOCION";
                case "IN": return "INTERES";
                case "NE": return "NEGOCIACION";
                case "CE": return "CERRADA";
                case "FM": return "FACTURAR MES";
                case "FS": return "FACTURAR SEMANA";
                case "NC": return "NO COMPRO";
                case "VP": return "VENTA PERDIDA";
                case "VN": return "VENDIDO";
                default: return "FASE NO RECONOCIDA";
            }
        }

        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_Listado_Cotizaciones_Nuevo> detalle, string? titulo)
        {
            try
            {
                string sheetname = "COTIZACIONES REALIZADAS";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, titulo, 13);

                    //renglon += 1;

                    //sheet.Range(renglon, 1, renglon, 13).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");

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


                    //renglon++;

                    sheet.Cell(renglon, 1).Value = "FASE DE COTIZACION";
                    sheet.Cell(renglon, 2).Value = "CLIENTE";
                    sheet.Cell(renglon, 3).Value = "ASESOR";
                    sheet.Cell(renglon, 4).Value = "SUCURSAL";
                    sheet.Cell(renglon, 5).Value = "LINEA";
                    sheet.Cell(renglon, 6).Value = "MODELO";
                    sheet.Cell(renglon, 7).Value = "ESQUEMA";
                    sheet.Cell(renglon, 8).Value = "MONTO";
                    sheet.Cell(renglon, 9).Value = "FECHA DE COTIZACION";
                    sheet.Cell(renglon, 10).Value = "VIGENCIA";
                    sheet.Cell(renglon, 11).Value = "FECHA DE VENTA";
                    sheet.Cell(renglon, 12).Value = "COMENTARIO";
                    sheet.Cell(renglon, 13).Value = "CULTIVO";


                    // Estilo para los encabezados de la tabla
                    var rango = sheet.Range(renglon, 1, renglon, 13);
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
                        sheet.Cell(renglon, 1).Value = ObtenerNombreFase(det.fase_cotizacion);
                        sheet.Cell(renglon, 2).Value = det.razon_social?.ToUpper();
                        sheet.Cell(renglon, 3).Value = det.nombre_asesor?.ToUpper();
                        sheet.Cell(renglon, 4).Value = det.sucursal;
                        sheet.Cell(renglon, 5).Value = det.linea;
                        sheet.Cell(renglon, 6).Value = det.modelo;
                        sheet.Cell(renglon, 7).Value = det.esquema?.ToUpper();
                        sheet.Cell(renglon, 8).Value = det.monto_total;
                        sheet.Cell(renglon, 9).Value = det.createdate;
                        sheet.Cell(renglon, 10).Value = det.vigencia;
                        sheet.Cell(renglon, 11).Value = det.fecha_venta;
                        sheet.Cell(renglon, 12).Value = det.comentario;
                        sheet.Cell(renglon, 13).Value = det.cultivo;
                        renglon++;
                    }

                    sheet.Column(8).Style.NumberFormat.Format = "#,##0.00";

                    //sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(7).Style.NumberFormat.Format = "0.0 %";

                    //rango = sheet.Range(renglon, 1, renglon, 8);
                    //rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#e5e6e6");
                    //rango.Style.Font.Bold = true;

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
