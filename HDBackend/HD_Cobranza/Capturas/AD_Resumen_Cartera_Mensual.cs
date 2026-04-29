using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;

namespace HD_Cobranza.Capturas
{
    public class AD_Resumen_Cartera_Mensual
    {
        private string CadenaConexion;
        public AD_Resumen_Cartera_Mensual(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlResumenCarteraMensual>> Obtener(int ejercicio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    ejercicio = ejercicio,
                };

                var result = await factory.SQL.QueryAsync<mdlResumenCarteraMensual>("Cartera_Clientes.dbo.sp_obtenere_Resumen_Cartera_Mensual", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
