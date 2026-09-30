using Dapper;
using HD.AccesoDatos;
using HD_Dashboard.Modelos;

namespace HD_Dashboard.Consultas.Vendedor
{
    public class VenScorecard
    {
        private string CadenaConexion;
        public VenScorecard(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Scorecard>> Listado(string? usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    usuario
                };

                IEnumerable<mdl_Scorecard> result = await factory.SQL.QueryAsync<mdl_Scorecard>("Ventas.sp_Get_Scordcar_por_Usuario", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
