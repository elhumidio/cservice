using Application.Core;
using AutoMapper;
using Domain.DTO.Requests;
using Domain.Repositories;
using MediatR;

namespace Application.ContractCreation.Commands
{
    public class UpdateContractProductSalesforceIdHandler : IRequestHandler<UpdateContractProductSalesforceIdRequest, Result<bool>>
    {
        private readonly IContractProductRepository contractProductrepository;
        private readonly IMapper mapper;

        public UpdateContractProductSalesforceIdHandler(IContractProductRepository _contractProductrepo,IMapper _mapper)
        {
          contractProductrepository = _contractProductrepo;
            mapper = _mapper;
        }

        public async Task<Result<bool>> Handle(UpdateContractProductSalesforceIdRequest request, CancellationToken cancellationToken)
        {
            var args = new UpdateContractProductSForceId();
            args = mapper.Map(request, args);
            var ret = await contractProductrepository.UpdateContractProductSalesforceId(args);
            return Result<bool>.Success(ret);
        }
    }
}
