using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.Cultivos
{
    public class AD_Cultivos_BuscarID
    {
        private string CadenaConexion;
        public AD_Cultivos_BuscarID(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlCultivos> BuscarID(int idcultivo)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcultivo
                };
                mdlCultivos result = await factory.SQL.QueryFirstOrDefaultAsync<mdlCultivos>("Credito.sp_cultivos_obtenerporID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
