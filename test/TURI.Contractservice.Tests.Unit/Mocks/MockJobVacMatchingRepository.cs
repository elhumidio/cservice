using Domain.Entities;
using Domain.Repositories;
using Moq;

namespace TURI.Contractservice.Tests.Unit.Mocks
{
    public static class MockJobVacMatchingRepository
    {
        public static Mock<IRegJobVacMatchingRepository> GetJobVacMatchingRepository(bool pass)
        {
            RegJobVacMatching match = new RegJobVacMatching
            {
                IdjobVacancy = 1,
                Redirection = string.Empty,
                ExternalId = "xxxxx",
                AppEmail = "dummy@email.com",
                Identerprise = 10175
            };
            var mockRepo = new Mock<IRegJobVacMatchingRepository>();
            if (pass)
            {
                mockRepo.Setup(r => r.Add(match)).ReturnsAsync(1);
                mockRepo.Setup(r => r.GetAtsIntegrationInfo(It.IsAny<string>())).ReturnsAsync(new RegJobVacMatching());
            }
            else
            {
                match.AppEmail = string.Empty;
                match.ExternalId = string.Empty;
                mockRepo.Setup(r => r.Add(match)).ReturnsAsync(-1);
                mockRepo.Setup(r => r.GetAtsIntegrationInfo(It.IsAny<string>())).ReturnsAsync(new RegJobVacMatching());
            }

            return mockRepo;
        }
    }
}
