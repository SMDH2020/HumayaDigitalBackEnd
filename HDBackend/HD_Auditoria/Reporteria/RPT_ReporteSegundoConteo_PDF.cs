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

                // ── Separar y ordenar ─────────────────────────────────────────
                var faltantes = difs
                    .Where(d => d.tipo_diferencia == "F" ||
                                d.tipo_diferencia?.Contains("faltante", StringComparison.OrdinalIgnoreCase) == true)
                    .OrderByDescending(d => Math.Abs(d.importe_dif))
                    .ToList();

                var sobrantes = difs
                    .Where(d => d.tipo_diferencia == "S" ||
                                d.tipo_diferencia?.Contains("sobrante", StringComparison.OrdinalIgnoreCase) == true)
                    .OrderByDescending(d => Math.Abs(d.importe_dif))
                    .ToList();

                var correctos = difs
                    .Where(d => d.tipo_diferencia == "C")
                    .OrderBy(d => d.descripcion)
                    .ToList();

                byte[] doc = Document.Create(document =>
                {
                    document.Page(page =>
                    {
                        // ── VERTICAL ─────────────────────────────────────────
                        page.Size(PageSizes.Letter);

                        // ── HEADER ────────────────────────────────────────────
                        page.Header().Height(100).Row(row =>
                        {
                            byte[] logo = File.ReadAllBytes(
                                "C:\\Nube\\HumayaDigital\\HumayaDigitalBackEnd\\HDBackend\\HD_Reporteria\\Imagenes\\Logo.jpg");
                            row.ConstantItem(100).Image(logo);

                            row.RelativeItem().PaddingTop(25).Height(50).Background(verde)
                                .Padding(10).PaddingLeft(14)
                                .Text("SEGUNDO CONTEO - " + folio)
                                .FontColor("#fff").FontSize(16).Bold().FontFamily(font);
                        });

                        // ── CONTENIDO ─────────────────────────────────────────
                        page.Content().PaddingTop(12).PaddingLeft(20).PaddingRight(20).Column(col =>
                        {
                            col.Item().Height(8);

                            // ── 1. Métricas — tabla compacta de 4 columnas ────────────────────────────
                            col.Item().Border(0.5f).BorderColor(verde).Column(sec =>
                            {
                                sec.Item().Background(verdeOscuro).BorderBottom(1).BorderColor(amarillo)
                                    .Padding(4)
                                    .Text("MÉTRICAS DEL RESULTADO")
                                    .FontSize(7f).Bold().FontFamily(font).FontColor("#fff");

                                // Fila 1 — importes con porcentaje inline separado por |
                                sec.Item().Table(t =>
                                {
                                    t.ColumnsDefinition(c =>
                                    {
                                        c.RelativeColumn(1); c.RelativeColumn(1);
                                        c.RelativeColumn(1); c.RelativeColumn(1);
                                    });
                                    KpiMontoConPorcentaje(t, "IMPORTE TOTAL", info.importe_total_inventario.ToString("C2", culturaMoneda), null, verde, verdePanel, grisLinea, font);                       // sin estado
                                    KpiMontoConPorcentaje(t, "TOTAL NETO", info.total_neto.ToString("C2", culturaMoneda), $"{Math.Abs(info.porc_total_neto):N1}%", verdeOscuro, verdeClaro, grisLinea, font, menorEsMejor: false); // mayor es mejor
                                    KpiMontoConPorcentaje(t, "FALTANTE", info.importe_faltante.ToString("C2", culturaMoneda), $"{Math.Abs(info.porc_faltante):N1}%", "#c0392b", "#fff0f0", grisLinea, font, menorEsMejor: true);  // menor es mejor
                                    KpiMontoConPorcentaje(t, "SOBRANTE", info.importe_sobrante.ToString("C2", culturaMoneda), $"{Math.Abs(info.porc_sobrante):N1}%", "#1a6fa8", "#f0f5ff", grisLinea, font, menorEsMejor: true);  // menor es mejor
                                });

                                sec.Item().Height(1).Background(amarillo);

                                // Fila 2 — confiabilidades con barra
                                sec.Item().Table(t =>
                                {
                                    t.ColumnsDefinition(c =>
                                    {
                                        c.RelativeColumn(1); c.RelativeColumn(1);
                                    });
                                    KpiPorcentaje(t, "CONFIABILIDAD DE INVENTARIO", info.confiabilidad, verde, verdePanel, grisLinea, font, menorEsMejor: false);
                                    KpiPorcentaje(t, "CONFIABILIDAD DE UBICACIÓN", info.confiabilidad_ubi, verde, verdePanel, grisLinea, font, menorEsMejor: false);
                                });
                            });

                            col.Item().Height(10);

                            // ── 2. Tabla de diferencias ───────────────────────
                            col.Item().Border(0.5f).BorderColor(verde).Column(sec =>
                            {
                                sec.Item().Background(verdeOscuro).BorderBottom(1).BorderColor(amarillo)
                                    .Padding(5)
                                    .Text($"DIFERENCIAS DETECTADAS  " +
                                          $"({faltantes.Count} faltante{(faltantes.Count != 1 ? "s" : "")}, " +
                                          $"{sobrantes.Count} sobrante{(sobrantes.Count != 1 ? "s" : "")}, " +
                                          $"{correctos.Count} correcto{(correctos.Count != 1 ? "s" : "")})")
                                    .FontSize(7.5f).Bold().FontFamily(font).FontColor("#fff");

                                sec.Item().Table(t =>
                                {
                                    // 9 columnas en vertical — sin columna "TIPO"
                                    t.ColumnsDefinition(c =>
                                    {
                                        c.RelativeColumn(0.8f);  // familia
                                        c.RelativeColumn(1.0f);  // sku
                                        c.RelativeColumn(2.6f);  // descripcion
                                        c.RelativeColumn(0.75f); // posicion
                                        c.RelativeColumn(0.65f); // existencia
                                        c.RelativeColumn(0.65f); // conteo
                                        c.RelativeColumn(0.65f); // diferencia
                                        c.RelativeColumn(1.0f);  // precio unitario
                                        c.RelativeColumn(1.0f);  // importe
                                    });

                                    // Header
                                    t.Header(h =>
                                    {
                                        void TH(string txt) => h.Cell()
                                            .Background(verde).BorderBottom(0.5f).BorderColor(amarillo)
                                            .Padding(3).AlignCenter()
                                            .Text(txt).FontSize(6.5f).Bold().FontFamily(font).FontColor("#fff");

                                        TH("FAMILIA"); TH("SKU"); TH("DESCRIPCIÓN"); TH("POSICIÓN");
                                        TH("EXISTENCIA"); TH("CONTEO"); TH("DIFERENCIA");
                                        TH("PRECIO UNIT."); TH("IMPORTE DIF.");
                                        // ← sin columna TIPO
                                    });

                                    // ── CORRECTOS ─────────────────────────────
                                    if (correctos.Any())
                                    {
                                        t.Cell().ColumnSpan(9)
                                            .Background("#eaf3de").BorderBottom(0.5f).BorderColor("#97c459")
                                            .PaddingVertical(4).PaddingHorizontal(8)
                                            .Text($"CORRECTOS — {correctos.Count} registro{(correctos.Count != 1 ? "s" : "")}")
                                            .FontSize(7).Bold().FontFamily(font).FontColor("#27500a");
                                    }

                                    int idx = 0;
                                    foreach (var d in correctos)
                                    {
                                        string bg = idx % 2 == 0 ? "#ffffff" : "#f5fbf0";

                                        // TD — texto normal, izquierda por defecto
                                        void TD(string? txt, string? fondo = null, string? color = null, bool centro = false) =>
                                            t.Cell().Background(fondo ?? bg).BorderBottom(0.5f).BorderColor(grisLinea)
                                                .PaddingVertical(3).PaddingHorizontal(4)
                                                .Element(e => centro ? e.AlignCenter() : e.AlignLeft())
                                                .Text(txt ?? "").FontSize(6.5f).FontFamily(font).FontColor(color ?? "#333");

                                        // TS — cantidades/importes, derecha por defecto
                                        void TS(string? txt, string? fondo = null, string? color = null, bool centro = false) =>
                                            t.Cell().Background(fondo ?? bg).BorderBottom(0.5f).BorderColor(grisLinea)
                                                .PaddingVertical(3).PaddingHorizontal(4)
                                                .Element(e => centro ? e.AlignCenter() : e.AlignRight())  // ← derecha por defecto
                                                .Text(txt ?? "").FontSize(6.5f).FontFamily(font).FontColor(color ?? "#333");

                                        TD(d.familia, centro: true);
                                        TD(d.sku);
                                        TD(d.descripcion);
                                        TD(d.posicion, centro: true);
                                        TS(d.existencia.ToString("N2"));                                        // ← TS derecha
                                        TS(d.conteo.ToString("N2"));                                            // ← TS derecha
                                        TS(d.diferencias.ToString("N2"), "#eaf3de", "#27500a");
                                        TS(d.precio_unitario.ToString("C2", culturaMoneda));
                                        TS(d.importe_dif.ToString("C2", culturaMoneda));
                                        idx++;
                                    }

                                    // ── FALTANTES ─────────────────────────────
                                    if (faltantes.Any())
                                    {
                                        t.Cell().ColumnSpan(9)
                                            .Background("#fff0f0").BorderBottom(0.5f).BorderColor("#f09595")
                                            .PaddingVertical(4).PaddingHorizontal(8)
                                            .Text($"FALTANTES — {faltantes.Count} registro{(faltantes.Count != 1 ? "s" : "")}")
                                            .FontSize(7).Bold().FontFamily(font).FontColor("#c0392b");
                                    }

                                    idx = 0;
                                    foreach (var d in faltantes)
                                    {
                                        string bg = idx % 2 == 0 ? "#ffffff" : "#fff7f7";

                                        // TD — texto normal, izquierda por defecto
                                        void TD(string? txt, string? fondo = null, string? color = null, bool centro = false) =>
                                            t.Cell().Background(fondo ?? bg).BorderBottom(0.5f).BorderColor(grisLinea)
                                                .PaddingVertical(3).PaddingHorizontal(4)
                                                .Element(e => centro ? e.AlignCenter() : e.AlignLeft())
                                                .Text(txt ?? "").FontSize(6.5f).FontFamily(font).FontColor(color ?? "#333");

                                        // TS — cantidades/importes, derecha por defecto
                                        void TS(string? txt, string? fondo = null, string? color = null, bool centro = false) =>
                                            t.Cell().Background(fondo ?? bg).BorderBottom(0.5f).BorderColor(grisLinea)
                                                .PaddingVertical(3).PaddingHorizontal(4)
                                                .Element(e => centro ? e.AlignCenter() : e.AlignRight())  // ← derecha por defecto
                                                .Text(txt ?? "").FontSize(6.5f).FontFamily(font).FontColor(color ?? "#333");

                                        TD(d.familia, centro: true);
                                        TD(d.sku);
                                        TD(d.descripcion);
                                        TD(d.posicion, centro: true);
                                        TS(d.existencia.ToString("N2"));                                        // ← TS derecha
                                        TS(d.conteo.ToString("N2"));                                            // ← TS derecha
                                        TS(d.diferencias.ToString("N2"), "#eaf3de", "#27500a");
                                        TS(d.precio_unitario.ToString("C2", culturaMoneda));
                                        TS(d.importe_dif.ToString("C2", culturaMoneda));
                                        idx++;
                                    }

                                    // ── SOBRANTES ─────────────────────────────
                                    if (sobrantes.Any())
                                    {
                                        t.Cell().ColumnSpan(9)
                                            .Background("#f0f5ff").BorderBottom(0.5f).BorderColor("#85b7eb")
                                            .PaddingVertical(4).PaddingHorizontal(8)
                                            .Text($"SOBRANTES — {sobrantes.Count} registro{(sobrantes.Count != 1 ? "s" : "")}")
                                            .FontSize(7).Bold().FontFamily(font).FontColor("#1a6fa8");
                                    }

                                    idx = 0;
                                    foreach (var d in sobrantes)
                                    {
                                        string bg = idx % 2 == 0 ? "#ffffff" : "#f5f8ff";

                                        // TD — texto normal, izquierda por defecto
                                        void TD(string? txt, string? fondo = null, string? color = null, bool centro = false) =>
                                            t.Cell().Background(fondo ?? bg).BorderBottom(0.5f).BorderColor(grisLinea)
                                                .PaddingVertical(3).PaddingHorizontal(4)
                                                .Element(e => centro ? e.AlignCenter() : e.AlignLeft())
                                                .Text(txt ?? "").FontSize(6.5f).FontFamily(font).FontColor(color ?? "#333");

                                        // TS — cantidades/importes, derecha por defecto
                                        void TS(string? txt, string? fondo = null, string? color = null, bool centro = false) =>
                                            t.Cell().Background(fondo ?? bg).BorderBottom(0.5f).BorderColor(grisLinea)
                                                .PaddingVertical(3).PaddingHorizontal(4)
                                                .Element(e => centro ? e.AlignCenter() : e.AlignRight())  // ← derecha por defecto
                                                .Text(txt ?? "").FontSize(6.5f).FontFamily(font).FontColor(color ?? "#333");

                                        TD(d.familia, centro: true);
                                        TD(d.sku);
                                        TD(d.descripcion);
                                        TD(d.posicion, centro: true);
                                        TS(d.existencia.ToString("N2"));                                        // ← TS derecha
                                        TS(d.conteo.ToString("N2"));                                            // ← TS derecha
                                        TS(d.diferencias.ToString("N2"), "#eaf3de", "#27500a");
                                        TS(d.precio_unitario.ToString("C2", culturaMoneda));
                                        TS(d.importe_dif.ToString("C2", culturaMoneda));
                                        idx++;
                                    }

                                });
                            });

                            col.Item().Height(28);

                            // ── 3. Firmas ─────────────────────────────────────
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

                                r.ConstantItem(60);

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

                        // ── FOOTER ────────────────────────────────────────────
                        page.Footer().Height(28).PaddingHorizontal(20).Row(row =>
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
                    nombredocumento = $"Segundo_inventario_{folio}",
                    documento = Convert.ToBase64String(doc)
                };
            }
            catch (Exception ex) { throw ex; }
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private static void KpiMonto(TableDescriptor t,
string etiqueta, string valor,
string colorValor, string fondoCelda, string gris, string font)
        {
            t.Cell()
                .Background(fondoCelda)
                .BorderRight(0.5f).BorderColor(gris)
                .BorderBottom(0.5f).BorderColor(gris)
                .Padding(6)          // ← era 10
                .Column(c =>
                {
                    c.Item().Text(etiqueta)
                        .FontSize(6f).Bold().FontFamily(font).FontColor("#555");  // ← era 6.5
                    c.Item().PaddingTop(2).Text(valor)
                        .FontSize(10).Bold().FontFamily(font).FontColor(colorValor);  // ← era 13
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
                .Padding(6)          // ← era 8
                .Column(c =>
                {
                    c.Item().Text(etiqueta)
                        .FontSize(6f).Bold().FontFamily(font).FontColor("#555");  // ← era 6.5
                    c.Item().PaddingTop(2).Text($"{valor:N1}%")
                        .FontSize(11).Bold().FontFamily(font).FontColor(colorValor);  // ← era 15
                    c.Item().PaddingTop(3).Height(5).SkiaSharpCanvas((canvas, size) =>  // ← altura era 6
                    {
                        using var pFondo = new SKPaint
                        {
                            Color = SKColor.Parse("#d8d8d8"),
                            Style = SKPaintStyle.Fill,
                            IsAntialias = true
                        };
                        canvas.DrawRoundRect(
                            new SKRoundRect(new SKRect(0, 0, size.Width, size.Height), 3, 3), pFondo);

                        float w = size.Width * pct;
                        if (w > 0)
                        {
                            using var pRelleno = new SKPaint
                            {
                                Color = SKColor.Parse(colorValor),
                                Style = SKPaintStyle.Fill,
                                IsAntialias = true
                            };
                            canvas.DrawRoundRect(
                                new SKRoundRect(new SKRect(0, 0, w, size.Height), 3, 3), pRelleno);
                        }
                    });
                    c.Item().PaddingTop(1).Text(  // ← era PaddingTop(2)
                            menorEsMejor
                                ? (valor < 5 ? "ÓPTIMO" : valor < 15 ? "ACEPTABLE" : "CRÍTICO")
                                : (valor >= 95 ? "EXCELENTE" : valor >= 80 ? "ACEPTABLE" : "REQUIERE ATENCIÓN"))
                        .FontSize(5.5f).FontFamily(font).FontColor(colorValor);  // ← era 6
                });
        }

        private static void KpiMontoConPorcentaje(TableDescriptor t,
    string etiqueta, string monto, string? porcentaje,
    string colorValor, string fondoCelda, string gris, string font,
    bool? menorEsMejor = null)  // ← nullable: si null no muestra estado
        {
            // Calcular estado solo si viene porcentaje y menorEsMejor
            string? estado = null;
            string colorEstado = colorValor;

            if (porcentaje != null && menorEsMejor.HasValue)
            {
                // Extraer el valor numérico del string de porcentaje "12.5%"
                if (double.TryParse(porcentaje.Replace("%", "").Replace(",", ".").Trim(),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out double valorPct))
                {
                    if (menorEsMejor.Value)
                    {
                        colorEstado = valorPct < 5 ? "#275027" : valorPct < 15 ? "#b8860b" : "#c0392b";
                        estado = valorPct < 5 ? "ÓPTIMO" : valorPct < 15 ? "ACEPTABLE" : "CRÍTICO";
                    }
                    else
                    {
                        colorEstado = valorPct >= 95 ? "#275027" : valorPct >= 80 ? "#b8860b" : "#c0392b";
                        estado = valorPct >= 95 ? "EXCELENTE" : valorPct >= 80 ? "ACEPTABLE" : "REQUIERE ATENCIÓN";
                    }
                }
            }

            t.Cell()
                .Background(fondoCelda)
                .BorderRight(0.5f).BorderColor(gris)
                .BorderBottom(0.5f).BorderColor(gris)
                .Padding(6).Column(c =>
                {
                    c.Item().Text(etiqueta)
                        .FontSize(6f).Bold().FontFamily(font).FontColor("#555");

                    // Monto + separador + porcentaje en la misma línea
                    c.Item().PaddingTop(3).Row(r =>
                    {
                        r.AutoItem()
                            .Text(monto)
                            .FontSize(10).Bold().FontFamily(font).FontColor(colorValor);

                        if (!string.IsNullOrEmpty(porcentaje))
                        {
                            r.AutoItem().PaddingHorizontal(4)
                                .Text("|")
                                .FontSize(10).FontFamily(font).FontColor(gris);

                            r.AutoItem()
                                .Text(porcentaje)
                                .FontSize(10).Bold().FontFamily(font).FontColor(colorEstado);
                        }
                    });

                    // Estado debajo — solo si se calculó
                    if (estado != null)
                    {
                        c.Item().PaddingTop(1)
                            .Text(estado)
                            .FontSize(5.5f).FontFamily(font).FontColor(colorEstado);
                    }
                });
        }
    }
}
