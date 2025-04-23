namespace ProjectE.DTO.FeedbackDtos
{
    public class LikeFeedbackDto
    {
        public string FeedbackId { get; set; }
        public bool IsLike { get; set; }  // true → like, false → dislike
    }
}
