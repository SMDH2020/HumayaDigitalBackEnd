using Dapper;
using HD.AccesoDatos;
using HD.Notifications.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usados.Consultas.Inventario;

namespace HD.Notifications.Consultas
{
    public class AD_HD_Notificaciones_Guardar
    {
        private string CadenaConexion;
        public AD_HD_Notificaciones_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
       
        public async Task<bool> Guardar(mdl_HD_Notificaciones mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idnotificacion = mdl.idnotificacion,
                    mensaje = mdl.mensaje,
                    fecha_evento = mdl.fecha_evento,
                    repite = mdl.repite,
                    dias = mdl.dias,
                    usuario = mdl.usuario,
                };
                await factory.SQL.QueryAsync("HumayaDigital_Eventos.dbo.sp_HD_Notificaciones_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
