using HD.Security;
using HD_Finanzas.AccesoDatos.Tasa_de_intereses;
using HD_Finanzas.Modelos.Tasa_de_intereses;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Finanzas
{
    public class FTasaInteresController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public FTasaInteresController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Fmdl_TipoTasas mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_TipoTasas_Guardar datos = new FAD_TipoTasas_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito", listado = result });

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> BuscarID(int idtipo_tasa)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_TipoTasas_ObtenerporID datos = new FAD_TipoTasas_ObtenerporID(CadenaConexion);
            var result = await datos.BuscarID(idtipo_tasa);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoAnio(int anio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_TipoTasas_ListadoporAnio datos = new FAD_TipoTasas_ListadoporAnio(CadenaConexion);
            var result = await datos.ListadoAnio(anio);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoMes(int mes)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_TipoTasas_ListadoporMes datos = new FAD_TipoTasas_ListadoporMes(CadenaConexion);
            var result = await datos.ListadoMes(mes);
            return Ok(result);

        }

    }
}
