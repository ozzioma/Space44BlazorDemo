using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}