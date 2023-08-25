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
        public async Task<bool> Guardar(mdlClientes_EQUIP mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente_equip = mdl.idcliente_equip,
                    idcliente = mdl.idcliente,
                    idequip = mdl.idequip,
                    estatus = mdl.estatus,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("Credito.sp_clientes_equip_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
