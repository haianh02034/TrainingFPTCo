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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string sessionRoleId;

        public TopicController(TrainingDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            sessionRoleId = _httpContextAccessor.HttpContext.Session.GetString("SessionRoleId");
        }

        [HttpGet]
        public IActionResult Add()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUserId")))
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            if (sessionRoleId != "4")
            {
                // Redirect to access denied page or return forbidden status
                return RedirectToAction("AccessDenied", "Error");
            }
            else
            {
                TopicDetail model = new TopicDetail();
                model.SessionRoleId = sessionRoleId;
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TopicDetail topic, IFormFile PosterImage)
        {
            if (sessionRoleId != "4")
            {
                // Redirect to access denied page or return forbidden status
                return RedirectToAction("AccessDenied", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Upload file
                    string uniquePoster = UploadFileHelper.UploadFile(PosterImage, "images");

                    int idTopic = new TopicQuery().InsertItemTopic(
                        topic.CourseId,
                        topic.Name,
                        topic.Description,
                        topic.Status,
                        topic.Documents,
                        topic.AttachFile,
                        uniquePoster,
                        topic.TypeDocument
                    );

                    if (idTopic > 0)
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

                return RedirectToAction(nameof(TopicController.Index), "Topic");
            }

            return View(topic);
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