using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.ClientesDocumentacion
{
    public class AD_ClientesDocumentacion_Guardar
    {
        private string CadenaConexion;
        public AD_ClientesDocumentacion_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlClientesDocumento_Listado>> Guardar(mdlClientesDocumentacion_Guardar mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idclientedocumento = mdl.idclientedocumento,
                    idcliente = mdl.idcliente,
                    iddocumento = mdl.iddocumento,
                    orden = mdl.orden,
                    documento = mdl.documento,
                    extension=mdl.extension,
                    vigencia = mdl.vigencia,
                    comentarios = mdl.comentarios,
                    estatus = mdl.estatus,
                    usuario = mdl.usuario,

                };
                IEnumerable<mdlClientesDocumento_Listado> result= await factory.SQL.QueryAsync<mdlClientesDocumento_Listado>("Credito.sp_Clientes_Documentacion_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
