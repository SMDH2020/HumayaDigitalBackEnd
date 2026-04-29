using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;
using HD_Cobranza.Modelos.ConvenioPago;

namespace HD_Cobranza.Capturas.ConvenioPago
{
    public class AD_Clientes_Datos_Contacto_Cargar
    {
        private string CadenaConexion;
        public AD_Clientes_Datos_Contacto_Cargar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdlClientes_Datos_Contacto>> Listado(int idcliente)
        {
            try
            {
                var parametros = new
                {
                    idcliente = idcliente
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlClientes_Datos_Contacto> result = await factory.SQL.QueryAsync<mdlClientes_Datos_Contacto>("Credito.sp_Clientes_Datos_Contacto_Buscar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
