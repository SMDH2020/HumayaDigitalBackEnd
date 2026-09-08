using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.SolicitudesCerradas;

namespace HD_Ventas.Consultas
{
    public class AD_Operaciones_Cerradas_Cards
    {
        private string CadenaConexion;
        public AD_Operaciones_Cerradas_Cards(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdlOperacionesCerradasCards>> Obtener(mdlOperacionesCerradasCardsView mdl)
        {
            try
            {
                var parametros = new
                {
                    ejercicio = mdl.ejercicio,
                    periodo = mdl.periodo,
                    linea = mdl.linea,
                    card = mdl.card,
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlOperacionesCerradasCards> result = await factory.SQL.QueryAsync<mdlOperacionesCerradasCards>("Ventas.sp_Resultado_operaciones_card", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
