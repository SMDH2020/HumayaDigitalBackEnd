using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.ClientesGiroEmpresarial
{
    public class AD_ClientesGiroEmpresarial_Guardar
    {
        private string CadenaConexion;
        public AD_ClientesGiroEmpresarial_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdlClientes_Giro_Empresarial mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente_giro_empresarial = mdl.idcliente_giro_empresarial,
                    idcliente = mdl.idcliente,
                    idgiro_empresarial = mdl.idgiro_empresarial,
                    estatus = mdl.estatus,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("Credito.sp_clientes_giro_empresarial_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
