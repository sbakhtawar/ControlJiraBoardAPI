using Microsoft.AspNetCore.Mvc;

namespace JiraTicketsAPI.Controllers
{
    public class TransitionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
