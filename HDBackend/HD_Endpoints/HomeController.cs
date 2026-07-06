using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints
{
    [AllowAnonymous]
    [Controller]
    public class HomeController: Controller
    {
        public ActionResult Index()
        {
            return Content("Humaya Digital Backend", "text/plain");
        }
    }
}
