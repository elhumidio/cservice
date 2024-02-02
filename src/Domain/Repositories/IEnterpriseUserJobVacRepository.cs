using Domain.Entities;

namespace Domain.Repositories
{
    public interface IEnterpriseUserJobVacRepository
    {
        public Task<int> Add(EnterpriseUserJobVac ujobvac);
        public Task<List<EnterpriseUserJobVac>> GetAssignmentsByUserProductAndContract(int idEnterpriseUser, int idjobvactype, int idcontract);
    }
}
