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
                    ac_cajabancos = mdl.ac_cajabancos,
                    ac_clientes = mdl.ac_clientes,
                    ac_deudoresdiversos = mdl.ac_deudoresdiversos,
                    ac_ivaporrecuperar = mdl.ac_ivaporrecuperar,
                    ac_apoyodegobierno = mdl.ac_apoyodegobierno,
                    ac_inventariodeinsumos = mdl.ac_inventariodeinsumos,
                    ac_inversionencultivos = mdl.ac_inversionencultivos,
                    ac_otrosactivos = mdl.ac_otrosactivos,
                    af_terrenosenpropiedad = mdl.af_terrenosenpropiedad,
                    af_terrenosenejidal = mdl.af_terrenosenejidal,
                    af_construcciones = mdl.af_construcciones,
                    af_maquinariayequipo = mdl.af_maquinariayequipo,
                    af_equipodetransporte = mdl.af_equipodetransporte,
                    af_mobiliarioyequipo = mdl.af_mobiliarioyequipo,
                    af_depresiaciones = mdl.af_depresiaciones,
                    af_otrosactivos = mdl.af_otrosactivos,
                    pc_creditosdirectos = mdl.pc_creditosdirectos,
                    pc_creditosdeavio = mdl.pc_creditosdeavio,
                    pc_proveedores = mdl.pc_proveedores,
                    pc_acreedoresdiversos = mdl.pc_acreedoresdiversos,
                    pc_impuestosycuotas = mdl.pc_impuestosycuotas,
                    pc_amortizaciones = mdl.pc_amortizaciones,
                    pc_otrospasivos = mdl.pc_otrospasivos,
                    pf_creditosrefaccionarios = mdl.pf_creditosrefaccionarios,
                    pf_creditosdejdfm = mdl.pf_creditosdejdfm,
                    pf_otros = mdl.pf_otros,
                    usuario=mdl.usuario
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
