using Application.ContractProducts.DTO;
using Application.ContractProducts.Queries;
using Application.Core;
using Domain.DTO;
using Domain.Repositories;
using MediatR;

namespace Application.ContractProducts.Commands
{
    public class AvailableCreditsByTypeCompanyAndUsersHandler : IRequestHandler<AvailableCreditsByTypeCompanyAndUsersRequest, Result<CreditsAvailableByTypeCompanyAndUsers>>
    {
        private readonly IContractRepository _contractRepository;
        private readonly IMediator _mediatr;

        public AvailableCreditsByTypeCompanyAndUsersHandler(IContractRepository contractRepository,
            IMediator mediator)
        {
            _contractRepository = contractRepository;
            _mediatr = mediator;
        }

        public async Task<Result<CreditsAvailableByTypeCompanyAndUsers>> Handle(AvailableCreditsByTypeCompanyAndUsersRequest request, CancellationToken cancellationToken)
        {
            CreditsAvailableByTypeCompanyAndUsers result = new CreditsAvailableByTypeCompanyAndUsers();
            List<CreditsAvailableByTypeCompanyAndEnterpriseUser> creditsAvailableByTypeCompanyAndUsersList = new List<CreditsAvailableByTypeCompanyAndEnterpriseUser>();

            if (request.IDEnterpriseUsers != null)
            {
                foreach (int IDEnterpriseUsers in request.IDEnterpriseUsers)
                {
                    List<ContractsDistDto> con = await _contractRepository.GetValidContracts(request.IDEnterprise);

                    var credits = await _mediatr.Send(new CreditsAvailableByUser.Query
                    {
                        IDEnterpriseUser = IDEnterpriseUsers,
                        MultiplesUsers = true,
                        ContractsDist = con
                    });

                    if (credits.Value != null)
                    {
                        CreditsAvailableByTypeCompanyAndEnterpriseUser creditsByEnterpriseUser = new CreditsAvailableByTypeCompanyAndEnterpriseUser()
                        {
                            IDEnterpriseUser = IDEnterpriseUsers,
                            ProductCredits = credits.Value.ProductCredits,
                        };
                        creditsAvailableByTypeCompanyAndUsersList.Add(creditsByEnterpriseUser);
                    }
                }
            }

            result.CreditsByUsers = creditsAvailableByTypeCompanyAndUsersList;

            return Result<CreditsAvailableByTypeCompanyAndUsers>.Success(result);
        }
    }
}
