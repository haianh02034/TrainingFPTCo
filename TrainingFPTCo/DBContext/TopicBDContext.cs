using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TrainingFPTCo.DBContext
{
    public class TopicBDContext
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("CourseId"), Required]
        public required int CourseId { get; set; }

        [Column("NameTopic", TypeName = "Varchar(200)"), Required]
        public required string NameTopic { get; set; }

        [Column("Description", TypeName = "Varchar(200)"), AllowNull]
        public string? DescriptionRole { get; set; }
        [Column("Status", TypeName = "Varchar(200)"), Required]
        public required string Status { get; set; }

        [Column("Documents", TypeName = "Varchar(MAX)"), Required]
        public required string Documents { get; set; }

        [Column("AttachFile", TypeName = "Varchar(MAX)"), Required]
        public required string AttachFile { get; set; }
        [Column("PoterTopic", TypeName = "Varchar(50)"), Required]
        public required string PoterTopic { get; set; }
        [Column("TypeDocument", TypeName = "Varchar(50)"), Required]
        public required string TypeDocument { get; set; }

        [AllowNull]
        public DateTime? CreatedAt { get; set; }

        [AllowNull]
        public DateTime? UpdatedAt { get; set; }

        [AllowNull]
        public DateTime? DeletedAt { get; set; }

    }
}
