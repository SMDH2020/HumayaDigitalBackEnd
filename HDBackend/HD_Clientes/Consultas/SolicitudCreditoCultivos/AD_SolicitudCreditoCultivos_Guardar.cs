using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.SolicitudCreditoCultivos
{
    public class AD_SolicitudCreditoCultivos_Guardar
    {
        private string CadenaConexion;
        public AD_SolicitudCreditoCultivos_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdlSolicitud_Credito_Cultivos mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idsolicitud_cultivo = mdl.idsolicitud_cultivo,
                    folio = mdl.folio,
                    idcultivo = mdl.idcultivo,
                    hectareas = mdl.hectareas,
                    ciclo = mdl.ciclo,
                    tipo_riego = mdl.tipo_riego,
                    temporal = mdl.temporal,
                    rendimiento = mdl.rendimiento,
                    precio = mdl.precio,
                    mes_cosecha = mdl.mes_cosecha,
                    total_toneladas = mdl.total_toneladas,
                    usuario = mdl.usuario,
                    estatus = mdl.estatus
                };
                await factory.SQL.QueryAsync("Credito.sp_solicitud_credito_cultivos_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
