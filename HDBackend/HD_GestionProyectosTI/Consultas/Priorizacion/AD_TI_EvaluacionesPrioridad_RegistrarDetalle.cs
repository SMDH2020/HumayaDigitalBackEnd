using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;
using System.Data.SqlClient;

namespace HD_GestionProyectosTI.Consultas.Priorizacion
{
    public class AD_TI_EvaluacionesPrioridad_RegistrarDetalle
    {
        private string CadenaConexion;
        public AD_TI_EvaluacionesPrioridad_RegistrarDetalle(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task RegistrarDetalle(mdl_TI_EvaluacionRegistrarDetalle mdl, int usuarioId)
        {
            try
            {
                var parametros = new
                {
                    mdl.EvaluacionPrioridadId,
                    mdl.CriterioPrioridadId,
                    mdl.Puntuacion,
                    mdl.Comentario,
                    usuarioId
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync(
                    "dbo.sp_TI_EvaluacionesPrioridad_RegistrarDetalle", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
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
