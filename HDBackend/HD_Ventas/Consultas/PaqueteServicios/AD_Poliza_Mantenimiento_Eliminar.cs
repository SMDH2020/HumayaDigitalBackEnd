using Dapper;
using HD.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Consultas.PaqueteServicios
{
    public class AD_Poliza_Mantenimiento_Eliminar
    {
        private string CadenaConexion;
        public AD_Poliza_Mantenimiento_Eliminar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> eliminar(int idpoliza)
        {
            try
            {
                var parametros = new
                {
                    idpoliza = idpoliza,
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("Ventas.sp_Poliza_Mantenimiento_Eliminar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
