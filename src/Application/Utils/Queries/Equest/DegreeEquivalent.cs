using Application.Core;
using Domain.Repositories;
using MediatR;

namespace Application.Utils.Queries.Equest
{
    public class DegreeEquivalent
    {
        public class Query : IRequest<Result<int>>
        {
            public int DegreeId { get; set; }
            public int SiteId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<int>>
        {
            IEQuestDegreeEquivalentRepository _eqDegreeRepo;
            public Handler(IEQuestDegreeEquivalentRepository eqDegreeRepo)
            {
                _eqDegreeRepo = eqDegreeRepo;
            }

            public async Task<Result<int>> Handle(Query request, CancellationToken cancellationToken)
            {
                var ret = await _eqDegreeRepo.GeteQuestDegree(request.DegreeId, request.SiteId);
                return Result<int>.Success(ret);
            }


        }
    }
}
