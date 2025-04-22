using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectE.DTO.OfferDtos
{
    public class ResultOfferDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string CompanyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? Budget { get; set; }
        public bool IsApprovedByAdmin { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
