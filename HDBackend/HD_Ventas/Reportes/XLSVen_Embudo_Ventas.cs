using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Ventas.Modelos.EmbudoVentas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Reportes
{
    public class XLSVen_Embudo_Ventas
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
            switch (fase.Substring(0, 2))
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
        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_Embudo_Ventas_Item_Excel> data, IEnumerable<string> lineas, IEnumerable<string> sucursales, IEnumerable<string> fases, string titulo, string verPor)
        {
            try
            {
                string sheetname = "EMBUDO DE VENTAS";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, titulo, sucursales.Count() + 2);

                    int colEnc = 1;
                    sheet.Cell(renglon, colEnc++).Value = "FASE";

                    foreach (var suc in sucursales)
                        sheet.Cell(renglon, colEnc++).Value = suc.ToUpper();

                    sheet.Cell(renglon, colEnc).Value = "TOTAL";

                    var headerRange = sheet.Range(renglon, 1, renglon, colEnc);
                    headerRange.Style.Font.SetBold();
                    headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    headerRange.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#EBECEE"))
                    .Font.SetFontSize(12);

                    renglon++;

                    foreach (var linea in lineas)
                    {
                        sheet.Cell(renglon, 1).Value = $"LÍNEA DE {linea.ToUpper()}";
                        sheet.Range(renglon, 1, renglon, sucursales.Count() + 2).Merge()
                            .Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.FromHtml("#CCCCCC"))
                            .Font.SetFontSize(12);

                        renglon++;

                        //int col = 1;
                        //sheet.Cell(renglon, col++).Value = "FASE";

                        //foreach (var suc in sucursales)
                        //    sheet.Cell(renglon, col++).Value = suc.ToUpper();

                        //sheet.Cell(renglon, col).Value = "TOTAL";

                        //sheet.Range(renglon, 1, renglon, col).Style.Font.Bold = true;
                        //renglon++;

                        foreach (var fase in fases)
                        {
                            int writeCol = 1;
                            sheet.Cell(renglon, writeCol++).Value = fase;

                            decimal totalMonto = 0;
                            int totalUnidades = 0;

                            // ---- SUCURSAL POR SUCURSAL ----
                            foreach (var suc in sucursales)
                            {
                                var item = data.FirstOrDefault(x =>
                                    x.Linea.Equals(linea, StringComparison.OrdinalIgnoreCase) &&
                                    x.Fase.Equals(fase, StringComparison.OrdinalIgnoreCase) &&
                                    x.Columna.Equals(suc, StringComparison.OrdinalIgnoreCase)
                                );

                                if (verPor == "montos")
                                {
                                    decimal monto = item?.Monto ?? 0;
                                    var c = sheet.Cell(renglon, writeCol++);
                                    c.Value = monto;
                                    c.Style.NumberFormat.Format = "#,##0";
                                    totalMonto += monto;
                                }
                                else if (verPor == "unidades")
                                {
                                    int unidades = item?.Cantidad ?? 0;
                                    sheet.Cell(renglon, writeCol++).Value = unidades;
                                    totalUnidades += unidades;
                                }
                                else if (verPor == "unidadesmonto")
                                {
                                    decimal monto = item?.Monto ?? 0;
                                    int unidades = item?.Cantidad ?? 0;

                                    sheet.Cell(renglon, writeCol++).Value = $"{unidades} - ${monto:N0}";

                                    totalMonto += monto;
                                    totalUnidades += unidades;
                                }
                            }

                            // ---- TOTALES ----
                            if (verPor == "montos")
                            {
                                var cTot = sheet.Cell(renglon, writeCol++);
                                cTot.Value = totalMonto;
                                cTot.Style.NumberFormat.Format = "#,##0";
                                sheet.Cell(renglon, writeCol).Style.Font.Bold = true;
                            }
                            else if (verPor == "unidades")
                            {
                                sheet.Cell(renglon, writeCol).Value = totalUnidades;
                                sheet.Cell(renglon, writeCol).Style.Font.Bold = true;
                            }
                            else if (verPor == "unidadesmonto")
                            {
                                sheet.Cell(renglon, writeCol).Value = $"{totalUnidades} - ${totalMonto:N0}";
                                sheet.Cell(renglon, writeCol).Style.Font.Bold = true;
                            }

                            renglon++;
                        }

                        // ---- TOTAL POR COLUMNA ----
                        int colSumLinea = 1;
                        sheet.Cell(renglon, colSumLinea++).Value = "TOTAL";
                        sheet.Cell(renglon, 1).Style.Font.Bold = true;

                        foreach (var suc in sucursales)
                        {
                            int cantidad = data
                                .Where(x =>
                                    x.Linea.Equals(linea, StringComparison.OrdinalIgnoreCase) &&
                                    x.Columna.Equals(suc, StringComparison.OrdinalIgnoreCase))
                                .Sum(x => x.Cantidad);

                            decimal monto = data
                                .Where(x =>
                                    x.Linea.Equals(linea, StringComparison.OrdinalIgnoreCase) &&
                                    x.Columna.Equals(suc, StringComparison.OrdinalIgnoreCase))
                                .Sum(x => x.Monto);

                            if (verPor == "montos")
                            {
                                var c = sheet.Cell(renglon, colSumLinea++);
                                c.Value = monto;
                                c.Style.NumberFormat.Format = "#,##0";
                            }
                            else if (verPor == "unidades")
                                sheet.Cell(renglon, colSumLinea++).Value = cantidad;
                            else if (verPor == "unidadesmonto")
                                sheet.Cell(renglon, colSumLinea++).Value = $"{cantidad} - ${monto:N0}";
                        }

                        // Total final por línea
                        int totalCantLinea = data
                            .Where(x => x.Linea.Equals(linea, StringComparison.OrdinalIgnoreCase))
                            .Sum(x => x.Cantidad);

                        decimal totalMontoLinea = data
                            .Where(x => x.Linea.Equals(linea, StringComparison.OrdinalIgnoreCase))
                            .Sum(x => x.Monto);

                        if (verPor == "montos")
                        {
                            var cFin = sheet.Cell(renglon, colSumLinea++);
                            cFin.Value = totalMontoLinea;
                            cFin.Style.NumberFormat.Format = "#,##0";
                        }
                        else if (verPor == "unidades")
                            sheet.Cell(renglon, colSumLinea).Value = totalCantLinea;
                        else if (verPor == "unidadesmonto")
                            sheet.Cell(renglon, colSumLinea).Value = $"{totalCantLinea} - ${totalMontoLinea:N0}";

                        sheet.Cell(renglon, colSumLinea).Style.Font.Bold = true;
                        sheet.Range(renglon, 1, renglon, colSumLinea).Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.FromColor(System.Drawing.Color.FromArgb(218, 230, 190)));

                        renglon++;
                    }

                    if (lineas.Count() > 1)
                    {
                        // Titulo de sección
                        sheet.Cell(renglon, 1).Value = "CONSOLIDADO";
                        sheet.Range(renglon, 1, renglon, sucursales.Count() + 2)
                            .Merge()
                            .Style.Font.SetBold()
                            .Fill.SetBackgroundColor(XLColor.FromHtml("#CCCCCC"))
                            .Font.SetFontSize(12);
                        renglon++;

                        //// Encabezados
                        //int col = 1;
                        //sheet.Cell(renglon, col++).Value = "FASE";

                        //foreach (var suc in sucursales)
                        //    sheet.Cell(renglon, col++).Value = suc.ToUpper();

                        //sheet.Cell(renglon, col).Value = "TOTAL";

                        //sheet.Range(renglon, 1, renglon, col).Style.Font.Bold = true;
                        //renglon++;

                        // ---- Consolidado por fase ----
                        foreach (var fase in fases)
                        {
                            int writeCol = 1;
                            sheet.Cell(renglon, writeCol++).Value = fase;

                            decimal totalMonto = 0;
                            int totalUnidades = 0;

                            foreach (var suc in sucursales)
                            {
                                var items = data.Where(x =>
                                    x.Fase.Equals(fase, StringComparison.OrdinalIgnoreCase) &&
                                    x.Columna.Equals(suc, StringComparison.OrdinalIgnoreCase)
                                );

                                int cantidad = items.Sum(x => x.Cantidad);
                                decimal monto = items.Sum(x => x.Monto);

                                if (verPor == "montos")
                                {
                                    var c = sheet.Cell(renglon, writeCol++);
                                    c.Value = monto;
                                    c.Style.NumberFormat.Format = "#,##0";
                                }
                                else if (verPor == "unidades")
                                {
                                    sheet.Cell(renglon, writeCol++).Value = cantidad;
                                }
                                else if (verPor == "unidadesmonto")
                                {
                                    sheet.Cell(renglon, writeCol++).Value = $"{cantidad} - ${monto:N0}";
                                }

                                totalMonto += monto;
                                totalUnidades += cantidad;
                            }

                            // TOTAL POR FASE
                            if (verPor == "montos")
                            {
                                sheet.Cell(renglon, writeCol).Value = totalMonto;
                                sheet.Cell(renglon, writeCol++).Style.NumberFormat.Format = "#,##0";
                            }
                            else if (verPor == "unidades")
                                sheet.Cell(renglon, writeCol).Value = totalUnidades;
                            else if (verPor == "unidadesmonto")
                                sheet.Cell(renglon, writeCol).Value = $"{totalUnidades} - ${totalMonto:N0}";

                            sheet.Cell(renglon, writeCol).Style.Font.Bold = true;

                            renglon++;
                        }

                        // ---- TOTAL FINAL ----
                        int colSum = 1;
                        sheet.Cell(renglon, colSum++).Value = "TOTAL";
                        sheet.Cell(renglon, 1).Style.Font.Bold = true;

                        foreach (var suc in sucursales)
                        {
                            int cantidad = data
                                .Where(x => x.Columna.Equals(suc, StringComparison.OrdinalIgnoreCase))
                                .Sum(x => x.Cantidad);

                            decimal monto = data
                                .Where(x => x.Columna.Equals(suc, StringComparison.OrdinalIgnoreCase))
                                .Sum(x => x.Monto);

                            if (verPor == "montos")
                            {
                                var c = sheet.Cell(renglon, colSum++);
                                c.Value = monto;
                                c.Style.NumberFormat.Format = "#,##0";
                            }
                            else if (verPor == "unidades")
                                sheet.Cell(renglon, colSum++).Value = cantidad;
                            else if (verPor == "unidadesmonto")
                                sheet.Cell(renglon, colSum++).Value = $"{cantidad} - ${monto:N0}";
                        }

                        // Total final (suma de todos los totales columna-columna)
                        int totalCantFinal = data.Sum(x => x.Cantidad);
                        decimal totalMontoFinal = data.Sum(x => x.Monto);

                        if (verPor == "montos")
                        {
                            var cFinCon = sheet.Cell(renglon, colSum++);
                            cFinCon.Value = totalMontoFinal;
                            cFinCon.Style.NumberFormat.Format = "#,##0";
                        }
                        else if (verPor == "unidades")
                            sheet.Cell(renglon, colSum).Value = totalCantFinal;
                        else if (verPor == "unidadesmonto")
                            sheet.Cell(renglon, colSum).Value = $"{totalCantFinal} - ${totalMontoFinal:N0}";

                        sheet.Cell(renglon, colSum).Style.Font.Bold = true;
                        sheet.Range(renglon, 1, renglon, colSum).Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.FromColor(System.Drawing.Color.FromArgb(218, 230, 190)));
                        renglon++;
                    }

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


