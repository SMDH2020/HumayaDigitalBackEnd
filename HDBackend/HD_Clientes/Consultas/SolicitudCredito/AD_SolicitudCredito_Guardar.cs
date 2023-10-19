using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.SolicitudCredito
{
    public class AD_SolicitudCredito_Guardar
    {
        private string CadenaConexion;
        public AD_SolicitudCredito_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdlSolicitud_Credito mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = mdl.folio,
                    consecutivo = mdl.consecutivo,
                    idcliente = mdl.idcliente,
                    seguro_cosecha = mdl.seguro_cosecha,
                    tipo_solicitud = mdl.tipo_solicitud,
                    importe = mdl.importe,
                    estatus = mdl.estatus,
                    createuser = mdl.usuario
                };
                await factory.SQL.QueryAsync("Credito.sp_solicitud_credito_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
