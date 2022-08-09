namespace Domain.Repositories
{
    public interface IContractPublicationRegionRepository
    {
        public List<int> AllowedRegionsByContract(int idContract);

    }
}
