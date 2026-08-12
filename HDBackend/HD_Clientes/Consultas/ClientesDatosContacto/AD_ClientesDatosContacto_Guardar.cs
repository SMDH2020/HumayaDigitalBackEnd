using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.ClientesDatosContacto
{
    public class AD_ClientesDatosContacto_Guardar
    {
        private string CadenaConexion;
        public AD_ClientesDatosContacto_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlClientes_Datos_Contacto>> Guardar(mdlClientes_Datos_Contacto mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente = mdl.idcliente,
                    orden=mdl.orden,
                    medio_contacto=mdl.medio_contacto,
                    tipo_contacto = mdl.tipo_contacto,
                    valor = mdl.valor,
                    comentarios=mdl.comentarios,
                    estatus = mdl.estatus,
                    usuario = mdl.usuario,
                    responsable = mdl.responsable
                };
                IEnumerable<mdlClientes_Datos_Contacto> result = await factory.SQL.QueryAsync<mdlClientes_Datos_Contacto>("Credito.sp_clientes_datos_contacto_Guardar_Dpto", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdlClientes_Datos_Contacto>> GuardarArray(mdlClientes_Datos_Contacto_Array mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    contactos = mdl.contactos,
                    usuario = mdl.usuario
                };
                IEnumerable<mdlClientes_Datos_Contacto> result = await factory.SQL.QueryAsync<mdlClientes_Datos_Contacto>("Credito.sp_clientes_datos_contacto_Guardar_Dpto_Array", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdlClientes_Datos_Contacto>> GetContactoEditar(mdlClientes_Datos_Contacto mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente = mdl.idcliente,
                    medio_contacto = mdl.medio_contacto
                };
                IEnumerable<mdlClientes_Datos_Contacto> result = await factory.SQL.QueryAsync<mdlClientes_Datos_Contacto>("Credito.sp_Get_Datos_Contacto_Cliente_Existente", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
