using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.InteresMensual
{
    public class AD_InteresMensual_Listado
    {
        private string CadenaConexion;
        public AD_InteresMensual_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlInteres_Mensual>> Listado(int ejercicio)
        {
            try
            {
                var parametros = new
                {
                    ejercicio
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlInteres_Mensual> result = await factory.SQL.QueryAsync<mdlInteres_Mensual>("Credito.sp_Interes_Mensual_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
