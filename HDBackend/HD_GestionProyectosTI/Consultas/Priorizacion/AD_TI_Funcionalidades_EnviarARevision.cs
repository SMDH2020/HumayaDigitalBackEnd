using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;
using System.Data.SqlClient;

namespace HD_GestionProyectosTI.Consultas.Priorizacion
{
    public class AD_TI_Funcionalidades_EnviarARevision
    {
        private string CadenaConexion;
        public AD_TI_Funcionalidades_EnviarARevision(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<long> EnviarARevision(mdl_TI_FuncionalidadEnviarRevision mdl, int usuarioId)
        {
            try
            {
                var parametros = new { mdl.FuncionalidadId, usuarioId };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                long cambioProgramacionId = await factory.SQL.QueryFirstAsync<long>(
                    "dbo.sp_TI_Funcionalidades_EnviarARevision", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return cambioProgramacionId;
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
