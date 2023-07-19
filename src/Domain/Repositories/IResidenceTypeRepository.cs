using Domain.Entities;

namespace Domain.Repositories
{
    public interface IResidenceTypeRepository
    {
        public bool IsRightResidenceType(int? residenceTypeId);

        public IQueryable<ResidenceType> GetResidenceTypes(int siteId, int languageId);

        public ResidenceType GetResidenceTypeById(int residenceTypeId, int siteId, int languageId);
    }
}
