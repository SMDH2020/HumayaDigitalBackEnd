using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.GiroEmpresarial
{
    public class AD_GiroEmpresarial_BuscarID
    {
        private string CadenaConexion;
        public AD_GiroEmpresarial_BuscarID(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlGiro_Empresarial> BuscarID(int idgiro_empresarial)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idgiro_empresarial
                };                
                mdlGiro_Empresarial result = await factory.SQL.QueryFirstOrDefaultAsync<mdlGiro_Empresarial>("Credito.sp_giro_empresarial_obtenerporID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
