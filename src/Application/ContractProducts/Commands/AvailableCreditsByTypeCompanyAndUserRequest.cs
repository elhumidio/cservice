using Application.ContractProducts.DTO;
using Application.Core;
using MediatR;

namespace Application.ContractProducts.Commands
{
    public class AvailableCreditsByTypeCompanyAndUserRequest : IRequest<Result<CreditsAvailableByTypeCompanyAndUser>>
    {  
        
        public int IDEnterprise { get; set; } = -1;
        public int IDEnterpriseUser { get; set; } = -1;
        public bool IsAdmin { get; set; }
        
    }
}
