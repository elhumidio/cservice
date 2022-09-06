using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Moq;

namespace TURI.Contractservice.Tests.Unit.Mocks
{
    public static class MockIUnitsRepository
    {
        public static Mock<IUnitsRepository> GetMockIUnitsRepository(bool success)
        {
            var genericList = new List<EnterpriseUserJobVac>();

            var listAssignments = new List<EnterpriseUserJobVac>
            {
                new EnterpriseUserJobVac{
                    Idcontract=1,
                    IdenterpriseUser =1,
                    JobVacUsed =4,
                     IdjobVacType =0
                },
                new EnterpriseUserJobVac{
                    Idcontract=1,
                    IdenterpriseUser =1,
                     JobVacUsed =4,
                     IdjobVacType =1
                }
            }.AsQueryable();
            var listAssignmentsWrong = new List<EnterpriseUserJobVac>
            {
                new EnterpriseUserJobVac{
                    Idcontract=1,
                    IdenterpriseUser =1,
                    JobVacUsed =0,
                     IdjobVacType =0
                },
                new EnterpriseUserJobVac{
                    Idcontract=1,
                    IdenterpriseUser =1,
                     JobVacUsed =0,
                     IdjobVacType =1
                }
            }.AsQueryable();

            if (!success)
            {
                genericList.AddRange(listAssignmentsWrong);
            }
            else
            {
                genericList.AddRange(listAssignments);
            }
            var mockRepo = new Mock<IUnitsRepository>();
            mockRepo.Setup(r => r.AssignUnitToManager(It.IsAny<int>(), It.IsAny<VacancyType>(), It.IsAny<int>())).Returns(false);
            mockRepo.Setup(r => r.TakeUnitFromManager(It.IsAny<int>(), It.IsAny<VacancyType>(), It.IsAny<int>())).Returns(false);
            mockRepo.Setup(r => r.GetAssignmentsByContract(It.IsAny<int>())).Returns(listAssignments);
            mockRepo.Setup(r => r.GetAssignmentsByContractAndManager(It.IsAny<int>(), It.IsAny<int>())).Returns(genericList.AsQueryable());

            return mockRepo;
        }
    }
}
