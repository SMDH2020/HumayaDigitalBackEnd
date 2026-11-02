using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Nomina.Modelos.Dashboard;
using HD_Ventas;
using HD_Ventas.Reportes;
using System.Globalization;

namespace HD_Nomina.Reportes
{
    public class XLS_Listado_Empleados
    {
        public static Task<DocResult> GenerarExcel(IEnumerable<mdl_Listado_Empleados> detalle)
        {
            try
            {
                var lista = (detalle ?? Enumerable.Empty<mdl_Listado_Empleados>()).ToList();

                // Sucursal -> Departamento -> Empleados
                var sucursales = lista
                    .GroupBy(e => new { e.IDSucursal, e.Sucursal })
                    .Select(s => new
                    {
                        Sucursal = s.Key.Sucursal,
                        Total = s.Count(),
                        Departamentos = s
                            .GroupBy(e => new { e.IDDepartamento, e.Departamento })
                            .Select(d => new
                            {
                                Departamento = d.Key.Departamento,
                                Total = d.Count(),
                                Empleados = d.OrderBy(x => x.empleado).ToList()
                            })
                            .OrderBy(d => d.Departamento)
                            .ToList()
                    })
                    .OrderBy(s => s.Sucursal)
                    .ToList();

                string sheetname = "LISTADO DE EMPLEADOS";
                string titulo = $"{sheetname} - {DateTime.Now.Year}";
                const int columnas = 10;

                using (var workbook = new XLWorkbook())
                using (var ms = new MemoryStream())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    // Los renglones de sucursal/departamento quedan ARRIBA de su detalle
                    sheet.Outline.SummaryVLocation = XLOutlineSummaryVLocation.Top;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, titulo, columnas);

                    string[] encabezados =
                    {
                        "EMPLEADO", "SEXO", "PUESTO", "DEPARTAMENTO", "SUCURSAL",
                        "JEFE DIRECTO", "CORREO", "CARGO CONTABLE", "INGRESO", "ANTIGÜEDAD (AÑOS)"
                    };
                    for (int i = 0; i < encabezados.Length; i++)
                        sheet.Cell(renglon, i + 1).Value = encabezados[i];

                    var rango = sheet.Range(renglon, 1, renglon, columnas);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    rango.Style.Alignment.WrapText = true;

                    int renglonEncabezado = renglon;
                    sheet.SheetView.FreezeRows(renglonEncabezado);
                    renglon++;

                    foreach (var suc in sucursales)
                    {
                        // ---- Renglon de sucursal ----
                        sheet.Cell(renglon, 1).Value = $"{suc.Sucursal?.ToUpper()} · {suc.Total} EMPLEADOS";
                        rango = sheet.Range(renglon, 1, renglon, columnas);
                        rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#D9EAD3");
                        rango.Style.Font.Bold = true;
                        rango.Style.Font.FontSize = 11;
                        renglon++;

                        int primerRenglonSucursal = renglon;

                        foreach (var dep in suc.Departamentos)
                        {
                            // ---- Renglon de departamento ----
                            sheet.Cell(renglon, 1).Value = $"{dep.Departamento?.ToUpper()} · {dep.Total}";
                            sheet.Cell(renglon, 1).Style.Alignment.Indent = 1;
                            rango = sheet.Range(renglon, 1, renglon, columnas);
                            rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EEF1E9");
                            rango.Style.Font.Bold = true;
                            renglon++;

                            int primerEmpleado = renglon;

                            foreach (var e in dep.Empleados)
                            {
                                sheet.Cell(renglon, 1).Value = e.empleado;
                                sheet.Cell(renglon, 1).Style.Alignment.Indent = 2;
                                sheet.Cell(renglon, 2).Value = e.Sexo == 0 ? "Mujer" : "Hombre";
                                sheet.Cell(renglon, 3).Value = e.Puesto?.ToUpper();
                                sheet.Cell(renglon, 4).Value = e.Departamento?.ToUpper();
                                sheet.Cell(renglon, 5).Value = e.Sucursal?.ToUpper();
                                sheet.Cell(renglon, 6).Value = e.nombre_jefe;
                                sheet.Cell(renglon, 7).Value = e.Correo?.ToLower();
                                sheet.Cell(renglon, 8).Value = e.cargo_contable?.ToUpper();

                                // FechaIngreso llega como texto dd/MM/yyyy; se guarda como fecha real
                                if (DateTime.TryParseExact(e.FechaIngreso, "dd/MM/yyyy",
                                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var fecha))
                                    sheet.Cell(renglon, 9).Value = fecha;
                                else
                                    sheet.Cell(renglon, 9).Value = e.FechaIngreso;

                                sheet.Cell(renglon, 10).Value = e.antiguedad;
                                renglon++;
                            }

                            // Empleados: nivel 1 de agrupacion
                            if (renglon > primerEmpleado)
                                sheet.Rows(primerEmpleado, renglon - 1).Group();
                        }

                        // Departamentos + empleados: nivel 2 (los renglones de departamento
                        // quedan en nivel 1 y los empleados en nivel 2)
                        if (renglon > primerRenglonSucursal)
                            sheet.Rows(primerRenglonSucursal, renglon - 1).Group();
                    }

                    // ---- Total general ----
                    sheet.Cell(renglon, 1).Value = $"TOTAL EMPLEADOS · {lista.Count}";
                    rango = sheet.Range(renglon, 1, renglon, columnas);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#e5e6e6");
                    rango.Style.Font.Bold = true;
                    rango.Style.Border.TopBorder = XLBorderStyleValues.Thin;

                    // Formatos
                    sheet.Column(9).Style.NumberFormat.Format = "dd/mm/yyyy";
                    sheet.Column(10).Style.NumberFormat.Format = "0.0";
                    sheet.Range(renglonEncabezado + 1, 2, renglon, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(renglonEncabezado + 1, 9, renglon, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    sheet.Columns().AdjustToContents();
                    sheet.Column(1).Width = 45;   // nombres largos y renglones de grupo
                    sheet.Column(7).Width = 35;

                    workbook.SaveAs(ms);

                    DocResult doc = new DocResult
                    {
                        documento = Convert.ToBase64String(ms.ToArray()),
                        filename = sheetname
                    };
                    return Task.FromResult(doc);
                }
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { errores = ex.Message });
            }
        }
    }
}