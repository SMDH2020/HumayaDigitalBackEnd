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
        public async Task<mdlResultstring> Guardar(mdlSolicitud_Credito mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = mdl.folio,
                    idcliente = mdl.idcliente,
                    tipo_solicitud = mdl.tipo_solicitud,
                    importe = mdl.importe,
                    usuario = mdl.usuario,
                    estatus = mdl.estatus
                };
                var folioresult=await factory.SQL.QueryFirstOrDefaultAsync<string>("Credito.sp_solicitud_credito_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return new mdlResultstring() { value = folioresult };
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
