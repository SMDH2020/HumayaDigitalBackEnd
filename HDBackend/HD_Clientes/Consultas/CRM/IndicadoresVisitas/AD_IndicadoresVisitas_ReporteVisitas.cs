using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.CRM.IndicadoresVisitas;
using System.Data.SqlClient;

namespace HD.Clientes.Consultas.CRM.IndicadoresVisitas
{
    public class AD_IndicadoresVisitas_ReporteVisitas
    {
        private string CadenaConexion;
        public AD_IndicadoresVisitas_ReporteVisitas(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        /// <summary>
        /// Devuelve el objetivo vs las visitas realizadas por asesor y semana.
        /// El SP regresa el listado ya ordenado por vendedor y fecha: no reordenar aqui.
        /// El pivoteo por semana lo arma el front.
        /// Los errores definidos por el usuario en SQL (numero mayor o igual a 50000)
        /// son validaciones del SP con mensaje para el usuario final y se devuelven
        /// tal cual como BadRequest.
        /// </summary>
        public async Task<IEnumerable<mdl_IndicadoresVisitas_ReporteVisitas>> ReporteVisitas(int ejercicio, int periodo, string? tipo)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {
                var parametros = new
                {
                    ejercicio = ejercicio,
                    periodo = periodo,
                    tipo = tipo
                };

                IEnumerable<mdl_IndicadoresVisitas_ReporteVisitas> result = await factory.SQL.QueryAsync<mdl_IndicadoresVisitas_ReporteVisitas>("CRM.sp_ObjetivosSemanales_ReporteVisitas", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (SqlException ex) when (ex.Number >= 50000)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.BadRequest, new { Mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
