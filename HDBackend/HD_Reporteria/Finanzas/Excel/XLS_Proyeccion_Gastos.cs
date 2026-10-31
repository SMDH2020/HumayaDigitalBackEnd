using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Cobranza;
using HD_Finanzas.Modelos.ProyeccionesGastos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Reporteria.Finanzas.Excel
{
    public class XLS_Proyeccion_Gastos
    {
        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_Poryeccion_Gasto_Anual> lista, int ejercicio)
        {
            try
            {
                string ruta = $"C:\\SMDH\\Procesados\\Proyeccion de Gastos {ejercicio}.xlsx";
                using (var worbook = new XLWorkbook())
                {
                    //Se agrupara la lista para obtener las sucursales que vienen en la lista
                    var grupo_sucursales = lista
                        .GroupBy(x => new { x.idsucursal, x.sucursal, x.sucnom, x.idadr, x.adr })
                        .Select(x => new { id = x.Key.idsucursal, sucursal = x.Key.sucursal, nomenclatura = x.Key.sucnom, idadr = x.Key.idadr, adr = x.Key.adr })
                        .ToList();
                    var grupo_cuentas_variables = lista.Where(x => x.tipo == "V")
                        .GroupBy(x => new { x.cuenta, x.concepto })
                        .Select(x => new { cuenta = x.Key.cuenta, concepto = x.Key.concepto })
                        .ToList();
                    var grupo_cuentas_fijas = lista.Where(x => x.tipo == "F")
                        .GroupBy(x => new { x.cuenta, x.concepto })
                        .Select(x => new { cuenta = x.Key.cuenta, concepto = x.Key.concepto })
                        .ToList();
                    List<directorio> directorio = new List<directorio>();
                    List<DirectorioDepartamentos> dirDepartamentos = new List<DirectorioDepartamentos>();
                    foreach (var suc in grupo_sucursales)
                    {
                        List<string> Nombre_hojas = new List<string>();
                        //se obtendran todos los registos correspondientes por secursal recorrida en el ciclo
                        var lista_By_Sucursal = lista.Where(x => x.idsucursal == suc.id).ToList();
                        //se agruparan los departamentos que se obtubieron del filtro por sucursal
                        var grupo_departamentos = lista_By_Sucursal
                                .GroupBy(x => new { x.iddepartamento, x.departamento, x.depnom })
                                .Select(x => new { id = x.Key.iddepartamento, departamento = x.Key.departamento, nomenclatura = x.Key.depnom })
                                .ToList();
                        foreach (var dep in grupo_departamentos)
                        {
                            if (dirDepartamentos.Where(x => x.departamento == dep.departamento).FirstOrDefault() is null)
                            {
                                var newdirdep = new DirectorioDepartamentos();
                                List<string> nelistadepartamentosdirecion = new List<string>();
                                nelistadepartamentosdirecion.Add($"{dep.nomenclatura} {suc.nomenclatura}");
                                newdirdep.departamento = dep.departamento;
                                newdirdep.iddepartamento = dep.id.ToString();
                                newdirdep.nomenclatura = dep.nomenclatura;
                                newdirdep.hojas = nelistadepartamentosdirecion;
                                dirDepartamentos.Add(newdirdep);
                            }
                            else
                            {
                                dirDepartamentos.Where(x => x.departamento == dep.departamento)
                                    .FirstOrDefault()
                                    .hojas.Add($"{dep.nomenclatura} {suc.nomenclatura}");
                            }
                            var sheet = worbook.Worksheets.Add($"{dep.nomenclatura} {suc.nomenclatura}");
                            Nombre_hojas.Add($"{dep.nomenclatura} {suc.nomenclatura}");
                            sheet.Row(1).Height = 30;

                            //Encabezado
                            var cell = sheet.Range(1, 1, 1, 15);
                            cell.Value = "PROYECCIÓN DE GASTOS";
                            cell.Style.Font.Bold = true;
                            cell.Style.Font.FontSize = 16;
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFDE00");
                            cell.Merge();

                            //subtitulos
                            cell = sheet.Range(2, 1, 2, 15);
                            cell.Value = $"Sucursal: {suc.id} - {suc.sucursal}     Departamento: {dep.id} - {dep.departamento}     Ejercicio: {ejercicio}";
                            cell.Style.Font.FontSize = 12;
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFDE00");
                            cell.Merge();

                            //Titulos Detalle
                            cell = sheet.Range(3, 1, 3, 15);
                            cell.Style.Font.FontSize = 12;
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#367C2B");
                            cell.Style.Font.FontColor = XLColor.White;

                            sheet.Cell(3, 1).Value = "CUENTA";
                            sheet.Cell(3, 2).Value = "CONCEPTO";
                            sheet.Cell(3, 3).Value = "ENERO";
                            sheet.Cell(3, 4).Value = "FEBRERO";
                            sheet.Cell(3, 5).Value = "MARZO";
                            sheet.Cell(3, 6).Value = "ABRIL";
                            sheet.Cell(3, 7).Value = "MAYO";
                            sheet.Cell(3, 8).Value = "JUNIO";
                            sheet.Cell(3, 9).Value = "JULIO";
                            sheet.Cell(3, 10).Value = "AGOSTO";
                            sheet.Cell(3, 11).Value = "SEPTIEMBRE";
                            sheet.Cell(3, 12).Value = "OCTUBRE";
                            sheet.Cell(3, 13).Value = "NOVIEMBRE";
                            sheet.Cell(3, 14).Value = "DICIEMBRE";
                            sheet.Cell(3, 15).Value = "ACUMULADO";

                            //Renglon donde se iniciara con el detalle
                            int renglon = 4;
                            var lista_detalle = lista_By_Sucursal.Where(x => x.iddepartamento == dep.id).ToList();
                            //var gastos_variables = lista_detalle.Where(x => x.tipo == "V").ToList();
                            foreach (var pg in grupo_cuentas_variables)
                            {
                                var detalle = lista_detalle.Where(x => x.cuenta == pg.cuenta).FirstOrDefault();
                                sheet.Cell(renglon, 1).Value = pg.cuenta;
                                sheet.Cell(renglon, 2).Value = pg.concepto.ToUpper();
                                sheet.Cell(renglon, 3).Value = detalle is null ? 0 : detalle.enero;
                                sheet.Cell(renglon, 4).Value = detalle is null ? 0 : detalle.febrero;
                                sheet.Cell(renglon, 5).Value = detalle is null ? 0 : detalle.marzo;
                                sheet.Cell(renglon, 6).Value = detalle is null ? 0 : detalle.abril;
                                sheet.Cell(renglon, 7).Value = detalle is null ? 0 : detalle.mayo;
                                sheet.Cell(renglon, 8).Value = detalle is null ? 0 : detalle.junio;
                                sheet.Cell(renglon, 9).Value = detalle is null ? 0 : detalle.julio;
                                sheet.Cell(renglon, 10).Value = detalle is null ? 0 : detalle.agosto;
                                sheet.Cell(renglon, 11).Value = detalle is null ? 0 : detalle.septiembre;
                                sheet.Cell(renglon, 12).Value = detalle is null ? 0 : detalle.octubre;
                                sheet.Cell(renglon, 13).Value = detalle is null ? 0 : detalle.noviembre;
                                sheet.Cell(renglon, 14).Value = detalle is null ? 0 : detalle.diciembre;
                                sheet.Cell(renglon, 15).SetFormulaA1($"=SUM(C{renglon}:N{renglon})");
                                renglon += 1;
                            }
                            cell = sheet.Range(renglon, 1, renglon, 15);
                            cell.Style.Font.Bold = true;
                            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#CCFFCC");

                            cell = sheet.Range(renglon, 1, renglon, 2);
                            cell.Merge();
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            sheet.Cell(renglon, 1).Value = "ACUMULADO VARIABLE";

                            sheet.Cell(renglon, 3).SetFormulaA1($"=SUM(C4:C{renglon - 1})");
                            sheet.Cell(renglon, 4).SetFormulaA1($"=SUM(D4:D{renglon - 1})");
                            sheet.Cell(renglon, 5).SetFormulaA1($"=SUM(E4:E{renglon - 1})");
                            sheet.Cell(renglon, 6).SetFormulaA1($"=SUM(F4:F{renglon - 1})");
                            sheet.Cell(renglon, 7).SetFormulaA1($"=SUM(G4:G{renglon - 1})");
                            sheet.Cell(renglon, 8).SetFormulaA1($"=SUM(H4:H{renglon - 1})");
                            sheet.Cell(renglon, 9).SetFormulaA1($"=SUM(I4:I{renglon - 1})");
                            sheet.Cell(renglon, 10).SetFormulaA1($"=SUM(J4:J{renglon - 1})");
                            sheet.Cell(renglon, 11).SetFormulaA1($"=SUM(K4:K{renglon - 1})");
                            sheet.Cell(renglon, 12).SetFormulaA1($"=SUM(L4:L{renglon - 1})");
                            sheet.Cell(renglon, 13).SetFormulaA1($"=SUM(M4:M{renglon - 1})");
                            sheet.Cell(renglon, 14).SetFormulaA1($"=SUM(N4:N{renglon - 1})");
                            sheet.Cell(renglon, 15).SetFormulaA1($"=SUM(O4:O{renglon - 1})");

                            renglon += 2;
                            var renglon_inicial = renglon;

                            //var gastos_fijos = lista_detalle.Where(x => x.tipo == "F").ToList();
                            foreach (var pg in grupo_cuentas_fijas)
                            {
                                var detalle = lista_detalle.Where(x => x.cuenta == pg.cuenta).FirstOrDefault();
                                sheet.Cell(renglon, 1).Value = pg.cuenta;
                                sheet.Cell(renglon, 2).Value = pg.concepto.ToUpper();
                                sheet.Cell(renglon, 3).Value = detalle is null ? 0 : detalle.enero;
                                sheet.Cell(renglon, 4).Value = detalle is null ? 0 : detalle.febrero;
                                sheet.Cell(renglon, 5).Value = detalle is null ? 0 : detalle.marzo;
                                sheet.Cell(renglon, 6).Value = detalle is null ? 0 : detalle.abril;
                                sheet.Cell(renglon, 7).Value = detalle is null ? 0 : detalle.mayo;
                                sheet.Cell(renglon, 8).Value = detalle is null ? 0 : detalle.junio;
                                sheet.Cell(renglon, 9).Value = detalle is null ? 0 : detalle.julio;
                                sheet.Cell(renglon, 10).Value = detalle is null ? 0 : detalle.agosto;
                                sheet.Cell(renglon, 11).Value = detalle is null ? 0 : detalle.septiembre;
                                sheet.Cell(renglon, 12).Value = detalle is null ? 0 : detalle.octubre;
                                sheet.Cell(renglon, 13).Value = detalle is null ? 0 : detalle.noviembre;
                                sheet.Cell(renglon, 14).Value = detalle is null ? 0 : detalle.diciembre;
                                sheet.Cell(renglon, 15).SetFormulaA1($"=SUM(C{renglon}:N{renglon})");
                                renglon += 1;
                            }
                            cell = sheet.Range(renglon, 1, renglon, 15);
                            cell.Style.Font.Bold = true;
                            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#CCFFCC");

                            cell = sheet.Range(renglon, 1, renglon, 2);
                            cell.Merge();
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            sheet.Cell(renglon, 1).Value = "ACUMULADO FIJOS";

                            sheet.Cell(renglon, 3).SetFormulaA1($"=SUM(C{renglon_inicial}:C{renglon - 1})");
                            sheet.Cell(renglon, 4).SetFormulaA1($"=SUM(D{renglon_inicial}:D{renglon - 1})");
                            sheet.Cell(renglon, 5).SetFormulaA1($"=SUM(E{renglon_inicial}:E{renglon - 1})");
                            sheet.Cell(renglon, 6).SetFormulaA1($"=SUM(F{renglon_inicial}:F{renglon - 1})");
                            sheet.Cell(renglon, 7).SetFormulaA1($"=SUM(G{renglon_inicial}:G{renglon - 1})");
                            sheet.Cell(renglon, 8).SetFormulaA1($"=SUM(H{renglon_inicial}:H{renglon - 1})");
                            sheet.Cell(renglon, 9).SetFormulaA1($"=SUM(I{renglon_inicial}:I{renglon - 1})");
                            sheet.Cell(renglon, 10).SetFormulaA1($"=SUM(J{renglon_inicial}:J{renglon - 1})");
                            sheet.Cell(renglon, 11).SetFormulaA1($"=SUM(K{renglon_inicial}:K{renglon - 1})");
                            sheet.Cell(renglon, 12).SetFormulaA1($"=SUM(L{renglon_inicial}:L{renglon - 1})");
                            sheet.Cell(renglon, 13).SetFormulaA1($"=SUM(M{renglon_inicial}:M{renglon - 1})");
                            sheet.Cell(renglon, 14).SetFormulaA1($"=SUM(N{renglon_inicial}:N{renglon - 1})");
                            sheet.Cell(renglon, 15).SetFormulaA1($"=SUM(O{renglon_inicial}:O{renglon - 1})");

                            renglon += 2;
                            cell = sheet.Range(renglon, 1, renglon, 15);
                            cell.Style.Font.Bold = true;
                            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#33BBFF");

                            cell = sheet.Range(renglon, 1, renglon, 2);
                            cell.Merge();
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            sheet.Cell(renglon, 1).Value = "ACUMULADO";

                            sheet.Cell(renglon, 3).SetFormulaA1($"=C{renglon_inicial - 2} + C{renglon - 2}");
                            sheet.Cell(renglon, 4).SetFormulaA1($"=D{renglon_inicial - 2} + D{renglon - 2}");
                            sheet.Cell(renglon, 5).SetFormulaA1($"=E{renglon_inicial - 2} + E{renglon - 2}");
                            sheet.Cell(renglon, 6).SetFormulaA1($"=F{renglon_inicial - 2} + F{renglon - 2}");
                            sheet.Cell(renglon, 7).SetFormulaA1($"=G{renglon_inicial - 2} + G{renglon - 2}");
                            sheet.Cell(renglon, 8).SetFormulaA1($"=H{renglon_inicial - 2} + H{renglon - 2}");
                            sheet.Cell(renglon, 9).SetFormulaA1($"=I{renglon_inicial - 2} + I{renglon - 2}");
                            sheet.Cell(renglon, 10).SetFormulaA1($"=J{renglon_inicial - 2} + J{renglon - 2}");
                            sheet.Cell(renglon, 11).SetFormulaA1($"=K{renglon_inicial - 2} + K{renglon - 2}");
                            sheet.Cell(renglon, 12).SetFormulaA1($"=L{renglon_inicial - 2} + L{renglon - 2}");
                            sheet.Cell(renglon, 13).SetFormulaA1($"=M{renglon_inicial - 2} + M{renglon - 2}");
                            sheet.Cell(renglon, 14).SetFormulaA1($"=N{renglon_inicial - 2} + N{renglon - 2}");
                            sheet.Cell(renglon, 15).SetFormulaA1($"=O{renglon_inicial - 2} + O{renglon - 2}");


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
                            sheet.Column(14).Style.NumberFormat.Format = "#,##0.00";
                            sheet.Column(15).Style.NumberFormat.Format = "#,##0.00";
                            sheet.Columns().AdjustToContents();
                        }
                        //SUCURSALES

                        directorio.Add(new directorio
                        {
                            idsucursal = suc.id,
                            nombre = suc.sucursal,
                            nomenclatura = suc.nomenclatura,
                            hojas = Nombre_hojas
                        });

                        var sheetsuc = worbook.Worksheets.Add($"{suc.sucursal}");
                        sheetsuc.TabColor = XLColor.FromHtml("FFFF00");
                        sheetsuc.Row(1).Height = 30;

                        //Encabezado
                        var cellsuc = sheetsuc.Range(1, 1, 1, 15);
                        cellsuc.Value = "PROYECCIÓN DE GASTOS";
                        cellsuc.Style.Font.Bold = true;
                        cellsuc.Style.Font.FontSize = 16;
                        cellsuc.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        cellsuc.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        cellsuc.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFDE00");
                        cellsuc.Merge();

                        //subtitulos
                        cellsuc = sheetsuc.Range(2, 1, 2, 15);
                        cellsuc.Value = $"Sucursal: {suc.id} - {suc.sucursal}     Departamento: Todos     Ejercicio: {ejercicio}";
                        cellsuc.Style.Font.FontSize = 12;
                        cellsuc.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        cellsuc.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        cellsuc.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFDE00");
                        cellsuc.Merge();

                        //Titulos Detalle
                        cellsuc = sheetsuc.Range(3, 1, 3, 15);
                        cellsuc.Style.Font.FontSize = 12;
                        cellsuc.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        cellsuc.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        cellsuc.Style.Fill.BackgroundColor = XLColor.FromHtml("#367C2B");
                        cellsuc.Style.Font.FontColor = XLColor.White;

                        sheetsuc.Cell(3, 1).Value = "CUENTA";
                        sheetsuc.Cell(3, 2).Value = "CONCEPTO";
                        sheetsuc.Cell(3, 3).Value = "ENERO";
                        sheetsuc.Cell(3, 4).Value = "FEBRERO";
                        sheetsuc.Cell(3, 5).Value = "MARZO";
                        sheetsuc.Cell(3, 6).Value = "ABRIL";
                        sheetsuc.Cell(3, 7).Value = "MAYO";
                        sheetsuc.Cell(3, 8).Value = "JUNIO";
                        sheetsuc.Cell(3, 9).Value = "JULIO";
                        sheetsuc.Cell(3, 10).Value = "AGOSTO";
                        sheetsuc.Cell(3, 11).Value = "SEPTIEMBRE";
                        sheetsuc.Cell(3, 12).Value = "OCTUBRE";
                        sheetsuc.Cell(3, 13).Value = "NOVIEMBRE";
                        sheetsuc.Cell(3, 14).Value = "DICIEMBRE";
                        sheetsuc.Cell(3, 15).Value = "ACUMULADO";

                        int sucrenglon = 4;
                        string cadenaENE = "=";
                        string cadenaFEB = "=";
                        string cadenaMAR = "=";
                        string cadenaABR = "=";
                        string cadenaMAY = "=";
                        string cadenaJUN = "=";
                        string cadenaJUL = "=";
                        string cadenaAGO = "=";
                        string cadenaSEP = "=";
                        string cadenaOCT = "=";
                        string cadenaNOV = "=";
                        string cadenaDIC = "=";
                        string cadenaACUM = "=";
                        foreach (string str in Nombre_hojas)
                        {
                            cadenaENE += "+'" + str + "'!C{0}";
                            cadenaFEB += "+'" + str + "'!D{0}";
                            cadenaMAR += "+'" + str + "'!E{0}";
                            cadenaABR += "+'" + str + "'!F{0}";
                            cadenaMAY += "+'" + str + "'!G{0}";
                            cadenaJUN += "+'" + str + "'!H{0}";
                            cadenaJUL += "+'" + str + "'!I{0}";
                            cadenaAGO += "+'" + str + "'!J{0}";
                            cadenaSEP += "+'" + str + "'!K{0}";
                            cadenaOCT += "+'" + str + "'!L{0}";
                            cadenaNOV += "+'" + str + "'!M{0}";
                            cadenaDIC += "+'" + str + "'!N{0}";
                            cadenaACUM += "+'" + str + "'!O{0}";
                        }
                        foreach (var variables in grupo_cuentas_variables)
                        {
                            sheetsuc.Cell(sucrenglon, 1).Value = variables.cuenta;
                            sheetsuc.Cell(sucrenglon, 2).Value = variables.concepto;
                            sheetsuc.Cell(sucrenglon, 3).SetFormulaA1(string.Format(cadenaENE, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 4).SetFormulaA1(string.Format(cadenaFEB, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 5).SetFormulaA1(string.Format(cadenaMAR, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 6).SetFormulaA1(string.Format(cadenaABR, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 7).SetFormulaA1(string.Format(cadenaMAY, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 8).SetFormulaA1(string.Format(cadenaJUN, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 9).SetFormulaA1(string.Format(cadenaJUL, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 10).SetFormulaA1(string.Format(cadenaAGO, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 11).SetFormulaA1(string.Format(cadenaSEP, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 12).SetFormulaA1(string.Format(cadenaOCT, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 13).SetFormulaA1(string.Format(cadenaNOV, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 14).SetFormulaA1(string.Format(cadenaDIC, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 15).SetFormulaA1(string.Format(cadenaACUM, sucrenglon));
                            sucrenglon += 1;
                        }
                        cellsuc = sheetsuc.Range(sucrenglon, 1, sucrenglon, 15);
                        cellsuc.Style.Font.Bold = true;
                        cellsuc.Style.Fill.BackgroundColor = XLColor.FromHtml("#CCFFCC");

                        cellsuc = sheetsuc.Range(sucrenglon, 1, sucrenglon, 2);
                        cellsuc.Merge();
                        cellsuc.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        sheetsuc.Cell(sucrenglon, 1).Value = "ACUMULADO VARIABLE";

                        sheetsuc.Cell(sucrenglon, 3).SetFormulaA1($"=SUM(C4:C{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 4).SetFormulaA1($"=SUM(D4:D{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 5).SetFormulaA1($"=SUM(E4:E{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 6).SetFormulaA1($"=SUM(F4:F{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 7).SetFormulaA1($"=SUM(G4:G{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 8).SetFormulaA1($"=SUM(H4:H{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 9).SetFormulaA1($"=SUM(I4:I{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 10).SetFormulaA1($"=SUM(J4:J{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 11).SetFormulaA1($"=SUM(K4:K{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 12).SetFormulaA1($"=SUM(L4:L{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 13).SetFormulaA1($"=SUM(M4:M{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 14).SetFormulaA1($"=SUM(N4:N{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 15).SetFormulaA1($"=SUM(O4:O{sucrenglon - 1})");

                        sucrenglon += 2;
                        var renglon_inicial_suc = sucrenglon;

                        foreach (var variables in grupo_cuentas_fijas)
                        {
                            sheetsuc.Cell(sucrenglon, 1).Value = variables.cuenta;
                            sheetsuc.Cell(sucrenglon, 2).Value = variables.concepto;
                            sheetsuc.Cell(sucrenglon, 3).SetFormulaA1(string.Format(cadenaENE, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 4).SetFormulaA1(string.Format(cadenaFEB, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 5).SetFormulaA1(string.Format(cadenaMAR, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 6).SetFormulaA1(string.Format(cadenaABR, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 7).SetFormulaA1(string.Format(cadenaMAY, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 8).SetFormulaA1(string.Format(cadenaJUN, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 9).SetFormulaA1(string.Format(cadenaJUL, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 10).SetFormulaA1(string.Format(cadenaAGO, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 11).SetFormulaA1(string.Format(cadenaSEP, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 12).SetFormulaA1(string.Format(cadenaOCT, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 13).SetFormulaA1(string.Format(cadenaNOV, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 14).SetFormulaA1(string.Format(cadenaDIC, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 15).SetFormulaA1(string.Format(cadenaACUM, sucrenglon));
                            sucrenglon += 1;
                        }
                        cellsuc = sheetsuc.Range(sucrenglon, 1, sucrenglon, 15);
                        cellsuc.Style.Font.Bold = true;
                        cellsuc.Style.Fill.BackgroundColor = XLColor.FromHtml("#CCFFCC");

                        cellsuc = sheetsuc.Range(sucrenglon, 1, sucrenglon, 2);
                        cellsuc.Merge();
                        cellsuc.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        sheetsuc.Cell(sucrenglon, 1).Value = "ACUMULADO VARIABLE";

                        sheetsuc.Cell(sucrenglon, 3).SetFormulaA1($"=SUM(C{renglon_inicial_suc}:C{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 4).SetFormulaA1($"=SUM(D{renglon_inicial_suc}:D{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 5).SetFormulaA1($"=SUM(E{renglon_inicial_suc}:E{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 6).SetFormulaA1($"=SUM(F{renglon_inicial_suc}:F{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 7).SetFormulaA1($"=SUM(G{renglon_inicial_suc}:G{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 8).SetFormulaA1($"=SUM(H{renglon_inicial_suc}:H{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 9).SetFormulaA1($"=SUM(I{renglon_inicial_suc}:I{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 10).SetFormulaA1($"=SUM(J{renglon_inicial_suc}:J{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 11).SetFormulaA1($"=SUM(K{renglon_inicial_suc}:K{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 12).SetFormulaA1($"=SUM(L{renglon_inicial_suc}:L{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 13).SetFormulaA1($"=SUM(M{renglon_inicial_suc}:M{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 14).SetFormulaA1($"=SUM(N{renglon_inicial_suc}:N{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 15).SetFormulaA1($"=SUM(O{renglon_inicial_suc}:O{sucrenglon - 1})");
                        sucrenglon += 2;
                        cellsuc = sheetsuc.Range(sucrenglon, 1, sucrenglon, 15);
                        cellsuc.Style.Font.Bold = true;
                        cellsuc.Style.Fill.BackgroundColor = XLColor.FromHtml("#33BBFF");

                        cellsuc = sheetsuc.Range(sucrenglon, 1, sucrenglon, 2);
                        cellsuc.Merge();
                        cellsuc.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        sheetsuc.Cell(sucrenglon, 1).Value = "ACUMULADO";

                        sheetsuc.Cell(sucrenglon, 3).SetFormulaA1($"=C{renglon_inicial_suc - 2} + C{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 4).SetFormulaA1($"=D{renglon_inicial_suc - 2} + D{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 5).SetFormulaA1($"=E{renglon_inicial_suc - 2} + E{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 6).SetFormulaA1($"=F{renglon_inicial_suc - 2} + F{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 7).SetFormulaA1($"=G{renglon_inicial_suc - 2} + G{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 8).SetFormulaA1($"=H{renglon_inicial_suc - 2} + H{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 9).SetFormulaA1($"=I{renglon_inicial_suc - 2} + I{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 10).SetFormulaA1($"=J{renglon_inicial_suc - 2} + J{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 11).SetFormulaA1($"=K{renglon_inicial_suc - 2} + K{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 12).SetFormulaA1($"=L{renglon_inicial_suc - 2} + L{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 13).SetFormulaA1($"=M{renglon_inicial_suc - 2} + M{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 14).SetFormulaA1($"=N{renglon_inicial_suc - 2} + N{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 15).SetFormulaA1($"=O{renglon_inicial_suc - 2} + O{sucrenglon - 2}");


                        sheetsuc.Column(3).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(4).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(5).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(6).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(7).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(8).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(9).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(10).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(11).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(12).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(13).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(14).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(15).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Columns().AdjustToContents();
                    }

                    foreach (var tapdep in dirDepartamentos)
                    {
                        var sheetsuc = worbook.Worksheets.Add($"{tapdep.nomenclatura} GPO");
                        sheetsuc.TabColor = XLColor.FromHtml("7030A0");
                        sheetsuc.Row(1).Height = 30;

                        //Encabezado
                        var cellsuc = sheetsuc.Range(1, 1, 1, 15);
                        cellsuc.Value = "PROYECCIÓN DE GASTOS";
                        cellsuc.Style.Font.Bold = true;
                        cellsuc.Style.Font.FontSize = 16;
                        cellsuc.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        cellsuc.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        cellsuc.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFDE00");
                        cellsuc.Merge();

                        //subtitulos
                        cellsuc = sheetsuc.Range(2, 1, 2, 15);
                        cellsuc.Value = $"Departamento: {tapdep.iddepartamento} - {tapdep.departamento}     Ejercicio: {ejercicio}";
                        cellsuc.Style.Font.FontSize = 12;
                        cellsuc.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        cellsuc.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        cellsuc.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFDE00");
                        cellsuc.Merge();

                        //Titulos Detalle
                        cellsuc = sheetsuc.Range(3, 1, 3, 15);
                        cellsuc.Style.Font.FontSize = 12;
                        cellsuc.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        cellsuc.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        cellsuc.Style.Fill.BackgroundColor = XLColor.FromHtml("#367C2B");
                        cellsuc.Style.Font.FontColor = XLColor.White;

                        sheetsuc.Cell(3, 1).Value = "CUENTA";
                        sheetsuc.Cell(3, 2).Value = "CONCEPTO";
                        sheetsuc.Cell(3, 3).Value = "ENERO";
                        sheetsuc.Cell(3, 4).Value = "FEBRERO";
                        sheetsuc.Cell(3, 5).Value = "MARZO";
                        sheetsuc.Cell(3, 6).Value = "ABRIL";
                        sheetsuc.Cell(3, 7).Value = "MAYO";
                        sheetsuc.Cell(3, 8).Value = "JUNIO";
                        sheetsuc.Cell(3, 9).Value = "JULIO";
                        sheetsuc.Cell(3, 10).Value = "AGOSTO";
                        sheetsuc.Cell(3, 11).Value = "SEPTIEMBRE";
                        sheetsuc.Cell(3, 12).Value = "OCTUBRE";
                        sheetsuc.Cell(3, 13).Value = "NOVIEMBRE";
                        sheetsuc.Cell(3, 14).Value = "DICIEMBRE";
                        sheetsuc.Cell(3, 15).Value = "ACUMULADO";

                        //Renglon donde se iniciara con el detalle
                        int sucrenglon = 4;
                        string cadenaENE = "=";
                        string cadenaFEB = "=";
                        string cadenaMAR = "=";
                        string cadenaABR = "=";
                        string cadenaMAY = "=";
                        string cadenaJUN = "=";
                        string cadenaJUL = "=";
                        string cadenaAGO = "=";
                        string cadenaSEP = "=";
                        string cadenaOCT = "=";
                        string cadenaNOV = "=";
                        string cadenaDIC = "=";
                        string cadenaACUM = "=";
                        foreach (string str in tapdep.hojas)
                        {
                            cadenaENE += "+'" + str + "'!C{0}";
                            cadenaFEB += "+'" + str + "'!D{0}";
                            cadenaMAR += "+'" + str + "'!E{0}";
                            cadenaABR += "+'" + str + "'!F{0}";
                            cadenaMAY += "+'" + str + "'!G{0}";
                            cadenaJUN += "+'" + str + "'!H{0}";
                            cadenaJUL += "+'" + str + "'!I{0}";
                            cadenaAGO += "+'" + str + "'!J{0}";
                            cadenaSEP += "+'" + str + "'!K{0}";
                            cadenaOCT += "+'" + str + "'!L{0}";
                            cadenaNOV += "+'" + str + "'!M{0}";
                            cadenaDIC += "+'" + str + "'!N{0}";
                            cadenaACUM += "+'" + str + "'!O{0}";
                        }
                        foreach (var variables in grupo_cuentas_variables)
                        {
                            sheetsuc.Cell(sucrenglon, 1).Value = variables.cuenta;
                            sheetsuc.Cell(sucrenglon, 2).Value = variables.concepto;
                            sheetsuc.Cell(sucrenglon, 3).SetFormulaA1(string.Format(cadenaENE, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 4).SetFormulaA1(string.Format(cadenaFEB, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 5).SetFormulaA1(string.Format(cadenaMAR, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 6).SetFormulaA1(string.Format(cadenaABR, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 7).SetFormulaA1(string.Format(cadenaMAY, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 8).SetFormulaA1(string.Format(cadenaJUN, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 9).SetFormulaA1(string.Format(cadenaJUL, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 10).SetFormulaA1(string.Format(cadenaAGO, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 11).SetFormulaA1(string.Format(cadenaSEP, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 12).SetFormulaA1(string.Format(cadenaOCT, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 13).SetFormulaA1(string.Format(cadenaNOV, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 14).SetFormulaA1(string.Format(cadenaDIC, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 15).SetFormulaA1(string.Format(cadenaACUM, sucrenglon));
                            sucrenglon += 1;
                        }
                        cellsuc = sheetsuc.Range(sucrenglon, 1, sucrenglon, 15);
                        cellsuc.Style.Font.Bold = true;
                        cellsuc.Style.Fill.BackgroundColor = XLColor.FromHtml("#CCFFCC");

                        cellsuc = sheetsuc.Range(sucrenglon, 1, sucrenglon, 2);
                        cellsuc.Merge();
                        cellsuc.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        sheetsuc.Cell(sucrenglon, 1).Value = "ACUMULADO VARIABLE";

                        sheetsuc.Cell(sucrenglon, 3).SetFormulaA1($"=SUM(C4:C{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 4).SetFormulaA1($"=SUM(D4:D{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 5).SetFormulaA1($"=SUM(E4:E{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 6).SetFormulaA1($"=SUM(F4:F{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 7).SetFormulaA1($"=SUM(G4:G{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 8).SetFormulaA1($"=SUM(H4:H{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 9).SetFormulaA1($"=SUM(I4:I{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 10).SetFormulaA1($"=SUM(J4:J{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 11).SetFormulaA1($"=SUM(K4:K{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 12).SetFormulaA1($"=SUM(L4:L{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 13).SetFormulaA1($"=SUM(M4:M{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 14).SetFormulaA1($"=SUM(N4:N{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 15).SetFormulaA1($"=SUM(O4:O{sucrenglon - 1})");

                        sucrenglon += 2;
                        var renglon_inicial_suc = sucrenglon;

                        foreach (var variables in grupo_cuentas_fijas)
                        {
                            sheetsuc.Cell(sucrenglon, 1).Value = variables.cuenta;
                            sheetsuc.Cell(sucrenglon, 2).Value = variables.concepto;
                            sheetsuc.Cell(sucrenglon, 3).SetFormulaA1(string.Format(cadenaENE, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 4).SetFormulaA1(string.Format(cadenaFEB, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 5).SetFormulaA1(string.Format(cadenaMAR, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 6).SetFormulaA1(string.Format(cadenaABR, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 7).SetFormulaA1(string.Format(cadenaMAY, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 8).SetFormulaA1(string.Format(cadenaJUN, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 9).SetFormulaA1(string.Format(cadenaJUL, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 10).SetFormulaA1(string.Format(cadenaAGO, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 11).SetFormulaA1(string.Format(cadenaSEP, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 12).SetFormulaA1(string.Format(cadenaOCT, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 13).SetFormulaA1(string.Format(cadenaNOV, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 14).SetFormulaA1(string.Format(cadenaDIC, sucrenglon));
                            sheetsuc.Cell(sucrenglon, 15).SetFormulaA1(string.Format(cadenaACUM, sucrenglon));
                            sucrenglon += 1;
                        }
                        cellsuc = sheetsuc.Range(sucrenglon, 1, sucrenglon, 15);
                        cellsuc.Style.Font.Bold = true;
                        cellsuc.Style.Fill.BackgroundColor = XLColor.FromHtml("#CCFFCC");

                        cellsuc = sheetsuc.Range(sucrenglon, 1, sucrenglon, 2);
                        cellsuc.Merge();
                        cellsuc.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        sheetsuc.Cell(sucrenglon, 1).Value = "ACUMULADO VARIABLE";

                        sheetsuc.Cell(sucrenglon, 3).SetFormulaA1($"=SUM(C{renglon_inicial_suc}:C{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 4).SetFormulaA1($"=SUM(D{renglon_inicial_suc}:D{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 5).SetFormulaA1($"=SUM(E{renglon_inicial_suc}:E{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 6).SetFormulaA1($"=SUM(F{renglon_inicial_suc}:F{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 7).SetFormulaA1($"=SUM(G{renglon_inicial_suc}:G{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 8).SetFormulaA1($"=SUM(H{renglon_inicial_suc}:H{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 9).SetFormulaA1($"=SUM(I{renglon_inicial_suc}:I{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 10).SetFormulaA1($"=SUM(J{renglon_inicial_suc}:J{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 11).SetFormulaA1($"=SUM(K{renglon_inicial_suc}:K{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 12).SetFormulaA1($"=SUM(L{renglon_inicial_suc}:L{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 13).SetFormulaA1($"=SUM(M{renglon_inicial_suc}:M{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 14).SetFormulaA1($"=SUM(N{renglon_inicial_suc}:N{sucrenglon - 1})");
                        sheetsuc.Cell(sucrenglon, 15).SetFormulaA1($"=SUM(O{renglon_inicial_suc}:O{sucrenglon - 1})");
                        sucrenglon += 2;
                        cellsuc = sheetsuc.Range(sucrenglon, 1, sucrenglon, 15);
                        cellsuc.Style.Font.Bold = true;
                        cellsuc.Style.Fill.BackgroundColor = XLColor.FromHtml("#33BBFF");

                        cellsuc = sheetsuc.Range(sucrenglon, 1, sucrenglon, 2);
                        cellsuc.Merge();
                        cellsuc.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        sheetsuc.Cell(sucrenglon, 1).Value = "ACUMULADO";

                        sheetsuc.Cell(sucrenglon, 3).SetFormulaA1($"=C{renglon_inicial_suc - 2} + C{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 4).SetFormulaA1($"=D{renglon_inicial_suc - 2} + D{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 5).SetFormulaA1($"=E{renglon_inicial_suc - 2} + E{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 6).SetFormulaA1($"=F{renglon_inicial_suc - 2} + F{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 7).SetFormulaA1($"=G{renglon_inicial_suc - 2} + G{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 8).SetFormulaA1($"=H{renglon_inicial_suc - 2} + H{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 9).SetFormulaA1($"=I{renglon_inicial_suc - 2} + I{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 10).SetFormulaA1($"=J{renglon_inicial_suc - 2} + J{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 11).SetFormulaA1($"=K{renglon_inicial_suc - 2} + K{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 12).SetFormulaA1($"=L{renglon_inicial_suc - 2} + L{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 13).SetFormulaA1($"=M{renglon_inicial_suc - 2} + M{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 14).SetFormulaA1($"=N{renglon_inicial_suc - 2} + N{sucrenglon - 2}");
                        sheetsuc.Cell(sucrenglon, 15).SetFormulaA1($"=O{renglon_inicial_suc - 2} + O{sucrenglon - 2}");
                        sheetsuc.Column(3).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(4).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(5).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(6).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(7).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(8).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(9).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(10).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(11).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(12).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(13).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(14).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Column(15).Style.NumberFormat.Format = "#,##0.00";
                        sheetsuc.Columns().AdjustToContents();


                    }
                    var grupo_adr = grupo_sucursales
                        .GroupBy(x => new { x.idadr, x.adr })
                        .Select(x => new { id = x.Key.idadr, adr = x.Key.adr })
                        .ToList();
                    foreach (var idadr in grupo_adr)
                    {
                        var sheet = worbook.Worksheets.Add($"ADR {idadr.adr} ");
                        sheet.TabColor = XLColor.FromHtml("#FFFF00");
                        sheet.Row(1).Height = 30;

                        //Encabezado
                        var cell = sheet.Range(1, 1, 1, 15);
                        cell.Value = "PROYECCIÓN DE GASTOS";
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.FontSize = 16;
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFDE00");
                        cell.Merge();

                        //subtitulos
                        cell = sheet.Range(2, 1, 2, 15);
                        cell.Value = $"ADR: {idadr.id} - {idadr.adr}     Departamento: Todos los Departamentos     Ejercicio: {ejercicio}";
                        cell.Style.Font.FontSize = 12;
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFDE00");
                        cell.Merge();

                        //Titulos Detalle
                        cell = sheet.Range(3, 1, 3, 15);
                        cell.Style.Font.FontSize = 12;
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#367C2B");
                        cell.Style.Font.FontColor = XLColor.White;

                        sheet.Cell(3, 1).Value = "CUENTA";
                        sheet.Cell(3, 2).Value = "CONCEPTO";
                        sheet.Cell(3, 3).Value = "ENERO";
                        sheet.Cell(3, 4).Value = "FEBRERO";
                        sheet.Cell(3, 5).Value = "MARZO";
                        sheet.Cell(3, 6).Value = "ABRIL";
                        sheet.Cell(3, 7).Value = "MAYO";
                        sheet.Cell(3, 8).Value = "JUNIO";
                        sheet.Cell(3, 9).Value = "JULIO";
                        sheet.Cell(3, 10).Value = "AGOSTO";
                        sheet.Cell(3, 11).Value = "SEPTIEMBRE";
                        sheet.Cell(3, 12).Value = "OCTUBRE";
                        sheet.Cell(3, 13).Value = "NOVIEMBRE";
                        sheet.Cell(3, 14).Value = "DICIEMBRE";
                        sheet.Cell(3, 15).Value = "ACUMULADO";

                        //Renglon donde se iniciara con el detalle
                        int renglon = 4;
                        string adrcadenaENE = "=";
                        string adrcadenaFEB = "=";
                        string adrcadenaMAR = "=";
                        string adrcadenaABR = "=";
                        string adrcadenaMAY = "=";
                        string adrcadenaJUN = "=";
                        string adrcadenaJUL = "=";
                        string adrcadenaAGO = "=";
                        string adrcadenaSEP = "=";
                        string adrcadenaOCT = "=";
                        string adrcadenaNOV = "=";
                        string adrcadenaDIC = "=";
                        string adrcadenaACUM = "=";
                        foreach (var str in grupo_sucursales.Where(x => x.idadr == idadr.id).ToList())
                        {
                            adrcadenaENE += "+'" + str.sucursal + "'!C{0}";
                            adrcadenaFEB += "+'" + str.sucursal + "'!D{0}";
                            adrcadenaMAR += "+'" + str.sucursal + "'!E{0}";
                            adrcadenaABR += "+'" + str.sucursal + "'!F{0}";
                            adrcadenaMAY += "+'" + str.sucursal + "'!G{0}";
                            adrcadenaJUN += "+'" + str.sucursal + "'!H{0}";
                            adrcadenaJUL += "+'" + str.sucursal + "'!I{0}";
                            adrcadenaAGO += "+'" + str.sucursal + "'!J{0}";
                            adrcadenaSEP += "+'" + str.sucursal + "'!K{0}";
                            adrcadenaOCT += "+'" + str.sucursal + "'!L{0}";
                            adrcadenaNOV += "+'" + str.sucursal + "'!M{0}";
                            adrcadenaDIC += "+'" + str.sucursal + "'!N{0}";
                            adrcadenaACUM += "+'" + str.sucursal + "'!O{0}";
                        }
                        foreach (var variables in grupo_cuentas_variables)
                        {
                            sheet.Cell(renglon, 1).Value = variables.cuenta;
                            sheet.Cell(renglon, 2).Value = variables.concepto;
                            sheet.Cell(renglon, 3).SetFormulaA1(string.Format(adrcadenaENE, renglon));
                            sheet.Cell(renglon, 4).SetFormulaA1(string.Format(adrcadenaFEB, renglon));
                            sheet.Cell(renglon, 5).SetFormulaA1(string.Format(adrcadenaMAR, renglon));
                            sheet.Cell(renglon, 6).SetFormulaA1(string.Format(adrcadenaABR, renglon));
                            sheet.Cell(renglon, 7).SetFormulaA1(string.Format(adrcadenaMAY, renglon));
                            sheet.Cell(renglon, 8).SetFormulaA1(string.Format(adrcadenaJUN, renglon));
                            sheet.Cell(renglon, 9).SetFormulaA1(string.Format(adrcadenaJUL, renglon));
                            sheet.Cell(renglon, 10).SetFormulaA1(string.Format(adrcadenaAGO, renglon));
                            sheet.Cell(renglon, 11).SetFormulaA1(string.Format(adrcadenaSEP, renglon));
                            sheet.Cell(renglon, 12).SetFormulaA1(string.Format(adrcadenaOCT, renglon));
                            sheet.Cell(renglon, 13).SetFormulaA1(string.Format(adrcadenaNOV, renglon));
                            sheet.Cell(renglon, 14).SetFormulaA1(string.Format(adrcadenaDIC, renglon));
                            sheet.Cell(renglon, 15).SetFormulaA1(string.Format(adrcadenaACUM, renglon));
                            renglon += 1;
                        }
                        cell = sheet.Range(renglon, 1, renglon, 15);
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#CCFFCC");

                        cell = sheet.Range(renglon, 1, renglon, 2);
                        cell.Merge();
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        sheet.Cell(renglon, 1).Value = "ACUMULADO VARIABLE";

                        sheet.Cell(renglon, 3).SetFormulaA1($"=SUM(C4:C{renglon - 1})");
                        sheet.Cell(renglon, 4).SetFormulaA1($"=SUM(D4:D{renglon - 1})");
                        sheet.Cell(renglon, 5).SetFormulaA1($"=SUM(E4:E{renglon - 1})");
                        sheet.Cell(renglon, 6).SetFormulaA1($"=SUM(F4:F{renglon - 1})");
                        sheet.Cell(renglon, 7).SetFormulaA1($"=SUM(G4:G{renglon - 1})");
                        sheet.Cell(renglon, 8).SetFormulaA1($"=SUM(H4:H{renglon - 1})");
                        sheet.Cell(renglon, 9).SetFormulaA1($"=SUM(I4:I{renglon - 1})");
                        sheet.Cell(renglon, 10).SetFormulaA1($"=SUM(J4:J{renglon - 1})");
                        sheet.Cell(renglon, 11).SetFormulaA1($"=SUM(K4:K{renglon - 1})");
                        sheet.Cell(renglon, 12).SetFormulaA1($"=SUM(L4:L{renglon - 1})");
                        sheet.Cell(renglon, 13).SetFormulaA1($"=SUM(M4:M{renglon - 1})");
                        sheet.Cell(renglon, 14).SetFormulaA1($"=SUM(N4:N{renglon - 1})");
                        sheet.Cell(renglon, 15).SetFormulaA1($"=SUM(O4:O{renglon - 1})");

                        renglon += 2;
                        var renglon_inicial = renglon;

                        foreach (var variables in grupo_cuentas_fijas)
                        {
                            sheet.Cell(renglon, 1).Value = variables.cuenta;
                            sheet.Cell(renglon, 2).Value = variables.concepto;
                            sheet.Cell(renglon, 3).SetFormulaA1(string.Format(adrcadenaENE, renglon));
                            sheet.Cell(renglon, 4).SetFormulaA1(string.Format(adrcadenaFEB, renglon));
                            sheet.Cell(renglon, 5).SetFormulaA1(string.Format(adrcadenaMAR, renglon));
                            sheet.Cell(renglon, 6).SetFormulaA1(string.Format(adrcadenaABR, renglon));
                            sheet.Cell(renglon, 7).SetFormulaA1(string.Format(adrcadenaMAY, renglon));
                            sheet.Cell(renglon, 8).SetFormulaA1(string.Format(adrcadenaJUN, renglon));
                            sheet.Cell(renglon, 9).SetFormulaA1(string.Format(adrcadenaJUL, renglon));
                            sheet.Cell(renglon, 10).SetFormulaA1(string.Format(adrcadenaAGO, renglon));
                            sheet.Cell(renglon, 11).SetFormulaA1(string.Format(adrcadenaSEP, renglon));
                            sheet.Cell(renglon, 12).SetFormulaA1(string.Format(adrcadenaOCT, renglon));
                            sheet.Cell(renglon, 13).SetFormulaA1(string.Format(adrcadenaNOV, renglon));
                            sheet.Cell(renglon, 14).SetFormulaA1(string.Format(adrcadenaDIC, renglon));
                            sheet.Cell(renglon, 15).SetFormulaA1(string.Format(adrcadenaACUM, renglon));
                            renglon += 1;
                        }
                        cell = sheet.Range(renglon, 1, renglon, 15);
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#CCFFCC");

                        cell = sheet.Range(renglon, 1, renglon, 2);
                        cell.Merge();
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        sheet.Cell(renglon, 1).Value = "ACUMULADO VARIABLE";

                        sheet.Cell(renglon, 3).SetFormulaA1($"=SUM(C{renglon_inicial}:C{renglon - 1})");
                        sheet.Cell(renglon, 4).SetFormulaA1($"=SUM(D{renglon_inicial}:D{renglon - 1})");
                        sheet.Cell(renglon, 5).SetFormulaA1($"=SUM(E{renglon_inicial}:E{renglon - 1})");
                        sheet.Cell(renglon, 6).SetFormulaA1($"=SUM(F{renglon_inicial}:F{renglon - 1})");
                        sheet.Cell(renglon, 7).SetFormulaA1($"=SUM(G{renglon_inicial}:G{renglon - 1})");
                        sheet.Cell(renglon, 8).SetFormulaA1($"=SUM(H{renglon_inicial}:H{renglon - 1})");
                        sheet.Cell(renglon, 9).SetFormulaA1($"=SUM(I{renglon_inicial}:I{renglon - 1})");
                        sheet.Cell(renglon, 10).SetFormulaA1($"=SUM(J{renglon_inicial}:J{renglon - 1})");
                        sheet.Cell(renglon, 11).SetFormulaA1($"=SUM(K{renglon_inicial}:K{renglon - 1})");
                        sheet.Cell(renglon, 12).SetFormulaA1($"=SUM(L{renglon_inicial}:L{renglon - 1})");
                        sheet.Cell(renglon, 13).SetFormulaA1($"=SUM(M{renglon_inicial}:M{renglon - 1})");
                        sheet.Cell(renglon, 14).SetFormulaA1($"=SUM(N{renglon_inicial}:N{renglon - 1})");
                        sheet.Cell(renglon, 15).SetFormulaA1($"=SUM(O{renglon_inicial}:O{renglon - 1})");
                        renglon += 2;
                        cell = sheet.Range(renglon, 1, renglon, 15);
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#33BBFF");

                        cell = sheet.Range(renglon, 1, renglon, 2);
                        cell.Merge();
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        sheet.Cell(renglon, 1).Value = "ACUMULADO";

                        sheet.Cell(renglon, 3).SetFormulaA1($"=C{renglon_inicial - 2} + C{renglon - 2}");
                        sheet.Cell(renglon, 4).SetFormulaA1($"=D{renglon_inicial - 2} + D{renglon - 2}");
                        sheet.Cell(renglon, 5).SetFormulaA1($"=E{renglon_inicial - 2} + E{renglon - 2}");
                        sheet.Cell(renglon, 6).SetFormulaA1($"=F{renglon_inicial - 2} + F{renglon - 2}");
                        sheet.Cell(renglon, 7).SetFormulaA1($"=G{renglon_inicial - 2} + G{renglon - 2}");
                        sheet.Cell(renglon, 8).SetFormulaA1($"=H{renglon_inicial - 2} + H{renglon - 2}");
                        sheet.Cell(renglon, 9).SetFormulaA1($"=I{renglon_inicial - 2} + I{renglon - 2}");
                        sheet.Cell(renglon, 10).SetFormulaA1($"=J{renglon_inicial - 2} + J{renglon - 2}");
                        sheet.Cell(renglon, 11).SetFormulaA1($"=K{renglon_inicial - 2} + K{renglon - 2}");
                        sheet.Cell(renglon, 12).SetFormulaA1($"=L{renglon_inicial - 2} + L{renglon - 2}");
                        sheet.Cell(renglon, 13).SetFormulaA1($"=M{renglon_inicial - 2} + M{renglon - 2}");
                        sheet.Cell(renglon, 14).SetFormulaA1($"=N{renglon_inicial - 2} + N{renglon - 2}");
                        sheet.Cell(renglon, 15).SetFormulaA1($"=O{renglon_inicial - 2} + O{renglon - 2}");
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
                        sheet.Column(14).Style.NumberFormat.Format = "#,##0.00";
                        sheet.Column(15).Style.NumberFormat.Format = "#,##0.00";
                        sheet.Columns().AdjustToContents();

                    }

                    ///GRUPO MAQUIMARIA  DEL HUMAYA 

                    var sheetgroup = worbook.Worksheets.Add($"GRUPO");
                    sheetgroup.TabColor = XLColor.FromHtml("#FFFF00");
                    sheetgroup.Row(1).Height = 30;

                    //Encabezado
                    var cellgroup = sheetgroup.Range(1, 1, 1, 15);
                    cellgroup.Value = "PROYECCIÓN DE GASTOS";
                    cellgroup.Style.Font.Bold = true;
                    cellgroup.Style.Font.FontSize = 16;
                    cellgroup.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cellgroup.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    cellgroup.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFDE00");
                    cellgroup.Merge();

                    //subtitulos
                    cellgroup = sheetgroup.Range(2, 1, 2, 15);
                    cellgroup.Value = $"GRUPO     Departamento: TODOS     Ejercicio: {ejercicio}";
                    cellgroup.Style.Font.FontSize = 12;
                    cellgroup.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cellgroup.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    cellgroup.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFDE00");
                    cellgroup.Merge();

                    //Titulos Detalle
                    cellgroup = sheetgroup.Range(3, 1, 3, 15);
                    cellgroup.Style.Font.FontSize = 12;
                    cellgroup.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cellgroup.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    cellgroup.Style.Fill.BackgroundColor = XLColor.FromHtml("#367C2B");
                    cellgroup.Style.Font.FontColor = XLColor.White;

                    sheetgroup.Cell(3, 1).Value = "CUENTA";
                    sheetgroup.Cell(3, 2).Value = "CONCEPTO";
                    sheetgroup.Cell(3, 3).Value = "ENERO";
                    sheetgroup.Cell(3, 4).Value = "FEBRERO";
                    sheetgroup.Cell(3, 5).Value = "MARZO";
                    sheetgroup.Cell(3, 6).Value = "ABRIL";
                    sheetgroup.Cell(3, 7).Value = "MAYO";
                    sheetgroup.Cell(3, 8).Value = "JUNIO";
                    sheetgroup.Cell(3, 9).Value = "JULIO";
                    sheetgroup.Cell(3, 10).Value = "AGOSTO";
                    sheetgroup.Cell(3, 11).Value = "SEPTIEMBRE";
                    sheetgroup.Cell(3, 12).Value = "OCTUBRE";
                    sheetgroup.Cell(3, 13).Value = "NOVIEMBRE";
                    sheetgroup.Cell(3, 14).Value = "DICIEMBRE";
                    sheetgroup.Cell(3, 15).Value = "ACUMULADO";

                    //Renglon donde se iniciara con el detalle
                    int renglon_grupo = 4;
                    string gpocadenaENE = "=";
                    string gpocadenaFEB = "=";
                    string gpocadenaMAR = "=";
                    string gpocadenaABR = "=";
                    string gpocadenaMAY = "=";
                    string gpocadenaJUN = "=";
                    string gpocadenaJUL = "=";
                    string gpocadenaAGO = "=";
                    string gpocadenaSEP = "=";
                    string gpocadenaOCT = "=";
                    string gpocadenaNOV = "=";
                    string gpocadenaDIC = "=";
                    string gpocadenaACUM = "=";
                    //+'ADR Sinaloa'!C5 + 'ADR Nayarit '!/*/*C5*/*/
                    gpocadenaENE += "+'ADR Sinaloa '!C{0}+'ADR Nayarit '!C{0}";
                    gpocadenaFEB += "+'ADR Sinaloa '!D{0}+'ADR Nayarit '!D{0}";
                    gpocadenaMAR += "+'ADR Sinaloa '!E{0}+'ADR Nayarit '!E{0}";
                    gpocadenaABR += "+'ADR Sinaloa '!F{0}+'ADR Nayarit '!F{0}";
                    gpocadenaMAY += "+'ADR Sinaloa '!G{0}+'ADR Nayarit '!G{0}";
                    gpocadenaJUN += "+'ADR Sinaloa '!H{0}+'ADR Nayarit '!H{0}";
                    gpocadenaJUL += "+'ADR Sinaloa '!I{0}+'ADR Nayarit '!I{0}";
                    gpocadenaAGO += "+'ADR Sinaloa '!J{0}+'ADR Nayarit '!J{0}";
                    gpocadenaSEP += "+'ADR Sinaloa '!K{0}+'ADR Nayarit '!K{0}";
                    gpocadenaOCT += "+'ADR Sinaloa '!L{0}+'ADR Nayarit '!L{0}";
                    gpocadenaNOV += "+'ADR Sinaloa '!M{0}+'ADR Nayarit '!M{0}";
                    gpocadenaDIC += "+'ADR Sinaloa '!N{0}+'ADR Nayarit '!N{0}";
                    gpocadenaACUM += "+'ADR Sinaloa '!O{0}+'ADR Nayarit '!O{0}";

                    foreach (var variables in grupo_cuentas_variables)
                    {
                        sheetgroup.Cell(renglon_grupo, 1).Value = variables.cuenta;
                        sheetgroup.Cell(renglon_grupo, 2).Value = variables.concepto;
                        sheetgroup.Cell(renglon_grupo, 3).SetFormulaA1(string.Format(gpocadenaENE, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 4).SetFormulaA1(string.Format(gpocadenaFEB, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 5).SetFormulaA1(string.Format(gpocadenaMAR, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 6).SetFormulaA1(string.Format(gpocadenaABR, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 7).SetFormulaA1(string.Format(gpocadenaMAY, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 8).SetFormulaA1(string.Format(gpocadenaJUN, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 9).SetFormulaA1(string.Format(gpocadenaJUL, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 10).SetFormulaA1(string.Format(gpocadenaAGO, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 11).SetFormulaA1(string.Format(gpocadenaSEP, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 12).SetFormulaA1(string.Format(gpocadenaOCT, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 13).SetFormulaA1(string.Format(gpocadenaNOV, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 14).SetFormulaA1(string.Format(gpocadenaDIC, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 15).SetFormulaA1(string.Format(gpocadenaACUM, renglon_grupo));
                        renglon_grupo += 1;
                    }
                    cellgroup = sheetgroup.Range(renglon_grupo, 1, renglon_grupo, 15);
                    cellgroup.Style.Font.Bold = true;
                    cellgroup.Style.Fill.BackgroundColor = XLColor.FromHtml("#CCFFCC");

                    cellgroup = sheetgroup.Range(renglon_grupo, 1, renglon_grupo, 2);
                    cellgroup.Merge();
                    cellgroup.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    sheetgroup.Cell(renglon_grupo, 1).Value = "ACUMULADO VARIABLE";

                    sheetgroup.Cell(renglon_grupo, 3).SetFormulaA1($"=SUM(C4:C{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 4).SetFormulaA1($"=SUM(D4:D{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 5).SetFormulaA1($"=SUM(E4:E{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 6).SetFormulaA1($"=SUM(F4:F{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 7).SetFormulaA1($"=SUM(G4:G{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 8).SetFormulaA1($"=SUM(H4:H{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 9).SetFormulaA1($"=SUM(I4:I{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 10).SetFormulaA1($"=SUM(J4:J{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 11).SetFormulaA1($"=SUM(K4:K{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 12).SetFormulaA1($"=SUM(L4:L{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 13).SetFormulaA1($"=SUM(M4:M{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 14).SetFormulaA1($"=SUM(N4:N{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 15).SetFormulaA1($"=SUM(O4:O{renglon_grupo - 1})");

                    renglon_grupo += 2;
                    var renglon_grupo_inicial = renglon_grupo;

                    foreach (var variables in grupo_cuentas_fijas)
                    {
                        sheetgroup.Cell(renglon_grupo, 1).Value = variables.cuenta;
                        sheetgroup.Cell(renglon_grupo, 2).Value = variables.concepto;
                        sheetgroup.Cell(renglon_grupo, 3).SetFormulaA1(string.Format(gpocadenaENE, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 4).SetFormulaA1(string.Format(gpocadenaFEB, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 5).SetFormulaA1(string.Format(gpocadenaMAR, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 6).SetFormulaA1(string.Format(gpocadenaABR, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 7).SetFormulaA1(string.Format(gpocadenaMAY, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 8).SetFormulaA1(string.Format(gpocadenaJUN, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 9).SetFormulaA1(string.Format(gpocadenaJUL, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 10).SetFormulaA1(string.Format(gpocadenaAGO, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 11).SetFormulaA1(string.Format(gpocadenaSEP, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 12).SetFormulaA1(string.Format(gpocadenaOCT, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 13).SetFormulaA1(string.Format(gpocadenaNOV, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 14).SetFormulaA1(string.Format(gpocadenaDIC, renglon_grupo));
                        sheetgroup.Cell(renglon_grupo, 15).SetFormulaA1(string.Format(gpocadenaACUM, renglon_grupo));
                        renglon_grupo += 1;
                    }
                    cellgroup = sheetgroup.Range(renglon_grupo, 1, renglon_grupo, 15);
                    cellgroup.Style.Font.Bold = true;
                    cellgroup.Style.Fill.BackgroundColor = XLColor.FromHtml("#CCFFCC");

                    cellgroup = sheetgroup.Range(renglon_grupo, 1, renglon_grupo, 2);
                    cellgroup.Merge();
                    cellgroup.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    sheetgroup.Cell(renglon_grupo, 1).Value = "ACUMULADO VARIABLE";

                    sheetgroup.Cell(renglon_grupo, 3).SetFormulaA1($"=SUM(C{renglon_grupo_inicial}:C{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 4).SetFormulaA1($"=SUM(D{renglon_grupo_inicial}:D{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 5).SetFormulaA1($"=SUM(E{renglon_grupo_inicial}:E{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 6).SetFormulaA1($"=SUM(F{renglon_grupo_inicial}:F{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 7).SetFormulaA1($"=SUM(G{renglon_grupo_inicial}:G{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 8).SetFormulaA1($"=SUM(H{renglon_grupo_inicial}:H{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 9).SetFormulaA1($"=SUM(I{renglon_grupo_inicial}:I{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 10).SetFormulaA1($"=SUM(J{renglon_grupo_inicial}:J{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 11).SetFormulaA1($"=SUM(K{renglon_grupo_inicial}:K{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 12).SetFormulaA1($"=SUM(L{renglon_grupo_inicial}:L{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 13).SetFormulaA1($"=SUM(M{renglon_grupo_inicial}:M{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 14).SetFormulaA1($"=SUM(N{renglon_grupo_inicial}:N{renglon_grupo - 1})");
                    sheetgroup.Cell(renglon_grupo, 15).SetFormulaA1($"=SUM(O{renglon_grupo_inicial}:O{renglon_grupo - 1})");
                    renglon_grupo += 2;
                    cellgroup = sheetgroup.Range(renglon_grupo, 1, renglon_grupo, 15);
                    cellgroup.Style.Font.Bold = true;
                    cellgroup.Style.Fill.BackgroundColor = XLColor.FromHtml("#33BBFF");

                    cellgroup = sheetgroup.Range(renglon_grupo, 1, renglon_grupo, 2);
                    cellgroup.Merge();
                    cellgroup.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    sheetgroup.Cell(renglon_grupo, 1).Value = "ACUMULADO";

                    sheetgroup.Cell(renglon_grupo, 3).SetFormulaA1($"=C{renglon_grupo_inicial - 2} + C{renglon_grupo - 2}");
                    sheetgroup.Cell(renglon_grupo, 4).SetFormulaA1($"=D{renglon_grupo_inicial - 2} + D{renglon_grupo - 2}");
                    sheetgroup.Cell(renglon_grupo, 5).SetFormulaA1($"=E{renglon_grupo_inicial - 2} + E{renglon_grupo - 2}");
                    sheetgroup.Cell(renglon_grupo, 6).SetFormulaA1($"=F{renglon_grupo_inicial - 2} + F{renglon_grupo - 2}");
                    sheetgroup.Cell(renglon_grupo, 7).SetFormulaA1($"=G{renglon_grupo_inicial - 2} + G{renglon_grupo - 2}");
                    sheetgroup.Cell(renglon_grupo, 8).SetFormulaA1($"=H{renglon_grupo_inicial - 2} + H{renglon_grupo - 2}");
                    sheetgroup.Cell(renglon_grupo, 9).SetFormulaA1($"=I{renglon_grupo_inicial - 2} + I{renglon_grupo - 2}");
                    sheetgroup.Cell(renglon_grupo, 10).SetFormulaA1($"=J{renglon_grupo_inicial - 2} + J{renglon_grupo - 2}");
                    sheetgroup.Cell(renglon_grupo, 11).SetFormulaA1($"=K{renglon_grupo_inicial - 2} + K{renglon_grupo - 2}");
                    sheetgroup.Cell(renglon_grupo, 12).SetFormulaA1($"=L{renglon_grupo_inicial - 2} + L{renglon_grupo - 2}");
                    sheetgroup.Cell(renglon_grupo, 13).SetFormulaA1($"=M{renglon_grupo_inicial - 2} + M{renglon_grupo - 2}");
                    sheetgroup.Cell(renglon_grupo, 14).SetFormulaA1($"=N{renglon_grupo_inicial - 2} + N{renglon_grupo - 2}");
                    sheetgroup.Cell(renglon_grupo, 15).SetFormulaA1($"=O{renglon_grupo_inicial - 2} + O{renglon_grupo - 2}");
                    sheetgroup.Column(3).Style.NumberFormat.Format = "#,##0.00";
                    sheetgroup.Column(4).Style.NumberFormat.Format = "#,##0.00";
                    sheetgroup.Column(5).Style.NumberFormat.Format = "#,##0.00";
                    sheetgroup.Column(6).Style.NumberFormat.Format = "#,##0.00";
                    sheetgroup.Column(7).Style.NumberFormat.Format = "#,##0.00";
                    sheetgroup.Column(8).Style.NumberFormat.Format = "#,##0.00";
                    sheetgroup.Column(9).Style.NumberFormat.Format = "#,##0.00";
                    sheetgroup.Column(10).Style.NumberFormat.Format = "#,##0.00";
                    sheetgroup.Column(11).Style.NumberFormat.Format = "#,##0.00";
                    sheetgroup.Column(12).Style.NumberFormat.Format = "#,##0.00";
                    sheetgroup.Column(13).Style.NumberFormat.Format = "#,##0.00";
                    sheetgroup.Column(14).Style.NumberFormat.Format = "#,##0.00";
                    sheetgroup.Column(15).Style.NumberFormat.Format = "#,##0.00";
                    sheetgroup.Columns().AdjustToContents();







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
                        filename = $"Proyeccion de Gastos {ejercicio}"
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
    internal class directorio
    {
        public int idsucursal { get; set; }
        public string nombre { get; set; }
        public string nomenclatura { get; set; }
        public List<string> hojas { get; set; }
    }
    internal class DirectorioDepartamentos
    {
        public string iddepartamento { get; set; }
        public string departamento { get; set; }
        public string nomenclatura { get; set; }
        public List<string> hojas { get; set; }
    }
}
