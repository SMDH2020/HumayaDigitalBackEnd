using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.GestionCobranza.Modelos;
using HD_Cobranza.Modelos;

namespace HD_Cobranza.GestionCobranza.Capturas
{
    public class AD_Obtener_Convenios_Cliente_Gestionar
    {
        private string CadenaConexion;
        public AD_Obtener_Convenios_Cliente_Gestionar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Obtener_Convenios_Cliente_Gestionar>> ObtenerConvenios(int idcliente, int card)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente,
                    card
                };

                IEnumerable<mdl_Obtener_Convenios_Cliente_Gestionar> result = await factory.SQL.QueryAsync<mdl_Obtener_Convenios_Cliente_Gestionar>("Cartera_Clientes.Cobranza.sp_Obtener_Facturas_ClienteGestionar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdl_Obtener_Convenios_Cliente_Gestionar>> ObtenerConveniosOperacion(int idcliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente
                };

                IEnumerable<mdl_Obtener_Convenios_Cliente_Gestionar> result = await factory.SQL.QueryAsync<mdl_Obtener_Convenios_Cliente_Gestionar>("GestionCobranza.sp_Obtener_Convenios_Cliente_Gestionar_Operacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdl_Obtener_Convenios_Cliente_Gestionar>> ObtenerConveniosRevolvente(int idcliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente
                };

                IEnumerable<mdl_Obtener_Convenios_Cliente_Gestionar> result = await factory.SQL.QueryAsync<mdl_Obtener_Convenios_Cliente_Gestionar>("GestionCobranza.sp_Obtener_Convenios_Cliente_Gestionar_Revolvente", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdlPedidos_Facturados>> ObtenerInformacionCreditoFactura(int cliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    cliente = cliente
                };

                IEnumerable<mdlPedidos_Facturados> result = await factory.SQL.QueryAsync<mdlPedidos_Facturados>("GestionCobranza.sp_Facturas_Obtener_Informacion_Credito", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
