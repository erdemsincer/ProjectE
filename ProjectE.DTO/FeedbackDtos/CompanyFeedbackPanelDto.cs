namespace ProjectE.DTO.FeedbackDtos
{
    public class CompanyFeedbackPanelDto
    {
        public double AverageRating { get; set; }
        public int FeedbackCount { get; set; }
        public List<ResultFeedbackDto> Feedbacks { get; set; }
    }
}
