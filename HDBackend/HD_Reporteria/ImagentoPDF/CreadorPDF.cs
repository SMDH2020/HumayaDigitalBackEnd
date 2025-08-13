using HD.Clientes.Modelos.Pedido_Impresion;
using HD_Reporteria.ImagentoPDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.IO;

namespace HD_Reporteria.Solicitud_Credito
{
    public class CreadorPDF
    {
        public static RPT_Result Generar(mdl_Covertor mdl)
        {
            try
            {
                //var imagenesBase64 = new[] {mdl.imagen1, mdl.imagen2, mdl.imagen3, mdl.imagen4};

                byte[] doc = Document.Create(document =>
                {
                    foreach (var imgBase64 in mdl.ImagenesBase64)
                    {
                        if (string.IsNullOrWhiteSpace(imgBase64))
                            continue;

                        // Convertir Base64 a byte[]
                        byte[] imgBytes = Convert.FromBase64String(imgBase64);

                        document.Page(page =>
                        {
                            page.Size(PageSizes.A4);
                            page.Margin(0); // sin márgenes
                            page.PageColor(Colors.White);

                            page.Content().AlignCenter()
                                .Image(imgBytes) // coloca la imagen
                                .FitArea(); // que se ajuste a toda el área de la página

                            // (Opcional) footer con número de página
                            page.Footer()
                                .AlignRight()
                                .Text(text =>
                                {
                                    text.Span("Página ").FontSize(10);
                                    text.CurrentPageNumber().FontSize(10).Bold();
                                });
                        });
                    }
                }).GeneratePdf();

                return new RPT_Result
                {
                    extension = "pdf",
                    nombredocumento = "Pedido maquinaria",
                    documento = Convert.ToBase64String(doc)
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
