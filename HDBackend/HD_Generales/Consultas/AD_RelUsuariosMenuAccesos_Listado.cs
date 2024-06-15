using Dapper;
using HD.AccesoDatos;
using HD.Generales.Autenticate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Generales.Consultas
{
    public class AD_RelUsuariosMenuAccesos_Listado
    {
        private string CadenaConexion;
        public AD_RelUsuariosMenuAccesos_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlUsuarioMenuAccesos>> AccesosMenus(int idmodulo, string idusuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idmodulo,
                    idusuario
                };
                IEnumerable<mdlUsuarioMenuAccesos> result = await factory.SQL.QueryAsync<mdlUsuarioMenuAccesos>("humayadigital_usuarios.dbo.sp_Usuarios_Menu_Accesos_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
