using Microsoft.AspNetCore.Mvc;

namespace TrainingFPTCo.Controllers
{
    public class Trainer : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
