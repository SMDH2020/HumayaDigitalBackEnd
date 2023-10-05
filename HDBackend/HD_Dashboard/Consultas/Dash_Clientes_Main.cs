using Dapper;
using HD.AccesoDatos;
using HD_Dashboard.Modelos.Clientes;

namespace HD_Dashboard.Consultas
{
    public class Dash_Clientes_Main
    {
        private string CadenaConexion;
        public Dash_Clientes_Main(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlDashboard_Clientes>> Dashboard(int idcliente)
        {
            try
            {
                var parametros = new
                {
                    idcliente
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlDashboard_Clientes> result = await factory.SQL.QueryAsync<mdlDashboard_Clientes>("Dashboard.sp_Clientes_general", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
