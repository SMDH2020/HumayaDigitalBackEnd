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
                string CapitalizeWords(string input)
                {
                    if (string.IsNullOrWhiteSpace(input))
                        return string.Empty;

                    return string.Join(" ", input.Split(' ')
                        .Where(w => !string.IsNullOrWhiteSpace(w)) 
                        .Select(w => char.ToUpper(w[0]) + w.Substring(1).ToLower()));
                }
                DateTime fecha = DateTime.Now;
                DateTime fechaanterior = fecha.AddDays(-1);
                string fechaActual = fechaanterior.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("es-ES"));
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
                                    col.Item().AlignLeft().PaddingTop(10).Height(30).Text(txt =>
                                    {
                                        //"(" + mdl.FirstOrDefault().idcliente + ") " +
                                        txt.Span(mdl.FirstOrDefault().razonsocial)
                                           .FontSize(12)
                                           .FontFamily("arial")
                                           .Bold();

                                        //col.Item().AlignLeft().Height(30).Text(txt =>
                                        //{
                                        //    txt.Span(CapitalizeWords(mdl.FirstOrDefault().direccion)).FontSize(10).FontFamily("arial");
                                        //});
                                    });
                                });
                               
                                row.ConstantItem(190).Column(col =>
                                {
                                    col.Item().AlignLeft().Height(15).Text(txt =>
                                    {
                                        txt.Span("MAQUINARIA DEL HUMAYA").FontSize(10).FontFamily("arial").Bold();
                                    });

                                    col.Item().AlignLeft().Height(30).Text(txt =>
                                    {
                                        txt.Span("Carret. Navolato-Culiacan #1185 ote. San Pedro de Rosales, Navolato, Sinaloa. ")
                                           .FontSize(10)
                                           .FontFamily("arial");
                                    });
                                });
                            });

                            //col1.Item().LineHorizontal(0.01f).LineColor("#D3D3D3");

                                col1.Item().Row(row =>
                                {
                                    row.RelativeItem().AlignLeft().Text(txt =>
                                    {
                                        txt.Span("Información al: ").Bold().FontSize(10);
                                        txt.Span(fechaActual).FontSize(10);
                                    });

                                    //row.RelativeItem().AlignRight().Text(txt =>
                                    //{
                                    //    txt.Span("Plazo 60 días").FontSize(10);
                                    //});
                                });

                                //col1.Item().LineHorizontal(0.01f).LineColor("#D3D3D3");

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
                                    col1.Item().Row(row =>
                                    {
                                        row.RelativeItem().AlignCenter().Text(txt =>
                                        {
                                            //txt.Span("Línea: ").Bold().FontSize(10);
                                            txt.Span("REFACCIONES Y TALLER").FontSize(10).Bold();
                                        });

                                        //row.RelativeItem().AlignRight().Text(txt =>
                                        //{
                                        //    txt.Span("Total $ ").Bold().FontSize(10);
                                        //    txt.Span(sumaImporteTotal.ToString("N2")).FontSize(10);
                                        //});
                                    });

                                    //col1.Item().LineHorizontal(0.01f).LineColor("#D3D3D3");

                                    //col1.Item().Text("Sucursal: " + mdl.FirstOrDefault().sucursal).Bold().FontSize(10);
                                    col1.Item().Text(txt =>
                                    {
                                        txt.Span("Sucursal: ").Bold().FontSize(10);
                                        txt.Span(CapitalizeWords(registrosRefaccionesYTaller.FirstOrDefault().sucursal)).FontSize(10);
                                    });

                                    col1.Item().PaddingVertical(4).Table(tabla =>
                                    {
                                        tabla.ColumnsDefinition(Columns =>
                                        {
                                            Columns.RelativeColumn(1.4f);
                                            Columns.RelativeColumn(1.4f);
                                            Columns.RelativeColumn(1f);
                                            Columns.RelativeColumn(1f);
                                            Columns.RelativeColumn(1f);
                                            Columns.RelativeColumn(1.5f);
                                            Columns.RelativeColumn(1.4f);
                                            Columns.RelativeColumn(1.4f);
                                            Columns.RelativeColumn(1.4f);
                                        });

                                        tabla.Header(header =>
                                        {
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("FACTURA").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("DESCRIPCIÓN").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("FECHA").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("VENC.").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("DIAS VDOS.").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("IMPORTE").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("ABONOS").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("INT. MORATORIO").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("TOTAL").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        });

                                        foreach (var item in registrosPrimeraSucursal)
                                        {
                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                           .Text(item.serie_fiscal + " " + item.folio_fiscal).FontSize(8).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                           .Text(item.descripcion).FontSize(8).FontFamily(fontFamily);

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
                                                    txt.Span(CapitalizeWords(registrosSucursal.FirstOrDefault()?.sucursal)).FontSize(10);
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
                                                        Columns.RelativeColumn(1.4f);
                                                        Columns.RelativeColumn(1.4f);
                                                        Columns.RelativeColumn(1f);
                                                        Columns.RelativeColumn(1f);
                                                        Columns.RelativeColumn(1f);
                                                        Columns.RelativeColumn(1.5f);
                                                        Columns.RelativeColumn(1.4f);
                                                        Columns.RelativeColumn(1.4f);
                                                        Columns.RelativeColumn(1.4f);
                                                    });

                                                    tabla.Header(header =>
                                                    {
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("FACTURA").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("DESCRIPCIÓN").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("FECHA").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("VENC.").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("DIAS VDOS.").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("IMPORTE").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("ABONOS").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("INT. MORATORIO").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("TOTAL").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                    });

                                                    foreach (var item in registrosSucursal)
                                                    {

                                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                                       .Text(item.serie_fiscal + " " + item.folio_fiscal).FontSize(8).FontFamily(fontFamily);

                                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                                       .Text(item.descripcion).FontSize(8).FontFamily(fontFamily);

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
                                    //col1.Item().PaddingTop(10).Table(tablaCliente =>
                                    //{
                                    //    var referenciasUnicas = registrosRefaccionesYTaller.Select(x => x.referencia).Distinct();
                                    //    var primeraReferencia = registrosRefaccionesYTaller.Select(x => x.referencia).First();
                                    //    var registrosVencidos1a30 = registrosRefaccionesYTaller.Where(x => x.diasvencido >= 1 && x.diasvencido <= 30);
                                    //    var registrosVencidos31a60 = registrosRefaccionesYTaller.Where(x => x.diasvencido >= 31 && x.diasvencido <= 60);
                                    //    var registrosVencidos61a90 = registrosRefaccionesYTaller.Where(x => x.diasvencido >= 61 && x.diasvencido <= 90);
                                    //    var registrosVencidosmas90 = registrosRefaccionesYTaller.Where(x => x.diasvencido >= 91);
                                    //    var registrosporVencer = registrosRefaccionesYTaller.Where(x => x.diasvencido <= 0);

                                    //    double sumaVencidos1a30 = registrosVencidos1a30.Sum(x => x.saldo);
                                    //    double sumaVencidos31a60 = registrosVencidos31a60.Sum(x => x.saldo);
                                    //    double sumaVencidos61a90 = registrosVencidos61a90.Sum(x => x.saldo);
                                    //    double sumaVencidosmas90 = registrosVencidosmas90.Sum(x => x.saldo);
                                    //    double sumaporVencer = registrosporVencer.Sum(x => x.saldo);



                                    //    double sumaVencidoTotal = sumaVencidos1a30 + sumaVencidos31a60 + sumaVencidos61a90 + sumaVencidosmas90;
                                    //    double sumaVencidoporVencer = sumaporVencer + sumaVencidoTotal;
                                    //    double sumaTotalPagado = registrosRefaccionesYTaller.Sum(x => x.importepagado);

                                    //    tablaCliente.ColumnsDefinition(columns =>
                                    //    {
                                    //        columns.RelativeColumn(1f);
                                    //        columns.RelativeColumn(1f);
                                    //        //columns.RelativeColumn(1f);
                                    //        //columns.RelativeColumn(1f);
                                    //        //columns.RelativeColumn(1f);
                                    //    });

                                    //    //tablaCliente.Header(header =>
                                    //    //{
                                    //    //    header.Cell().Border(1).BorderColor("#D3D3D3").Background("#275027").AlignCenter().AlignMiddle()
                                    //    //        .Padding(1).Text("NÚMERO DE REFERENCIA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    //    //    header.Cell().Border(1).BorderColor("#D3D3D3").AlignCenter().AlignMiddle()
                                    //    //          .Padding(1).Text(primeraReferencia).FontSize(8).FontFamily(fontFamily).FontColor("#000");
                                    //    //    //header.Cell().Border(1).BorderColor("#D3D3D3").Background("#275027").AlignCenter().AlignMiddle()
                                    //    //    //    .Padding(1).Text("NOMBRE DEL CLIENTE").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    //    //    //header.Cell().ColumnSpan(2).Border(1).BorderColor("#D3D3D3").AlignCenter().AlignMiddle()
                                    //    //    //    .Padding(1).Text(registrosRefaccionesYTaller.FirstOrDefault().razonsocial).FontSize(8).FontFamily(fontFamily).FontColor("#000");
                                    //    //});

                                    //    //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO DE UNO A 30 DIAS\n" + sumaVencidos1a30.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                    //    //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO DE 31 A 60 DIAS\n" + sumaVencidos31a60.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                    //    //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO DE 61 A 90 DIAS\n" + sumaVencidos61a90.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                    //    //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO MÁS DE 90 DIAS\n" + sumaVencidosmas90.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                    //    //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("TOTAL VENCIDO\n" + sumaVencidoTotal.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    //    //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("POR VENCER\n" + sumaporVencer.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                    //    //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO Y POR VENCER\n" + sumaVencidoporVencer.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                    //    //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("SALDO A FAVOR\n" + "0").FontSize(8).FontFamily(fontFamily);
                                    //    //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("SALDO NETO + INTS.\n" + sumaImporteTotal.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                    //    //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("TOTAL PAGADO\n" + sumaTotalPagado.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                    //});
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

                                    col1.Item().PaddingBottom(2).Row(row =>
                                    {
                                        // Columna 1: "Cliente"
                                        row.ConstantItem(340).Column(col =>
                                        {
                                            col.Item().AlignLeft().PaddingTop(10).Height(30).Text(txt =>
                                            {
                                                //"(" + mdl.FirstOrDefault().idcliente + ") " +
                                                txt.Span(mdl.FirstOrDefault().razonsocial)
                                                   .FontSize(12)
                                                   .FontFamily("arial")
                                                   .Bold();

                                                //col.Item().AlignLeft().Height(30).Text(txt =>
                                                //{
                                                //    txt.Span(CapitalizeWords(mdl.FirstOrDefault().direccion)).FontSize(10).FontFamily("arial");
                                                //});
                                            });
                                        });

                                        row.ConstantItem(190).Column(col =>
                                        {
                                            col.Item().AlignLeft().Height(15).Text(txt =>
                                            {
                                                txt.Span("MAQUINARIA DEL HUMAYA").FontSize(10).FontFamily("arial").Bold();
                                            });

                                            col.Item().AlignLeft().Height(30).Text(txt =>
                                            {
                                                txt.Span("Carret. Navolato-Culiacan #1185 ote. San Pedro de Rosales, Navolato, Sinaloa. ")
                                                   .FontSize(10)
                                                   .FontFamily("arial");
                                            });
                                        });
                                    });

                                    //col1.Item().LineHorizontal(0.01f).LineColor("#D3D3D3");

                                    col1.Item().Row(row =>
                                    {
                                        row.RelativeItem().AlignLeft().Text(txt =>
                                        {
                                            txt.Span("Información al: ").Bold().FontSize(10);
                                            txt.Span(fechaActual).FontSize(10);
                                        });

                                        //row.RelativeItem().AlignRight().Text(txt =>
                                        //{
                                        //    txt.Span("Plazo 60 días").FontSize(10);
                                        //});
                                    });

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

                                    col1.Item().Row(row =>
                                    {
                                        row.RelativeItem().AlignCenter().Text(txt =>
                                        {
                                            //txt.Span("Línea: ").Bold().FontSize(10);
                                            txt.Span("MAQUINARIA Y OTRAS LINEAS").FontSize(10).Bold();
                                        });

                                        //row.RelativeItem().AlignRight().Text(txt =>
                                        //{
                                        //    txt.Span("Total $ ").Bold().FontSize(10);
                                        //    txt.Span(sumaImporteTotal.ToString("N2")).FontSize(10);
                                        //});
                                    });

                                    //col1.Item().LineHorizontal(0.01f).LineColor("#D3D3D3");

                                    col1.Item().Text(txt =>
                                    {
                                        txt.Span("Sucursal: ").Bold().FontSize(10);
                                        txt.Span(CapitalizeWords(registrosOtros.FirstOrDefault().sucursal)).FontSize(10);
                                    });

                                    col1.Item().PaddingVertical(4).Table(tabla =>
                                    {
                                        tabla.ColumnsDefinition(Columns =>
                                        {
                                            Columns.RelativeColumn(1.4f);
                                            Columns.RelativeColumn(1.4f);
                                            Columns.RelativeColumn(1.2f);
                                            Columns.RelativeColumn(1.2f);
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
                                            .Padding(1).Text("FACTURA").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("DESCRIPCIÓN").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("FECHA").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("VENC.").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("DIAS VDOS.").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("IMPORTE").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("ABONOS").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("INT. NORMAL").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("INT. MORAT.").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                            header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("TOTAL").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        });

                                        foreach (var item in registrosPrimeraSucursalOtros)
                                        {
                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                           .Text(item.serie_fiscal + " " + item.folio_fiscal).FontSize(8).FontFamily(fontFamily);

                                            tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                           .Text(item.descripcion).FontSize(8).FontFamily(fontFamily);

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
                                                    txt.Span(CapitalizeWords(registrosSucursal.FirstOrDefault()?.sucursal)).FontSize(10);
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
                                                        Columns.RelativeColumn(1.4f);
                                                        Columns.RelativeColumn(1.4f);
                                                        Columns.RelativeColumn(1.2f);
                                                        Columns.RelativeColumn(1.2f);
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
                                                        .Padding(1).Text("FACTURA").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("DESCRIPCIÓN").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("FECHA").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("VENC.").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("DIAS VDOS.").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("IMPORTE").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("ABONOS").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("INT. NORMAL").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("INT. MORAT.").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                                        .Padding(1).Text("TOTAL").FontSize(08).Bold().FontFamily(fontFamily).FontColor("#fff");
                                                    });

                                                    foreach (var item in registrosSucursal)
                                                    {
                                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                                        .Text(item.serie_fiscal + " " + item.folio_fiscal).FontSize(8).FontFamily(fontFamily);

                                                        tabla.Cell().BorderBottom(1).BorderColor("#afb69d").Height(15).Padding(1)
                                                        .Text(item.descripcion).FontSize(8).FontFamily(fontFamily);

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

                                    //col1.Item().PaddingVertical(10).Table(tablaCliente =>
                                    //{
                                    //    var referenciasUnicas = registrosOtros.Select(x => x.referencia).Distinct();
                                    //    var primeraReferencia = registrosOtros.Select(x => x.referencia).First();
                                    //    var registrosVencidos1a30 = registrosOtros.Where(x => x.diasvencido >= 1 && x.diasvencido <= 30);
                                    //    var registrosVencidos31a60 = registrosOtros.Where(x => x.diasvencido >= 31 && x.diasvencido <= 60);
                                    //    var registrosVencidos61a90 = registrosOtros.Where(x => x.diasvencido >= 61 && x.diasvencido <= 90);
                                    //    var registrosVencidosmas90 = registrosOtros.Where(x => x.diasvencido >= 91);
                                    //    var registrosporVencer = registrosOtros.Where(x => x.diasvencido <= 0);

                                    //    double sumaVencidos1a30 = registrosVencidos1a30.Sum(x => x.saldo);
                                    //    double sumaVencidos31a60 = registrosVencidos31a60.Sum(x => x.saldo);
                                    //    double sumaVencidos61a90 = registrosVencidos61a90.Sum(x => x.saldo);
                                    //    double sumaVencidosmas90 = registrosVencidosmas90.Sum(x => x.saldo);
                                    //    double sumaporVencer = registrosporVencer.Sum(x => x.saldo);



                                    //    double sumaVencidoTotal = sumaVencidos1a30 + sumaVencidos31a60 + sumaVencidos61a90 + sumaVencidosmas90;
                                    //    double sumaVencidoporVencer = sumaporVencer + sumaVencidoTotal;
                                    //    double sumaTotalPagado = registrosOtros.Sum(x => x.importepagado);

                                    //    tablaCliente.ColumnsDefinition(columns =>
                                    //    {
                                    //        columns.RelativeColumn(1f);
                                    //        columns.RelativeColumn(1f);
                                    //        columns.RelativeColumn(1f);
                                    //        columns.RelativeColumn(1f);
                                    //        columns.RelativeColumn(1f);
                                    //    });

                                    //    tablaCliente.Header(header =>
                                    //    {
                                    //        header.Cell().Border(1).BorderColor("#D3D3D3").Background("#275027").AlignCenter().AlignMiddle()
                                    //            .Padding(1).Text("NÚMERO DE REFERENCIA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    //        header.Cell().Border(1).BorderColor("#D3D3D3").AlignCenter().AlignMiddle()
                                    //              .Padding(1).Text(primeraReferencia).FontSize(8).FontFamily(fontFamily).FontColor("#000");
                                    //        header.Cell().Border(1).BorderColor("#D3D3D3").Background("#275027").AlignCenter().AlignMiddle()
                                    //            .Padding(1).Text("NOMBRE DEL CLIENTE").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    //        header.Cell().ColumnSpan(2).Border(1).BorderColor("#D3D3D3").AlignCenter().AlignMiddle()
                                    //            .Padding(1).Text(registrosOtros.FirstOrDefault().razonsocial).FontSize(8).FontFamily(fontFamily).FontColor("#000");
                                    //    });

                                    //    tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO DE UNO A 30 DIAS\n" + sumaVencidos1a30.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                    //    tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO DE 31 A 60 DIAS\n" + sumaVencidos31a60.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                    //    tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO DE 61 A 90 DIAS\n" + sumaVencidos61a90.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                    //    tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO MÁS DE 90 DIAS\n" + sumaVencidosmas90.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                    //    tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("TOTAL VENCIDO\n" + sumaVencidoTotal.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                    //    tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("POR VENCER\n" + sumaporVencer.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                    //    tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO Y POR VENCER\n" + sumaVencidoporVencer.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                    //    tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("SALDO A FAVOR\n" + "0").FontSize(8).FontFamily(fontFamily);
                                    //    tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("SALDO NETO + INTS.\n" + sumaImporteTotal.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                    //    tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("TOTAL PAGADO\n" + sumaTotalPagado.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                    //});
                                }

                                col1.Item().PageBreak();

                                //if (registrosRefaccionesYTaller.Any())
                                //{

                                col1.Item().PaddingBottom(2).Row(row =>
                                {
                                    // Columna 1: "Cliente"
                                    row.ConstantItem(340).Column(col =>
                                    {
                                        col.Item().AlignLeft().PaddingTop(10).Height(30).Text(txt =>
                                        {
                                            //"(" + mdl.FirstOrDefault().idcliente + ") " +
                                            txt.Span(mdl.FirstOrDefault().razonsocial)
                                               .FontSize(12)
                                               .FontFamily("arial")
                                               .Bold();

                                            //col.Item().AlignLeft().Height(30).Text(txt =>
                                            //{
                                            //    txt.Span(CapitalizeWords(mdl.FirstOrDefault().direccion)).FontSize(10).FontFamily("arial");
                                            //});
                                        });
                                    });

                                    row.ConstantItem(190).Column(col =>
                                    {
                                        col.Item().AlignLeft().Height(15).Text(txt =>
                                        {
                                            txt.Span("MAQUINARIA DEL HUMAYA").FontSize(10).FontFamily("arial").Bold();
                                        });

                                        col.Item().AlignLeft().Height(30).Text(txt =>
                                        {
                                            txt.Span("Carret. Navolato-Culiacan #1185 ote. San Pedro de Rosales, Navolato, Sinaloa. ")
                                               .FontSize(10)
                                               .FontFamily("arial");
                                        });
                                    });
                                });

                                //col1.Item().LineHorizontal(0.01f).LineColor("#D3D3D3");

                                col1.Item().Row(row =>
                                {
                                    row.RelativeItem().AlignLeft().Text(txt =>
                                    {
                                        txt.Span("Información al: ").Bold().FontSize(10);
                                        txt.Span(fechaActual).FontSize(10);
                                    });

                                    //row.RelativeItem().AlignRight().Text(txt =>
                                    //{
                                    //    txt.Span("Plazo 60 días").FontSize(10);
                                    //});
                                });

                                col1.Item().AlignCenter().Text(txt =>
                                    {
                                        txt.Span("RESUMEN DE SALDOS").Bold().FontSize(10);
                                    });
                                    col1.Item().PaddingTop(2).PaddingBottom(10).Table(tablaCliente =>
                                    {
                                        double sumaVencidos1a30Revolvente = 0;
                                        double sumaVencidos31a60Revolvente = 0;
                                        double sumaVencidos61a90Revolvente = 0;
                                        double sumaVencidosmas90Revolvente = 0;
                                        double sumaporVencerRevolvente = 0;

                                        double sumaInteresNormalRevolvente = 0;
                                        double sumaInteresMoratorioRevolvente = 0;


                                        double sumaVencidoTotalRevolvente = 0;
                                        double sumaVencidoporVencerRevolvente = 0;

                                        double sumaTotalPagadoRevolvente = 0;

                                        double sumaVencidos1a30Operacion = 0;
                                        double sumaVencidos31a60Operacion = 0;
                                        double sumaVencidos61a90Operacion = 0;
                                        double sumaVencidosmas90Operacion = 0;
                                        double sumaporVencerOperacion = 0;

                                        double sumaInteresNormalOperacion = 0;
                                        double sumaInteresMoratorioOperacion = 0;

                                        double sumaVencidoTotalOperacion = 0;
                                        double sumaVencidoporVencerOperacion = 0;
                                        double sumaTotalPagadoOperacion = 0;

                                        double sumaImporteTotalOperacion = 0;
                                        double sumaImporteTotalRevolvente = 0;

                                        if (registrosRefaccionesYTaller.Any())
                                        {

                                            foreach (var item in registrosRefaccionesYTaller)
                                            {
                                                double importetotalRevolvente = item.saldo + item.interes_moratorio + item.interes_pactado;
                                                sumaImporteTotalRevolvente += importetotalRevolvente;
                                            };

                                            var referenciasUnicasRevolvente = registrosRefaccionesYTaller.Select(x => x.referencia).Distinct();
                                            var primeraReferenciaRevolvente = registrosRefaccionesYTaller.Select(x => x.referencia).First();
                                            var registrosVencidos1a30Revolvente = registrosRefaccionesYTaller.Where(x => x.diasvencido >= 1 && x.diasvencido <= 30);
                                            var registrosVencidos31a60Revolvente = registrosRefaccionesYTaller.Where(x => x.diasvencido >= 31 && x.diasvencido <= 60);
                                            var registrosVencidos61a90Revolvente = registrosRefaccionesYTaller.Where(x => x.diasvencido >= 61 && x.diasvencido <= 90);
                                            var registrosVencidosmas90Revolvente = registrosRefaccionesYTaller.Where(x => x.diasvencido >= 91);
                                            var registrosporVencerRevolvente = registrosRefaccionesYTaller.Where(x => x.diasvencido <= 0);

                                            sumaVencidos1a30Revolvente = registrosVencidos1a30Revolvente.Sum(x => x.saldo);
                                            sumaVencidos31a60Revolvente = registrosVencidos31a60Revolvente.Sum(x => x.saldo);
                                            sumaVencidos61a90Revolvente = registrosVencidos61a90Revolvente.Sum(x => x.saldo);
                                            sumaVencidosmas90Revolvente = registrosVencidosmas90Revolvente.Sum(x => x.saldo);
                                            sumaporVencerRevolvente = registrosporVencerRevolvente.Sum(x => x.saldo);

                                            sumaInteresNormalRevolvente = registrosRefaccionesYTaller.Sum(x => x.interes_pactado);
                                            sumaInteresMoratorioRevolvente = registrosRefaccionesYTaller.Sum(x => x.interes_moratorio);


                                            sumaVencidoTotalRevolvente = sumaVencidos1a30Revolvente + sumaVencidos31a60Revolvente + sumaVencidos61a90Revolvente + sumaVencidosmas90Revolvente;
                                            sumaVencidoporVencerRevolvente = sumaporVencerRevolvente + sumaVencidoTotalRevolvente;
                                            sumaTotalPagadoRevolvente = registrosRefaccionesYTaller.Sum(x => x.importepagado);
                                        }

                                        if (registrosOtros.Any()) { 

                                        foreach (var item in registrosOtros)
                                        {
                                            double importetotalOperacion = item.saldo + item.interes_moratorio + item.interes_pactado;
                                            sumaImporteTotalOperacion += importetotalOperacion;
                                        };
                                        var referenciasUnicasOperacion = registrosOtros.Select(x => x.referencia).Distinct();
                                        var primeraReferenciaOperacion = registrosOtros.Select(x => x.referencia).First();
                                        var registrosVencidos1a30Operacion = registrosOtros.Where(x => x.diasvencido >= 1 && x.diasvencido <= 30);
                                        var registrosVencidos31a60Operacion = registrosOtros.Where(x => x.diasvencido >= 31 && x.diasvencido <= 60);
                                        var registrosVencidos61a90Operacion = registrosOtros.Where(x => x.diasvencido >= 61 && x.diasvencido <= 90);
                                        var registrosVencidosmas90Operacion = registrosOtros.Where(x => x.diasvencido >= 91);
                                        var registrosporVencerOperacion = registrosOtros.Where(x => x.diasvencido <= 0);

                                        sumaVencidos1a30Operacion = registrosVencidos1a30Operacion.Sum(x => x.saldo);
                                        sumaVencidos31a60Operacion = registrosVencidos31a60Operacion.Sum(x => x.saldo);
                                        sumaVencidos61a90Operacion = registrosVencidos61a90Operacion.Sum(x => x.saldo);
                                        sumaVencidosmas90Operacion = registrosVencidosmas90Operacion.Sum(x => x.saldo);
                                        sumaporVencerOperacion = registrosporVencerOperacion.Sum(x => x.saldo);

                                        sumaInteresNormalOperacion = registrosOtros.Sum(x => x.interes_pactado);
                                        sumaInteresMoratorioOperacion = registrosOtros.Sum(x => x.interes_moratorio);

                                        sumaVencidoTotalOperacion = sumaVencidos1a30Operacion + sumaVencidos31a60Operacion + sumaVencidos61a90Operacion + sumaVencidosmas90Operacion;
                                        sumaVencidoporVencerOperacion = sumaporVencerOperacion + sumaVencidoTotalOperacion;
                                        sumaTotalPagadoOperacion = registrosOtros.Sum(x => x.importepagado);

                                        }

                                        double sumaTotalVencidoAmbos = sumaVencidoTotalRevolvente + sumaVencidoTotalOperacion;
                                        double sumaVencidoMas90Ambos = sumaVencidosmas90Revolvente + sumaVencidosmas90Operacion;
                                        double sumaVencido61a90Ambos = sumaVencidos61a90Revolvente + sumaVencidos61a90Operacion;
                                        double sumaVencido31a60Ambos = sumaVencidos31a60Revolvente + sumaVencidos31a60Operacion;
                                        double sumavencido1a30Ambos = sumaVencidos1a30Revolvente + sumaVencidos1a30Operacion;
                                        double sumaporVencerAmbos = sumaporVencerRevolvente + sumaporVencerOperacion;
                                        double sumaInteresNormalAmbos = sumaInteresNormalRevolvente + sumaInteresNormalOperacion;
                                        double sumaInteresMoratorioAmbos = sumaInteresMoratorioRevolvente + sumaInteresMoratorioOperacion;
                                        double sumaImporteTotalAmbos = sumaImporteTotalRevolvente + sumaImporteTotalOperacion;

                                        tablaCliente.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn(1f);
                                            columns.RelativeColumn(1f);
                                            columns.RelativeColumn(1f);
                                            columns.RelativeColumn(1f);
                                        });
                                        tablaCliente.Header(header =>
                                    {
                                        //header.Cell().Border(1).BorderColor("#D3D3D3").Background("#275027").AlignCenter().AlignMiddle()
                                        //    .Padding(1).Text("NÚMERO DE REFERENCIA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        //header.Cell().Border(1).BorderColor("#D3D3D3").AlignCenter().AlignMiddle()
                                        //      .Padding(1).Text(primeraReferencia).FontSize(8).FontFamily(fontFamily).FontColor("#000");
                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("ANTIGÜEDAD").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("MAQUINARIA Y OTRAS LINEAS").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("REFACCIONES Y TALLER").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                        header.Cell().Background("#275027").AlignCenter().AlignMiddle()
                                            .Padding(1).Text("TOTAL").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    });
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("TOTAL VENCIDO").FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaVencidoTotalOperacion.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaVencidoTotalRevolvente.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaTotalVencidoAmbos.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Background("#ECF3DB").Padding(1).AlignCenter().Text("VENCIDO MÁS DE 90 DIAS").FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Background("#ECF3DB").Padding(1).AlignCenter().Text(sumaVencidosmas90Operacion.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Background("#ECF3DB").Padding(1).AlignCenter().Text(sumaVencidosmas90Revolvente.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Background("#ECF3DB").Padding(1).AlignCenter().Text(sumaVencidoMas90Ambos.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Background("#ECF3DB").Padding(1).AlignCenter().Text("VENCIDO DE 61 A 90 DIAS").FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Background("#ECF3DB").Padding(1).AlignCenter().Text(sumaVencidos61a90Operacion.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Background("#ECF3DB").Padding(1).AlignCenter().Text(sumaVencidos61a90Revolvente.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Background("#ECF3DB").Padding(1).AlignCenter().Text(sumaVencido61a90Ambos.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Background("#ECF3DB").Padding(1).AlignCenter().Text("VENCIDO DE 31 A 60 DIAS").FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Background("#ECF3DB").Padding(1).AlignCenter().Text(sumaVencidos31a60Operacion.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Background("#ECF3DB").Padding(1).AlignCenter().Text(sumaVencidos31a60Revolvente.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Background("#ECF3DB").Padding(1).AlignCenter().Text(sumaVencido31a60Ambos.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Background("#ECF3DB").Padding(1).AlignCenter().Text("VENCIDO DE UNO A 30 DIAS").FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Background("#ECF3DB").Padding(1).AlignCenter().Text(sumaVencidos1a30Operacion.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Background("#ECF3DB").Padding(1).AlignCenter().Text(sumaVencidos1a30Revolvente.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Background("#ECF3DB").Padding(1).AlignCenter().Text(sumavencido1a30Ambos.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("POR VENCER").FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaporVencerOperacion.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaporVencerRevolvente.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaporVencerAmbos.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("INT. NORMAL").FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaInteresNormalOperacion.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaInteresNormalRevolvente.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaInteresNormalAmbos.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("INT. MORATORIO").FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaInteresMoratorioOperacion.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaInteresMoratorioRevolvente.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaInteresMoratorioAmbos.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("SALDO TOTAL").FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaImporteTotalOperacion.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaImporteTotalRevolvente.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaImporteTotalAmbos.ToString("N2")).FontSize(8).FontFamily(fontFamily).Bold();


                                        //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("TOTAL VENCIDO").FontSize(8).FontFamily(fontFamily);
                                        //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaVencidoTotal.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO MÁS DE 90 DIAS").FontSize(8).FontFamily(fontFamily);
                                        //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaVencidosmas90.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO DE 61 A 90 DIAS").FontSize(8).FontFamily(fontFamily);
                                        //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaVencidos61a90.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO DE 31 A 60 DIAS").FontSize(8).FontFamily(fontFamily);
                                        //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaVencidos31a60.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO DE UNO A 30 DIAS").FontSize(8).FontFamily(fontFamily);
                                        //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaVencidos1a30.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("POR VENCER").FontSize(8).FontFamily(fontFamily);
                                        //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaporVencer.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO Y POR VENCER").FontSize(8).FontFamily(fontFamily);
                                        //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaVencidoporVencer.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("SALDO A FAVOR").FontSize(8).FontFamily(fontFamily);
                                        //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("0").FontSize(8).FontFamily(fontFamily);

                                        //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("SALDO NETO + INTS.").FontSize(8).FontFamily(fontFamily);
                                        //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaImporteTotal.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                        //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("TOTAL PAGADO").FontSize(8).FontFamily(fontFamily);
                                        //tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaTotalPagado.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                    });
                                //}

                                //if (registrosOtros.Any())
                                //{

                                //    col1.Item().Text(txt =>
                                //    {
                                //        txt.Span("Resumen de Línea: ").Bold().FontSize(10);
                                //        txt.Span("Maquinaria y Otras Líneas").FontSize(10);
                                //    });
                                //    col1.Item().PaddingTop(2).PaddingBottom(10).Table(tablaCliente =>
                                //    {
                                //        double sumaImporteTotal = 0;

                                //        foreach (var item in registrosOtros)
                                //        {
                                //            double importetotal = item.saldo + item.interes_moratorio + item.interes_pactado;
                                //            sumaImporteTotal += importetotal;
                                //        };
                                //        var referenciasUnicas = registrosOtros.Select(x => x.referencia).Distinct();
                                //        var primeraReferencia = registrosOtros.Select(x => x.referencia).First();
                                //        var registrosVencidos1a30 = registrosOtros.Where(x => x.diasvencido >= 1 && x.diasvencido <= 30);
                                //        var registrosVencidos31a60 = registrosOtros.Where(x => x.diasvencido >= 31 && x.diasvencido <= 60);
                                //        var registrosVencidos61a90 = registrosOtros.Where(x => x.diasvencido >= 61 && x.diasvencido <= 90);
                                //        var registrosVencidosmas90 = registrosOtros.Where(x => x.diasvencido >= 91);
                                //        var registrosporVencer = registrosOtros.Where(x => x.diasvencido <= 0);

                                //        double sumaVencidos1a30 = registrosVencidos1a30.Sum(x => x.saldo);
                                //        double sumaVencidos31a60 = registrosVencidos31a60.Sum(x => x.saldo);
                                //        double sumaVencidos61a90 = registrosVencidos61a90.Sum(x => x.saldo);
                                //        double sumaVencidosmas90 = registrosVencidosmas90.Sum(x => x.saldo);
                                //        double sumaporVencer = registrosporVencer.Sum(x => x.saldo);



                                //        double sumaVencidoTotal = sumaVencidos1a30 + sumaVencidos31a60 + sumaVencidos61a90 + sumaVencidosmas90;
                                //        double sumaVencidoporVencer = sumaporVencer + sumaVencidoTotal;
                                //        double sumaTotalPagado = registrosOtros.Sum(x => x.importepagado);

                                //        tablaCliente.ColumnsDefinition(columns =>
                                //        {
                                //            columns.RelativeColumn(1f);
                                //            columns.RelativeColumn(1f);
                                //        });

                                //        tablaCliente.Header(header =>
                                //        {
                                //            header.Cell().Border(1).BorderColor("#D3D3D3").Background("#275027").AlignCenter().AlignMiddle()
                                //                .Padding(1).Text("NÚMERO DE REFERENCIA").FontSize(8).Bold().FontFamily(fontFamily).FontColor("#fff");
                                //            header.Cell().Border(1).BorderColor("#D3D3D3").AlignCenter().AlignMiddle()
                                //                  .Padding(1).Text(primeraReferencia).FontSize(8).FontFamily(fontFamily).FontColor("#000");
                                //        });
                                        
                                //        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("TOTAL VENCIDO").FontSize(8).FontFamily(fontFamily);
                                //        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaVencidoTotal.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                //        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO MÁS DE 90 DIAS").FontSize(8).FontFamily(fontFamily);
                                //        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaVencidosmas90.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                //        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO DE 61 A 90 DIAS").FontSize(8).FontFamily(fontFamily);
                                //        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaVencidos61a90.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                //        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO DE 31 A 60 DIAS").FontSize(8).FontFamily(fontFamily);
                                //        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaVencidos31a60.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                //        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO DE UNO A 30 DIAS").FontSize(8).FontFamily(fontFamily);
                                //        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaVencidos1a30.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                //        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("POR VENCER").FontSize(8).FontFamily(fontFamily);
                                //        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaporVencer.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                //        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("VENCIDO Y POR VENCER").FontSize(8).FontFamily(fontFamily);
                                //        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaVencidoporVencer.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                //        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("SALDO A FAVOR").FontSize(8).FontFamily(fontFamily);
                                //        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("0").FontSize(8).FontFamily(fontFamily);

                                //        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("SALDO NETO + INTS.").FontSize(8).FontFamily(fontFamily);
                                //        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaImporteTotal.ToString("N2")).FontSize(8).FontFamily(fontFamily);

                                //        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("TOTAL PAGADO").FontSize(8).FontFamily(fontFamily);
                                //        tablaCliente.Cell().Border(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(sumaTotalPagado.ToString("N2")).FontSize(8).FontFamily(fontFamily);
                                //    });
                                //}

                                col1.Item().PaddingVertical(12).Table(tablaDatos =>
                                {
                                    var primeraReferencia = mdl.Select(x => x.referencia).First();

                                    tablaDatos.ColumnsDefinition(Columns =>
                                    {
                                        Columns.RelativeColumn(1f);
                                        Columns.RelativeColumn(1f);
                                        Columns.RelativeColumn(1f);
                                        Columns.RelativeColumn(1f);
                                    });

                                    tablaDatos.Header(header =>
                                    {
                                        header.Cell().ColumnSpan(4).Background("#275027").AlignCenter().AlignMiddle().Padding(1).Text("NUMEROS DE CUENTA").FontSize(10).Bold().FontFamily(fontFamily).FontColor("#fff");
                                    });

                                    

                                    tablaDatos.Cell().BorderLeft(1).BorderBottom(1).BorderTop(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("BANCO").FontSize(10).FontFamily(fontFamily).Bold();
                                    tablaDatos.Cell().BorderBottom(1).BorderTop(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("CUENTA").FontSize(10).FontFamily(fontFamily).Bold();
                                    tablaDatos.Cell().BorderBottom(1).BorderTop(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("CONVENIO").FontSize(10).FontFamily(fontFamily).Bold();
                                    tablaDatos.Cell().BorderRight(1).BorderBottom(1).BorderTop(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text("CLABE").FontSize(10).FontFamily(fontFamily).Bold();

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

                                    tablaDatos.Cell().ColumnSpan(2).RowSpan(2).BorderLeft(1).BorderTop(1).BorderBottom(1).BorderColor("#D3D3D3").Background("#ECF3DB").Padding(1).AlignCenter().AlignMiddle().Text("NUMERO DE REFERENCIA").FontSize(12).FontFamily(fontFamily).Bold();
                                    tablaDatos.Cell().ColumnSpan(2).BorderRight(1).BorderTop(1).BorderBottom(1).BorderColor("#D3D3D3").Padding(1).AlignCenter().Text(txt =>
                                    {
                                        txt.Span("CONCEPTO DE PAGO: ").FontSize(10).FontFamily(fontFamily); 
                                        txt.Span(primeraReferencia.ToString()).FontSize(10).FontFamily(fontFamily).Bold(); 
                                        txt.Span("\n"); // Salto de línea
                                        txt.Span("REFERENCIA DE PAGO: ").FontSize(10).FontFamily(fontFamily); 
                                        txt.Span(primeraReferencia.ToString()).FontSize(10).FontFamily(fontFamily).Bold(); 
                                    });

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
