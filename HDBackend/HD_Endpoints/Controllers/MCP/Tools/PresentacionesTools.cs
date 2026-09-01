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
        
        [McpServerTool(Name = "Actualizar_html_presentacion")]
        [Description("Actualiza el HTML de una presentación. Usa el presentacionId obtenido de listar_presentaciones.")]
        public async Task<object> ActualizarHtmlPresentacion(
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
        
        [McpServerTool(Name = "Guardar_presentacion")]
        [Description("Crea o Genera una nueva presentacion.")]
        public async Task<object> GuardarPresentacion(
            [Description("Nombre de la presentacion")] string nombre,
            [Description("Contenido HTML completo generado")] string htmlContenido,
            [Description("Breve descripcion para identificar la presentacion")] string descripcion
            )
        {
            string CadenaConexion = _conn;
            AD_Presentaciones datos = new AD_Presentaciones(CadenaConexion);

            mdl_Presentaciones_Guardar_completo mdl = new mdl_Presentaciones_Guardar_completo();
            mdl.presentacionId = Guid.NewGuid();
            mdl.nombre = nombre;
            mdl.htmlContenido = htmlContenido;
            mdl.descripcion = descripcion;
            mdl.usuario = "1";
            var result = await datos.GuardarPresentacion(mdl);
            return result;

        }

        [McpServerTool(Name = "Eliminar_presentacion")]
        [Description("Elimina una presentacion existente. Usa el presentacionId obtenido de listar_presentaciones.")]
        public async Task<object> EliminarPresentacion(
             [Description("ID de la presentación a actualizar")] string presentacionId)
        {
            string CadenaConexion = _conn;
            AD_Presentaciones datos = new AD_Presentaciones(CadenaConexion);
            if (Guid.TryParse(presentacionId, out Guid id))
            {
                var result = await datos.Eliminar(Guid.Parse(presentacionId));
                return result;
            }
            else
            {
                return $"Error al Eliminar la Presentacion invalido";
            }
        }
    }
}
