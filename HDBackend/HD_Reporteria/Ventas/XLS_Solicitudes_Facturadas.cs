using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Cobranza;
using HD_Cobranza.Reportes;
using HD_Ventas.Modelos.SolicitudesCerradas;

namespace HD_Reporteria.Ventas
{
    public class XLS_Solicitudes_Facturadas
    {
        public static Task<DocResult> CrearExcel(IEnumerable<mdlOperacionesDetalle> lista, string card, int periodo, int ejercicio)
        {
            try
            {
                string sheetname;
                string encabezado;
                string nombre_mes(int mes)
                {
                    string nombre = "";
                    switch (mes)
                    {
                        case 1: nombre = "Enero"; break;
                        case 2: nombre = "Febrero"; break;
                        case 3: nombre = "Marzo"; break;
                        case 4: nombre = "Abril"; break;
                        case 5: nombre = "Mayo"; break;
                        case 6: nombre = "Junio"; break;
                        case 7: nombre = "Julio"; break;
                        case 8: nombre = "Agosto"; break;
                        case 9: nombre = "Septiembre"; break;
                        case 10: nombre = "Octubre"; break;
                        case 11: nombre = "Noviembre"; break;
                        case 12: nombre = "Diciembre"; break;
                    }
                    return nombre;
                }
                switch (card)
                {
                    case "T":
                        sheetname = "OPERACIONES CERRADAS";
                        break;
                    case "J":
                        sheetname = "OPERACIONES JDF";
                        break;
                    case "P": 
                        sheetname = "OPERACIONES DE PRESTAMOS";
                        break;
                    case "O":
                        sheetname = "OPERACIONES MHUSA";
                        break;
                    case "C":
                        sheetname = "OPERACIONES DE CONTADO";
                        break;
                    default:
                        sheetname = "SOLICITUDES FACTURADAS";
                        break;
                }

                switch (card)
                {
                    case "T":
                        encabezado = "Operaciones Cerradas de " + nombre_mes(periodo) + " " + ejercicio;
                        break;
                    case "J":
                        encabezado = "Operaciones Jdf de " + nombre_mes(periodo) + " " + ejercicio;
                        break;
                    case "P":
                        encabezado = "Operaciones de Prestamos de " + nombre_mes(periodo) + " " + ejercicio;
                        break;
                    case "O":
                        encabezado = "Operaciones Mhusa de " + nombre_mes(periodo) + " " + ejercicio;
                        break;
                    case "C":
                        encabezado = "Operaciones de Contado de " + nombre_mes(periodo) + " " + ejercicio;
                        break;
                    default:
                        encabezado = "Operaciones Cerradas de " + nombre_mes(periodo) + " " + ejercicio;
                        break;
                }
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, encabezado, 9);

                    sheet.Cell(renglon, 1).Value = "Sucursal";
                    sheet.Cell(renglon, 2).Value = "Cliente";
                    sheet.Cell(renglon, 3).Value = "Asesor";
                    sheet.Cell(renglon, 4).Value = "Linea";
                    sheet.Cell(renglon, 5).Value = "Modelo";
                    sheet.Cell(renglon, 6).Value = "Operación";
                    sheet.Cell(renglon, 7).Value = "Creado";
                    sheet.Cell(renglon, 8).Value = "Facturado";
                    sheet.Cell(renglon, 9).Value = "Importe";

                    var rango = sheet.Range(renglon, 1, renglon, 9);
                    //rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    //// Filtra la lista para incluir solo aquellos elementos con datos en "facturado"
                    //var listaFiltrada = lista.Where(x => !string.IsNullOrEmpty(x.facturado)).OrderBy(x => x.idestado).ThenBy(x => x.idsucursal).ToList();
                    string sucursalActual = string.Empty;
                    int renglonInicioSucursal = renglon;
                    double totalGeneral = 0.0;

                    foreach (var solicitudes in lista)
                    {
                        // Si la sucursal actual es diferente, inserta una fila con el nombre de la sucursal.
                        if (solicitudes.sucursal != sucursalActual)
                        {
                            if (!string.IsNullOrEmpty(sucursalActual))
                            {
                                // Agregar fila total para la sucursal anterior.
                                //sheet.Cell(renglon, 2).Value = "TOTALES";
                                //sheet.Cell(renglon, 9).FormulaA1 = $"SUBTOTAL(9,I{renglonInicioSucursal}:I{renglon - 1})";
                                //// Aplicar formato a la fila de totales
                                //var rangoTotalSucursal = sheet.Range(renglon, 1, renglon, 9);
                                //rangoTotalSucursal.Style.Fill.BackgroundColor = XLColor.FromHtml("#e3dca5");
                                //rangoTotalSucursal.Style.Font.Bold = true;
                                //renglon++;
                            }

                            // Agregar nombre de la sucursal
                            //sheet.Cell(renglon, 1).Value = $"{solicitudes.sucursal}";
                            //sheet.Range(renglon, 1, renglon, 9).Merge().Style.Font.Bold = true;
                            //sheet.Range(renglon, 1, renglon, 9).Merge().Style.Font.FontSize = 14;
                            //sheet.Range(renglon, 1, renglon, 9).Style.Font.FontColor = XLColor.FromHtml("#ffc000");
                            //sheet.Range(renglon, 1, renglon, 9).Style.Fill.BackgroundColor = XLColor.FromHtml("#006600");
                            //renglon++;

                            // Actualizar el inicio del rango para el total de la nueva sucursal
                            renglonInicioSucursal = renglon;
                            sucursalActual = solicitudes.sucursal;
                        }

                        // Agregar los datos de la solicitud
                        sheet.Cell(renglon, 1).Value = Capitalize(solicitudes.sucursal);
                        sheet.Cell(renglon, 2).Value = solicitudes.cliente;
                        sheet.Cell(renglon, 3).Value = solicitudes.asesor;
                        sheet.Cell(renglon, 4).Value = solicitudes.linea;
                        sheet.Cell(renglon, 5).Value = Capitalize(solicitudes.modelo);
                        sheet.Cell(renglon, 6).Value = solicitudes.linea_credito;
                        sheet.Cell(renglon, 7).Value = solicitudes.creado;
                        sheet.Cell(renglon, 8).Value = solicitudes.facturado;
                        sheet.Cell(renglon, 9).Value = solicitudes.importe;

                        // Sumar al total general
                        totalGeneral += solicitudes.importe;
                        renglon++;
                    }

                    // Agregar la fila total para la última sucursal
                    //if (!string.IsNullOrEmpty(sucursalActual))
                    //{
                    //    sheet.Cell(renglon, 2).Value = "TOTALES";
                    //    sheet.Cell(renglon, 9).FormulaA1 = $"SUBTOTAL(9,I{renglonInicioSucursal}:I{renglon - 1})";
                    //    // Aplicar formato a la fila de totales
                    //    var rangoTotalSucursal = sheet.Range(renglon, 1, renglon, 9);
                    //    rangoTotalSucursal.Style.Fill.BackgroundColor = XLColor.FromHtml("#e5e6e6");
                    //    rangoTotalSucursal.Style.Font.Bold = true;
                    //    renglon++;
                    //}

                    // Agregar fila total general
                    sheet.Cell(renglon, 2).Value = "TOTAL GENERAL";
                    sheet.Cell(renglon, 9).Value = totalGeneral;
                    sheet.Cell(renglon, 9).Style.Font.Bold = true;
                    //sheet.Cell(renglon, 2).Style.Font.FontColor = XLColor.FromHtml("#ffc000");
                    //sheet.Cell(renglon, 9).Style.Font.FontColor = XLColor.FromHtml("#ffc000");
                    //sheet.Cell(renglon, 9).Style.Fill.BackgroundColor = XLColor.FromHtml("#006600");

                    // Formato para la columna de importes
                    sheet.Column(9).Style.NumberFormat.Format = "#,##0.00";

                    // Aplicar formato a la última fila
                    var rangoTotalGeneral = sheet.Range(renglon, 1, renglon, 9);
                    rangoTotalGeneral.Style.Font.FontSize = 10;
                    //rangoTotalGeneral.Style.Fill.BackgroundColor = XLColor.FromHtml("#006600");
                    rangoTotalGeneral.Style.Font.Bold = true;

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
        public static string Capitalize(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input; // Devuelve el string si es nulo o vacío

            // Toma la primera letra en mayúscula y el resto en minúsculas
            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }
    }
}
