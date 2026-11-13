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

       [McpServerTool(Name = "obtener_altas_y_bajas")]
       [Description("""
           Obtiene el reporte de altas y bajas de personal de Maquinaria del Humaya en un período determinado.
           Retorna dos listas:
           - altas: empleados que ingresaron a la empresa en el período indicado.
           - bajas: empleados que causaron baja en la empresa en el período indicado.
           Cada registro incluye: IDEmpleado, Nombre, ApellidoPaterno, ApellidoMaterno,
           Estado, Sucursal, Departamento, Puesto, Correo, FechaIngreso, Antiguedad,
           FechaNacimiento, Sexo, JEFE, sueldo_diario, sueldo_promedio, cargo_contable.
           Las fechas deben enviarse en formato dd/MM/yyyy. Ejemplo: 01/01/2026.
           """)]
       public async Task<object> ObtenerAltasYBajas(
           [Description("Fecha de inicio del período en formato dd/MM/yyyy. Ejemplo: 01/01/2026")]
           string fecha_inicio,
           [Description("Fecha de fin del período en formato dd/MM/yyyy. Ejemplo: 31/10/2026")]
           string fecha_fin)
       {
           try
           {
               if (!DateTime.TryParseExact(fecha_inicio, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime fi))
                   return new { error = true, mensaje = $"Fecha inicio inválida: '{fecha_inicio}'. Usar formato dd/MM/yyyy." };
               if (!DateTime.TryParseExact(fecha_fin, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime ff))
                   return new { error = true, mensaje = $"Fecha fin inválida: '{fecha_fin}'. Usar formato dd/MM/yyyy." };

               AD_Empleados datos = new AD_Empleados(_conn);
               var result = await datos.ObtenerAltasYBajas(fi, ff);
               return result;
           }
           catch (Exception ex)
           {
               return new { error = true, mensaje = ex.Message, inner = ex.InnerException?.Message };
           }
       }
   }
}
