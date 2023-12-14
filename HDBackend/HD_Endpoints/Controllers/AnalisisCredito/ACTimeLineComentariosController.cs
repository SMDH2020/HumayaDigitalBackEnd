using HD.Clientes.Consultas.Clientes;
using HD.Clientes.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.AnalisisCredito
{
    public class ACTimeLineComentariosController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ACTimeLineComentariosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlClientes_Datos_Persona_Fisica mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_Guardar datos = new AD_Clientes_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            mdl.idcliente = await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });
        }
    }
}
