using Microsoft.AspNetCore.Mvc;

namespace OnTheFlyAPI.Company.Controllers
{
    public class GetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
