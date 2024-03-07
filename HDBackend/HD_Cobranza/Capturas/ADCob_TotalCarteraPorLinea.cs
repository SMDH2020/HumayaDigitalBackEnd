using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;

namespace HD_Cobranza.Capturas
{
    public class ADCob_TotalCarteraPorLinea
    {

        private string CadenaConexion;
        public ADCob_TotalCarteraPorLinea(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlCob_TotalCarteraPorLinea>> Listado(int idsucursal)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idsucursal
                };
                var result = await factory.SQL.QueryAsync<mdlCob_TotalCarteraPorLinea>("Credito.sp_obtener_resumen_cartera_por_lineaySucursal", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                List<mdlCob_TotalCarteraPorLinea> listado = result.ToList();
                foreach (var sucursal in result.GroupBy(x => x.idsucursal))
                {
                    listado.Add(new mdlCob_TotalCarteraPorLinea()
                    {
                        idsucursal = sucursal.Key,
                        sucursal = result.Where(x => x.idsucursal == sucursal.Key).First().sucursal,
                        linea = "TOTAL",
                        mas90 = result.Where(x => x.idsucursal == sucursal.Key).Sum(x => x.mas90),
                        mas60 = result.Where(x => x.idsucursal == sucursal.Key).Sum(x => x.mas60),
                        mas30 = result.Where(x => x.idsucursal == sucursal.Key).Sum(x => x.mas30),
                        mas15 = result.Where(x => x.idsucursal == sucursal.Key).Sum(x => x.mas15),
                        de1a15 = result.Where(x => x.idsucursal == sucursal.Key).Sum(x => x.de1a15),
                        vencido = result.Where(x => x.idsucursal == sucursal.Key).Sum(x => x.vencido),
                        porvencer = result.Where(x => x.idsucursal == sucursal.Key).Sum(x => x.porvencer),
                        totalcartera = result.Where(x => x.idsucursal == sucursal.Key).Sum(x => x.totalcartera),
                        saldoafavor = result.Where(x => x.idsucursal == sucursal.Key).Sum(x => x.saldoafavor),
                        total = result.Where(x => x.idsucursal == sucursal.Key).Sum(x => x.total),
                        activo = result.Where(x => x.idsucursal == sucursal.Key).Sum(x => x.activo),
                        juridico = result.Where(x => x.idsucursal == sucursal.Key).Sum(x => x.juridico),
                    });
                }
                
                return listado;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

    }
}
