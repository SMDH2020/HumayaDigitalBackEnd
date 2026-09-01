using HD_Ventas.Consultas;
using HD_Ventas.Modelos;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace HD.Endpoints.Controllers.MCP.Tools
{
    [McpServerToolType]
    public class VendedoresTools
    {
        private readonly string _conn;

        public VendedoresTools(IConfiguration config)
        {
            _conn = config.GetConnectionString("Servicio");
        }

        [McpServerTool(Name = "obtener_scorecard_vendedores")]
        [Description("""
            Obtiene el Scorecard CONSOLIDADO POR LÍNEA DE NEGOCIO del grupo Maquinaria del Humaya (distribuidor John Deere).
            Retorna una fila por cada línea de negocio (Tractores, Implementos, Drones, etc.) con métricas de:
            objetivo, unidades vendidas, importe, porcentaje de cumplimiento mensual y acumulado.
            NO desglosa por vendedor individual — para ver resultados por vendedor usar "obtener_scorecard_detalle_por_vendedor".
            Filtra por región (ADR), sucursal y rango de ejercicio/período (año y mes de inicio y fin).
            Usar 0 en adr y sucursal cuando se pida el grupo completo o no se especifique filtro.
            """)]
        public async Task<IEnumerable<mdlCarga_Scorecard_porVendedor_Dash>> Obtener_Scorecard_Vendedores(

            [Description("""
                Región (ADR). Valores permitidos:
                0 = Todos / Grupo completo
                1 = Región Sinaloa
                2 = Región Nayarit
                Usar 0 cuando el usuario pida el grupo, todas las regiones o no especifique región.
                """)]
            string adr,

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
            string sucursal,

            [Description("Año de inicio del período. Ejemplo: 2025")]
            string ejercicio_inicio,

            [Description("Mes de inicio del período (número). Ejemplo: 4 para abril")]
            string periodo_inicio,

            [Description("Año final del período. Ejemplo: 2026")]
            string ejercicio,

            [Description("Mes final del período (número). Ejemplo: 6 para junio")]
            string mes_actual)
        {
            AD_Carga_Scorecard_porParametros_Dash datos = new AD_Carga_Scorecard_porParametros_Dash(_conn);
            var result = await datos.Scorecard(
                int.Parse(adr),
                sucursal,
                "0",
                int.Parse(ejercicio_inicio),
                int.Parse(periodo_inicio),
                int.Parse(ejercicio),
                int.Parse(mes_actual),
                8919);
            return result;
        }

        [McpServerTool(Name = "obtener_scorecard_detalle_por_vendedor")]
        [Description("""
            Obtiene el Scorecard DESGLOSADO POR VENDEDOR de Maquinaria del Humaya (distribuidor John Deere).
            Retorna una fila por cada combinación de sucursal + vendedor + línea de negocio con:
            sucursal, vendedor, línea, unidades_objetivo, unidades_vendidas, importe_real.
            Permite ver el desempeño individual de cada vendedor en cada sucursal y línea.
            Filtra por rango de período: ejercicio inicio, mes inicio, ejercicio fin, mes fin.
            Ejemplos de uso:
            - "últimos 6 meses" desde enero 2026 hasta junio 2026 → ejercicio_inicio=2026, periodo_inicio=1, ejercicio_fin=2026, periodo_fin=6
            - "primer trimestre 2025" → ejercicio_inicio=2025, periodo_inicio=1, ejercicio_fin=2025, periodo_fin=3
            """)]
        public async Task<IEnumerable<mdlMCP_Scorecard_Vendedor>> Obtener_Scorecard_Detalle_Por_Vendedor(

            [Description("Año de inicio del período. Ejemplo: 2026")]
            string ejercicio_inicio,

            [Description("Mes de inicio del período (número, 1-12). Ejemplo: 1 para enero")]
            string periodo_inicio,

            [Description("Año fin del período. Ejemplo: 2026")]
            string ejercicio_fin,

            [Description("Mes fin del período (número, 1-12). Ejemplo: 6 para junio")]
            string periodo_fin)
        {
            AD_MCP_Scorecard_Vendedor datos = new AD_MCP_Scorecard_Vendedor(_conn);
            var result = await datos.ObtenerScorecard(
                int.Parse(ejercicio_inicio),
                int.Parse(periodo_inicio),
                int.Parse(ejercicio_fin),
                int.Parse(periodo_fin));
            return result;
        }
    }
}
