using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace HD_GestionProyectosTI.Consultas.Priorizacion
{
    public class AD_TI_EvaluacionesPrioridad_Crear
    {
        private string CadenaConexion;
        public AD_TI_EvaluacionesPrioridad_Crear(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<(long EvaluacionPrioridadId, int NumeroEvaluacion)> Crear(mdl_TI_EvaluacionCrear mdl)
        {
            try
            {
                var parametros = new
                {
                    mdl.SolicitudId,
                    mdl.VersionAlcanceId,
                    mdl.EvaluadaPorUsuarioId,
                    json = JsonConvert.SerializeObject(mdl.Detalle)
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryFirstAsync<(long EvaluacionPrioridadId, int NumeroEvaluacion)>(
                    "dbo.sp_TI_EvaluacionesPrioridad_Crear", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
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
