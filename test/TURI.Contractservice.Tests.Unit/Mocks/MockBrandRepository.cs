using Domain.Repositories;
using Moq;

namespace TURI.Contractservice.Tests.Unit.Mocks
{
    public static class MockBrandRepository
    {
        public static Mock<IBrandRepository> GetBrandRepository(bool pass)
        {
            var brandsList = new List<int> { 1, 2, 3, 4 };
            if (pass)
            {
                var mockRepo = new Mock<IBrandRepository>();
                mockRepo.Setup(r => r.GetBrands(It.IsAny<int>())).Returns(brandsList);
                mockRepo.Setup(r => r.IsRightBrand(It.IsAny<int>(), It.IsAny<int>())).Returns(brandsList.Any());
                return mockRepo;
            }
            else
            {
                brandsList.Clear();
                var mockRepo = new Mock<IBrandRepository>();
                mockRepo.Setup(r => r.GetBrands(It.IsAny<int>())).Returns(brandsList);
                mockRepo.Setup(r => r.IsRightBrand(It.IsAny<int>(), It.IsAny<int>())).Returns(brandsList.Any());
                return mockRepo;
            }
        }
    }
}
