using Dapper;
using HD.AccesoDatos;
using System.Data.SqlClient;

namespace HD_GestionProyectosTI.Consultas.Priorizacion
{
    public class AD_TI_EvaluacionesPrioridad_Completar
    {
        private string CadenaConexion;
        public AD_TI_EvaluacionesPrioridad_Completar(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<decimal> Completar(long evaluacionPrioridadId, int usuarioId)
        {
            try
            {
                var parametros = new { evaluacionPrioridadId, usuarioId };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                decimal puntajeTotal = await factory.SQL.QueryFirstAsync<decimal>(
                    "dbo.sp_TI_EvaluacionesPrioridad_Completar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return puntajeTotal;
            }
            catch (SqlException ex) when (ex.Number >= 50000)
            {
                throw new Excepciones(System.Net.HttpStatusCode.BadRequest, new { Mensaje = ex.Message });
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
