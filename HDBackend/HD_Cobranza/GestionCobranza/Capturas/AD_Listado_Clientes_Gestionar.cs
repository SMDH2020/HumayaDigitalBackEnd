using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.GestionCobranza.Modelos;

namespace HD_Cobranza.GestionCobranza.Capturas
{
    public class AD_Listado_Clientes_Gestionar
    {
        private string CadenaConexion;
        public AD_Listado_Clientes_Gestionar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Listado_Clientes_Gestionar>> Clientes(string adr, string sucursal, int responsable)
        {
            try
            {
                var parametros = new
                {
                    @adr = adr,
                    @sucursal = sucursal,
                    @responsable = responsable
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Listado_Clientes_Gestionar> result = await factory.SQL.QueryAsync<mdl_Listado_Clientes_Gestionar>("GestionCobranza.sp_Listado_Clientes_Gestionar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
