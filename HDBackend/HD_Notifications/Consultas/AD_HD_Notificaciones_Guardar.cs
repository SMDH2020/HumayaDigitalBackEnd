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
                    idencabezado = mdl.idencabezado,
                    mensaje = mdl.mensaje,
                    //fecha_evento = mdl.fecha_evento,
                    fecha_inicio = mdl.fecha_inicio,
                    fecha_fin = mdl.fecha_fin,
                    redireccion = mdl.redireccion,
                    tipo = mdl.tipo,
                    @dia = mdl.dia,
                    hora = mdl.hora,
                    //duracion = mdl.duracion,
                    usuario = mdl.usuario,
                    iddepartamento = mdl.iddepartamento
                };
                await factory.SQL.QueryAsync("HumayaDigital_Eventos.dbo.sp_HD_Notificaciones_Guardar_Nuevo", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_HD_Notificaciones_Listado> GuardarInstantanea(mdl_HD_Notificaciones mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idencabezado = mdl.idencabezado,
                    mensaje = mdl.mensaje,
                    //fecha_evento = mdl.fecha_evento,
                    fecha_inicio = mdl.fecha_inicio,
                    fecha_fin = mdl.fecha_fin,
                    redireccion = mdl.redireccion,
                    tipo = mdl.tipo,
                    @dia = mdl.dia,
                    hora = mdl.hora,
                    //duracion = mdl.duracion,
                    usuario = mdl.usuario,
                };
                mdl_HD_Notificaciones_Listado result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_HD_Notificaciones_Listado>("HumayaDigital_Eventos.dbo.sp_HD_Notificaciones_Guardar_Nuevo", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
