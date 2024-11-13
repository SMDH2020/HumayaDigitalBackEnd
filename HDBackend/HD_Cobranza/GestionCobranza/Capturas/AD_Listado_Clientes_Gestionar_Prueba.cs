using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.GestionCobranza.Modelos;

namespace HD_Cobranza.GestionCobranza.Capturas
{
    public class AD_Listado_Clientes_Gestionar_Prueba
    {
        private string CadenaConexion;
        public AD_Listado_Clientes_Gestionar_Prueba(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Listado_Clientes_Gestionar_Prueba>> Clientes(string adr, string sucursal, int responsable, string linea, string cartera, string convenio, string juridico)
        {
            try
            {
                var parametros = new
                {
                    @adr = adr,
                    @sucursal = sucursal,
                    @responsable = responsable,
                    @linea_credito = linea,
                    @cartera = cartera,
                    @convenio = convenio,
                    @juridico = juridico
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Listado_Clientes_Gestionar_Prueba> result = await factory.SQL.QueryAsync<mdl_Listado_Clientes_Gestionar_Prueba>("Cartera_Clientes.Cobranza.sp_Obtener_Gestion", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
