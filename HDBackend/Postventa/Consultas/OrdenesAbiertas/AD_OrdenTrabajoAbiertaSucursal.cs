using Dapper;
using HD.AccesoDatos;
using Postventa.Modelos.OrdenesAbiertas;

namespace Postventa.Consultas.OrdenesAbiertas
{
    public class AD_OrdenTrabajoAbiertaSucursal
    {
        private string CadenaConexion;
        public AD_OrdenTrabajoAbiertaSucursal(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_OrdenTrabajoAbiertaSucursal_Detalle>> ObtenerOrdenesAbiertasSucursal(int usuario, int sucursal)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new { usuario = usuario, idsucursal = sucursal };

                IEnumerable<mdl_OrdenTrabajoAbiertaSucursal_Detalle> result = await factory.SQL.QueryAsync<mdl_OrdenTrabajoAbiertaSucursal_Detalle>(
                    "EQUIP.servicio.sp_Obtener_Ordenes_Abiertas_sucursal",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure);

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