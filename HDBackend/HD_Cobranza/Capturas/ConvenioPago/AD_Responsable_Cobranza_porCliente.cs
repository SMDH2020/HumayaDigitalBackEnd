using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;
using HD_Cobranza.Modelos.ConvenioPago;

namespace HD_Cobranza.Capturas.ConvenioPago
{
    public class AD_Responsable_Cobranza_porCliente
    {
        private string CadenaConexion;
        public AD_Responsable_Cobranza_porCliente(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdlResponsable_Cobranza_porCliente>> Cliente(int idcliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idcliente = idcliente,
                };
                IEnumerable<mdlResponsable_Cobranza_porCliente> result = await factory.SQL.QueryAsync<mdlResponsable_Cobranza_porCliente>("Credito.sp_Responsable_Cobranza_porCliente", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
