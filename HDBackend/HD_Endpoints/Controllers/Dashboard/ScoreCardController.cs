using HD.Security;
using HD_Dashboard.Consultas.Vendedor;
using Microsoft.AspNetCore.Mvc;
using HD_Dashboard.Consultas.Vendedor;
using HD_Dashboard.Modelos;
using Newtonsoft.Json;

namespace HD.Endpoints.Controllers.Dashboard
{
    public class ScoreCardController:MyBase
    {

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> Listado()
        {
            var result =  await Dash_ScoreCard_Main.ScoreCards(); 
            return Ok(result);

        }
    }
}
