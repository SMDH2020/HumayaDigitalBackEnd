using ClosedXML.Excel;
using HD.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD_Finanzas.Modelos.ResultadosSucursal;
using HD_Cobranza;

namespace HD_Reporteria.Finanzas.Excel
{
    public class XLS_Resultados_Sucursal
    {
        public static Task<DocResult> CrearExel(IEnumerable<mdl_Resultado_Sucursal> vm, string subtitulo)
        {
            try
            {
                string ruta = $"C:\\SMDH\\Procesados\\Resultadosporsucursal.xlsx";
                using (var worbook = new XLWorkbook())
                {
                    var sheet = worbook.Worksheets.Add("RESULTADOS POR SUCURSAL");
                    var cell = sheet.Range(1, 1, 1, 20);
                    cell.Value = "MAQUINARIA DEL HUMAYA, S.A. DE C.V.";
                    cell.Style.Font.Bold = true;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    cell.Style.Font.FontSize = 16;
                    cell.Merge();

                    cell = sheet.Range(2, 1, 2, 20);
                    cell.Value = subtitulo;
                    cell.Style.Font.Bold = true;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    cell.Style.Font.FontSize = 12;
                    cell.Merge();

                    int rowHeader = 3;
                    sheet.Cell(rowHeader, 1).Value = "CONCEPTO".ToUpper();
                    sheet.Cell(rowHeader, 2).Value = "NAVOLATO".ToUpper();
                    sheet.Cell(rowHeader, 3).Value = "CAIMANERO";
                    sheet.Cell(rowHeader, 4).Value = "ELDORADO".ToUpper();
                    sheet.Cell(rowHeader, 5).Value = "COSTA RICA";
                    sheet.Cell(rowHeader, 6).Value = "LA CRUZ".ToUpper();
                    sheet.Cell(rowHeader, 7).Value = "EL ROSARIO";
                    sheet.Cell(rowHeader, 8).Value = "VILLA UNION".ToUpper();
                    sheet.Cell(rowHeader, 9).Value = "TEPIC";
                    sheet.Cell(rowHeader, 10).Value = "SAN JOSE".ToUpper();
                    sheet.Cell(rowHeader, 11).Value = "SANTIAGO";
                    sheet.Cell(rowHeader, 12).Value = "TECUALA".ToUpper();
                    sheet.Cell(rowHeader, 13).Value = "LAS VARAS";
                    sheet.Cell(rowHeader, 14).Value = "";
                    sheet.Cell(rowHeader, 15).Value = "TOTAL";
                    sheet.Cell(rowHeader, 16).Value = "%".ToUpper();
                    sheet.Cell(rowHeader, 17).Value = "SINALOA";
                    sheet.Cell(rowHeader, 18).Value = "%".ToUpper();
                    sheet.Cell(rowHeader, 19).Value = "NAYARIT";
                    sheet.Cell(rowHeader, 20).Value = "%".ToUpper();

                    cell = sheet.Range(rowHeader, 1, rowHeader, 20);
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.FromArgb(155, 176, 111);
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    rowHeader++;
                    var grupos = vm.GroupBy(x => x.concepto).ToList();
                    foreach (var gpo in grupos)
                    {
                        var total = vm.Where(x => x.concepto == gpo.Key && x.iddepartamento == 0)
                            .FirstOrDefault();

                        sheet.Cell(rowHeader, 1).Value = gpo.Key;
                        sheet.Cell(rowHeader, 2).Value = total is null ? 0 : total.navolato / total.total;
                        sheet.Cell(rowHeader, 3).Value = total is null ? 0 : total.caimanero / total.total;
                        sheet.Cell(rowHeader, 4).Value = total is null ? 0 : total.eldorado / total.total;
                        sheet.Cell(rowHeader, 5).Value = total is null ? 0 : total.costarica / total.total;
                        sheet.Cell(rowHeader, 6).Value = total is null ? 0 : total.lacruz / total.total;
                        sheet.Cell(rowHeader, 7).Value = total is null ? 0 : total.rosario / total.total;
                        sheet.Cell(rowHeader, 8).Value = total is null ? 0 : total.villaunion / total.total;
                        sheet.Cell(rowHeader, 9).Value = total is null ? 0 : total.tepic / total.total;
                        sheet.Cell(rowHeader, 10).Value = total is null ? 0 : total.sanjose / total.total;
                        sheet.Cell(rowHeader, 11).Value = total is null ? 0 : total.santiago / total.total;
                        sheet.Cell(rowHeader, 12).Value = total is null ? 0 : total.tecuala / total.total;
                        sheet.Cell(rowHeader, 13).Value = total is null ? 0 : total.lasvaras / total.total;
                        sheet.Cell(rowHeader, 2).Style.NumberFormat.Format = "0.0#%";
                        sheet.Cell(rowHeader, 3).Style.NumberFormat.Format = "0.0#%";
                        sheet.Cell(rowHeader, 4).Style.NumberFormat.Format = "0.0#%";
                        sheet.Cell(rowHeader, 5).Style.NumberFormat.Format = "0.0#%";
                        sheet.Cell(rowHeader, 6).Style.NumberFormat.Format = "0.0#%";
                        sheet.Cell(rowHeader, 7).Style.NumberFormat.Format = "0.0#%";
                        sheet.Cell(rowHeader, 8).Style.NumberFormat.Format = "0.0#%";
                        sheet.Cell(rowHeader, 9).Style.NumberFormat.Format = "0.0#%";
                        sheet.Cell(rowHeader, 10).Style.NumberFormat.Format = "0.0#%";
                        sheet.Cell(rowHeader, 11).Style.NumberFormat.Format = "0.0#%";
                        sheet.Cell(rowHeader, 12).Style.NumberFormat.Format = "0.0#%";
                        sheet.Cell(rowHeader, 13).Style.NumberFormat.Format = "0.0#%";
                        cell = sheet.Range(rowHeader, 1, rowHeader, 20);
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.BackgroundColor = XLColor.FromArgb(188, 210, 138);
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        cell = sheet.Range(rowHeader, 2, rowHeader, 14);
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        rowHeader++;
                        var detalle = vm.Where(x => x.concepto == gpo.Key);
                        foreach (var item in detalle)
                        {
                            sheet.Cell(rowHeader, 1).Value = item.departamento;
                            sheet.Cell(rowHeader, 2).Value = item.navolato;
                            sheet.Cell(rowHeader, 3).Value = item.caimanero;
                            sheet.Cell(rowHeader, 4).Value = item.eldorado;
                            sheet.Cell(rowHeader, 5).Value = item.costarica;
                            sheet.Cell(rowHeader, 6).Value = item.lacruz;
                            sheet.Cell(rowHeader, 7).Value = item.rosario;
                            sheet.Cell(rowHeader, 8).Value = item.villaunion;
                            sheet.Cell(rowHeader, 9).Value = item.tepic;
                            sheet.Cell(rowHeader, 10).Value = item.sanjose;
                            sheet.Cell(rowHeader, 11).Value = item.santiago;
                            sheet.Cell(rowHeader, 12).Value = item.tecuala;
                            sheet.Cell(rowHeader, 13).Value = item.lasvaras;
                            sheet.Cell(rowHeader, 14).Value = "";
                            sheet.Cell(rowHeader, 15).Value = item.total;
                            sheet.Cell(rowHeader, 16).Value = item.portotal / 100;
                            sheet.Cell(rowHeader, 17).Value = item.sinaloa;
                            sheet.Cell(rowHeader, 18).Value = item.porsinaloa / 100;
                            sheet.Cell(rowHeader, 19).Value = item.nayarit;
                            sheet.Cell(rowHeader, 20).Value = item.pornayarit / 100;
                            sheet.Cell(rowHeader, 2).Style.NumberFormat.Format = "#,##0";
                            sheet.Cell(rowHeader, 3).Style.NumberFormat.Format = "#,##0";
                            sheet.Cell(rowHeader, 4).Style.NumberFormat.Format = "#,##0";
                            sheet.Cell(rowHeader, 5).Style.NumberFormat.Format = "#,##0";
                            sheet.Cell(rowHeader, 6).Style.NumberFormat.Format = "#,##0";
                            sheet.Cell(rowHeader, 7).Style.NumberFormat.Format = "#,##0";
                            sheet.Cell(rowHeader, 8).Style.NumberFormat.Format = "#,##0";
                            sheet.Cell(rowHeader, 9).Style.NumberFormat.Format = "#,##0";
                            sheet.Cell(rowHeader, 10).Style.NumberFormat.Format = "#,##0";
                            sheet.Cell(rowHeader, 11).Style.NumberFormat.Format = "#,##0";
                            sheet.Cell(rowHeader, 12).Style.NumberFormat.Format = "#,##0";
                            sheet.Cell(rowHeader, 13).Style.NumberFormat.Format = "#,##0";
                            //sheet.Cell(rowHeader, 14).Style.NumberFormat.Format = "#,##0";
                            sheet.Cell(rowHeader, 15).Style.NumberFormat.Format = "#,##0";
                            sheet.Cell(rowHeader, 16).Style.NumberFormat.Format = "0.0#%";
                            sheet.Cell(rowHeader, 17).Style.NumberFormat.Format = "#,##0";
                            sheet.Cell(rowHeader, 18).Style.NumberFormat.Format = "0.0#%";
                            sheet.Cell(rowHeader, 19).Style.NumberFormat.Format = "#,##0";
                            sheet.Cell(rowHeader, 20).Style.NumberFormat.Format = "0.0#%";
                            rowHeader++;
                        }
                        rowHeader += 1;
                    }

                    sheet.Columns().AdjustToContents();
                    worbook.SaveAs(ruta);
                }
                if (System.IO.File.Exists(ruta))
                {
                    byte[] docbytes = System.IO.File.ReadAllBytes(ruta);
                    string docBase64 = Convert.ToBase64String(docbytes);
                    System.IO.File.Delete(ruta);

                    DocResult doc = new DocResult
                    {
                        documento = docBase64,
                        filename = "Resultados por sucursal"
                    };

                    return Task.FromResult(doc);

                }
                DocResult result = new DocResult();
                return Task.FromResult(result);
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { errores = ex.Message });
            }
        }
    }
}
