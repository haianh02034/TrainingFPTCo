using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TrainingFPTCo.Validations;

namespace TrainingFPTCo.Models
{
    public class CourseViewModel
    {
        public List<CourseDetail> CourseDetailList { get; set; }

    }
    public class CourseDetail
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }

        public string? ViewCategoryName { get; set; }


        [Required(ErrorMessage = "Enter name's Course,please ")]
        public string Name { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "Choose file, please")]
        [AllowExtensionFile(new string[] { ".png", ".jpg", ".jpeg" })]
        [AllowSizeFile(5 * 1024 * 1024)]
        public IFormFile? Image { get; set; }
        //public string ParentId { get; set; }
        [AllowNull]
        public string? NameImage { get; set; }
        //view ten image
        public int? LikeCourse { get; set; }
        public int? StarCourse { get; set; }

        [Required(ErrorMessage = "Choose Status,please ")]
        public string Status { get; set; }

        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}",ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Choose StartDate, please")]
        public DateTime StartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}

