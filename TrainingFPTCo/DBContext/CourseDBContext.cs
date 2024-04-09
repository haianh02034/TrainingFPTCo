using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace TrainingFPTCo.DBContext
{
    public class CourseDBContext
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("CategoryId"), Required]
        public required CategoryDBContext categories {  get; set; }
        public required int CategoryId { get; set; }

        [Column("NameCourse", TypeName = "Varchar(MAX)"), Required]
        public required string NameCourse { get; set; }

        [Column("Description", TypeName = "Varchar(MAX)"), AllowNull]
        public string? Description { get; set; }

        [Column("Image", TypeName = "Varchar(MAX)"), Required]
        public required string Image { get; set; }

        [Column("LikeCourse", TypeName = "Varchar(MAX)"), AllowNull]
        public string ? LikeCourse { get; set; }

        [Column("StarCourse", TypeName = "Integer"), AllowNull,]
        public int ? StarCourse { get; set; }

        [Column("Status", TypeName = "Varchar(200)"), Required]
        public required string Status { get; set; }

        [AllowNull]
        public DateTime? CreatedAt { get; set; }

        [AllowNull]
        public DateTime? UpdatedAt { get; set; }

        [AllowNull]
        public DateTime? DeletedAt { get; set; }


    }
}
