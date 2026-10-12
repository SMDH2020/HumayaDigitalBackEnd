using Dapper;
using HD.AccesoDatos;
using Postventa.Modelos.OrdenesAbiertas;
using System.Linq;

namespace Postventa.Consultas.OrdenesAbiertas
{
    public class AD_DashboardOTAbiertas
    {
        private string CadenaConexion;
        public AD_DashboardOTAbiertas(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<object> ObtenerOrdenesAbiertas(int usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new { usuario = usuario };

                using (var multi = await factory.SQL.QueryMultipleAsync(
                    "EQUIP.servicio.sp_Obtener_Ordenes_Abiertas",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure))
                {
                    var resumen = await multi.ReadFirstOrDefaultAsync<mdl_DashboardOTAbiertas_Resumen>();
                    var detalle = await multi.ReadAsync<mdl_DashboardOTAbiertas_Detalle>();
                    var detalleTipo = await multi.ReadAsync<mdl_DashboardOTAbiertas_DetalleTipo>();

                    factory.SQL.Close();

                    return new { resumen, detalle, detalleTipo };
                }
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}