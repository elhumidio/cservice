using Application.Core;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;


namespace Application.ContractCRUD.Commands
{
    public class UpdateContractCommandHandler : IRequestHandler<UpdateContractCommand, Result<bool>>
    {
        private IContractRepository _contractRepository;
        private IEnterpriseRepository _enterpriseRepository;
        private IMapper _mapper;

        public UpdateContractCommandHandler(IContractRepository contractRepository, IEnterpriseRepository enterpriseRepository, IMapper mapper)
        {
            _contractRepository = contractRepository;
            _enterpriseRepository = enterpriseRepository;
            _mapper = mapper;
        }

        public Task<Result<bool>> Handle(UpdateContractCommand request, CancellationToken cancellationToken)
        {

            //Check Backoffice user is 0 - If so, check TEnterprise to get it.
            request.IdBackOfUser = GetBackOfficeUser(in request);

            //Get chkApprovedOld and finishDateOld from TContract
            var oldContract = _contractRepository.GetContracts(request.IdEnterprise).FirstOrDefault(con => con.Idcontract == request.IdContract);
            if (oldContract == null)
            {
                return Task.FromResult(Result<bool>.Failure("Contract does not exist"));
            }

            //If only just approved, set the date to today
            request.ApprovedDate = request.chkApproved == true && oldContract.ChkApproved == false
                ? DateTime.Now
                : request.ApprovedDate;

            //No pack logic

            //--UPDATE Contract
            var contract = new Contract();
            _mapper.Map(request, contract);

            var result = _contractRepository.UpdateContract(contract);

            if (result.Result == false)
                return Task.FromResult(Result<bool>.Failure("Failed to update Contract"));

            //TODO: Add new Units to RegEnterpriseConsums - Being changed from current usage in the future?

            return Task.FromResult(Result<bool>.Success(true));
        }

        private int GetBackOfficeUser(in UpdateContractCommand request)
        {
            if (request.IdBackOfUser > 0)
                return request.IdBackOfUser;

            var enterpriseUser = _enterpriseRepository.Get(request.IdEnterprise).IdbackOfUser;

            if (enterpriseUser > 0)
                return enterpriseUser;

            return 0;
        }
    }
}
