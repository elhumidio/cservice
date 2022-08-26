using Grpc.Core;

namespace GrpcContract
{
    public class ContractService : ContractGrpc.ContractGrpcBase
    {
        //    private readonly IMediator _mediator;
        //    private readonly IMapper _mapper;

        /* public ContractService(IMediator mediator, IMapper mapper)
         {
             _mapper = mapper;
             _mediator = mediator;

         }*/
        /*  public override async Task<AvailableUnitsResult> GetAvailableUnits(ContractId request, ServerCallContext context)
          {
              var result = await _mediator.Send(new GetAvailableUnits.Query
              {
                  ContractId = request.Id
              });
              var dest = new AvailableUnitsResult();
              var ret = _mapper.Map(result.Value, dest);
              return ret;
          }*/

        //public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        //{
        //    return new HelloReply { Message = $"Hi {request.Name}!" };
        //}

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply { Message = $"Hi {request.Name}!" });
        }
    }
}
