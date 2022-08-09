namespace Domain.Repositories
{
    public interface IJobCategoryRepository
    {
        public bool IsRightCategory(int? jobCatId);
    }
}
