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
            Obtiene el reporte de Rotación de Cuentas por Cobrar (CXC) de Maquinaria del Humaya.
            Retorna dos elementos:
            - editor_guia (bool): indica si el usuario tiene permiso de editar la guía.
            - rotacion (lista): detalle de rotación CXC por línea de negocio o por sucursal,
              con campos: departamento, sucursal, saldo_inicial, credito, saldo_final,
              rcxc, guia, guia_semestral, guia_anual, rcxc_semestral, rcxc_anual,
              cartera_optima, diferencia_cartera_optima.

            Usar tipo_reporte = "D" para ver el listado desglosado por línea de negocio (departamento).
            Usar tipo_reporte = "S" para ver el listado desglosado por sucursal.
            """)]
        public async Task<mdl_RotacionCXC_View> Obtener_Rotacion_CXC(
            [Description("Año a consultar. Ejemplo: 2026")]
            string ejercicio,
            [Description("Mes a consultar (número, 1-12). Ejemplo: 6 para junio")]
            string periodo,
            [Description("""
                Tipo de reporte:
                D = Desglose por línea de negocio (departamento)
                S = Desglose por sucursal
                """)]
            string tipo_reporte)
        {
            AD_RotacionCXC_Reporte datos = new AD_RotacionCXC_Reporte(_conn);
            var result = await datos.reporte(
                int.Parse(ejercicio),
                int.Parse(periodo),
                "G",
                "0",
                "8929",
                tipo_reporte);
            return result;
        }
    }
}
