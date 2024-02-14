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

            var contracts = await _contractRepository.GetValidContractsByProduct((int)request.identerprise, (int)request.idprod, SITE, LANG);
            var contractsIds = contracts.Select(a => a.ContractId).ToList();
            var distribution = await _enterpriseUserJobVacRepository.GetCreditsAssignedFromValidContracts(contractsIds, (int)request.identerpriseuser);
            var distributionByType = distribution.Where(d => d.Idproduct == request.idprod).ToList();
            var actualContractsFromDistribution = distributionByType.Select(a => a.Idcontract).ToList();
            var actualContractsFromDistributionForSubtract = distributionByType.Where(c => c.JobVacUsed > 0).Select(a => a.Idcontract).ToList();




            if (request.action == "added")
            {
                var olderContract = _contractRepository.GetOlderContractFromList(actualContractsFromDistribution);
                var distr = _enterpriseUserJobVacRepository.GetDistributionByProdUserAndContract((int)request.idprod, (int)request.identerpriseuser, olderContract);
                var assignment = distributionByType.FirstOrDefault();

                if (assignment != null)
                {//todo get distribution for update
                   
                    distr.JobVacUsed = distr.JobVacUsed + 1;
                    
                    var ret = await _enterpriseUserJobVacRepository.UpdateUnitsAssigned(distr);
                    if (ret)
                    {
                        return Result<AddSubtractCreditsManagerResponse>.Success(new AddSubtractCreditsManagerResponse { Updated = true, ContractId = olderContract }); ;
                    }
                    else
                    {
                        return Result<AddSubtractCreditsManagerResponse>.Failure("couldn't add units");
                    }
                }
            }
            else
            {
                var olderContract = _contractRepository.GetOlderContractFromList(actualContractsFromDistributionForSubtract);
                var distr = _enterpriseUserJobVacRepository.GetDistributionByProdUserAndContract((int)request.idprod, (int)request.identerpriseuser, olderContract);
                var assignment = distributionByType.FirstOrDefault();
                if (assignment != null)
                {
                    distr.JobVacUsed = distr.JobVacUsed - 1;
                    
                    var ret = await _enterpriseUserJobVacRepository.UpdateUnitsAssigned(distr);
                    if (ret)
                    {
                        return Result<AddSubtractCreditsManagerResponse>.Success(new AddSubtractCreditsManagerResponse { Updated = true, ContractId = olderContract }); ;
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
