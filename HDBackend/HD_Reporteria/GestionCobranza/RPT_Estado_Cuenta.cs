using HD_Cobranza.GestionCobranza.Modelos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using DocumentFormat.OpenXml.Spreadsheet;

namespace HD_Reporteria.GestionCobranza
{
    public class RPT_Estado_Cuenta
    {
        public static RPT_Result GenerarEstadoCuenta(IEnumerable<mdl_Facturas_Estado_Cuenta> mdl)
        {
            try
            {
                string fechaActual = DateTime.Now.ToString("dd/MMM/yyyy", new System.Globalization.CultureInfo("es-ES"));
                string fontFamily = "Calibri";
                var rutaImagenQR = mdl.FirstOrDefault().ADR == 2
                ? Path.Combine("C:\\Nube\\HumayaDigital\\HumayaDigitalBackEnd\\HDBackend\\HD_Reporteria\\Imagenes\\QRNayarit.png")
                : Path.Combine("C:\\Nube\\HumayaDigital\\HumayaDigitalBackEnd\\HDBackend\\HD_Reporteria\\Imagenes\\QRSinaloa.png");

                var telefono = mdl.FirstOrDefault().ADR == 2 ? "Tel. (311) 341 4978" : "Tel. (667) 502 3527";

                var extension = mdl.FirstOrDefault().ADR == 2 ? "Ext. 8511" : "Ext. 8111";
                byte[] doc = Document.Create(document =>
                {
                    document.Page(page =>
                    {
                        var registrosRefaccionesYTaller = mdl.Where(x => x.descripcion.Contains("Refacciones") || x.descripcion.Contains("Taller"));
                        var registrosOtros = mdl.Where(x => !x.descripcion.Contains("Refacciones") && !x.descripcion.Contains("Taller"));
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
                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text("ESTADO DE CUENTA").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
                                });

                            });

                        });

