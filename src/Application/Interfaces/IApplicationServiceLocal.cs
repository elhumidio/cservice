using Domain.DTO;

namespace Application.Interfaces
{
    public interface IApplicationServiceLocal
    {
        public Task<GenericOfferCounter> CountApplicantsByOffers(ListOffersRequest jobIds);
        public Task<GenericOfferCounter> CountRedirectsByOffer(ListOffersRequest jobIds);
    }
}
