using HD.AccesoDatos;
using HD_Nomina.Modelos.Dashboard;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace HD_Nomina.Consultas
{
    public class AD_Dashboard
    {
        private string CadenaConexion;
        public AD_Dashboard(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_Dashboard_View> obtenerDashboard()
        {
            try
            {
                var parametros = new
                {
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("Nomina.sp_Dashboard_Nomina", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Dashboard_View mdl = new mdl_Dashboard_View();
                mdl.headers = result.Read<mdl_Headers_Dash>().FirstOrDefault();
                mdl.personal_sucursal = result.Read<mdl_Personal_Sucursal>().ToList();
                mdl.personal_departamento = result.Read<mdl_Personal_Departamento>().ToList();
                mdl.distribucion_antiguedad = result.Read<mdl_Distribucion_Antiguedad>().FirstOrDefault();
                mdl.contrataciones_mes = result.Read<mdl_Contrataciones_Mes>().ToList();
                mdl.estructura_jefes = result.Read<mdl_Estructura_Jefe>().ToList();
                mdl.empleados_Puestos = result.Read<mdl_Empleados_Puesto>().ToList();
                mdl.listado_empleado = result.Read<mdl_Listado_Empleados>().ToList();

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
