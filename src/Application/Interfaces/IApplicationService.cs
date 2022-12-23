using Application.JobOffer.DTO;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IApplicationService
    {
        public Task<GenericOfferCounter> CountApplicantsByOffers(ListOffersRequest jobIds);
        public Task<GenericOfferCounter> CountRedirectsByOffer(ListOffersRequest jobIds);
    }
}
