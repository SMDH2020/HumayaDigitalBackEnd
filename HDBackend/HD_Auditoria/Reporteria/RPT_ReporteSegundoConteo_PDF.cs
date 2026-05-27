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
    public class RPT_ReporteSegundoConteo_PDF
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

                // Separar y ordenar
                var faltantes = difs.Where(d => d.tipo_diferencia == "F" ||
                                d.tipo_diferencia?.Contains("faltante", StringComparison.OrdinalIgnoreCase) == true)
                                .OrderByDescending(d => Math.Abs(d.importe_dif))
                                .ToList();

                var sobrantes = difs.Where(d => d.tipo_diferencia == "S" ||
                                d.tipo_diferencia?.Contains("sobrante", StringComparison.OrdinalIgnoreCase) == true)
                                .OrderByDescending(d => Math.Abs(d.importe_dif))
                                .ToList();

                byte[] doc = Document.Create(document =>
                {
                    document.Page(page =>
                    {
                        page.Size(PageSizes.Letter.Landscape());

                        // ── HEADER ────────────────────────────────────────────────
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
                                        .Text("FINALIZACIÓN DE INVENTARIO - " + folio)
                                        .FontColor("#fff").FontSize(20).Bold().FontFamily(font);
                                });
                            });
                        });

                        // ── CONTENIDO ─────────────────────────────────────────────
                        page.Content().PaddingTop(14).PaddingLeft(25).PaddingRight(25).Column(col =>
                        {
                            col.Item().Height(10);

                            // ── Métricas ──────────────────────────────────────────
                            col.Item().Border(0.5f).BorderColor(verde).Column(sec =>
                            {
                                sec.Item().Background(verdeOscuro).BorderBottom(1).BorderColor(amarillo)
                                    .Padding(5)
                                    .Text("MÉTRICAS DEL RESULTADO")
                                    .FontSize(8).Bold().FontFamily(font).FontColor("#fff");

                                sec.Item().Table(t =>
                                {
                                    t.ColumnsDefinition(c =>
                                    {
                                        c.RelativeColumn(1); c.RelativeColumn(1);
                                        c.RelativeColumn(1); c.RelativeColumn(1);
                                    });
                                    KpiMonto(t, "IMPORTE TOTAL INVENTARIO", info.importe_total_inventario.ToString("C2", culturaMoneda), verde, verdePanel, grisLinea, font);
                                    KpiMonto(t, "IMPORTE FALTANTE", info.importe_faltante.ToString("C2", culturaMoneda), "#c0392b", "#fff0f0", grisLinea, font);
                                    KpiMonto(t, "IMPORTE SOBRANTE", info.importe_sobrante.ToString("C2", culturaMoneda), "#1a6fa8", "#f0f5ff", grisLinea, font);
                                    KpiMonto(t, "TOTAL NETO", info.total_neto.ToString("C2", culturaMoneda), verdeOscuro, verdeClaro, grisLinea, font);
                                });

                                sec.Item().Height(1).Background(amarillo);

                                sec.Item().Table(t =>
                                {
                                    t.ColumnsDefinition(c =>
                                    {
                                        c.RelativeColumn(1); c.RelativeColumn(1);
                                        c.RelativeColumn(1); c.RelativeColumn(1);
                                        c.RelativeColumn(1);
                                    });
                                    KpiPorcentaje(t, "% FALTANTE", Math.Abs(info.porc_faltante), "#c0392b", "#fff0f0", grisLinea, font, menorEsMejor: true);
                                    KpiPorcentaje(t, "% SOBRANTE", Math.Abs(info.porc_sobrante), "#1a6fa8", "#f0f5ff", grisLinea, font, menorEsMejor: true);
                                    KpiPorcentaje(t, "% TOTAL NETO", Math.Abs(info.porc_total_neto), verdeOscuro, verdeClaro, grisLinea, font, menorEsMejor: false);
                                    KpiPorcentaje(t, "CONFIABILIDAD DE INVENTARIO", info.confiabilidad, verde, verdePanel, grisLinea, font, menorEsMejor: false);
                                    KpiPorcentaje(t, "CONFIABILIDAD DE UBICACIÓN", info.confiabilidad_ubi, verde, verdePanel, grisLinea, font, menorEsMejor: false);
                                });
                            });

                            col.Item().Height(10);

                            // ── Tabla de diferencias CON precio unitario ──────────
                            col.Item().Border(0.5f).BorderColor(verde).Column(sec =>
                            {
                                sec.Item().Background(verdeOscuro).BorderBottom(1).BorderColor(amarillo)
                                    .Padding(5)
                                    .Text($"DIFERENCIAS DETECTADAS  ({difs.Count} registro{(difs.Count != 1 ? "s" : "")})")
                                    .FontSize(8).Bold().FontFamily(font).FontColor("#fff");

                                sec.Item().Table(t =>
                                {
                                    // ← columna precio_unitario añadida
                                    t.ColumnsDefinition(c =>
                                    {
                                        c.RelativeColumn(0.8f);  // familia
                                        c.RelativeColumn(1.0f);  // sku
                                        c.RelativeColumn(2.4f);  // descripcion
                                        c.RelativeColumn(0.7f);  // posicion
                                        c.RelativeColumn(0.6f);  // existencia
                                        c.RelativeColumn(0.6f);  // conteo
                                        c.RelativeColumn(0.6f);  // diferencia
                                        c.RelativeColumn(0.8f);  // tipo
                                        c.RelativeColumn(1.0f);  // precio_unitario ← nuevo
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
                                        TH("TIPO"); TH("PRECIO UNIT."); TH("IMPORTE DIF."); // ← nuevo header
                                    });

                                    // ── Subheader + filas FALTANTES ───────────────
                                    if (faltantes.Any())
                                    {
                                        t.Cell().ColumnSpan(10) // ← 10 columnas ahora
                                            .Background("#fff0f0").BorderBottom(0.5f).BorderColor("#f09595")
                                            .PaddingVertical(4).PaddingHorizontal(8)
                                            .Text($"▼ FALTANTES — {faltantes.Count} registro{(faltantes.Count != 1 ? "s" : "")}")
                                            .FontSize(7).Bold().FontFamily(font).FontColor("#c0392b");
                                    }

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
                                        TD(d.diferencias.ToString("N2"), "#fff0f0", "#c0392b", centro: true);
                                        TD("Faltante", "#fff0f0", "#c0392b", centro: true);
                                        TD(d.precio_unitario.ToString("C2", culturaMoneda), centro: true); // ← nuevo
                                        TD(d.importe_dif.ToString("C2", culturaMoneda), centro: true);
                                        idx++;
                                    }

                                    // ── Subheader + filas SOBRANTES ───────────────
                                    if (sobrantes.Any())
                                    {
                                        t.Cell().ColumnSpan(10) // ← 10 columnas ahora
                                            .Background("#f0f5ff").BorderBottom(0.5f).BorderColor("#85b7eb")
                                            .PaddingVertical(4).PaddingHorizontal(8)
                                            .Text($"▲ SOBRANTES — {sobrantes.Count} registro{(sobrantes.Count != 1 ? "s" : "")}")
                                            .FontSize(7).Bold().FontFamily(font).FontColor("#1a6fa8");
                                    }

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
                                        TD(d.diferencias.ToString("N2"), "#f0f5ff", "#1a6fa8", centro: true);
                                        TD("Sobrante", "#f0f5ff", "#1a6fa8", centro: true);
                                        TD(d.precio_unitario.ToString("C2", culturaMoneda), centro: true); // ← nuevo
                                        TD(d.importe_dif.ToString("C2", culturaMoneda), centro: true);
                                        idx++;
                                    }
                                });
                            });

                            col.Item().Height(28);

                            // ── Firmas ────────────────────────────────────────────
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
                    nombredocumento = $"Metricas_Inventario_{folio}",
                    documento = Convert.ToBase64String(doc)
                };
            }
            catch (Exception ex) { throw ex; }
        }

        // ── Helpers — idénticos al RPT original ──────────────────────────────────
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

        private static void KpiPorcentaje(TableDescriptor t,
            string etiqueta, double valor,
            string colorBase, string fondoCelda, string gris, string font,
            bool menorEsMejor)
        {
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
                        using var pFondo = new SKPaint { Color = SKColor.Parse("#d8d8d8"), Style = SKPaintStyle.Fill, IsAntialias = true };
                        canvas.DrawRoundRect(new SKRoundRect(new SKRect(0, 0, size.Width, size.Height), 3, 3), pFondo);
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
