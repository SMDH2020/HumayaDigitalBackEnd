using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.InteresMensual
{
    public class AD_InteresMensual_ObtenerporID
    {
        private string CadenaConexion;
        public AD_InteresMensual_ObtenerporID(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlInteres_Mensual> BuscarID(int idinteres)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idinteres
                };
                mdlInteres_Mensual result = await factory.SQL.QueryFirstOrDefaultAsync<mdlInteres_Mensual>("Credito.sp_Interes_Mensual_ObtenerporID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
