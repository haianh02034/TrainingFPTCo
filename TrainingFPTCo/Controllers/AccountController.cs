using Microsoft.AspNetCore.Mvc;
using TrainingFPTCo.DataDBContext;
using TrainingFPTCo.Models;
using TrainingFPTCo.Models.Queries;
using TrainingFPTCo.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TrainingFPTCo.Controllers
{
    public class AccountController : Controller
    {

        private readonly TrainingDbContext _dbContext;

        public AccountController(TrainingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index(string SearchString, string FilterStatus)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUserId")))
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            if(HttpContext.Session.GetString("SessionRoleId") == "1")
            {
                AccountViewModel accountModel = new AccountViewModel();
                accountModel.AccountDetailList = new List<AccountDetail>();

                var dataAccount = new AccountQuery().GetAllAccounts(SearchString, FilterStatus);

                foreach (var item in dataAccount)
                {
                    accountModel.AccountDetailList.Add(new AccountDetail
                    {
                        Id = item.Id,
                        RoleId = item.RoleId,
                        ViewRoleName = item.ViewRoleName,
                        UserName = item.UserName,
                        ExtraCode = item.ExtraCode,
                        Email = item.Email,
                        Phone = item.Phone,
                        Address = item.Address,
                        FullName = item.FullName,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Birthday = item.Birthday,
                        Gender = item.Gender,
                        Education = item.Education,
                        ProgramingLanguage = item.ProgramingLanguage,
                        ToeicScore = item.ToeicScore,
                        IPClient = item.IPClient,
                        LastLogin = item.LastLogin,
                        LastLogout = item.LastLogout,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt
                    });
                }

                ViewData["currentFilter"] = SearchString;
                ViewBag.FilterStatus = FilterStatus;

                return View(accountModel);
            }
            else
            {
                return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            AccountDetail account = new AccountDetail();
            List<SelectListItem> itemRole = new List<SelectListItem>();
            var roleViewModel = new RoleQuery().GetAllRoles(null, null);
            foreach (var role in roleViewModel.RoleDetailList)
            {
                itemRole.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name
                });
            }
            ViewBag.Roles = itemRole;
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AccountDetail account, IFormFile PosterImage)
        {
            //return Ok(ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    int idAccount = new AccountQuery().InsertItemAccount(account.RoleId,account.UserName
                        , account.Password,account.ExtraCode,account.Email,account.Phone,account.Address,account.FullName,account.Birthday,account.Gender, account.Status);
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
                return RedirectToAction(nameof(AccountController.Index), "Account");
            }
            return View(account);
        }

    }
}
