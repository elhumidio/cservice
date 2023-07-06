namespace Domain.DTO
{
    public class VerifyGoalsOfferResponse
    {
        public List<GoalsOffer> GoalsOffersList { get; set; }

        public VerifyGoalsOfferResponse() {
            GoalsOffersList = new List<GoalsOffer>();   
        }   
    }

    public class GoalsOffer
    {
        public int OfferId { get; set; }
        public bool ReachedGoals { get; set; }
    }
}
