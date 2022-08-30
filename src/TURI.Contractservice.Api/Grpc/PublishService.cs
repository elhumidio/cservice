using Application.Contracts.Queries;
using Application.EnterpriseContract.Queries;
using Application.JobOffer.Commands;
using Application.JobOffer.Queries;
using AutoMapper;
using Grpc.Core;
using MediatR;
using System.ComponentModel.Design;
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
        public override async Task<CompanyInfoResult> GetCompanyInfo(UserName info, ServerCallContext context)  {
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
            if (result.Value != null) {
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
            if(result.IsSuccess)
                res.Message = "Offer created";
            else {
                 res.Message = result.Value ;
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
                res.Message = result.Value;
            }
            return res;
        }






    }
}
