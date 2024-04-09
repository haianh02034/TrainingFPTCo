using Azure.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrainingFPTCo.DataDBContext;
using TrainingFPTCo.Helpers;
using TrainingFPTCo.Models;
using TrainingFPTCo.Models.Queries;

namespace TrainingFPTCo.Controllers
{
    public class CoursesController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var sessionRoleId = HttpContext.Session.GetString("SessionRoleId");

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUserId")))
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            if (sessionRoleId == "1" || sessionRoleId == "3" || sessionRoleId == "4")
            {
                CourseViewModel courseModel = new CourseViewModel();
                courseModel.CourseDetailList = new List<CourseDetail>();
                var dataCourse = new CourseQuery().GetAllDataCourses();
                foreach (var item in dataCourse)
                {
                    courseModel.CourseDetailList.Add(new CourseDetail
                    {
                        Id = item.Id,
                        CategoryId = item.CategoryId,
                        ViewCategoryName = item.ViewCategoryName,
                        Name = item.Name,
                        Description = item.Description,
                        Status = item.Status,
                        NameImage = item.NameImage,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt,
                    });
                }


                return View(courseModel);
            }

            else
            {
                return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
            }
        }
        [HttpGet]
        public IActionResult Add()
        {
            CourseDetail course = new CourseDetail();
            List<SelectListItem> itemCategories = new List<SelectListItem>();
            var dataCategory = new CategoryQuery().GetAllCategories(null, null);
            foreach (var item in dataCategory)
            {
                itemCategories.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                });
            }

            ViewBag.Categories = itemCategories;
            return View(course);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CourseDetail course, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                //xu ly insert coursr vafo database
                try
                {
                    string uniquePoster = UploadFileHelper.UploadFile(Image, "images");
                    int idCourse = new CourseQuery().InsertItemCourse(course.Name, course.CategoryId, course.Description, course.StartDate, course.EndDate, uniquePoster, course.Status);
                    if (idCourse > 0)
                    {
                        TempData["saveStatus"] = true;

                    }
                    else
                    {
                        TempData["saveStatus"] = false;

                    }
                }
                catch (Exception ex) {
                    return Ok(ex.Message);
                }
                return RedirectToAction(nameof(CoursesController.Index), "Courses");

            }

            List<SelectListItem> itemCategories = new List<SelectListItem>();
            var dataCategory = new CategoryQuery().GetAllCategories(null, null);
            foreach (var item in dataCategory)
            {
                itemCategories.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                });
            }

            ViewBag.Categories = itemCategories;
            return View(course);
        }
        [HttpPost]
        public JsonResult Delete(int id = 0)
        {
            bool deleteCourse = new CourseQuery().DeleteCourseById(id);
            if (deleteCourse)
            {
                return Json(new { cod = 200, message = "Successfully" });
            }
            return Json(new { cod = 500, message = "Failure" });
        }


        [HttpGet]
        public IActionResult Update(int id)
        {
            CourseDetail detail =new CourseQuery().GetDetailCourseById(id);
            List<SelectListItem> itemCategories = new List<SelectListItem>();
            var dataCategory = new CategoryQuery().GetAllCategories(null, null);
            foreach (var item in dataCategory)
            {
                itemCategories.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                });
            }
            ViewBag.Categories = itemCategories;
            return View(detail);
        }

        [HttpPost]
        public IActionResult Update(CourseDetail courseDetail, IFormFile Image)
        {
            try
            {
                var infoCourse = new CourseQuery().GetDetailCourseById(courseDetail.Id);
                string imageCourse = infoCourse.NameImage;
                // check xem nguoi co thay anh hay ko?
                if (courseDetail.Image != null)
                {
                    // co muon thay anh
                    imageCourse = UploadFileHelper.UploadFile(Image, "images");
                }
                bool update = new CourseQuery().UpdateCourseById(
                       courseDetail.CategoryId,
                       courseDetail.Name,
                       courseDetail.Description,
                       imageCourse,
                       courseDetail.StartDate,
                       courseDetail.EndDate,
                       courseDetail.Status,
                       courseDetail.Id
                   );
                if (update)
                {
                    TempData["updateStatus"] = true;
                }
                else
                {
                    TempData["updateStatus"] = false;
                }
                return RedirectToAction("Index", "Courses");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
            List<SelectListItem> itemCategories = new List<SelectListItem>();
            var dataCategory = new CategoryQuery().GetAllCategories(null, null);
            foreach (var item in dataCategory)
            {
                itemCategories.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                });
            }
            ViewBag.Categories = itemCategories;
            return View(courseDetail);
        }
    }
}