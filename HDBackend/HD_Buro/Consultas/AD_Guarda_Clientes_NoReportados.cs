using Dapper;
using HD.AccesoDatos;
using HD_Buro.Modelos;

namespace HD_Buro.Consultas
{
    public class AD_Guarda_Clientes_NoReportados
    {
        private string CadenaConexion;
        public AD_Guarda_Clientes_NoReportados(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlGuarda_Clientes_NoReportados>>

            Guardar(mdlGuarda_Clientes_NoReportados mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idcliente = mdl.idcliente,
                    @usuario = mdl.usuario,
                };

                var result = await
                factory.SQL.QueryAsync<mdlGuarda_Clientes_NoReportados>("BuroCredito.dbo.sp_Guarda_Clientes_NoReportados",
                parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new
                Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }

        }
    }
}
