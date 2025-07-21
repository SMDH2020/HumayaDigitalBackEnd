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
    public class AD_Conseguir_Mensaje_Manual
    {
        private string CadenaConexion;
        public AD_Conseguir_Mensaje_Manual(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_HD_Notificaciones_Listado> obtenerID(int idencabezado, DateTime fecha_evento, string? usuario)
        {
            try
            {
                var parametros = new
                {
                    idencabezado = idencabezado,
                    fecha = fecha_evento,
                    usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                mdl_HD_Notificaciones_Listado result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_HD_Notificaciones_Listado>("HumayaDigital_Eventos.dbo.Obtener_Mensaje_Push", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
