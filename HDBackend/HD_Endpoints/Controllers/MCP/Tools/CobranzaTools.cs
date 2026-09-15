using HD_Finanzas.AccesoDatos.RotacionInventario;
using HD_Finanzas.Modelos.RotacionInventario;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace HD.Endpoints.Controllers.MCP.Tools
{
    [McpServerToolType]
    public class CobranzaTools
    {
        private readonly string _conn;

        public CobranzaTools(IConfiguration config)
        {
            _conn = config.GetConnectionString("Servicio");
        }

        [McpServerTool(Name = "obtener_rotacion_cxc")]
        [Description("""
            Obtiene el reporte de Rotación de Cuentas por Cobrar (CXC) de Maquinaria del Humaya,
            desglosado por línea de negocio (departamento).
            Retorna dos elementos:
            - editor_guia (bool): indica si el usuario tiene permiso de editar la guía.
            - rotacion (lista): detalle de rotación CXC con campos: departamento, sucursal,
              saldo_inicial, credito, saldo_final, rcxc, dias_promedio_cobro, guia,
              guia_semestral, guia_anual, rcxc_semestral, rcxc_anual,
              cartera_optima, diferencia_cartera_optima.

            El filtro de alcance se controla con tipoubi + id:
            - Grupo completo  → tipoubi="G", id="0"
            - Por región      → tipoubi="R", id="1" (Sinaloa) o id="2" (Nayarit)
            - Por sucursal    → tipoubi="S", id=<id de sucursal>

            NOTA: tipo_reporte siempre es "D" (fijo internamente). No se puede cambiar.
            """)]
        public async Task<mdl_RotacionCXC_View> Obtener_Rotacion_CXC(
            [Description("Año a consultar. Ejemplo: 2026")]
            string ejercicio,
            [Description("Mes a consultar (número, 1-12). Ejemplo: 6 para junio")]
            string periodo,
            [Description("""
                Alcance del reporte:
                G = Grupo completo (todas las regiones y sucursales)
                R = Filtrar por región (usar con id="1" para Sinaloa o id="2" para Nayarit)
                S = Filtrar por sucursal específica (usar con el id de la sucursal)
                Si no se especifica filtro, usar G.
                """)]
            string tipoubi,
            [Description("""
                Identificador del alcance según tipoubi:
                0  = Grupo completo (usar con tipoubi=G)
                --- Regiones (usar con tipoubi=R) ---
                1  = Región Sinaloa
                2  = Región Nayarit
                --- Sucursales (usar con tipoubi=S) ---
                1  = Navolato
                2  = Tepic
                11 = Caimanero
                12 = San José
                21 = Eldorado
                22 = Santiago
                31 = Costa Rica
                32 = Tecuala
                41 = La Cruz
                42 = Las Varas
                51 = El Rosario
                52 = San Vicente
                61 = Villa Unión
                """)]
            string id)
        {
            try
            {
                AD_RotacionCXC_Reporte datos = new AD_RotacionCXC_Reporte(_conn);
                var result = await datos.reporte(
                    int.Parse(ejercicio),
                    int.Parse(periodo),
                    tipoubi,
                    id,
                    "8929",
                    "D");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en obtener_rotacion_cxc: {ex.Message} | Inner: {ex.InnerException?.Message}");
            }
        }
    }
}
