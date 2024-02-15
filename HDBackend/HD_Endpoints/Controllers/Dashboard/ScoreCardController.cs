using DocumentFormat.OpenXml.Office.CustomUI;
using HD.Security;
using HD_Dashboard.Consultas.Vendedor;
using HD_Dashboard.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Dashboard
{
    public class ScoreCardController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ScoreCardController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> Listado()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            DashScordCardPerfilVendedor ADVendedor = new DashScordCardPerfilVendedor(CadenaConexion);
            var ven = await ADVendedor.Listado(Sesion.usuario());

            var result = await Dash_ScoreCard_Main.ScoreCards();


            List<mdlScoreCardResult> Listado = new List<mdlScoreCardResult>();

            foreach (var card in ven) {
                var find = result.Where(element => element.scorecard.hoja.ToUpper().Equals(card.hoja.ToUpper())).FirstOrDefault();
                if(find != null)
                {
                    if(ven.Count() == 1) {

                        var acumulado = find;
                        acumulado.scorecard.nombre = "Objetivo Acumulado";
                        var mensual = find;
                        mensual.scorecard.nombre = "Objetivo Mensual";
                        Listado.Add(acumulado);
                        Listado.Add(mensual);
                    }
                    else
                    {

                        Listado.Add(find);
                    }
                }
            }


            return Ok(Listado);

        }
    }
}
