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

        public async Task<mdl_Contactos_Mensajeria_View> obtenerContactos(int idusuario)
        {
            try
            {
                var parametros = new
                {
                    idusuario = idusuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("HD_Mensajeria.dbo.sp_Obtener_Listado_Clientes_Contactados_2", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Contactos_Mensajeria_View mdl = new mdl_Contactos_Mensajeria_View();
                mdl.postventa = result.Read<mdl_Contactos_Mensajeria_Menu>().ToList();
                mdl.cobranza = result.Read<mdl_Contactos_Mensajeria_Menu>().ToList();
                factory.SQL.Close();
                return mdl;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
