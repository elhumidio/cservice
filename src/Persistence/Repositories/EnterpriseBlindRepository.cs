using Domain.Repositories;

namespace Persistence.Repositories
{
    public class EnterpriseBlindRepository : IEnterpriseBlindRepository
    {
        private DataContext _dataContext;

        public EnterpriseBlindRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string GetEnterpriseBlindName(int enterpriseId)
        {
            var enterpriseBlind = _dataContext.EnterpriseBlinds.Where(l => l.Identerprise == enterpriseId).FirstOrDefault();
            if (enterpriseBlind == null)
            {
                return "CONFIDENTIAL";
            }
            return enterpriseBlind.Name;
        }
    }
}
