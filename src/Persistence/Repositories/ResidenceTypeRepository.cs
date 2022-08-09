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
    }
}
