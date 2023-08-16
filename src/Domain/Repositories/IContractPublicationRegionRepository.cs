using Domain.Entities;

namespace Domain.Repositories
{
    public interface IContractPublicationRegionRepository
    {
        public List<int> AllowedRegionsByContract(int idContract);
        public Task<List<Domain.DTO.RegionsAllowedDto>> GetAllowedRegionsNamesByContract(int contractId);
        public Task<bool> AddRestriction(ContractPublicationRegion regionRestriction);
        
    }
}
