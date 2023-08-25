using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.ClientesDatosPersonaFisica
{
    public class AD_ClientesDatosPersonaFisica_Listado
    {
        private string CadenaConexion;
        public AD_ClientesDatosPersonaFisica_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlClientes_Datos_Persona_Fisica>> Listado(short filtrar)
        {
            try
            {
                var parametros = new
                {
                    filtrar
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlClientes_Datos_Persona_Fisica> result = await factory.SQL.QueryAsync<mdlClientes_Datos_Persona_Fisica>("Credito.sp_clientes_datos_persona_fisica_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
