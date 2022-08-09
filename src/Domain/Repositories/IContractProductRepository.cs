namespace Domain.Repositories
{
    public interface IContractProductRepository
    {
        public bool IsPack(int contractId);
        public int GetIdProductByContract(int contractId);
    }
}
