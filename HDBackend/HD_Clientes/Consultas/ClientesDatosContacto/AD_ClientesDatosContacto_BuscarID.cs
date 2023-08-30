using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.ClientesDatosContacto
{
    public class AD_ClientesDatosContacto_BuscarID
    {
        private string CadenaConexion;
        public AD_ClientesDatosContacto_BuscarID(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlClientes_Datos_Contacto> BuscarID(int idcliente,int orden)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente,
                    orden
                };
                mdlClientes_Datos_Contacto result = await factory.SQL.QueryFirstOrDefaultAsync<mdlClientes_Datos_Contacto>("Credito.sp_clientes_datos_contacto_obtenerporID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
