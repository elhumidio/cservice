namespace Domain.Repositories
{
    public interface IRegEnterpriseContractRepository
    {
        public Task<int> UpdateUnits(int contractId, int jobTypeId);
        public Task<int> ReduceUnits(int contractId, int jobTypeId);
    }
}
