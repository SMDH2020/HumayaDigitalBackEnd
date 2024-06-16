using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;

namespace HD_Cobranza.Capturas
{
    public class ADRecuperacionCarteraMensual
    {
        private string CadenaConexion;
        public ADRecuperacionCarteraMensual(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlRecuperacionCarteraMenusal>> Obtener(int ejercicio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    ejercicio = ejercicio,
                };

                var result = await factory.SQL.QueryAsync<mdlRecuperacionCarteraMenusal>("Cartera_Clientes.dbo.sp_obtenere_Recuperacion_Cartera_Mensual", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
