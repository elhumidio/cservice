using Domain.Entities;
using Domain.Repositories;
using Moq;

namespace TURI.Contractservice.Tests.Unit.Mocks
{
    public static class MockJobOfferRepository
    {
        public static Mock<IJobOfferRepository> GetJobOfferRepository()
        {


            var listOffers = new List<JobVacancy>
            {
                new JobVacancy{ IdjobVacancy=1 },new JobVacancy{IdjobVacancy =2 }


            }.AsQueryable();
            var mockRepo = new Mock<IJobOfferRepository>();
            mockRepo.Setup(r => r.GetActiveOffersByContractAndManager(It.IsAny<int>(), It.IsAny<int>())).Returns(listOffers);
            mockRepo.Setup(r => r.UpdateOffer(It.IsAny<JobVacancy>())).ReturnsAsync(1);


            mockRepo.Setup(r => r.Add(It.IsAny<JobVacancy>())).Returns((JobVacancy job) =>
            {
                listOffers.ToList().Add(job);
                return 1;
            });
            mockRepo.Setup(r => r.FileOffer(It.IsAny<JobVacancy>())).ReturnsAsync(1);


            mockRepo.Setup(r => r.GetActiveOffersByContract(It.IsAny<int>())).Returns(listOffers);
            mockRepo.Setup(r => r.GetActiveOffersByContractNoPack(It.IsAny<int>())).Returns(listOffers);
            mockRepo.Setup(r => r.GetActiveOffersByCompany(It.IsAny<int>())).Returns(listOffers);
            mockRepo.Setup(r => r.GetActiveOffersByContractOwnerType(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(listOffers);
            mockRepo.Setup(r => r.GetActiveOffersByContractAndManagerNoPack(It.IsAny<int>(), It.IsAny<int>())).Returns(listOffers);
            mockRepo.Setup(r => r.GetActiveOffersByContractOwnerTypeNoPack(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(listOffers);
            mockRepo.Setup(r => r.GetConsumedUnitsWelcomeNotSpain(It.IsAny<int>())).Returns(listOffers);
            mockRepo.Setup(r => r.GetOfferById(It.IsAny<int>())).Returns(new JobVacancy { IdjobVacancy = 1 });
            mockRepo.Setup(r => r.GetActiveOffersByContractAndManager(It.IsAny<int>(), It.IsAny<int>())).Returns(listOffers);

            mockRepo.Setup(r => r.GetOffersByContract(It.IsAny<int>())).Returns(listOffers);
            return mockRepo;

        }
    }
}
