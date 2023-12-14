using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis;

namespace HD.Clientes.Consultas.AnalisisCredito
{
    public class ADAnalisis_Comentarios
    {
        private string CadenaConexion;
        public ADAnalisis_Comentarios(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<object> BuscarFolio(mdlSCAnalisis_Comentarios comentario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = comentario.folio,
                    idproceso=comentario.idproceso,
                    consecutivo=comentario.consecutivo,
                    comentario=comentario.comentarios,
                    usuario = comentario.usuario
                };
                 await factory.SQL.QueryAsync("Credito.Solicitud_Credito_Comentarios_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return new { mensaje="Comentario creado con éxito"};
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
