using DocumentFormat.OpenXml.Math;
using HD.Security;
using HD_Cobranza.GestionCobranza.Capturas;
using HD_Cobranza.Reportes;
using HD_Ventas.Consultas;
using HD_Ventas.Modelos;
using Microsoft.AspNetCore.Mvc;
using Ventas.Consultas.CotizacionesVentas;
using Ventas.Modelos.CotizacionesVentas;
using Ventas.Reportes;

namespace HD.Endpoints.Controllers.Ventas
{
    public class CategoriasModelosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public CategoriasModelosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetListadoCategorias()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Categorias_Modelos datos = new AD_Categorias_Modelos(CadenaConexion);
            var result = await datos.Categorias();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetCategoriaID(int id)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Categorias_Modelos datos = new AD_Categorias_Modelos(CadenaConexion);
            var result = await datos.Categoriasid(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AgregarCategoria(mdl_Agregar_Categoria_Modelo mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Categorias_Modelos datos = new AD_Categorias_Modelos(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            await datos.AgregarCategoria(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EditarCategoria(mdl_Categorias_Modelos mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Categorias_Modelos datos = new AD_Categorias_Modelos(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            await datos.Editar_Categoria(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EditarEstatusCategoria(mdl_Categorias_Modelos mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Categorias_Modelos datos = new AD_Categorias_Modelos(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            await datos.Editar_Categoria_Estatus(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }
    }
}
