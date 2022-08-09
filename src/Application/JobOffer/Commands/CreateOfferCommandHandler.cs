using API.DataContext;
using Application.Core;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using MediatR;
using Persistence.Repositories;

namespace Application.JobOffer.Commands
{
    public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, Result<Unit>>
    {
        #region PRIVATE PROPERTIES

        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IJobOfferRepository offerRepo;
        private readonly IContractRepository contractRepo;
        private readonly IProductRepository productRepo;
        private readonly IValidator<CreateOfferCommand> _validator;
        private readonly IRegJobVacMatchingRepository _regJobVacRepo;
        private readonly IRegEnterpriseContractRepository _regContractRepo;

        #endregion


        public CreateOfferCommandHandler(IRegEnterpriseContractRepository regContractRepo,
            IRegJobVacMatchingRepository regJobVacRepo,
            IValidator<CreateOfferCommand> validator,
            IMapper _mapper,
            IJobOfferRepository _offerRepo,
            IContractRepository _contractRepo,
            IProductRepository _productRepo)
        {
            contractRepo = _contractRepo;
            productRepo = _productRepo;
            offerRepo = _offerRepo;
            mapper = _mapper;
            _validator = validator;
            _regContractRepo = regContractRepo;
            _regJobVacRepo = regJobVacRepo;

        }
        public async Task<Result<Unit>> Handle(CreateOfferCommand offer, CancellationToken cancellationToken)
        {
            var job = new JobVacancy();
            var entity = mapper.Map(offer, job);

            var jobVacancyId = offerRepo.Add(entity);
            if (jobVacancyId == 0)
            {
                return Result<Unit>.Failure("Failed to create offer");
            }
            else
            {
                var updatedUnits = await _regContractRepo.UpdateUnits(job.Idcontract, job.IdjobVacType);

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
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
