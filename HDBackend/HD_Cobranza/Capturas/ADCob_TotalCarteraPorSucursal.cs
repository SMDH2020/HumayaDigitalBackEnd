using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;

namespace HD_Cobranza.Capturas
{
    public class ADCob_TotalCarteraPorSucursal
    {
        private string CadenaConexion;
        public ADCob_TotalCarteraPorSucursal(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlCob_TotalCarteraPorSucursal>> Listado()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    
                };
                var result = await factory.SQL.QueryAsync<mdlCob_TotalCarteraPorSucursal>("EQUIP.Credito.sp_obtener_resumen_cartera", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();

                List<mdlCob_TotalCarteraPorSucursal> listado = result.ToList();
                listado.Add(new mdlCob_TotalCarteraPorSucursal()
                {
                    idsucursal=13,
                    sucursal="TOTALES",
                    mas90= result.Sum(x=> x.mas90),
                    mas60= result.Sum(x=> x.mas60),
                    mas30= result.Sum(x=> x.mas30),
                    mas15= result.Sum(x=> x.mas15),
                    de1a15= result.Sum(x=> x.de1a15),
                    vencido= result.Sum(x=> x.vencido),
                    porvencer= result.Sum(x=> x.porvencer),
                    totalcartera= result.Sum(x=> x.totalcartera),
                    saldoafavor= result.Sum(x=> x.saldoafavor),
                    total= result.Sum(x=> x.total),
                });

                return listado;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
