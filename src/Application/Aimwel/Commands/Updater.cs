using Application.Aimwel.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using DPGRecruitmentCampaignClient;
using MediatR;
using Newtonsoft.Json;

namespace Application.Aimwel.Commands
{
    public class Updater
    {
        public class Command : IRequest<Response>
        {
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly IJobOfferRepository _offerRepo;
            private readonly IAimwelCampaign _manageCampaign;
            private readonly ICampaignsManagementRepository _campaignsManagementRepository;

            public Handler(IJobOfferRepository offerRepo, IAimwelCampaign aimwelCampaign, ICampaignsManagementRepository campaignsManagementRepository)
            {
                _offerRepo = offerRepo;
                _manageCampaign = aimwelCampaign;
                _campaignsManagementRepository = campaignsManagementRepository;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
              //  string[] lines = File.ReadAllLines("CampaignManagement.txt");
              //  var campaignslistfromfile = JsonConvert.DeserializeObject<List<CampaignsManagement>>(lines[0]);
              ////  _campaignsManagementRepository.AddRange(campaignslistfromfile);
              //  foreach (var item in campaignslistfromfile)
              //  {
              //      Guid guidOutput = new Guid();
              //      bool isValid = Guid.TryParse(item.ExternalCampaignId, out guidOutput);
              //      if(isValid)
              //          await _campaignsManagementRepository.Add(item);
              //  }

                List<CampaignsManagement> campaignsList = new();
                var needUpdate = await _manageCampaign.GetCampaignNeedsUpdate("aimwel");
                if (needUpdate)
                {
                    var offersList = _offerRepo.GetActiveOffers().ToList();
        
                    foreach (JobVacancy offer in offersList)
                    {
                        bool cancelCampaign = await _manageCampaign.StopCampaign(offer.IdjobVacancy);
                        var campaignCreated = await _manageCampaign.CreateCampaingUpdater(offer);
                        if (campaignCreated != null && !string.IsNullOrEmpty(campaignCreated.ExternalCampaignId))
                        {
                            campaignsList.Add(campaignCreated); 
                        }
                    }
                    _campaignsManagementRepository.AddRange(campaignsList);
                    var json = JsonConvert.SerializeObject(campaignsList);
                    File.WriteAllText("CampaignManagement.txt",json);
                    bool updateMarked = await _manageCampaign.MarkUpdateCampaign("aimwel");
                }
                return new Response();
            }
        }
    }
}
