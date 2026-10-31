using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;

namespace HD_GestionProyectosTI.Consultas.Priorizacion
{
    public class AD_TI_Funcionalidades_ContextoCorreo
    {
        private string CadenaConexion;
        public AD_TI_Funcionalidades_ContextoCorreo(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<mdl_TI_FuncionalidadContextoCorreo?> Obtener(long funcionalidadId)
        {
            try
            {
                var parametros = new { FuncionalidadId = funcionalidadId };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                mdl_TI_FuncionalidadContextoCorreo? result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_TI_FuncionalidadContextoCorreo>(
                    "dbo.sp_TI_Funcionalidades_ContextoCorreo", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
