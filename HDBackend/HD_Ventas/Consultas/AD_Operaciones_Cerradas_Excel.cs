using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.SolicitudesCerradas;

namespace HD_Ventas.Consultas
{
    public class AD_Operaciones_Cerradas_Excel
    {
        private string CadenaConexion;
        public AD_Operaciones_Cerradas_Excel(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlOperacionesDetalle>> Listado(int ejercicio, int periodo, string linea, string card)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    ejercicio,
                    periodo,
                    linea,
                    card
                };
                var result = await factory.SQL.QueryAsync<mdlOperacionesDetalle>("Ventas.sp_Resultado_operaciones_card_Detalle_Excel", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();

                List<mdlOperacionesDetalle> listado = result.ToList();

                return listado;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
