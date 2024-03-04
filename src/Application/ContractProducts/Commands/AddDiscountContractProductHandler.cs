using Application.Core;
using Domain.Repositories;
using MediatR;

namespace Application.ContractProducts.Commands
{
    public class AddDiscountContractProductHandler : IRequestHandler<ProductDiscountRequest, Result<bool>>
    {
        private readonly IContractProductRepository _contractProdRepo;

        public AddDiscountContractProductHandler(IContractProductRepository contractProductRepository)
        {
            _contractProdRepo = contractProductRepository;
        }

        public async Task<Result<bool>> Handle(ProductDiscountRequest request, CancellationToken cancellationToken)
        {
            var cp = _contractProdRepo.GetContractProducts(request.List.FirstOrDefault().ContractId);
            foreach (var cproduct in cp)
            {
                //add discount
                var discount = request.List.Where(d => d.ProductId == cproduct.Idproduct).FirstOrDefault().CommercialDiscount;
                cproduct.CommercialDiscount = discount;
                var ret = await  _contractProdRepo.Update(cproduct);
                return Result<bool>.Success(ret);
            }
            return Result<bool>.Failure("Couldn't add discount");
   
        }
    }
}
