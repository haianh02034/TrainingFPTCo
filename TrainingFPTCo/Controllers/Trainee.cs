using Microsoft.AspNetCore.Mvc;

namespace TrainingFPTCo.Controllers
{
    public class Trainee : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
