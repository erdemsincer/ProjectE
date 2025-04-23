namespace ProjectE.DTO.FeedbackDtos
{
    public class CreateFeedbackDto
    {
        public string OfferId { get; set; }     // Hangi teklif üzerinden yorum geliyor
        public string Comment { get; set; }
        public int Rating { get; set; }         // 1 ile 5 arası
    }
}
