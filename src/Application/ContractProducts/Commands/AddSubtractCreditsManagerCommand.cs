using Application.ContractProducts.DTO;
using Application.Core;
using MediatR;

namespace Application.ContractProducts.Commands
{
    public class AddSubtractCreditsManagerCommand : IRequest<Result<AddSubtractCreditsManagerResponse>>
    {
        public int? identerpriseuser { get; set; } = 0;
        public int? identerprise { get; set; }
        public string? action { get; set; } = string.Empty;
        public int? type { get; set; } = 0;
        public int? idprod { get; set; } = 0;
        
    }
}
