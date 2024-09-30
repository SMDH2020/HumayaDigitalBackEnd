using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.GestionCobranza.Modelos;

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

                IEnumerable<mdl_Obtener_Convenios_Cliente_Gestionar> result = await factory.SQL.QueryAsync<mdl_Obtener_Convenios_Cliente_Gestionar>("GestionCobranza.sp_Obtener_Facturas_ClienteGestionar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
    }
}
