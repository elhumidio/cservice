using Application.Core;
using Application.JobOffer.Commands;
using AutoMapper;
using Domain.Repositories;
using Moq;
using NUnit.Framework;
using Shouldly;
using TURI.Contractservice.Tests.Unit.Mocks;

namespace TURI.Contractservice.Tests.Unit.JobOffer.Commands
{
    public class CreateOfferCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IContractProductRepository> _contractProductRepoMock;
        private readonly Mock<IContractRepository> _contractRepositoryMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IJobOfferRepository> _jobOfferRepositoryMock;
        private readonly Mock<IRegEnterpriseContractRepository> _regEnterpriseContractRepositoryMock;
        private readonly Mock<IUnitsRepository> _unitsRepositoryMock;
        private readonly Mock<IRegJobVacMatchingRepository> _regVacMatchingRepositoryMock;
        private readonly Mock<IEnterpriseRepository> _enterpriseRepositoryMock;
        private readonly FluentValidation.AbstractValidator<CreateOfferCommand> _validationContextMock;

        public CreateOfferCommandHandlerTest()
        {

            _contractProductRepoMock = MockContractProductRepository.GetContractProductRepository(true);
            _contractRepositoryMock = MockContractRepository.GetContractRepository();
            _productRepositoryMock = MockProductRepository.GetProductRepository();
            _jobOfferRepositoryMock = MockJobOfferRepository.GetJobOfferRepository(true);
            _regEnterpriseContractRepositoryMock = MockRegContractRepo.GetRegEnterpriseContractRepository();
            _unitsRepositoryMock = MockIUnitsRepository.GetMockIUnitsRepository(true);
            _regVacMatchingRepositoryMock = MockJobVacMatchingRepository.GetJobVacMatchingRepository(true);
            _enterpriseRepositoryMock = MockEnterpriseRepository.GetEnterpriseRepository(true);
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfiles>();
            });
            _mapper = mapperConfig.CreateMapper();
        }


        [Test]
        public async Task CreateOfferCommandHandlerSuccess()
        {

            var handler = new CreateOfferCommandHandler(_regEnterpriseContractRepositoryMock.Object,
                _regVacMatchingRepositoryMock.Object,
                _validationContextMock,
                _mapper,
                _jobOfferRepositoryMock.Object,
                _contractRepositoryMock.Object,
                 _productRepositoryMock.Object,
                 _enterpriseRepositoryMock.Object,
                 _contractProductRepoMock.Object
                );

            var result = await handler.Handle(new CreateOfferCommand() { }, CancellationToken.None);
            result.Value.ShouldNotBeNullOrEmpty();
            result.Value.ShouldNotMatch("Failed to create offer");
            Assert.IsNotNull(result);

        }
    }
}
