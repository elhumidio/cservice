using Application.Aimwel.Interfaces;
using Application.Aimwel.Queries;
using Application.JobOffer.DTO;
using Domain.Entities;
using Domain.Repositories;
using DPGRecruitmentCampaignClient;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.JobOffer.Commands
{
    public class UpdateDate
    {
        public class Command : IRequest<OfferModificationResult>
        {
            public int id { get; set; }
        }

        public class Handler : IRequestHandler<Command, OfferModificationResult>
        {
            private readonly IJobOfferRepository _offerRepo;
            

            public Handler(IJobOfferRepository offerRepo)
            {
                _offerRepo = offerRepo;
            }

            public async Task<OfferModificationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                
                string msg = string.Empty;

                var job = _offerRepo.GetOfferById(request.id);
                if (job == null)
                {
                    return OfferModificationResult.Success(new List<string> { $"Offer {request.id} not available"}) ;
                }
                var ret = _offerRepo.DeleteOffer(job);
                if (ret <= 0)
                {
                    msg += $"Offer {request.id} - Couldn't update it";
                }
                else
                {
                    job.UpdatingDate = DateTime.Now;    
                    await _offerRepo.UpdateOffer(job);
                    msg += $"Offer {request.id} - udpated Successfully ";                                                          
                }

                return OfferModificationResult.Success(new List<string> { msg });
            }
        }
    }
}
