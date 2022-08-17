using Application.Core;
using Domain.Repositories;
using MediatR;

namespace Application.Utils.Queries.Equest
{
    public class IndustryEquivalent
    {
        public class Query : IRequest<Result<int>>
        {
            public int industryCode { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<int>>
        {
            IindustryEQRepository _eqindustryRepo;
            public Handler(IindustryEQRepository eqindustryRepo)
            {
                _eqindustryRepo = eqindustryRepo;
            }

            public async Task<Result<int>> Handle(Query request, CancellationToken cancellationToken)
            {
                var ret = await _eqindustryRepo.GetEQuestIndustryCode(request.industryCode);
                return Result<int>.Success(ret);
            }


        }
    }
}
