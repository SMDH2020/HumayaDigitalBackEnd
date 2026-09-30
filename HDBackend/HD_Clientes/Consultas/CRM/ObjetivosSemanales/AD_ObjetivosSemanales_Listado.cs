using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.CRM.ObjetivosSemanales;

namespace HD.Clientes.Consultas.CRM.ObjetivosSemanales
{
    public class AD_ObjetivosSemanales_Listado
    {
        private string CadenaConexion;
        public AD_ObjetivosSemanales_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        /// <summary>
        /// Devuelve los objetivos capturados del ejercicio y periodo, un renglon
        /// por semana / vendedor / linea.
        /// </summary>
        public async Task<IEnumerable<mdl_ObjetivosSemanales_Listado>> Listado(int ejercicio, int periodo)
        {
            try
            {
                var parametros = new
                {
                    ejercicio = ejercicio,
                    periodo = periodo
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_ObjetivosSemanales_Listado> result = await factory.SQL.QueryAsync<mdl_ObjetivosSemanales_Listado>("CRM.sp_ObjetivosSemanales_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
