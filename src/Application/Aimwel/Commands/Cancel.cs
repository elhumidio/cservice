using Application.Aimwel.Interfaces;
using Application.Core;
using MediatR;

namespace Application.Aimwel.Commands
{
    public class Cancel
    {
        public class Command : IRequest<Result<bool>>
        {
            public int offerId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<bool>>
        {
            private readonly IAimwelCampaign _manageCampaign;

            public Handler(
                IAimwelCampaign aimwelCampaign)
            {
                _manageCampaign = aimwelCampaign;
            }

            public async Task<Result<bool>> Handle(Command request, CancellationToken cancellationToken)
            {
                var response = await _manageCampaign.StopCampaign(request.offerId);
                return Result<bool>.Success(response);
            }
        }
    }
}
