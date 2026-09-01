using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Cobranza.Reportes;
using HD_Cobranza.Modelos;
using DocumentFormat.OpenXml.Bibliography;
using HD_Cobranza.GestionCobranza.Modelos;
using HD_Cobranza.Modelos.Dashboard;
using System.Globalization;

namespace HD_Cobranza.Reportes
{
    public class XLSCob_Dashboard_ReporteProyeccionMensual
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
                default:
                    return "";

            }
        }

        public static string obtenerLinea(string linea)
        {
            switch (linea)
            {
                case "O":
                    return "OPERACION";
                case "R":
                    return "REVOLVENTE";
                case "E":
                    return "ESPECIAL";
                default:
                    return "";

            }
        }

        public static string FormatearMesAnio(string mes)
        {
            // Verifica que el formato sea correcto
            if (DateTime.TryParseExact(mes, "yyyy-MM", null, System.Globalization.DateTimeStyles.None, out DateTime fecha))
            {
                string nombreMes = obtenernombre_mes(fecha.Month);
                string anio = fecha.Year.ToString();
                return $"{nombreMes} {anio}";
            }
            else
            {
                throw new ArgumentException("El formato de la variable mes no es válido. Debe ser 'yyyy-MM'.");
            }
        }

        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_Reporte_Proyeccion_Recuperacion_Mensual_tipo_cartera> detalle, int ejercicio, int periodo, string mes, string sucursales, string adr, string tipo_cartera)
        {
            try
            {
                string sheetname = "REPORTE PROYECCION";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, "PROYECCION DE RECUPERACION DE CARTERA " + obtenerCartera(tipo_cartera) + " " + FormatearMesAnio(mes), 17);


                    //renglon++;

                    sheet.Cell(renglon, 1).Value = "LINEA";
                    sheet.Cell(renglon, 2).Value = "CLIENTE";
                    sheet.Cell(renglon, 3).Value = "SUCURSAL";
                    sheet.Cell(renglon, 4).Value = "VENCIMIENTO";
                    sheet.Cell(renglon, 5).Value = "IMPORTE DE LA FACTURA";
                    sheet.Cell(renglon, 6).Value = "RECUPERADO";
                    sheet.Cell(renglon, 7).Value = "SALDO";
                    sheet.Cell(renglon, 8).Value = "INT. NORMAL";
                    sheet.Cell(renglon, 9).Value = "INT. MORATORIO";
                    sheet.Cell(renglon, 10).Value = "SALDO TOTAL";
                    sheet.Cell(renglon, 11).Value = "FECHA DE RECUPERACION";
                    sheet.Cell(renglon, 12).Value = "FECHA DE CONTACTO";
                    sheet.Cell(renglon, 13).Value = "FECHA COMPROMISO";
                    sheet.Cell(renglon, 14).Value = "CONVENIO";
                    sheet.Cell(renglon, 15).Value = "OBJECION";
                    sheet.Cell(renglon, 16).Value = "OBSERVACIONES";
                    sheet.Cell(renglon, 17).Value = "RESPONSABLE";

                    // Estilo para los encabezados de la tabla
                    var rango = sheet.Range(renglon, 1, renglon, 17);
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
                        DateTime fecha = DateTime.ParseExact(det.vencimiento_factura, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                        sheet.Cell(renglon, 1).Value = obtenerLinea(det.linea_credito);
                        sheet.Cell(renglon, 2).Value = det.cliente?.ToUpper();
                        sheet.Cell(renglon, 3).Value = det.sucursal;
                        sheet.Cell(renglon, 4).Value = fecha;
                        sheet.Cell(renglon, 5).Value = det.importe_factura;
                        sheet.Cell(renglon, 6).Value = det.pagado;
                        sheet.Cell(renglon, 7).Value = det.saldo;
                        sheet.Cell(renglon, 8).Value = det.interes_normal;
                        sheet.Cell(renglon, 9).Value = det.interes_moratorio;
                        sheet.Cell(renglon, 10).Value = det.saldo_total;
                        sheet.Cell(renglon, 11).Value = det.fecha_recuperacion;
                        sheet.Cell(renglon, 12).Value = det.fecha_contacto;
                        sheet.Cell(renglon, 13).Value = det.fecha_convenio;
                        sheet.Cell(renglon, 14).Value = det.tiene_convenio;
                        sheet.Cell(renglon, 15).Value = det.objecion;
                        sheet.Cell(renglon, 16).Value = det.observaciones;
                        sheet.Cell(renglon, 17).Value = det.responsable;
                        renglon++;
                    }


                    sheet.Cell(renglon, 4).Style.DateFormat.Format = "dd/MM/yyyy";
                    sheet.Column(5).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(7).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(8).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(9).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(10).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Column(11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Column(12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Column(13).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;



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
