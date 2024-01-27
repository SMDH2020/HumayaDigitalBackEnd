using QuestPDF.Fluent;

namespace HD_Reporteria.Cobranza
{
    public class RPT_TotalCartera_PorLinea
    {
        public static RPT_Result Generar()
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
                                    row2.RelativeItem().Padding(10).PaddingLeft(30).Text("PEDIDO DE MAQUINARIA").FontColor("#fff").FontSize(20).Bold().FontFamily(fontFamily);
                                });
                            });


                        });


                    });
                }).GeneratePdf();
                RPT_Result result = new RPT_Result();
                result.extension = "pdf";
                result.nombredocumento = "RESUMEN CARTERA POR LINEA";
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
