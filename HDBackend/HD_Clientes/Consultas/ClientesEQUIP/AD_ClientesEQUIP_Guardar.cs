using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.ClientesEQUIP
{
    public class AD_ClientesEQUIP_Guardar
    {
        private string CadenaConexion;
        public AD_ClientesEQUIP_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlClientes_EQUIP>> Guardar(mdlClientes_EQUIP mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente = mdl.idcliente,
                    idequip = mdl.idequip,
                    idsucursal = mdl.sucursal,
                    usuario = mdl.usuario
                };
                var result =await factory.SQL.QueryAsync< mdlClientes_EQUIP>("Credito.sp_Clientes_EQUIP_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
