using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos.RecuperacionCartera;

namespace HD_Cobranza.Capturas.RecuperacionCartera
{
    public class ADREC_recuperacion
    {
        private string CadenaConexion;
        public ADREC_recuperacion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<RC_mdl_Result> Obtener(RC_mdl_view obj)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @fechainicio = obj.inicio,
                    @fechafin = obj.fin
                };

                var result = await factory.SQL.QueryMultipleAsync("EQUIP.Credito.Sp_ObtenerFacturacionPorRecuperacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                RC_mdl_Result mdl = new RC_mdl_Result();
                mdl.encabezado = result.Read<string>().FirstOrDefault();
                mdl.titulos = result.Read<RC_mdl_titulos>().ToList();
                mdl.detalle = result.Read<RC_mdl_detalle>().ToList();
                mdl.sucursales = result.Read<RC_mdl_sucursales>().ToList();
                factory.SQL.Close();
                return mdl;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

    }
}
