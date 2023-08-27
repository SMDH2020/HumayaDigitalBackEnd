using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.ClientesDatosPersonaFisica
{
    public class AD_ClientesDatosPersonaFisica_BuscarID
    {
        private string CadenaConexion;
        public AD_ClientesDatosPersonaFisica_BuscarID(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlClientes_Datos_Persona_Fisica> BuscarID(int idcliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente
                };
                mdlClientes_Datos_Persona_Fisica result = await factory.SQL.QueryFirstOrDefaultAsync<mdlClientes_Datos_Persona_Fisica>("Credito.sp_clientes_datos_persona_fisica_obtenerporID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
