using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.SolicitudCreditoSiniestros
{
    public class AD_SolicitudCreditoSiniestros_Guardar
    {
        private string CadenaConexion;
        public AD_SolicitudCreditoSiniestros_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdlSolicitud_Credito_Siniestros mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = mdl.folio,
                    registro = mdl.registro,
                    siniestro = mdl.siniestro,
                    ptotal = mdl.ptotal,
                    pparcial = mdl.pparcial,
                    ciclo = mdl.ciclo,
                    indemnizacion = mdl.indemnizacion,
                    monto = mdl.monto,
                    estatus = mdl.estatus,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("Credito.sp_solicitud_credito_siniestros_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
