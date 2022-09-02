using Application.Core;
using Domain.Repositories;
using MediatR;

namespace Application.JobOffer.Commands
{
    public class FileJobs
    {
        public class Command : IRequest<Result<string>>
        {
            public List<int> offers { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<string>>
        {
            private readonly IJobOfferRepository _offerRepo;

            public Handler(IJobOfferRepository offerRepo)
            {
                _offerRepo = offerRepo;
            }

            public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                string msgWrong = string.Empty;
                string msgRight = string.Empty;
                foreach (var id in request.offers)
                {
                    var job = _offerRepo.GetOfferById(id);
                    if (job != null)
                    {
                        var ret = await _offerRepo.FileOffer(job);
                        if (ret == 0)
                            msgRight += $"Failed to file offer {id}\n\r";
                        else msgWrong += $"Offer {id} - Filed Successfully ";
                    }
                }
                return Result<string>.Success($"{msgRight}\n\r{msgWrong}");
            }
        }
    }
}
