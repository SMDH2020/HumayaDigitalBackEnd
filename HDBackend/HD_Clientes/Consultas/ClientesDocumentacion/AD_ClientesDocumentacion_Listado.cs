using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.ClientesDocumentacion
{
    public class AD_ClientesDocumentacion_Listado
    {
        private string CadenaConexion;
        public AD_ClientesDocumentacion_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlClientesDocumento_Listado>> Listado(int idcliente,string financiera)
        {
            try
            {
                var parametros = new
                {
                    idcliente,
                    financiera
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlClientesDocumento_Listado> result = await factory.SQL.QueryAsync<mdlClientesDocumento_Listado>("Credito.sp_Clientes_Documentacion_BuscarPorCliente", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdlClientesDocumento_Listado>> ListadoPorLinea(int idcliente,string lineaCredito)
        {
            try
            {
                var parametros = new
                {
                    idcliente,
                    lineaCredito
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlClientesDocumento_Listado> result = await factory.SQL.QueryAsync<mdlClientesDocumento_Listado>("Credito.sp_Clientes_Documentacion_BuscarPorClienteAndLineaCredito", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
