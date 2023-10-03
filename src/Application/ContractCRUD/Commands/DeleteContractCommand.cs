using Application.Core;
using MediatR;

namespace Application.ContractCRUD.Commands
{
    public class DeleteContractCommand : IRequest<Result<bool>>
    {
        public int IdContract { get; set; }
    }
}
