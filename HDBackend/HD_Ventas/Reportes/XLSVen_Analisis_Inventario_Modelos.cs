using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Ventas.Modelos.AnalisisInventarioModelos;

namespace HD_Ventas.Reportes
{
    /// <summary>
    /// Excel del reporte "Analizar tendencia de inventario". Replica la tabla de la pantalla con las mismas
    /// reglas del front: existencia mensual, estado, avance, orden natural, grupos por linea y totales.
    /// </summary>
    public class XLSVen_Analisis_Inventario_Modelos
    {
        private static readonly string[] MesesTitulo = { "", "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };

        private const string FormatoNumero = "#,##0";
        private const string FormatoDif = "#,##0;-#,##0;0";

        private static readonly XLColor VerdeOscuro = XLColor.FromHtml("#275027");
        private static readonly XLColor VerdeClaro = XLColor.FromHtml("#E8F3E8");
        private static readonly XLColor Gris = XLColor.FromHtml("#EBECEE");
        private static readonly XLColor Amarillo = XLColor.FromHtml("#FFF8DC");
        private static readonly XLColor Rojo = XLColor.FromHtml("#C00000");
        private static readonly XLColor RojoClaro = XLColor.FromHtml("#FADBD8");
        private static readonly XLColor Verde = XLColor.FromHtml("#1E7B34");
        private static readonly XLColor VerdeExist = XLColor.FromHtml("#DDF3E3");
        private static readonly XLColor Ambar = XLColor.FromHtml("#B7791F");
        private static readonly XLColor Resaltado = XLColor.FromHtml("#FFE699");

        // Acumulador: sirve igual para un modelo, para una linea y para el total general
        private class Acum
        {
            public int objetivo, vc, va, vcResto, objResto, inv, pedResto, difStock;
            public int[] exist;
            public int[] proy;
            public Acum(int meses) { exist = new int[meses]; proy = new int[meses]; }
            public int dif => va - vc;
            public int difResto => objResto - vcResto;
            public void Sumar(Acum o)
            {
                objetivo += o.objetivo; vc += o.vc; va += o.va;
                vcResto += o.vcResto; objResto += o.objResto;
                inv += o.inv; pedResto += o.pedResto; difStock += o.difStock;
                for (int i = 0; i < exist.Length; i++) { exist[i] += o.exist[i]; proy[i] += o.proy[i]; }
            }
        }

        // Orden natural: 3028EN antes de 3028ENDT, 2 antes de 10
        private class OrdenNatural : IComparer<string?>
        {
            public int Compare(string? a, string? b)
            {
                a ??= ""; b ??= "";
                int i = 0, j = 0;
                while (i < a.Length && j < b.Length)
                {
                    if (char.IsDigit(a[i]) && char.IsDigit(b[j]))
                    {
                        int si = i; while (i < a.Length && char.IsDigit(a[i])) i++;
                        int sj = j; while (j < b.Length && char.IsDigit(b[j])) j++;
                        string na = a.Substring(si, i - si).TrimStart('0');
                        string nb = b.Substring(sj, j - sj).TrimStart('0');
                        if (na.Length != nb.Length) return na.Length.CompareTo(nb.Length);
                        int c = string.CompareOrdinal(na, nb);
                        if (c != 0) return c;
                    }
                    else
                    {
                        int c = char.ToUpperInvariant(a[i]).CompareTo(char.ToUpperInvariant(b[j]));
                        if (c != 0) return c;
                        i++; j++;
                    }
                }
                return (a.Length - i).CompareTo(b.Length - j);
            }
        }

        private static List<(int ejercicio, int periodo)> ListaMeses(int inicio, int fin)
        {
            var lista = new List<(int, int)>();
            int y = inicio / 100, m = inicio % 100, guard = 0;
            while (m >= 1 && m <= 12 && (y * 100 + m) <= fin && guard++ < 60)
            {
                lista.Add((y, m));
                m++;
                if (m > 12) { m = 1; y++; }
            }
            return lista;
        }

        private static string Mes(int periodo) => periodo >= 1 && periodo <= 12 ? MesesTitulo[periodo] : "";

        public static string TituloPorDefecto(int inicio_tendencia, int fin_tendencia)
            => $"Analizar tendencia de inventario - {Mes(inicio_tendencia % 100)} {inicio_tendencia / 100} - {Mes(fin_tendencia % 100)} {fin_tendencia / 100}";

        public static Task<DocResult> GenerarExcel(mdl_AnalisisInventarioModelos_Reporte_View data, string? titulo,
            int inicio_tendencia, int fin_tendencia, int inicio_resto, int fin_resto,
            int inicio_proyeccion, int fin_proyeccion, int comparativo)
        {
            try
            {
                string sheetname = "ANALISIS DE INVENTARIO";
                if (string.IsNullOrWhiteSpace(titulo)) titulo = TituloPorDefecto(inicio_tendencia, fin_tendencia);

                var meses = ListaMeses(inicio_proyeccion, fin_proyeccion);
                int nM = meses.Count;
                int mesesResto = ListaMeses(inicio_resto, fin_resto).Count;
                bool cruzaAnio = meses.Select(x => x.ejercicio).Distinct().Count() > 1;
                string Etiqueta((int ejercicio, int periodo) x) => cruzaAnio ? $"{Mes(x.periodo)} {x.ejercicio % 100:00}" : Mes(x.periodo);

                // detalle indexado por llave y por ejercicio*100+periodo (los meses sin valores no vienen = 0)
                var detalle = data.detalle
                    .GroupBy(d => d.llave ?? "")
                    .ToDictionary(g => g.Key, g => g.GroupBy(x => x.ejercicio * 100 + x.periodo).ToDictionary(k => k.Key, k => k.First()));

                // ---- calculo por modelo ----
                var filas = new List<(mdl_AnalisisInventarioModelos_Reporte m, Acum a, string estado)>();
                foreach (var m in data.modelos)
                {
                    var a = new Acum(nM)
                    {
                        objetivo = m.objetivo, vc = m.venta_comparativo_corte, va = m.venta_actual_corte,
                        vcResto = m.venta_comparativo_resto, objResto = m.objetivo_resto,
                        inv = m.inventario_actual, pedResto = m.pedidos_resto, difStock = m.diferencia_stock
                    };
                    detalle.TryGetValue(m.llave ?? "", out var det);

                    // existencia(1) = inventario_proyeccion + pedidos + proyectado - objetivo; luego acumulada
                    int existencia = m.inventario_proyeccion;
                    int objetivoProy = 0, pedidosProy = 0;
                    bool faltante = false;
                    for (int i = 0; i < nM; i++)
                    {
                        int obj = 0, ped = 0, proy = 0;
                        if (det != null && det.TryGetValue(meses[i].ejercicio * 100 + meses[i].periodo, out var d))
                        { obj = d.objetivo; ped = d.pedidos; proy = d.proyectado; }
                        existencia = existencia + ped + proy - obj;
                        a.exist[i] = existencia;
                        a.proy[i] = proy;
                        objetivoProy += obj;
                        pedidosProy += ped;
                        if (existencia < 0) faltante = true;
                    }

                    // Estado: gana el primero que se cumpla (proyectado NO cuenta en "Sin venta est.")
                    string estado = faltante ? "Faltante"
                        : (objetivoProy == 0 && m.inventario_proyeccion + pedidosProy > 0) ? "Sin venta est."
                        : (objetivoProy == 0) ? "Sin movimiento"
                        : "En equilibrio";
                    filas.Add((m, a, estado));
                }

                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    const int colFijas = 14;
                    int colTotal = colFijas + nM * 2;
                    int renglon = XLSEncabezado.Encabezado(ref sheet, titulo!, colTotal);

                    // ---- encabezado de grupos ----
                    string rangoTend = $"{Mes(inicio_tendencia % 100).ToUpper()}-{Mes(fin_tendencia % 100).ToUpper()}";
                    string rangoResto = $"{Mes(inicio_resto % 100).ToUpper()}-{Mes(fin_resto % 100).ToUpper()}";
                    int anioActual = fin_tendencia / 100;

                    void Grupo(int c1, int c2, string texto)
                    {
                        var r = sheet.Range(renglon, c1, renglon, c2);
                        r.Merge();
                        r.Value = texto;
                    }
                    Grupo(1, 2, "");
                    Grupo(3, 7, $"TENDENCIA {rangoTend}");
                    Grupo(8, 10, $"RESTO DEL AÑO {rangoResto}");
                    Grupo(11, 14, "COBERTURA");
                    if (nM > 0) Grupo(15, colTotal, "PROYECCIÓN DE EXISTENCIA Y PEDIDOS A AGREGAR");
                    renglon++;

                    // ---- encabezado de columnas ----
                    var encabezados = new List<string>
                    {
                        "Modelo", "Estado",
                        $"Objetivo {anioActual}", $"Venta {comparativo}", $"Venta {anioActual}", $"Dif. vs {comparativo}", "Avance objetivo",
                        $"Venta {comparativo}", $"Objetivo {anioActual}", "Dif.",
                        "Inv. actual", $"Pedidos {rangoResto}", "Inv. + ped. - obj.", "Meses estimados"
                    };
                    foreach (var mes in meses) encabezados.Add($"Exist. {Etiqueta(mes)}");
                    foreach (var mes in meses) encabezados.Add($"+ Ped. {Etiqueta(mes)}");
                    for (int c = 0; c < encabezados.Count; c++) sheet.Cell(renglon, c + 1).Value = encabezados[c];

                    int encGrupos = renglon - 1, encCols = renglon;
                    var rEnc = sheet.Range(encGrupos, 1, encCols, colTotal);
                    rEnc.Style.Fill.BackgroundColor = VerdeOscuro;
                    rEnc.Style.Font.FontColor = XLColor.White;
                    rEnc.Style.Font.Bold = true;
                    rEnc.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rEnc.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    rEnc.Style.Alignment.WrapText = true;
                    sheet.Row(encCols).Height = 32;
                    sheet.Range(encCols, 1, encCols, colTotal).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                    sheet.Range(encCols, 1, encCols, colTotal).Style.Border.BottomBorderColor = XLColor.FromHtml("#E9AE06");
                    renglon++;
                    int primerRenglonDatos = renglon;

                    // ---- cuerpo: lineas del catalogo (orden natural), luego "Sin linea", luego "No registrados" ----
                    var orden = new OrdenNatural();
                    var grupos = filas
                        .GroupBy(f => f.m.en_catalogo == 0 ? (tipo: 2, nombre: "No registrados en Modelos")
                                    : string.IsNullOrWhiteSpace(f.m.linea) ? (tipo: 1, nombre: "Sin línea")
                                    : (tipo: 0, nombre: f.m.linea!.Trim()))
                        .OrderBy(g => g.Key.tipo)
                        .ThenBy(g => g.Key.nombre, orden);

                    var total = new Acum(nM);
                    foreach (var grupo in grupos)
                    {
                        var lista = grupo.OrderBy(f => f.m.modelo, orden).ToList();
                        var sub = new Acum(nM);
                        foreach (var f in lista) sub.Sumar(f.a);
                        total.Sumar(sub);

                        string etiquetaGrupo = grupo.Key.tipo == 0 ? $"Línea {grupo.Key.nombre}" : grupo.Key.nombre;
                        var rg = sheet.Range(renglon, 1, renglon, colTotal);
                        rg.Style.Fill.BackgroundColor = VerdeClaro;
                        rg.Style.Font.Bold = true;
                        EscribirFila(sheet, renglon, $"{etiquetaGrupo}  ·  {lista.Count} modelos", null, sub, null, true, nM, mesesResto);
                        renglon++;

                        foreach (var f in lista)
                        {
                            EscribirFila(sheet, renglon, f.m.modelo ?? "", f.estado, f.a, f.m.meses_inventario, false, nM, mesesResto);
                            var cm = sheet.Cell(renglon, 1);
                            cm.Style.Font.Bold = true;
                            if (f.m.en_catalogo == 0)
                            {
                                cm.Style.Fill.BackgroundColor = Resaltado;
                                cm.Style.Font.FontColor = Rojo;
                                if (!string.IsNullOrEmpty(f.m.advertencia)) cm.CreateComment().AddText(f.m.advertencia);
                            }
                            renglon++;
                        }
                    }

                    // ---- total general ----
                    var rt = sheet.Range(renglon, 1, renglon, colTotal);
                    rt.Style.Fill.BackgroundColor = Gris;
                    rt.Style.Font.Bold = true;
                    rt.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                    EscribirFila(sheet, renglon, "Total general", null, total, null, true, nM, mesesResto);

                    // ---- formato general ----
                    sheet.Range(primerRenglonDatos, 3, renglon, colTotal).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Range(primerRenglonDatos, 1, renglon, colTotal).Style.Border.BottomBorder = XLBorderStyleValues.Hair;
                    foreach (int c in new[] { 2, 7, 10, 14, 14 + nM })
                    {
                        if (c <= colTotal)
                        {
                            sheet.Range(encGrupos, c, renglon, c).Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            sheet.Range(encGrupos, c, renglon, c).Style.Border.RightBorderColor = VerdeOscuro;
                        }
                    }

                    sheet.Columns().AdjustToContents();
                    sheet.Column(1).Width = 30;
                    sheet.Column(2).Width = 16;
                    for (int c = 3; c <= colTotal; c++) if (sheet.Column(c).Width < 11) sheet.Column(c).Width = 11;
                    sheet.Row(1).Height = 50;
                    sheet.SheetView.FreezeRows(encCols);
                    sheet.SheetView.FreezeColumns(2);

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return Task.FromResult(new DocResult
                        {
                            documento = Convert.ToBase64String(stream.ToArray()),
                            filename = sheetname
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        private static void EscribirFila(IXLWorksheet sheet, int r, string etiqueta, string? estado, Acum a, decimal? meses, bool esResumen, int nM, int mesesResto)
        {
            sheet.Cell(r, 1).Value = etiqueta;
            if (estado != null)
            {
                var ce = sheet.Cell(r, 2);
                ce.Value = estado;
                ce.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                switch (estado)
                {
                    case "Faltante": ce.Style.Fill.BackgroundColor = RojoClaro; ce.Style.Font.FontColor = Rojo; break;
                    case "Sin venta est.": ce.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFF3CD"); ce.Style.Font.FontColor = XLColor.FromHtml("#8A6D00"); break;
                    case "Sin movimiento": ce.Style.Fill.BackgroundColor = Gris; ce.Style.Font.FontColor = XLColor.FromHtml("#495057"); break;
                    default: ce.Style.Fill.BackgroundColor = XLColor.FromHtml("#D1E7DD"); ce.Style.Font.FontColor = XLColor.FromHtml("#0F5132"); break; // En equilibrio
                }
            }

            Numero(sheet, r, 3, a.objetivo);
            Numero(sheet, r, 4, a.vc);
            Numero(sheet, r, 5, a.va);
            Dif(sheet, r, 6, a.dif);

            // Avance = round(venta_actual / objetivo * 100); verde >= 70 %, ambar si es menor; sin objetivo = "—"
            if (a.objetivo > 0)
            {
                double pct = Math.Round((double)a.va / a.objetivo * 100, MidpointRounding.AwayFromZero);
                var c = sheet.Cell(r, 7);
                c.Value = pct / 100;
                c.Style.NumberFormat.Format = "0%";
                c.Style.Font.FontColor = pct >= 70 ? Verde : Ambar;
            }
            else sheet.Cell(r, 7).Value = "—";

            Numero(sheet, r, 8, a.vcResto);
            Numero(sheet, r, 9, a.objResto);
            Dif(sheet, r, 10, a.difResto);

            Numero(sheet, r, 11, a.inv);
            sheet.Cell(r, 11).Style.Fill.BackgroundColor = esResumen ? sheet.Cell(r, 11).Style.Fill.BackgroundColor : Amarillo;
            Numero(sheet, r, 12, a.pedResto);
            Dif(sheet, r, 13, a.difStock);

            // Meses estimados: 1 decimal, rojo si es menor a los meses del rango del resto; vacio en subtotales/total
            if (!esResumen && meses.HasValue)
            {
                var c = sheet.Cell(r, 14);
                c.Value = meses.Value;
                c.Style.NumberFormat.Format = "0.0";
                if (meses.Value < mesesResto) c.Style.Font.FontColor = Rojo;
            }
            else sheet.Cell(r, 14).Value = "—";

            for (int i = 0; i < nM; i++)
            {
                var ce = sheet.Cell(r, 15 + i);
                ce.Value = a.exist[i];
                ce.Style.NumberFormat.Format = FormatoNumero;
                if (a.exist[i] < 0)
                {
                    ce.Style.Font.FontColor = Rojo;
                    ce.Style.Fill.BackgroundColor = RojoClaro;
                }
                else if (a.exist[i] > 0 && !esResumen)
                {
                    ce.Style.Fill.BackgroundColor = VerdeExist;
                }

                var cp = sheet.Cell(r, 15 + nM + i);
                cp.Value = a.proy[i];
                cp.Style.NumberFormat.Format = esResumen ? "\"+\"#,##0;-#,##0;0" : FormatoNumero;
            }
        }

        private static void Numero(IXLWorksheet sheet, int r, int c, int valor)
        {
            var ce = sheet.Cell(r, c);
            ce.Value = valor;
            ce.Style.NumberFormat.Format = FormatoNumero;
        }

        private static void Dif(IXLWorksheet sheet, int r, int c, int valor)
        {
            var ce = sheet.Cell(r, c);
            ce.Value = valor;
            ce.Style.NumberFormat.Format = FormatoDif;
            if (valor > 0) ce.Style.Font.FontColor = Verde;
            else if (valor < 0) ce.Style.Font.FontColor = Rojo;
        }
    }
}
