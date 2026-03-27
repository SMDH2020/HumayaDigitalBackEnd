using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Cobranza.Reportes;
using HD_Cobranza.Modelos;
using DocumentFormat.OpenXml.Bibliography;
using HD_Cobranza.GestionCobranza.Modelos;

namespace HD_Cobranza.Reportes
{
    public class XLSCob_Listado_Clientes_Gestionar_Prueba_2
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
        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_Listado_Clientes_Gestionar_Prueba_2> detalle)
        {
            try
            {
                string sheetname = "CLIENTES A GESTIONAR";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, $"LISTADO DE CLIENTES A GESTIONAR", 22);

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

                    sheet.Cell(renglon, 1).Value = "CLIENTE";
                    sheet.Cell(renglon, 2).Value = "ESTADO";
                    sheet.Cell(renglon, 3).Value = "SUCURSAL";
                    sheet.Cell(renglon, 4).Value = "CREDITO";
                    sheet.Cell(renglon, 5).Value = "DOCUMENTO";
                    sheet.Cell(renglon, 6).Value = "DIAS VENCIDOS";
                    sheet.Cell(renglon, 7).Value = "VENCIMIENTO";
                    sheet.Cell(renglon, 8).Value = "CAPITAL";
                    sheet.Cell(renglon, 9).Value = "INT. NORMAL";
                    sheet.Cell(renglon, 10).Value = "INT. MORATORIO";
                    sheet.Cell(renglon, 11).Value = "TOTAL";
                    sheet.Cell(renglon, 12).Value = "OBJETIVO";
                    sheet.Cell(renglon, 13).Value = "RECUPERADO";
                    sheet.Cell(renglon, 14).Value = "SALDO";
                    sheet.Cell(renglon, 15).Value = "FECHA RECUPERACION";
                    sheet.Cell(renglon, 16).Value = "FECHA CONTACTO";
                    sheet.Cell(renglon, 17).Value = "FECHA COMPROMISO";
                    sheet.Cell(renglon, 18).Value = "CONVENIO";
                    sheet.Cell(renglon, 19).Value = "COMENTARIO";
                    sheet.Cell(renglon, 20).Value = "OBJECION";
                    sheet.Cell(renglon, 21).Value = "OBSERVACIONES";
                    sheet.Cell(renglon, 22).Value = "RESPONSABLE";

                    // Estilo para los encabezados de la tabla
                    var rango = sheet.Range(renglon, 1, renglon, 22);
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
                        sheet.Cell(renglon, 1).Value = det.razon_social?.ToUpper();
                        sheet.Cell(renglon, 2).Value = det.adr;
                        sheet.Cell(renglon, 3).Value = det.sucursal;
                        sheet.Cell(renglon, 4).Value = det.linea_credito == "R" ? "REVOLVENTE" : "OPERACION";
                        sheet.Cell(renglon, 5).Value = det.documento == "0" ? "-" : det.documento;
                        sheet.Cell(renglon, 6).Value = det.dias_vencidos;
                        sheet.Cell(renglon, 7).Value = det.vencimiento;
                        sheet.Cell(renglon, 8).Value = det.capital;
                        sheet.Cell(renglon, 9).Value = det.interes_normal;
                        sheet.Cell(renglon, 10).Value = det.interes_moratorio;
                        sheet.Cell(renglon, 11).Value = det.saldo_total;
                        sheet.Cell(renglon, 12).Value = det.objetivo;
                        sheet.Cell(renglon, 13).Value = det.recuperado;
                        sheet.Cell(renglon, 14).Value = det.saldo;
                        sheet.Cell(renglon, 15).Value = det.fecha_recuperacion;
                        sheet.Cell(renglon, 16).Value = det.fecha_contacto;
                        sheet.Cell(renglon, 17).Value = det.fecha_compromiso;
                        sheet.Cell(renglon, 18).Value = det.convenio;
                        sheet.Cell(renglon, 19).Value = det.comentario;
                        sheet.Cell(renglon, 20).Value = det.objecion;
                        sheet.Cell(renglon, 21).Value = det.observaciones;
                        sheet.Cell(renglon, 22).Value = det.nombre_responsable?.ToUpper();
                        renglon++;
                    }

                    sheet.Column(8).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(9).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(10).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(11).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(12).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(13).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(14).Style.NumberFormat.Format = "#,##0.00";

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
