using ExcelDataReader;
using HD.AccesoDatos;
using HD_Dashboard.Modelos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        string mes = "JUL";// DateTime.Now.ToString("MMM", new CultureInfo("es-MX")).TrimEnd('.').ToUpper();
                        for (int i = 1; i < result.Tables.Count - 2; i++)
                        {
                            int mesIndex = 0;

                            for (int j = 2; j < 26; j++)
                            {
                                string value = result.Tables[i].Rows[7][j].ToString();
                                if ( value== mes)
                                {
                                    mesIndex = j;
                                    break;
                                }
                            }

                            ScoreCard scoreCard = new ScoreCard(result.Tables[i].Rows[3][2].ToString(),
                                Convert.ToInt32(result.Tables[i].Rows[12][mesIndex]), Convert.ToInt32(result.Tables[i].Rows[12][mesIndex + 1]),
                                Convert.ToInt32(result.Tables[i].Rows[81][mesIndex]), Convert.ToInt32(result.Tables[i].Rows[81][mesIndex + 1]),
                                Convert.ToInt32(result.Tables[i].Rows[151][mesIndex]), Convert.ToInt32(result.Tables[i].Rows[151][mesIndex + 1]),
                                Convert.ToInt32(result.Tables[i].Rows[181][mesIndex]), Convert.ToInt32(result.Tables[i].Rows[181][mesIndex + 1]));
                            mdlScoreCardResult mdlresult = new mdlScoreCardResult();
                            mdlresult.scorecard = scoreCard;
                            mdlresult.porcentaje =
                                Math.Round(((scoreCard.objetivoUsadas == 0 ? 100 : scoreCard.porcentajeUsadas) +
                                (scoreCard.objetivoTractores == 0 ? 100 : scoreCard.porcentajeTractores) +
                                (scoreCard.objetivoImplementos == 0 ? 100 : scoreCard.porcentajeImplementos) +
                                (scoreCard.objetivoCombinadas == 0 ? 100 : scoreCard.porcentajeCombinadas))*.25, 0);
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
