using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Facturar_Equipo;

namespace HD.Clientes.Consultas.Facturar_Equipo
{
    public class AD_FacturarEquipo_CreditoOtorgado
    {
        private string CadenaConexion;
        public AD_FacturarEquipo_CreditoOtorgado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlListFacCerrada>> Listado(int usuario)
        {
            try
            {
                var parametros = new
                {
                    usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlListFacCerrada> result = await factory.SQL.QueryAsync<mdlListFacCerrada>("Credito.sp_Obtener_Pedidos_De_Creditos_Cerrados", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
