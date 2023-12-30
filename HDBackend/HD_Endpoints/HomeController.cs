using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints
{
    public class HomeController: Controller
    {
        public ActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/HTML");
        }
    }
}
