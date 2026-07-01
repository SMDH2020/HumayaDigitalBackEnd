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
    public class AD_Poliza_Mantenimiento_Listado
    {
        private string CadenaConexion;
        public AD_Poliza_Mantenimiento_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Poliza_Mantenimiento_Listado>> Listado(int ejercicioInicio, int periodoInicio, int ejercicioFin, int periodoFin, string? region, string? sucursal, string? vendedor,string usuario)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {
                var parametros = new
                {
                    ejercicioInicio = ejercicioInicio,
                    periodoInicio = periodoInicio,
                    ejercicioFin = ejercicioFin,
                    periodoFin = periodoFin,
                    region = region,
                    sucursal = sucursal,
                    vendedor = vendedor,
                    usuario
                };
                IEnumerable<mdl_Poliza_Mantenimiento_Listado> result = await factory.SQL.QueryAsync<mdl_Poliza_Mantenimiento_Listado>("Ventas.sp_Poliza_Mantenimiento_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
            finally
            {
                factory.SQL.Close();
            }
        }
    }
}
