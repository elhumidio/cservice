using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class ResidenceRepository : IResidenceTypeRepository
    {
        private readonly DataContext _dataContext;
        public ResidenceRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public bool IsRightResidenceType(int? _residenceTypeId)
        {
            var residenceType = _dataContext.ResidenceTypes.Where(a => a.IdresidenceType == _residenceTypeId);
            return residenceType.Any();
        }

        public IQueryable<ResidenceType> GetResidenceTypes(int siteId, int languageId)
        {
            var residenceTypes = _dataContext.ResidenceTypes
                .Where(a => a.Idsite == siteId && a.Idslanguage == languageId);

            if (residenceTypes != null)
            {
                return residenceTypes;
            }
            else
            {
                return null;
            }
        }
    }
}
