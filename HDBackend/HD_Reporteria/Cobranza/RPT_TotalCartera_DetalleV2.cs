using HD_Cobranza.Modelos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace HD_Reporteria.Cobranza
{
    /// <summary>
    /// Version ejecutiva del reporte "Resumen de cartera detalle".
    /// No sustituye a RPT_TotalCartera_Detalle: es un archivo aparte para poder
    /// cambiar el reporte desde el controlador con una sola linea.
    /// </summary>
    public class RPT_TotalCartera_DetalleV2
    {
        // ==================================================================
        //  PARAMETROS RAPIDOS  (cambiar aqui, sin tocar el resto del codigo)
        // ==================================================================

        /// <summary>Segunda tabla: true = solo clientes con saldo vencido. false = todos.</summary>
        public const bool SOLO_CLIENTES_CON_VENCIDO = true;

        /// <summary>Agrupa el detalle por sucursal con su banda y subtotal.</summary>
        public const bool AGRUPAR_POR_SUCURSAL = true;

        /// <summary>Barra de composicion (100%) al final de cada renglon.</summary>
        public const bool MOSTRAR_BARRA_MEZCLA = true;

        /// <summary>false = los ceros se imprimen como guion tenue.</summary>
        public const bool MOSTRAR_CEROS = false;

        /// <summary>Formato de los porcentajes: "N1" o "N2".</summary>
        public const string FORMATO_PCT = "N1";

        /// <summary>Alto minimo del renglon de detalle (antes era 30).</summary>
        public const float ALTO_FILA = 16f;

        /// <summary>Tamano de letra de los importes.</summary>
        private const float TAM_NUM = 7.5f;

        /// <summary>Aire a cada lado de la linea separadora de la banda de contexto.</summary>
        private const float SEPARACION_CTX = 20f;

        private const string GUION = "-";

        // ---------- Paleta ----------
        private const string VERDE = "#477c2c";
        private const string VERDE_OSC = "#275027";
        private const string VERDE_PROF = "#1c3a1c";
        private const string VERDE_TINTE = "#eaf0e3";
        private const string VERDE_BORDE = "#c7d4ba";
        private const string GRIS_SUB = "#eff2eb";
        private const string HAIR = "#dde3d6";
        private const string ZEBRA = "#f5f8f1";
        private const string BLANCO = "#ffffff";
        private const string TINTA = "#1b2416";
        private const string TENUE = "#b9c1b0";
        private const string SUAVE = "#7a8372";
        private const string ROJO = "#a32015";

        private const string M_JUR = "#a32015";
        private const string M_ACT = "#477c2c";
        private const string M_PV = "#8fae3e";
        private const string M_VEN = "#c77e0a";
        private const string M_VACIO = "#edf0e9";

        private const string HEAT1 = "#f6f8f2";
        private const string HEAT2 = "#eaf0e0";
        private const string HEAT3 = "#f6e9ce";
        private const string HEAT4 = "#efd2a8";
        private const string HEAT5 = "#e4b478";

        private const string RUTA_LOGO = "C:\\Nube\\HumayaDigital\\HumayaDigitalBackEnd\\HDBackend\\HD_Reporteria\\Imagenes\\Logo.jpg";

        // ==================================================================

        public static RPT_Result Generar(IEnumerable<mdlCob_TotalCartera_Detalle> resumen, bool? soloConVencido = null)
        {
            try
            {
                // Si el acomodo no se resuelve, que truene rapido en lugar de quedarse
                // generando hojas para siempre.
                QuestPDF.Settings.DocumentLayoutExceptionThreshold = 100;

                bool filtrarVencido = soloConVencido ?? SOLO_CLIENTES_CON_VENCIDO;
                string fontFamily = "Calibri";

                // El origen de datos agrega un renglon con razonsocial = "TOTAL".
                // Aqui se descarta y los totales se recalculan, para que la
                // agrupacion y los subtotales cuadren.
                var lista = (resumen ?? Enumerable.Empty<mdlCob_TotalCartera_Detalle>())
                    .Where(x => !string.Equals((x.razonsocial ?? "").Trim(), "TOTAL", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                var grupos = AGRUPAR_POR_SUCURSAL
                    ? lista.GroupBy(x => string.IsNullOrWhiteSpace(x.sucursal) ? "SIN SUCURSAL" : x.sucursal!.Trim())
                           .Select(g => new Grupo(g.Key, g.ToList())).ToList()
                    : new List<Grupo> { new Grupo("", lista) };

                var vencidos = filtrarVencido ? lista.Where(x => x.vencido != 0).ToList() : lista;
                var gruposVencido = AGRUPAR_POR_SUCURSAL
                    ? vencidos.GroupBy(x => string.IsNullOrWhiteSpace(x.sucursal) ? "SIN SUCURSAL" : x.sucursal!.Trim())
                              .Select(g => new Grupo(g.Key, g.ToList())).ToList()
                    : new List<Grupo> { new Grupo("", vencidos.ToList()) };

                double gCartera = lista.Sum(Cartera);
                double gSaldoFavor = lista.Sum(x => x.saldoafavor);
                double gNeto = lista.Sum(Neto);
                double gJuridico = lista.Sum(x => x.juridico);
                double gActivo = lista.Sum(x => x.activo);
                double gPorVencer = lista.Sum(x => x.porvencer);
                double gVencido = lista.Sum(x => x.vencido);
                double gVencidoTabla = vencidos.Sum(x => x.vencido);

                string fechaCorte = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();

                byte[] doc = Document.Create(document =>
                {
                    // ============================================================
                    //  HOJA(S) 1 : DETALLE
                    // ============================================================
                    document.Page(page =>
                    {
                        page.Size(PageSizes.A4.Landscape());

                        page.Header().Element(c => Encabezado(c, "RESUMEN DE CARTERA DETALLE", fontFamily));

                        page.Content().PaddingTop(6).PaddingLeft(30).PaddingRight(30).Column(col1 =>
                        {
                            col1.Item().Element(c => BandaContexto(c, fontFamily, new[]
                            {
                                ("CORTE AL", fechaCorte, false),
                                ("MONEDA", "MXN", false),
                                ("CLIENTES", lista.Count.ToString("N0"), false),
                                ("CARTERA TOTAL", gCartera.ToString("N2"), true),
                                ("VENCIDA", gVencido.ToString("N2") + "  ·  " + Pct(gVencido, gCartera) + "%", true)
                            }));

                            col1.Item().PaddingTop(8).Table(tabla =>
                            {
                                tabla.ColumnsDefinition(c =>
                                {
                                    c.ConstantColumn(34);   // id
                                    c.RelativeColumn(1);    // razon social
                                    c.ConstantColumn(56);   // cartera
                                    c.ConstantColumn(52);   // saldo a favor
                                    c.ConstantColumn(56);   // neto
                                    c.ConstantColumn(56);   // juridico
                                    c.ConstantColumn(30);   // %
                                    c.ConstantColumn(56);   // activa
                                    c.ConstantColumn(30);   // %
                                    c.ConstantColumn(56);   // por vencer
                                    c.ConstantColumn(30);   // %
                                    c.ConstantColumn(56);   // vencida
                                    c.ConstantColumn(30);   // %
                                    c.ConstantColumn(MOSTRAR_BARRA_MEZCLA ? 54 : 1); // mezcla
                                });

                                tabla.Header(header =>
                                {
                                    // --- primer nivel: grupos de columnas ---
                                    CeldaGrupo(header.Cell().ColumnSpan(2), "CLIENTE", fontFamily);
                                    CeldaGrupo(header.Cell().ColumnSpan(3), "SALDO", fontFamily);
                                    CeldaGrupo(header.Cell().ColumnSpan(8), "COMPOSICION DE LA CARTERA", fontFamily);
                                    CeldaGrupo(header.Cell(), MOSTRAR_BARRA_MEZCLA ? "MEZCLA" : "", fontFamily);

                                    // --- segundo nivel: columnas ---
                                    CeldaTitulo(header.Cell(), "ID", fontFamily, false);
                                    CeldaTitulo(header.Cell(), "RAZON SOCIAL", fontFamily, false);
                                    CeldaTitulo(header.Cell(), "CARTERA", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "A FAVOR", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "NETO", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "JURIDICO", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "%", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "ACTIVA", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "%", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "POR VENCER", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "%", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "VENCIDA", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "%", fontFamily, true);
                                    CeldaTitulo(header.Cell(), MOSTRAR_BARRA_MEZCLA ? "100%" : "", fontFamily, true);
                                });

                                foreach (var grupo in grupos)
                                {
                                    if (AGRUPAR_POR_SUCURSAL && grupos.Count > 0 && !string.IsNullOrEmpty(grupo.Nombre))
                                        BandaGrupo(tabla.Cell().ColumnSpan(14),
                                                   "SUCURSAL  ·  " + grupo.Nombre.ToUpper(),
                                                   grupo.Filas.Count + (grupo.Filas.Count == 1 ? " cliente" : " clientes"),
                                                   fontFamily);

                                    int i = 0;
                                    foreach (var mdl in grupo.Filas)
                                    {
                                        string fondo = (i++ % 2 == 1) ? ZEBRA : BLANCO;
                                        double cartera = Cartera(mdl);

                                        Celda(tabla.Cell(), fondo, mdl.idcliente.ToString(), fontFamily, false, false, SUAVE, 7.5f);
                                        Celda(tabla.Cell(), fondo, mdl.razonsocial ?? "", fontFamily, false, false, TINTA, 8f);
                                        Celda(tabla.Cell(), fondo, Mon(cartera), fontFamily, true, false, TINTA, TAM_NUM);
                                        Celda(tabla.Cell(), fondo, Mon(mdl.saldoafavor), fontFamily, true, false, mdl.saldoafavor != 0 ? ROJO : TENUE, TAM_NUM);
                                        Celda(tabla.Cell(), fondo, Mon(Neto(mdl)), fontFamily, true, false, Neto(mdl) < 0 ? ROJO : TINTA, TAM_NUM);
                                        Celda(tabla.Cell(), fondo, Mon(mdl.juridico), fontFamily, true, false, TINTA, TAM_NUM);
                                        Celda(tabla.Cell(), fondo, PctTxt(mdl.juridico, cartera), fontFamily, true, false, SUAVE, 7.5f);
                                        Celda(tabla.Cell(), fondo, Mon(mdl.activo), fontFamily, true, false, TINTA, TAM_NUM);
                                        Celda(tabla.Cell(), fondo, PctTxt(mdl.activo, cartera), fontFamily, true, false, SUAVE, 7.5f);
                                        Celda(tabla.Cell(), fondo, Mon(mdl.porvencer), fontFamily, true, false, TINTA, TAM_NUM);
                                        Celda(tabla.Cell(), fondo, PctTxt(mdl.porvencer, cartera), fontFamily, true, false, SUAVE, 7.5f);
                                        Celda(tabla.Cell(), fondo, Mon(mdl.vencido), fontFamily, true, false, TINTA, TAM_NUM);
                                        Celda(tabla.Cell(), fondo, PctTxt(mdl.vencido, cartera), fontFamily, true, false, SUAVE, 7.5f);
                                        CeldaMezcla(tabla.Cell(), fondo, mdl.juridico, mdl.activo, mdl.porvencer, mdl.vencido);
                                    }

                                    if (AGRUPAR_POR_SUCURSAL && grupos.Count > 1 && !string.IsNullOrEmpty(grupo.Nombre))
                                    {
                                        double sCartera = grupo.Filas.Sum(Cartera);
                                        FilaResumen(tabla, "SUBTOTAL " + grupo.Nombre.ToUpper(), fontFamily,
                                            sCartera,
                                            grupo.Filas.Sum(x => x.saldoafavor),
                                            grupo.Filas.Sum(Neto),
                                            grupo.Filas.Sum(x => x.juridico),
                                            grupo.Filas.Sum(x => x.activo),
                                            grupo.Filas.Sum(x => x.porvencer),
                                            grupo.Filas.Sum(x => x.vencido),
                                            false);
                                    }
                                }

                                FilaResumen(tabla, "TOTAL GENERAL  ·  " + lista.Count.ToString("N0") + " clientes", fontFamily,
                                    gCartera, gSaldoFavor, gNeto, gJuridico, gActivo, gPorVencer, gVencido, true);
                            });
                        });

                        page.Footer().Element(c => PieDePagina(c, fontFamily, "Resumen de cartera detalle", true));
                    });

                    // ============================================================
                    //  HOJA(S) 2 : ANTIGUEDAD DE SALDOS VENCIDOS
                    // ============================================================
                    document.Page(page =>
                    {
                        page.Size(PageSizes.A4.Landscape());

                        page.Header().Element(c => Encabezado(c, "CARTERA VENCIDA POR ANTIGUEDAD", fontFamily));

                        page.Content().PaddingTop(6).PaddingLeft(30).PaddingRight(30).Column(col1 =>
                        {
                            col1.Item().Element(c => BandaContexto(c, fontFamily, new[]
                            {
                                ("CORTE AL", fechaCorte, false),
                                (filtrarVencido ? "CLIENTES CON MORA" : "CLIENTES", vencidos.Count().ToString("N0"), false),
                                ("VENCIDO TOTAL", gVencidoTabla.ToString("N2"), true),
                                ("CONCENTRACION +90", Pct(vencidos.Sum(x => x.mas90), gVencidoTabla) + "%", true)
                            }));

                            col1.Item().PaddingTop(8).Table(tabla =>
                            {
                                tabla.ColumnsDefinition(c =>
                                {
                                    c.ConstantColumn(34);   // id
                                    c.RelativeColumn(1);    // razon social
                                    c.ConstantColumn(62);   // vencido total
                                    c.ConstantColumn(56); c.ConstantColumn(30);  // 1 a 15
                                    c.ConstantColumn(56); c.ConstantColumn(30);  // mas 15
                                    c.ConstantColumn(56); c.ConstantColumn(30);  // mas 30
                                    c.ConstantColumn(56); c.ConstantColumn(30);  // mas 60
                                    c.ConstantColumn(56); c.ConstantColumn(30);  // mas 90
                                });

                                tabla.Header(header =>
                                {
                                    CeldaGrupo(header.Cell().ColumnSpan(2), "CLIENTE", fontFamily);
                                    CeldaGrupo(header.Cell(), "VENCIDO", fontFamily);
                                    CeldaGrupo(header.Cell().ColumnSpan(10), "DIAS DE ATRASO", fontFamily);

                                    CeldaTitulo(header.Cell(), "ID", fontFamily, false);
                                    CeldaTitulo(header.Cell(), "RAZON SOCIAL", fontFamily, false);
                                    CeldaTitulo(header.Cell(), "TOTAL", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "DE 1 A 15", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "%", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "MAS DE 15", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "%", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "MAS DE 30", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "%", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "MAS DE 60", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "%", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "MAS DE 90", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "%", fontFamily, true);
                                });

                                foreach (var grupo in gruposVencido)
                                {
                                    if (grupo.Filas.Count == 0) continue;

                                    if (AGRUPAR_POR_SUCURSAL && !string.IsNullOrEmpty(grupo.Nombre))
                                        BandaGrupo(tabla.Cell().ColumnSpan(13),
                                                   "SUCURSAL  ·  " + grupo.Nombre.ToUpper(),
                                                   grupo.Filas.Count + (grupo.Filas.Count == 1 ? " cliente" : " clientes"),
                                                   fontFamily);

                                    int i = 0;
                                    foreach (var mdl in grupo.Filas)
                                    {
                                        string fondo = (i++ % 2 == 1) ? ZEBRA : BLANCO;

                                        Celda(tabla.Cell(), fondo, mdl.idcliente.ToString(), fontFamily, false, false, SUAVE, 7.5f);
                                        Celda(tabla.Cell(), fondo, mdl.razonsocial ?? "", fontFamily, false, false, TINTA, 8f);
                                        Celda(tabla.Cell(), fondo, Mon(mdl.vencido), fontFamily, true, false, TINTA, TAM_NUM);

                                        ParRango(tabla, fondo, mdl.de1a15, mdl.vencido, HEAT1, fontFamily);
                                        ParRango(tabla, fondo, mdl.mas15, mdl.vencido, HEAT2, fontFamily);
                                        ParRango(tabla, fondo, mdl.mas30, mdl.vencido, HEAT3, fontFamily);
                                        ParRango(tabla, fondo, mdl.mas60, mdl.vencido, HEAT4, fontFamily);
                                        ParRango(tabla, fondo, mdl.mas90, mdl.vencido, HEAT5, fontFamily);
                                    }
                                }

                                // ---- total vencido ----
                                double tV = vencidos.Sum(x => x.vencido);
                                Celda(tabla.Cell().ColumnSpan(2), VERDE_OSC, "TOTAL VENCIDO", fontFamily, false, true, BLANCO, 8f);
                                Celda(tabla.Cell(), VERDE_OSC, tV.ToString("N2"), fontFamily, true, true, BLANCO, 8f);
                                foreach (var v in new[] { vencidos.Sum(x => x.de1a15), vencidos.Sum(x => x.mas15), vencidos.Sum(x => x.mas30), vencidos.Sum(x => x.mas60), vencidos.Sum(x => x.mas90) })
                                {
                                    Celda(tabla.Cell(), VERDE_OSC, v.ToString("N2"), fontFamily, true, true, BLANCO, 8f);
                                    Celda(tabla.Cell(), VERDE_OSC, Pct(v, tV), fontFamily, true, true, "#cfe0c2", 7.5f);
                                }
                            });
                        });

                        page.Footer().Element(c => PieDePagina(c, fontFamily, "Cartera vencida por antiguedad", false));
                    });
                }).GeneratePdf();

                RPT_Result result = new RPT_Result();
                result.extension = "pdf";
                result.nombredocumento = "RESUMEN CARTERA DETALLE POR CLIENTE";
                result.documento = Convert.ToBase64String(doc);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // ==================================================================
        //  Piezas del documento
        // ==================================================================

        /// <summary>Encabezado con el logo. Se conserva tal cual el del reporte original.</summary>
        private static void Encabezado(IContainer container, string titulo, string fontFamily)
        {
            container.Height(120).Row(row =>
            {
                row.RelativeItem().PaddingTop(35).Height(50).Background(VERDE).Row(row2 => { });

                row.ConstantColumn(0).Row(row1 =>
                {
                    var rutaImagen = Path.Combine(RUTA_LOGO);
                    byte[] imageData = System.IO.File.ReadAllBytes(rutaImagen);
                    row.ConstantItem(120).Image(imageData);

                    row.ConstantColumn(693).PaddingTop(35).Height(50).Background(VERDE).Row(row2 =>
                    {
                        row2.RelativeItem().Padding(10).PaddingLeft(30)
                            .Text(titulo).FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
                    });
                });
            });
        }

        /// <summary>Franja delgada con los datos de contexto del corte.</summary>
        private static void BandaContexto(IContainer container, string fontFamily, (string k, string v, bool destacado)[] datos)
        {
            container.BorderTop(2).BorderBottom(1).BorderColor(HAIR).PaddingVertical(6).Row(row =>
            {
                for (int i = 0; i < datos.Length; i++)
                {
                    var d = datos[i];
                    bool ultimo = i == datos.Length - 1;
                    var item = ultimo ? row.RelativeItem() : row.AutoItem();

                    // El borde va primero y el padding despues: asi la linea
                    // separadora queda por fuera del texto y no pegada a el.
                    item.BorderRight(ultimo ? 0 : 1).BorderColor(HAIR)
                        .PaddingLeft(i == 0 ? 0 : SEPARACION_CTX)
                        .PaddingRight(ultimo ? 0 : SEPARACION_CTX)
                        .Column(c =>
                        {
                            c.Item().AlignLeft().Text(d.k).FontSize(6.5f).FontColor(SUAVE).Bold().FontFamily(fontFamily);
                            c.Item().AlignLeft().Text(d.v).FontSize(d.destacado ? 10f : 9f)
                                      .FontColor(d.destacado ? VERDE_OSC : TINTA).Bold().FontFamily(fontFamily);
                        });
                }
            });
        }

        private static void PieDePagina(IContainer container, string fontFamily, string nombreReporte, bool conLeyenda)
        {
            container.PaddingLeft(30).PaddingRight(30).PaddingBottom(10)
                .BorderTop(1).BorderColor(HAIR).PaddingTop(4).Row(row =>
                {
                    row.RelativeItem().AlignLeft().AlignMiddle()
                       .Text("Maquinaria del Humaya  ·  Cobranza  ·  " + nombreReporte)
                       .FontSize(7).FontColor(SUAVE).FontFamily(fontFamily);

                    if (conLeyenda && MOSTRAR_BARRA_MEZCLA)
                    {
                        row.AutoItem().AlignMiddle().Row(r =>
                        {
                            Leyenda(r, M_JUR, "Juridico", fontFamily);
                            Leyenda(r, M_ACT, "Activa", fontFamily);
                            Leyenda(r, M_PV, "Por vencer", fontFamily);
                            Leyenda(r, M_VEN, "Vencida", fontFamily);
                        });
                    }

                    row.RelativeItem().AlignRight().AlignMiddle().Text(t =>
                    {
                        t.DefaultTextStyle(x => x.FontSize(7.5f).FontColor(TINTA).FontFamily(fontFamily).Bold());
                        t.Span("Hoja ");
                        t.CurrentPageNumber();
                        t.Span(" de ");
                        t.TotalPages();
                    });
                });
        }

        private static void Leyenda(RowDescriptor row, string color, string texto, string fontFamily)
        {
            row.AutoItem().PaddingRight(3).AlignMiddle().Width(6).Height(6).Background(color);
            row.AutoItem().PaddingRight(12).AlignMiddle()
               .Text(texto).FontSize(7).FontColor(SUAVE).FontFamily(fontFamily);
        }

        // ==================================================================
        //  Celdas
        // ==================================================================

        private static void CeldaGrupo(IContainer c, string texto, string fontFamily)
        {
            c.Background(VERDE_PROF).BorderRight(1).BorderColor("#ffffff")
             .MinHeight(14).AlignMiddle().AlignCenter().PaddingVertical(2)
             .Text(texto).FontSize(6.5f).Bold().FontColor("#fff").FontFamily(fontFamily);
        }

        private static void CeldaTitulo(IContainer c, string texto, string fontFamily, bool derecha)
        {
            var b = c.Background(VERDE_OSC).BorderRight(1).BorderColor("#ffffff")
                     .MinHeight(17).AlignMiddle().PaddingHorizontal(3).PaddingVertical(2);
            b = derecha ? b.AlignRight() : b.AlignLeft();
            b.Text(texto).FontSize(7f).Bold().FontColor("#fff").FontFamily(fontFamily);
        }

        private static void BandaGrupo(IContainer c, string nombre, string conteo, string fontFamily)
        {
            c.Background(VERDE_TINTE).BorderTop(1).BorderBottom(1).BorderColor(VERDE_BORDE)
             .MinHeight(15).AlignMiddle().PaddingHorizontal(3).PaddingVertical(2).Row(row =>
             {
                 row.AutoItem().AlignMiddle()
                    .Text(nombre).FontSize(8).Bold().FontColor(VERDE_OSC).FontFamily(fontFamily);
                 row.RelativeItem().PaddingLeft(8).AlignMiddle()
                    .Text(conteo).FontSize(7).Bold().FontColor(VERDE).FontFamily(fontFamily);
             });
        }

        private static void Celda(IContainer c, string fondo, string texto, string fontFamily,
                                  bool derecha, bool negrita, string color, float size)
        {
            var b = c.Background(fondo).BorderBottom(1).BorderColor(HAIR)
                     .MinHeight(ALTO_FILA).AlignMiddle().PaddingHorizontal(3).PaddingVertical(1);
            b = derecha ? b.AlignRight() : b.AlignLeft();

            var t = b.Text(texto == GUION ? GUION : texto)
                     .FontSize(size)
                     .FontColor(texto == GUION ? TENUE : color)
                     .FontFamily(fontFamily);
            if (negrita) t.Bold();
        }

        private static void CeldaMezcla(IContainer c, string fondo, double jur, double act, double pv, double ven)
        {
            var b = c.Background(fondo).BorderBottom(1).BorderColor(HAIR)
                     .MinHeight(ALTO_FILA).AlignMiddle().PaddingHorizontal(3);

            if (!MOSTRAR_BARRA_MEZCLA) { b.Text(""); return; }

            double tot = jur + act + pv + ven;
            if (tot <= 0) { b.Height(5).Background(M_VACIO).Row(r => { }); return; }

            b.Height(5).Row(r =>
            {
                if (jur > 0) r.RelativeItem((float)(jur / tot)).Background(M_JUR);
                if (act > 0) r.RelativeItem((float)(act / tot)).Background(M_ACT);
                if (pv > 0) r.RelativeItem((float)(pv / tot)).Background(M_PV);
                if (ven > 0) r.RelativeItem((float)(ven / tot)).Background(M_VEN);
            });
        }

        /// <summary>Par importe + % de un rango de antiguedad, con sombreado.</summary>
        private static void ParRango(TableDescriptor tabla, string fondo, double valor, double vencido, string heat, string fontFamily)
        {
            string f = valor != 0 ? heat : fondo;
            Celda(tabla.Cell(), f, Mon(valor), fontFamily, true, false, TINTA, TAM_NUM);
            Celda(tabla.Cell(), f, PctTxt(valor, vencido), fontFamily, true, false, SUAVE, 7.5f);
        }

        /// <summary>Renglon de subtotal (gris) o de total general (verde).</summary>
        private static void FilaResumen(TableDescriptor tabla, string etiqueta, string fontFamily,
                                        double cartera, double saldoFavor, double neto,
                                        double juridico, double activo, double porVencer, double vencido,
                                        bool esTotalGeneral)
        {
            string fondo = esTotalGeneral ? VERDE_OSC : GRIS_SUB;
            string tinta = esTotalGeneral ? BLANCO : VERDE_OSC;
            string tintaPct = esTotalGeneral ? "#cfe0c2" : VERDE;
            float size = esTotalGeneral ? 8f : TAM_NUM;

            Celda(tabla.Cell().ColumnSpan(2), fondo, etiqueta, fontFamily, false, true, tinta, size);
            Celda(tabla.Cell(), fondo, cartera.ToString("N2"), fontFamily, true, true, tinta, size);
            Celda(tabla.Cell(), fondo, saldoFavor.ToString("N2"), fontFamily, true, true, esTotalGeneral ? "#ffd9d4" : ROJO, size);
            Celda(tabla.Cell(), fondo, neto.ToString("N2"), fontFamily, true, true, tinta, size);
            Celda(tabla.Cell(), fondo, juridico.ToString("N2"), fontFamily, true, true, tinta, size);
            Celda(tabla.Cell(), fondo, Pct(juridico, cartera), fontFamily, true, true, tintaPct, 7.5f);
            Celda(tabla.Cell(), fondo, activo.ToString("N2"), fontFamily, true, true, tinta, size);
            Celda(tabla.Cell(), fondo, Pct(activo, cartera), fontFamily, true, true, tintaPct, 7.5f);
            Celda(tabla.Cell(), fondo, porVencer.ToString("N2"), fontFamily, true, true, tinta, size);
            Celda(tabla.Cell(), fondo, Pct(porVencer, cartera), fontFamily, true, true, tintaPct, 7.5f);
            Celda(tabla.Cell(), fondo, vencido.ToString("N2"), fontFamily, true, true, tinta, size);
            Celda(tabla.Cell(), fondo, Pct(vencido, cartera), fontFamily, true, true, tintaPct, 7.5f);
            CeldaMezcla(tabla.Cell(), fondo, juridico, activo, porVencer, vencido);
        }

        // ==================================================================
        //  Utilerias
        // ==================================================================

        private static double Cartera(mdlCob_TotalCartera_Detalle x) => x.totalcartera;
        private static double Neto(mdlCob_TotalCartera_Detalle x) => x.total;

        /// <summary>Importe con separador de miles; cero = guion (segun MOSTRAR_CEROS).</summary>
        private static string Mon(double v) => (v == 0 && !MOSTRAR_CEROS) ? GUION : v.ToString("N2");

        /// <summary>Porcentaje sin division entre cero; vacio = guion.</summary>
        private static string PctTxt(double parte, double total)
        {
            if (total == 0) return GUION;
            if (parte == 0 && !MOSTRAR_CEROS) return GUION;
            return (parte / total * 100).ToString(FORMATO_PCT);
        }

        private static string Pct(double parte, double total)
            => total == 0 ? "0.0" : (parte / total * 100).ToString(FORMATO_PCT);

        private class Grupo
        {
            public string Nombre { get; }
            public List<mdlCob_TotalCartera_Detalle> Filas { get; }
            public Grupo(string nombre, List<mdlCob_TotalCartera_Detalle> filas)
            {
                Nombre = nombre;
                Filas = filas;
            }
        }
    }
}
