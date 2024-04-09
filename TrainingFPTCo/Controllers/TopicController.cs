using Microsoft.AspNetCore.Mvc;
using TrainingFPTCo.DataDBContext;
using TrainingFPTCo.Models;
using TrainingFPTCo.Models.Queries;
using TrainingFPTCo.Helpers;

namespace TrainingFPTCo.Controllers
{
    public class TopicController : Controller
    {
        private readonly TrainingDbContext _dbContext;

        public TopicController(TrainingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index(string SearchString, string FilterStatus)
        {
            var sessionRoleId = HttpContext.Session.GetString("SessionRoleId");

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUserId")))
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            if (sessionRoleId == "1" || sessionRoleId == "2" || sessionRoleId == "4")
            {
                TopicViewModel topicModel = new TopicViewModel();
                topicModel.TopicDetailList = new List<TopicDetail>();
                var dataTopics = new TopicQuery().GetAllTopics(SearchString, FilterStatus);

                foreach (var item in dataTopics)
                {
                    topicModel.TopicDetailList.Add(new TopicDetail
                    {
                        Id = item.Id,
                        CourseId = item.CourseId,
                        Name = item.Name,
                        Description = item.Description,
                        Status = item.Status,
                        Documents = item.Documents,
                        AttachFile = item.AttachFile,
                        TypeDocument = item.TypeDocument,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt,
                    });
                }

                ViewData["currentFilter"] = SearchString;
                ViewBag.FilterStatus = FilterStatus;

                return View(topicModel);           
            }
            else
            {
                return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
            }
        }
    }
}