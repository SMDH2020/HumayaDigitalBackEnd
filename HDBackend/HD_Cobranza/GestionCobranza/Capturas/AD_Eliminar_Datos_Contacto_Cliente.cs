using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.GestionCobranza.Modelos;

namespace HD_Cobranza.GestionCobranza.Capturas
{
    public class AD_Eliminar_Datos_Contacto_Cliente
    {
        private string CadenaConexion;
        public AD_Eliminar_Datos_Contacto_Cliente(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Obtener_Datos_Contacto_Cliente>> idmedio(int idmedio)
        {
            try
            {
                var parametros = new
                {
                    idmedio = idmedio
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Obtener_Datos_Contacto_Cliente> result = await factory.SQL.QueryAsync<mdl_Obtener_Datos_Contacto_Cliente>("GestionCobranza.Eliminar_Contacto_Cliente", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
