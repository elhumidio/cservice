using Domain.Entities;

namespace Domain.Repositories
{
    public interface IEnterpriseUserJobVacRepository
    {
        public Task<int> Add(EnterpriseUserJobVac ujobvac);
    }
}
