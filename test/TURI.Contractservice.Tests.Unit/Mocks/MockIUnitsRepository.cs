using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Moq;

namespace TURI.Contractservice.Tests.Unit.Mocks
{
    public static class MockIUnitsRepository
    {
        public static Mock<IUnitsRepository> GetMockIUnitsRepository()
        {
            var listAssignments = new List<EnterpriseUserJobVac>
            {
                new EnterpriseUserJobVac{
                    Idcontract=1,
                    IdenterpriseUser =1,
                    JobVacUsed =1,
                     IdjobVacType =0

                },
                new EnterpriseUserJobVac{
                    Idcontract=2,
                    IdenterpriseUser =1,
                     JobVacUsed =1,
                     IdjobVacType =0
                }
            }.AsQueryable();

            var mockRepo = new Mock<IUnitsRepository>();
            mockRepo.Setup(r => r.AssignUnitToManager(It.IsAny<int>(), It.IsAny<VacancyType>(), It.IsAny<int>())).Returns(false);
            mockRepo.Setup(r => r.TakeUnitFromManager(It.IsAny<int>(), It.IsAny<VacancyType>(), It.IsAny<int>())).Returns(false);
            mockRepo.Setup(r => r.GetAssignmentsByContract(It.IsAny<int>())).Returns(listAssignments);
            mockRepo.Setup(r => r.GetAssignmentsByContractAndManager(It.IsAny<int>(), It.IsAny<int>())).Returns(listAssignments);
            return mockRepo;
        }
    }
}
