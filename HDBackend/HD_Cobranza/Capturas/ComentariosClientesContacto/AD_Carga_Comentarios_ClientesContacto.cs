using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;

namespace HD_Cobranza.Capturas.ComentariosClientesContacto
{
    public class AD_Carga_Comentarios_ClientesContacto
    {
        private string CadenaConexion;
        public AD_Carga_Comentarios_ClientesContacto(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlComentarios_Clientes_Contacto>> Comentarios(int idcliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente
                };
                IEnumerable<mdlComentarios_Clientes_Contacto> result = await factory.SQL.QueryAsync<mdlComentarios_Clientes_Contacto>("Cobranza.sp_Muestra_Comentarios_ClienteContacto", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
