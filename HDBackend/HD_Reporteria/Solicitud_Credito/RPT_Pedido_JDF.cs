using HD.Clientes.Modelos.Pedido_Impresion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace HD_Reporteria.Solicitud_Credito
{
    public class RPT_Pedido_JDF
    {
        public static RPT_Result Generar(mdl_pedido_impresion mdl)
        {
            try
            {
                string fontFamily = "Calibri";
                byte[] doc = Document.Create(document =>
                {
                    document.Page(page =>
                    {

                        //string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        //string imagePath = Path.Combine(desktopPath, "proyecto C#", "Logo.jpg");


                        page.Header().Height(100).Row(row =>
                        {

                            //row.ConstantItem(140).Border(1).Placeholder();
                            row.RelativeItem().PaddingTop(30).Height(47).Background("#477c2c").Row(row2 =>
                            {

                            });

                            row.ConstantColumn(0).Row(row1 =>
                            {
                                var rutaImagen = Path.Combine("C:\\Nube\\HumayaDigital\\HumayaDigitalBackEnd\\HDBackend\\HD_Reporteria\\Imagenes\\Logo.jpg");
                                byte[] imageData = System.IO.File.ReadAllBytes(rutaImagen);
                                row.ConstantItem(100).Image(imageData);

                                row.ConstantColumn(450).PaddingTop(30).Height(47).Background("#477c2c").Row(row2 =>
                                {
                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text("PEDIDO DE MAQUINARIA").FontColor("#fff").FontSize(18).Bold().FontFamily(fontFamily);
                                });
                            });

                        });

                        page.Content().PaddingLeft(30).PaddingRight(30).Column(col1 =>
                        {


                            col1.Item().AlignRight().Row(row1 =>
                            {
                                row1.ConstantItem(120).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Fecha de pedido: ").Bold().FontSize(08).FontFamily(fontFamily);
                                    });
                                });
                                row1.ConstantItem(080).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().AlignBottom().Height(15).Text(txt2 =>
                                    {
                                        string formattedDate = mdl.solicitante.fecha_elaboracion_pedido;
                                        txt2.Span(formattedDate).FontSize(08).FontFamily(fontFamily);
                                    });
                                });
                            });

                            col1.Item().AlignRight().Row(row1 =>
                            {
                                row1.ConstantItem(120).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Unidad de Negocio a Facturar: ").Bold().FontSize(08).FontFamily(fontFamily);
                                    });
                                });
                                row1.ConstantItem(080).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().AlignBottom().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante.unidad_facturar).FontSize(08).FontFamily(fontFamily);
                                    });
                                });
                            });

                            col1.Item().AlignRight().Row(row1 =>
                            {
                                row1.ConstantItem(120).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Numero de cotizacion JDQuote2: ").Bold().FontSize(08).FontFamily(fontFamily);
                                    });
                                });
                                row1.ConstantItem(080).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().AlignBottom().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("").FontSize(08).FontFamily(fontFamily);
                                    });
                                });
                            });

                            col1.Item().AlignRight().Row(row1 =>
                            {
                                row1.ConstantItem(120).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("MONEDA: ").Bold().FontSize(08).FontFamily(fontFamily);
                                    });
                                });
                                row1.ConstantItem(080).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().AlignBottom().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.condiciones.moneda).FontSize(08).FontFamily(fontFamily);
                                    });
                                });
                            });

                            //col1.Item().LineHorizontal(0.5f);


                            col1.Item().PaddingTop(10).Row(row1 =>
                            {
                                row1.ConstantItem(80).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Solicitado por: ").Bold().FontSize(10).FontFamily(fontFamily);
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });

                                row1.ConstantItem(240).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignBottom().Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.solicitante).FontSize(8).FontFamily(fontFamily);
                                    });
                                });

                                row1.ConstantItem(95).PaddingLeft(15).PaddingRight(10).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Contacto Cel: ").Bold().FontSize(10).FontFamily(fontFamily);
                                    });
                                });

                                row1.ConstantItem(120).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.celular).FontSize(8).FontFamily(fontFamily);
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(100).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Encargado de pagos: ").Bold().FontSize(10).FontFamily(fontFamily);
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });

                                row1.ConstantItem(220).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.solicitante).FontSize(8).FontFamily(fontFamily);
                                    });
                                });

                                row1.ConstantItem(95).PaddingLeft(15).PaddingRight(10).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Contacto Cel: ").Bold().FontSize(10).FontFamily(fontFamily);
                                    });
                                });

                                row1.ConstantItem(120).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante.celular).FontSize(8).FontFamily(fontFamily);
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(090).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Lugar de Entrega: ").Bold().FontSize(10).FontFamily(fontFamily);
                                    });
                                });

                                row1.ConstantItem(230).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.lugarentrega).FontSize(8).FontFamily(fontFamily);
                                    });
                                });

                                row1.ConstantItem(95).PaddingLeft(15).PaddingRight(10).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Fecha entrega: ").Bold().FontSize(10).FontFamily(fontFamily);
                                    });
                                });

                                row1.ConstantItem(120).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.fechaentrega).FontSize(8).FontFamily(fontFamily);
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(170).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Condiciones o medidas solicitadas: ").Bold().FontSize(10).FontFamily(fontFamily);
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });

                                row1.ConstantItem(365).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                { 
                                    txt1.Item().Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.condicionescredito).FontSize(8).FontFamily(fontFamily);
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(060).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Facturar a: ").Bold().FontSize(10).FontFamily(fontFamily);
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });

                                row1.ConstantItem(475).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("JOHN DEERE FINANCIAL").FontSize(8).FontFamily(fontFamily);
                                        //txt2.Span(mdl.solicitante?.solicitante).FontSize(8).FontFamily(fontFamily);
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(060).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("RFC: ").Bold().FontSize(10).FontFamily(fontFamily);
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });

                                row1.ConstantItem(475).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("JDC960417VA1").FontSize(8).FontFamily(fontFamily);
                                        //txt2.Span(mdl.solicitante?.rfc).FontSize(8).FontFamily(fontFamily);
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(060).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Domicilio: ").Bold().FontSize(10).FontFamily(fontFamily);
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });

                                row1.ConstantItem(475).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("BLVD. DIAZ ORDAZ NO.500, COL. LA LEONA, SAN PEDRO GARZA GARCIA, NUEVO LEON. C.P. 66210").FontSize(8).FontFamily(fontFamily);
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(100).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Metodo de pago: ").Bold().FontSize(10).FontFamily(fontFamily);
                                    });
                                });

                                row1.ConstantItem(435).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.metodopago).FontSize(8).FontFamily(fontFamily);
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(100).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Forma de pago: ").Bold().FontSize(10).FontFamily(fontFamily);
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });

                                row1.ConstantItem(435).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.formapago).FontSize(8).FontFamily(fontFamily);
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(100).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Uso del CFDI: ").Bold().FontSize(10).FontFamily(fontFamily);
                                    });
                                });

                                row1.ConstantItem(435).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.usocfdi).FontSize(8).FontFamily(fontFamily);
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(100).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Tipo de relacion: ").Bold().FontSize(10).FontFamily(fontFamily);
                                    });
                                });

                                row1.ConstantItem(435).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().PaddingTop(2).Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.tiporelacion).FontSize(8).FontFamily(fontFamily);
                                    });
                                });

                            });


                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(100).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Anticipos: ").Bold().FontSize(10).FontFamily(fontFamily);
                                    });
                                });

                                row1.ConstantItem(200).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.anticipos).FontSize(8).FontFamily(fontFamily);
                                    });
                                });

                                row1.ConstantItem(115).PaddingLeft(15).PaddingRight(10).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Folios de Anticipos: ").Bold().FontSize(10).FontFamily(fontFamily);
                                    });
                                });

                                row1.ConstantItem(120).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.foliosanticipos).FontSize(10).FontFamily(fontFamily);
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(100).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Correo: ").Bold().FontSize(10).FontFamily(fontFamily);
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });

                                row1.ConstantItem(435).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.correoelectronico).FontSize(10).FontFamily(fontFamily);
                                    });
                                });

                            });

                            col1.Item().PaddingVertical(10).Table(tabla =>
                            {
                                tabla.ColumnsDefinition(Columns =>
                                {
                                    Columns.RelativeColumn(0.5f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1.5f);
                                    Columns.RelativeColumn(1.5f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);

                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Cant").FontSize(10).Bold().FontFamily(fontFamily);
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Modelo").FontSize(10).Bold().FontFamily(fontFamily);
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Linea").FontSize(10).Bold().FontFamily(fontFamily);
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Serie").FontSize(10).Bold().FontFamily(fontFamily);
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Precio Lista").FontSize(10).Bold().FontFamily(fontFamily);
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Descuento").FontSize(10).Bold().FontFamily(fontFamily);
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Valor").FontSize(10).Bold().FontFamily(fontFamily);
                                });

                                // Variables para los totales
                                double totalCantidad = 0;
                                double totalPrecio = 0;
                                double totalDescuento = 0;
                                double totalValor = 0;

                                foreach (var item in mdl.unidades)
                                {
                                    var cantidad = Placeholders.Random.Next(1, 10);
                                    var precio = Placeholders.Random.Next(500, 50000);
                                    var descuento = Placeholders.Random.Next(50, 500);
                                    var total = (precio - descuento) * cantidad;
                                    var formattedPrecio = $"{precio:N0}";
                                    var formattedDescuento = $"{descuento:N0}";
                                    var formattedTotal = $"{total:N0}";

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignCenter()
                                    .Text(item.cantidad).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignCenter()
                                   .Text(txt => { txt.Span(item.modelo).FontSize(8).FontFamily(fontFamily); });

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1)
                                   .Text(item.estado).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1)
                                   .Text(item.serie).FontSize(8).FontFamily(fontFamily);

                                    if (item.precio > 0)
                                    {
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                            .Text(item.precio.ToString("N")).FontSize(8).FontFamily(fontFamily);
                                    }
                                    else
                                    {
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                            .Text(item.precio.ToString()).FontSize(8).FontFamily(fontFamily);
                                    }

                                    if (item.descuento > 0)
                                    {
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                            .Text(item.descuento.ToString("N")).FontSize(8).FontFamily(fontFamily);
                                    }
                                    else
                                    {
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                            .Text(item.descuento.ToString()).FontSize(8).FontFamily(fontFamily);
                                    }


                                    if (item.total > 0)
                                    {
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                            .Text(item.total.ToString("N")).FontSize(8).FontFamily(fontFamily);
                                    }
                                    else
                                    {
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                            .Text(item.total.ToString()).FontSize(8).FontFamily(fontFamily);
                                    }

                                    // Actualiza los totales
                                    totalCantidad += item.cantidad;
                                    totalPrecio += item.precio;
                                    totalDescuento += item.descuento;
                                    totalValor += item.total;

                                    // tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                    //.Text($"{formattedTotal}").FontSize(8).FontFamily(fontFamily);
                                }

                                // Agregar el pie de tabla
                                tabla.Footer(footer =>
                                {
                                    footer.Cell().BorderTop(1).BorderColor("#afb69d").Padding(1).AlignCenter()
                                    .Text("Total").FontSize(8).Bold().FontFamily(fontFamily);

                                    footer.Cell().BorderTop(1).BorderColor("#afb69d").Padding(1).AlignCenter().Text("");

                                    footer.Cell().BorderTop(1).BorderColor("#afb69d").Padding(1).AlignCenter().Text("");

                                    footer.Cell().BorderTop(1).BorderColor("#afb69d").Padding(1).AlignCenter().Text("");

                                    footer.Cell().BorderTop(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                    .Text(totalPrecio.ToString("N")).FontSize(8).Bold().FontFamily(fontFamily);

                                    footer.Cell().BorderTop(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                    .Text(totalDescuento.ToString("N")).FontSize(8).Bold().FontFamily(fontFamily);

                                    footer.Cell().BorderTop(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                    .Text(totalValor.ToString("N")).FontSize(8).Bold().FontFamily(fontFamily);
                                });
                            });

                            //col1.Item().Text("Fecha de pedido:").Bold();
                            col1.Item().Text("Condiciones para operacion de venta").FontSize(10).FontFamily(fontFamily);
                            col1.Item().Border(1).BorderColor("#afb69d").Padding(05).Text(mdl.condiciones?.condiciones).FontSize(8).FontFamily(fontFamily);

                            col1.Item().PaddingTop(05).Text("Observaciones").FontSize(10).FontFamily(fontFamily);
                            col1.Item().Border(1).BorderColor("#afb69d").Padding(05).Text(mdl.condiciones?.observaciones).FontSize(8).FontFamily(fontFamily);

                            col1.Item().PaddingTop(05).Row(row1 =>
                            {
                                row1.ConstantItem(120).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Deposito Realizado: ").Bold().FontSize(10).FontFamily(fontFamily);
                                    });
                                });

                                row1.ConstantItem(065).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        // Asegúrate de que mdl.condiciones?.deposito tenga un valor antes de intentar formatearlo
                                        if (mdl.condiciones?.deposito > 0)
                                        {
                                            txt2.Span(mdl.condiciones?.deposito.ToString("N")).FontSize(10).FontFamily(fontFamily);
                                        }
                                        else
                                        {
                                            // Si mdl.condiciones?.deposito es nulo, puedes manejarlo de acuerdo a tus necesidades
                                            txt2.Span("0").FontSize(10).FontFamily(fontFamily);
                                        }
                                    });
                                });


                                row1.ConstantItem(350).AlignRight().Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Las operaciones de contado cuentan con deposito total del valor de venta ").FontSize(10).FontFamily(fontFamily);
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(120).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Tasa anual aplicada: ").Bold().FontSize(10).FontFamily(fontFamily);
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });

                                row1.ConstantItem(065).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.condiciones?.taza.ToString()).FontSize(10).FontFamily(fontFamily);
                                    });
                                });

                                row1.ConstantItem(120).PaddingLeft(15).PaddingRight(10).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Plazo: ").Bold().FontSize(10).FontFamily(fontFamily);
                                    });
                                });

                                row1.ConstantItem(065).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.condiciones?.tiempo_plazo + " " + mdl.condiciones?.plazo).FontSize(10).FontFamily(fontFamily);
                                    });
                                });

                                row1.ConstantItem(100).PaddingLeft(15).PaddingRight(10).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("MHUSA o JDF: ").Bold().FontSize(10).FontFamily(fontFamily);
                                    });
                                });

                                row1.ConstantItem(065).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.condiciones?.mhusajdf).FontSize(10).FontFamily(fontFamily);
                                    });
                                });


                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(120).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Anticipo total: ").Bold().FontSize(10).FontFamily(fontFamily);
                                    });
                                });


                                row1.ConstantItem(065).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        // Asegúrate de que mdl.condiciones?.deposito tenga un valor antes de intentar formatearlo
                                        if (mdl.condiciones?.anticipo > 0)
                                        {
                                            txt2.Span(mdl.condiciones?.anticipo.ToString("N")).FontSize(10).FontFamily(fontFamily);
                                        }
                                        else
                                        {
                                            // Si mdl.condiciones?.deposito es nulo, puedes manejarlo de acuerdo a tus necesidades
                                            txt2.Span("0").FontSize(10).FontFamily(fontFamily);
                                        }
                                    });
                                });

                                row1.ConstantItem(120).PaddingLeft(15).PaddingRight(10).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Gastos y seguros jdf:").Bold().FontSize(10).FontFamily(fontFamily);
                                    });
                                });


                                row1.ConstantItem(065).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        // Asegúrate de que mdl.condiciones?.deposito tenga un valor antes de intentar formatearlo
                                        if (mdl.condiciones?.gastos > 0)
                                        {
                                            txt2.Span(mdl.condiciones?.gastos.ToString("N")).FontSize(10).FontFamily(fontFamily);
                                        }
                                        else
                                        {
                                            // Si mdl.condiciones?.deposito es nulo, puedes manejarlo de acuerdo a tus necesidades
                                            txt2.Span("0").FontSize(10).FontFamily(fontFamily);
                                        }
                                    });
                                });

                                row1.ConstantItem(100).PaddingLeft(15).PaddingRight(10).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Enganche JDF:").Bold().FontSize(10).FontFamily(fontFamily);
                                    });
                                });


                                row1.ConstantItem(065).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        // Asegúrate de que mdl.condiciones?.deposito tenga un valor antes de intentar formatearlo
                                        if (mdl.condiciones?.enganche > 0)
                                        {
                                            txt2.Span(mdl.condiciones?.enganche.ToString("N")).FontSize(10).FontFamily(fontFamily);
                                        }
                                        else
                                        {
                                            // Si mdl.condiciones?.deposito es nulo, puedes manejarlo de acuerdo a tus necesidades
                                            txt2.Span("0").FontSize(10).FontFamily(fontFamily);
                                        }
                                    });
                                });

                            });

                            col1.Item().PaddingVertical(10).Table(tabla =>
                            {
                                tabla.ColumnsDefinition(Columns =>
                                {
                                    Columns.RelativeColumn(0.5f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1.5f);
                                    Columns.RelativeColumn(0.5f);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1.5f);

                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Docto.").FontSize(10).Bold().FontFamily(fontFamily);
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Vencto.").FontSize(10).Bold().FontFamily(fontFamily);
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Impte. A financ.").FontSize(10).Bold().FontFamily(fontFamily);
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Dias").FontSize(10).Bold().FontFamily(fontFamily);
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Tasa %").FontSize(10).Bold().FontFamily(fontFamily);
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Interes").FontSize(10).Bold().FontFamily(fontFamily);
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Total a Pagar").FontSize(10).Bold().FontFamily(fontFamily);
                                });

                                // Variables para los totales
                                double totalImporteFinanciar = 0;
                                double totalInteres = 0;
                                double totalPagar = 0;

                                foreach (var item in mdl.financiamiento)
                                {
                                    var cantidad = Placeholders.Random.Next(1, 10);
                                    var precio = Placeholders.Random.Next(500, 50000);
                                    var descuento = Placeholders.Random.Next(50, 500);
                                    var total = (precio - descuento) * cantidad;
                                    var formattedPrecio = $"{precio:N0}";
                                    var formattedDescuento = $"{descuento:N0}";
                                    var formattedTotal = $"{total:N0}";

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignCenter()
                                    .Text(item.docto).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignCenter()
                                   .Text(item.vencimiento).FontSize(8).FontFamily(fontFamily);

                                    if (item.importefinanciar > 0)
                                    {
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                        .Text(item.importefinanciar.ToString("N")).FontSize(8).FontFamily(fontFamily);
                                        totalImporteFinanciar += item.importefinanciar;
                                    }
                                    else
                                    {
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                        .Text(item.importefinanciar.ToString()).FontSize(8).FontFamily(fontFamily);
                                    }

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                   .Text(item.dias).FontSize(8).FontFamily(fontFamily);

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                   .Text(item.tasa).FontSize(8).FontFamily(fontFamily);

                                    if (item.interes > 0)
                                    {
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                        .Text(item.interes.ToString("N")).FontSize(8).FontFamily(fontFamily);
                                        totalInteres += item.interes;
                                    }
                                    else
                                    {
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                        .Text(item.interes.ToString()).FontSize(8).FontFamily(fontFamily);
                                    }

                                    if (item.totalpagar > 0)
                                    {
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                        .Text(item.totalpagar.ToString("N")).FontSize(8).FontFamily(fontFamily);
                                        totalPagar += item.totalpagar;
                                    }
                                    else
                                    {
                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                        .Text(item.totalpagar.ToString()).FontSize(8).FontFamily(fontFamily);
                                    }
                                }
                                // Agregar el pie de tabla
                                tabla.Footer(footer =>
                                {
                                    footer.Cell().BorderTop(1).BorderColor("#afb69d").Padding(1).AlignCenter().Text("");

                                    footer.Cell().BorderTop(1).BorderColor("#afb69d").Padding(1).AlignCenter()
                                    .Text("Total").FontSize(8).Bold().FontFamily(fontFamily);

                                    footer.Cell().BorderTop(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                    .Text(totalImporteFinanciar.ToString("N")).FontSize(8).Bold().FontFamily(fontFamily);

                                    footer.Cell().BorderTop(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                    .Text(""); // Si no necesitas total para "Dias", deja la celda en blanco.

                                    footer.Cell().BorderTop(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                    .Text(""); // Si no necesitas total para "Tasa %", deja la celda en blanco.

                                    footer.Cell().BorderTop(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                    .Text(totalInteres.ToString("N")).FontSize(8).Bold().FontFamily(fontFamily);

                                    footer.Cell().BorderTop(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                    .Text(totalPagar.ToString("N")).FontSize(8).Bold().FontFamily(fontFamily);
                                });
                            });

                            col1.Item().Text("La tasa es informativa, la tasa real a pagar en cada vencimiento será la que rija al momento de la liquidación del documento o firma de contrato JDF.").FontSize(6).FontFamily(fontFamily);
                            col1.Item().Text("Acepto que se elabore la factura con los datos anteriormente proporcionados, los cuales manifiesto son correctos y acepto que dicha factura no podrá cancelarse o re-facturarse posteriormente.").FontSize(6).FontFamily(fontFamily);
                            col1.Item().Text("Se firma el presente escrito como constancia, para los efectos fiscales y legales que corresponda, quitando como entendido de que no se realizará refacturación alguna. Si por algún motivo requieren cancelación de la misma, se realizará un cobro adicional del 5% por los gastos que dicha cancelación origina.").FontSize(6).FontFamily(fontFamily);
                            col1.Item().Text("Se aceptan condiciones de operacion anteriormente descritas e incluye todo lo acordado en la negociacion, firma de comun acuerdo.").FontSize(6).FontFamily(fontFamily);
                        });

                        page.Footer().Height(120).PaddingLeft(30).PaddingRight(30).Column(col1 =>
                        {
                            col1.Item().PaddingTop(00).Row(row1 =>
                            {
                                row1.ConstantItem(180).AlignCenter().Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Cliente").FontSize(08).FontFamily(fontFamily);
                                    });
                                });


                                row1.ConstantItem(180).AlignCenter().Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Vendedor").FontSize(08).FontFamily(fontFamily);
                                    });
                                });


                                row1.ConstantItem(175).AlignCenter().Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Autoriza").FontSize(08).FontFamily(fontFamily);
                                    });
                                });
                            });

                            col1.Item().PaddingTop(0).Row(row1 =>
                            {
                                row1.ConstantItem(180).BorderBottom(1).Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter()
                                    .AlignBottom()
                                    .Text(txt2 =>
                                    {
                                        txt2.Span(mdl.firmas?.firma_Cliente.ToUpper()).FontSize(10).Bold().FontFamily(fontFamily);
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });


                                row1.ConstantItem(180).PaddingLeft(15).BorderBottom(1).Column(txt1 =>
                                {
                                    txt1.Item()
                                    .AlignCenter()
                                    .AlignBottom()
                                    .Text(txt2 =>
                                    {
                                        txt2.Span(mdl.firmas?.firma_vendedor.ToUpper()).FontSize(10).Bold().FontFamily(fontFamily);
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });


                                row1.ConstantItem(175).PaddingLeft(15).BorderBottom(1).Column(txt1 =>
                                {
                                    txt1.Item()
                                    .AlignCenter()
                                    .AlignBottom()
                                    .Text(txt2 =>
                                    {
                                        txt2.Span(mdl.firmas?.firma_autoriza.ToUpper()).FontSize(10).Bold().FontFamily(fontFamily);
                                    });
                                });
                            });

                            col1.Item().PaddingTop(00).Row(row1 =>
                            {
                                row1.ConstantItem(180).AlignCenter().Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Nombre y Firma").FontSize(08).FontFamily(fontFamily);
                                    });
                                });


                                row1.ConstantItem(180).AlignCenter().Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Nombre y Firma").FontSize(08).FontFamily(fontFamily);
                                    });
                                });


                                row1.ConstantItem(175).AlignCenter().Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Nombre y Firma").FontSize(08).FontFamily(fontFamily);
                                    });
                                });
                            });
                        });

                    });
                }).GeneratePdf();
                RPT_Result result = new RPT_Result();
                result.extension = "pdf";
                result.nombredocumento = "Pedido maquinaria";
                result.documento = Convert.ToBase64String(doc);
                return result;


            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
    }
}
