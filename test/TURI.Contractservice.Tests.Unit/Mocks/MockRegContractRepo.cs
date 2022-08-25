using Domain.Repositories;
using Moq;

namespace TURI.Contractservice.Tests.Unit.Mocks
{
    public static class MockRegContractRepo
    {
        public static Mock<IRegEnterpriseContractRepository> GetRegEnterpriseContractRepository()
        {
            var mockRepo = new Mock<IRegEnterpriseContractRepository>();
            mockRepo.Setup(r => r.UpdateUnits(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(1);
            return mockRepo;
        }
    }
}
