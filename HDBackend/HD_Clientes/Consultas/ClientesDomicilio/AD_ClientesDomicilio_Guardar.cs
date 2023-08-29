using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.ClientesDomicilio
{
    public class AD_ClientesDomicilio_Guardar
    {
        private string CadenaConexion;
        public AD_ClientesDomicilio_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdlClientes_Domicilio mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente = mdl.idcliente,
                    orden= mdl.orden,
                    idlocalidad=mdl.idlocalidad,
                    direccion=mdl.direccion,
                    tipodomicilio=mdl.tipodomicilio,
                    principal=mdl.principal,
                    referencia1=mdl.referencia1,
                    referencia2=mdl.referencia2,
                    estatus = mdl.estatus,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("Credito.sp_Clientes_Domicilio_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
