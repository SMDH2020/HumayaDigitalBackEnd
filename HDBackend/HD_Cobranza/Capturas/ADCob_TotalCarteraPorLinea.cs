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
                var result = await factory.SQL.QueryAsync<mdlCob_TotalCarteraPorLinea>("Equip.Credito.sp_obtener_resumen_cartera_por_linea_Sucursal", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();

                List<mdlCob_TotalCarteraPorLinea> listado = result.ToList();
                if(idsucursal == 0)
                {
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
                        });
                    }
                    foreach (var linea in result.GroupBy(x => x.linea))
                    {
                        listado.Add(new mdlCob_TotalCarteraPorLinea()
                        {
                            idsucursal = 100,
                            sucursal = "RESUMEN DE CARTERA POR LINEA",
                            linea = result.Where(x => x.linea == linea.Key).First().linea,
                            mas90 = result.Where(x => x.linea == linea.Key).Sum(x => x.mas90),
                            mas60 = result.Where(x => x.linea == linea.Key).Sum(x => x.mas60),
                            mas30 = result.Where(x => x.linea == linea.Key).Sum(x => x.mas30),
                            mas15 = result.Where(x => x.linea == linea.Key).Sum(x => x.mas15),
                            de1a15 = result.Where(x => x.linea == linea.Key).Sum(x => x.de1a15),
                            vencido = result.Where(x => x.linea == linea.Key).Sum(x => x.vencido),
                            porvencer = result.Where(x => x.linea == linea.Key).Sum(x => x.porvencer),
                            totalcartera = result.Where(x => x.linea == linea.Key).Sum(x => x.totalcartera),
                            saldoafavor = result.Where(x => x.linea == linea.Key).Sum(x => x.saldoafavor),
                            total = result.Where(x => x.linea == linea.Key).Sum(x => x.total),
                        });
                    }
                    listado.Add(new mdlCob_TotalCarteraPorLinea()
                    {
                        idsucursal = 101,
                        sucursal = "RESUMEN DE CARTERA POR LINEA",
                        linea = "TOTAL",
                        mas90 = result.Sum(x => x.mas90),
                        mas60 = result.Sum(x => x.mas60),
                        mas30 = result.Sum(x => x.mas30),
                        mas15 = result.Sum(x => x.mas15),
                        de1a15 = result.Sum(x => x.de1a15),
                        vencido = result.Sum(x => x.vencido),
                        porvencer = result.Sum(x => x.porvencer),
                        totalcartera = result.Sum(x => x.totalcartera),
                        saldoafavor = result.Sum(x => x.saldoafavor),
                        total = result.Sum(x => x.total),
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
