namespace ProjectE.DTO.FeedbackDtos
{
    public class ResultFeedbackDto
    {
        public string Id { get; set; }
        public string OfferId { get; set; }
        public string UserId { get; set; }
        public string CompanyId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
