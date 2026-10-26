using Dapper;
using HD.AccesoDatos;

namespace HD_GestionProyectosTI.Consultas.Solicitudes
{
    public class AD_TI_Solicitudes_EsParticipante
    {
        private string CadenaConexion;
        public AD_TI_Solicitudes_EsParticipante(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<bool> EsParticipante(long solicitudId, int usuarioId)
        {
            try
            {
                var parametros = new { solicitudId, usuarioId };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                bool result = await factory.SQL.QueryFirstAsync<bool>(
                    "dbo.sp_TI_Solicitudes_EsParticipante", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
