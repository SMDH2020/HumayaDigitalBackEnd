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
    public class AD_HD_Notificaciones_Listado_Filtrado
    {
        private string CadenaConexion;
        public AD_HD_Notificaciones_Listado_Filtrado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_HD_Notificaciones_Listado>> Listado()
        {
            try
            {
                var parametros = new
                {
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_HD_Notificaciones_Listado> result = await factory.SQL.QueryAsync<mdl_HD_Notificaciones_Listado>("HumayaDigital_Eventos.dbo.sp_HD_Notificaciones_Filtrado_Nuevo", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
