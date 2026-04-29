using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.GestionCobranza.Modelos;


namespace HD_Cobranza.GestionCobranza.Capturas
{
    public class AD_Mostrar_Contacto_Cliente
    {
        private string CadenaConexion;
        public AD_Mostrar_Contacto_Cliente(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Mostrar_Contacto_Cliente>> Contacto(int idcliente, string medio)
        {
            try
            {
                var parametros = new
                {
                    @idcliente = idcliente,
                    @medio = medio
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Mostrar_Contacto_Cliente> result = await factory.SQL.QueryAsync<mdl_Mostrar_Contacto_Cliente>("GestionCobranza.sp_Mostrar_Contacto_Cliente", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
