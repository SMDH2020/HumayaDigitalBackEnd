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
            Obtiene el Scorecard por Vendedor de Maquinaria del Humaya (distribuidor John Deere).
            Muestra métricas de ventas por línea: objetivo, unidades vendidas, importe, porcentaje de cumplimiento
            tanto mensual como acumulado.
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
    }
}
