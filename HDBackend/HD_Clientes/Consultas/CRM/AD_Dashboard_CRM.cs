using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.CRM
{
    public class AD_Dashboard_CRM
    {

        private string CadenaConexion;
        public AD_Dashboard_CRM(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Dashboard_CRM_View> obtenerDashboard(int idcliente)
        {
            try
            {
                var parametros = new
                {
                    idcliente = idcliente
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("CRM.sp_Dashboard_Obtener", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Dashboard_CRM_View mdl = new mdl_Dashboard_CRM_View();
                mdl.generales = result.Read<mdl_Dashboard_CRM_Generales>().FirstOrDefault();
                mdl.solicitudes = result.Read<mdl_Dashboard_CRM_Solicitudes>().FirstOrDefault();
                mdl.cotizaciones = result.Read<mdl_Dashboard_CRM_Cotizaciones>().FirstOrDefault();
                mdl.Facturacion_cotizaciones = result.Read<mdl_Dashboard_CRM_Fechas_Facturacion_Linea>().ToList();
                mdl.lineasCredito = result.Read<mdl_Dashboard_CRM_Credito>().ToList();
                mdl.referencias = result.Read<mdl_Dashboard_CRM_Referencias>().FirstOrDefault();
                mdl.documentacionMhusa = result.Read<mdl_Dashboard_CRM_Expediente_Digital>().FirstOrDefault();
                mdl.documentacionJDF = result.Read<mdl_Dashboard_CRM_Expediente_Digital>().FirstOrDefault();

                if (mdl.referencias == null)
                {
                    mdl.referencias = new mdl_Dashboard_CRM_Referencias
                    {
                        total = 0,
                        referencia = "",
                        idcliente = "",
                    };
                }
                factory.SQL.Close();
                return mdl;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
