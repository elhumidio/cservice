using Application.Core;
using Application.Interfaces;
using Application.JobOffer.Commands;
using Application.Utils;
using AutoMapper;
using Domain.Repositories;
using Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Persistence;
using Persistence.Repositories;

namespace TURI.Contractservice.Tests.Unit.JobOffer.Commands
{
    public class CreateOfferCommandHandlerTest
    {
        //Not set as readonly to allow overwriting later
        private IRegEnterpriseContractRepository _regContractRepo;
        private IRegJobVacMatchingRepository _regJobVacRepo;
        private IMapper _mapper;
        private IJobOfferRepository _offerRepo;
        private IEnterpriseRepository _enterpriseRepository;
        private ILogger<CreateOfferCommandHandler> _logger;
        private IConfiguration _config;
        private IJobVacancyLanguageRepository _jobVacancyLangRepo;
        private IRegJobVacWorkPermitRepository _regJobVacWorkPermitRepository;
        private ICityRepository _cityRepository;
        private IAreaRepository _areaRepository;
        private IQuestService _questService;
        private IApiUtils _utils;
        private IJobTitleDenominationsRepository _denominationsRepository;
        private IAIService _aiService;


        public CreateOfferCommandHandlerTest()
        {
            var baseMockRepo = new MockRepository(MockBehavior.Loose);
            _regContractRepo = baseMockRepo.Create<IRegEnterpriseContractRepository>().Object;
            _regJobVacRepo = baseMockRepo.Create<IRegJobVacMatchingRepository>().Object;
            _mapper = baseMockRepo.Create<IMapper>().Object;
            _offerRepo = baseMockRepo.Create<IJobOfferRepository>().Object;
            _enterpriseRepository = baseMockRepo.Create<IEnterpriseRepository>().Object;
            _logger = new Logger<CreateOfferCommandHandler>(new LoggerFactory());
            _config = baseMockRepo.Create<IConfiguration>().Object;
            _jobVacancyLangRepo = baseMockRepo.Create<IJobVacancyLanguageRepository>().Object;
            _regJobVacWorkPermitRepository = baseMockRepo.Create<IRegJobVacWorkPermitRepository>().Object;
            _cityRepository = baseMockRepo.Create<ICityRepository>().Object;
            _areaRepository = baseMockRepo.Create<IAreaRepository>().Object;
            _questService = baseMockRepo.Create<IQuestService>().Object;
            _utils = baseMockRepo.Create<IApiUtils>().Object;
            _denominationsRepository = baseMockRepo.Create<IJobTitleDenominationsRepository>().Object;
            _aiService = baseMockRepo.Create<IAIService>().Object;


            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfiles>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Test]
        public async Task CreateOfferCommandHandlerSuccess()
        {
            /*var _mediatr = new Mock<IMediator>();
            _mediatr.Setup(m => m.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()));
            var _loggerMock = new Mock<ILogger<CreateOfferCommandHandler>>();

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlServer("Data Source=mssql-brandturijobs.dev.stepstone.tools;Initial Catalog=Turijobs.master;User ID=tjweb;Password=VntEH+5A2kuMqaYH;")
                .Options;
            var dataContext = new DataContext(options);

            _denominationsRepository = new JobTitleDenominationsRepository(dataContext);
            _config = ;
            _aiService = new AIService(_config);

            var handler = new CreateOfferCommandHandler(_regContractRepo,
                _regJobVacRepo,
                _mapper,
                _offerRepo,
                _enterpriseRepository,
                _logger,
                _mediatr.Object,
                _config,
                _jobVacancyLangRepo,
                _regJobVacWorkPermitRepository,
                _cityRepository,
                _areaRepository,
                _questService,
                _utils,
                _denominationsRepository,
                _aiService);

            var result = await handler.Handle(new CreateOfferCommand() { Title = "Art Guard", Idsite = 6, Idarea = 20 }, CancellationToken.None);*/
            // result.Errors.ShouldNotBeNull();
            // result.Errors..ShouldNotMatch("Failed to create offer");
            //   Assert.IsNotNull(result);
        }
    }
}
