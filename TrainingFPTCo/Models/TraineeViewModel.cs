namespace TrainingFPTCo.Models
{
    public class TraineeViewModel
    {
        public List<TraineeDetail> TraineeDetailList { get; set; }

    }
    public class TraineeDetail
    {
        public string Name { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

    }

}
