using Domain.Repositories;

namespace Persistence.Repositories
{
    public class IndustryEQRepository : IindustryEQRepository
    {
        private readonly DataContext _dataContext;

        public IndustryEQRepository(DataContext dataContext)
        {

            _dataContext = dataContext;
        }


        public Task<int> GetEQuestIndustryCode(int industryCode)
        {
            var codes = _dataContext.EquestIndustries.Where(i => i.IdindustryCode == industryCode).FirstOrDefault();
            if (codes.EquivalentId != null)
                return Task.FromResult((int)codes.EquivalentId);
            else return Task.FromResult(-1);
        }
    }
}
