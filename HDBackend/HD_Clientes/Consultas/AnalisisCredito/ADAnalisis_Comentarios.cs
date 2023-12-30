using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis;
using HD.Clientes.Modelos.SC_Analisis.Modal;

namespace HD.Clientes.Consultas.AnalisisCredito
{
    public class ADAnalisis_Comentarios
    {
        private string CadenaConexion;
        public ADAnalisis_Comentarios(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSCAnalisis_Pedido_Estado> Guardar(mdlSCAnalisis_Comentarios comentario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = comentario.folio,
                    idproceso=comentario.idproceso,
                    consecutivo=comentario.consecutivo,
                    comentarios=comentario.comentarios,
                    estatus=comentario.estatus,
                    usuario = comentario.usuario
                };
                mdlSCAnalisis_Pedido_Estado result=await factory.SQL.QueryFirstOrDefaultAsync<mdlSCAnalisis_Pedido_Estado>("Credito.SP_Solicitud_Credito_Comentarios_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
