using Dapper;
using HD.AccesoDatos;
using HD.Notifications.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Notifications.Consultas
{
    public class AD_Modulos_Redireccion_Listado
    {
        private string CadenaConexion;
        public AD_Modulos_Redireccion_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Notificaciones_Opciones_DDL> Listado()
        {
            try
            {
                var parametros = new
                {
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                //IEnumerable<mdl_Modulos_Redireccion> result = await factory.SQL.QueryAsync<mdl_Modulos_Redireccion>("HumayaDigital_Eventos.dbo.sp_Modulos_Redireccion_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
                var result = await factory.SQL.QueryMultipleAsync("HumayaDigital_Eventos.dbo.sp_Modulos_Redireccion_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Notificaciones_Opciones_DDL mdl = new mdl_Notificaciones_Opciones_DDL();
                mdl.redirecciones = result.Read<mdl_Modulos_Redireccion>().ToList();
                mdl.departamentos = result.Read<mdl_Departamentos_DDL>().ToList();
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
