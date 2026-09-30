using Dapper;
using HD.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Notifications.Consultas
{
    public class AD_HD_Notificaciones_Eliminar_Notificacion
    {
        private string CadenaConexion;
        public AD_HD_Notificaciones_Eliminar_Notificacion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Eliminar(int idencabezado)
        {
            try
            {
                var parametros = new
                {
                    idencabezado = idencabezado,
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("HumayaDigital_Eventos.dbo.sp_HD_Notificaciones_Eliminar_Notificacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
