using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Facturar_Equipo;

namespace HD.Clientes.Consultas.Facturar_Equipo
{
    public class AD_Facturar_Equipo
    {
        private string CadenaConexion;
        public AD_Facturar_Equipo(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Factuar_Equipo_View>> Listado(int usuario)
        {
            try
            {
                var parametros = new
                {
                    usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Factuar_Equipo_View> result = await factory.SQL.QueryAsync<mdl_Factuar_Equipo_View>("Credito.sp_Solicitudes_por_Facturar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
