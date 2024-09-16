using Dapper;
using HD.AccesoDatos;
using HD_Administracion.Modelos;

namespace HD_Administracion.Consultas
{
    public class AD_Carga_Flujo_Efectivo_Semanal
    {
        private string CadenaConexion;
        public AD_Carga_Flujo_Efectivo_Semanal(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdlCargaFlujoEfectivoSemanal>> FlujoEfectivo(int ejercicio, int periodo)
        {
            try
            {
                var parametros = new
                {
                    ejercicio = ejercicio,
                    periodo = periodo
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlCargaFlujoEfectivoSemanal> result = await factory.SQL.QueryAsync<mdlCargaFlujoEfectivoSemanal>("Administracion.sp_Flujo_Efectivo_Semanal_Mostrar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
