using Microsoft.AspNetCore.Mvc;
using TrainingFPTCo.DataDBContext;
using TrainingFPTCo.Models;
using TrainingFPTCo.Models.Queries;
using TrainingFPTCo.Helpers;

namespace TrainingFPTCo.Controllers
{
    public class CategoryController : Controller
    {
        private readonly TrainingDbContext _dbContext;
        public CategoryController(TrainingDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult Index( string SearchString, string FilterStatus)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUserId")))
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            else
            {
                CategoryViewModel categoryModel = new CategoryViewModel();
                categoryModel.CategoryDetailList = new List<CategoryDetail>();
                var dataCategory = new CategoryQuery().GetAllCategories(SearchString, FilterStatus);
                foreach (var item in dataCategory)
                {
                    categoryModel.CategoryDetailList.Add(new CategoryDetail
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Status = item.Status,
                        PosterNameImage = item.PosterNameImage,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt,
                    });
                }
                ViewData["currentFilter"] = SearchString;
                ViewBag.FilterStatus = FilterStatus;

                return View(categoryModel);
            }
        }
        [HttpGet]
        public IActionResult Add()
        {
            CategoryDetail model = new CategoryDetail();
            return View(model);  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CategoryDetail category, IFormFile PosterImage)
        {
            //return Ok(ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    //tien hanh up load
                    string uniquePoster = UploadFileHelper.UploadFile(PosterImage,"images");
                   // var categoryData = new Category()
                   // {
                   //     Name = category.Name,
                   //     Description = category.Description,
                   //     ParentId = 0,
                   //     Status = category.Status,
                   //     PosterImage = uniquePoster,
                   //     CreatedAt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))

                   // };
                   // _dbContext.Categories.Add(categoryData);
                   // _dbContext.SaveChangesAsync(true);
                   //// thong bao thah cong
                   // TempData["saveStatus"] = true;

                    int idCategory = new CategoryQuery().InsertItemCategory(category.Name, category.Description, uniquePoster, category.Status);
                    if(idCategory > 0)
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
                    TempData["saveStatus"] = false;
                }
                return RedirectToAction(nameof(CategoryController.Index), "Category");
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            bool deleteCategory = new CategoryQuery().DeleteItemCategoryById(id);
            if (deleteCategory)
            {
                TempData["statusDelete"] = true;
            }
            else
            {
                TempData["statusDelete"] = false;

            }
            return RedirectToAction(nameof(CategoryController.Index), "Category");
        }
      

        [HttpGet]
        public IActionResult Update(int id = 0)
        {
            CategoryDetail category = new CategoryQuery().GetCategoryById(id);
            //if (category == null)
            //{
            //    return NotFound();
            //}

            //return View(category);
            return View(category);
        }
        [HttpPost]
        public IActionResult Update(CategoryDetail category, IFormFile file)
        {   
            
            try
            {
                var detail = new CategoryQuery().GetCategoryById(category.Id);
                string uniqueFilePoster = detail.PosterNameImage;
                // nguowfi dung co up loa file ko
                if(category.PosterImage != null)
                {
                    uniqueFilePoster = UploadFileHelper.UploadFile(file, "images");
                }
                bool updateCategory = new CategoryQuery().UpdateCategoryById(category.Id ,category.Name,category.Description, uniqueFilePoster, category.Status);
                    if(updateCategory)
                {
                    TempData["statusUpdate"] = true;
                }
                else
                {
                    TempData["statusUpdate"] = false;

                }
                return RedirectToAction(nameof(CategoryController.Index), "Category");

            }
            catch (Exception ex)
            {
                //return Ok(ex.Message);
                return View(category);
            }    
           
        }

    }
}
