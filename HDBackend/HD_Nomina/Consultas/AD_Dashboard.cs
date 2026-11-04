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

        public async Task<mdl_Dashboard_View> obtenerDashboard(int ejercicio, string adr, string sucursales, string departamentos, string header)
        {
            try
            {
                var parametros = new
                {
                    ejercicio,
                    adr,
                    sucursales,
                    departamentos,
                    header
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
                mdl.personal_cargo_contable = result.Read<mdl_Personal_Cargo_Contable>().ToList();

                factory.SQL.Close();
                return mdl;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Get_Foto_Empleado> GetFotoEmpleado(int idempleado)
        {
            try
            {
                var parametros = new
                {
                    idempleado = idempleado
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_Get_Foto_Empleado>("nomina.sp_Get_Empleado_Foto", parametros, commandType: System.Data.CommandType.StoredProcedure);

                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
