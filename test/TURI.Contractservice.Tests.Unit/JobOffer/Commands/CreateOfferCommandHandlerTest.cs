using Application.Aimwel.Interfaces;
using Application.Core;
using Application.JobOffer.Commands;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TURI.Contractservice.Tests.Unit.Mocks;

namespace TURI.Contractservice.Tests.Unit.JobOffer.Commands
{
    public class CreateOfferCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CreateOfferCommandHandler> _logger;
        private readonly Mock<IJobOfferRepository> _jobOfferRepositoryMock;
        private readonly Mock<IRegEnterpriseContractRepository> _regEnterpriseContractRepositoryMock;
        private readonly Mock<IUnitsRepository> _unitsRepositoryMock;
        private readonly Mock<IRegJobVacMatchingRepository> _regVacMatchingRepositoryMock;
        private readonly Mock<IEnterpriseRepository> _enterpriseRepositoryMock;
        private readonly IMediator _mediatr;
        private readonly IAimwelCampaign _managecampaignMock;

        public CreateOfferCommandHandlerTest()
        {
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

            //.Callback<GetResult.Get, CancellationToken>((notification,cToken) =>  GetResult.Get(notification, cToken));
            //.Callback<GetResult.Get, CancellationToken>()

            //(notification,token)=>CreateOfferCommandHandler(notifica));
        }

        [Test]
        public async Task CreateOfferCommandHandlerSuccess()
        {
            /* var _mediatr = new Mock<IMediator>();
             _mediatr.Setup(m => m.Send(It.IsAny<GetResult.Get>(), It.IsAny<CancellationToken>()));
             var _loggerMock = new Mock<ILogger<CreateOfferCommandHandler>>();
             var handler = new CreateOfferCommandHandler(_regEnterpriseContractRepositoryMock.Object,
                 _regVacMatchingRepositoryMock.Object,
                 _mapper,
                 _jobOfferRepositoryMock.Object,
                  _enterpriseRepositoryMock.Object,
                  _loggerMock.Object,
                  _mediatr.Object,_managecampaignMock

                 );

             var result = await handler.Handle(new CreateOfferCommand() { }, CancellationToken.None);*/
            // result.Errors.ShouldNotBeNull();
            // result.Errors..ShouldNotMatch("Failed to create offer");
            //   Assert.IsNotNull(result);
        }
    }
}
