namespace ProjectE.DTO.FeedbackDtos
{
    public class ResultFeedbackDto
    {
        public string Id { get; set; }
        public string OfferId { get; set; }
        public string UserId { get; set; }
        public string CompanyId { get; set; }
        public string Comment { get; set; }
        public double AverageRating { get; set; }
        public string FeedbackReply { get; set; } // ✅ EKLENDİ
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }

        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
