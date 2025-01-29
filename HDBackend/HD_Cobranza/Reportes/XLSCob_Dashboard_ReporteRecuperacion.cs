using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Cobranza.Reportes;
using HD_Cobranza.Modelos;
using DocumentFormat.OpenXml.Bibliography;
using HD_Cobranza.GestionCobranza.Modelos;
using HD_Cobranza.Modelos.Dashboard;

namespace HD_Cobranza.Reportes
{
    public class XLSCob_Dashboard_ReporteRecuperacion
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

        public static string obtenerCartera(string cartera)
        {
            switch (cartera)
            {
                case "O":
                    return "DE OPERACION";
                case "R":
                    return "REVOLVENTE";
                case "E":
                    return "ESPECIAL";
                case "M":
                    return "JURIDICA";
                default:
                    return "";

            }
        }

        public static string obtenerEstado(string cartera)
        {
            switch (cartera)
            {
                case "A":
                    return "ACTIVA";
                case "P":
                    return "POR VENCER";
                case "V":
                    return "VENCIDA";
                default:
                    return "";

            }
        }

        public static string obtenerResponsable(string responsable)
        {
            switch (responsable)
            {
                case "EC":
                    return "EJECUTIVO COBRANZA";
                case "CS":
                    return "COBRANZA SINALOA";
                case "CN":
                    return "COBRANZA NAYARIT";
                case "GC":
                    return "GERENCIA DE COBRANZA";
                default:
                    return "";

            }
        }

        public static string obtenerTitulo(string tipo_grafica, string tipo_cartera, string estado, string responsable, int ejercicio, int periodo)
        {
            switch (tipo_grafica)
            {
                case "O":
                    return "OBJETIVO DE CARTERA " + obtenerCartera(tipo_cartera) + " " + obtenernombre_mes(periodo) + " " + ejercicio;
                case "T":
                    return "RECUPERACION DE CARTERA " + obtenerCartera(tipo_cartera) + " " + obtenerEstado(estado) + " " + obtenernombre_mes(periodo) + " " + ejercicio;
                case "R":
                    return "RECUPERACION DE CARTERA " + " " + obtenerEstado(estado) + " DE " + obtenerResponsable(responsable) + " " + obtenernombre_mes(periodo) + " " + ejercicio;
                default:
                    return "";

            }
        }

        public static int obtenerAncho(string tipo_grafica)
        {
            switch (tipo_grafica)
            {
                case "O":
                    return 5;
                case "T":
                    return 6;
                case "R":
                    return 6;
                default:
                    return 0;

            }
        }

        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_Dashboard_Reporte_Grafica_Total> detalle, int ejercicio, int periodo, string tipo_cartera, string tipo_grafica, string estado, string responsable)
        {
            try
            {
                string sheetname = "REPORTE DE RECUPERACION";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, obtenerTitulo(tipo_grafica, tipo_cartera, estado, responsable, ejercicio, periodo), obtenerAncho(tipo_grafica));

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

                    sheet.Cell(renglon, 1).Value = "SUCURSAL";
                    sheet.Cell(renglon, 2).Value = "CLIENTE";
                    sheet.Cell(renglon, 3).Value = "VENCIMIENTO";
                    sheet.Cell(renglon, 4).Value = "DIAS VENCIDO";
                    if (tipo_grafica == "O")
                    {
                        sheet.Cell(renglon, 5).Value = "SALDO";
                    }
                    if (tipo_grafica == "T" || tipo_grafica == "R")
                    {
                        sheet.Cell(renglon, 5).Value = "RECUPERADO";
                        sheet.Cell(renglon, 6).Value = "OBJETIVO";
                    }

                    // Estilo para los encabezados de la tabla
                    var rango = sheet.Range(renglon, 1, renglon, obtenerAncho(tipo_grafica));
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
                        sheet.Cell(renglon, 1).Value = det.sucursal;
                        sheet.Cell(renglon, 2).Value = det.razon_social?.ToUpper();
                        sheet.Cell(renglon, 3).Value = DateTime.ParseExact(det.vencimiento, "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        sheet.Cell(renglon, 4).Value = det.dias_Vencido;
                        if (tipo_grafica == "O")
                        {
                            sheet.Cell(renglon, 5).Value = det.saldo;
                        }
                        if (tipo_grafica == "T" || tipo_grafica == "R")
                        {
                            sheet.Cell(renglon, 5).Value = det.recuperado;
                            sheet.Cell(renglon, 6).Value = det.objetivo;
                        }
                        renglon++;
                    }

                    sheet.Column(3).Style.DateFormat.Format = "dd/MM/yyyy";
                    sheet.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Column(4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Column(5).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";

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
