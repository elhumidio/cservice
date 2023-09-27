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
        private IBackOfUserRepository _backOfUserRepository;
        private IEnterpriseRepository _enterpriseRepository;
        private IMapper _mapper;

        public UpdateContractCommandHandler(IContractRepository contractRepository, IBackOfUserRepository backOfUserRepository, IEnterpriseRepository enterpriseRepository, IMapper mapper)
        {
            _contractRepository = contractRepository;
            _backOfUserRepository = backOfUserRepository;
            _enterpriseRepository = enterpriseRepository;
            _mapper = mapper;
        }

        public Task<Result<bool>> Handle(UpdateContractCommand request, CancellationToken cancellationToken)
        {

            //Check Backoffice user is 0 - If so, check first TBackOfUser with our user, then TEnterprise to get it.
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
                : null;

            //No pack logic

            //--UPDATE Contract
            var contract = new Contract();
            _mapper.Map(request, contract);

            var result = _contractRepository.UpdateContract(contract);

            if (result.Result == false)
                return Task.FromResult(Result<bool>.Failure("Failed to update Contract"));


            //If the finish Date is not the same, we now need to update all the offers for this contract with the new date
            //SP_TcontractChangeOfferDates //TODO: Remove from here

            //SP_TContractProduct_U

            //Add new Units to RegEnterpriseConsums //Being changed from current usage in the future

            //Logic to call SP_UpdateOffers if we've just unapproved the contract - Think this should be a separate call?


            return Task.FromResult(Result<bool>.Success(true));
        }

        private int GetBackOfficeUser(in UpdateContractCommand request)
        {
            if (request.IdBackOfUser > 0)
                return request.IdBackOfUser;

            var registeredBOUser = _backOfUserRepository.GetUserBoID(request.IdUser);
            if (registeredBOUser> 0)
                return registeredBOUser ?? 0;

            var enterpriseUser = _enterpriseRepository.Get(request.IdEnterprise).IdbackOfUser;

            if (enterpriseUser > 0)
                return enterpriseUser;

            return 0;
        }
    }
}
