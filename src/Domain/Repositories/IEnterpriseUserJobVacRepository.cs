using Domain.DTO.Distribution;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IEnterpriseUserJobVacRepository
    {
        public Task<int> Add(EnterpriseUserJobVac ujobvac);

        public Task<List<EnterpriseUserJobVac>> GetAssignmentsByUserProductAndContract(int idEnterpriseUser, int idjobvactype, int idcontract);

        public Task<List<EnterpriseUserJobVac>> GetAssignmentsByUserIDProductAndContract(int idEnterpriseUser, int idprod, int idcontract);

        public Task<bool> UpdateUnitsAssigned(EnterpriseUserJobVac jvac);
        public Task<List<EnterpriseUserJobVacDto>> GetAssignmentsByUserProductAndContractForOffers(int idEnterpriseUser, int idjobvactype, int idcontract);
        public Task<List<EnterpriseUserJobVacDto>> GetCreditsAssignedFromValidContracts(List<int> contracts, int idEnterpriseUser);
        public EnterpriseUserJobVac GetDistributionByProdUserAndContract(int prod, int user, int contract);
    }
}
