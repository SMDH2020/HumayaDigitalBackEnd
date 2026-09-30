using Dapper;
using HD.AccesoDatos;
using HD_Buro.Modelos;

namespace HD_Buro.Consultas
{
    public class AD_Carga_Detalle_Buro_Vencido
    {
        private string CadenaConexion;
        public AD_Carga_Detalle_Buro_Vencido(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlBuroMarcadosResult> detalle_vencido(int ejercicio, int periodo, string idcliente)
        {
            try
            {
                var parametros = new
                {
                    Ejercicio = ejercicio,
                    Periodo = periodo,
                    idCliente = idcliente
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("Cartera_clientes.dbo.sp_Cargar_Detalle_Buro_Vencido", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlBuroMarcadosResult mdlresult = new mdlBuroMarcadosResult();
                mdlresult.operacion = result.Read<mdlCarga_Detalle_Buro>().ToList();
                mdlresult.revolvente = result.Read<mdlCarga_Detalle_Buro>().ToList();
                factory.SQL.Close();
                return mdlresult;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
