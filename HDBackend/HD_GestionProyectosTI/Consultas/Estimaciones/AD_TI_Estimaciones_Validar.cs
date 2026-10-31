using Dapper;
using HD.AccesoDatos;
using System.Data.SqlClient;

namespace HD_GestionProyectosTI.Consultas.Estimaciones
{
    public class AD_TI_Estimaciones_Validar
    {
        private string CadenaConexion;
        public AD_TI_Estimaciones_Validar(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task Validar(long estimacionId, int usuarioId)
        {
            try
            {
                var parametros = new { estimacionId, usuarioId };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync(
                    "dbo.sp_TI_Estimaciones_Validar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
