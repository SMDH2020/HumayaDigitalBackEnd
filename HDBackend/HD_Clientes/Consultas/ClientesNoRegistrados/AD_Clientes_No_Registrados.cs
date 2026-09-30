using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.ClientesNoRegistrados
{
    public class AD_Clientes_No_Registrados
    {
        private string CadenaConexion;
        public AD_Clientes_No_Registrados(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Clientes_No_Registrados>> Clientes(string ADR, string sucursales)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @ADR = ADR,
                    @sucursal = sucursales
                };
                IEnumerable<mdl_Clientes_No_Registrados> result = await factory.SQL.QueryAsync<mdl_Clientes_No_Registrados>("Credito.sp_Listado_Clientes_noRegistradosHD", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
