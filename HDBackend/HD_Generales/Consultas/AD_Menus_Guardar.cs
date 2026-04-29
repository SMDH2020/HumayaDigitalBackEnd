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
    public class AD_Menus_Guardar
    {
        private string CadenaConexion;
        public AD_Menus_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlMenu>> Guardar(mdlMenu mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idmenu = mdl.idmenu,
                    @idmodulo = mdl.idmodulo,
                    @descripcion = mdl.descripcion,
                    @nomenclatura = mdl.nomenclatura,
                    @estatus = mdl.estatus,
                    @ruta=mdl.ruta,
                    @usuario = mdl.usuario
                };

                var result = await factory.SQL.QueryAsync<mdlMenu>("humayadigital_usuarios.dbo.sp_Menus_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
