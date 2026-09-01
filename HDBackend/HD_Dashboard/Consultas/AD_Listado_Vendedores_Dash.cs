using Dapper;
using HD.AccesoDatos;
using HD_Dashboard.Modelos;

namespace HD_Dashboard.Consultas
{
    public class AD_Listado_Vendedores_Dash
    {
        private string CadenaConexion;
        public AD_Listado_Vendedores_Dash(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdlListado_Vendedores_Dash>> ListadoVendedores(int usuario)
        {
            try
            {
                var parametros = new
                {
                    usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlListado_Vendedores_Dash> result = await factory.SQL.QueryAsync<mdlListado_Vendedores_Dash>("dashboard.sp_Obtener_Listado_Vendedores", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
