using Domain.Repositories;

namespace Persistence.Repositories
{
    public class ContractPublicationRegionRepository : IContractPublicationRegionRepository
    {
        DataContext _dataContext;

        public ContractPublicationRegionRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public List<int> AllowedRegionsByContract(int _idContract)
        {
            List<int> regionsAllowed = new List<int>();
            var allRegionsAllowed = !_dataContext.ContractPublicationRegions.Where(r => r.Idcontract == _idContract && r.ChkActive).Any();

            var regions = _dataContext.ContractPublicationRegions.Where(r => r.Idcontract == _idContract && r.ChkActive);

            if (regions.Any())
            {
                regionsAllowed.AddRange(regions.Select(region => region.Idregion));
            }

            return regionsAllowed;

        }
    }
}
