using Dapper;
using HD.AccesoDatos;
using HD_Finanzas.Modelos.Estado_Resultados;

namespace HD_Finanzas.AccesoDatos
{
    public class FAD_EstadiResultados_Grafica
    {
        private string CadenaConexion;
        public FAD_EstadiResultados_Grafica(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<Fmdl_Estado_Resultados_Grafica>> EstadoResultadosGrafica(Fmdl_Estado_Resultados_Grafica_Filtro mdl)
        {
            try
            {
                var parametros = new
                {
                    ejercicio = mdl.ejercicio,
                    ejerciciofin = mdl.ejerciciofin,
                    periodo = mdl.periodo,
                    periodofin = mdl.periodofin,
                    adr = mdl.adr,
                    sucursales = mdl.sucursales,
                    departamentos = mdl.departamentos,
                    usuario = 1
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<Fmdl_Estado_Resultados_Grafica> result = await factory.SQL.QueryAsync<Fmdl_Estado_Resultados_Grafica>("PixelCode.dbo.SP_Get_EstadoResultadosDetalle_Graficas", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
