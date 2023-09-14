using Domain.DTO.ManageJobs;

namespace Application.JobOffer.DTO
{
    public class ManageJobsDto
    {
        public int Filed { get; set; }
        public int Total { get; set; }
        public int Actives { get; set; }
        public List<OfferModel>? Offers { get; set; }
    }
}
