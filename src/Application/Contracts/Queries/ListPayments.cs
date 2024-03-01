using Application.Core;
using AutoMapper;
using Domain.DTO;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Contracts.Queries
{
    public class ListPayments
    {
        public class Query : IRequest<Result<ContractPaymentDto>>
        {
            public int ContractId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ContractPaymentDto>>
        {
            private readonly IContractPaymentRepository _contractPaymentRepo;
            private readonly IMapper _mapper;

            public Handler(IContractPaymentRepository contractRepo, IMapper mapper)
            {
                _contractPaymentRepo = contractRepo;
                _mapper = mapper;
            }

            public async Task<Result<ContractPaymentDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = await _contractPaymentRepo.GetPaymentByContractId(request.ContractId);
                if(query!=null)
                {
                    return  Result<ContractPaymentDto>.Success(_mapper.Map<ContractPaymentDto>(query));
                }
                return Result<ContractPaymentDto>.Success(new ContractPaymentDto() {
                    ConvertRate=0,
                    PaymentWithoutTax =0,
                    Payment=0,
                    CouponDiscount=0,
                    TaxAmount=0});
            }
        }
    }

}
