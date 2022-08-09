namespace Domain.Repositories
{
    public interface IJobContractTypeRepository
    {
        public bool IsRightContractType(int? contractTypeId);
    }

}
