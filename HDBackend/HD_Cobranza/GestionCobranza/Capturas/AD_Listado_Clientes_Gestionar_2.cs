using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.GestionCobranza.Modelos;

namespace HD_Cobranza.GestionCobranza.Capturas
{
    public class AD_Listado_Clientes_Gestionar_2
    {
        private string CadenaConexion;
        public AD_Listado_Clientes_Gestionar_2(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Listado_Clientes_Gestionar_Prueba_2>> Clientes(string adr, string sucursal, string responsable, string linea, string cartera, string gestion, int ejercicio, int periodo)
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
                    @gestion = gestion,
                    @ejercicio = ejercicio,
                    @periodo = periodo

                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Listado_Clientes_Gestionar_Prueba_2> result = await factory.SQL.QueryAsync<mdl_Listado_Clientes_Gestionar_Prueba_2>("Cartera_Clientes.Cobranza.sp_Obtener_Gestion_3", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
