using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;
using System.Data.SqlClient;

namespace HD_GestionProyectosTI.Consultas.Solicitudes
{
    public class AD_TI_Solicitudes_ResolverRevisionPrioridad
    {
        private string CadenaConexion;
        public AD_TI_Solicitudes_ResolverRevisionPrioridad(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task Resolver(mdl_TI_RevisionPrioridadResolver mdl, int usuarioId)
        {
            try
            {
                var parametros = new
                {
                    mdl.SolicitudId,
                    mdl.EstadoResolucion,
                    mdl.Motivo,
                    usuarioId
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync(
                    "dbo.sp_TI_Solicitudes_ResolverRevisionPrioridad", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
