using Application.Core;
using Application.JobOffer.DTO;
using Domain.Repositories;
using MediatR;

namespace Application.JobOffer.Queries
{
    public class ListActivesByManagerList
    {
        public class Query : IRequest<Result<List<ContractOwnerResponseDto>>>
        {
            public ContractOwnerRequestDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<ContractOwnerResponseDto>>>
        {
            private readonly IContractProductRepository _contractProductRepo;

            private readonly IMediator _mediator;

            public Handler(
                IContractProductRepository contractProductRepo,
                IMediator mediator)
            {
                _contractProductRepo = contractProductRepo;
                _mediator = mediator;
            }

            public async Task<Result<List<ContractOwnerResponseDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<ContractOwnerResponseDto> responseList = new List<ContractOwnerResponseDto>();
                foreach (var dto in request.Dto.ContractOwnerDtos)
                {
                    var dtoResponse = new ContractOwnerResponseDto();
                    var isPack = _contractProductRepo.IsPack(dto.ContractId);

                    var query = await _mediator.Send(new ListActivesByManager.Query
                    {
                        ContractID = dto.ContractId,
                        OwnerID = dto.OwnerId
                    });

                    dtoResponse.Offers = query.Value.ToList();
                    dtoResponse.OwnerId = dto.OwnerId;
                    dtoResponse.ContractId = dto.ContractId;
                    responseList.Add(dtoResponse);
                }
                return Result<List<ContractOwnerResponseDto>>.Success(responseList);
            }
        }
    }
}

