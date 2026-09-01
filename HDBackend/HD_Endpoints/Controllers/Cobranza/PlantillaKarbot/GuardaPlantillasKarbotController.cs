using HD_Cobranza.Capturas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Cobranza.Capturas.PlantillaKarbot;
using HD_Cobranza.Modelos.PlantillaKarbot;
using HD_Cobranza.Capturas.ConvenioPago;
using HD_Cobranza.Modelos.ConvenioPago;

namespace HD.Endpoints.Controllers.Cobranza.PlantillaKarbot
{
    public class GuardaPlantillasKarbotController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public GuardaPlantillasKarbotController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarPlantillas(mdl_Guarda_PlantillasKarbot mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADGuarda_PlantillasKarbot datos = new ADGuarda_PlantillasKarbot(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            var result = await datos.Guardar(mdl);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarTelefono(int idcliente, string telefono)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADGuarda_Telefonos_Karbot datos = new ADGuarda_Telefonos_Karbot(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Contacto(idcliente, telefono, usuario);
            return Ok(result);
        }

        //private ActionResult Ok(IEnumerable<mdl_Guarda_PlantillasKarbot> result)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
