using Dapper;
using HD.AccesoDatos;
using HD_Mensajeria.Modelos;

namespace HD_Mensajeria.Consultas
{
    public class AD_Obtener_Listado_Contactos_Mensajeria_Menu
    {
        private string CadenaConexion;
        public AD_Obtener_Listado_Contactos_Mensajeria_Menu(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Contactos_Mensajeria_Menu>> obtenerContactos(int usuario)
        {
            try
            {
                var parametros = new
                {
                    usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Contactos_Mensajeria_Menu> result = await factory.SQL.QueryAsync<mdl_Contactos_Mensajeria_Menu>("HD_Mensajeria.dbo.sp_Obtener_Listado_Clientes_Contactados", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
