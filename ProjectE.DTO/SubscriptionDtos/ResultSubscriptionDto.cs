namespace ProjectE.DTO.SubscriptionDtos
{
    public class ResultSubscriptionDto
    {
        public string Id { get; set; }
        public string CompanyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool IsActive { get; set; }
    }
}
