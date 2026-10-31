using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;
using System.Data.SqlClient;

namespace HD_GestionProyectosTI.Consultas.VersionesAlcance
{
    public class AD_TI_Funcionalidades_Obtener
    {
        private string CadenaConexion;
        public AD_TI_Funcionalidades_Obtener(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<mdl_TI_FuncionalidadDetalle?> Obtener(long funcionalidadId)
        {
            try
            {
                var parametros = new { funcionalidadId };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                mdl_TI_FuncionalidadDetalle? result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_TI_FuncionalidadDetalle>(
                    "dbo.sp_TI_Funcionalidades_Obtener", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
