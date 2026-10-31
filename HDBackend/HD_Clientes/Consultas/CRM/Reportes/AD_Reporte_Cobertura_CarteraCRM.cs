using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.CRM;
using HD.Clientes.Modelos.CRM.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.CRM.Reportes
{
    public class AD_Reporte_Cobertura_CarteraCRM
    {
        private string CadenaConexion;
        public AD_Reporte_Cobertura_CarteraCRM(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Reporte_Cobertura_Cartera_View> Listado(string fechainicio, string fechafin, string adr, string sucursal, int usuario)
        {
            try
            {
                var parametros = new
                {
                    fechainicio,
                    fechafin,
                    adr,
                    sucursal,
                    usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("CRM.sp_Obtener_Listado_Cobertura_CarteraCRM", parametros, commandType: System.Data.CommandType.StoredProcedure);

                mdl_Reporte_Cobertura_Cartera_View mdl = new mdl_Reporte_Cobertura_Cartera_View();
                mdl.listado = result.Read<mdl_Reporte_Cobertura_Cartera>().ToList();
                mdl.permisos = result.Read<mdl_Permisos_CRM>().FirstOrDefault();

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
