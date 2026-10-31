using HD_Checador.Consultas;
using HD_Checador.Modelos;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace HD.Endpoints.Controllers.MCP.Tools
{
    [McpServerToolType]
    public class NominaTools
    {
        private readonly string _conn;

        public NominaTools(IConfiguration config)
        {
            _conn = config.GetConnectionString("Servicio");
        }

        [McpServerTool(Name = "obtener_empleados")]
        [Description("""
            Obtiene el listado completo de empleados activos de Maquinaria del Humaya.
            Retorna una lista con los siguientes campos por empleado:
            - IDEmpleado, Nombre, ApellidoPaterno, ApellidoMaterno
            - IDEstado, Estado
            - IDSucursal, Sucursal
            - IDDepartamento, Departamento
            - IDPuesto, Puesto
            - Correo
            - FechaIngreso, Antiguedad
            - FechaNacimiento, Sexo
            - JEFE (nombre del jefe directo)
            - sueldo_diario, sueldo_promedio
            - cargo_contable
            """)]
        public async Task<IEnumerable<mdl_Empleado>> ObtenerEmpleados()
        {
            try
            {
                AD_Empleados datos = new AD_Empleados(_conn);
                return await datos.ObtenerEmpleados();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en obtener_empleados: {ex.Message} | Inner: {ex.InnerException?.Message}");
            }
        }
    }
}
