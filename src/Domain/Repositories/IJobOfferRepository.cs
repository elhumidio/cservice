using Domain.Entities;
namespace Domain.Repositories
{
    public interface IJobOfferRepository
    {
        public IQueryable<JobVacancy> GetActiveOffersByContract(int contractId);
        public IQueryable<JobVacancy> GetOffersByContract(int contractId);
        public IQueryable<JobVacancy> GetActiveOffersByContractAndManager(int contractId, int managerId);
        public IQueryable<JobVacancy> GetActiveOffersByContractAndManagerNoPack(int contractId, int managerId);
        public IQueryable<JobVacancy> GetActiveOffersByContractNoPack(int contractId);
        public IQueryable<JobVacancy> GetActiveOffersByContractOwnerTypeNoPack(int contractId, int owner, int type);
        public IQueryable<JobVacancy> GetActiveOffersByContractOwnerType(int contractId, int owner, int type);
        public IQueryable<JobVacancy> GetActiveOffersByCompany(int enterpriseId);
        IQueryable<JobVacancy> GetConsumedUnitsWelcomeNotSpain(int companyId);
        public int Add(JobVacancy job);
        public JobVacancy GetOfferById(int id);
        Task<int> UpdateOffer(JobVacancy jobUpdated);
        Task<int> FileOffer(JobVacancy job);
        public IQueryable<JobVacancy> GetActiveOffersByContractAndType(int contractId, int type);
        public IQueryable<JobVacancy> GetActiveOffersByContractAndTypeNoPack(int contractId, int type);




    }
}
