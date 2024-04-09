namespace TrainingFPTCo.Models
{
    public class TrainerViewModel
    {
        public List<TrainerDetail> TrainerDetailList { get; set; }
    }
    public class TrainerDetail
    {
        public string Name { get; set; }
        public string TopicName { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
