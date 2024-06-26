﻿using Microsoft.AspNetCore.Mvc;
using TrainingFPTCo.DataDBContext;
using TrainingFPTCo.Models;
using TrainingFPTCo.Models.Queries;
using TrainingFPTCo.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrainingFPTCo.Migrations;

namespace TrainingFPTCo.Controllers
{
    public class AccountController : Controller
    {

        private readonly TrainingDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string sessionRoleId;

        public AccountController(TrainingDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            sessionRoleId = _httpContextAccessor.HttpContext.Session.GetString("SessionRoleId");
        }

        [HttpGet]
        public IActionResult Index(string SearchString, string FilterStatus)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUserId")))
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            if (HttpContext.Session.GetString("SessionRoleId") == "1")
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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUserId")))
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            if (sessionRoleId != "1")
            {
                // Redirect to access denied page or return forbidden status
                return RedirectToAction("AccessDenied", "Error");
            }
            AccountDetail account = new AccountDetail();
            List<SelectListItem> itemRole = new List<SelectListItem>();

            var dataRole = new RoleQuery().GetAllRoles("", "");
            foreach (var item in dataRole.RoleDetailList) 
            {
                itemRole.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                });
            }
            account.SessionRoleId = sessionRoleId;
            ViewBag.Roles = itemRole;

            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AccountDetail account)
        {
            //return Ok(ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    int idAccount = new AccountQuery().InsertItemAccount(account.RoleId,account.UserName, account.Password,account.ExtraCode,account.Email,account.Phone,account.Address,account.FullName,account.Birthday,account.Gender, account.Status);
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
            List<SelectListItem> itemRole = new List<SelectListItem>();
            var dataRole = new RoleQuery().GetAllRoles("", "");
            foreach (var item in dataRole.RoleDetailList)
            {
                itemRole.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                });
            }

            ViewBag.Roles = itemRole;

            //return Ok(account);
            return View(account);

        }

        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            AccountDetail model = new AccountDetail();
            model.SessionRoleId = sessionRoleId;

            if (sessionRoleId != "1")
            {
                // Redirect to access denied page or return forbidden status
                return RedirectToAction("AccessDenied", "Error");
            }
            bool deleteAccount = new AccountQuery().DeleteAccountById(id);
            if (deleteAccount)
            {
                TempData["statusDelete"] = true;
            }
            else
            {
                TempData["statusDelete"] = false;

            }

            return RedirectToAction(nameof(AccountController.Index), "Account");
        }

        [HttpGet]
        public IActionResult Update(int id = 0)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUserId")))
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            if (sessionRoleId != "1")
            {
                // Redirect to access denied page or return forbidden status
                return RedirectToAction("AccessDenied", "Error");
            }
            AccountDetail detail = new AccountQuery().GetAccountById(id);
            List<SelectListItem> itemRoles = new List<SelectListItem>();
            var dataRole = new RoleQuery().GetAllRoles("", "");
            foreach (var item in dataRole.RoleDetailList)
            {
                itemRoles.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                });
            }
            ViewBag.Roles = itemRoles;
            detail.SessionRoleId = sessionRoleId;

            //return Ok(detail);
            return View(detail);

        }

        [HttpPost]
        public IActionResult Update(AccountDetail Account, IFormFile file)
        {
            if (sessionRoleId != "1")
            {
                // Redirect to access denied page or return forbidden status
                return RedirectToAction("AccessDenied", "Error");
            }
            try
            {
                var detail = new AccountQuery().GetAccountById(Account.Id);

                bool updateAccount = new AccountQuery().UpdateAccountById(
                            Account.RoleId,
                            Account.UserName,
                            Account.Password,
                            Account.ExtraCode,
                            Account.Email,
                            Account.Phone,
                            Account.Address,
                            Account.FullName,
                            Account.Birthday,
                            Account.Gender,
                            Account.Status,
                            Account.Id
                        ); if (updateAccount)
                {
                    TempData["statusUpdate"] = true;
                }
                else
                {
                    TempData["statusUpdate"] = false;

                }
                return RedirectToAction(nameof(AccountController.Index), "Account");

            }
            catch (Exception ex)
            {
                //return Ok(ex.Message);
                return View(Account);
            }

        }

    }
}
