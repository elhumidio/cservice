using Domain.DTO;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class ContractPublicationRegionRepository : IContractPublicationRegionRepository
    {
        private DataContext _dataContext;

        public ContractPublicationRegionRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<int> AllowedRegionsByContract(int _idContract)
        {
            List<int> regionsAllowed = new List<int>();
            var regions = _dataContext.ContractPublicationRegions.Where(r => r.Idcontract == _idContract && r.ChkActive);

            if (regions.Any())
            {
                regionsAllowed.AddRange(regions.Select(region => region.Idregion));
            }

            return regionsAllowed;
        }

        public async Task<List<RegionsAllowedDto>> GetAllowedRegionsNamesByContract(int contractId)
        {
            var res = await _dataContext.ContractPublicationRegions
                .Join(_dataContext.Regions, p => new { p.Idregion }, cp => new { cp.Idregion },
                    (p, cp) => new { p, cp })

                .Where(o => o.p.Idcontract == contractId && o.cp.Idslanguage == 7)
                .Select(o => new RegionsAllowedDto
                {
                    RegionId = o.p.Idregion,
                    RegionName = o.cp.BaseName
                })
                .ToListAsync();
            return res;
        }
    }
}

