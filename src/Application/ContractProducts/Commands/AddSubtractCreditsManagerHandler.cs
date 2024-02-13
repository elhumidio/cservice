using Application.ContractProducts.DTO;
using Application.Core;
using Domain.Repositories;
using MediatR;

namespace Application.ContractProducts.Commands
{
    public class AddSubtractCreditsManagerHandler : IRequestHandler<AddSubtractCreditsManagerCommand, Result<AddSubtractCreditsManagerResponse>>
    {
        private const int SITE = 6;
        private const int LANG = 7;
        private readonly IContractRepository _contractRepository;
        private readonly IEnterpriseUserJobVacRepository _enterpriseUserJobVacRepository;

        public AddSubtractCreditsManagerHandler(IContractRepository contractRepository,
            IEnterpriseUserJobVacRepository enterpriseUserJobVacRepository)
        {
            _contractRepository = contractRepository;
            _enterpriseUserJobVacRepository = enterpriseUserJobVacRepository;
        }

        public async Task<Result<AddSubtractCreditsManagerResponse>> Handle(AddSubtractCreditsManagerCommand request, CancellationToken cancellationToken)
        {
            var assigned = 0;

            var contracts = await _contractRepository.GetValidContractsByProduct((int)request.identerprise,(int)request.idprod, SITE, LANG);
            var olderContract = contracts.OrderByDescending(x => x.StartDate).FirstOrDefault();
            var distribution = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContract((int)request.identerpriseuser, (int)request.type, olderContract.ContractId);
            if (request.action == "added")
            {
                var assignment = distribution.FirstOrDefault();

                if (assignment != null)
                {
                    assigned = assignment.JobVacUsed + 1;
                    assignment.JobVacUsed = assigned;
                    var ret = await _enterpriseUserJobVacRepository.UpdateUnitsAssigned(assignment);
                    if (ret)
                    {
                        return Result<AddSubtractCreditsManagerResponse>.Success(new AddSubtractCreditsManagerResponse { Updated=true, ContractId = olderContract.ContractId  });;
                    }
                    else
                    {
                        return Result<AddSubtractCreditsManagerResponse>.Failure("couldn't add units");
                    }
                }
            }
            else
            {
                var assignment = distribution.FirstOrDefault();
                if (assignment != null)
                {
                    assigned = assignment.JobVacUsed - 1;
                    assignment.JobVacUsed = assigned;
                    var ret = await _enterpriseUserJobVacRepository.UpdateUnitsAssigned(assignment);
                    if (ret)
                    {
                        return Result<AddSubtractCreditsManagerResponse>.Success(new AddSubtractCreditsManagerResponse { Updated = true, ContractId = olderContract.ContractId }); ;
                    }
                    else
                    {
                        return Result<AddSubtractCreditsManagerResponse>.Failure("couldn't subtract units");
                    }
                }
               
            }
            return Result<AddSubtractCreditsManagerResponse>.Failure("couldn't find distribution");
        }
    }
}
