using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Clientes_Juridico;

namespace HD.Clientes.Consultas.Clientes_Juridico
{
    public class AD_Detalle_Clientes_Juridico
    {
        private string CadenaConexion;
        public AD_Detalle_Clientes_Juridico(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Estados_Cliente_DropdownList>> EstadosCliente()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                };
                IEnumerable<mdl_Estados_Cliente_DropdownList> result = await factory.SQL.QueryAsync<mdl_Estados_Cliente_DropdownList>("Credito.sp_Estados_Cliente_DropdownList", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Estados_Demanda_Dropdownlist>> EstadosDemanda()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                };
                IEnumerable<mdl_Estados_Demanda_Dropdownlist> result = await factory.SQL.QueryAsync<mdl_Estados_Demanda_Dropdownlist>("Credito.sp_Estados_Demanda_DropdownList", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Detalle_Clientes_Juridico>> DetalleCliente(int idcliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idcliente = idcliente
                };
                IEnumerable<mdl_Detalle_Clientes_Juridico> result = await factory.SQL.QueryAsync<mdl_Detalle_Clientes_Juridico>("Credito.sp_Detalle_Clientes_Juridico", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdl_Detalle_Clientes_Juridico>> Guardar(mdl_Guardar_Gestion_Judicial mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idcliente = mdl.idcliente,
                    @estatus_cliente = mdl.estatus_cliente,
                    @expediente = mdl.expediente,
                    @juzgado = mdl.juzgado,
                    @estatus_demanda = mdl.estatus_demanda,
                    @usuario = mdl.usuario
                };

                var result = await factory.SQL.QueryAsync<mdl_Detalle_Clientes_Juridico>("Credito.sp_Guardar_Gestion_Judicial", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
