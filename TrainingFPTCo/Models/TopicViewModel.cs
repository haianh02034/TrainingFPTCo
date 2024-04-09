namespace TrainingFPTCo.Models
{
    public class TopicViewModel
    {
        public List<TopicDetail> TopicDetailList { get; set; }

    }
    public class TopicDetail
    {
        public int Id { get; set; }
        public int CourseId { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }

        public string Status { get; set; }

        public string Documents { get; set; }

        public string AttachFile { get; set; }

        public string PoterTopic { get; set; }
        public string TypeDocument { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }



    }
}
