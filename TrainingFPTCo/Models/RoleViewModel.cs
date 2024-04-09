namespace TrainingFPTCo.Models
{
    public class RoleViewModel
    {
        public List<RoleDetail> RoleDetailList { get; set; }

    }
    public class RoleDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set;}
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
