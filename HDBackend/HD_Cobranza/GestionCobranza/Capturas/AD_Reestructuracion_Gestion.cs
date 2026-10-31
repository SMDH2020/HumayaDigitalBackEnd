using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.GestionCobranza.Modelos;

namespace HD_Cobranza.GestionCobranza.Capturas
{
    public class AD_Reestructuracion_Gestion
    {
        private string CadenaConexion;
        public AD_Reestructuracion_Gestion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Listado_Reestructuracion_Gestion>> Get(int idcliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente = idcliente

                };
                IEnumerable<mdl_Listado_Reestructuracion_Gestion> result = await factory.SQL.QueryAsync<mdl_Listado_Reestructuracion_Gestion>("GestionCobranza.sp_Obtener_Reestructuraciones_Cliente", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
