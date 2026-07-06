using HD.Generales.Consultas;
using HD.Generales.Modelos;
using HD.Security;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace HD.Endpoints.Controllers.MCP.Tools
{
    [McpServerToolType]
    public class PresentacionesTools
    {
        private readonly string _conn;

        public PresentacionesTools(IConfiguration config)
        {
            _conn = config.GetConnectionString("Servicio");
        }
        [McpServerTool(Name = "listar_presentaciones")]
        [Description("Lista todas las presentaciones disponibles. Retorna id, nombre, descripcion, usuario y fecha de última actualización.")]
        public async Task<IEnumerable<mld_Presentaciones_Listado>> ListarPresentaciones()
        {
            AD_Presentaciones datos = new AD_Presentaciones(_conn);
            var result = await datos.Listado();
            return result;
        }
        [McpServerTool(Name = "guardar_html_presentacion")]
        [Description("Guarda el HTML actualizado de una presentación. Usa el presentacionId obtenido de listar_presentaciones.")]
        public async Task<object> GuardarHtmlPresentacion(
            [Description("ID de la presentación a actualizar")] string presentacionId,
            [Description("Contenido HTML completo generado")] string htmlContenido)
                {
            string CadenaConexion = _conn;
            AD_Presentaciones datos = new AD_Presentaciones(CadenaConexion);

            mdl_Presentaciones_Html mdl = new mdl_Presentaciones_Html();
            if (Guid.TryParse(presentacionId, out Guid id))
            {

                mdl.presentacionId = Guid.Parse(presentacionId);
                mdl.htmlContenido = htmlContenido;
                mdl.usuario = "1";
                var result = await datos.GuardarHtml(mdl);
                return result;
            }
            else
            {
                return $"Error al guardar PResentacion invalido";
            }
        }
    }
}
