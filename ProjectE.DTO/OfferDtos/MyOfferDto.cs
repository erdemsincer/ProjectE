namespace ProjectE.DTO.OfferDtos
{
    public class MyOfferDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Budget { get; set; }
        public bool IsApprovedByAdmin { get; set; }
        public string? CompanyId { get; set; } // Firma teklif aldıysa
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
    }
}
