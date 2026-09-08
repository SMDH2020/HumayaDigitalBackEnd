using Dapper;
using HD.AccesoDatos;
using HD_Mensajeria.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Mensajeria.Consultas
{
    public class AD_MEnsajeria_Enviada_Listado
    {
        private string CadenaConexion;
        public AD_MEnsajeria_Enviada_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Mensajeria_Enviada>> mensajeriaListado(string fechainicio, string fechafin, string seccion)
        {
            try
            {
                var parametros = new
                {
                    fechainicio = fechainicio,
                    fechafin = fechafin,
                    seccion = seccion,
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Mensajeria_Enviada> result = await factory.SQL.QueryAsync<mdl_Mensajeria_Enviada>("HD_Mensajeria.dbo.sp_Mensajeria_Enviada", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Mensajeria_Enviados_Detalle>> mensajeriaDetalle(string fecha, string seccion)
        {
            try
            {
                var parametros = new
                {
                    fecha = fecha,
                    seccion = seccion,
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Mensajeria_Enviados_Detalle> result = await factory.SQL.QueryAsync<mdl_Mensajeria_Enviados_Detalle>("HD_Mensajeria.dbo.sp_Mensajeria_Enviada_Detalle", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
