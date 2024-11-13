using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Especiales;

namespace HD.Clientes.Consultas.Especiales
{
    public class ADClientesEspeciales
    {
        string CadenaConexion = "";
        public ADClientesEspeciales(string _CadenaConexion)
        {
            CadenaConexion = _CadenaConexion;
        }
        public async Task<bool> Guardar(mdlClientesEspeciales mdl)
        {
            try
            {
                var parametros = new
                {
                    idcliente = mdl.idcliente,
                    comentarios = mdl.comentarios,
                    estatus = mdl.estatus,
                    tipo_cliente = mdl.tipo_cliente,
                    usuario = mdl.usuario,
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Clientes_Especiales_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdlClientesespecialesList>> Listado()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryAsync<mdlClientesespecialesList>("Credito.sp_Clientes_Especiales_Listado", commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
