using Application.ContractProducts.DTO;
using Application.ContractProducts.Queries;
using Application.Core;
using Domain.DTO;
using Domain.Repositories;
using MediatR;

namespace Application.ContractProducts.Commands
{
    public class AvailableCreditsByTypeCompanyAndUserHandler : IRequestHandler<AvailableCreditsByTypeCompanyAndUserRequest, Result<CreditsAvailableByTypeCompanyAndUser>>
    {
        private readonly IContractRepository _contractRepository;
        private readonly IMediator _mediatr;

        public AvailableCreditsByTypeCompanyAndUserHandler(IContractRepository contractRepository,
            IMediator mediator)
        {
            _contractRepository = contractRepository;
            _mediatr = mediator;
        }

        public async Task<Result<CreditsAvailableByTypeCompanyAndUser>> Handle(AvailableCreditsByTypeCompanyAndUserRequest request, CancellationToken cancellationToken)
        {
            List<ContractsDistDto> con = await _contractRepository.GetValidContracts(request.IDEnterprise);

            var credits = await _mediatr.Send(new CreditsAvailableByUser.Query
            {
                IDEnterpriseUser = request.IDEnterpriseUser,
                MultiplesUsers = false,
                ContractsDist = con
            });

            return Result<CreditsAvailableByTypeCompanyAndUser>.Success(credits.Value);
        }
    }
}
