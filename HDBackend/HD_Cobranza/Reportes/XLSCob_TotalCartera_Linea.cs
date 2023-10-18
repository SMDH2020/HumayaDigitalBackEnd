using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;

namespace HD_Cobranza.Reportes
{
    public static class XLSCob_TotalCartera_Linea
    {
        public static Task<DocResult> CrearExcel(IEnumerable<mdlCob_TotalCarteraPorLinea> lista)
        {
            try
            {
                string sheetname = "Facturas por vencer";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Arial";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, $"RESUMEN DE CARTERA POR LINEA", 13);

                    sheet.Cell(renglon, 1).Value = "Sucursal";
                    sheet.Cell(renglon, 2).Value = "Más de 90";
                    sheet.Cell(renglon, 3).Value = "Más de 60";
                    sheet.Cell(renglon, 4).Value = "Más de 30";
                    sheet.Cell(renglon, 5).Value = "Más de 15";
                    sheet.Cell(renglon, 6).Value = "De 1 a 15";
                    sheet.Cell(renglon, 7).Value = "Total Vencido";
                    sheet.Cell(renglon, 8).Value = "Por vencer";
                    sheet.Cell(renglon, 9).Value = "Total Cartera";
                    sheet.Cell(renglon, 10).Value = "Saldo a Favor";
                    sheet.Cell(renglon, 11).Value = "Total";
                    sheet.Cell(renglon, 12).Value = "% Vencido";
                    sheet.Cell(renglon, 13).Value = "% Por Vencer";

                    var rango = sheet.Range(renglon, 1, renglon, 13);
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
                        rango = sheet.Range(renglon, 1, renglon, 13);
                        rango.Style.Fill.BackgroundColor = XLColor.FromArgb(218, 230, 190);
                        rango.Style.Font.Bold = true;
                        rango.Style.Font.FontSize = 10;
                        //rango.RangeUsed().SetAutoFilter();
                        rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        renglon++;
                        foreach (mdlCob_TotalCarteraPorLinea activos in lista.Where(item => item.sucursal == mdl.Key))
                        {
                            sheet.Cell(renglon, 1).Value = activos.linea.ToUpper();
                            sheet.Cell(renglon, 2).Value = activos.mas90;
                            sheet.Cell(renglon, 3).Value = activos.mas60;
                            sheet.Cell(renglon, 4).Value = activos.mas30;
                            sheet.Cell(renglon, 5).Value = activos.mas15;
                            sheet.Cell(renglon, 6).Value = activos.de1a15;
                            sheet.Cell(renglon, 7).Value = activos.vencido;
                            sheet.Cell(renglon, 8).Value = activos.porvencer;
                            sheet.Cell(renglon, 9).Value = activos.totalcartera;
                            sheet.Cell(renglon, 10).Value = activos.saldoafavor;
                            sheet.Cell(renglon, 11).Value = activos.total;
                            sheet.Cell(renglon, 12).Value = (activos.vencido / activos.totalcartera) * 100;
                            sheet.Cell(renglon, 13).Value = (activos.porvencer / activos.totalcartera) * 100;
                            renglon++;
                        }
                        //sheet.Cell(renglon, 1).Value = "TOTAL";
                        //sheet.Cell(renglon, 2).Value = lista.Where(item => item.sucursal == mdl.Key).Sum(item => item.mas90);
                        //sheet.Cell(renglon, 3).Value = lista.Where(item => item.sucursal == mdl.Key).Sum(item => item.mas60);
                        //sheet.Cell(renglon, 4).Value = lista.Where(item => item.sucursal == mdl.Key).Sum(item => item.mas30);
                        //sheet.Cell(renglon, 5).Value = lista.Where(item => item.sucursal == mdl.Key).Sum(item => item.mas15);
                        //sheet.Cell(renglon, 6).Value = lista.Where(item => item.sucursal == mdl.Key).Sum(item => item.de1a15);
                        //sheet.Cell(renglon, 7).Value = lista.Where(item => item.sucursal == mdl.Key).Sum(item => item.vencido);
                        //sheet.Cell(renglon, 8).Value = lista.Where(item => item.sucursal == mdl.Key).Sum(item => item.porvencer);
                        //sheet.Cell(renglon, 9).Value = lista.Where(item => item.sucursal == mdl.Key).Sum(item => item.totalcartera);
                        //sheet.Cell(renglon, 10).Value = lista.Where(item => item.sucursal == mdl.Key).Sum(item => item.saldoafavor);
                        //sheet.Cell(renglon, 11).Value = lista.Where(item => item.sucursal == mdl.Key).Sum(item => item.total);
                        //sheet.Cell(renglon, 12).FormulaA1 = $"=G{renglon}/I{renglon}*100";
                        //sheet.Cell(renglon, 13).FormulaA1 = $"=H{renglon}/I{renglon}*100";

                        rango = sheet.Range(renglon, 1, renglon, 13);
                        rango.Style.Font.Bold = true;
                        renglon += 1;
                    }

                    renglon++;
                    //var lineas = lista.GroupBy(item => item.linea).ToList();
                    //sheet.Cell(renglon, 1).Value = "RESUMEN";
                    //rango = sheet.Range(renglon, 1, renglon, 13);
                    //rango.Style.Fill.BackgroundColor = XLColor.FromArgb(218, 230, 190);
                    //rango.Style.Font.Bold = true;
                    //rango.Style.Font.FontSize = 10;
                    ////rango.RangeUsed().SetAutoFilter();
                    //rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    //rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    //foreach (var linea in lineas)
                    //{
                    //    renglon++;
                    //    sheet.Cell(renglon, 1).Value = linea.Key.ToUpper();
                    //    sheet.Cell(renglon, 2).Value = lista.Where(item => item.linea == linea.Key).Sum(item => item.mas90);
                    //    sheet.Cell(renglon, 3).Value = lista.Where(item => item.linea == linea.Key).Sum(item => item.mas60);
                    //    sheet.Cell(renglon, 4).Value = lista.Where(item => item.linea == linea.Key).Sum(item => item.mas30);
                    //    sheet.Cell(renglon, 5).Value = lista.Where(item => item.linea == linea.Key).Sum(item => item.mas15);
                    //    sheet.Cell(renglon, 6).Value = lista.Where(item => item.linea == linea.Key).Sum(item => item.de1a15);
                    //    sheet.Cell(renglon, 7).Value = lista.Where(item => item.linea == linea.Key).Sum(item => item.vencido);
                    //    sheet.Cell(renglon, 8).Value = lista.Where(item => item.linea == linea.Key).Sum(item => item.porvencer);
                    //    sheet.Cell(renglon, 9).Value = lista.Where(item => item.linea == linea.Key).Sum(item => item.totalcartera);
                    //    sheet.Cell(renglon, 10).Value = lista.Where(item => item.linea == linea.Key).Sum(item => item.saldoafavor);
                    //    sheet.Cell(renglon, 11).Value = lista.Where(item => item.linea == linea.Key).Sum(item => item.total);
                    //    sheet.Cell(renglon, 12).FormulaA1 = $"=G{renglon}/I{renglon}*100";
                    //    sheet.Cell(renglon, 13).FormulaA1 = $"=H{renglon}/I{renglon}*100";
                    //}


                    //renglon++;
                    //sheet.Cell(renglon, 1).Value = "TOTAL";
                    //sheet.Cell(renglon, 2).Value = lista.Sum(item => item.mas90);
                    //sheet.Cell(renglon, 3).Value = lista.Sum(item => item.mas60);
                    //sheet.Cell(renglon, 4).Value = lista.Sum(item => item.mas30);
                    //sheet.Cell(renglon, 5).Value = lista.Sum(item => item.mas15);
                    //sheet.Cell(renglon, 6).Value = lista.Sum(item => item.de1a15);
                    //sheet.Cell(renglon, 7).Value = lista.Sum(item => item.vencido);
                    //sheet.Cell(renglon, 8).Value = lista.Sum(item => item.porvencer);
                    //sheet.Cell(renglon, 9).Value = lista.Sum(item => item.totalcartera);
                    //sheet.Cell(renglon, 10).Value = lista.Sum(item => item.saldoafavor);
                    //sheet.Cell(renglon, 11).Value = lista.Sum(item => item.total);
                    //sheet.Cell(renglon, 12).FormulaA1 = $"=G{renglon}/I{renglon}*100";
                    //sheet.Cell(renglon, 13).FormulaA1 = $"=H{renglon}/I{renglon}*100";

                    rango = sheet.Range(renglon, 1, renglon, 13);
                    rango.Style.Font.Bold = true;

                    sheet.Column(2).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(3).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(4).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(5).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(7).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(8).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(9).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(10).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(11).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(12).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Column(13).Style.NumberFormat.Format = "#,##0.00";

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
