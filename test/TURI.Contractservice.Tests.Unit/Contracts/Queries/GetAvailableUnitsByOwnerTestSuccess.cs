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
    public class GetAvailableUnitsByOwnerTestSuccess
    {
        private readonly IMapper _mapper;
        private readonly Mock<IJobOfferRepository> _jobOfferRepoMock;
        private readonly Mock<IContractProductRepository> _contractProductRepoMock;
        private readonly Mock<IUnitsRepository> _unitsRepoMock;

        public GetAvailableUnitsByOwnerTestSuccess()
        {
            _jobOfferRepoMock = MockJobOfferRepository.GetJobOfferRepository(true);
            _contractProductRepoMock = MockContractProductRepository.GetContractProductRepository(true);
            _unitsRepoMock = MockIUnitsRepository.GetMockIUnitsRepository(true);
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfiles>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Test]
        public async Task GetAvailableUnitsByOwnerSuccess()
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
