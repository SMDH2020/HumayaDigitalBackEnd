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
    public class HD_Notificaciones_Pausar_Notificacion
    {
        private string CadenaConexion;
        public HD_Notificaciones_Pausar_Notificacion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Pausar(int idencabezado, bool estatus, string usuario)
        {
            try
            {
                var parametros = new
                {
                    idencabezado=idencabezado,
                    estatus=estatus,
                    usuario=usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
               await factory.SQL.ExecuteAsync("HumayaDigital_Eventos.dbo.sp_HD_Notificaciones_Pausar_Notificacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
