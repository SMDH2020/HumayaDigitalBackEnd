using QuestPDF.Fluent;
using QuestPDF.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using HD.Clientes.Consultas.Modelos;
using HD.Clientes.Modelos.CRM.Cotizaciones;
using HD.Clientes.Consultas.SolicitudCreditoDocumento;

namespace HD.Clientes.Reportes
{
    public class RPT_Cotizacion_CRM
    {
        // Ajusta esta ruta si tus otras plantillas (RPT_Cotizacion) usan una distinta
        private const string RutaLogos = "C:\\Nube\\HumayaDigital\\HumayaDigitalBackEnd\\HDBackend\\HD_Reporteria\\Imagenes\\";
        private const string fontFamily = "Calibri";

        // =====================================================================
        // PUNTO DE ENTRADA
        // =====================================================================

        public static RPT_Result GenerarPDF(mdl_Cotizaciones_CRM_Folio_View vista, string plantilla)
        {
            try
            {
                if (vista?.Cotizacion == null)
                    throw new ArgumentException("No se encontró información para la cotización.");

                var c = Mapear(vista);
                byte[] doc;

                switch ((plantilla ?? string.Empty).Trim().ToLower())
                {
                    case "dron":
                        doc = GenerarDron(c);
                        break;
                    case "riego":
                        doc = GenerarRiego(c);
                        break;
                    case "servicio":
                        doc = GenerarServicio(c);
                        break;
                    default:
                        throw new ArgumentException($"La plantilla '{plantilla}' no es válida. Use 'dron', 'riego' o 'servicio'.");
                }

                return new RPT_Result
                {
                    extension = "pdf",
                    nombredocumento = "COTIZACION",
                    documento = Convert.ToBase64String(doc)
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // =====================================================================
        // MAPEO desde el resultado real de AD_Cotizaciones_CRM.ObtenerPorFolio
        // =====================================================================

        private static mdl_Cotizacion_CRM_Imprimir Mapear(mdl_Cotizaciones_CRM_Folio_View vista)
        {
            var cot = vista.Cotizacion;

            var cliente = vista.Clientes?.FirstOrDefault(x => x.idcliente == cot.idcliente);
            var asesor = vista.Asesores?.FirstOrDefault(x => x.IDEmpleado == cot.id_asesor);
            var propietario = vista.Asesores?.FirstOrDefault(x => x.IDEmpleado == cot.id_propietario);

            var mdl = new mdl_Cotizacion_CRM_Imprimir
            {
                folio_crm = cot.folio,
                asunto = cot.asunto,
                apreciable = cot.nombre_contacto,
                empresa = cliente?.razon_social ?? "",
                direccion = "",   // no disponible en el catálogo actual de clientes
                ciudad = "",   // no disponible en el catálogo actual de clientes
                sucursal = asesor?.sucursal ?? "",
                telefono_sucursal = "",   // sin catálogo de teléfonos por sucursal aún
                sitio_web = "www.humaya.com.mx",
                asesorventa = asesor?.empleado ?? "",
                atendio = asesor?.empleado ?? "",
                atentamente = propietario?.empleado ?? "",
                fecha = DateTime.Now, // TODO: reemplazar cuando exista fecha de creación real
                vigencia = cot.vigencia.ToString("dd/MM/yyyy"),
                terminos = "Los precios están sujetos a cambio sin previo aviso. Cotización válida "
                                   + "únicamente durante el periodo de vigencia indicado. Precios netos, no "
                                   + "incluyen instalación salvo que se especifique lo contrario.",
                moneda = "Pesos",

                subtotal = cot.subtotal,
                descuento_general = cot.descuento,
                ajuste = cot.ajuste,
                total = cot.total
            };

            var detalle = vista.Detalle?.Select(d => new mdl_Cotizacion_CRM_Detalle_Imprimir
            {
                cantidad = d.cantidad,
                modelo = d.nombre_servicio,
                descripcion = d.descripcion,
                precio_lista = d.precio_lista,
                descuento = d.descuento,
                impuesto = d.impuesto,
                importe = d.importe,
                importe_total = d.importe_total
            }).ToList() ?? new List<mdl_Cotizacion_CRM_Detalle_Imprimir>();

            mdl.detalle = JsonConvert.SerializeObject(detalle);
            return mdl;
        }

        // =====================================================================
        // HELPERS COMPARTIDOS
        // =====================================================================

        private static List<mdl_Cotizacion_CRM_Detalle_Imprimir> ObtenerModelos(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return new List<mdl_Cotizacion_CRM_Detalle_Imprimir>();
            return JsonConvert.DeserializeObject<List<mdl_Cotizacion_CRM_Detalle_Imprimir>>(json) ?? new List<mdl_Cotizacion_CRM_Detalle_Imprimir>();
        }

        private static string FormatearMoneda(double valor)
        {
            return valor.ToString("$#,##0.00", CultureInfo.GetCultureInfo("es-MX"));
        }

        private static byte[] LeerLogo(string archivo)
        {
            try { return File.ReadAllBytes(RutaLogos + archivo); }
            catch { return null; }
        }

        private static void DatosCliente(QuestPDF.Infrastructure.IContainer container, mdl_Cotizacion_CRM_Imprimir c)
        {
            container.Column(col =>
            {
                col.Item().Text(txt => { txt.Span("Apreciable: ").Bold(); txt.Span(c.apreciable); });
                col.Item().Text(txt => { txt.Span("Empresa: ").Bold(); txt.Span(c.empresa); });

                if (!string.IsNullOrWhiteSpace(c.direccion))
                    col.Item().Text(txt => { txt.Span("Dirección: ").Bold(); txt.Span(c.direccion); });

                if (!string.IsNullOrWhiteSpace(c.ciudad))
                    col.Item().Text(txt => { txt.Span("Ciudad: ").Bold(); txt.Span(c.ciudad); });
            });
        }

        private static void RenglonDetalle(QuestPDF.Infrastructure.IContainer cell, mdl_Cotizacion_CRM_Detalle_Imprimir m)
        {
            cell.Column(cc =>
            {
                cc.Item().Text(m.modelo).Bold();

                if (!string.IsNullOrWhiteSpace(m.descripcion))
                {
                    var lineas = m.descripcion.Replace("\\n", "\n").Split('\n');
                    cc.Item().Column(inner =>
                    {
                        foreach (var linea in lineas)
                        {
                            if (!string.IsNullOrWhiteSpace(linea))
                                inner.Item().Text(linea.Trim()).FontSize(8);
                        }
                    });
                }
            });
        }

        private static void ImporteDetalle(QuestPDF.Infrastructure.IContainer cell, mdl_Cotizacion_CRM_Detalle_Imprimir m, string moneda)
        {
            cell.Column(cc =>
            {
                cc.Item().Text(txt =>
                {
                    txt.Span("Precio de lista: ").FontSize(8);
                    txt.Span($"{FormatearMoneda(m.precio_lista)} {moneda}").Bold().FontSize(8);
                });

                if (m.descuento > 0)
                    cc.Item().Text(txt => { txt.Span("Descuento: ").FontSize(8); txt.Span(FormatearMoneda(m.descuento)).Bold().FontSize(8); });

                cc.Item().Text(txt => { txt.Span("Importe: ").FontSize(8); txt.Span(FormatearMoneda(m.importe_total)).Bold().FontSize(8); });
            });
        }

        private static void TotalesFinales(QuestPDF.Infrastructure.IContainer container, mdl_Cotizacion_CRM_Imprimir c, List<mdl_Cotizacion_CRM_Detalle_Imprimir> modelos)
        {
            double impuestoTotal = modelos.Sum(m => m.impuesto);

            container.AlignRight().Column(tot =>
            {
                tot.Item().Text(txt => { txt.Span("Subtotal ").Bold(); txt.Span(FormatearMoneda(c.subtotal)); });

                if (c.descuento_general > 0)
                    tot.Item().Text(txt => { txt.Span("Descuento ").Bold(); txt.Span(FormatearMoneda(c.descuento_general)); });

                if (c.ajuste != 0)
                    tot.Item().Text(txt => { txt.Span("Ajuste ").Bold(); txt.Span(FormatearMoneda(c.ajuste)); });

                tot.Item().Text(txt => { txt.Span("Impuestos ").Bold(); txt.Span(FormatearMoneda(impuestoTotal)); });
                tot.Item().Text(txt => { txt.Span("Total ").Bold().FontSize(11); txt.Span(FormatearMoneda(c.total)).FontSize(11); });
            });
        }

        private static void FirmasFooter(QuestPDF.Infrastructure.IContainer container, mdl_Cotizacion_CRM_Imprimir c)
        {
            container.Row(row =>
            {
                row.RelativeItem().AlignCenter().Column(col =>
                {
                    col.Item().AlignCenter().Text("ATENTAMENTE").FontSize(9);
                    col.Item().AlignCenter().Text(c.atentamente).Bold().FontSize(9);
                });
                row.RelativeItem().AlignCenter().Column(col =>
                {
                    col.Item().AlignCenter().Text("ATENDIÓ").FontSize(9);
                    col.Item().AlignCenter().Text(c.atendio).Bold().FontSize(9);
                });
                row.RelativeItem().AlignCenter().Column(col =>
                {
                    col.Item().AlignCenter().Text("VÁLIDO HASTA").FontSize(9);
                    col.Item().AlignCenter().Text(c.vigencia).Bold().FontSize(9);
                });
            });
        }

        private static void PiePagina(QuestPDF.Infrastructure.IContainer container)
        {
            container.AlignRight().Text(txt =>
            {
                txt.Span("Pág. ").FontSize(8);
                txt.CurrentPageNumber().FontSize(8).Bold();
                txt.Span(" de ").FontSize(8);
                txt.TotalPages().FontSize(8).Bold();
            });
        }

        // =====================================================================
        // 1) DRON — estilo DJI Agriculture
        // =====================================================================

        private static byte[] GenerarDron(mdl_Cotizacion_CRM_Imprimir c)
        {
            var modelos = ObtenerModelos(c.detalle);

            return QuestPDF.Fluent.Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.Letter.Portrait());
                    page.Margin(30);
                    page.DefaultTextStyle(t => t.FontFamily(fontFamily).FontSize(9));

                    page.Header().Column(head =>
                    {
                        head.Item().Row(row =>
                        {
                            row.RelativeItem();
                            row.ConstantItem(220).AlignRight().Column(col =>
                            {
                                var logo = LeerLogo("Logo_DJI.jpg");
                                if (logo != null) col.Item().AlignRight().Width(160).Image(logo);
                                else col.Item().AlignRight().Text("DJI AGRICULTURE").Bold().FontSize(16);

                                col.Item().AlignRight().Text("MAQUINARIA DEL HUMAYA").FontSize(9).Bold();
                            });
                        });
                        head.Item().PaddingTop(8).Height(6).Background("#000");
                    });

                    page.Content().PaddingTop(10).Column(col1 =>
                    {
                        col1.Item().Row(row =>
                        {
                            row.RelativeItem().Element(e => DatosCliente(e, c));

                            row.RelativeItem().AlignRight().Column(col =>
                            {
                                col.Item().AlignRight().Text(txt => { txt.Span("Folio Cotización CRM: ").Bold(); txt.Span(c.folio_crm); });
                                col.Item().AlignRight().Text(txt => { txt.Span("Asesor de ventas: ").Bold(); txt.Span(c.asesorventa); });
                                col.Item().AlignRight().Text(c.fecha.ToString("dd/MM/yyyy HH:mm"));
                            });
                        });

                        col1.Item().PaddingTop(10)
                            .Text("De acuerdo a su amable solicitud tenemos el gusto de cotizar a usted lo siguiente:");

                        col1.Item().PaddingTop(8).Table(tabla =>
                        {
                            tabla.ColumnsDefinition(c2 =>
                            {
                                c2.ConstantColumn(40);
                                c2.RelativeColumn(3);
                                c2.RelativeColumn(2);
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#000").Padding(5).Text("Cant.").Bold().FontColor("#fff");
                                header.Cell().Background("#000").Padding(5).Text("Descripción").Bold().FontColor("#fff");
                                header.Cell().Background("#000").Padding(5).AlignRight().Text("Importe").Bold().FontColor("#fff");
                            });

                            foreach (var m in modelos)
                            {
                                tabla.Cell().BorderBottom(1).BorderColor("#000").Padding(5).AlignCenter().Text(m.cantidad.ToString());
                                RenglonDetalle(tabla.Cell().BorderBottom(1).BorderColor("#000").Padding(5), m);
                                ImporteDetalle(tabla.Cell().BorderBottom(1).BorderColor("#000").Padding(5), m, c.moneda);
                            }
                        });

                        col1.Item().PaddingTop(8).Element(e => TotalesFinales(e, c, modelos));

                        col1.Item().PaddingTop(10)
                            .Text("Estos precios son netos y de contado quedan sujetos a cambio sin previo aviso, prevaleciendo los que estén en vigor al momento de facturar.")
                            .FontSize(8);

                        if (!string.IsNullOrWhiteSpace(c.terminos))
                        {
                            col1.Item().PaddingTop(10).Text("Términos y condiciones").Bold();
                            col1.Item().PaddingTop(3).Text(c.terminos).FontSize(8).Justify();
                        }

                        col1.Item().PaddingTop(60).Element(e => FirmasFooter(e, c));
                    });

                    page.Footer().Height(30).PaddingRight(30).PaddingBottom(10).Element(PiePagina);
                });
            }).GeneratePdf();
        }

        // =====================================================================
        // 2) RIEGO — estilo Rivulis
        // =====================================================================

        private static byte[] GenerarRiego(mdl_Cotizacion_CRM_Imprimir c)
        {
            var modelos = ObtenerModelos(c.detalle);

            return QuestPDF.Fluent.Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.Letter.Portrait());
                    page.Margin(30);
                    page.DefaultTextStyle(t => t.FontFamily(fontFamily).FontSize(9));

                    page.Header().Column(head =>
                    {
                        head.Item().Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text($"Sucursal: {c.sucursal}").Bold();
                                if (!string.IsNullOrWhiteSpace(c.telefono_sucursal))
                                    col.Item().Text(c.telefono_sucursal);
                                col.Item().Text(c.sitio_web).FontColor("#0072BC").Underline();
                            });

                            row.ConstantItem(150).AlignRight().Height(50).Element(e =>
                            {
                                var logo = LeerLogo("Logo_Rivulis.jpg");
                                if (logo != null) e.Image(logo); else e.AlignRight().Text("Rivulis").Bold().FontSize(18).FontColor("#0072BC");
                            });
                        });

                        head.Item().PaddingTop(15).Row(row =>
                        {
                            row.RelativeItem().Text(txt =>
                            {
                                txt.Span("Asunto: ").FontSize(10);
                                txt.Span(c.asunto).Bold().FontSize(10);
                            });

                            row.RelativeItem().AlignRight().Column(col =>
                            {
                                col.Item().Text(txt => { txt.Span("Folio de cotización CRM: ").FontSize(8); txt.Span(c.folio_crm).Bold().FontSize(8); });
                                col.Item().Text(txt => { txt.Span("Asesor de ventas: ").FontSize(8); txt.Span(c.asesorventa).Bold().FontSize(8); });
                            });
                        });
                    });

                    page.Content().PaddingTop(10).Column(col1 =>
                    {
                        col1.Item().Border(1).BorderColor("#000").Padding(6).Row(row =>
                        {
                            row.RelativeItem(3).Element(e => DatosCliente(e, c));
                            row.ConstantItem(90).AlignTop().AlignRight().Text(c.fecha.ToString("dd/MM/yyyy HH:mm")).FontSize(8);
                        });

                        col1.Item().PaddingTop(10)
                            .Text("De acuerdo a su amable solicitud tenemos el gusto de cotizar a usted lo siguiente:");

                        col1.Item().PaddingTop(8).Table(tabla =>
                        {
                            tabla.ColumnsDefinition(c2 =>
                            {
                                c2.ConstantColumn(40);
                                c2.RelativeColumn(3);
                                c2.RelativeColumn(2);
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().BorderBottom(2).BorderColor("#000").Padding(4).Text("CANT.").Bold();
                                header.Cell().BorderBottom(2).BorderColor("#000").Padding(4).Text("DESCRIPCIÓN").Bold();
                                header.Cell().BorderBottom(2).BorderColor("#000").Padding(4).AlignRight().Text("IMPORTE").Bold();
                            });

                            foreach (var m in modelos)
                            {
                                tabla.Cell().BorderBottom(1).BorderColor("#CCC").Padding(4).AlignCenter().Text(m.cantidad.ToString());
                                RenglonDetalle(tabla.Cell().BorderBottom(1).BorderColor("#CCC").Padding(4), m);
                                ImporteDetalle(tabla.Cell().BorderBottom(1).BorderColor("#CCC").Padding(4), m, c.moneda);
                            }
                        });

                        col1.Item().PaddingTop(8).Element(e => TotalesFinales(e, c, modelos));

                        col1.Item().PaddingTop(10)
                            .Text("Estos precios son netos y de contado quedan sujetos a cambio sin previo aviso, prevaleciendo los que estén en vigor al momento de facturar.")
                            .FontSize(8);

                        if (!string.IsNullOrWhiteSpace(c.terminos))
                        {
                            col1.Item().PaddingTop(6).Text(txt =>
                            {
                                txt.Span("Términos y Condiciones: ").Bold().FontSize(8);
                                txt.Span(c.terminos).FontSize(8);
                            });
                        }

                        col1.Item().PaddingTop(60).Element(e => FirmasFooter(e, c));
                    });

                    page.Footer().Height(30).PaddingRight(30).PaddingBottom(10).Element(PiePagina);
                });
            }).GeneratePdf();
        }

        // =====================================================================
        // 3) SERVICIO — estilo John Deere
        // =====================================================================

        private static byte[] GenerarServicio(mdl_Cotizacion_CRM_Imprimir c)
        {
            var modelos = ObtenerModelos(c.detalle);

            return QuestPDF.Fluent.Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.Letter.Portrait());
                    page.Margin(30);
                    page.DefaultTextStyle(t => t.FontFamily(fontFamily).FontSize(9));

                    page.Header().Column(head =>
                    {
                        head.Item().Row(row =>
                        {
                            row.RelativeItem().AlignMiddle().Text("Maquinaria del Humaya, S.A. de C.V.").Bold().FontSize(14);
                            row.ConstantItem(120).AlignRight().Element(e =>
                            {
                                var logo = LeerLogo("Logo_JohnDeere.jpg");
                                if (logo != null) e.Image(logo);
                            });
                        });

                        head.Item().PaddingTop(4).Row(row =>
                        {
                            row.RelativeItem().Height(6).Background("#367C2B");
                            row.RelativeItem().Height(6).Background("#FFDE00");
                        });

                        head.Item().PaddingTop(8).Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text($"Sucursal: {c.sucursal}").Bold();
                                if (!string.IsNullOrWhiteSpace(c.telefono_sucursal))
                                    col.Item().Text(c.telefono_sucursal);
                                col.Item().Text(c.sitio_web).FontColor("#367C2B").Underline();
                            });

                            row.RelativeItem().AlignRight().Column(col =>
                            {
                                col.Item().Text(txt => { txt.Span("Folio de cotización CRM: ").FontSize(8); txt.Span(c.folio_crm).Bold().FontSize(8); });
                                col.Item().Text(txt => { txt.Span("Asesor de ventas: ").FontSize(8); txt.Span(c.asesorventa).Bold().FontSize(8); });
                            });
                        });

                        head.Item().PaddingTop(10).Text(txt =>
                        {
                            txt.Span("Asunto: ").Bold().FontSize(10);
                            txt.Span(c.asunto).Bold().FontSize(10);
                        });
                    });

                    page.Content().PaddingTop(10).Column(col1 =>
                    {
                        col1.Item().Border(1).BorderColor("#000").Padding(6).Row(row =>
                        {
                            row.RelativeItem(3).Element(e => DatosCliente(e, c));
                            row.ConstantItem(90).AlignTop().AlignRight().Text(c.fecha.ToString("dd/MM/yyyy HH:mm")).FontSize(8);
                        });

                        col1.Item().PaddingTop(10)
                            .Text("De acuerdo a su amable solicitud tenemos el gusto de cotizar a usted lo siguiente:");

                        col1.Item().PaddingTop(8).Table(tabla =>
                        {
                            tabla.ColumnsDefinition(c2 =>
                            {
                                c2.ConstantColumn(40);
                                c2.RelativeColumn(3);
                                c2.RelativeColumn(2);
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().BorderBottom(2).BorderColor("#367C2B").Padding(4).Text("CANT.").Bold();
                                header.Cell().BorderBottom(2).BorderColor("#367C2B").Padding(4).Text("DESCRIPCIÓN").Bold();
                                header.Cell().BorderBottom(2).BorderColor("#367C2B").Padding(4).AlignRight().Text("IMPORTE").Bold();
                            });

                            foreach (var m in modelos)
                            {
                                tabla.Cell().BorderBottom(1).BorderColor("#CCC").Padding(4).AlignCenter().Text(m.cantidad.ToString());
                                RenglonDetalle(tabla.Cell().BorderBottom(1).BorderColor("#CCC").Padding(4), m);
                                ImporteDetalle(tabla.Cell().BorderBottom(1).BorderColor("#CCC").Padding(4), m, c.moneda);
                            }
                        });

                        col1.Item().PaddingTop(8).Element(e => TotalesFinales(e, c, modelos));

                        col1.Item().PaddingTop(10)
                            .Text("Estos precios son netos y de contado quedan sujetos a cambio sin previo aviso, prevaleciendo los que estén en vigor al momento de facturar.")
                            .FontSize(8);

                        if (!string.IsNullOrWhiteSpace(c.terminos))
                        {
                            col1.Item().PaddingTop(6).Text(txt =>
                            {
                                txt.Span("Términos y Condiciones: ").Bold().FontSize(8);
                                txt.Span(c.terminos).FontColor("#666").FontSize(8);
                            });
                        }

                        col1.Item().PaddingTop(60).Element(e => FirmasFooter(e, c));
                    });

                    page.Footer().Height(30).PaddingRight(30).PaddingBottom(10).Element(PiePagina);
                });
            }).GeneratePdf();
        }
    }
}