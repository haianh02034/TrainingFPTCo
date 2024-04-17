using System.ComponentModel.DataAnnotations;
using TrainingFPTCo.Validations;

namespace TrainingFPTCo.Models
{
    public class TopicViewModel
    {
        public List<TopicDetail> TopicDetailList { get; set; }

    }
    public class TopicDetail
    {
        public string SessionRoleId { get; set; }

        public int Id { get; set; }
        public string? CourseName { get; set; }
        public int CourseId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string Status { get; set; }
        public string? NameDocuments { get; set; }
        [Required(ErrorMessage = "Choose file, please")]
        [AllowSizeFile(100 * 1024 * 1024)]
        public IFormFile? Documents { get; set; }



        public string? NameAttachFile { get; set; }

        [Required(ErrorMessage = "Choose video file, please")]
        [AllowExtensionFile(new string[] { ".mp4", ".avi", ".mkv", ".wmv" })]
        [AllowSizeFile(5 * 1024 * 1024)]
        public IFormFile? AttachFile { get; set; }
        public string? NamePoterTopic { get; set; }

        [Required(ErrorMessage = "Choose file, please")]
        [AllowExtensionFile(new string[] { ".pdf" })]
        [AllowSizeFile(5 * 1024 * 1024)]
        public IFormFile? PoterTopic { get; set; }
        public string? TypeDocument { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }



    }
}