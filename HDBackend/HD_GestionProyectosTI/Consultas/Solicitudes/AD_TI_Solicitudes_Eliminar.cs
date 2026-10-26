using Dapper;
using HD.AccesoDatos;
using System.Data.SqlClient;

namespace HD_GestionProyectosTI.Consultas.Solicitudes
{
    public class AD_TI_Solicitudes_Eliminar
    {
        private string CadenaConexion;
        public AD_TI_Solicitudes_Eliminar(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task Eliminar(long solicitudId, int usuarioId)
        {
            try
            {
                var parametros = new { solicitudId, usuarioId };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync(
                    "dbo.sp_TI_Solicitudes_Eliminar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
