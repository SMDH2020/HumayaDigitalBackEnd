using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Buro.Modelos;
using HD_Cobranza;
using HD_Cobranza.Reportes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Buro.Consultas
{
    public class AD_Cartera_Vencida_XLS
    {
        public static Task<DocResult> CrearResumenPorSucursal(IEnumerable<mdlCartera_Vencida> lista)
        {
            try
            {
                string sheetname = "Cartera Vencida";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, $"BURO DE CREDITO", 37);

                    sheet.Cell(renglon, 1).Value = "IDCLIENTE";
                    sheet.Cell(renglon, 2).Value = "FECHA";
                    sheet.Cell(renglon, 3).Value = "TELEFONO";
                    sheet.Cell(renglon, 4).Value = "TELEFONO CELULAR";
                    sheet.Cell(renglon, 5).Value = "IDSUCURSAL";
                    sheet.Cell(renglon, 6).Value = "SUCURSAL";
                    sheet.Cell(renglon, 7).Value = "NOMBRE";
                    sheet.Cell(renglon, 8).Value = "VALOR ORIGINAL";
                    sheet.Cell(renglon, 9).Value = "REG";
                    sheet.Cell(renglon, 10).Value = "PAGADO";
                    sheet.Cell(renglon, 11).Value = "SALDO";
                    sheet.Cell(renglon, 12).Value = "CREDITO";
                    sheet.Cell(renglon, 13).Value = "TERMINOCRED2";
                    sheet.Cell(renglon, 14).Value = "TERMINOCRED1";
                    sheet.Cell(renglon, 15).Value = "TIPO CLAVE";
                    sheet.Cell(renglon, 16).Value = "INVO";
                    sheet.Cell(renglon, 17).Value = "ORIGEN";
                    sheet.Cell(renglon, 18).Value = "NOMBRE DE MODULO";
                    sheet.Cell(renglon, 19).Value = "SERIE FISCAL";
                    sheet.Cell(renglon, 20).Value = "DOC FISCAL";
                    sheet.Cell(renglon, 21).Value = "NOMBRE DE MODULO 2";
                    sheet.Cell(renglon, 22).Value = "TERMINOCREDX";
                    sheet.Cell(renglon, 23).Value = "TERMINOCRED";
                    sheet.Cell(renglon, 24).Value = "VENCIMIENTO";
                    sheet.Cell(renglon, 25).Value = "MAS 360";
                    sheet.Cell(renglon, 26).Value = "DE 271 A 360";
                    sheet.Cell(renglon, 27).Value = "DE 211 A 270";
                    sheet.Cell(renglon, 28).Value = "DE 151 A 210";
                    sheet.Cell(renglon, 29).Value = "DE 91 A 150";
                    sheet.Cell(renglon, 30).Value = "DE 61 A 90";
                    sheet.Cell(renglon, 31).Value = "DE 31 A 60";
                    sheet.Cell(renglon, 32).Value = "DE 16 A 30";
                    sheet.Cell(renglon, 33).Value = "DE 1 A 15";
                    sheet.Cell(renglon, 34).Value = "POR VENCER";
                    sheet.Cell(renglon, 35).Value = "TOTAL VENCIDO";
                    sheet.Cell(renglon, 36).Value = "SUBTOTAL";
                    sheet.Cell(renglon, 37).Value = "TOTAL";
                    sheet.Cell(renglon, 38).Value = "DIFERENCIA DIAS";


                    var rango = sheet.Range(renglon, 1, renglon, 37);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    foreach (mdlCartera_Vencida activos in lista)
                    {
                        TimeSpan diferencia = DateTime.Now - activos.vencimiento;
                        int diferenciaDias = Math.Abs(diferencia.Days);

                        sheet.Cell(renglon, 1).Value = activos.cliente;
                        sheet.Cell(renglon, 2).Value = activos.fecha;
                        sheet.Cell(renglon, 3).Value = activos.telefono;
                        sheet.Cell(renglon, 4).Value = activos.telefonoCel;
                        sheet.Cell(renglon, 5).Value = activos.sucursal;
                        sheet.Cell(renglon, 6).Value = activos.nombreSucursal;
                        sheet.Cell(renglon, 7).Value = activos.nombre;
                        sheet.Cell(renglon, 8).Value = activos.valororiginal;
                        sheet.Cell(renglon, 9).Value = activos.reg;
                        sheet.Cell(renglon, 10).Value = activos.pagado;
                        sheet.Cell(renglon, 11).Value = activos.saldo;
                        sheet.Cell(renglon, 12).Value = activos.credito;
                        sheet.Cell(renglon, 13).Value = activos.terminocred2;
                        sheet.Cell(renglon, 14).Value = activos.terminocred1;
                        sheet.Cell(renglon, 15).Value = activos.tipoclave;
                        sheet.Cell(renglon, 16).Value = activos.invo;
                        sheet.Cell(renglon, 17).Value = activos.origen;
                        sheet.Cell(renglon, 18).Value = activos.nombremodulo;
                        sheet.Cell(renglon, 19).Value = activos.seriefiscal;
                        sheet.Cell(renglon, 20).Value = activos.docfiscal;
                        sheet.Cell(renglon, 21).Value = activos.nombremodulo2;
                        sheet.Cell(renglon, 22).Value = activos.terminocredX;
                        sheet.Cell(renglon, 23).Value = activos.terminocred;
                        sheet.Cell(renglon, 24).Value = activos.vencimiento;
                        sheet.Cell(renglon, 25).Value = activos.mas360;
                        sheet.Cell(renglon, 26).Value = activos.de271a360;
                        sheet.Cell(renglon, 27).Value = activos.de211a270;
                        sheet.Cell(renglon, 28).Value = activos.de151a210;
                        sheet.Cell(renglon, 29).Value = activos.de91a150;
                        sheet.Cell(renglon, 30).Value = activos.de61a90;
                        sheet.Cell(renglon, 31).Value = activos.de31a60;
                        sheet.Cell(renglon, 32).Value = activos.de16a30;
                        sheet.Cell(renglon, 33).Value = activos.de1a15;
                        sheet.Cell(renglon, 34).Value = activos.porvencer;
                        sheet.Cell(renglon, 35).Value = activos.totalvencido;
                        sheet.Cell(renglon, 36).Value = activos.subtotal;
                        sheet.Cell(renglon, 37).Value = activos.total;
                        sheet.Cell(renglon, 38).Value = diferenciaDias;
                        renglon++;
                    }

                    sheet.Cell(renglon, 2).Value = "TOTALES";
                    sheet.Cell(renglon, 3).FormulaA1 = $"SUBTOTAL(9,C5:C{renglon - 1})";
                    sheet.Cell(renglon, 4).FormulaA1 = $"SUBTOTAL(9,D5:D{renglon - 1})";
                    sheet.Cell(renglon, 5).FormulaA1 = $"SUBTOTAL(9,E5:E{renglon - 1})";
                    sheet.Cell(renglon, 6).FormulaA1 = $"SUBTOTAL(9,F5:F{renglon - 1})";
                    sheet.Cell(renglon, 7).FormulaA1 = $"=C{renglon}/F{renglon}/100";
                    sheet.Cell(renglon, 8).FormulaA1 = $"SUBTOTAL(9,H5:H{renglon - 1})";
                    sheet.Cell(renglon, 9).FormulaA1 = $"=C{renglon}/H{renglon}/100";
                    sheet.Cell(renglon, 10).FormulaA1 = $"SUBTOTAL(9,J5:J{renglon - 1})";
                    sheet.Cell(renglon, 11).FormulaA1 = $"=C{renglon}/J{renglon}/100";
                    sheet.Cell(renglon, 12).FormulaA1 = $"SUBTOTAL(9,L5:L{renglon - 1})";
                    sheet.Cell(renglon, 13).FormulaA1 = $"=C{renglon}/L{renglon}/100";
                    sheet.Cell(renglon, 14).FormulaA1 = $"SUBTOTAL(9,N5:N{renglon - 1})";
                    sheet.Cell(renglon, 15).FormulaA1 = $"SUBTOTAL(9,O5:O{renglon - 1})";
                    sheet.Cell(renglon, 16).FormulaA1 = $"SUBTOTAL(9,P5:P{renglon - 1})";
                    sheet.Cell(renglon, 17).FormulaA1 = $"SUBTOTAL(9,Q5:Q{renglon - 1})";
                    sheet.Cell(renglon, 18).FormulaA1 = $"SUBTOTAL(9,R5:R{renglon - 1})";

                    //sheet.Column(3).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(4).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(5).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(6).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(7).Style.NumberFormat.Format = "0.0 %";
                    //sheet.Column(8).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(9).Style.NumberFormat.Format = "0.0 %";
                    //sheet.Column(10).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(11).Style.NumberFormat.Format = "0.0 %";
                    //sheet.Column(12).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(13).Style.NumberFormat.Format = "0.0 %";
                    //sheet.Column(14).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(15).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(16).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(17).Style.NumberFormat.Format = "#,##0.00";
                    //sheet.Column(18).Style.NumberFormat.Format = "#,##0.00";

                    rango = sheet.Range(renglon, 1, renglon, 18);
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
