using Application.ContractProducts.DTO;
using Application.Core;
using MediatR;
using System.Runtime.Serialization;

namespace Application.ContractProducts.Commands
{
    [DataContract]
    public class ProductDiscountRequest : IRequest<Result<bool>>
    {
        [DataMember]
        public List<ProductDiscountDto> List { get; set; } = new List<ProductDiscountDto>();
    }
}
