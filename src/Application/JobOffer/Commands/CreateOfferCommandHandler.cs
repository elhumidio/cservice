using Application.Core;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.JobOffer.Commands
{
    public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, Result<string>>
    {
        #region PRIVATE PROPERTIES

        private readonly IMediator mediator;
        private readonly IMapper _mapper;
        private readonly IJobOfferRepository _offerRepo;
        private readonly IContractRepository _contractRepo;
        private readonly IProductRepository _productRepo;
        private readonly IValidator<CreateOfferCommand> _validator;
        private readonly IRegJobVacMatchingRepository _regJobVacRepo;
        private readonly IRegEnterpriseContractRepository _regContractRepo;
        private readonly IEnterpriseRepository _enterpriseRepository;
        private readonly IContractProductRepository _contractProductRepo;

        #endregion


        public CreateOfferCommandHandler(IRegEnterpriseContractRepository regContractRepo,
            IRegJobVacMatchingRepository regJobVacRepo,
            IValidator<CreateOfferCommand> validator,
            IMapper mapper,
            IJobOfferRepository offerRepo,
            IContractRepository contractRepo,
            IProductRepository productRepo,
            IEnterpriseRepository enterpriseRepository, IContractProductRepository contractProductRepo)
        {
            _contractProductRepo = contractProductRepo;
            _contractRepo = contractRepo;
            _productRepo = productRepo;
            _offerRepo = offerRepo;
            _mapper = mapper;
            _regContractRepo = regContractRepo;
            _regJobVacRepo = regJobVacRepo;
            _enterpriseRepository = enterpriseRepository;
        }
        public async Task<Result<string>> Handle(CreateOfferCommand offer, CancellationToken cancellationToken)
        {
            var job = new JobVacancy();
            var integrationInfo = await _regJobVacRepo.GetAtsIntegrationInfo(offer.IntegrationData.ApplicationReference);
            if (integrationInfo != null && integrationInfo.IdjobVacancy > 0) //caso update ATS
            {
                var existentOfferAts = _offerRepo.GetOfferById(integrationInfo.IdjobVacancy);
                return Result<string>.Failure("Offer already exists");
            }
            else
            {
                var entity = _mapper.Map(offer, job);
                var jobVacancyId = _offerRepo.Add(entity);
                if (jobVacancyId == 0)
                {
                    return Result<string>.Failure("Failed to create offer");
                }
                else
                {
                    await _regContractRepo.UpdateUnits(job.Idcontract, job.IdjobVacType);

                    if (!string.IsNullOrEmpty(offer.IntegrationData.ApplicationReference))
                    {
                        RegJobVacMatching obj = new RegJobVacMatching
                        {
                            ExternalId = offer.IntegrationData.ApplicationReference,
                            Idintegration = offer.IntegrationData.IDIntegration,
                            IdjobVacancy = jobVacancyId,
                            AppEmail = offer.IntegrationData.ApplicationEmail,
                            Identerprise = offer.Identerprise,
                            Redirection = offer.IntegrationData.ApplicationUrl
                        };
                        await _regJobVacRepo.Add(obj);
                    }
                    _enterpriseRepository.UpdateATS(entity.Identerprise);

                    return Result<string>.Success("Offer Added");
                }
            }



        }
        #region PRIVATE METHODS
        private Result<string> Update(CreateOfferCommand offer, JobVacancy existentOfferAts)
        {
            bool IsActivate = existentOfferAts.ChkFilled;
            bool IsPack = _contractProductRepo.IsPack(existentOfferAts.Idcontract);
            bool CantUpdate = (IsActivate && !IsPack);
            if (CantUpdate)
            {
                return Result<string>.Failure("Failed to Update offer");
            }
            else
            {
                var entity = _mapper.Map(offer, existentOfferAts);
                if (IsActivate)
                {
                    existentOfferAts.FilledDate = null;
                }


                _offerRepo.UpdateOffer(existentOfferAts);
                _enterpriseRepository.UpdateATS(entity.Identerprise);
                return Result<string>.Success("Updated");
            }
        }

        private Result<string> Update(CreateOfferCommand offer)
        {
            var existentOffer = _offerRepo.GetOfferById(offer.IdjobVacancy);
            bool IsActivate = existentOffer.ChkFilled;
            bool IsPack = _contractProductRepo.IsPack(existentOffer.Idcontract);
            bool CantUpdate = (IsActivate && !IsPack);
            if (CantUpdate)
            {
                return Result<string>.Failure("Failed to Update offer");
            }
            else
            {
                var entity = _mapper.Map(offer, existentOffer);
                _offerRepo.UpdateOffer(existentOffer);
                return Result<string>.Success("Updated");
            }
        }
        #endregion
    }
}
