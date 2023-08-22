using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MyBase : ControllerBase
    {
    }
}
