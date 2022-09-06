using Application.Contracts.Queries;
using Application.EnterpriseContract.Queries;
using Application.JobOffer.Commands;
using Application.JobOffer.Queries;
using Application.Utils.Queries.Equest;
using AutoMapper;
using Grpc.Core;
using MediatR;
using TURI.Contractservice.Grpc.MappingProfiles;

namespace GrpcPublish
{
    public class PublishService : PublishGrpc.PublishGrpcBase
    {
        private readonly IMediator _mediator;
        private readonly MapperConfiguration _mapperConfig;
        private readonly IMapper _mapper;

        public PublishService(IMediator mediator)
        {
            _mediator = mediator;
            _mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfiles>();
            });
            _mapper = _mapperConfig.CreateMapper();
        }

        /// <summary>
        /// Gets Available Units by contract
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<AvailableUnitsResult> GetAvailableUnits(ContractIdRequest request, ServerCallContext context)
        {
            var result = await _mediator.Send(new GetAvailableUnits.Query
            {
                ContractId = request.Id
            });
            var dest = new AvailableUnitsResult();

            foreach (var item in result.Value)
            {
                AvailableUnitsDto dto = new AvailableUnitsDto
                {
                    ContractId = item.ContractId,
                    IsPack = item.IsPack,
                    OwnerId = item.OwnerId,
                    Units = item.Units
                };
                dest.Units.Add(dto);
            }
            return dest;
        }

        /// <summary>
        /// Gets basic company info
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<CompanyInfoResult> GetCompanyInfo(UserName info, ServerCallContext context)
        {
            var result = await _mediator.Send(new GetCompanyInfo.Query
            {
                Email = info.User
            });
            CompanyInfoResult ans = new CompanyInfoResult();
            ans.Brands.AddRange(result.Value.Brands.AsEnumerable());
            ans.CompanyId = result.Value.CompanyId;
            ans.IDEnterpriseUser = result.Value.IDEnterpriseUser;
            ans.IDSUser = result.Value.IDSUser;
            ans.UserEmail = result.Value.UserEmail;
            ans.SiteId = result.Value.SiteId;

            return ans;
        }

        /// <summary>
        /// Gets Contract And Type
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<ContractAndTypeResult> GetContractAndType(CompanyAndTypeRequest info, ServerCallContext context)
        {
            var result = await _mediator.Send(new GetContract.Query
            {
                CompanyId = info.CompanyId,
                type = (Domain.Enums.VacancyType)info.Type
            });

            ContractAndTypeResult ans = new ContractAndTypeResult();
            if (result.Value != null)
            {
                ans.ContractId = result.Value.Idcontract;
                ans.Type = (VacancyType)result.Value.IdJobVacType;
            }
            else
            {
                ans.ContractId = 0;
            }

            return ans;
        }

        /// <summary>
        /// Publish an offer
        /// </summary>
        /// <param name="offer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<GenericMessage> PublishOffer(Offer offer, ServerCallContext context)
        {
            GenericMessage res = new();
            CreateOfferCommand command = new();
            var offercommand = _mapper.Map(offer, command);
            var result = await _mediator.Send(offercommand);
            if (result.IsSuccess)
                res.Message = "Offer created";
            else
            {
                var msg = string.Empty;
                foreach (string failure in result.Failures)
                {
                    res.Message += $"{failure} \n\r";
                }
            }
            return res;
        }

        /// <summary>
        /// Files offers
        /// </summary>
        /// <param name="_offers"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<GenericMessage> FileOffers(ListInt _offers, ServerCallContext context)
        {
            List<int> listIds = new List<int>();
            listIds.AddRange(_offers.Ids);
            var result = await _mediator.Send(new FileJobs.Command
            {
                offers = listIds
            });

            GenericMessage res = new()
            {
                Message = result.Value
            };
            return res;
        }

        /// <summary>
        /// Updates Offer
        /// </summary>
        /// <param name="offer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<GenericMessage> UpdateOffer(Offer offer, ServerCallContext context)
        {
            GenericMessage res = new();
            UpdateOfferCommand command = new();
            var offercommand = _mapper.Map(offer, command);
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
                res.Message = "Offer created";
            else
            {
                var msg = string.Empty;
                foreach (string failure in result.Failures)
                {
                    res.Message += $"{failure} \n\r";
                }
            }
            return res;
        }

        /// <summary>
        /// retrieves an offer from external publisher, id not exists return 0
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<IdOffer> GetAtsOffer(OfferExternalReference reference, ServerCallContext context)
        {
            var result = await _mediator.Send(new VerifyOffer.Query
            {
                ExternalId = reference.ExternalReference
            });

            return new IdOffer { IdJobVacancy = result.Value };
        }

        /// <summary>
        /// File Ats Offer
        /// </summary>
        /// <param name="data"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<GenericMessage> FileAtsOffer(IntegrationData data, ServerCallContext context)
        {
            var apiData = new Application.JobOffer.Commands.FileAtsOfferCommand();
            var adaptedData = _mapper.Map(data, apiData);
            var result = await _mediator.Send(adaptedData);
            return new GenericMessage() { Message = String.Empty };
        }

        /// <summary>
        /// Get Equest Degree
        /// </summary>
        /// <param name="values"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<GenericIntReqRet> GetEquestDegree(EquestValue values, ServerCallContext context)
        {
            var result = await _mediator.Send(new DegreeEquivalent.Query
            {
                DegreeId = values.EqDegreeId,
                SiteId = values.SiteId
            });
            return new GenericIntReqRet() { Value = result.Value };
        }

        /// <summary>
        ///  Get EQuest Industry Code
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<GenericIntReqRet> GetEQuestIndustryCode(GenericIntReqRet id, ServerCallContext context)
        {
            var result = await _mediator.Send(new IndustryEquivalent.Query
            {
                industryCode = id.Value
            });

            return new GenericIntReqRet { Value = result.Value };
        }

        /// <summary>
        /// Get EQuest Country State
        /// </summary>
        /// <param name="value"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<GenericIntReqRet> GetEQuestCountryState(GenericMessage value, ServerCallContext context)
        {
            var result = await _mediator.Send(new CountryStateEQuivalent.Query
            {
                countryId = value.Message
            });

            return new GenericIntReqRet() { Value = result.Value };
        }
    }
}
