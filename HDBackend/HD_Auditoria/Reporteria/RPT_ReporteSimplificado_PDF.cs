using HD_Auditoria.Modelos.Programar_Inventario;
using HD_Reporteria;
using HD_Reporteria.Cobranza;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Reporteria
{
    public class RPT_ReporteSimplificado_PDF
    {
        public static RPT_Result GenerarPDF(mdl_ReporteSimplificado_View detalle, string? folio)
        {
            try
            {
                string font = "Calibri";
                string verde = "#477c2c";
                string verdeOscuro = "#275027";
                string amarillo = "#fedb05";
                string grisLinea = "#afb69d";
                string verdeClaro = "#DAE6BE";
                string verdePanel = "#eef4e8";

                var info = detalle.info;
                var difs = detalle.diferencias?.ToList() ?? new();
                var firmas = detalle.firmas;

                var culturaMoneda = new CultureInfo("es-MX");

                byte[] doc = Document.Create(document =>
                {
                    document.Page(page =>
                    {
                        // ── HORIZONTAL ───────────────────────────────────────────
                        page.Size(PageSizes.Letter.Landscape());

                        // ── HEADER (igual al de referencia) ───────────────────────
                        page.Header().Height(120).Row(row =>
                        {
                            row.RelativeItem().PaddingTop(35).Height(50).Background(verde).Row(_ => { });
                            row.ConstantColumn(0).Row(row1 =>
                            {
                                byte[] logo = File.ReadAllBytes(
                                    "C:\\Nube\\HumayaDigital\\HumayaDigitalBackEnd\\HDBackend\\HD_Reporteria\\Imagenes\\Logo.jpg");
                                row.ConstantItem(120).Image(logo);
                                row.ConstantColumn(600).PaddingTop(35).Height(50).Background(verde).Row(row2 =>
                                {
                                    row2.RelativeItem().Padding(10).PaddingLeft(10)
                                        .Text("REPORTE SIMPLIFICADO - " + folio)
                                        .FontColor("#fff").FontSize(20).Bold().FontFamily(font);
                                });
                            });
                        });

                        // ── CONTENIDO ─────────────────────────────────────────────
                        page.Content().PaddingTop(14).PaddingLeft(25).PaddingRight(25).Column(col =>
                        {
                            // ── 1. Encabezado folio / fecha ───────────────────────
                            //col.Item().Border(0.5f).BorderColor(verde).Table(t =>
                            //{
                            //    t.ColumnsDefinition(c => { c.RelativeColumn(1); c.RelativeColumn(1); });
                            //    t.Header(h =>
                            //    {
                            //        h.Cell().ColumnSpan(2)
                            //            .Background(verdeOscuro).BorderBottom(1).BorderColor(amarillo)
                            //            .Padding(5)
                            //            .Text("INFORMACIÓN DEL INVENTARIO")
                            //            .FontSize(8).Bold().FontFamily(font).FontColor("#fff");
                            //    });
                            //    t.Cell().Background(verdePanel).Padding(7).Column(c =>
                            //    {
                            //        c.Item().Text("FOLIO").FontSize(6).Bold().FontFamily(font).FontColor(verdeOscuro);
                            //        c.Item().Text(folio ?? "-").FontSize(14).Bold().FontFamily(font).FontColor(verde);
                            //    });
                            //});

                            col.Item().Height(10);

                            // ── 2. Métricas ───────────────────────────────────────
                            col.Item().Border(0.5f).BorderColor(verde).Column(sec =>
                            {
                                sec.Item().Background(verdeOscuro).BorderBottom(1).BorderColor(amarillo)
                                    .Padding(5)
                                    .Text("MÉTRICAS DEL RESULTADO")
                                    .FontSize(8).Bold().FontFamily(font).FontColor("#fff");

                                // Fila 1 — importes (4 tarjetas)
                                sec.Item().Table(t =>
                                {
                                    t.ColumnsDefinition(c =>
                                    {
                                        c.RelativeColumn(1); c.RelativeColumn(1);
                                        c.RelativeColumn(1); c.RelativeColumn(1);
                                    });
                                    KpiMonto(t, "IMPORTE TOTAL INVENTARIO", info.importe_total_inventario.ToString("C2", culturaMoneda), verde, verdePanel, grisLinea, font);
                                    KpiMonto(t, "IMPORTE FALTANTE", info.importe_faltante.ToString("C2", culturaMoneda), "#c0392b", "#fff0f0", grisLinea, font);
                                    KpiMonto(t, "IMPORTE SOBRANTE", info.importe_sobrante.ToString("C2", culturaMoneda) ?? "—", "#1a6fa8", "#f0f5ff", grisLinea, font);
                                    KpiMonto(t, "TOTAL NETO", info.total_neto.ToString("C2", culturaMoneda), verdeOscuro, verdeClaro, grisLinea, font);
                                });

                                // Divisor
                                sec.Item().Height(1).Background(amarillo);

                                // Fila 2 — porcentajes y confiabilidad (5 KPIs con barra)
                                sec.Item().Table(t =>
                                {
                                    t.ColumnsDefinition(c =>
                                    {
                                        c.RelativeColumn(1); c.RelativeColumn(1);
                                        c.RelativeColumn(1); c.RelativeColumn(1);
                                        c.RelativeColumn(1); // ← quinta columna
                                    });
                                    KpiPorcentaje(t, "% FALTANTE", Math.Abs(info.porc_faltante), "#c0392b", "#fff0f0", grisLinea, font, menorEsMejor: true);
                                    KpiPorcentaje(t, "% SOBRANTE", Math.Abs(info.porc_sobrante), "#1a6fa8", "#f0f5ff", grisLinea, font, menorEsMejor: true);
                                    KpiPorcentaje(t, "% TOTAL NETO", Math.Abs(info.porc_total_neto), verdeOscuro, verdeClaro, grisLinea, font, menorEsMejor: false);
                                    KpiPorcentaje(t, "CONFIABILIDAD DE INVENTARIO", info.confiabilidad, verde, verdePanel, grisLinea, font, menorEsMejor: false);
                                    KpiPorcentaje(t, "CONFIABILIDAD DE UBICACIÓN", info.confiabilidad_ubi, verde, verdePanel, grisLinea, font, menorEsMejor: false);
                                });
                            });

                            col.Item().Height(10);

                            // ── 3. Tabla de diferencias ───────────────────────────
                            col.Item().Border(0.5f).BorderColor(verde).Column(sec =>
                            {
                                sec.Item().Background(verdeOscuro).BorderBottom(1).BorderColor(amarillo)
                                    .Padding(5)
                                    .Text($"DIFERENCIAS DETECTADAS  ({difs.Count} registro{(difs.Count != 1 ? "s" : "")})")
                                    .FontSize(8).Bold().FontFamily(font).FontColor("#fff");

                                // Separar y ordenar
                                var faltantes = difs.Where(d => d.tipo_diferencia == "F" ||
                                                d.tipo_diferencia?.Contains("faltante", StringComparison.OrdinalIgnoreCase) == true)
                                                .OrderByDescending(d => Math.Abs(d.importe_dif))
                                                .ToList();

                                var sobrantes = difs.Where(d => d.tipo_diferencia == "S" ||
                                                d.tipo_diferencia?.Contains("sobrante", StringComparison.OrdinalIgnoreCase) == true)
                                                .OrderByDescending(d => Math.Abs(d.importe_dif))
                                                .ToList();

                                sec.Item().Table(t =>
                                {
                                    t.ColumnsDefinition(c =>
                                    {
                                        c.RelativeColumn(0.9f);  // familia
                                        c.RelativeColumn(1.1f);  // sku
                                        c.RelativeColumn(2.8f);  // descripcion
                                        c.RelativeColumn(0.8f);  // posicion
                                        c.RelativeColumn(0.65f); // existencia
                                        c.RelativeColumn(0.65f); // conteo
                                        c.RelativeColumn(0.65f); // diferencia
                                        c.RelativeColumn(0.9f);  // tipo
                                        c.RelativeColumn(1.0f);  // importe
                                    });

                                    t.Header(h =>
                                    {
                                        void TH(string txt) => h.Cell()
                                            .Background(verde).BorderBottom(0.5f).BorderColor(amarillo)
                                            .Padding(3).AlignCenter()
                                            .Text(txt).FontSize(6.5f).Bold().FontFamily(font).FontColor("#fff");

                                        TH("FAMILIA"); TH("SKU"); TH("DESCRIPCIÓN"); TH("POSICIÓN");
                                        TH("EXISTENCIA"); TH("CONTEO"); TH("DIFERENCIA");
                                        TH("TIPO"); TH("IMPORTE DIF.");
                                    });

                                    // ── Subheader FALTANTES ──
                                    if (faltantes.Any())
                                    {
                                        t.Cell().ColumnSpan(9)
                                            .Background("#fff0f0").BorderBottom(0.5f).BorderColor("#f09595")
                                            .PaddingVertical(4).PaddingHorizontal(8)
                                            .Text($"FALTANTES — {faltantes.Count} registro{(faltantes.Count != 1 ? "s" : "")}")
                                            .FontSize(7).Bold().FontFamily(font).FontColor("#c0392b");
                                    }

                                    // Filas faltantes
                                    int idx = 0;
                                    foreach (var d in faltantes)
                                    {
                                        string bg = idx % 2 == 0 ? "#ffffff" : "#fff7f7";

                                        void TD(string? txt, string? fondo = null, string? color = null, bool centro = false) =>
                                            t.Cell().Background(fondo ?? bg).BorderBottom(0.5f).BorderColor(grisLinea)
                                                .PaddingVertical(3).PaddingHorizontal(4)
                                                .Element(e => centro ? e.AlignCenter() : e.AlignLeft())
                                                .Text(txt ?? "").FontSize(6.5f).FontFamily(font).FontColor(color ?? "#333");

                                        TD(d.familia);
                                        TD(d.sku);
                                        TD(d.descripcion);
                                        TD(d.posicion, centro: true);
                                        TD(d.existencia.ToString("N2"), centro: true);
                                        TD(d.conteo.ToString("N2"), centro: true);
                                        TD(d.diferencias.ToString("N2"), fondo: "#fff0f0", color: "#c0392b", centro: true);
                                        TD("Faltante", fondo: "#fff0f0", color: "#c0392b", centro: true);
                                        TD(d.importe_dif.ToString("C2", culturaMoneda), centro: true);
                                        idx++;
                                    }

                                    // ── Subheader SOBRANTES ──
                                    if (sobrantes.Any())
                                    {
                                        t.Cell().ColumnSpan(9)
                                            .Background("#f0f5ff").BorderBottom(0.5f).BorderColor("#85b7eb")
                                            .PaddingVertical(4).PaddingHorizontal(8)
                                            .Text($"SOBRANTES — {sobrantes.Count} registro{(sobrantes.Count != 1 ? "s" : "")}")
                                            .FontSize(7).Bold().FontFamily(font).FontColor("#1a6fa8");
                                    }

                                    // Filas sobrantes
                                    idx = 0;
                                    foreach (var d in sobrantes)
                                    {
                                        string bg = idx % 2 == 0 ? "#ffffff" : "#f5f8ff";

                                        void TD(string? txt, string? fondo = null, string? color = null, bool centro = false) =>
                                            t.Cell().Background(fondo ?? bg).BorderBottom(0.5f).BorderColor(grisLinea)
                                                .PaddingVertical(3).PaddingHorizontal(4)
                                                .Element(e => centro ? e.AlignCenter() : e.AlignLeft())
                                                .Text(txt ?? "").FontSize(6.5f).FontFamily(font).FontColor(color ?? "#333");

                                        TD(d.familia);
                                        TD(d.sku);
                                        TD(d.descripcion);
                                        TD(d.posicion, centro: true);
                                        TD(d.existencia.ToString("N2"), centro: true);
                                        TD(d.conteo.ToString("N2"), centro: true);
                                        TD(d.diferencias.ToString("N2"), fondo: "#f0f5ff", color: "#1a6fa8", centro: true);
                                        TD("Sobrante", fondo: "#f0f5ff", color: "#1a6fa8", centro: true);
                                        TD(d.importe_dif.ToString("C2", culturaMoneda), centro: true);
                                        idx++;
                                    }

                                    //int idx = 0;
                                    //foreach (var d in difs)
                                    //{
                                    //    string bg = idx % 2 == 0 ? "#ffffff" : "#f5f5f5";

                                    //    bool esFaltante = d.tipo_diferencia?.Contains("faltante", StringComparison.OrdinalIgnoreCase) == true;
                                    //    bool esSobrante = d.tipo_diferencia?.Contains("sobrante", StringComparison.OrdinalIgnoreCase) == true;
                                    //    string bgTipo = esFaltante ? "#fff0f0" : esSobrante ? "#f0f5ff" : bg;
                                    //    string colorTipo = esFaltante ? "#c0392b" : esSobrante ? "#1a6fa8" : "#333";

                                    //    void TD(string? txt, string? fondo = null, string? color = null, bool centro = false) =>
                                    //        t.Cell().Background(fondo ?? bg).BorderBottom(0.5f).BorderColor(grisLinea)
                                    //            .PaddingVertical(3).PaddingHorizontal(4)
                                    //            .Element(e => centro ? e.AlignCenter() : e.AlignLeft())
                                    //            .Text(txt ?? "").FontSize(6.5f).FontFamily(font)
                                    //            .FontColor(color ?? "#333");

                                    //    TD(d.familia);
                                    //    TD(d.sku);
                                    //    TD(d.descripcion);
                                    //    TD(d.posicion, centro: true);
                                    //    TD(d.existencia.ToString("N2"), centro: true);
                                    //    TD(d.conteo.ToString("N2"), centro: true);
                                    //    TD(d.diferencias.ToString("N2"), fondo: bgTipo, color: colorTipo, centro: true);
                                    //    TD(d.tipo_diferencia == "S" ? "Sobrante" : "Faltante", fondo: bgTipo, color: colorTipo, centro: true);
                                    //    TD(d.importe_dif.ToString("C2", culturaMoneda), centro: true);

                                    //    idx++;
                                    //}
                                });
                            });

                            col.Item().Height(28);

                            // ── 4. Firmas ─────────────────────────────────────────
                            col.Item().Row(r =>
                            {
                                r.RelativeItem().Column(c =>
                                {
                                    c.Item().Height(36);
                                    c.Item().BorderTop(0.8f).BorderColor("#444").PaddingTop(5)
                                        .AlignCenter()
                                        .Text("ENCARGADO DE ALMACÉN")
                                        .FontSize(7.5f).Bold().FontFamily(font).FontColor(verdeOscuro);
                                    c.Item().AlignCenter()
                                        .Text(firmas?.encargado_almacen?.ToUpper() ?? "")
                                        .FontSize(7).FontFamily(font).FontColor("#333");
                                });

                                r.ConstantItem(80);

                                r.RelativeItem().Column(c =>
                                {
                                    c.Item().Height(36);
                                    c.Item().BorderTop(0.8f).BorderColor("#444").PaddingTop(5)
                                        .AlignCenter()
                                        .Text("AUDITOR")
                                        .FontSize(7.5f).Bold().FontFamily(font).FontColor(verdeOscuro);
                                    c.Item().AlignCenter()
                                        .Text(firmas?.auditor?.ToUpper() ?? "")
                                        .FontSize(7).FontFamily(font).FontColor("#333");
                                });
                            });
                        });

                        // ── FOOTER ────────────────────────────────────────────────
                        page.Footer().Height(30).PaddingHorizontal(25).Row(row =>
                        {
                            row.RelativeItem().AlignLeft().PaddingTop(8)
                                .Text($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}")
                                .FontSize(7).FontFamily(font).FontColor("#aaa");

                            row.RelativeItem().AlignRight().PaddingTop(8).Text(txt =>
                            {
                                txt.Span("Pág. ").FontSize(8).FontFamily("arial");
                                txt.CurrentPageNumber().FontSize(8).Bold().FontFamily("arial");
                                txt.Span(" de ").FontSize(8).FontFamily("arial");
                                txt.TotalPages().FontSize(8).Bold().FontFamily("arial");
                            });
                        });
                    });
                }).GeneratePdf();

                return new RPT_Result
                {
                    extension = "pdf",
                    nombredocumento = $"Reporte_Simplificado_{folio}",
                    documento = Convert.ToBase64String(doc)
                };
            }
            catch (Exception ex) { throw ex; }
        }

        // ── Helpers ──────────────────────────────────────────────────────────────

        /// Tarjeta de valor monetario/texto
        private static void KpiMonto(TableDescriptor t,
            string etiqueta, string valor,
            string colorValor, string fondoCelda, string gris, string font)
        {
            t.Cell()
                .Background(fondoCelda)
                .BorderRight(0.5f).BorderColor(gris)
                .BorderBottom(0.5f).BorderColor(gris)
                .Padding(10).Column(c =>
                {
                    c.Item().Text(etiqueta)
                        .FontSize(6.5f).Bold().FontFamily(font).FontColor("#555");
                    c.Item().PaddingTop(4).Text(valor)
                        .FontSize(14).Bold().FontFamily(font).FontColor(colorValor);
                });
        }

        /// Tarjeta de porcentaje con barra Skia
        private static void KpiPorcentaje(TableDescriptor t,
            string etiqueta, double valor,
            string colorBase, string fondoCelda, string gris, string font,
            bool menorEsMejor)
        {
            // Para "menor es mejor" (faltante/sobrante): verde si < 5%, amarillo si < 15%, rojo si >= 15%
            // Para "mayor es mejor" (confiabilidad): verde si >= 95%, amarillo si >= 80%, rojo si < 80%
            string colorValor;
            if (menorEsMejor)
                colorValor = valor < 5 ? "#275027" : valor < 15 ? "#b8860b" : "#c0392b";
            else
                colorValor = valor >= 95 ? "#275027" : valor >= 80 ? "#b8860b" : "#c0392b";

            float pct = (float)Math.Min(Math.Max(valor / 100.0, 0.0), 1.0);

            t.Cell()
                .Background(fondoCelda)
                .BorderRight(0.5f).BorderColor(gris)
                .BorderBottom(0.5f).BorderColor(gris)
                .Padding(8).Column(c =>
                {
                    c.Item().Text(etiqueta).FontSize(6.5f).Bold().FontFamily(font).FontColor("#555");

                    c.Item().PaddingTop(3).Text($"{valor:N1}%")
                        .FontSize(16).Bold().FontFamily(font).FontColor(colorValor);

                    c.Item().PaddingTop(4).Height(7).SkiaSharpCanvas((canvas, size) =>
                    {
                        // Fondo
                        using var pFondo = new SKPaint { Color = SKColor.Parse("#d8d8d8"), Style = SKPaintStyle.Fill, IsAntialias = true };
                        canvas.DrawRoundRect(new SKRoundRect(new SKRect(0, 0, size.Width, size.Height), 3, 3), pFondo);
                        // Relleno
                        float w = size.Width * pct;
                        if (w > 0)
                        {
                            using var pRelleno = new SKPaint { Color = SKColor.Parse(colorValor), Style = SKPaintStyle.Fill, IsAntialias = true };
                            canvas.DrawRoundRect(new SKRoundRect(new SKRect(0, 0, w, size.Height), 3, 3), pRelleno);
                        }
                    });

                    c.Item().PaddingTop(2).Text(
                        menorEsMejor
                            ? (valor < 5 ? "ÓPTIMO" : valor < 15 ? "ACEPTABLE" : "CRÍTICO")
                            : (valor >= 95 ? "EXCELENTE" : valor >= 80 ? "ACEPTABLE" : "REQUIERE ATENCIÓN"))
                        .FontSize(6).FontFamily(font).FontColor(colorValor);
                });
        }
    }
}
