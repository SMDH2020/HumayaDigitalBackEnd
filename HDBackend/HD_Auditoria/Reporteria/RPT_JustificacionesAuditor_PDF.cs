using HD_Auditoria.Modelos.Programar_Inventario;
using HD_Reporteria;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Reporteria
{
    public class RPT_JustificacionesAuditor_PDF
    {
        public static RPT_Result GenerarPDF(IEnumerable<mdl_JustificacionesAuditor> detalle, string? folio)
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
                                .Text("REPORTE JUSTIFICACIONES - " + folio)
                                .FontColor("#fff").FontSize(16).Bold().FontFamily(font);
                        });

                        // ── CONTENIDO ─────────────────────────────────────────
                        page.Content().PaddingTop(12).PaddingLeft(20).PaddingRight(20).Column(col =>
                        {
                            col.Item().Height(8);


                            // ── 2. Tabla de diferencias ───────────────────────
                            col.Item().Border(0.5f).BorderColor(verde).Column(sec =>
                            {

                                sec.Item().Table(t =>
                                {
                                    // 9 columnas en vertical — sin columna "TIPO"
                                    t.ColumnsDefinition(c =>
                                    {
                                        c.RelativeColumn(1);  // Codigo
                                        c.RelativeColumn(2);  // Descripcion
                                        c.RelativeColumn(3); // Justificacion  
                                        c.RelativeColumn(0.3f);  // Justificadas
                                        c.RelativeColumn(1.5f);  // Auiditor
                                    });

                                    t.Header(h =>
                                    {
                                        void TH(string txt) => h.Cell()
                                            .Background(verde).BorderBottom(0.5f).BorderColor(amarillo)
                                            .Padding(3).AlignCenter()
                                            .Text(txt).FontSize(6.5f).Bold().FontFamily(font).FontColor("#fff");

                                        TH("Codigo"); TH("Descripcion"); TH("Justificación"); TH("#");
                                        TH("Auditor");
                                    });

                                    int idx = 0;
                                    foreach (var d in detalle)
                                    {
                                        string bg = idx % 2 == 0 ? "#ffffff" : "#f5fbf0";

                                        // TD — texto normal, izquierda por defecto
                                        void TD(string? txt, string? fondo = null, string? color = null, bool centro = false) =>
                                            t.Cell().Background(fondo ?? bg).BorderBottom(0.5f).BorderColor(grisLinea)
                                                .PaddingVertical(3).PaddingHorizontal(4)
                                                .AlignMiddle()
                                                .Element(e => centro ? e.AlignCenter() : e.AlignLeft())
                                                .Text(txt ?? "").FontSize(6.5f).FontFamily(font).FontColor(color ?? "#333");

                                        // TS — cantidades/importes, derecha por defecto
                                        void TS(string? txt, string? fondo = null, string? color = null, bool centro = false) =>
                                            t.Cell().Background(fondo ?? bg).BorderBottom(0.5f).BorderColor(grisLinea)
                                                .PaddingVertical(3).PaddingHorizontal(4)
                                                .Element(e => centro ? e.AlignCenter() : e.AlignRight())  // ← derecha por defecto
                                                .Text(txt ?? "").FontSize(6.5f).FontFamily(font).FontColor(color ?? "#333");

                                        TD(d.codigo);
                                        TD(d.descripcion);
                                        TD(d.justificacion);
                                        TD(d.justificadas, centro: true);
                                        TD(d.auditor);                    
                                        idx++;
                                    }

                                });
                            });

                            col.Item().Height(28);

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
                    nombredocumento = $"Reporte_justificaciones_{folio}",
                    documento = Convert.ToBase64String(doc)
                };
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
