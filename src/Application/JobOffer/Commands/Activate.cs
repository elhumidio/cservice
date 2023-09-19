using Application.Contracts.Queries;
using Application.JobOffer.DTO;
using AutoMapper;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.JobOffer.Commands
{
    public class Activate : IRequest<OfferModificationResult>
    {
        public class Command : IRequest<OfferModificationResult>
        {
            public int id { get; set; }
            public int OwnerId { get; set; }
        }

        public class Handler : IRequestHandler<Command, OfferModificationResult>
        {
            private readonly IJobOfferRepository _offerRepo;
            private readonly IRegEnterpriseContractRepository _regEnterpriseContractRepository;
            private readonly IConfiguration _config;
            private readonly IContractProductRepository _contractProductRepo;
            private readonly IMediator _mediatr;
            private readonly IMapper _mapper;
            private readonly IEnterpriseRepository _enterpriseRepository;

            public Handler(IJobOfferRepository offerRepo,
                IRegEnterpriseContractRepository regEnterpriseContractRepository,
                IConfiguration config,
                IContractProductRepository contractProductRepo,
                IMediator mediatr,
                IMapper mapper,
                IEnterpriseRepository enterpriseRepository)
            {
                _offerRepo = offerRepo;
                _regEnterpriseContractRepository = regEnterpriseContractRepository;
                _config = config;
                _contractProductRepo = contractProductRepo;
                _mediatr = mediatr;
                _mapper = mapper;
                _enterpriseRepository = enterpriseRepository;
            }

            public async Task<OfferModificationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                string msg = string.Empty;
                bool offerUpdated = false;
                var job = _offerRepo.GetOfferById(request.id);
                var company = _enterpriseRepository.Get(job.Identerprise);

                if (job == null)
                {
                    return OfferModificationResult.Success(new List<string> { msg });
                }

                var units = _mediatr.Send(new GetAvailableUnitsByOwner.Query
                {
                    ContractId = job.Idcontract,
                    OwnerId = request.OwnerId
                }).Result.Value;
                bool hasUnits = units != null && units.FirstOrDefault(u => u.type == (VacancyType)job.IdjobVacType).Units > 0;

                if (!hasUnits)
                {
                    msg += "not_enough_units";
                    return OfferModificationResult.Success(new List<string> { msg });
                }
                else
                {
                    job.FilledDate = null;
                    job.ChkFilled = false;
                    job.ChkDeleted = false;
                    job.ModificationDate = DateTime.Now;

                    if (company.Idstatus != (int)EnterpriseStatus.Active)
                        job.Idstatus = (int)OfferStatus.Pending;
                    else
                        job.Idstatus = (int)OfferStatus.Active;

                    var ret = await _offerRepo.UpdateOffer(job);

                    var isPack = _contractProductRepo.IsPack(job.Idcontract);
                    if (isPack)
                        await _regEnterpriseContractRepository.UpdateUnits(job.Idcontract, job.IdjobVacType);
                    msg += $"Activated offer {request.id}";
                    offerUpdated = true;
                }
                OfferResultDto dto = new OfferResultDto();
                dto = _mapper.Map(job, dto);
                return OfferModificationResult.Success(new List<string> { msg });
            }
        }
    }
}
