using HD_Cobranza.Modelos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace HD_Reporteria.Cobranza
{
    /// <summary>
    /// Version ejecutiva del detalle de cartera de un cliente.
    /// Archivo aparte: no sustituye a RPT_TotalCartera_Detalle_Cliente.
    /// </summary>
    public class RPT_TotalCartera_Detalle_ClienteV2
    {
        // ==================================================================
        //  PARAMETROS RAPIDOS  (cambiar aqui, sin tocar el resto del codigo)
        // ==================================================================

        /// <summary>Agrupa los documentos por sucursal con banda y subtotal.
        /// Solo se nota cuando el cliente tiene mas de una sucursal.</summary>
        public static readonly bool AGRUPAR_POR_SUCURSAL = true;

        /// <summary>Colorea la celda de DIAS segun la antiguedad del vencimiento.</summary>
        public static readonly bool MOSTRAR_SEMAFORO_DIAS = true;

        /// <summary>Muestra las columnas LINEA y ESTATUS. En false desaparecen.</summary>
        public static readonly bool MOSTRAR_LINEA_Y_ESTATUS = true;

        /// <summary>false = los ceros se imprimen como guion tenue.</summary>
        public static readonly bool MOSTRAR_CEROS = false;

        /// <summary>Quita las comillas con las que llega el numero de documento.</summary>
        public static readonly bool LIMPIAR_COMILLAS_DOCUMENTO = true;

        /// <summary>Formato de los porcentajes: "N1" o "N2".</summary>
        public const string FORMATO_PCT = "N1";

        /// <summary>Alto minimo del renglon de detalle (antes era 20 fijo).</summary>
        public const float ALTO_FILA = 16f;

        /// <summary>Tamano de letra de los importes.</summary>
        private const float TAM_NUM = 8f;

        /// <summary>Aire a cada lado de la linea separadora de la banda de contexto.</summary>
        private const float SEPARACION_CTX = 6f;

        /// <summary>Aire arriba de los subtotales y de las bandas de sucursal.</summary>
        private const float ESPACIO_GRUPO = 9f;

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

        private const string HEAT1 = "#f6f8f2";
        private const string HEAT2 = "#eaf0e0";
        private const string HEAT3 = "#f6e9ce";
        private const string HEAT4 = "#efd2a8";
        private const string HEAT5 = "#e4b478";

        private const string RUTA_LOGO = "C:\\Nube\\HumayaDigital\\HumayaDigitalBackEnd\\HDBackend\\HD_Reporteria\\Imagenes\\Logo.jpg";

        // ==================================================================

        public static RPT_Result Generar(IEnumerable<mdlResumenCartera_Clientes> resumen)
        {
            try
            {
                // Si el acomodo no se resuelve, que truene rapido en lugar de
                // quedarse generando hojas para siempre.
                QuestPDF.Settings.DocumentLayoutExceptionThreshold = 100;

                string fontFamily = "Calibri";

                // Se descarta el renglon de totales que pueda venir del origen:
                // los totales se recalculan aqui.
                var lista = (resumen ?? Enumerable.Empty<mdlResumenCartera_Clientes>())
                    .Where(x => !EsRenglonTotal(x))
                    .ToList();

                var grupos = AGRUPAR_POR_SUCURSAL
                    ? lista.GroupBy(x => string.IsNullOrWhiteSpace(x.sucursal) ? "SIN SUCURSAL" : x.sucursal!.Trim())
                           .Select(g => new Grupo(g.Key, g.ToList())).ToList()
                    : new List<Grupo> { new Grupo("", lista) };

                bool conBandas = AGRUPAR_POR_SUCURSAL && grupos.Count > 1;

                double gSaldo = lista.Sum(x => x.saldo);
                double gIntereses = lista.Sum(x => x.interesbase);
                double gTotal = lista.Sum(x => x.importe);
                double gVencido = lista.Where(x => x.diasvencido > 0).Sum(x => x.importe);
                double gPorVencer = lista.Where(x => x.diasvencido <= 0).Sum(x => x.importe);

                // El codigo de colores del pie solo tiene sentido si alguna
                // celda de DIAS quedo pintada.
                bool hayVencidos = lista.Any(x => x.diasvencido > 0);

                var primero = lista.FirstOrDefault();
                string idCliente = primero?.idcliente ?? "";
                string razonSocial = primero?.razonsocial ?? "";
                string encabezadoCliente = string.IsNullOrWhiteSpace(razonSocial)
                    ? (string.IsNullOrWhiteSpace(idCliente) ? "CLIENTE" : idCliente)
                    : (string.IsNullOrWhiteSpace(idCliente) ? razonSocial : idCliente + "  ·  " + razonSocial);

                string fechaCorte = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();

                byte[] doc = Document.Create(document =>
                {
                    document.Page(page =>
                    {
                        page.Size(PageSizes.A4.Landscape());

                        page.Header().Element(c => Encabezado(c, "RESUMEN CARTERA DETALLE", fontFamily));

                        page.Content().PaddingTop(6).PaddingLeft(30).PaddingRight(30).Column(col1 =>
                        {
                            // ---- nombre del cliente ----
                            col1.Item().PaddingBottom(5)
                                .Text(encabezadoCliente).FontSize(13).Bold()
                                .FontColor(VERDE_OSC).FontFamily(fontFamily);

                            // ---- banda de contexto ----
                            col1.Item().Element(c => BandaContexto(c, fontFamily, new[]
                            {
                                ("CORTE AL", fechaCorte, "", false),
                                ("DOCUMENTOS", lista.Count.ToString("N0"), "", false),
                                ("IMPORTE", gSaldo.ToString("N2"), "", false),
                                ("INTERESES", gIntereses.ToString("N2"), "", false),
                                ("TOTAL", gTotal.ToString("N2"), "", true),
                                ("POR VENCER", gPorVencer.ToString("N2"), Pct(gPorVencer, gTotal), false),
                                ("VENCIDO", gVencido.ToString("N2"), Pct(gVencido, gTotal), true)
                            }));

                            col1.Item().PaddingTop(8).Table(tabla =>
                            {
                                tabla.ColumnsDefinition(c =>
                                {
                                    c.RelativeColumn(1.2f);                                  // sucursal
                                    if (MOSTRAR_LINEA_Y_ESTATUS) c.RelativeColumn(1.0f);     // linea
                                    if (MOSTRAR_LINEA_Y_ESTATUS) c.RelativeColumn(1.0f);     // estatus
                                    c.RelativeColumn(1.2f);                                  // documento
                                    c.RelativeColumn(1.0f);                                  // vencimiento
                                    c.RelativeColumn(0.5f);                                  // dias
                                    c.RelativeColumn(1.3f);                                  // importe
                                    c.RelativeColumn(1.1f);                                  // intereses
                                    c.RelativeColumn(1.3f);                                  // total
                                });

                                tabla.Header(header =>
                                {
                                    // --- primer nivel ---
                                    CeldaGrupo(header.Cell().ColumnSpan(MOSTRAR_LINEA_Y_ESTATUS ? 3u : 1u), "CLASIFICACION", fontFamily);
                                    CeldaGrupo(header.Cell().ColumnSpan(3), "DOCUMENTO", fontFamily);
                                    CeldaGrupo(header.Cell().ColumnSpan(3), "IMPORTES", fontFamily);

                                    // --- segundo nivel ---
                                    CeldaTitulo(header.Cell(), "SUCURSAL", fontFamily, false);
                                    if (MOSTRAR_LINEA_Y_ESTATUS) CeldaTitulo(header.Cell(), "LINEA", fontFamily, false);
                                    if (MOSTRAR_LINEA_Y_ESTATUS) CeldaTitulo(header.Cell(), "ESTATUS", fontFamily, false);
                                    CeldaTitulo(header.Cell(), "FOLIO", fontFamily, false);
                                    CeldaTitulo(header.Cell(), "VENCIMIENTO", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "DIAS", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "IMPORTE", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "INTERESES", fontFamily, true);
                                    CeldaTitulo(header.Cell(), "TOTAL", fontFamily, true);
                                });

                                bool primerGrupo = true;
                                foreach (var grupo in grupos)
                                {
                                    if (conBandas && !string.IsNullOrEmpty(grupo.Nombre))
                                    {
                                        BandaGrupo(tabla.Cell().ColumnSpan(ColumnasDetalle),
                                                   "SUCURSAL  ·  " + grupo.Nombre.ToUpper(),
                                                   grupo.Filas.Count + (grupo.Filas.Count == 1 ? " documento" : " documentos"),
                                                   fontFamily, !primerGrupo);
                                    }
                                    primerGrupo = false;

                                    int i = 0;
                                    foreach (var mdl in grupo.Filas)
                                    {
                                        string fondo = (i++ % 2 == 1) ? ZEBRA : BLANCO;
                                        bool vencido = mdl.diasvencido > 0;
                                        string fondoDias = MOSTRAR_SEMAFORO_DIAS ? ColorDias(mdl.diasvencido, fondo) : fondo;

                                        Celda(tabla.Cell(), fondo, mdl.sucursal ?? "", fontFamily, false, false, TINTA, 8f);
                                        if (MOSTRAR_LINEA_Y_ESTATUS)
                                            Celda(tabla.Cell(), fondo, mdl.linea ?? "", fontFamily, false, false, SUAVE, 7.5f);
                                        if (MOSTRAR_LINEA_Y_ESTATUS)
                                            Celda(tabla.Cell(), fondo, mdl.estatus ?? "", fontFamily, false, false, SUAVE, 7.5f);

                                        Celda(tabla.Cell(), fondo, Folio(mdl.documento), fontFamily, false, false, TINTA, 8f);
                                        Celda(tabla.Cell(), fondo, mdl.vencimiento ?? "", fontFamily, true, false, vencido ? ROJO : TINTA, 8f);
                                        Celda(tabla.Cell(), fondoDias, vencido ? mdl.diasvencido.ToString("N0") : GUION,
                                              fontFamily, true, vencido, vencido ? TINTA : TENUE, 8f);

                                        Celda(tabla.Cell(), fondo, Mon(mdl.saldo), fontFamily, true, false, TINTA, TAM_NUM);
                                        Celda(tabla.Cell(), fondo, Mon(mdl.interesbase), fontFamily, true, false, TINTA, TAM_NUM);
                                        Celda(tabla.Cell(), fondo, Mon(mdl.importe), fontFamily, true, false, TINTA, TAM_NUM);
                                    }

                                    if (conBandas && !string.IsNullOrEmpty(grupo.Nombre))
                                        FilaResumen(tabla, "SUBTOTAL " + grupo.Nombre.ToUpper(), fontFamily,
                                            grupo.Filas.Sum(x => x.saldo),
                                            grupo.Filas.Sum(x => x.interesbase),
                                            grupo.Filas.Sum(x => x.importe),
                                            false);
                                }

                                FilaResumen(tabla, "TOTAL  ·  " + lista.Count.ToString("N0") + (lista.Count == 1 ? " documento" : " documentos"),
                                            fontFamily, gSaldo, gIntereses, gTotal, true);
                            });
                        });

                        page.Footer().Element(c => PieDePagina(c, fontFamily, hayVencidos));
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

        /// <summary>Franja delgada con los totales del cliente.</summary>
        private static void BandaContexto(IContainer container, string fontFamily, (string k, string v, string pct, bool destacado)[] datos)
        {
            container.BorderTop(2).BorderBottom(1).BorderColor(HAIR).PaddingVertical(6).Row(row =>
            {
                for (int i = 0; i < datos.Length; i++)
                {
                    var d = datos[i];
                    bool ultimo = i == datos.Length - 1;
                    bool primero = i == 0;
                    string pct = string.IsNullOrEmpty(d.pct) ? "" : d.pct + "%";

                    // Cada dato ocupa un pedazo del renglon proporcional a lo que
                    // mide su texto: asi la franja se reparte completa de orilla a
                    // orilla y el porcentaje cabe junto al importe, no debajo.
                    float anchoTexto = Math.Max(d.k.Length * 4.0f,
                                                d.v.Length * 4.4f + (pct.Length > 0 ? (pct.Length + 1) * 3.8f : 0f));
                    float peso = anchoTexto + (primero || ultimo ? SEPARACION_CTX : SEPARACION_CTX * 2);

                    // El borde va primero y el padding despues: asi la linea
                    // separadora queda por fuera del texto y no pegada a el.
                    row.RelativeItem(peso)
                       .BorderRight(ultimo ? 0 : 1).BorderColor(HAIR)
                       .PaddingLeft(primero ? 0 : SEPARACION_CTX)
                       .PaddingRight(ultimo ? 0 : SEPARACION_CTX)
                       .Column(c =>
                       {
                           c.Item().AlignLeft().Text(d.k).FontSize(6.5f).FontColor(SUAVE).Bold().FontFamily(fontFamily);
                           c.Item().AlignLeft().Text(t =>
                           {
                               t.DefaultTextStyle(x => x.FontFamily(fontFamily).Bold());
                               t.Span(d.v).FontSize(d.destacado ? 9f : 8.5f)
                                          .FontColor(d.destacado ? VERDE_OSC : TINTA);
                               if (pct.Length > 0)
                                   t.Span("  " + pct).FontSize(7f).FontColor(VERDE);
                           });
                       });
                }
            });
        }

        private static void PieDePagina(IContainer container, string fontFamily, bool conLeyenda)
        {
            container.PaddingLeft(30).PaddingRight(30).PaddingBottom(10)
                .BorderTop(1).BorderColor(HAIR).PaddingTop(4).Row(row =>
                {
                    row.RelativeItem().AlignLeft().AlignMiddle()
                       .Text("Maquinaria del Humaya  ·  Cobranza  ·  Cartera detalle por cliente")
                       .FontSize(7).FontColor(SUAVE).FontFamily(fontFamily);

                    if (MOSTRAR_SEMAFORO_DIAS && conLeyenda)
                    {
                        row.AutoItem().AlignMiddle().Row(r =>
                        {
                            Leyenda(r, HEAT1, "1 a 15", fontFamily);
                            Leyenda(r, HEAT2, "16 a 30", fontFamily);
                            Leyenda(r, HEAT3, "31 a 60", fontFamily);
                            Leyenda(r, HEAT4, "61 a 90", fontFamily);
                            Leyenda(r, HEAT5, "mas de 90", fontFamily);
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

        /// <summary>Columnas de la tabla (cambia con LINEA y ESTATUS).</summary>
        private static uint ColumnasDetalle => MOSTRAR_LINEA_Y_ESTATUS ? 9u : 7u;

        private static void CeldaGrupo(IContainer c, string texto, string fontFamily)
        {
            c.Background(VERDE_PROF).BorderRight(1).BorderColor("#ffffff")
             .MinHeight(14).AlignMiddle().AlignCenter().PaddingVertical(2)
             .Text(texto).FontSize(6.5f).Bold().FontColor("#fff").FontFamily(fontFamily);
        }

        private static void CeldaTitulo(IContainer c, string texto, string fontFamily, bool derecha)
        {
            var b = c.Background(VERDE_OSC).BorderRight(1).BorderColor("#ffffff")
                     .MinHeight(17).AlignMiddle().PaddingHorizontal(4).PaddingVertical(2);
            b = derecha ? b.AlignRight() : b.AlignLeft();
            b.Text(texto).FontSize(7f).Bold().FontColor("#fff").FontFamily(fontFamily);
        }

        private static void BandaGrupo(IContainer c, string nombre, string conteo, string fontFamily, bool conAire)
        {
            (conAire ? c.PaddingTop(ESPACIO_GRUPO) : c)
             .Background(VERDE_TINTE).BorderTop(1).BorderBottom(1).BorderColor(VERDE_BORDE)
             .MinHeight(15).AlignMiddle().PaddingHorizontal(4).PaddingVertical(2).Row(row =>
             {
                 row.AutoItem().AlignMiddle()
                    .Text(nombre).FontSize(8).Bold().FontColor(VERDE_OSC).FontFamily(fontFamily);
                 row.RelativeItem().PaddingLeft(8).AlignMiddle()
                    .Text(conteo).FontSize(7).Bold().FontColor(VERDE).FontFamily(fontFamily);
             });
        }

        private static void Celda(IContainer c, string fondo, string texto, string fontFamily,
                                  bool derecha, bool negrita, string color, float size, float aireArriba = 0f)
        {
            var b = (aireArriba > 0 ? c.PaddingTop(aireArriba) : c)
                     .Background(fondo).BorderBottom(1).BorderColor(HAIR)
                     .MinHeight(ALTO_FILA).AlignMiddle().PaddingHorizontal(4).PaddingVertical(1);
            b = derecha ? b.AlignRight() : b.AlignLeft();

            var t = b.Text(texto)
                     .FontSize(size)
                     .FontColor(texto == GUION ? TENUE : color)
                     .FontFamily(fontFamily);
            if (negrita) t.Bold();
        }

        /// <summary>Renglon de subtotal (gris) o de total (verde).</summary>
        private static void FilaResumen(TableDescriptor tabla, string etiqueta, string fontFamily,
                                        double saldo, double intereses, double total, bool esTotalGeneral)
        {
            string fondo = esTotalGeneral ? VERDE_OSC : GRIS_SUB;
            string tinta = esTotalGeneral ? BLANCO : VERDE_OSC;
            float size = esTotalGeneral ? 8.5f : 8f;
            float aire = ESPACIO_GRUPO;

            uint columnasEtiqueta = MOSTRAR_LINEA_Y_ESTATUS ? 6u : 4u;

            Celda(tabla.Cell().ColumnSpan(columnasEtiqueta), fondo, etiqueta, fontFamily, false, true, tinta, size, aire);
            Celda(tabla.Cell(), fondo, saldo.ToString("N2"), fontFamily, true, true, tinta, size, aire);
            Celda(tabla.Cell(), fondo, intereses.ToString("N2"), fontFamily, true, true, tinta, size, aire);
            Celda(tabla.Cell(), fondo, total.ToString("N2"), fontFamily, true, true, tinta, size, aire);
        }

        // ==================================================================
        //  Utilerias
        // ==================================================================

        /// <summary>Color de fondo de la celda DIAS segun la antiguedad.</summary>
        private static string ColorDias(int dias, string fondoBase)
        {
            if (dias <= 0) return fondoBase;
            if (dias <= 15) return HEAT1;
            if (dias <= 30) return HEAT2;
            if (dias <= 60) return HEAT3;
            if (dias <= 90) return HEAT4;
            return HEAT5;
        }

        /// <summary>El folio llega entre comillas desde el origen de datos.</summary>
        private static string Folio(string? documento)
        {
            string d = (documento ?? "").Trim();
            return LIMPIAR_COMILLAS_DOCUMENTO ? d.Trim('"').Trim() : d;
        }

        /// <summary>Detecta el renglon de totales que pueda venir del origen.</summary>
        private static bool EsRenglonTotal(mdlResumenCartera_Clientes x)
        {
            return Igual(x.sucursal, "TOTAL") || Igual(x.razonsocial, "TOTAL") || Igual(x.documento, "TOTAL");
        }

        private static bool Igual(string? valor, string texto)
            => string.Equals((valor ?? "").Trim().Trim('"'), texto, StringComparison.OrdinalIgnoreCase);

        /// <summary>Importe con separador de miles; cero = guion (segun MOSTRAR_CEROS).</summary>
        private static string Mon(double v) => (v == 0 && !MOSTRAR_CEROS) ? GUION : v.ToString("N2");

        private static string Pct(double parte, double total)
            => total == 0 ? "0.0" : (parte / total * 100).ToString(FORMATO_PCT);

        private class Grupo
        {
            public string Nombre { get; }
            public List<mdlResumenCartera_Clientes> Filas { get; }
            public Grupo(string nombre, List<mdlResumenCartera_Clientes> filas)
            {
                Nombre = nombre;
                Filas = filas;
            }
        }
    }
}
