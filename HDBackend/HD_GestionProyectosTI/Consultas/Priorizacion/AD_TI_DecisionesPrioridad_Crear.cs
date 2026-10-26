using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;
using System.Data.SqlClient;

namespace HD_GestionProyectosTI.Consultas.Priorizacion
{
    public class AD_TI_DecisionesPrioridad_Crear
    {
        private string CadenaConexion;
        public AD_TI_DecisionesPrioridad_Crear(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<(long DecisionPrioridadId, int NumeroDecision)> Crear(mdl_TI_DecisionCrear mdl, int usuarioId)
        {
            try
            {
                var parametros = new
                {
                    mdl.SolicitudId,
                    mdl.EvaluacionPrioridadId,
                    mdl.TipoDecision,
                    mdl.PrioridadId,
                    mdl.PosicionBacklog,
                    mdl.Motivo,
                    usuarioId
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryFirstAsync<(long DecisionPrioridadId, int NumeroDecision)>(
                    "dbo.sp_TI_DecisionesPrioridad_Crear", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
