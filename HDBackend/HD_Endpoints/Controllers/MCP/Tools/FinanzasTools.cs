using HD_Finanzas.AccesoDatos.Actions;
using HD_Finanzas.Modelos.Estado_Resultados;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace HD.Endpoints.Controllers.MCP.Tools
{
    [McpServerToolType]
    public class FinanzasTools
    {
        private readonly string _conn;

        public FinanzasTools(IConfiguration config)
        {
            _conn = config.GetConnectionString("Servicio");
        }

        [McpServerTool(Name = "obtener_estado_resultados")]
        [Description("""
            Obtiene el Estado de Resultados de Maquinaria del Humaya (distribuidor John Deere).
            Filtra por rango de fechas, región (ADR), departamento y sucursal.
            Cuando el usuario pida "todo", "grupo" o varias regiones a la vez, usar 0 en adr, departamento y sucursal.
            """)]
        public async Task<List<Fmdl_EstadoResultados_View>> Obtener_Estado_Resultados(

            [Description("Fecha inicio del período en formato dd/mm/yyyy. Ejemplo: 01/01/2025")]
            string fecha_inicio,

            [Description("Fecha fin del período en formato dd/mm/yyyy. Ejemplo: 31/03/2025")]
            string fecha_fin,

            [Description("""
                Región (ADR). Valores permitidos:
                0 = Todos / Grupo completo
                1 = Región Sinaloa
                2 = Región Nayarit
                Usar 0 cuando el usuario pida el grupo, todas las regiones o no especifique región.
                Cuando se filtre por sucursal específica, usar el ADR correspondiente a esa sucursal.
                """)]
            string adr,

            [Description("""
                Departamento. Un solo valor o varios separados por coma (ej: "1,2").
                0  = Todos los departamentos
                1  = Maquinaria
                2  = Refacciones
                3  = Servicio
                11 = Productos Aliados
                12 = Ferretería
                13 = Sistemas de Riego
                14 = AMS
                16 = Drones
                24 = Usados
                """)]
            string departamento,

            [Description("""
                Sucursal. Un solo valor o varios separados por coma (ej: "1,21").
                0  = Todas (usar cuando se pida región completa, grupo o todo)
                --- Región Sinaloa (ADR=1) ---
                1  = Navolato
                11 = Caimanero
                21 = Eldorado
                31 = Costa Rica
                41 = La Cruz
                51 = El Rosario
                61 = Villa Unión
                --- Región Nayarit (ADR=2) ---
                2  = Tepic
                12 = San José
                22 = Santiago
                32 = Tecuala
                52 = San Vicente
                """)]
            string sucursal)
        {
            FAD_EstadoResultados estadoresultados = new FAD_EstadoResultados(_conn);
            var result = await estadoresultados.GetEstadoResultadosByDireccionRolado_Claude(
                fecha_inicio, fecha_fin, departamento, sucursal, adr, "8919");
            return result;
        }
    }
}