                            page.Content().PaddingTop(3).PaddingLeft(30).PaddingRight(30).Column(col1 =>
                            {

                            // Agrupar por sucursal y obtener todas las sucursales
                            var sucursales = registrosRefaccionesYTaller.GroupBy(x => x.idsucursal).ToList();

                            // Mostrar la tabla para la primera sucursal
                            var primeraSucursal = sucursales.FirstOrDefault()?.Key;
                            var registrosPrimeraSucursal = sucursales.FirstOrDefault()?.ToList();

                            col1.Item().PaddingBottom(2).Row(row =>
                            {
                                // Columna 1: "Cliente"
                                row.ConstantItem(340).Column(col =>
                                {
                                    col.Item().AlignLeft().Height(15).Text(txt =>
                                    {
                                        txt.Span("Cliente: ").FontSize(10).FontFamily("arial");
                                    });

                                    col.Item().PaddingTop(2).AlignLeft().Height(20).Text(txt =>
                                    {
                                        txt.Span("(" + registrosRefaccionesYTaller.FirstOrDefault().idcliente + ") " + registrosRefaccionesYTaller.FirstOrDefault().razonsocial)
                                           .FontSize(10)
                                           .FontFamily("arial")
                                           .Bold();
                                    });
                                });

                                row.ConstantItem(190).Column(col =>
                                {
                                    col.Item().AlignLeft().Height(15).Text(txt =>
                                    {
                                        txt.Span("MAQUINARIA DEL HUMAYA").FontSize(10).FontFamily("arial").Bold();
                                    });

                                    col.Item().PaddingTop(2).AlignLeft().Height(30).Text(txt =>
                                    {
                                        txt.Span("Carret. Navolato-Culiacan #1185 ote. San Pedro de rosales")
                                           .FontSize(10)
                                           .FontFamily("arial");
                                    });
                                });
                            });

                            col1.Item().Text(txt =>
                            {
                                txt.Span("Información al: ").Bold().FontSize(8); 
                                txt.Span(fechaActual).FontSize(8);
                                txt.Span("                  Plazo 60 dias").FontSize(8);
                            });

                            col1.Item().LineHorizontal(0.01f).LineColor("#D3D3D3");

                                if (registrosRefaccionesYTaller.Any())
                                {
                                    double sumaImporteTotal = 0;

                                    foreach (var item in registrosRefaccionesYTaller)
                                    {
                                        double importetotal = item.saldo + item.interes_moratorio + item.interes_pactado;
                                        sumaImporteTotal += importetotal;
                                    };

                                    double sumaImporteTotalPrimeraSucursal = 0;

                                    foreach (var item in registrosPrimeraSucursal)
                                    {
                                        double importetotal = item.saldo + item.interes_moratorio + item.interes_pactado;
                                        sumaImporteTotalPrimeraSucursal += importetotal;
                                    };

                                    //col1.Item().Text("Línea: Refacciones y Taller        Total $ " + sumaImporteTotal.ToString("N2")).Bold().FontSize(10);
                                    col1.Item().Text(txt =>
                                    {
                                        txt.Span("Línea: ").Bold().FontSize(10);
                                        txt.Span("Refacciones y Taller        ").FontSize(10);
                                        txt.Span("Total $ ").Bold().FontSize(10);
                                        txt.Span(sumaImporteTotal.ToString("N2")).FontSize(10);
                                    });

                                    col1.Item().LineHorizontal(0.01f).LineColor("#D3D3D3");

                                    //col1.Item().Text("Sucursal: " + mdl.FirstOrDefault().sucursal).Bold().FontSize(10);
                                    col1.Item().Text(txt =>
                                    {
                                        txt.Span("Sucursal: ").Bold().FontSize(10);
                                        txt.Span(registrosRefaccionesYTaller.FirstOrDefault().sucursal).FontSize(10);
                                    });

                                    col1.Item().PaddingVertical(4).Table(tabla =>
                                    {
                                        tabla.ColumnsDefinition(Columns =>
                                        {
                                            Columns.RelativeColumn(1f);
                                            Columns.RelativeColumn(1f);
                                            Columns.RelativeColumn(1f);
                                            Columns.RelativeColumn(1.5f);
                                            Columns.RelativeColumn(1.4f);
                                            Columns.RelativeColumn(1.4f);
                                            Columns.RelativeColumn(1.4f);
                                            Columns.RelativeColumn(1.4f);
                                            Columns.RelativeColumn(1.4f);
                                            Columns.RelativeColumn(1.4f);
                                        });

                                        tabla.Header(header =>
                                        {
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("FECHA").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("VENC.").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("DIAS VDOS.").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("DOCUMENTO").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("SERIE").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("DESCRIPCIÓN").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("CARGO").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("ABONOS").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("INTERESES").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("SALDO").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        });

                                        foreach (var item in registrosPrimeraSucursal)
                                        {

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                            .Text($"{item.fecha}").FontSize(8).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                           .Text(item.vencimiento).FontSize(8).FontFamily(fontFamily);

                                            if (item.diasvencido < 0)
                                            {
                                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                               .Text("").FontSize(8).FontFamily(fontFamily);
                                            }
                                            else
                                            {
                                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                                .Text(item.diasvencido).FontSize(8).FontFamily(fontFamily);
                                            }

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                           .Text(item.documento).FontSize(8).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                           .Text(item.serie_fiscal + " " + item.folio_fiscal).FontSize(8).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                           .Text(item.descripcion).FontSize(8).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignRight()
                                           .Text($"{item.importefactura.ToString("N2")}").FontSize(8).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignRight()
                                            .Text(item.importepagado.ToString("N2")).FontSize(8).FontFamily(fontFamily);


                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignRight()
                                            .Text((item.interes_moratorio + item.interes_pactado).ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                            double importetotal = item.saldo + item.interes_moratorio + item.interes_pactado;

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignRight()
                                            .Text(importetotal.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        }
                                        tabla.Footer(footer =>
                                        {
                                            double sumaInteresesTotal = (registrosPrimeraSucursal.Sum(item => item.interes_pactado)) + (registrosPrimeraSucursal.Sum(item => item.interes_moratorio));
                                            double sumasaldoTotal = registrosPrimeraSucursal.Sum(item => item.saldo);
                                            double sumaImportePagadoTotal = registrosPrimeraSucursal.Sum(item => item.importepagado);
                                            double sumaImporteFacturaTotal = registrosPrimeraSucursal.Sum(item => item.importefactura);

                                            footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                            footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                            footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                            footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                            footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                            footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignLeft().Text("TOTAL").FontSize(8).FontFamily("arial").Bold();
                                            footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignRight().Text(sumaImporteFacturaTotal.ToString("N2")).FontSize(8).FontFamily("arial").Bold();
                                            footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignRight().Text(sumaImportePagadoTotal.ToString("N2")).FontSize(8).FontFamily("arial").Bold();
                                            footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignRight().Text(sumaInteresesTotal.ToString("N2")).FontSize(8).FontFamily("arial").Bold();
                                            footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignRight().Text(sumaImporteTotalPrimeraSucursal.ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();
                                        }



                                        );

                                        if (sucursales.Count > 1)
                                        {
                                            foreach (var sucursal in sucursales.Skip(1))
                                            {
                                                var registrosSucursal = sucursal.ToList();

                                                // Mostrar la tabla para la sucursal diferente
                                                col1.Item().PaddingTop(10).Text(txt =>
                                                {
                                                    txt.Span("Sucursal: ").Bold().FontSize(10);
                                                    txt.Span(registrosSucursal.FirstOrDefault()?.sucursal).FontSize(10);
                                                });

                                                double sumaImporteTotalsucursal = 0;

                                                foreach (var item in registrosSucursal)
                                                {
                                                    double importetotal = item.saldo + item.interes_moratorio + item.interes_pactado;
                                                    sumaImporteTotalsucursal += importetotal;
                                                };

                                                col1.Item().PaddingVertical(4).Table(tabla =>
                                                {
                                                    tabla.ColumnsDefinition(Columns =>
                                                    {
                                                        Columns.RelativeColumn(1f);
                                                        Columns.RelativeColumn(1f);
                                                        Columns.RelativeColumn(1f);
                                                        Columns.RelativeColumn(1.5f);
                                                        Columns.RelativeColumn(1.4f);
                                                        Columns.RelativeColumn(1.4f);
                                                        Columns.RelativeColumn(1.4f);
                                                        Columns.RelativeColumn(1.4f);
                                                        Columns.RelativeColumn(1.4f);
                                                        Columns.RelativeColumn(1.4f);
                                                    });

                                                    tabla.Header(header =>
                                                    {
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("FECHA").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("VENC.").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("DIAS VDOS.").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("DOCUMENTO").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("SERIE").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("DESCRIPCIÓN").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("CARGO").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("ABONOS").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("INTERESES").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("SALDO").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                    });

                                                    foreach (var item in registrosSucursal)
                                                    {

                                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                                        .Text($"{item.fecha}").FontSize(8).FontFamily(fontFamily);

                                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                                       .Text(item.vencimiento).FontSize(8).FontFamily(fontFamily);

                                                        if (item.diasvencido < 0)
                                                        {
                                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                                           .Text("").FontSize(8).FontFamily(fontFamily);
                                                        }
                                                        else
                                                        {
                                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                                            .Text(item.diasvencido).FontSize(8).FontFamily(fontFamily);
                                                        }

                                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                                       .Text(item.documento).FontSize(8).FontFamily(fontFamily);

                                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                                       .Text(item.serie_fiscal + " " + item.folio_fiscal).FontSize(8).FontFamily(fontFamily);

                                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                                       .Text(item.descripcion).FontSize(8).FontFamily(fontFamily);

                                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignRight()
                                                       .Text($"{item.importefactura.ToString("N2")}").FontSize(8).FontFamily(fontFamily);

                                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignRight()
                                                        .Text(item.importepagado.ToString("N2")).FontSize(8).FontFamily(fontFamily);


                                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignRight()
                                                        .Text((item.interes_moratorio + item.interes_pactado).ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                                        double importetotal = item.saldo + item.interes_moratorio + item.interes_pactado;

                                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignRight()
                                                        .Text(importetotal.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                                    }
                                                    tabla.Footer(footer =>
                                                    {
                                                        double sumaInteresesTotalSucursal = (registrosSucursal.Sum(item => item.interes_pactado)) + (registrosSucursal.Sum(item => item.interes_moratorio));
                                                        double sumasaldoTotalSucursal = registrosSucursal.Sum(item => item.saldo);
                                                        double sumaImportePagadoTotalSucursal = registrosSucursal.Sum(item => item.importepagado);
                                                        double sumaImporteFacturaTotalSucursal = registrosSucursal.Sum(item => item.importefactura);

                                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignLeft().Text("TOTAL").FontSize(8).FontFamily("arial").Bold();
                                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignRight().Text(sumaImporteFacturaTotalSucursal.ToString("N2")).FontSize(8).FontFamily("arial").Bold();
                                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignRight().Text(sumaImportePagadoTotalSucursal.ToString("N2")).FontSize(8).FontFamily("arial").Bold();
                                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignRight().Text(sumaInteresesTotalSucursal.ToString("N2")).FontSize(8).FontFamily("arial").Bold();
                                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignRight().Text(sumaImporteTotalsucursal.ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();
                                                    });
                                                });
                                            }
                                        }
                                    });
                                    col1.Item().PaddingTop(10).Table(tablaCliente =>
                                    {
                                        var referenciasUnicas = registrosRefaccionesYTaller.Select(x => x.referencia).Distinct();
                                        var registrosVencidos1a30 = registrosRefaccionesYTaller.Where(x => x.diasvencido >= 1 && x.diasvencido <= 30);
                                        var registrosVencidos31a60 = registrosRefaccionesYTaller.Where(x => x.diasvencido >= 31 && x.diasvencido <= 60);
                                        var registrosVencidos61a90 = registrosRefaccionesYTaller.Where(x => x.diasvencido >= 61 && x.diasvencido <= 90);
                                        var registrosVencidosmas90 = registrosRefaccionesYTaller.Where(x => x.diasvencido >= 91);
                                        var registrosporVencer = registrosRefaccionesYTaller.Where(x => x.diasvencido <= 0);

                                        double sumaVencidos1a30 = registrosVencidos1a30.Sum(x => x.saldo);
                                        double sumaVencidos31a60 = registrosVencidos31a60.Sum(x => x.saldo);
                                        double sumaVencidos61a90 = registrosVencidos61a90.Sum(x => x.saldo);
                                        double sumaVencidosmas90 = registrosVencidosmas90.Sum(x => x.saldo);
                                        double sumaporVencer = registrosporVencer.Sum(x => x.saldo);



                                        double sumaVencidoTotal = sumaVencidos1a30 + sumaVencidos31a60 + sumaVencidos61a90 + sumaVencidosmas90;
                                        double sumaVencidoporVencer = sumaporVencer + sumaVencidoTotal;
                                        double sumaTotalPagado = registrosRefaccionesYTaller.Sum(x => x.importepagado);

                                        tablaCliente.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn(1f);
                                            columns.RelativeColumn(1f);
                                            columns.RelativeColumn(1f);
                                            columns.RelativeColumn(1f);
                                            columns.RelativeColumn(1f);
                                        });

                                        tablaCliente.Header(header =>
                                        {
                                            header.Cell().Border(1).BorderColor("#D3D3D3").Background("#275027").AlignCenter().AlignMiddle()
                                                .Padding(1).Text("NÚMERO DE REFERENCIA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Border(1).BorderColor("#D3D3D3").AlignCenter().AlignMiddle()
                                                  .Padding(1).Text(string.Join("\n", referenciasUnicas)).FontSize(8).FontFamily(fontFamily).FontColor("#000");
                                            header.Cell().Border(1).BorderColor("#D3D3D3").Background("#275027").AlignCenter().AlignMiddle()
                                                .Padding(1).Text("NOMBRE DEL CLIENTE").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().ColumnSpan(2).Border(1).BorderColor("#D3D3D3").AlignCenter().AlignMiddle()
                                                .Padding(1).Text(registrosRefaccionesYTaller.FirstOrDefault().razonsocial).FontSize(8).FontFamily(fontFamily).FontColor("#000");
                                        });

                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO DE UNO A 30 DIAS\n" + sumaVencidos1a30.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO DE 31 A 60 DIAS\n" + sumaVencidos31a60.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO DE 61 A 90 DIAS\n" + sumaVencidos61a90.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO MÁS DE 90 DIAS\n" + sumaVencidosmas90.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("TOTAL VENCIDO\n" + sumaVencidoTotal.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("POR VENCER\n" + sumaporVencer.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO Y POR VENCER\n" + sumaVencidoporVencer.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("SALDO A FAVOR\n" + "0").FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("SALDO NETO + INTS.\n" + sumaImporteTotal.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("TOTAL PAGADO\n" + sumaTotalPagado.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                    });
                                }

                                if (registrosOtros.Any() && registrosRefaccionesYTaller.Any())
                                {
                                    col1.Item().PageBreak();
                                }

                                if (registrosOtros.Any())
                                {
                                    var sucursalesOtros = registrosOtros.GroupBy(x => x.idsucursal).ToList();

                                    // Mostrar la tabla para la primera sucursal
                                    var primeraSucursalOtros = sucursalesOtros.FirstOrDefault()?.Key;
                                    var registrosPrimeraSucursalOtros = sucursalesOtros.FirstOrDefault()?.ToList();

                                    double sumaImporteTotal = 0;

                                    foreach (var item in registrosOtros)
                                    {
                                        double importetotal = item.saldo + item.interes_moratorio + item.interes_pactado;
                                        sumaImporteTotal += importetotal;
                                    };

                                    double sumaImporteTotalPrimeraSucursal = 0;

                                    foreach (var item in registrosPrimeraSucursalOtros)
                                    {
                                        double importetotal = item.saldo + item.interes_moratorio + item.interes_pactado;
                                        sumaImporteTotalPrimeraSucursal += importetotal;
                                    };

                                    //col1.Item().Text("Línea: Refacciones y Taller        Total $ " + sumaImporteTotal.ToString("N2")).Bold().FontSize(10);
                                    col1.Item().PaddingTop(10).Text(txt =>
                                    {
                                        txt.Span("Línea: ").Bold().FontSize(10);
                                        txt.Span("Maquinaria y Otras Líneas       ").FontSize(10);
                                        txt.Span("Total $ ").Bold().FontSize(10);
                                        txt.Span(sumaImporteTotal.ToString("N2")).FontSize(10);
                                    });

                                    col1.Item().LineHorizontal(0.01f).LineColor("#D3D3D3");

                                    //col1.Item().Text("Sucursal: " + mdl.FirstOrDefault().sucursal).Bold().FontSize(10);
                                    col1.Item().Text(txt =>
                                    {
                                        txt.Span("Sucursal: ").Bold().FontSize(10);
                                        txt.Span(registrosOtros.FirstOrDefault().sucursal).FontSize(10);
                                    });

                                    col1.Item().PaddingVertical(4).Table(tabla =>
                                    {
                                        tabla.ColumnsDefinition(Columns =>
                                        {
                                            Columns.RelativeColumn(1.2f);
                                            Columns.RelativeColumn(1.2f);
                                            Columns.RelativeColumn(1.4f);
                                            Columns.RelativeColumn(1.4f);
                                            Columns.RelativeColumn(1.4f);
                                            Columns.RelativeColumn(1.4f);
                                            Columns.RelativeColumn(1.4f);
                                            Columns.RelativeColumn(1.4f);
                                            Columns.RelativeColumn(1.4f);
                                            Columns.RelativeColumn(1.4f);
                                            Columns.RelativeColumn(1.4f);
                                        });

                                        tabla.Header(header =>
                                        {
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("FECHA").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("VENC.").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("DIAS VDOS.").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("DOCUMENTO").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("SERIE").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("DESCRIPCIÓN").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("CARGO").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("ABONOS").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("INT. NORMAL").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("INT. MORAT.").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("SALDO").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        });

                                        foreach (var item in registrosPrimeraSucursalOtros)
                                        {

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                            .Text($"{item.fecha}").FontSize(8).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                           .Text(item.vencimiento).FontSize(8).FontFamily(fontFamily);

                                            if (item.diasvencido < 0)
                                            {
                                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                               .Text("").FontSize(8).FontFamily(fontFamily);
                                            }
                                            else
                                            {
                                                tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                                .Text(item.diasvencido).FontSize(8).FontFamily(fontFamily);
                                            }

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                           .Text(item.documento).FontSize(8).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                           .Text(item.serie_fiscal + " " + item.folio_fiscal).FontSize(8).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                           .Text(item.descripcion).FontSize(8).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignRight()
                                           .Text($"{item.importefactura.ToString("N2")}").FontSize(8).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignRight()
                                            .Text(item.importepagado.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignRight()
                                            .Text("0").FontSize(8).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignRight()
                                            .Text((item.interes_moratorio + item.interes_pactado).ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                            double importetotal = item.saldo + item.interes_moratorio + item.interes_pactado;

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignRight()
                                            .Text(importetotal.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        }
                                        tabla.Footer(footer =>
                                        {
                                            double sumaInteresesTotal = (registrosPrimeraSucursalOtros.Sum(item => item.interes_pactado)) + (registrosPrimeraSucursalOtros.Sum(item => item.interes_moratorio));
                                            double sumasaldoTotal = registrosPrimeraSucursalOtros.Sum(item => item.saldo);
                                            double sumaImportePagadoTotal = registrosPrimeraSucursalOtros.Sum(item => item.importepagado);
                                            double sumaImporteFacturaTotal = registrosPrimeraSucursalOtros.Sum(item => item.importefactura);

                                            footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                            footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                            footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                            footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                            footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                            footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignLeft().Text("TOTAL").FontSize(8).FontFamily("arial").Bold();
                                            footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignRight().Text(sumaImporteFacturaTotal.ToString("N2")).FontSize(8).FontFamily("arial").Bold();
                                            footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignRight().Text(sumaImportePagadoTotal.ToString("N2")).FontSize(8).FontFamily("arial").Bold();
                                            footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignRight().Text("0").FontSize(8).FontFamily("arial").Bold();
                                            footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignRight().Text(sumaInteresesTotal.ToString("N2")).FontSize(8).FontFamily("arial").Bold();
                                            footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignRight().Text(sumaImporteTotalPrimeraSucursal.ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();
                                        }



                                        );

                                        if (sucursalesOtros.Count > 1)
                                        {
                                            foreach (var sucursal in sucursalesOtros.Skip(1))
                                            {
                                                var registrosSucursal = sucursal.ToList();

                                                // Mostrar la tabla para la sucursal diferente
                                                col1.Item().PaddingTop(10).Text(txt =>
                                                {
                                                    txt.Span("Sucursal: ").Bold().FontSize(10);
                                                    txt.Span(registrosSucursal.FirstOrDefault()?.sucursal).FontSize(10);
                                                });

                                                double sumaImporteTotalsucursal = 0;

                                                foreach (var item in registrosSucursal)
                                                {
                                                    double importetotal = item.saldo + item.interes_moratorio + item.interes_pactado;
                                                    sumaImporteTotalsucursal += importetotal;
                                                };

                                                col1.Item().PaddingVertical(4).Table(tabla =>
                                                {
                                                    tabla.ColumnsDefinition(Columns =>
                                                    {
                                                        Columns.RelativeColumn(1.2f);
                                                        Columns.RelativeColumn(1.2f);
                                                        Columns.RelativeColumn(1.4f);
                                                        Columns.RelativeColumn(1.4f);
                                                        Columns.RelativeColumn(1.4f);
                                                        Columns.RelativeColumn(1.4f);
                                                        Columns.RelativeColumn(1.4f);
                                                        Columns.RelativeColumn(1.4f);
                                                        Columns.RelativeColumn(1.4f);
                                                        Columns.RelativeColumn(1.4f);
                                                        Columns.RelativeColumn(1.4f);
                                                    });

                                                    tabla.Header(header =>
                                                    {
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("FECHA").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("VENC.").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("DIAS VDOS.").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("DOCUMENTO").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("SERIE").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("DESCRIPCIÓN").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("CARGO").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("ABONOS").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("INT. NORMAL").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("INT. MORAT.").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("SALDO").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                    });

                                                    foreach (var item in registrosSucursal)
                                                    {

                                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                                        .Text($"{item.fecha}").FontSize(8).FontFamily(fontFamily);

                                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                                       .Text(item.vencimiento).FontSize(8).FontFamily(fontFamily);

                                                        if (item.diasvencido < 0)
                                                        {
                                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                                           .Text("").FontSize(8).FontFamily(fontFamily);
                                                        }
                                                        else
                                                        {
                                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignCenter()
                                                            .Text(item.diasvencido).FontSize(8).FontFamily(fontFamily);
                                                        }

                                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                                       .Text(item.documento).FontSize(8).FontFamily(fontFamily);

                                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                                       .Text(item.serie_fiscal + " " + item.folio_fiscal).FontSize(8).FontFamily(fontFamily);

                                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                                       .Text(item.descripcion).FontSize(8).FontFamily(fontFamily);

                                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignRight()
                                                       .Text($"{item.importefactura.ToString("N2")}").FontSize(8).FontFamily(fontFamily);

                                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignRight()
                                                        .Text(item.importepagado.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignRight()
                                                        .Text("0").FontSize(8).FontFamily(fontFamily);

                                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignRight()
                                                        .Text((item.interes_moratorio + item.interes_pactado).ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                                        double importetotal = item.saldo + item.interes_moratorio + item.interes_pactado;

                                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1).AlignRight()
                                                        .Text(importetotal.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                                    }
                                                    tabla.Footer(footer =>
                                                    {
                                                        double sumaInteresesTotalSucursal = (registrosSucursal.Sum(item => item.interes_pactado)) + (registrosSucursal.Sum(item => item.interes_moratorio));
                                                        double sumasaldoTotalSucursal = registrosSucursal.Sum(item => item.saldo);
                                                        double sumaImportePagadoTotalSucursal = registrosSucursal.Sum(item => item.importepagado);
                                                        double sumaImporteFacturaTotalSucursal = registrosSucursal.Sum(item => item.importefactura);

                                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignCenter().Text("").FontSize(8).FontFamily("arial");
                                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignLeft().Text("TOTAL").FontSize(8).FontFamily("arial").Bold();
                                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignRight().Text(sumaImporteFacturaTotalSucursal.ToString("N2")).FontSize(8).FontFamily("arial").Bold();
                                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignRight().Text(sumaImportePagadoTotalSucursal.ToString("N2")).FontSize(8).FontFamily("arial").Bold();
                                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignLeft().Text("0").FontSize(8).FontFamily("arial").Bold();
                                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignRight().Text(sumaInteresesTotalSucursal.ToString("N2")).FontSize(8).FontFamily("arial").Bold();
                                                        footer.Cell().BorderBottom(1).BorderColor("#ccc").Padding(2).AlignRight().Text(sumaImporteTotalsucursal.ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();
                                                    });
                                                });
                                            }
                                        }
                                    });

                                    col1.Item().PaddingVertical(10).Table(tablaCliente =>
                                    {
                                        var referenciasUnicas = registrosOtros.Select(x => x.referencia).Distinct();
                                        var registrosVencidos1a30 = registrosOtros.Where(x => x.diasvencido >= 1 && x.diasvencido <= 30);
                                        var registrosVencidos31a60 = registrosOtros.Where(x => x.diasvencido >= 31 && x.diasvencido <= 60);
                                        var registrosVencidos61a90 = registrosOtros.Where(x => x.diasvencido >= 61 && x.diasvencido <= 90);
                                        var registrosVencidosmas90 = registrosOtros.Where(x => x.diasvencido >= 91);
                                        var registrosporVencer = registrosOtros.Where(x => x.diasvencido <= 0);

                                        double sumaVencidos1a30 = registrosVencidos1a30.Sum(x => x.saldo);
                                        double sumaVencidos31a60 = registrosVencidos31a60.Sum(x => x.saldo);
                                        double sumaVencidos61a90 = registrosVencidos61a90.Sum(x => x.saldo);
                                        double sumaVencidosmas90 = registrosVencidosmas90.Sum(x => x.saldo);
                                        double sumaporVencer = registrosporVencer.Sum(x => x.saldo);



                                        double sumaVencidoTotal = sumaVencidos1a30 + sumaVencidos31a60 + sumaVencidos61a90 + sumaVencidosmas90;
                                        double sumaVencidoporVencer = sumaporVencer + sumaVencidoTotal;
                                        double sumaTotalPagado = registrosOtros.Sum(x => x.importepagado);

                                        tablaCliente.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn(1f);
                                            columns.RelativeColumn(1f);
                                            columns.RelativeColumn(1f);
                                            columns.RelativeColumn(1f);
                                            columns.RelativeColumn(1f);
                                        });

                                        tablaCliente.Header(header =>
                                        {
                                            header.Cell().Border(1).BorderColor("#D3D3D3").Background("#275027").AlignCenter().AlignMiddle()
                                                .Padding(1).Text("NÚMERO DE REFERENCIA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Border(1).BorderColor("#D3D3D3").AlignCenter().AlignMiddle()
                                                  .Padding(1).Text(string.Join("\n", referenciasUnicas)).FontSize(8).FontFamily(fontFamily).FontColor("#000");
                                            header.Cell().Border(1).BorderColor("#D3D3D3").Background("#275027").AlignCenter().AlignMiddle()
                                                .Padding(1).Text("NOMBRE DEL CLIENTE").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().ColumnSpan(2).Border(1).BorderColor("#D3D3D3").AlignCenter().AlignMiddle()
                                                .Padding(1).Text(registrosOtros.FirstOrDefault().razonsocial).FontSize(8).FontFamily(fontFamily).FontColor("#000");
                                        });

                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO DE UNO A 30 DIAS\n" + sumaVencidos1a30.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO DE 31 A 60 DIAS\n" + sumaVencidos31a60.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO DE 61 A 90 DIAS\n" + sumaVencidos61a90.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO MÁS DE 90 DIAS\n" + sumaVencidosmas90.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("TOTAL VENCIDO\n" + sumaVencidoTotal.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("POR VENCER\n" + sumaporVencer.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO Y POR VENCER\n" + sumaVencidoporVencer.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("SALDO A FAVOR\n" + "0").FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("SALDO NETO + INTS.\n" + sumaImporteTotal.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("TOTAL PAGADO\n" + sumaTotalPagado.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                    });
                                }

                                col1.Item().PageBreak();

                                col1.Item().PaddingVertical(12).Table(tablaDatos =>
                                {
                                    tablaDatos.ColumnsDefinition(Columns =>
                                    {
                                        Columns.RelativeColumn(1f);
                                        Columns.RelativeColumn(1f);
                                        Columns.RelativeColumn(1f);
                                        Columns.RelativeColumn(1f);
                                    });

                                    tablaDatos.Header(header =>
                                    {
                                        header.Cell().ColumnSpan(4).Background("#275027").AlignCenter().AlignMiddle()
                                        .Padding(1).Text("Números de Cuenta").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    });

                                    tablaDatos.Cell().BorderLeft(1).BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("BANCO").FontSize(10).FontFamily(fontFamily).Bold();
                                    tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("CUENTA").FontSize(10).FontFamily(fontFamily).Bold();
                                    tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("CONVENIO").FontSize(10).FontFamily(fontFamily).Bold();
                                    tablaDatos.Cell().BorderRight(1).BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("CLABE").FontSize(10).FontFamily(fontFamily).Bold();

                                    tablaDatos.Cell().BorderLeft(1).BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("BAJIO").FontSize(10).FontFamily(fontFamily);
                                    tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("3487139").FontSize(10).FontFamily(fontFamily);
                                    tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("2974").FontSize(10).FontFamily(fontFamily);
                                    tablaDatos.Cell().BorderRight(1).BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("030730348713902015").FontSize(10).FontFamily(fontFamily);

                                    tablaDatos.Cell().BorderLeft(1).BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("SANTANDER").FontSize(10).FontFamily(fontFamily);
                                    tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("65500056527").FontSize(10).FontFamily(fontFamily);
                                    tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("").FontSize(10).FontFamily(fontFamily);
                                    tablaDatos.Cell().BorderRight(1).BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("014730655000565272").FontSize(10).FontFamily(fontFamily);

                                    tablaDatos.Cell().BorderLeft(1).BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("BBVA").FontSize(10).FontFamily(fontFamily);
                                    tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("0119696946").FontSize(10).FontFamily(fontFamily);
                                    tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("2174774").FontSize(10).FontFamily(fontFamily);
                                    tablaDatos.Cell().BorderRight(1).BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("012914002021747741").FontSize(10).FontFamily(fontFamily);

                                    tablaDatos.Cell().BorderLeft(1).BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("BANORTE").FontSize(10).FontFamily(fontFamily);
                                    tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("1219732793").FontSize(10).FontFamily(fontFamily);
                                    tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("005516").FontSize(10).FontFamily(fontFamily);
                                    tablaDatos.Cell().BorderRight(1).BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("072730012197327937").FontSize(10).FontFamily(fontFamily);

                                    tablaDatos.Cell().BorderLeft(1).BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("HSBC").FontSize(10).FontFamily(fontFamily);
                                    tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("4068669746").FontSize(10).FontFamily(fontFamily);
                                    tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("").FontSize(10).FontFamily(fontFamily);
                                    tablaDatos.Cell().BorderRight(1).BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("021730040686697453").FontSize(10).FontFamily(fontFamily);

                                    tablaDatos.Cell().BorderLeft(1).BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("BANCOPPEL").FontSize(10).FontFamily(fontFamily);
                                    tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("12000010160").FontSize(10).FontFamily(fontFamily);
                                    tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("").FontSize(10).FontFamily(fontFamily);
                                    tablaDatos.Cell().BorderRight(1).BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("137730120000101608").FontSize(10).FontFamily(fontFamily);

                                    tablaDatos.Cell().BorderLeft(1).BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("BANAMEX").FontSize(10).FontFamily(fontFamily);
                                    tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("92300500891").FontSize(10).FontFamily(fontFamily);
                                    tablaDatos.Cell().BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("").FontSize(10).FontFamily(fontFamily);
                                    tablaDatos.Cell().BorderRight(1).BorderBottom(1).BorderColor("#D3D3D3").Background("#f0f0f0").Padding(1).AlignCenter().Text("002730092305008915").FontSize(10).FontFamily(fontFamily);

                                });

                            });

                        page.Footer().Height(100).PaddingLeft(30).PaddingRight(30).PaddingBottom(20).Row(row =>
                        {
                            row.ConstantColumn(0).Row(row1 =>
                            {
                                //var rutaImagen = Path.Combine("C:\\Nube\\HumayaDigital\\HumayaDigitalBackEnd\\HDBackend\\HD_Reporteria\\Imagenes\\QRSinaloa.png");
                                byte[] imageData = System.IO.File.ReadAllBytes(rutaImagenQR);
                                row.ConstantItem(80).BorderRight(1).Image(imageData);

                                row.ConstantColumn(180).Row(row2 =>
                                {
                                    row2.RelativeItem().PaddingLeft(10).Column(col1 =>
                                    {
                                        col1.Item().Row(row3 =>
                                        {
                                            var rutaImagen = Path.Combine("C:\\Nube\\HumayaDigital\\HumayaDigitalBackEnd\\HDBackend\\HD_Reporteria\\Imagenes\\whatsapp.png");
                                            byte[] imageData = System.IO.File.ReadAllBytes(rutaImagen);
                                            row3.ConstantItem(15).PaddingTop(5).Image(imageData);
                                            row3.RelativeItem().PaddingLeft(5).PaddingTop(5).Text(telefono).FontSize(10).FontFamily("arial");
                                        });
                                        col1.Item().PaddingTop(10).Text(txt =>
                                        {
                                            txt.Span("Tel. (667) 502 3527 ").FontSize(10).FontFamily("arial");
                                            txt.Span(extension).Bold().FontSize(10).FontFamily("arial");
                                        });
                                        col1.Item().PaddingTop(10).Text("www.humaya.com.mx").FontSize(10).FontFamily("arial");
                                    });
                                });
                            });
                            row.RelativeItem().AlignRight().PaddingTop(60).Text(txt =>
                            {
                                txt.Span("Pág. ").FontSize(10).FontFamily("arial");
                                txt.CurrentPageNumber().FontSize(10).Bold().FontFamily("arial");
                                txt.Span(" de ").FontSize(10).FontFamily("arial");
                                txt.TotalPages().FontSize(10).Bold().FontFamily("arial");
                            });
                        });

                    });
                }).GeneratePdf();
                RPT_Result result = new RPT_Result();
                result.extension = "pdf";
                result.nombredocumento = "ESTADO DE CUENTA";
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
