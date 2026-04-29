using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.PaqueteServicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Consultas.PaqueteServicios
{
    public class AD_Poliza_Mantenimiento_ObtenerID
    {
        private string CadenaConexion;
        public AD_Poliza_Mantenimiento_ObtenerID(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Poliza_Mantenimiento_Listado> Listado(int idpoliza)
        {
            try
            {
                var parametros = new
                {
                    idpoliza = idpoliza,
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                mdl_Poliza_Mantenimiento_Listado result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_Poliza_Mantenimiento_Listado>("Ventas.sp_Poliza_Mantenimiento_ObtenerID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
