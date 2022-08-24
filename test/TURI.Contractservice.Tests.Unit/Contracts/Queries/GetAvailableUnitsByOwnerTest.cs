using Application.Contracts.DTO;
using Application.Contracts.Queries;
using Application.Core;
using AutoMapper;
using Domain.Repositories;
using Moq;
using NUnit.Framework;
using Shouldly;
using TURI.Contractservice.Tests.Unit.Mocks;

namespace TURI.Contractservice.Tests.Unit.Contracts.Queries
{
    public class GetAvailableUnitsByOwnerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IJobOfferRepository> _jobOfferRepoMock;
        private readonly Mock<IContractProductRepository> _contractProductRepoMock;
        private readonly Mock<IUnitsRepository> _unitsRepoMock;

        public GetAvailableUnitsByOwnerTest()
        {
            _jobOfferRepoMock = MockJobOfferRepository.GetJobOfferRepository();
            _contractProductRepoMock = MockContractProductRepository.GetContractProductRepository();
            _unitsRepoMock = MockIUnitsRepository.GetMockIUnitsRepository();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfiles>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Test]
        public async Task GetAvailableUnitsByOwner()
        {
            var handler = new GetAvailableUnitsByOwner.Handler(_mapper,
                _jobOfferRepoMock.Object,
                _contractProductRepoMock.Object,
                _unitsRepoMock.Object);

            var result = await handler.Handle(new GetAvailableUnitsByOwner.Query(), CancellationToken.None);
            result.Value.ShouldBeOfType<List<AvailableUnitsDto>>();
            result.Value.Count.ShouldBeGreaterThan(0);
            result.Value.Sum(a => a.Units).ShouldBeGreaterThan(0);
        }
    }
}
