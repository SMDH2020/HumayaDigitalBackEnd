using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using HD.AccesoDatos;
using HD_Cobranza;
using HD_Finanzas.Modelos.Margenes;
using HD_Ventas.Modelos;
using HD_Ventas.Reportes;
using System.Linq;

namespace HD_Reporteria.Finanzas.Excel
{
    public class XLS_Margenes_Brutos
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
        public static Task<DocResult> GenerarExcel(mdl_Margenes_Brutos_View margenes, int ejercicio, string periodo)
        {
            try
            {
                string sheetname = "SEMAFORO DE MARGENES ";
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, $"SEMAFORO POR SUCURSAL Y LINEA DE PRODUCTOS", 17);

                    //renglon += 1;

                    sheet.Range(renglon, 1, renglon, 17).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");

                    sheet.Cell(renglon, 1).Value = "";
                    sheet.Cell(renglon, 2).Value = "GUIA";
                    sheet.Cell(renglon, 3).Value = "GRUPO";
                    sheet.Cell(renglon, 4).Value = "SINALOA";
                    sheet.Cell(renglon, 5).Value = "NAYARIT";
                    sheet.Cell(renglon, 6).Value = "NAVOLATO";
                    sheet.Cell(renglon, 7).Value = "ELDORADO";
                    sheet.Cell(renglon, 8).Value = "LA CRUZ";
                    sheet.Cell(renglon, 9).Value = "EL ROSARIO";
                    sheet.Cell(renglon, 10).Value = "TEPIC";
                    sheet.Cell(renglon, 11).Value = "SANTIAGO";
                    sheet.Cell(renglon, 12).Value = "TECUALA";
                    sheet.Cell(renglon, 13).Value = "SAN JOSE";
                    sheet.Cell(renglon, 14).Value = "CAIMANERO";
                    sheet.Cell(renglon, 15).Value = "COSTA RICA";
                    sheet.Cell(renglon, 16).Value = "VILLA UNION";
                    sheet.Cell(renglon, 17).Value = "SAN VICENTE";

                    // Estilo para los encabezados de la tabla
                    var rango = sheet.Range(renglon, 1, renglon, 17);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;

                    var ordenEspecifico = new List<(int idadr, int idsucursal)>
                    {
                        (0, 0),
                        (1, 0),
                        (2, 0),
                        (1, 1),
                        (1, 21),
                        (1, 41),
                        (1, 51),
                        (2, 2),
                        (2, 22),
                        (2, 32),
                        (2, 12),
                        (1, 11),
                        (1, 31),
                        (1, 61),
                        (2, 52)
                    };

                    sheet.Cell(renglon, 1).Value = "Maquinaria";
                    var rango2 = sheet.Range(renglon, 1, renglon, 17);
                    rango2.Style.Fill.BackgroundColor = XLColor.FromHtml("#DAE6BE");
                    rango2.Style.Font.Bold = true;
                    rango2.Style.Font.FontSize = 12;
                    renglon++;

                    //var GrupoGuia = margenes.guias.GroupBy(x => x.departamento);
                    var guiasMaquinaria = margenes.guias.Where(x => x.departamento == "MAQUINARIA");
                    var margenesMaquinaria = margenes.margenes.Where(x => x.departamento == "MAQUINARIA");
                    string[] conceptosOrdenados = { "Utilidad bruta", "Gastos", "Utilidad de Operación", "Sueldos", "" };
                    string[] conceptosOrdenadosDB = { "Utilidad bruta", "Gastos", "Utilidad Operacion", "Sueldos", "Porcentaje sueldos" };

                    var adrsSucursales = margenesMaquinaria
                    .Where(x => ordenEspecifico.Any(o => o.idadr == x.idadr && o.idsucursal == x.idsucursal)) // Filtrar solo las sucursales que están en ordenEspecifico
                    .Select(x => new { x.idadr, x.adr, x.idsucursal, x.sucursal })
                    .Distinct()
                    .OrderBy(x => ordenEspecifico.FindIndex(o => o.idadr == x.idadr && o.idsucursal == x.idsucursal)) // Ordenar según el índice en ordenEspecifico
                    .ToList();

                    for (int i = 0; i < conceptosOrdenados.Length; i++)
                    {
                        string conceptoExcel = conceptosOrdenados[i];
                        string conceptoDB = conceptosOrdenadosDB[i];

                        var registrosConcepto = guiasMaquinaria.Where(x => x.concepto == conceptoDB).ToList();

                        sheet.Cell(renglon, 1).Value = conceptoExcel;

                        if (registrosConcepto.Any())
                        {
                            foreach (var guia in registrosConcepto)
                            {
                                if (conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación" || conceptoExcel == "Gastos" || conceptoExcel == "")
                                    sheet.Cell(renglon, 2).Value = guia.guia / 100;

                                else
                                    sheet.Cell(renglon, 2).Value = guia.guia;
                            }
                        }

                        int col = 3;
                        foreach (var adrSucursal in adrsSucursales)
                        {
                            var margen = margenesMaquinaria.FirstOrDefault(x =>
                                x.idadr == adrSucursal.idadr &&
                                x.adr == adrSucursal.adr &&
                                x.idsucursal == adrSucursal.idsucursal &&
                                x.sucursal == adrSucursal.sucursal);

                            if (margen != null)
                            {
                                double valor = conceptoExcel switch
                                {
                                    "Utilidad bruta" => margen.utilidad_bruta / 100,
                                    "Gastos" => margen.gasto_Departamento / 100,
                                    "Utilidad de Operación" => margen.utilidad_operacion / 100,
                                    "Sueldos" => margen.nomina,
                                    "" => margen.porc_nomina / 100,
                                    _ => 0
                                };



                                sheet.Cell(renglon, col).Value = valor;
                                var celda = sheet.Cell(renglon, col);
                                celda.Value = valor;

                                var valorGuia = sheet.Cell(renglon, 2).GetValue<string>();

                                if ((conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación") && valor < Convert.ToDouble(valorGuia))
                                {
                                    celda.Style.Font.FontColor = XLColor.Red;
                                }

                                if ((conceptoExcel == "Gastos" || conceptoExcel == "Sueldos" || conceptoExcel == "") && valor > Convert.ToDouble(valorGuia))
                                {
                                    celda.Style.Font.FontColor = XLColor.Red;
                                }

                                if (conceptoExcel == "Sueldos")
                                {
                                    sheet.Row(renglon).Style.NumberFormat.Format = "#,##0.00";
                                }
                                else if (conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación" || conceptoExcel == "Gastos" || conceptoExcel == "")
                                {
                                    sheet.Row(renglon).Style.NumberFormat.Format = "0.00 %";
                                }

                                var rango3 = sheet.Range(renglon, 1, renglon, 17);

                                if ((conceptoExcel == "Gastos" || conceptoExcel == ""))
                                    rango3.Style.Fill.BackgroundColor = XLColor.FromHtml("#f0f0f0");

                                if ((conceptoExcel == "Sueldos" || conceptoExcel == ""))
                                    rango3.Style.Fill.BackgroundColor = XLColor.FromHtml("#e0e0e0");
                            }

                            col++; // Mover a la siguiente columna
                        }

                        renglon++;
                    }

                    renglon++;

                    //REFACCIONES

                    sheet.Cell(renglon, 1).Value = "Refacciones";
                    var rangoRef = sheet.Range(renglon, 1, renglon, 17);
                    rangoRef.Style.Fill.BackgroundColor = XLColor.FromHtml("#DAE6BE");
                    rangoRef.Style.Font.Bold = true;
                    rangoRef.Style.Font.FontSize = 12;
                    renglon++;

                    var guiasRefacciones = margenes.guias.Where(x => x.departamento == "REFACCIONES");
                    var margenesRefacciones = margenes.margenes.Where(x => x.departamento == "REFACCIONES");

                    var adrsSucursalesRefacciones = margenesRefacciones
                    .Where(x => ordenEspecifico.Any(o => o.idadr == x.idadr && o.idsucursal == x.idsucursal)) // Filtrar solo las sucursales que están en ordenEspecifico
                    .Select(x => new { x.idadr, x.adr, x.idsucursal, x.sucursal })
                    .Distinct()
                    .OrderBy(x => ordenEspecifico.FindIndex(o => o.idadr == x.idadr && o.idsucursal == x.idsucursal)) // Ordenar según el índice en ordenEspecifico
                    .ToList();

                    for (int i = 0; i < conceptosOrdenados.Length; i++)
                    {
                        string conceptoExcel = conceptosOrdenados[i];
                        string conceptoDB = conceptosOrdenadosDB[i];

                        var registrosConcepto = guiasRefacciones.Where(x => x.concepto == conceptoDB).ToList();

                        sheet.Cell(renglon, 1).Value = conceptoExcel;

                        if (registrosConcepto.Any())
                        {
                            foreach (var guia in registrosConcepto)
                            {
                                if (conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación" || conceptoExcel == "Gastos" || conceptoExcel == "")
                                    sheet.Cell(renglon, 2).Value = guia.guia / 100;

                                else
                                    sheet.Cell(renglon, 2).Value = guia.guia;
                            }
                        }

                        int col = 3;
                        foreach (var adrSucursal in adrsSucursales)
                        {
                            var margen = margenesRefacciones.FirstOrDefault(x =>
                                x.idadr == adrSucursal.idadr &&
                                x.adr == adrSucursal.adr &&
                                x.idsucursal == adrSucursal.idsucursal &&
                                x.sucursal == adrSucursal.sucursal);

                            if (margen != null)
                            {
                                double valor = conceptoExcel switch
                                {
                                    "Utilidad bruta" => margen.utilidad_bruta / 100,
                                    "Gastos" => margen.gasto_Departamento / 100,
                                    "Utilidad de Operación" => margen.utilidad_operacion / 100,
                                    "Sueldos" => margen.nomina,
                                    "" => margen.porc_nomina / 100,
                                    _ => 0
                                };



                                sheet.Cell(renglon, col).Value = valor;
                                var celda = sheet.Cell(renglon, col);
                                celda.Value = valor;

                                var valorGuia = sheet.Cell(renglon, 2).GetValue<string>();

                                if ((conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación") && valor < Convert.ToDouble(valorGuia))
                                {
                                    celda.Style.Font.FontColor = XLColor.Red;
                                }

                                if ((conceptoExcel == "Gastos" || conceptoExcel == "Sueldos" || conceptoExcel == "") && valor > Convert.ToDouble(valorGuia))
                                {
                                    celda.Style.Font.FontColor = XLColor.Red;
                                }

                                if (conceptoExcel == "Sueldos")
                                {
                                    sheet.Row(renglon).Style.NumberFormat.Format = "#,##0.00";
                                }
                                else if (conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación" || conceptoExcel == "Gastos" || conceptoExcel == "")
                                {
                                    sheet.Row(renglon).Style.NumberFormat.Format = "0.00 %";
                                }

                                var rango3 = sheet.Range(renglon, 1, renglon, 17);

                                if ((conceptoExcel == "Gastos" || conceptoExcel == ""))
                                    rango3.Style.Fill.BackgroundColor = XLColor.FromHtml("#f0f0f0");

                                if ((conceptoExcel == "Sueldos" || conceptoExcel == ""))
                                    rango3.Style.Fill.BackgroundColor = XLColor.FromHtml("#e0e0e0");
                            }

                            col++; // Mover a la siguiente columna
                        }

                        renglon++;
                    }

                    renglon++;

                    //SERVICIO

                    sheet.Cell(renglon, 1).Value = "Servicio";
                    var rangoServicio = sheet.Range(renglon, 1, renglon, 17);
                    rangoServicio.Style.Fill.BackgroundColor = XLColor.FromHtml("#DAE6BE");
                    rangoServicio.Style.Font.Bold = true;
                    rangoServicio.Style.Font.FontSize = 12;
                    renglon++;

                    var guiasServicio = margenes.guias.Where(x => x.departamento == "SERVICIO");
                    var margenesServicio = margenes.margenes.Where(x => x.departamento == "SERVICIO");

                    var adrsSucursalesServicio = margenesServicio
                    .Where(x => ordenEspecifico.Any(o => o.idadr == x.idadr && o.idsucursal == x.idsucursal)) // Filtrar solo las sucursales que están en ordenEspecifico
                    .Select(x => new { x.idadr, x.adr, x.idsucursal, x.sucursal })
                    .Distinct()
                    .OrderBy(x => ordenEspecifico.FindIndex(o => o.idadr == x.idadr && o.idsucursal == x.idsucursal)) // Ordenar según el índice en ordenEspecifico
                    .ToList();

                    for (int i = 0; i < conceptosOrdenados.Length; i++)
                    {
                        string conceptoExcel = conceptosOrdenados[i];
                        string conceptoDB = conceptosOrdenadosDB[i];

                        var registrosConcepto = guiasServicio.Where(x => x.concepto == conceptoDB).ToList();

                        sheet.Cell(renglon, 1).Value = conceptoExcel;

                        if (registrosConcepto.Any())
                        {
                            foreach (var guia in registrosConcepto)
                            {
                                if (conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación" || conceptoExcel == "Gastos" || conceptoExcel == "")
                                    sheet.Cell(renglon, 2).Value = guia.guia / 100;

                                else
                                    sheet.Cell(renglon, 2).Value = guia.guia;
                            }
                        }

                        int col = 3;
                        foreach (var adrSucursal in adrsSucursales)
                        {
                            var margen = margenesServicio.FirstOrDefault(x =>
                                x.idadr == adrSucursal.idadr &&
                                x.adr == adrSucursal.adr &&
                                x.idsucursal == adrSucursal.idsucursal &&
                                x.sucursal == adrSucursal.sucursal);

                            if (margen != null)
                            {
                                double valor = conceptoExcel switch
                                {
                                    "Utilidad bruta" => margen.utilidad_bruta / 100,
                                    "Gastos" => margen.gasto_Departamento / 100,
                                    "Utilidad de Operación" => margen.utilidad_operacion / 100,
                                    "Sueldos" => margen.nomina,
                                    "" => margen.porc_nomina / 100,
                                    _ => 0
                                };



                                sheet.Cell(renglon, col).Value = valor;
                                var celda = sheet.Cell(renglon, col);
                                celda.Value = valor;

                                var valorGuia = sheet.Cell(renglon, 2).GetValue<string>();

                                if ((conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación") && valor < Convert.ToDouble(valorGuia))
                                {
                                    celda.Style.Font.FontColor = XLColor.Red;
                                }

                                if ((conceptoExcel == "Gastos" || conceptoExcel == "Sueldos" || conceptoExcel == "") && valor > Convert.ToDouble(valorGuia))
                                {
                                    celda.Style.Font.FontColor = XLColor.Red;
                                }

                                if (conceptoExcel == "Sueldos")
                                {
                                    sheet.Row(renglon).Style.NumberFormat.Format = "#,##0.00";
                                }
                                else if (conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación" || conceptoExcel == "Gastos" || conceptoExcel == "")
                                {
                                    sheet.Row(renglon).Style.NumberFormat.Format = "0.00 %";
                                }

                                var rango3 = sheet.Range(renglon, 1, renglon, 17);

                                if ((conceptoExcel == "Gastos" || conceptoExcel == ""))
                                    rango3.Style.Fill.BackgroundColor = XLColor.FromHtml("#f0f0f0");

                                if ((conceptoExcel == "Sueldos" || conceptoExcel == ""))
                                    rango3.Style.Fill.BackgroundColor = XLColor.FromHtml("#e0e0e0");
                            }

                            col++; // Mover a la siguiente columna
                        }

                        renglon++;
                    }

                    renglon++;

                    //PRODUCTO ALLIADO

                    sheet.Cell(renglon, 1).Value = "Productos Aliados";
                    var rangoPA = sheet.Range(renglon, 1, renglon, 17);
                    rangoPA.Style.Fill.BackgroundColor = XLColor.FromHtml("#DAE6BE");
                    rangoPA.Style.Font.Bold = true;
                    rangoPA.Style.Font.FontSize = 12;
                    renglon++;

                    var guiasPA = margenes.guias.Where(x => x.departamento == "PRODUCTOS ALIADOS");
                    var margenesPA = margenes.margenes.Where(x => x.departamento == "PRODUCTOS ALIADOS");

                    var adrsSucursalesPA = margenesPA
                    .Where(x => ordenEspecifico.Any(o => o.idadr == x.idadr && o.idsucursal == x.idsucursal)) // Filtrar solo las sucursales que están en ordenEspecifico
                    .Select(x => new { x.idadr, x.adr, x.idsucursal, x.sucursal })
                    .Distinct()
                    .OrderBy(x => ordenEspecifico.FindIndex(o => o.idadr == x.idadr && o.idsucursal == x.idsucursal)) // Ordenar según el índice en ordenEspecifico
                    .ToList();

                    for (int i = 0; i < conceptosOrdenados.Length; i++)
                    {
                        string conceptoExcel = conceptosOrdenados[i];
                        string conceptoDB = conceptosOrdenadosDB[i];

                        var registrosConcepto = guiasPA.Where(x => x.concepto == conceptoDB).ToList();

                        sheet.Cell(renglon, 1).Value = conceptoExcel;

                        if (registrosConcepto.Any())
                        {
                            foreach (var guia in registrosConcepto)
                            {
                                if (conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación" || conceptoExcel == "Gastos" || conceptoExcel == "")
                                    sheet.Cell(renglon, 2).Value = guia.guia / 100;

                                else
                                    sheet.Cell(renglon, 2).Value = guia.guia;
                            }
                        }

                        int col = 3;
                        foreach (var adrSucursal in adrsSucursales)
                        {
                            var margen = margenesPA.FirstOrDefault(x =>
                                x.idadr == adrSucursal.idadr &&
                                x.adr == adrSucursal.adr &&
                                x.idsucursal == adrSucursal.idsucursal &&
                                x.sucursal == adrSucursal.sucursal);

                            if (margen != null)
                            {
                                double valor = conceptoExcel switch
                                {
                                    "Utilidad bruta" => margen.utilidad_bruta / 100,
                                    "Gastos" => margen.gasto_Departamento / 100,
                                    "Utilidad de Operación" => margen.utilidad_operacion / 100,
                                    "Sueldos" => margen.nomina,
                                    "" => margen.porc_nomina / 100,
                                    _ => 0
                                };



                                sheet.Cell(renglon, col).Value = valor;
                                var celda = sheet.Cell(renglon, col);
                                celda.Value = valor;

                                var valorGuia = sheet.Cell(renglon, 2).GetValue<string>();

                                if ((conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación") && valor < Convert.ToDouble(valorGuia))
                                {
                                    celda.Style.Font.FontColor = XLColor.Red;
                                }

                                if ((conceptoExcel == "Gastos" || conceptoExcel == "Sueldos" || conceptoExcel == "") && valor > Convert.ToDouble(valorGuia))
                                {
                                    celda.Style.Font.FontColor = XLColor.Red;
                                }

                                if (conceptoExcel == "Sueldos")
                                {
                                    sheet.Row(renglon).Style.NumberFormat.Format = "#,##0.00";
                                }
                                else if (conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación" || conceptoExcel == "Gastos" || conceptoExcel == "")
                                {
                                    sheet.Row(renglon).Style.NumberFormat.Format = "0.00 %";
                                }

                                var rango3 = sheet.Range(renglon, 1, renglon, 17);

                                if ((conceptoExcel == "Gastos" || conceptoExcel == ""))
                                    rango3.Style.Fill.BackgroundColor = XLColor.FromHtml("#f0f0f0");

                                if ((conceptoExcel == "Sueldos" || conceptoExcel == ""))
                                    rango3.Style.Fill.BackgroundColor = XLColor.FromHtml("#e0e0e0");
                            }

                            col++; // Mover a la siguiente columna
                        }

                        renglon++;
                    }

                    renglon++;

                    //FERRETERIA

                    sheet.Cell(renglon, 1).Value = "Ferreteria";
                    var rangoFerreteria = sheet.Range(renglon, 1, renglon, 17);
                    rangoFerreteria.Style.Fill.BackgroundColor = XLColor.FromHtml("#DAE6BE");
                    rangoFerreteria.Style.Font.Bold = true;
                    rangoFerreteria.Style.Font.FontSize = 12;
                    renglon++;

                    var guiasFerreteria = margenes.guias.Where(x => x.departamento == "FERRETERIA");
                    var margenesFerreteria = margenes.margenes.Where(x => x.departamento == "FERRETERIA");

                    var adrsSucursalesFerreteria = margenesFerreteria
                    .Where(x => ordenEspecifico.Any(o => o.idadr == x.idadr && o.idsucursal == x.idsucursal)) // Filtrar solo las sucursales que están en ordenEspecifico
                    .Select(x => new { x.idadr, x.adr, x.idsucursal, x.sucursal })
                    .Distinct()
                    .OrderBy(x => ordenEspecifico.FindIndex(o => o.idadr == x.idadr && o.idsucursal == x.idsucursal)) // Ordenar según el índice en ordenEspecifico
                    .ToList();

                    for (int i = 0; i < conceptosOrdenados.Length; i++)
                    {
                        string conceptoExcel = conceptosOrdenados[i];
                        string conceptoDB = conceptosOrdenadosDB[i];

                        var registrosConcepto = guiasFerreteria.Where(x => x.concepto == conceptoDB).ToList();

                        sheet.Cell(renglon, 1).Value = conceptoExcel;

                        if (registrosConcepto.Any())
                        {
                            foreach (var guia in registrosConcepto)
                            {
                                if (conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación" || conceptoExcel == "Gastos" || conceptoExcel == "")
                                    sheet.Cell(renglon, 2).Value = guia.guia / 100;

                                else
                                    sheet.Cell(renglon, 2).Value = guia.guia;
                            }
                        }

                        int col = 3;
                        foreach (var adrSucursal in adrsSucursales)
                        {
                            var margen = margenesFerreteria.FirstOrDefault(x =>
                                x.idadr == adrSucursal.idadr &&
                                x.adr == adrSucursal.adr &&
                                x.idsucursal == adrSucursal.idsucursal &&
                                x.sucursal == adrSucursal.sucursal);

                            if (margen != null)
                            {
                                double valor = conceptoExcel switch
                                {
                                    "Utilidad bruta" => margen.utilidad_bruta / 100,
                                    "Gastos" => margen.gasto_Departamento / 100,
                                    "Utilidad de Operación" => margen.utilidad_operacion / 100,
                                    "Sueldos" => margen.nomina,
                                    "" => margen.porc_nomina / 100,
                                    _ => 0
                                };



                                sheet.Cell(renglon, col).Value = valor;
                                var celda = sheet.Cell(renglon, col);
                                celda.Value = valor;

                                var valorGuia = sheet.Cell(renglon, 2).GetValue<string>();

                                if ((conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación") && valor < Convert.ToDouble(valorGuia))
                                {
                                    celda.Style.Font.FontColor = XLColor.Red;
                                }

                                if ((conceptoExcel == "Gastos" || conceptoExcel == "Sueldos" || conceptoExcel == "") && valor > Convert.ToDouble(valorGuia))
                                {
                                    celda.Style.Font.FontColor = XLColor.Red;
                                }

                                if (conceptoExcel == "Sueldos")
                                {
                                    sheet.Row(renglon).Style.NumberFormat.Format = "#,##0.00";
                                }
                                else if (conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación" || conceptoExcel == "Gastos" || conceptoExcel == "")
                                {
                                    sheet.Row(renglon).Style.NumberFormat.Format = "0.00 %";
                                }

                                var rango3 = sheet.Range(renglon, 1, renglon, 17);

                                if ((conceptoExcel == "Gastos" || conceptoExcel == ""))
                                    rango3.Style.Fill.BackgroundColor = XLColor.FromHtml("#f0f0f0");

                                if ((conceptoExcel == "Sueldos" || conceptoExcel == ""))
                                    rango3.Style.Fill.BackgroundColor = XLColor.FromHtml("#e0e0e0");
                            }

                            col++; // Mover a la siguiente columna
                        }

                        renglon++;
                    }

                    renglon++;

                    //SISTEMAS DE RIEGO

                    sheet.Cell(renglon, 1).Value = "Sistemas de riego";
                    var rangoSR = sheet.Range(renglon, 1, renglon, 17);
                    rangoSR.Style.Fill.BackgroundColor = XLColor.FromHtml("#DAE6BE");
                    rangoSR.Style.Font.Bold = true;
                    rangoSR.Style.Font.FontSize = 12;
                    renglon++;

                    var guiasSR = margenes.guias.Where(x => x.departamento == "SISTEMAS DE RIEGO");
                    var margenesSR = margenes.margenes.Where(x => x.departamento == "SISTEMAS DE RIEGO");

                    var adrsSucursalesSR = margenesSR
                    .Where(x => ordenEspecifico.Any(o => o.idadr == x.idadr && o.idsucursal == x.idsucursal)) // Filtrar solo las sucursales que están en ordenEspecifico
                    .Select(x => new { x.idadr, x.adr, x.idsucursal, x.sucursal })
                    .Distinct()
                    .OrderBy(x => ordenEspecifico.FindIndex(o => o.idadr == x.idadr && o.idsucursal == x.idsucursal)) // Ordenar según el índice en ordenEspecifico
                    .ToList();

                    for (int i = 0; i < conceptosOrdenados.Length; i++)
                    {
                        string conceptoExcel = conceptosOrdenados[i];
                        string conceptoDB = conceptosOrdenadosDB[i];

                        var registrosConcepto = guiasSR.Where(x => x.concepto == conceptoDB).ToList();

                        sheet.Cell(renglon, 1).Value = conceptoExcel;

                        if (registrosConcepto.Any())
                        {
                            foreach (var guia in registrosConcepto)
                            {
                                if (conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación" || conceptoExcel == "Gastos" || conceptoExcel == "")
                                    sheet.Cell(renglon, 2).Value = guia.guia / 100;

                                else
                                    sheet.Cell(renglon, 2).Value = guia.guia;
                            }
                        }

                        int col = 3;
                        foreach (var adrSucursal in adrsSucursales)
                        {
                            var margen = margenesSR.FirstOrDefault(x =>
                                x.idadr == adrSucursal.idadr &&
                                x.adr == adrSucursal.adr &&
                                x.idsucursal == adrSucursal.idsucursal &&
                                x.sucursal == adrSucursal.sucursal);

                            if (margen != null)
                            {
                                double valor = conceptoExcel switch
                                {
                                    "Utilidad bruta" => margen.utilidad_bruta / 100,
                                    "Gastos" => margen.gasto_Departamento / 100,
                                    "Utilidad de Operación" => margen.utilidad_operacion / 100,
                                    "Sueldos" => margen.nomina,
                                    "" => margen.porc_nomina / 100,
                                    _ => 0
                                };



                                sheet.Cell(renglon, col).Value = valor;
                                var celda = sheet.Cell(renglon, col);
                                celda.Value = valor;

                                var valorGuia = sheet.Cell(renglon, 2).GetValue<string>();

                                if ((conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación") && valor < Convert.ToDouble(valorGuia))
                                {
                                    celda.Style.Font.FontColor = XLColor.Red;
                                }

                                if ((conceptoExcel == "Gastos" || conceptoExcel == "Sueldos" || conceptoExcel == "") && valor > Convert.ToDouble(valorGuia))
                                {
                                    celda.Style.Font.FontColor = XLColor.Red;
                                }

                                if (conceptoExcel == "Sueldos")
                                {
                                    sheet.Row(renglon).Style.NumberFormat.Format = "#,##0.00";
                                }
                                else if (conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación" || conceptoExcel == "Gastos" || conceptoExcel == "")
                                {
                                    sheet.Row(renglon).Style.NumberFormat.Format = "0.00 %";
                                }

                                var rango3 = sheet.Range(renglon, 1, renglon, 17);

                                if ((conceptoExcel == "Gastos" || conceptoExcel == ""))
                                    rango3.Style.Fill.BackgroundColor = XLColor.FromHtml("#f0f0f0");

                                if ((conceptoExcel == "Sueldos" || conceptoExcel == ""))
                                    rango3.Style.Fill.BackgroundColor = XLColor.FromHtml("#e0e0e0");
                            }

                            col++; // Mover a la siguiente columna
                        }

                        renglon++;
                    }

                    renglon++;

                    //AMS

                    sheet.Cell(renglon, 1).Value = "Ams";
                    var rangoAMS = sheet.Range(renglon, 1, renglon, 17);
                    rangoAMS.Style.Fill.BackgroundColor = XLColor.FromHtml("#DAE6BE");
                    rangoAMS.Style.Font.Bold = true;
                    rangoAMS.Style.Font.FontSize = 12;
                    renglon++;

                    var guiasAMS = margenes.guias.Where(x => x.departamento == "AMS");
                    var margenesAMS = margenes.margenes.Where(x => x.departamento == "AMS");

                    var adrsSucursalesAMS = margenesAMS
                    .Where(x => ordenEspecifico.Any(o => o.idadr == x.idadr && o.idsucursal == x.idsucursal)) // Filtrar solo las sucursales que están en ordenEspecifico
                    .Select(x => new { x.idadr, x.adr, x.idsucursal, x.sucursal })
                    .Distinct()
                    .OrderBy(x => ordenEspecifico.FindIndex(o => o.idadr == x.idadr && o.idsucursal == x.idsucursal)) // Ordenar según el índice en ordenEspecifico
                    .ToList();

                    for (int i = 0; i < conceptosOrdenados.Length; i++)
                    {
                        string conceptoExcel = conceptosOrdenados[i];
                        string conceptoDB = conceptosOrdenadosDB[i];

                        var registrosConcepto = guiasAMS.Where(x => x.concepto == conceptoDB).ToList();

                        sheet.Cell(renglon, 1).Value = conceptoExcel;

                        if (registrosConcepto.Any())
                        {
                            foreach (var guia in registrosConcepto)
                            {
                                if (conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación" || conceptoExcel == "Gastos" || conceptoExcel == "")
                                    sheet.Cell(renglon, 2).Value = guia.guia / 100;

                                else
                                    sheet.Cell(renglon, 2).Value = guia.guia;
                            }
                        }

                        int col = 3;
                        foreach (var adrSucursal in adrsSucursales)
                        {
                            var margen = margenesAMS.FirstOrDefault(x =>
                                x.idadr == adrSucursal.idadr &&
                                x.adr == adrSucursal.adr &&
                                x.idsucursal == adrSucursal.idsucursal &&
                                x.sucursal == adrSucursal.sucursal);

                            if (margen != null)
                            {
                                double valor = conceptoExcel switch
                                {
                                    "Utilidad bruta" => margen.utilidad_bruta / 100,
                                    "Gastos" => margen.gasto_Departamento / 100,
                                    "Utilidad de Operación" => margen.utilidad_operacion / 100,
                                    "Sueldos" => margen.nomina,
                                    "" => margen.porc_nomina / 100,
                                    _ => 0
                                };



                                sheet.Cell(renglon, col).Value = valor;
                                var celda = sheet.Cell(renglon, col);
                                celda.Value = valor;

                                var valorGuia = sheet.Cell(renglon, 2).GetValue<string>();

                                if ((conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación") && valor < Convert.ToDouble(valorGuia))
                                {
                                    celda.Style.Font.FontColor = XLColor.Red;
                                }

                                if ((conceptoExcel == "Gastos" || conceptoExcel == "Sueldos" || conceptoExcel == "") && valor > Convert.ToDouble(valorGuia))
                                {
                                    celda.Style.Font.FontColor = XLColor.Red;
                                }

                                if (conceptoExcel == "Sueldos")
                                {
                                    sheet.Row(renglon).Style.NumberFormat.Format = "#,##0.00";
                                }
                                else if (conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación" || conceptoExcel == "Gastos" || conceptoExcel == "")
                                {
                                    sheet.Row(renglon).Style.NumberFormat.Format = "0.00 %";
                                }

                                var rango3 = sheet.Range(renglon, 1, renglon, 17);

                                if ((conceptoExcel == "Gastos" || conceptoExcel == ""))
                                    rango3.Style.Fill.BackgroundColor = XLColor.FromHtml("#f0f0f0");

                                if ((conceptoExcel == "Sueldos" || conceptoExcel == ""))
                                    rango3.Style.Fill.BackgroundColor = XLColor.FromHtml("#e0e0e0");
                            }

                            col++; // Mover a la siguiente columna
                        }

                        renglon++;
                    }

                    renglon++;

                    //USADOS

                    sheet.Cell(renglon, 1).Value = "Usados";
                    var rangoUsados = sheet.Range(renglon, 1, renglon, 17);
                    rangoUsados.Style.Fill.BackgroundColor = XLColor.FromHtml("#DAE6BE");
                    rangoUsados.Style.Font.Bold = true;
                    rangoUsados.Style.Font.FontSize = 12;
                    renglon++;

                    var guiasUsados = margenes.guias.Where(x => x.departamento == "USADOS");
                    var margenesUsados = margenes.margenes.Where(x => x.departamento == "USADOS");

                    var adrsSucursalesUsados = margenesUsados
                    .Where(x => ordenEspecifico.Any(o => o.idadr == x.idadr && o.idsucursal == x.idsucursal)) // Filtrar solo las sucursales que están en ordenEspecifico
                    .Select(x => new { x.idadr, x.adr, x.idsucursal, x.sucursal })
                    .Distinct()
                    .OrderBy(x => ordenEspecifico.FindIndex(o => o.idadr == x.idadr && o.idsucursal == x.idsucursal)) // Ordenar según el índice en ordenEspecifico
                    .ToList();

                    for (int i = 0; i < conceptosOrdenados.Length; i++)
                    {
                        string conceptoExcel = conceptosOrdenados[i];
                        string conceptoDB = conceptosOrdenadosDB[i];

                        var registrosConcepto = guiasUsados.Where(x => x.concepto == conceptoDB).ToList();

                        sheet.Cell(renglon, 1).Value = conceptoExcel;

                        if (registrosConcepto.Any())
                        {
                            foreach (var guia in registrosConcepto)
                            {
                                if (conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación" || conceptoExcel == "Gastos" || conceptoExcel == "")
                                    sheet.Cell(renglon, 2).Value = guia.guia / 100;

                                else
                                    sheet.Cell(renglon, 2).Value = guia.guia;
                            }
                        }

                        int col = 3;
                        foreach (var adrSucursal in adrsSucursales)
                        {
                            var margen = margenesUsados.FirstOrDefault(x =>
                                x.idadr == adrSucursal.idadr &&
                                x.adr == adrSucursal.adr &&
                                x.idsucursal == adrSucursal.idsucursal &&
                                x.sucursal == adrSucursal.sucursal);

                            if (margen != null)
                            {
                                double valor = conceptoExcel switch
                                {
                                    "Utilidad bruta" => margen.utilidad_bruta / 100,
                                    "Gastos" => margen.gasto_Departamento / 100,
                                    "Utilidad de Operación" => margen.utilidad_operacion / 100,
                                    "Sueldos" => margen.nomina,
                                    "" => margen.porc_nomina / 100,
                                    _ => 0
                                };



                                sheet.Cell(renglon, col).Value = valor;
                                var celda = sheet.Cell(renglon, col);
                                celda.Value = valor;

                                var valorGuia = sheet.Cell(renglon, 2).GetValue<string>();

                                if ((conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación") && valor < Convert.ToDouble(valorGuia))
                                {
                                    celda.Style.Font.FontColor = XLColor.Red;
                                }

                                if ((conceptoExcel == "Gastos" || conceptoExcel == "Sueldos" || conceptoExcel == "") && valor > Convert.ToDouble(valorGuia))
                                {
                                    celda.Style.Font.FontColor = XLColor.Red;
                                }

                                if (conceptoExcel == "Sueldos")
                                {
                                    sheet.Row(renglon).Style.NumberFormat.Format = "#,##0.00";
                                }
                                else if (conceptoExcel == "Utilidad bruta" || conceptoExcel == "Utilidad de Operación" || conceptoExcel == "Gastos" || conceptoExcel == "")
                                {
                                    sheet.Row(renglon).Style.NumberFormat.Format = "0.00 %";
                                }

                                var rango3 = sheet.Range(renglon, 1, renglon, 17);

                                if ((conceptoExcel == "Gastos" || conceptoExcel == ""))
                                    rango3.Style.Fill.BackgroundColor = XLColor.FromHtml("#f0f0f0");

                                if ((conceptoExcel == "Sueldos" || conceptoExcel == ""))
                                    rango3.Style.Fill.BackgroundColor = XLColor.FromHtml("#e0e0e0");
                            }

                            col++; // Mover a la siguiente columna
                        }

                        renglon++;
                    }

                    renglon++;

                    //RESUMEN

                    sheet.Cell(renglon, 1).Value = "Resumen";
                    var rangoResumen = sheet.Range(renglon, 1, renglon, 17);
                    rangoResumen.Style.Fill.BackgroundColor = XLColor.FromHtml("#DAE6BE");
                    rangoResumen.Style.Font.Bold = true;
                    rangoResumen.Style.Font.FontSize = 12;
                    renglon++;

                    var filasResumen = new List<string>
                    {
                        "UTILIDAD BRUTA", "GASTOS DEPARTAMENTALES", "GASTOS DE ADMINISTRACION",
                        "GASTOS DE STAFF", "GASTOS DE FINANZAS", "UTILIDAD DE OPERACION", "SUELDOS", ""
                    };

                    // Mapeo corregido con claves en mayúsculas para departamento y valores en minúsculas para concepto
                    var mapeoConceptos = new Dictionary<string, List<string>>
                    {
                        { "UTILIDAD BRUTA", new List<string> { "utilidad bruta" } },
                        { "GASTOS DEPARTAMENTALES", new List<string> { "gastos" } },
                        { "GASTOS DE ADMINISTRACION", new List<string> { "gasto" } },
                        { "GASTOS DE STAFF", new List<string> { "gasto" } },
                        { "GASTOS DE FINANZAS", new List<string> { "gasto" } },
                        { "UTILIDAD DE OPERACION", new List<string> { "gasto" } },
                        { "SUELDOS", new List<string> { "sueldos", "porcentaje sueldos" } }
                    };

                    var valoresUtilidadBruta = ordenEspecifico
                    .Select(orden =>
                        margenes.margenes
                            .Where(x => x.departamento.Trim().ToUpper() == "UTILIDAD BRUTA" &&
                                        x.idadr == orden.idadr &&
                                        x.idsucursal == orden.idsucursal
                            )
                            .Select(x => x.utilidad_bruta)
                            .FirstOrDefault()  
                    )
                    .ToList();

                    var guiaUtilidadBruta = margenes.guias
                    .Where(x => x.departamento.Trim().ToUpper() == "UTILIDAD BRUTA" &&
                                mapeoConceptos.ContainsKey("UTILIDAD BRUTA") &&
                                mapeoConceptos["UTILIDAD BRUTA"].Contains(x.concepto.Trim().ToLower()))
                    .Select(x => x.guia)
                    .FirstOrDefault();

                    sheet.Cell(renglon, 1).Value = "Utilidad Bruta";
                    sheet.Cell(renglon, 2).Value = guiaUtilidadBruta / 100;
                    for (int i = 0; i < valoresUtilidadBruta.Count; i++)
                    {
                        sheet.Cell(renglon, i + 3).Value = valoresUtilidadBruta[i] / 100;
                        if (valoresUtilidadBruta[i] / 100 < guiaUtilidadBruta / 100)  
                        {
                            sheet.Cell(renglon, i + 3).Style.Font.FontColor = XLColor.Red;

                        }
                    }
                    sheet.Row(renglon).Style.NumberFormat.Format = "0.00 %";
                    renglon++;

                    var valoresGastosDep = ordenEspecifico
                    .Select(orden =>
                        margenes.margenes
                            .Where(x => x.departamento.Trim().ToUpper() == "GASTOS DEPARTAMENTALES" &&
                                        x.idadr == orden.idadr &&
                                        x.idsucursal == orden.idsucursal
                            )
                            .Select(x => x.utilidad_bruta)
                            .FirstOrDefault()
                    )
                    .ToList();

                    var guiaGastosDep = margenes.guias
                    .Where(x => x.departamento.Trim().ToUpper() == "GASTOS DEPARTAMENTALES" &&
                                mapeoConceptos.ContainsKey("GASTOS DEPARTAMENTALES") &&
                                mapeoConceptos["GASTOS DEPARTAMENTALES"].Contains(x.concepto.Trim().ToLower()))
                    .Select(x => x.guia)
                    .FirstOrDefault();

                    sheet.Cell(renglon, 1).Value = "Gastos Departamentales";
                    sheet.Cell(renglon, 2).Value = guiaGastosDep / 100;
                    for (int i = 0; i < valoresGastosDep.Count; i++)
                    {
                        sheet.Cell(renglon, i + 3).Value = valoresGastosDep[i] / 100;
                        if (valoresGastosDep[i] / 100 > guiaGastosDep / 100)
                        {
                            sheet.Cell(renglon, i + 3).Style.Font.FontColor = XLColor.Red;

                        }
                    }
                    sheet.Row(renglon).Style.NumberFormat.Format = "0.00 %";
                    var rangodep = sheet.Range(renglon, 1, renglon, 17);
                    rangodep.Style.Fill.BackgroundColor = XLColor.FromHtml("#f0f0f0");
                    renglon++;


                    var valoresGastosAdmin = ordenEspecifico
                    .Select(orden =>
                        margenes.margenes
                            .Where(x => x.departamento.Trim().ToUpper() == "ADMINISTRACIÓN" &&
                                        x.idadr == orden.idadr &&
                                        x.idsucursal == orden.idsucursal
                            )
                            .Select(x => x.utilidad_bruta)
                            .FirstOrDefault()
                    )
                    .ToList();

                    var guiaGastosAdmin = margenes.guias
                    .Where(x => x.departamento.Trim().ToUpper() == "GASTOS DE ADMINISTRACION" &&
                                mapeoConceptos.ContainsKey("GASTOS DE ADMINISTRACION") &&
                                mapeoConceptos["GASTOS DE ADMINISTRACION"].Contains(x.concepto.Trim().ToLower()))
                    .Select(x => x.guia)
                    .FirstOrDefault();

                    sheet.Cell(renglon, 1).Value = "Gastos de Administración";
                    sheet.Cell(renglon, 2).Value = guiaGastosAdmin / 100;
                    for (int i = 0; i < valoresGastosAdmin.Count; i++)
                    {
                        sheet.Cell(renglon, i + 3).Value = valoresGastosAdmin[i] / 100;
                        if (valoresGastosAdmin[i] / 100 > guiaGastosAdmin / 100)
                        {
                            sheet.Cell(renglon, i + 3).Style.Font.FontColor = XLColor.Red;

                        }
                    }
                    sheet.Row(renglon).Style.NumberFormat.Format = "0.00 %";
                    renglon++;

                    var valoresGastosStaff = ordenEspecifico
                    .Select(orden =>
                        margenes.margenes
                            .Where(x => x.departamento.Trim().ToUpper() == "STAFF" &&
                                        x.idadr == orden.idadr &&
                                        x.idsucursal == orden.idsucursal
                            )
                            .Select(x => x.utilidad_bruta)
                            .FirstOrDefault()
                    )
                    .ToList();

                    var guiaGastosStaff = margenes.guias
                    .Where(x => x.departamento.Trim().ToUpper() == "GASTOS DE STAFF" &&
                                mapeoConceptos.ContainsKey("GASTOS DE STAFF") &&
                                mapeoConceptos["GASTOS DE STAFF"].Contains(x.concepto.Trim().ToLower()))
                    .Select(x => x.guia)
                    .FirstOrDefault();

                    sheet.Cell(renglon, 1).Value = "Gastos de Staff";
                    sheet.Cell(renglon, 2).Value = guiaGastosStaff / 100;
                    for (int i = 0; i < valoresGastosStaff.Count; i++)
                    {
                        sheet.Cell(renglon, i + 3).Value = valoresGastosStaff[i] / 100;
                        if (valoresGastosStaff[i] / 100 > guiaGastosStaff / 100)
                        {
                            sheet.Cell(renglon, i + 3).Style.Font.FontColor = XLColor.Red;

                        }
                    }
                    sheet.Row(renglon).Style.NumberFormat.Format = "0.00 %";
                    var rangostaff = sheet.Range(renglon, 1, renglon, 17);
                    rangostaff.Style.Fill.BackgroundColor = XLColor.FromHtml("#f0f0f0");
                    renglon++;

                    var valoresGastosFin = ordenEspecifico
                    .Select(orden =>
                        margenes.margenes
                            .Where(x => x.departamento.Trim().ToUpper() == "FINANZAS" &&
                                        x.idadr == orden.idadr &&
                                        x.idsucursal == orden.idsucursal
                            )
                            .Select(x => x.utilidad_bruta)
                            .FirstOrDefault()
                    )
                    .ToList();

                    var guiaGastosFin = margenes.guias
                    .Where(x => x.departamento.Trim().ToUpper() == "GASTOS DE FINANZAS" &&
                                mapeoConceptos.ContainsKey("GASTOS DE FINANZAS") &&
                                mapeoConceptos["GASTOS DE FINANZAS"].Contains(x.concepto.Trim().ToLower()))
                    .Select(x => x.guia)
                    .FirstOrDefault();

                    sheet.Cell(renglon, 1).Value = "Gastos de Finanzas";
                    sheet.Cell(renglon, 2).Value = guiaGastosFin / 100;
                    for (int i = 0; i < valoresGastosFin.Count; i++)
                    {
                        sheet.Cell(renglon, i + 3).Value = valoresGastosFin[i] / 100;
                        if (valoresGastosFin[i] / 100 > guiaGastosFin / 100)
                        {
                            sheet.Cell(renglon, i + 3).Style.Font.FontColor = XLColor.Red;

                        }
                    }
                    sheet.Row(renglon).Style.NumberFormat.Format = "0.00 %";
                    renglon++;

                    var valoresUtilidadOp = ordenEspecifico
                    .Select(orden =>
                        margenes.margenes
                            .Where(x => x.departamento.Trim().ToUpper() == "UTILIDAD DE OPERACION" &&
                                        x.idadr == orden.idadr &&
                                        x.idsucursal == orden.idsucursal
                            )
                            .Select(x => x.utilidad_bruta)
                            .FirstOrDefault()
                    )
                    .ToList();

                    var guiaUtilidadOp = margenes.guias
                    .Where(x => x.departamento.Trim().ToUpper() == "UTILIDAD DE OPERACION" &&
                                mapeoConceptos.ContainsKey("UTILIDAD DE OPERACION") &&
                                mapeoConceptos["UTILIDAD DE OPERACION"].Contains(x.concepto.Trim().ToLower()))
                    .Select(x => x.guia)
                    .FirstOrDefault();

                    sheet.Cell(renglon, 1).Value = "Utilidad de Operación";
                    sheet.Cell(renglon, 2).Value = guiaUtilidadOp / 100;
                    for (int i = 0; i < valoresUtilidadOp.Count; i++)
                    {
                        sheet.Cell(renglon, i + 3).Value = valoresUtilidadOp[i] / 100;
                        if (valoresUtilidadOp[i] / 100 < guiaUtilidadOp / 100)
                        {
                            sheet.Cell(renglon, i + 3).Style.Font.FontColor = XLColor.Red;

                        }
                    }
                    sheet.Row(renglon).Style.NumberFormat.Format = "0.00 %";
                    var rangoutilidad = sheet.Range(renglon, 1, renglon, 17);
                    rangoutilidad.Style.Fill.BackgroundColor = XLColor.FromHtml("#f0f0f0");
                    renglon++;

                    var valoresSueldos = ordenEspecifico
                    .Select(orden =>
                        margenes.margenes
                            .Where(x => x.departamento.Trim().ToUpper() == "SUELDOS" &&
                                        x.idadr == orden.idadr &&
                                        x.idsucursal == orden.idsucursal
                            )
                            .Select(x => x.nomina)
                            .FirstOrDefault()
                    )
                    .ToList();

                    var guiaSueldos = margenes.guias
                    .Where(x => x.departamento.Trim().ToUpper() == "SUELDOS" &&
                                x.concepto == "Sueldos" &&
                                mapeoConceptos.ContainsKey("SUELDOS") &&
                                mapeoConceptos["SUELDOS"].Contains(x.concepto.Trim().ToLower()))
                                
                    .Select(x => x.guia)
                    .FirstOrDefault();

                    sheet.Cell(renglon, 1).Value = "Sueldos";
                    sheet.Cell(renglon, 2).Value = guiaSueldos;
                    for (int i = 0; i < valoresSueldos.Count; i++)
                    {
                        sheet.Cell(renglon, i + 3).Value = valoresSueldos[i];
                        if (valoresSueldos[i] > guiaSueldos)
                        {
                            sheet.Cell(renglon, i + 3).Style.Font.FontColor = XLColor.Red;

                        }
                    }
                    sheet.Row(renglon).Style.NumberFormat.Format = "#,##0.00";
                    var rangoSueldo = sheet.Range(renglon, 1, renglon, 17);
                    rangoSueldo.Style.Fill.BackgroundColor = XLColor.FromHtml("#e0e0e0");
                    renglon++;

                    var valoresPorcSueldos = ordenEspecifico
                    .Select(orden =>
                        margenes.margenes
                            .Where(x => x.departamento.Trim().ToUpper() == "SUELDOS" &&
                                        x.idadr == orden.idadr &&
                                        x.idsucursal == orden.idsucursal
                            )
                            .Select(x => x.porc_nomina)
                            .FirstOrDefault()
                    )
                    .ToList();

                    var guiaPorcSueldos = margenes.guias
                    .Where(x => x.departamento.Trim().ToUpper() == "SUELDOS" &&
                                x.concepto == "Porcentaje sueldos" &&
                                mapeoConceptos.ContainsKey("SUELDOS") &&
                                mapeoConceptos["SUELDOS"].Contains(x.concepto.Trim().ToLower()))
                    .Select(x => x.guia)
                    .FirstOrDefault();

                    sheet.Cell(renglon, 1).Value = "";
                    sheet.Cell(renglon, 2).Value = guiaPorcSueldos / 100;
                    for (int i = 0; i < valoresPorcSueldos.Count; i++)
                    {
                        sheet.Cell(renglon, i + 3).Value = valoresPorcSueldos[i] / 100;
                        if (valoresPorcSueldos[i] / 100 > guiaPorcSueldos / 100)
                        {
                            sheet.Cell(renglon, i + 3).Style.Font.FontColor = XLColor.Red;

                        }
                    }
                    sheet.Row(renglon).Style.NumberFormat.Format = "0.00 %";
                    var rangoporc = sheet.Range(renglon, 1, renglon, 17);
                    rangoporc.Style.Fill.BackgroundColor = XLColor.FromHtml("#e0e0e0");
                    renglon++;






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
