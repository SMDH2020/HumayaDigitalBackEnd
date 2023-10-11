using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;

namespace HD_Cobranza.Capturas
{
    public class ADCob_ComentariosClientes
    {
        private string CadenaConexion;
        public ADCob_ComentariosClientes(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlCob_ComentariosCliente>> Guardar(mdlCob_ComentariosCliente obj)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idcomentario = obj.idcomentario,
                    @idcliente = obj.idcliente,
                    @comentarios = obj.comentarios,
                    @formacontacto = obj.formacontacto,
                    @compromisopago = obj.compromisopago,
                    @fechacompromisopago = obj.fechacompromisopago,
                    @importeconvenio = obj.importeconvenio,
                    @recordatorio = obj.recordatorio,
                    @fecharecordatorio = obj.fecharecordatorio,
                    @usuario = obj.usuario,
                };

                var result = await factory.SQL.QueryAsync<mdlCob_ComentariosCliente>("Credito.SP_Comentarios_Cliente_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdlCob_ComentariosCliente>> Listado(int idcliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente
                };
                var result = await factory.SQL.QueryAsync<mdlCob_ComentariosCliente>("Credito.sp_Comentarios_Cliente_Obtener", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
