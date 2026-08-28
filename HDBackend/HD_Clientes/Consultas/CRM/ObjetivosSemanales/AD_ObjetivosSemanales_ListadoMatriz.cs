using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.CRM.ObjetivosSemanales;

namespace HD.Clientes.Consultas.CRM.ObjetivosSemanales
{
    public class AD_ObjetivosSemanales_ListadoMatriz
    {
        private string CadenaConexion;
        public AD_ObjetivosSemanales_ListadoMatriz(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        /// <summary>
        /// Devuelve las lineas ya capturadas en la matriz de objetivos.
        /// El SP no recibe parametros y regresa el listado ya ordenado por el
        /// orden del catalogo: no reordenar aqui.
        /// </summary>
        public async Task<IEnumerable<mdl_ObjetivosSemanales_ListadoMatriz>> ListadoMatriz()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_ObjetivosSemanales_ListadoMatriz> result = await factory.SQL.QueryAsync<mdl_ObjetivosSemanales_ListadoMatriz>("CRM.sp_ObjetivosSemanales_ListadoMatriz", commandType: System.Data.CommandType.StoredProcedure);
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
