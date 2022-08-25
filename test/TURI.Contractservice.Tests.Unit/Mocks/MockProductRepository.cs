using Domain.Repositories;
using Moq;

namespace TURI.Contractservice.Tests.Unit.Mocks
{
    public static class MockProductRepository
    {
        public static Mock<IProductRepository> GetProductRepository()
        {
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(r => r.GetProductDuration(It.IsAny<int>())).Returns(90);

            return mockRepo;
        }
    }
}
