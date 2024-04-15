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

                    new LoginQuery().UpdateLastLogin(model.id);

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
                new LoginQuery().UpdateLastLogout(HttpContext.Session.GetString("SessionUserId"));

                HttpContext.Session.Remove("SessionUserId");
                    HttpContext.Session.Remove("SessionUsername");
                    HttpContext.Session.Remove("SessionRoleId");
                    HttpContext.Session.Remove("SessionEmail");
                    HttpContext.Session.Remove("SessionFullname");

                    HttpContext.Session.Clear();

            }
            return RedirectToAction(nameof(LoginController.Index), "Login");
            }


        [HttpGet]
        public IActionResult Register()
        {
            
            AccountDetail account = new AccountDetail();
           

            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AccountDetail account)
        {
            //return Ok(ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    int idAccount = new LoginQuery().RegisterAccount(account.UserName, account.Password,  account.Email, account.Phone, account.Address, account.FullName, account.Birthday, account.Gender);
                    if (idAccount > 0)
                    {
                        TempData["saveStatus"] = true;

                    }
                    else
                    {
                        TempData["saveStatus"] = false;

                    }
                }
                catch (Exception ex)
                {
                    return Ok(ex.Message);
                    TempData["saveStatus"] = false;
                }
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            
         
            return View(account);

        }
    }
    }
