using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using HD.Notifications.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usados.Consultas.Usados;

namespace HD.Notifications.Consultas
{
    public class AD_HD_Notificaciones_ObtenerporID
    {
        private string CadenaConexion;
        public AD_HD_Notificaciones_ObtenerporID(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_HD_Notificaciones_Listado> obtenerID(int iddetalle)
        {
            try
            {
                var parametros = new
                {
                    iddetalle = iddetalle
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                mdl_HD_Notificaciones_Listado result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_HD_Notificaciones_Listado>("HumayaDigital_Eventos.dbo.sp_HD_Notificaciones_ObtenerporID", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_HD_Notificaciones_Listado>> obtenerListadoDetalle(int idencabezado)
        {
            try
            {
                var parametros = new
                {
                    idencabezado = idencabezado
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_HD_Notificaciones_Listado> result = await factory.SQL.QueryAsync<mdl_HD_Notificaciones_Listado>("HumayaDigital_Eventos.dbo.sp_HD_Notificaciones_ListadoPorID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
