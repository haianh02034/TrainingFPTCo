using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TrainingFPTCo.DBContext
{
    public class RolesDBContext
    {
        //table roles
        [Key]
        public int Id { get; set; }
        [Column("NameRole", TypeName= "Varchar(50)"), Required]
        public required string NameRole { get; set; }
        [Column("Description" ,TypeName = "Varchar(200)"), AllowNull]
        public  string ? DescriptionRole { get; set; }
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
