using Domain.Repositories;
using Moq;

namespace TURI.Contractservice.Tests.Unit.Mocks
{
    public static class MockContractProductRepository
    {
        public static Mock<IContractProductRepository> GetContractProductRepository()
        {
            var mockRepo = new Mock<IContractProductRepository>();
            mockRepo.Setup(r => r.GetIdProductByContract(It.IsAny<int>())).Returns(1);
            mockRepo.Setup(r => r.IsPack(It.IsAny<int>())).Returns(false);
            return mockRepo;
        }
    }
}
