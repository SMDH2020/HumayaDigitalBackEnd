using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.GestionCobranza.Modelos;

namespace HD_Cobranza.GestionCobranza.Capturas
{
    public class AD_Listado_Gestiones_Realizadas_Comentario
    {
        private string CadenaConexion;
        public AD_Listado_Gestiones_Realizadas_Comentario(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Listado_Gestiones_Realizadas_Comentario>> Get(int ejercicio, int periodo, string adr, string sucursal, int responsable)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    ejercicio = ejercicio,
                    periodo = periodo,
                    adr = adr,
                    sucursal = sucursal,
                    responsable = responsable
                };
                IEnumerable<mdl_Listado_Gestiones_Realizadas_Comentario> result = await factory.SQL.QueryAsync<mdl_Listado_Gestiones_Realizadas_Comentario>("GestionCobranza.sp_Listado_Gestiones_Realizadas", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
