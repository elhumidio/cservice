namespace Application.JobOffer.Commands
{
    public class GetOfferDescriptionByIdCommand
    {
        public int OfferId { get; set; }
        public int LanguageId { get; set; }
        public int SiteId { get; set; }
    }
}
