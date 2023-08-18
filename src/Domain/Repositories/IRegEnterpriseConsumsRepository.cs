using Domain.Entities;

namespace Domain.Repositories
{
    public interface IRegEnterpriseConsumsRepository
    {
        public Task<int> Add(RegEnterpriseConsum contract);
    }
}
