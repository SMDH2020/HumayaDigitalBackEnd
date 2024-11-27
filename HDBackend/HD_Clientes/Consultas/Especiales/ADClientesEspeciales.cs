using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Especiales;
using System.Security.Cryptography;

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
                    @idcliente = mdl.idcliente,
                    @comentarios = mdl.comentarios,
                    @estatus = mdl.estatus,
                    @tipo_cliente = mdl.tipo_cliente,
                    @usuario = mdl.usuario
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

        public async Task<bool> GuardarDocumento(mdl_Clientes_Especiales_Documento mdl)
        {
            try
            {
                var parametros = new
                {
                    @usuario = mdl.usuario,
                    @tipodoc = mdl.tipodoc,
                    @documento = mdl.documento,
                    @extension = mdl.extension,
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Clientes_Especiales_Guardar_Documento", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Clientes_Especiales_Documento>> Documento(string tipo)
        {
            try
            {
                var parametros = new
                {
                    @tipo = tipo
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryAsync<mdl_Clientes_Especiales_Documento>("Credito.sp_Clientes_Especiales_Obtener_Documento", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { mensaje = ex.Message });
            }
        }



        public async Task<IEnumerable<mdlClientesespecialesList>> Listado(string tipo)
        {
            try
            {
                var parametros = new
                {
                    @tipo = tipo
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryAsync<mdlClientesespecialesList>("Credito.sp_Clientes_Especiales_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdldropdownlistClientesEspeciales>> DropDownList()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                };
                var result = await factory.SQL.QueryAsync<mdldropdownlistClientesEspeciales>("Credito.sp_clientes_especiales_dropdownlist", commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdldropdownlistClientesEspeciales>> DropDownListTodos()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                };
                var result = await factory.SQL.QueryAsync<mdldropdownlistClientesEspeciales>("Credito.sp_clientes_especiales_dropdownlist_todos", commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdlClientesespecialesList>> InfoCliente(int idCliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idCliente = idCliente
                };
                var result = await factory.SQL.QueryAsync<mdlClientesespecialesList>("Credito.sp_Obtener_Cliente_Especial_ID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
