using Application.Contracts.Queries;
using Application.EnterpriseContract.Queries;
using Application.JobOffer.Commands;
using Application.JobOffer.Queries;
using AutoMapper;
using Grpc.Core;
using MediatR;
using System.ComponentModel.Design;

namespace GrpcContract
{
    public class ContractService : ContractGrpc.ContractGrpcBase
    {
            private readonly IMediator _mediator;
            private readonly IMapper _mapper;

         public ContractService(IMediator mediator, IMapper mapper)
         {
             _mapper = mapper;
             _mediator = mediator;

         }
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
        public override async Task<PublishOfferResult> PublishOffer(Offer offer, ServerCallContext context)
        {
            PublishOfferResult res = new PublishOfferResult();
            CreateOfferCommand command = new CreateOfferCommand();
            var offercommand = _mapper.Map(offer, command);
            var result = await _mediator.Send(offercommand);
            if(result.IsSuccess)
                res.Message = "Offer created";
            else {
                 res.Message = result.Value ;
            }
            return res;
        }




    }
}
