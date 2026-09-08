using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.SolicitudCreditoEstadoResultados
{
    public class AD_SolicitudCreditoEstadoResultados_Guardar
    {
        private string CadenaConexion;
        public AD_SolicitudCreditoEstadoResultados_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdlSolicitud_Credito_Estado_Resultados mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = mdl.folio,
                    in_agricolas = mdl.in_agricolas,
                    in_ganado = mdl.in_ganado,
                    in_leche = mdl.in_leche,
                    in_maquilas = mdl.in_maquilas,
                    in_procampo = mdl.in_procampo,
                    in_rentas = mdl.in_rentas,
                    in_sueldos = mdl.in_sueldos,
                    in_otros = mdl.in_otros,
                    eg_agricolas = mdl.eg_agricolas,
                    eg_ganaderos = mdl.eg_ganaderos,
                    eg_maquilas = mdl.eg_maquilas,
                    eg_terrenos = mdl.eg_terrenos,
                    eg_refaccionarios = mdl.eg_refaccionarios,
                    eg_intereses = mdl.eg_intereses,
                    eg_impuestos = mdl.eg_impuestos,
                    eg_familiares = mdl.eg_familiares,
                    eg_otros = mdl.eg_otros,
                    usuario = mdl.usuario,
                };
                await factory.SQL.QueryAsync("Credito.sp_solicitud_credito_estado_resultados_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
