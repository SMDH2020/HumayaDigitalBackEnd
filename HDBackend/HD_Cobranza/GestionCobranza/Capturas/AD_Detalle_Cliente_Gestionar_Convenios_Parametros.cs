using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.GestionCobranza.Modelos;

namespace HD_Cobranza.GestionCobranza.Capturas
{
    public class AD_Detalle_Cliente_Gestionar_Convenios_Parametros
    {
        private string CadenaConexion;
        public AD_Detalle_Cliente_Gestionar_Convenios_Parametros(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Detalle_Clientes_Gestionar_Convenios>> Get(string? fechainicio, string? fechafin, string adr, string sucursal, int responsable, int objecion)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    fechainicio = fechainicio,
                    fechafin = fechafin,
                    adr = adr,
                    sucursal = sucursal,
                    responsable = responsable,
                    objecion = objecion
                };
                IEnumerable<mdl_Detalle_Clientes_Gestionar_Convenios> result = await factory.SQL.QueryAsync<mdl_Detalle_Clientes_Gestionar_Convenios>("GestionCobranza.sp_Detalle_Clientes_Gestionar_Parametros_PorRango", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
