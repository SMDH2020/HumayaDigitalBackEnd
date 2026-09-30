using HD.Endpoints.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints
{

    public class PingController : MyBase
    {
        [HttpPost("wefe")]
        public ContentResult Post()
        {
            return Content("POST");
        }

        [HttpGet("dfg")]
        public ContentResult Get()
        {
            return Content("GET");
        }
    }
}
