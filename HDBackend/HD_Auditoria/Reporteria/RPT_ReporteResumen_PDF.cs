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
    public class RPT_ReporteResumen_PDF
    {
        public static RPT_Result GenerarPDF(mdl_ReporteResumen_View detalle, string? folio)
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

                var info = detalle.primer_conteo;
                var info2 = detalle.segundo_conteo;
                var info3 = detalle.justificados;

                var culturaMoneda = new CultureInfo("es-MX");

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
                                .Text("REPORTE RESUMEN - " + folio)
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
                                    .Text("MÉTRICAS DEL RESULTADO PRIMER CONTEO")
                                    .FontSize(7f).Bold().FontFamily(font).FontColor("#fff");

                                // Fila 1 — importes con porcentaje inline separado por |
                                sec.Item().Table(t =>
                                {
                                    t.ColumnsDefinition(c =>
                                    {
                                        c.RelativeColumn(1); c.RelativeColumn(1);
                                        c.RelativeColumn(1); c.RelativeColumn(1);
                                    });
                                    KpiMontoConPorcentaje(t, "IMPORTE TOTAL", info.importe_total_inventario.ToString("C2", culturaMoneda), null, verde, verdePanel, grisLinea, font);

                                    KpiMontoConPorcentaje(t, "TOTAL NETO", info.total_neto.ToString("C2", culturaMoneda), $"{Math.Abs(info.porc_total_neto):N2}%", verdeOscuro, verdeClaro, grisLinea, font, tipoKpi: "total_neto", esNegativo: info.porc_total_neto < 0);

                                    KpiMontoConPorcentaje(t, "FALTANTE", info.importe_faltante.ToString("C2", culturaMoneda), $"{Math.Abs(info.porc_faltante):N2}%", "#c0392b", "#fff0f0", grisLinea, font, tipoKpi: "faltante_sobrante");

                                    KpiMontoConPorcentaje(t, "SOBRANTE", info.importe_sobrante.ToString("C2", culturaMoneda), $"{Math.Abs(info.porc_sobrante):N2}%", "#1a6fa8", "#f0f5ff", grisLinea, font, tipoKpi: "faltante_sobrante");
                                });

                                sec.Item().Height(1).Background(amarillo);

                                // Fila 2 — confiabilidades con barra
                                sec.Item().Table(t =>
                                {
                                    t.ColumnsDefinition(c =>
                                    {
                                        c.RelativeColumn(1); c.RelativeColumn(1);
                                    });
                                    KpiPorcentaje(t, "CONFIABILIDAD DE INVENTARIO", info.confiabilidad, verde, verdePanel, grisLinea, font, tipoConfiabilidad: "inventario");
                                    KpiPorcentaje(t, "CONFIABILIDAD DE UBICACIÓN", info.confiabilidad_ubi, verde, verdePanel, grisLinea, font, tipoConfiabilidad: "localizacion");
                                });
                            });

                            col.Item().Height(20);

                            // ── 2. Métricas 2 — tabla compacta de 4 columnas ────────────────────────────
                            col.Item().Border(0.5f).BorderColor(verde).Column(sec =>
                            {
                                sec.Item().Background(verdeOscuro).BorderBottom(1).BorderColor(amarillo)
                                    .Padding(4)
                                    .Text("MÉTRICAS DEL RESULTADO SEGUNDDO CONTEO")
                                    .FontSize(7f).Bold().FontFamily(font).FontColor("#fff");

                                // Fila 1 — importes con porcentaje inline separado por |
                                sec.Item().Table(t =>
                                {
                                    t.ColumnsDefinition(c =>
                                    {
                                        c.RelativeColumn(1); c.RelativeColumn(1);
                                        c.RelativeColumn(1); c.RelativeColumn(1);
                                    });
                                    KpiMontoConPorcentaje(t, "IMPORTE TOTAL", info2.importe_total_inventario.ToString("C2", culturaMoneda), null, verde, verdePanel, grisLinea, font);

                                    KpiMontoConPorcentaje(t, "TOTAL NETO", info2.total_neto.ToString("C2", culturaMoneda), $"{Math.Abs(info2.porc_total_neto):N2}%", verdeOscuro, verdeClaro, grisLinea, font, tipoKpi: "total_neto", esNegativo: info.porc_total_neto < 0);

                                    KpiMontoConPorcentaje(t, "FALTANTE", info2.importe_faltante.ToString("C2", culturaMoneda), $"{Math.Abs(info2.porc_faltante):N2}%", "#c0392b", "#fff0f0", grisLinea, font, tipoKpi: "faltante_sobrante");

                                    KpiMontoConPorcentaje(t, "SOBRANTE", info2.importe_sobrante.ToString("C2", culturaMoneda), $"{Math.Abs(info2.porc_sobrante):N2}%", "#1a6fa8", "#f0f5ff", grisLinea, font, tipoKpi: "faltante_sobrante");
                                });

                                sec.Item().Height(1).Background(amarillo);

                                // Fila 2 — confiabilidades con barra
                                sec.Item().Table(t =>
                                {
                                    t.ColumnsDefinition(c =>
                                    {
                                        c.RelativeColumn(1); c.RelativeColumn(1);
                                    });
                                    KpiPorcentaje(t, "CONFIABILIDAD DE INVENTARIO", info2.confiabilidad, verde, verdePanel, grisLinea, font, tipoConfiabilidad: "inventario");
                                    KpiPorcentaje(t, "CONFIABILIDAD DE UBICACIÓN", info2.confiabilidad_ubi, verde, verdePanel, grisLinea, font, tipoConfiabilidad: "localizacion");
                                });
                            });

                            col.Item().Height(20);

                            // ── 3. Métricas 3 — JUSTIFICACIONES ────────────────────────────
                            col.Item().Border(0.5f).BorderColor(verde).Column(sec =>
                            {
                                sec.Item().Background(verdeOscuro).BorderBottom(1).BorderColor(amarillo)
                                    .Padding(4)
                                    .Text("MÉTRICAS DEL RESULTADO JUSTIFICACIONES")
                                    .FontSize(7f).Bold().FontFamily(font).FontColor("#fff");

                                // Fila 1 — importes con porcentaje inline separado por |
                                sec.Item().Table(t =>
                                {
                                    t.ColumnsDefinition(c =>
                                    {
                                        c.RelativeColumn(1); c.RelativeColumn(1);
                                        c.RelativeColumn(1); c.RelativeColumn(1);
                                    });
                                    KpiMontoConPorcentaje(t, "IMPORTE TOTAL", info3.importe_total_inventario.ToString("C2", culturaMoneda), null, verde, verdePanel, grisLinea, font);

                                    KpiMontoConPorcentaje(t, "TOTAL NETO", info3.total_neto.ToString("C2", culturaMoneda), $"{Math.Abs(info3.porc_total_neto):N2}%", verdeOscuro, verdeClaro, grisLinea, font, tipoKpi: "total_neto", esNegativo: info.porc_total_neto < 0);

                                    KpiMontoConPorcentaje(t, "FALTANTE", info3.importe_faltante.ToString("C2", culturaMoneda), $"{Math.Abs(info3.porc_faltante):N2}%", "#c0392b", "#fff0f0", grisLinea, font, tipoKpi: "faltante_sobrante");

                                    KpiMontoConPorcentaje(t, "SOBRANTE", info3.importe_sobrante.ToString("C2", culturaMoneda), $"{Math.Abs(info3.porc_sobrante):N2}%", "#1a6fa8", "#f0f5ff", grisLinea, font, tipoKpi: "faltante_sobrante");
                                });

                                sec.Item().Height(1).Background(amarillo);

                                // Fila 2 — confiabilidades con barra
                                sec.Item().Table(t =>
                                {
                                    t.ColumnsDefinition(c =>
                                    {
                                        c.RelativeColumn(1); c.RelativeColumn(1);
                                    });
                                    KpiPorcentaje(t, "CONFIABILIDAD DE INVENTARIO", info3.confiabilidad, verde, verdePanel, grisLinea, font, tipoConfiabilidad: "inventario");
                                    KpiPorcentaje(t, "CONFIABILIDAD DE UBICACIÓN", info3.confiabilidad_ubi, verde, verdePanel, grisLinea, font, tipoConfiabilidad: "localizacion");
                                });
                            });

                            //col.Item().Height(28);

                            // ── 3. Firmas ─────────────────────────────────────
                            //col.Item().Row(r =>
                            //{
                            //    r.RelativeItem().Column(c =>
                            //    {
                            //        c.Item().Height(36);
                            //        c.Item().BorderTop(0.8f).BorderColor("#444").PaddingTop(5)
                            //            .AlignCenter()
                            //            .Text("ENCARGADO DE ALMACÉN")
                            //            .FontSize(7.5f).Bold().FontFamily(font).FontColor(verdeOscuro);
                            //        c.Item().AlignCenter()
                            //            .Text(firmas?.encargado_almacen?.ToUpper() ?? "")
                            //            .FontSize(7).FontFamily(font).FontColor("#333");
                            //    });

                            //    r.ConstantItem(60);

                            //    r.RelativeItem().Column(c =>
                            //    {
                            //        c.Item().Height(36);
                            //        c.Item().BorderTop(0.8f).BorderColor("#444").PaddingTop(5)
                            //            .AlignCenter()
                            //            .Text("AUDITOR")
                            //            .FontSize(7.5f).Bold().FontFamily(font).FontColor(verdeOscuro);
                            //        c.Item().AlignCenter()
                            //            .Text(firmas?.auditor?.ToUpper() ?? "")
                            //            .FontSize(7).FontFamily(font).FontColor("#333");
                            //    });
                            //});
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
                    nombredocumento = $"Reporte_resumen_{folio}",
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
                        .FontSize(10).Bold().FontFamily(font).FontColor(colorValor); // ← era 13

                });
        }

        private static void KpiPorcentaje(TableDescriptor t,
            string etiqueta, double valor,
            string colorBase, string fondoCelda, string gris, string font,
            string tipoConfiabilidad)   // "inventario" | "localizacion"
        {
            string colorValor;
            string etiquetaEstado;

            if (tipoConfiabilidad == "inventario")
            {
                if (valor >= 98) { colorValor = "#275027"; etiquetaEstado = "ÓPTIMO"; }
                else if (valor >= 95) { colorValor = "#1a6fa8"; etiquetaEstado = "EN CONTROL"; }
                else if (valor >= 90) { colorValor = "#b8860b"; etiquetaEstado = "SEGUIMIENTO"; }
                else { colorValor = "#c0392b"; etiquetaEstado = "REQUIERE ACCIÓN"; }
            }
            else // localizacion
            {
                if (valor >= 98) { colorValor = "#275027"; etiquetaEstado = "ÓPTIMO"; }
                else if (valor >= 95) { colorValor = "#1a6fa8"; etiquetaEstado = "CONTROLADO"; }
                else if (valor >= 90) { colorValor = "#b8860b"; etiquetaEstado = "SEGUIMIENTO"; }
                else { colorValor = "#c0392b"; etiquetaEstado = "REQUIERE CORRECCIÓN"; }
            }

            float pct = (float)Math.Min(Math.Max(valor / 100.0, 0.0), 1.0);

            t.Cell()
                .Background(fondoCelda)
                .BorderRight(0.5f).BorderColor(gris)
                .BorderBottom(0.5f).BorderColor(gris)
                .Padding(6).Column(c =>
                {
                    c.Item().Text(etiqueta)
                        .FontSize(6f).Bold().FontFamily(font).FontColor("#555");
                    c.Item().PaddingTop(2).Text($"{valor:N1}%")
                        .FontSize(11).Bold().FontFamily(font).FontColor(colorValor);
                    c.Item().PaddingTop(3).Height(5).SkiaSharpCanvas((canvas, size) =>
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
                    c.Item().PaddingTop(1)
                        .Text(etiquetaEstado)
                        .FontSize(5.5f).FontFamily(font).FontColor(colorValor);
                });
        }

        private static void KpiMontoConPorcentaje(TableDescriptor t,
            string etiqueta, string monto, string? porcentaje,
            string colorValor, string fondoCelda, string gris, string font,
            string? tipoKpi = null,       // "total_neto" | "faltante_sobrante" | null
            bool esNegativo = false)      // solo aplica para total_neto
        {
            string? estado = null;
            string colorEstado = colorValor;

            if (porcentaje != null && tipoKpi != null)
            {
                if (double.TryParse(
                    porcentaje.Replace("%", "").Replace(",", ".").Trim(),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out double valorPct))
                {
                    if (tipoKpi == "total_neto")
                    {
                        // Positivo → siempre DENTRO DE TOLERANCIA
                        // Negativo → tolerado hasta -0.20%, a partir de -0.21% excede
                        if (!esNegativo || valorPct <= 0.20)
                        {
                            colorEstado = "#275027";
                            estado = "DENTRO DE TOLERANCIA";
                        }
                        else
                        {
                            colorEstado = "#c0392b";
                            estado = "EXCEDE TOLERANCIA";
                        }
                    }
                    else if (tipoKpi == "faltante_sobrante")
                    {
                        // <= 0.50% → DENTRO DE TOLERANCIA | > 0.50% → EXCEDE TOLERANCIA
                        if (valorPct <= 0.50)
                        {
                            colorEstado = "#275027";
                            estado = "DENTRO DE TOLERANCIA";
                        }
                        else
                        {
                            colorEstado = "#c0392b";
                            estado = "EXCEDE TOLERANCIA";
                        }
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
