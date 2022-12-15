namespace Application.JobOffer.DTO
{
    public class OfferModificationResult
    {
        public bool IsSuccess { get; set; }
        public OfferResultDto Value { get; set; }
        public List<string> Errors { get; set; }

        public static OfferModificationResult Success(OfferResultDto value) => new OfferModificationResult { IsSuccess = true, Value = value };
        public static OfferModificationResult Success() => new OfferModificationResult { IsSuccess = true};
        public static OfferModificationResult Success(List<string> failures) => new OfferModificationResult { IsSuccess = false, Errors = failures };

        public static OfferModificationResult Failure(List<string> failures) => new OfferModificationResult { IsSuccess = false, Errors = failures };
    }
}
