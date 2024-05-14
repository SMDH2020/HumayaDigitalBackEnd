using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;

namespace HD_Cobranza.Modelos.ReporteRecuperacionCartera
{
    public class AD_ReporteRecuperacionCartera_Obtener
    {
        private string CadenaConexion;
        public AD_ReporteRecuperacionCartera_Obtener(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdlReporteRecuperacionCartera_Obtener>> Listado(DateTime fechainicio, DateTime fechafinal)
        {
            try
            {
                var parametros = new
                {
                    @Fecha_Inicio = fechainicio,
                    @Fecha_Fin = fechafinal
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlReporteRecuperacionCartera_Obtener> result = await factory.SQL.QueryAsync<mdlReporteRecuperacionCartera_Obtener>("EQUIP.Cobranza.sp_Reporte_Recuperacion_Cartera", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                List<mdlReporteRecuperacionCartera_Obtener> listado = result.ToList();
                //if (result.Count() > 0)
                //    listado.Add(new mdlReporteRecuperacionCartera_Obtener()
                //    {
                //        sucursal = result.First().sucursal,
                //        codigocliente = result.First().codigocliente,
                //        razonsocial = result.First().razonsocial,
                //        factura = result.First().factura,
                //        importe = result.First().importe,
                //        pago = result.First().pago,
                //        fecha = result.First().fecha,
                //        fechapago = result.First().fechapago,
                //        dias = result.First().dias
                //    });
                return listado;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
