using HD.Clientes.Consultas.Documentos;
using HD.Clientes.Consultas.InteresMensual;
using HD.Clientes.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{

       public class DocumentosController : MyBase
        {
            private readonly IConfiguration Configuracion;
            private readonly ISesion Sesion;
            public DocumentosController(IConfiguration configuration, ISesion sesion)
            {
                Configuracion = configuration;
                Sesion = sesion;
            }

            [HttpPost]
            public async Task<ActionResult> Post(mdlDocumentos mdl)
            {

                string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
                AD_Documentos_Guardar datos = new AD_Documentos_Guardar(CadenaConexion);
                mdl.usuario = Sesion.usuario();
                var result = await datos.Guardar(mdl);
                return Ok(new { mensaje = "datos cargados con exito", listado = result });

            }
            [HttpGet]
            [Route("/api/[controller]/[action]")]
            public async Task<ActionResult> BuscarID(int iddocumento)
            {
                string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
                AD_Documentos_ObtenerporID datos = new AD_Documentos_ObtenerporID(CadenaConexion);
                var result = await datos.BuscarID(iddocumento);
                return Ok(result);

            }

            [HttpGet]
            [Route("/api/[controller]/[action]")]
            public async Task<ActionResult> Listado(int jdf)
            {
                string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
                AD_Documentos_Listado datos = new AD_Documentos_Listado(CadenaConexion);
                var result = await datos.Listado(jdf);
                return Ok(result);

            }
       }
    }

