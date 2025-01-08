using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;
using HD_Cobranza.Modelos.ReporteRecuperacionCompleta;

namespace HD_Cobranza.Capturas.ReporteRecuperacionCompleta
{
    public class AD_Reporte_Recuperacion_Completa
    {
        private string CadenaConexion;
        public AD_Reporte_Recuperacion_Completa(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Recuperacion_Completa_View> ObtenerRecuperacion(int ejercicio, string adr, string sucursales)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    ejercicio = ejercicio,
                    adr,
                    sucursales
                };

                var result = await factory.SQL.QueryMultipleAsync("Cartera_Clientes.Cobranza.sp_Recuperacion_Objetivo_Mensual_segmentacion", parametros, commandType: System.Data.CommandType.StoredProcedure, commandTimeout: 60);
                var view = new mdl_Recuperacion_Completa_View();
                view.total = result.Read<mdl_Recuperacion_Completa>().ToList();
                view.operacion = result.Read<mdl_Recuperacion_Completa>().ToList();
                view.revolvente = result.Read<mdl_Recuperacion_Completa>().ToList();
                view.especial = result.Read<mdl_Recuperacion_Completa>().ToList();
                view.juridico = result.Read<mdl_Recuperacion_Completa>().ToList();
                factory.SQL.Close();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
