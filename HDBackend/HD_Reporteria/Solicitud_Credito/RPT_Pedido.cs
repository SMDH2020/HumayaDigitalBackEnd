using HD.Clientes.Modelos;
using HD.Clientes.Modelos.Pedido_Impresion;
using HD.Clientes.Modelos.Solicitud_Impresion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
namespace HD_Reporteria.Solicitud_Credito
{
    public class RPT_Pedido
    {
        public static RPT_Result Generar(mdl_pedido_impresion mdl)
        {
            try
            {
                byte[] doc = Document.Create(document =>
                {
                    document.Page(page =>
                    {

                        //string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        //string imagePath = Path.Combine(desktopPath, "proyecto C#", "Logo.jpg");


                        page.Header().Height(120).Row(row =>
                        {

                            //row.ConstantItem(140).Border(1).Placeholder();
                            row.RelativeItem().PaddingTop(35).Height(50).Background("#477c2c").Row(row2 =>
                            {

                            });

                            row.ConstantColumn(0).Row(row1 =>
                            {
                                var rutaImagen = Path.Combine("C:\\Nube\\HumayaDigital\\HumayaDigitalBackEnd\\HDBackend\\HD_Reporteria\\Imagenes\\Logo.jpg");
                                byte[] imageData = System.IO.File.ReadAllBytes(rutaImagen);
                                row.ConstantItem(120).Image(imageData);

                                row.ConstantColumn(450).PaddingTop(35).Height(50).Background("#477c2c").Row(row2 =>
                                {
                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text("PEDIDO DE MAQUINARIA").FontColor("#fff").FontSize(20).Bold().FontFamily("roboto");
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
                                        txt2.Span("Fecha de pedido: ").Bold().FontSize(08).FontFamily("roboto");
                                    });
                                });
                                row1.ConstantItem(080).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("14-11-2023").FontSize(08).FontFamily("roboto");
                                    });
                                });
                            });

                            col1.Item().AlignRight().Row(row1 =>
                            {
                                row1.ConstantItem(120).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Unidad de Negocio a Facturar: ").Bold().FontSize(08).FontFamily("roboto");
                                    });
                                });
                                row1.ConstantItem(080).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("NAVOLATO").FontSize(08).FontFamily("roboto");
                                    });
                                });
                            });

                            col1.Item().AlignRight().Row(row1 =>
                            {
                                row1.ConstantItem(120).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Numero de cotizacion JDQuote2: ").Bold().FontSize(08).FontFamily("roboto");
                                    });
                                });
                                row1.ConstantItem(080).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("").FontSize(08).FontFamily("roboto");
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
                                        txt2.Span("Solicitado por: ").Bold().FontSize(10).FontFamily("roboto");
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });

                                row1.ConstantItem(240).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.solicitante).FontSize(8).FontFamily("roboto");
                                    });
                                });

                                row1.ConstantItem(95).PaddingLeft(15).PaddingRight(10).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Contacto Cel: ").Bold().FontSize(10).FontFamily("roboto");
                                    });
                                });

                                row1.ConstantItem(120).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.celular).FontSize(8).FontFamily("roboto");
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(100).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Encargado de pagos: ").Bold().FontSize(10).FontFamily("roboto");
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });

                                row1.ConstantItem(220).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.solicitante).FontSize(8).FontFamily("roboto");
                                    });
                                });

                                row1.ConstantItem(95).PaddingLeft(15).PaddingRight(10).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Contacto Cel: ").Bold().FontSize(10).FontFamily("roboto");
                                    });
                                });

                                row1.ConstantItem(120).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante.celular).FontSize(8).FontFamily("roboto");
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(090).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Lugar de Entrega: ").Bold().FontSize(10).FontFamily("roboto");
                                    });
                                });

                                row1.ConstantItem(230).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.lugarentrega).FontSize(8).FontFamily("roboto");
                                    });
                                });

                                row1.ConstantItem(95).PaddingLeft(15).PaddingRight(10).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Fecha entrega: ").Bold().FontSize(10).FontFamily("roboto");
                                    });
                                });

                                row1.ConstantItem(120).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.fechaentrega).FontSize(8).FontFamily("roboto");
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(170).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Condiciones o medidas solicitadas: ").Bold().FontSize(10).FontFamily("roboto");
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });

                                row1.ConstantItem(365).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.condicionescredito).FontSize(8).FontFamily("roboto");
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(060).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Facturar a: ").Bold().FontSize(10).FontFamily("roboto");
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });

                                row1.ConstantItem(475).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.solicitante).FontSize(8).FontFamily("roboto");
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(060).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("RFC: ").Bold().FontSize(10).FontFamily("roboto");
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });

                                row1.ConstantItem(475).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.rfc).FontSize(8).FontFamily("roboto");
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(060).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Domicilio: ").Bold().FontSize(10).FontFamily("roboto");
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });

                                row1.ConstantItem(475).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.domicilio).FontSize(8).FontFamily("roboto");
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(100).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Metodo de pago: ").Bold().FontSize(10).FontFamily("roboto");
                                    });
                                });

                                row1.ConstantItem(435).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.metodopago).FontSize(8).FontFamily("roboto");
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(100).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Forma de pago: ").Bold().FontSize(10).FontFamily("roboto");
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });

                                row1.ConstantItem(435).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.formapago).FontSize(8).FontFamily("roboto");
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(100).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Uso del CFDI: ").Bold().FontSize(10).FontFamily("roboto");
                                    });
                                });

                                row1.ConstantItem(435).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.usocfdi).FontSize(8).FontFamily("roboto");
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(100).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Tipo de relacion: ").Bold().FontSize(10).FontFamily("roboto");
                                    });
                                });

                                row1.ConstantItem(435).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().PaddingTop(2).Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.tiporelacion).FontSize(8).FontFamily("roboto");
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(100).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Anticipos: ").Bold().FontSize(10).FontFamily("roboto");
                                    });
                                });

                                row1.ConstantItem(200).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.anticipos).FontSize(8).FontFamily("roboto");
                                    });
                                });

                                row1.ConstantItem(115).PaddingLeft(15).PaddingRight(10).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Folios de Anticipos: ").Bold().FontSize(10).FontFamily("roboto");
                                    });
                                });

                                row1.ConstantItem(120).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.foliosanticipos).FontSize(10).FontFamily("roboto");
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(100).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Correo: ").Bold().FontSize(10).FontFamily("roboto");
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });

                                row1.ConstantItem(435).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.solicitante?.correoelectronico).FontSize(10).FontFamily("roboto");
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
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);
                                    Columns.RelativeColumn(1);

                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Cant").FontSize(10).Bold().FontFamily("roboto");
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Modelo").FontSize(10).Bold().FontFamily("roboto");
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Estado").FontSize(10).Bold().FontFamily("roboto");
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Serie").FontSize(10).Bold().FontFamily("roboto");
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Preio Lista").FontSize(10).Bold().FontFamily("roboto");
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Descuento").FontSize(10).Bold().FontFamily("roboto");
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Valor").FontSize(10).Bold().FontFamily("roboto");
                                });

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
                                    .Text(item.cantidad).FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignCenter()
                                   .Text(txt=> { txt.Span(item.modelo).FontSize(8).FontFamily("roboto"); });

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1)
                                   .Text(item.estado).FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1)
                                   .Text(item.serie).FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                   .Text(item.precio).FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                   .Text(item.descuento).FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                   .Text(item.total).FontSize(8).FontFamily("roboto");

                                   // tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                   //.Text($"{formattedTotal}").FontSize(8).FontFamily("roboto");
                                }
                            });
                            //col1.Item().Text("Fecha de pedido:").Bold();
                            col1.Item().Text("Condiciones para operacion de venta").FontSize(10).FontFamily("roboto");
                            col1.Item().Border(1).BorderColor("#afb69d").Padding(05).Text("Acepto que se elabore la factura con los datos anteriormente proporcionados, los cuales manifiesto son correctos y acepto que dicha factura no podrá cancelarse o re-facturarse posteriormente.").FontSize(8).FontFamily("roboto");

                            col1.Item().PaddingTop(05).Text("Observaciones").FontSize(10).FontFamily("roboto");
                            col1.Item().Border(1).BorderColor("#afb69d").Padding(05).Text(mdl.condiciones?.observaciones).FontSize(8).FontFamily("roboto");

                            col1.Item().PaddingTop(05).Row(row1 =>
                            {
                                row1.ConstantItem(120).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Deposito Realizado: ").Bold().FontSize(10).FontFamily("roboto");
                                    });
                                });

                                row1.ConstantItem(065).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.condiciones?.deposito.ToString()).FontSize(10).FontFamily("roboto");

                                    });
                                });

                                row1.ConstantItem(350).AlignRight().Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Las operaciones de contado cuentan con deposito total del valor de venta ").FontSize(10).FontFamily("roboto");
                                    });
                                });

                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(120).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Tasa anual aplicada: ").Bold().FontSize(10).FontFamily("roboto");
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });

                                row1.ConstantItem(065).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.condiciones?.taza.ToString()).FontSize(10).FontFamily("roboto");
                                    });
                                });

                                row1.ConstantItem(120).PaddingLeft(15).PaddingRight(10).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Plazo: ").Bold().FontSize(10).FontFamily("roboto");
                                    });
                                });

                                row1.ConstantItem(065).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.condiciones?.plazo).FontSize(10).FontFamily("roboto");
                                    });
                                });

                                row1.ConstantItem(100).PaddingLeft(15).PaddingRight(10).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("MHUSA o JDF: ").Bold().FontSize(10).FontFamily("roboto");
                                    });
                                });

                                row1.ConstantItem(065).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.condiciones?.mhusajdf).FontSize(10).FontFamily("roboto");
                                    });
                                });


                            });

                            col1.Item().PaddingTop(02).Row(row1 =>
                            {
                                row1.ConstantItem(120).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Anticipo total: ").Bold().FontSize(10).FontFamily("roboto");
                                    });
                                });

                                row1.ConstantItem(065).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.condiciones?.anticipo.ToString()).FontSize(10).FontFamily("roboto");
                                    });
                                });

                                row1.ConstantItem(120).PaddingLeft(15).PaddingRight(10).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Gastos y seguros jdf:").Bold().FontSize(10).FontFamily("roboto");
                                    });
                                });

                                row1.ConstantItem(065).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.condiciones?.gastos.ToString()).FontSize(10).FontFamily("roboto");
                                    });
                                });

                                row1.ConstantItem(100).PaddingLeft(15).PaddingRight(10).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Enganche JDF:").Bold().FontSize(10).FontFamily("roboto");
                                    });
                                });

                                row1.ConstantItem(065).BorderBottom(1).BorderColor("#afb69d").Column(txt1 =>
                                {
                                    txt1.Item().AlignCenter().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span(mdl.condiciones?.enganche.ToString()).FontSize(10).FontFamily("roboto");
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
                                    .Padding(1).Text("Docto.").FontSize(10).Bold().FontFamily("roboto");
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Vencto.").FontSize(10).Bold().FontFamily("roboto");
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Impte. A financ.").FontSize(10).Bold().FontFamily("roboto");
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Dias").FontSize(10).Bold().FontFamily("roboto");
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Tasa %").FontSize(10).Bold().FontFamily("roboto");
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Interes").FontSize(10).Bold().FontFamily("roboto");
                                    header.Cell().Background("#ccc").AlignCenter()
                                    .Padding(1).Text("Total a Pagar").FontSize(10).Bold().FontFamily("roboto");
                                });

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
                                    .Text(item.docto).FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignCenter()
                                   .Text(item.vencimiento).FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                   .Text(item.importefinanciar).FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                   .Text(item.dias).FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                   .Text(item.tasa).FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                   .Text(item.interes).FontSize(8).FontFamily("roboto");

                                    tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Padding(1).AlignRight()
                                   .Text(item.totalpagar).FontSize(8).FontFamily("roboto");
                                }
                            });

                            col1.Item().Text("La tasa es informativa, la tasa real a pagar en cada vencimiento será la que rija al momento de la liquidación del documento o firma de contrato JDF.").FontSize(8).FontFamily("roboto");
                            col1.Item().Text("Acepto que se elabore la factura con los datos anteriormente proporcionados, los cuales manifiesto son correctos y acepto que dicha factura no podrá cancelarse o re-facturarse posteriormente.").FontSize(8).FontFamily("roboto");
                            col1.Item().Text("Se firma el presente escrito como constancia, para los efectos fiscales y legales que corresponda, quitando como entendido de que no se realizará refacturación alguna. Si por algún motivo requieren cancelación de la misma, se realizará un cobro adicional del 5% por los gastos que dicha cancelación origina.").FontSize(8).FontFamily("roboto");
                            col1.Item().Text("Se aceptan condiciones de operacion anteriormente descritas e incluye todo lo acordado en la negociacion, firma de comun acuerdo.").FontSize(8).FontFamily("roboto");
                        });

                        page.Footer().Height(80).PaddingLeft(30).PaddingRight(30).Column(col1 =>
                        {
                            col1.Item().PaddingTop(00).Row(row1 =>
                            {
                                row1.ConstantItem(180).AlignCenter().Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Cliente").FontSize(08).FontFamily("roboto");
                                    });
                                });


                                row1.ConstantItem(180).AlignCenter().Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Vendedor").FontSize(08).FontFamily("roboto");
                                    });
                                });


                                row1.ConstantItem(175).AlignCenter().Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Autoriza").FontSize(08).FontFamily("roboto");
                                    });
                                });
                            });

                            col1.Item().PaddingTop(15).Row(row1 =>
                            {
                                row1.ConstantItem(180).BorderBottom(1).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).AlignCenter().Text(txt2 =>
                                    {
                                        txt2.Span(mdl.firmas?.firma_Cliente).FontSize(10).Bold().FontFamily("roboto");
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });


                                row1.ConstantItem(180).PaddingLeft(15).BorderBottom(1).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).AlignCenter().Text(txt2 =>
                                    {
                                        txt2.Span(mdl.firmas?.firma_vendedor).FontSize(10).Bold().FontFamily("roboto");
                                        //txt2.Span("NAVOLATO").FontSize(10);
                                    });
                                });


                                row1.ConstantItem(175).PaddingLeft(15).BorderBottom(1).Column(txt1 =>
                                {
                                    txt1.Item().Height(15).AlignCenter().Text(txt2 =>
                                    {
                                        txt2.Span(mdl.firmas?.firma_autoriza).FontSize(10).Bold().FontFamily("roboto");
                                    });
                                });
                            });

                            col1.Item().PaddingTop(00).Row(row1 =>
                            {
                                row1.ConstantItem(180).AlignCenter().Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Nombre y Firma").FontSize(08).FontFamily("roboto");
                                    });
                                });


                                row1.ConstantItem(180).AlignCenter().Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Nombre y Firma").FontSize(08).FontFamily("roboto");
                                    });
                                });


                                row1.ConstantItem(175).AlignCenter().Column(txt1 =>
                                {
                                    txt1.Item().Height(15).Text(txt2 =>
                                    {
                                        txt2.Span("Nombre y Firma").FontSize(08).FontFamily("roboto");
                                    });
                                });
                            });
                        });

                    });
                }).GeneratePdf();
                RPT_Result result = new RPT_Result();
                result.extension = "PDF";
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
