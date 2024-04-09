using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TrainingFPTCo.Validations;
namespace TrainingFPTCo.Models
{
    public class AccountViewModel
    {
        public List<AccountDetail> AccountDetailList { get; set; }

    }
    public class AccountDetail
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public string? ViewRoleName { get; set; }

        [Required(ErrorMessage = "Enter name's Account,please ")]

        public string UserName { get; set; }
        public string Password { get; set; }
        public string ExtraCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; }
        public string Education { get; set; }
        public string ProgramingLanguage { get; set; }
        public string ToeicScore { get; set; }
        public string Skills { get; set; }
        public string IPClient { get; set; }
        public string Status { get; set; }
        public DateTime LastLogin { get; set; }

        public DateTime LastLogout { get; set; }


        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}

