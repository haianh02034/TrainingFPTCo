using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using TrainingFPTCo.Models;
using TrainingFPTCo.Models.Queries;
namespace TrainingFPTCo.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel model) {
            model = new LoginQuery().CheckUserLogin(model.UserName,model.Password);
            if (string.IsNullOrEmpty(model.id) || string.IsNullOrEmpty(model.Email)) {
                ViewData["MessageLogin"] = "Account invalid";
                return View(model);
            }

            // luu thong tin nguoi dung vao session

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUserId")))
            {
                HttpContext.Session.SetString("SessionUserId", model.id);
                HttpContext.Session.SetString("SessionUsername", model.UserName);
                HttpContext.Session.SetString("SessionRoleId", model.RoleId);
                HttpContext.Session.SetString("SessionEmail", model.Email);
                HttpContext.Session.SetString("SessionFullname", model.FullName);


            }

            // chuyen den trang home
        
            {
                return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
            }

            
        }

        [HttpPost]
        public IActionResult Logout()
        {
            //xoa sessuib da tai ra o login //quay ve trang dang nhap
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUserId")))
            {
                //xoa session
                HttpContext.Session.Remove("SessionUserId");
                HttpContext.Session.Remove("SessionUsername");
                HttpContext.Session.Remove("SessionRoleId");
                HttpContext.Session.Remove("SessionEmail");
                HttpContext.Session.Remove("SessionFullname");
            }
            return RedirectToAction(nameof(LoginController.Index), "Login");
        }
    }
}
