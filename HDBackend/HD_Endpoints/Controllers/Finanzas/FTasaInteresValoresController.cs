using HD.Security;
using HD_Finanzas.AccesoDatos.Tasa_de_intereses;
using HD_Finanzas.Modelos.Tasa_de_intereses;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Finanzas
{
    public class FTasaInteresValoresController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public FTasaInteresValoresController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Fmdl_TipoTasasValores mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_TipoTasasValores_Guardar datos = new FAD_TipoTasasValores_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito", listado = result });

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> BuscarID(int idtipo_tasa_valor)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_TipoTasasValores_ObtenerporID datos = new FAD_TipoTasasValores_ObtenerporID(CadenaConexion);
            var result = await datos.BuscarID(idtipo_tasa_valor);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(int idtipo_tasa)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_TipoTasasValores_Listado datos = new FAD_TipoTasasValores_Listado(CadenaConexion);
            var result = await datos.Listado(idtipo_tasa);
            return Ok(result);

        }
    }
}
