using Domain.Entities;
using Domain.Repositories;
using Moq;

namespace TURI.Contractservice.Tests.Unit.Mocks
{
    public static class MockContractRepository
    {
        public static Mock<IContractRepository> GetContractRepository()
        {

            var contractList = new List<Contract>
            {
                new Contract{ Idcontract =1,Identerprise=10175,FinishDate= DateTime.Now.AddDays(90), ChkApproved=true },
                new Contract{Idcontract =2,Identerprise=10175,FinishDate= DateTime.Now.AddDays(90), ChkApproved=false},
                new Contract{Idcontract =3,Identerprise=10175,FinishDate= DateTime.Now.AddDays(90), ChkApproved=true}


            }.AsQueryable();

            var contractListGet = new List<Contract>
            {
                new Contract{ Idcontract =1,Identerprise=10175,FinishDate= DateTime.Now.AddDays(90), ChkApproved=true }

            }.AsQueryable();

            var mockRepo = new Mock<IContractRepository>();
            mockRepo.Setup(r => r.IsValidContract(It.IsAny<int>())).Returns(true);
            mockRepo.Setup(r => r.GetContracts(It.IsAny<int>())).Returns(contractList);
            mockRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(contractListGet);
            return mockRepo;
        }
    }
}
