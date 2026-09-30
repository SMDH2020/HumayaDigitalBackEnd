using Dapper;
using HD.AccesoDatos;
using HD_Buro.Modelos;

namespace HD_Buro.Consultas
{
    public class AD_Guarda_ClientesBuro_Comentarios
    {
        private string CadenaConexion;
        public AD_Guarda_ClientesBuro_Comentarios(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdlGuarda_ClientesBuro_Comentarios mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente = mdl.idcliente,
                    comentarios = mdl.comentarios,
                    createuser = mdl.usuario
                };
                await factory.SQL.QueryAsync("Credito.sp_Guarda_Comentario_ClienteBuro", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
