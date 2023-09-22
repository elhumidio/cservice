using Domain.Enums;

namespace Domain.DTO.ManageJobs
{
    public class CounterOffersByState
    {
        public Dictionary<OfferDashboardStatus, int> Counts { get; set; }
    }
}
