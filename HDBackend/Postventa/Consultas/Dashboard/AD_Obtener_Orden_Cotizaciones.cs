using Dapper;
using HD.AccesoDatos;
using Postventa.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Consultas.Dashboard
{
    public class AD_Obtener_Orden_Cotizaciones
    {
        private string CadenaConexion;
        public AD_Obtener_Orden_Cotizaciones(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_orden_Cotizacion>> obtenerOrden(int folio)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("folio", folio, System.Data.DbType.Int32);

                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_orden_Cotizacion> result = await factory.SQL.QueryAsync<mdl_orden_Cotizacion>("Postventa.sp_cotizaciones_obtener_orden", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
