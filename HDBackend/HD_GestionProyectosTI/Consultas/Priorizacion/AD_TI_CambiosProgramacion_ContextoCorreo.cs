using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;

namespace HD_GestionProyectosTI.Consultas.Priorizacion
{
    public class AD_TI_CambiosProgramacion_ContextoCorreo
    {
        private string CadenaConexion;
        public AD_TI_CambiosProgramacion_ContextoCorreo(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<mdl_TI_CambioProgramacionContextoCorreo?> Obtener(long cambioProgramacionId)
        {
            try
            {
                var parametros = new { CambioProgramacionId = cambioProgramacionId };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                mdl_TI_CambioProgramacionContextoCorreo? result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_TI_CambioProgramacionContextoCorreo>(
                    "dbo.sp_TI_CambiosProgramacion_ContextoCorreo", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
