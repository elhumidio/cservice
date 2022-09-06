using Domain.Repositories;
using Moq;

namespace TURI.Contractservice.Tests.Unit.Mocks
{
    public static class MockEnterpriseRepository
    {
        public static Mock<IEnterpriseRepository> GetEnterpriseRepository(bool pass)
        {
            var mockRepo = new Mock<IEnterpriseRepository>();
            if (pass)
            {
                mockRepo.Setup(r => r.UpdateATS(It.IsAny<int>())).Returns(true);
                mockRepo.Setup(r => r.IsRightCompany(It.IsAny<int>())).Returns(true);
                mockRepo.Setup(r => r.GetSite(It.IsAny<int>())).Returns(6);
            }
            else
            {
                mockRepo.Setup(r => r.UpdateATS(It.IsAny<int>())).Returns(false);
                mockRepo.Setup(r => r.IsRightCompany(It.IsAny<int>())).Returns(false);
                mockRepo.Setup(r => r.GetSite(It.IsAny<int>())).Returns(54);
            }

            return mockRepo;
        }
    }
}
