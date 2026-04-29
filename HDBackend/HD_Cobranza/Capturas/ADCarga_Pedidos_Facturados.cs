using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;
using HD_Cobranza.Modelos.PlantillaKarbot;

namespace HD_Cobranza.Capturas
{
    public class ADCarga_Pedidos_Facturados
    {
        private string CadenaConexion;
        public ADCarga_Pedidos_Facturados(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdlPedidos_Facturados>> Pedidos(int ejercicio, int periodo, string adr, string sucursales, string operacion)
        {
            try
            {
                var parametros = new
                {
                    ejercicio = ejercicio,
                    periodo = periodo,
                    adr = adr,
                    sucursales = sucursales,
                    operacion = operacion
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlPedidos_Facturados> result = await factory.SQL.QueryAsync<mdlPedidos_Facturados>("Credito.sp_Pedidos_Facturados_Intereses", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
