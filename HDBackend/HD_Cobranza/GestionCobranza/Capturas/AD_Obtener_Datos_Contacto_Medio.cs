using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.GestionCobranza.Modelos;


namespace HD_Cobranza.GestionCobranza.Capturas
{
    public class AD_Obtener_Datos_Contacto_Medio
    {
        private string CadenaConexion;
        public AD_Obtener_Datos_Contacto_Medio(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Obtener_Datos_Contacto_Cliente>> Datos(int idmedio)
        {
            try
            {
                var parametros = new
                {
                    idmedio = idmedio
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Obtener_Datos_Contacto_Cliente> result = await factory.SQL.QueryAsync<mdl_Obtener_Datos_Contacto_Cliente>("GestionCobranza.sp_Obtener_Datos_Contacto_Medio", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
