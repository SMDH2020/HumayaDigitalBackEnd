using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.Documentos
{
    public class AD_Documentos_Guardar
    {
        private string CadenaConexion;
        public AD_Documentos_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdlDocumentos mdl)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {

                var parametros = new
                {
                    iddocumento = mdl.iddocumento,
                    documento = mdl.documento,
                    tipopersona = mdl.tipopersona,
                    linea_credito = mdl.linea_credito,
                    Opcional = mdl.Opcional,
                    Documentacion = mdl.Documentacion,
                    fase2 = mdl.fase2,
                    jdf = mdl.jdf,
                    dias_vigencia = mdl.dias_vigencia,
                    estatus = mdl.estatus,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("Credito.sp_Documentos_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
