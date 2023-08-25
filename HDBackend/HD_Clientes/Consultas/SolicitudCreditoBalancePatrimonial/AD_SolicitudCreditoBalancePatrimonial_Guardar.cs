using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.SolicitudCreditoBalancePatrimonial
{
    public class AD_SolicitudCreditoBalancePatrimonial_Guardar
    {
        private string CadenaConexion;
        public AD_SolicitudCreditoBalancePatrimonial_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdlSolicitud_Credito_Balance_Patrimonial mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = mdl.folio,
                    registro = mdl.registro,
                    concepto = mdl.concepto,
                    tipo = mdl.tipo,
                    importe = mdl.importe,
                    estatus = mdl.estatus,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("Credito.sp_solicitud_credito_balance_patrimonial_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
