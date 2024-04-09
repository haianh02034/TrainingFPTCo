using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace TrainingFPTCo.DataDBContext
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Column("Name", TypeName = "Varchar(50)"), Required]
        public required string Name { get; set; }

        [Column("Description", TypeName = "Varchar(MAX)"), AllowNull]
        public string? Description { get; set; }

        [Column("PosterImage", TypeName = "Varchar(MAX)"), Required]
        public required string PosterImage { get; set; }

        [Column("ParentId", TypeName = "Integer"), Required, DefaultValue(123)]
        public int ParentId { get; set; }

        [Column("Status", TypeName = "Varchar(50)"), Required,]
        public required string Status { get; set; }


        [AllowNull]
        public DateTime? CreatedAt { get; set; }

        [AllowNull]
        public DateTime? UpdatedAt { get; set; }

        [AllowNull]
        public DateTime? DeletedAt { get; set; }
    }
}
