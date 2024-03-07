using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos.RecuperacionCartera;

namespace HD_Cobranza.Capturas.RecuperacionCartera
{
    public class ADcob_RecuperacionCartera
    {
        private string CadenaConexion;
        public ADcob_RecuperacionCartera(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlCob_RecuperacionCartera>> Obtener(DateTime _fechainicio, DateTime _fechafin)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @fechainicio = _fechainicio,
                    @fechafin = _fechafin
                };

                
                IEnumerable<mdlCob_RecuperacionCartera> result = await factory.SQL.QueryAsync<mdlCob_RecuperacionCartera>("Cobranza.sp_Recuperacion_Cartera", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                
                List<mdlCob_RecuperacionCartera> reporte = result.ToList();
                double vencido = result.Sum(item => item.vencido);
                double recuperado_vencido = result.Sum(item => item.recuperado_vencido);
                double por_vencido = vencido == 0 || recuperado_vencido == 0 ? 0 :
                  Math.Round(recuperado_vencido / vencido * 100, 2);

                double porvencer = result.Sum(item => item.porvencer);
                double recuperado_porvencer = result.Sum(item => item.recuperado_porvencer);
                double por_porvencer = porvencer == 0 || recuperado_porvencer == 0 ? 0 :
                    Math.Round(recuperado_porvencer / porvencer * 100, 2);
                    ;

                double activo = result.Sum(item => item.activo);
                double recuperado_activo = result.Sum(item => item.recuperado_activo);
                double por_activo = activo == 0 || recuperado_activo == 0 ? 0 :
                    Math.Round(recuperado_activo / activo * 100, 2);

                reporte.Add(new mdlCob_RecuperacionCartera
                {
                    sucursal="TOTAL",
                    vencido=vencido,
                    recuperado_vencido= recuperado_vencido,
                    por_vencido= por_vencido,

                    porvencer= porvencer,
                    recuperado_porvencer= recuperado_porvencer,
                    por_porvencer= por_porvencer,

                    activo= activo,
                    recuperado_activo= recuperado_activo,
                    por_activo= por_activo,

                });
                return reporte;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
