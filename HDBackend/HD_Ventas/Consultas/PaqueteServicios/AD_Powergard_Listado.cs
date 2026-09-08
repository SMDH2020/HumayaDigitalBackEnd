using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.PaqueteServicios;

namespace HD_Ventas.Consultas.PaqueteServicios
{
    public class AD_Powergard_Listado
    {
        private string CadenaConexion;
        public AD_Powergard_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Powergard_Listado>> Listado(int ejercicioInicio, int periodoInicio, int ejercicioFin, int periodoFin, string? region, string? sucursal, string? vendedor, string usuario)
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
                IEnumerable<mdl_Powergard_Listado> result = await factory.SQL.QueryAsync<mdl_Powergard_Listado>("Ventas.sp_Powergard_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
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
