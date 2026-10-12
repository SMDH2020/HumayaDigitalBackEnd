using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.CRM;
using HD.Clientes.Modelos.CRM.Reportes;
using HD.Clientes.Modelos.CRM.Visitas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.CRM.Reportes
{
    public class AD_Reporte_Visitas_ProgramadasCRM
    {
        private string CadenaConexion;
        public AD_Reporte_Visitas_ProgramadasCRM(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_Reporte_Visitas_ProgramadasCRM_VIEW> ReporteVisitasProgramadas(string fechainicio, string fechafin, string adr, string sucursal, int usuario)
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
                var result = await factory.SQL.QueryMultipleAsync("CRM.sp_Obtener_listado_Reporte_Visitas_Programadas", parametros, commandType: System.Data.CommandType.StoredProcedure);

                mdl_Reporte_Visitas_ProgramadasCRM_VIEW mdl = new mdl_Reporte_Visitas_ProgramadasCRM_VIEW();
                mdl.listado_visitas = result.Read<mdl_Reporte_Visitas_ProgramadasCRM>().ToList();
                mdl.info_grafica = result.Read<mdl_Reporte_Visitas_ProgramadasCRM_Grafica>().ToList();
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
