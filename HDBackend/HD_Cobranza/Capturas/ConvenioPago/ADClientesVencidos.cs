using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos.ConvenioPago;

namespace HD_Cobranza.Capturas.ConvenioPago
{
    public class ADClientesVencidos
    {
        private string CadenaConexion;
        public ADClientesVencidos(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlClientesVencidos>> ClientesVencidos(int dias=1)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    dias
                };

                IEnumerable<mdlClientesVencidos> result =await factory.SQL.QueryAsync<mdlClientesVencidos>("Cobranza.sp_Clientes_Vencidos", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
