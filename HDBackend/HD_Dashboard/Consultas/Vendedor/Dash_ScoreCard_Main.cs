using ExcelDataReader;
using HD.AccesoDatos;
using HD_Dashboard.Modelos;
using System.Globalization;

namespace HD_Dashboard.Consultas.Vendedor
{
    public class Dash_ScoreCard_Main
    {
        public Dash_ScoreCard_Main() { }

        public static Task<mdlScoreCardResult[]> ScoreCards()
        {
            try
            {
                var filePath = "C:\\SMDH\\HumayaDigital\\ScoreCard Navolato.xls";
                //var filePath = "C:\\SMDH\\HumayaDigital\\Scordcard.xlsx";
                var scoreCards = new List<mdlScoreCardResult>();
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var config = new ExcelDataSetConfiguration()
                        {
                            // Indicar que se usen los nombres de las columnas del archivo
                            UseColumnDataType = true,
                            // Indicar que se omitan las primeras 7 filas antes de leer los nombres de las columnas
                            ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = false,
                            },
                        };

                        var result = reader.AsDataSet(config);
                        // string mes = "JUL";// DateTime.Now.ToString("MMM", new CultureInfo("es-MX")).TrimEnd('.').ToUpper();
                        string mesInicial = "ENE";
                        string mesActual = DateTime.Parse("30/10/2023").ToString("MMM", new CultureInfo("es-MX")).TrimEnd('.').ToUpper();
                        //string mesActual = DateTime.Now.ToString("MMM", new CultureInfo("es-MX")).TrimEnd('.').ToUpper();
                        for (int i = 1; i < result.Tables.Count - 2; i++)
                        {
                            int mesActualIndex = 0;
                            int mesInicialIndex = 0;

                            for (int j = 2; j < 26; j++)
                            {
                                if (result.Tables[i].Rows[7][j].ToString() == mesInicial)
                                {
                                    mesInicialIndex = j;
                                    break;
                                }
                            }

                            for (int j = mesInicialIndex; j < 26; j++)
                            {
                                if (result.Tables[i].Rows[7][j].ToString() == mesActual)
                                {
                                    mesActualIndex = j;
                                    break;
                                }
                            }
                            int objCombinadas = 0;
                            int realCombinadas = 0;
                            int objTractores = 0;
                            int realTractores = 0;
                            int objImplementos = 0;
                            int realImplementos = 0;
                            int objUsadas = 0;
                            int realUsadas = 0;


                            for (int j = mesInicialIndex; j <= mesActualIndex; j += 2)
                            {
                                objCombinadas += Convert.ToInt32(result.Tables[i].Rows[12][j]);
                                realCombinadas += Convert.ToInt32(result.Tables[i].Rows[12][j + 1]);
                                objTractores += Convert.ToInt32(result.Tables[i].Rows[81][j]);
                                realTractores += Convert.ToInt32(result.Tables[i].Rows[81][j + 1]);
                                objImplementos += Convert.ToInt32(result.Tables[i].Rows[151][j]);
                                realImplementos += Convert.ToInt32(result.Tables[i].Rows[151][j + 1]);
                                objUsadas += Convert.ToInt32(result.Tables[i].Rows[181][j]);
                                realUsadas += Convert.ToInt32(result.Tables[i].Rows[181][j + 1]);

                            }

                            string tablename = result.Tables[i].TableName;
                            ScoreCard scoreCard = new ScoreCard(result.Tables[i].Rows[3][2].ToString(),tablename, objCombinadas, realCombinadas, objTractores, realTractores, objImplementos, realImplementos, objUsadas, realUsadas);

                            mdlScoreCardResult mdlresult = new mdlScoreCardResult();
                            mdlresult.scorecard = scoreCard;
                            mdlresult.porcentaje =
                                Math.Round(((scoreCard.objetivoUsadas == 0 ? 100 : scoreCard.porcentajeUsadas) +
                                (scoreCard.objetivoTractores == 0 ? 100 : scoreCard.porcentajeTractores) +
                                (scoreCard.objetivoImplementos == 0 ? 100 : scoreCard.porcentajeImplementos) +
                                (scoreCard.objetivoCombinadas == 0 ? 100 : scoreCard.porcentajeCombinadas)) * .25, 0);
                            scoreCards.Add(mdlresult);
                            /*
                            Console.WriteLine(scoreCard.nombre);
                            Console.WriteLine("Combinadas ---> Objetivo: {0}, Reales: {1}, Porcentaje: {2}%", scoreCard.objCombinadas, scoreCard.realCombinadas, scoreCard.porcentajeCombinadas);
                            Console.WriteLine("Tractores ---> Objetivo: {0}, Reales: {1}, Porcentaje: {2}%", scoreCard.objTractores, scoreCard.realTractores, scoreCard.porcentajeTractores);
                            Console.WriteLine("Implementos ---> Objetivo: {0}, Reales: {1}, Porcentaje: {2}%", scoreCard.objImplementos, scoreCard.realImplementos, scoreCard.porcentajeImplementos);
                            Console.WriteLine("Usados ---> Objetivo: {0}, Reales: {1}, Porcentaje: {2}%", scoreCard.objUsadas, scoreCard.realUsadas, scoreCard.porcentajeUsadas);
                            Console.WriteLine();
                            */



                        }
                        //Console.ReadLine();
                    }
                }
                return Task.FromResult(scoreCards.ToArray());
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
