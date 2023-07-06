namespace Domain.DTO
{
    public class VerifyGoalsRequest
    {
        public List<int> OffersList { get; set; }
        public string Feed { get; set; }

        public VerifyGoalsRequest()
        {
            OffersList = new List<int>();
        }
    }
}
