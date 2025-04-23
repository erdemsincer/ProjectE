namespace ProjectE.DTO.CompanyDtos
{
    public class ResultCompanyDto
    {
        public string Id { get; set; }

        public string CompanyName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Description { get; set; }

        public bool IsAdvertiser { get; set; }

        public double AverageRating { get; set; } // ⭐ Ortalama puan
    }
}
