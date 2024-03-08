using Application.ContractProducts.DTO;
using Application.Core;
using MediatR;

namespace Application.ContractProducts.Commands
{
    public class AvailableCreditsByTypeCompanyAndUsersRequest : IRequest<Result<CreditsAvailableByTypeCompanyAndUsers>>
    {  
        
        public int IDEnterprise { get; set; } = -1;
        public List<int>? IDEnterpriseUsers { get; set; }
        
    }
}
