using Application.JobOffer.DTO;

namespace Application.Contracts.DTO
{
    public class AssignationsDto
    {
        public int OwnerId { get; set; }
        public int ContractId { get; set; }
        public List<JobOfferDto> UnitsConsumed { get; set; }
        public List<UnitsAssignmentDto> UnitsAssigned { get; set; }

        public AssignationsDto() {

            UnitsConsumed = new List<JobOfferDto>();
            UnitsAssigned = new List<UnitsAssignmentDto>();
        }
    }
}
