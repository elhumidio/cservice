using Domain.Classes;
using Domain.DTO;
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

        Task<IReadOnlyList<JobDataDefinition>> GetActiveJobs(int maxActiveDays);
        Task<List<int>?> GetActiveJobsByIds(List<int> offersIds);

        IQueryable<JobVacancy> GetConsumedUnitsWelcomeNotSpain(int companyId);

        public int Add(JobVacancy job);

        public IQueryable<JobVacancy> GetActiveOffers();

        public JobVacancy GetOfferById(int id);

        Task<int> UpdateOffer(JobVacancy jobUpdated);

        public int FileOffer(JobVacancy job);

        public IQueryable<JobVacancy> GetActiveOffersByContractAndType(int contractId, int type);

        public IQueryable<JobVacancy> GetActiveOffersByContractAndTypeNoPack(int contractId, int type);

        public Task<List<JobVacancy>> GetOffersCreatedLastTwoDays();

        public int DeleteOffer(JobVacancy job);

        public Task<int> CountOffersPublished(int days);

        public List<int>? GetoffersinCampaigns();

        public Task<List<JobVacancy>> GetInactiveOffersChunk();

        public Task<List<OfferMinInfoAtsDto>> GetOfferInfoByExternalId(string externalId, int companyId);
    }
}
