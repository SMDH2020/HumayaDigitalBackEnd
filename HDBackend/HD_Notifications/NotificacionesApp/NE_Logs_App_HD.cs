using Dapper;
using HD.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Notifications.NotificacionesApp
{
    public class NE_Logs_App_HD
    {
        private string CadenaConexion;
        public NE_Logs_App_HD(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task Guardar(string accion, string origen, string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {

                    accion = accion,
                    usuario = usuario, 
                    origen = origen
                };
                await factory.SQL.QueryAsync("HumayaDigital_Eventos.dbo.sp_Guardar_Evento_App_HD", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
            }
            catch 
            {
                //throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
