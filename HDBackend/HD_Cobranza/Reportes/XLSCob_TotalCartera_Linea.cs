using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;

namespace HD_Cobranza.Reportes
{
    public static class XLSCob_TotalCartera_Linea
    {
        public static Task<DocResult> CrearExcel(IEnumerable<mdlCob_TotalCarteraPorLinea> lista,string titulo)
        {
            try
            {
                string sheetname = "Facturas por vencer";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, titulo, 17);
                    sheet.Cell(renglon, 1).Value = "LINEA";
                    sheet.Cell(renglon, 2).Value = "TOTAL CARTERA";
                    sheet.Cell(renglon, 3).Value = "SALDO A FAVOR";
                    sheet.Cell(renglon, 4).Value = "TOTAL";
                    sheet.Cell(renglon, 5).Value = "JURIDICO";
                    sheet.Cell(renglon, 6).Value = "%";
                    sheet.Cell(renglon, 7).Value = "CARTERA ACTIVA";
                    sheet.Cell(renglon, 8).Value = "%";
                    sheet.Cell(renglon, 9).Value = "POR VENCER";
                    sheet.Cell(renglon, 10).Value = "%";
                    sheet.Cell(renglon, 11).Value = "VENCIDA";
                    sheet.Cell(renglon, 12).Value = "%";
                    sheet.Cell(renglon, 13).Value = "DE 1 A 15";
                    sheet.Cell(renglon, 14).Value = "MAS DE 15";
                    sheet.Cell(renglon, 15).Value = "MAS DE 30";
                    sheet.Cell(renglon, 16).Value = "MAS DE 60";
                    sheet.Cell(renglon, 17).Value = "MAS DE 90";

                    var rango = sheet.Range(renglon, 1, renglon, 17);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    var sucursales = lista.GroupBy(item => item.sucursal).ToList();

                    foreach (var mdl in sucursales)
                    {
                        sheet.Cell(renglon, 1).Value = mdl.Key;
                        rango = sheet.Range(renglon, 1, renglon, 17);
                        rango.Style.Fill.BackgroundColor = XLColor.FromArgb(218, 230, 190);
                        rango.Style.Font.Bold = true;
                        rango.Style.Font.FontSize = 10;
                        //rango.RangeUsed().SetAutoFilter();
                        rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        renglon++;
                        foreach (mdlCob_TotalCarteraPorLinea activos in lista.Where(item => item.sucursal == mdl.Key))
                        {
                            sheet.Cell(renglon, 1).Value = activos.linea;
                            sheet.Cell(renglon, 2).Value = activos.totalcartera + activos.juridico;
                            sheet.Cell(renglon, 3).Value = activos.saldoafavor;
                            sheet.Cell(renglon, 4).Value = activos.total + activos.juridico;
                            sheet.Cell(renglon, 5).Value = activos.juridico;
                            sheet.Cell(renglon, 6).Value = activos.juridico / (activos.totalcartera + activos.juridico);
                            sheet.Cell(renglon, 7).Value = activos.activo;
                            sheet.Cell(renglon, 8).Value = activos.activo / (activos.totalcartera + activos.juridico);
                            sheet.Cell(renglon, 9).Value = activos.porvencer;
                            sheet.Cell(renglon, 10).Value = activos.porvencer / (activos.totalcartera + activos.juridico);
                            sheet.Cell(renglon, 11).Value = activos.vencido;
                            sheet.Cell(renglon, 12).Value = activos.vencido / (activos.totalcartera + activos.juridico);
                            sheet.Cell(renglon, 13).Value = activos.de1a15;
                            sheet.Cell(renglon, 14).Value = activos.mas15;
                            sheet.Cell(renglon, 15).Value = activos.mas30;
                            sheet.Cell(renglon, 16).Value = activos.mas60;
                            sheet.Cell(renglon, 17).Value = activos.mas90;
                            renglon++;
                        }

                    }

                    //sheet.Cell(renglon, 1).Value = "TOTAL";
                    //sheet.Cell(renglon, 2).FormulaA1 = $"SUBTOTAL(9,C5:C{renglon - 1})";
                    //sheet.Cell(renglon, 3).FormulaA1 = $"SUBTOTAL(9,D5:D{renglon - 1})";
                    //sheet.Cell(renglon, 4).FormulaA1 = $"SUBTOTAL(9,E5:E{renglon - 1})";
                    //sheet.Cell(renglon, 5).FormulaA1 = $"SUBTOTAL(9,F5:F{renglon - 1})";
                    //sheet.Cell(renglon, 6).FormulaA1 = $"=C{renglon}/F{renglon}/100";
                    //sheet.Cell(renglon, 7).FormulaA1 = $"SUBTOTAL(9,H5:H{renglon - 1})";
                    //sheet.Cell(renglon, 8).FormulaA1 = $"=C{renglon}/H{renglon}/100";
                    //sheet.Cell(renglon, 9).FormulaA1 = $"SUBTOTAL(9,J5:J{renglon - 1})";
                    //sheet.Cell(renglon, 10).FormulaA1 = $"=C{renglon}/J{renglon}/100";
                    //sheet.Cell(renglon, 11).FormulaA1 = $"SUBTOTAL(9,L5:L{renglon - 1})";
                    //sheet.Cell(renglon, 12).FormulaA1 = $"=C{renglon}/L{renglon}/100";
                    //sheet.Cell(renglon, 13).FormulaA1 = $"SUBTOTAL(9,N5:N{renglon - 1})";
                    //sheet.Cell(renglon, 14).FormulaA1 = $"SUBTOTAL(9,O5:O{renglon - 1})";
                    //sheet.Cell(renglon, 15).FormulaA1 = $"SUBTOTAL(9,P5:P{renglon - 1})";
                    //sheet.Cell(renglon, 16).FormulaA1 = $"SUBTOTAL(9,Q5:Q{renglon - 1})";
                    //sheet.Cell(renglon, 17).FormulaA1 = $"SUBTOTAL(9,R5:R{renglon - 1})";

                    rango = sheet.Range(renglon-1, 1, renglon-1, 17);
                    rango.Style.Font.Bold = true;
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#e5e6e6");

                    sheet.Column(2).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(3).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(4).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(5).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(6).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(7).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(8).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(9).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(10).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(11).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(12).Style.NumberFormat.Format = "0.0 %";
                    sheet.Column(13).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(14).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(15).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(16).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(17).Style.NumberFormat.Format = "#,##0.00";

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
                        filename = "RESUMEN DE CARTERA POR LINEA"
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
