using HD_Cobranza.Capturas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Ventas.Consultas;
using HD_Cobranza.Capturas.PlantillaKarbot;

namespace HD.Endpoints.Controllers.Cobranza.PlantillaKarbot
{
    public class CargaPlantillasKarbotController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public CargaPlantillasKarbotController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]


        public async Task<ActionResult> CargarPlantillasKarbot(string linea,string cartera, string telefono)

        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADCarga_PlantillaKarbot datos = new ADCarga_PlantillaKarbot(CadenaConexion);
            //int usuario = int.Parse(Sesion.usuario());

            var result = await datos.Plantillas(linea,cartera,  telefono);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> CargarPlantillasKarbotOperacion(string linea, string cartera, string telefono)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADCarga_PlantillaKarbot datos = new ADCarga_PlantillaKarbot(CadenaConexion);
            //int usuario = int.Parse(Sesion.usuario());
            var result = await datos.PlantillasOperacion(linea, cartera, telefono);
            return Ok(result);
        }
    }
}
