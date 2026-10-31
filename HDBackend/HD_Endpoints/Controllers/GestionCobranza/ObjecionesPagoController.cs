using HD.Security;
using HD_Cobranza.GestionCobranza.Capturas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionCobranza
{
    public class ObjecionesPagoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ObjecionesPagoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> MostrarObjeciones()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Mostrar_Objeciones_Pago datos = new AD_Mostrar_Objeciones_Pago(CadenaConexion);
            var result = await datos.Objecion();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> MostrarObjecionID(int id_Objecion)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Objecion_Pago_ID datos = new AD_Obtener_Objecion_Pago_ID(CadenaConexion);
            var result = await datos.Objecion(id_Objecion);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AgregarObjeciones(string objecion, int usuario)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Agregar_Objeciones_Pago datos = new AD_Agregar_Objeciones_Pago(CadenaConexion);
            usuario = int.Parse(Sesion.usuario());
            var result = await datos.Objecion(objecion, usuario);
            return Ok(result);
        } 

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EditarObjeciones(int id_Objecion, string objecion, bool estatus, int usuario)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Editar_Objeciones_Pago datos = new AD_Editar_Objeciones_Pago(CadenaConexion);
            usuario = int.Parse(Sesion.usuario());
            var result = await datos.Objecion(id_Objecion, objecion, estatus, usuario);
            return Ok(result);
        }
    }
}
