using Application.Behaviors;
using Application.Interfaces;
using Application.JobOffer.Commands;
using Application.JobOffer.Queries;
using Application.Utils;
using Domain.Repositories;
using FluentValidation.AspNetCore;
using Infraestructure;
using Infraestructure.Integrations;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Persistence.Repositories;
using System.Reflection;
using TURI.ApplicationService.Contracts.Application.Services;
using TURI.EnterpriseService.Contracts.Services;
using TURI.SearchService.Contracts.Search.Services;
using TURI.Seoservice.Contracts.Services;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<Persistence.DataContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            services.AddMvc()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
            services.AddFluentValidation(new[] { typeof(CreateOfferCommandHandler).GetTypeInfo().Assembly });
            services.AddMediatR(typeof(CreateOfferCommand).Assembly);
            services.AddMediatR(typeof(ListActives.Handler).Assembly);
            services.AddAutoMapper(typeof(Application.Core.MappingProfiles).Assembly);
            services.AddControllers().AddNewtonsoftJson();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddSingleton<IMemoryCache, MemoryCache>();
            services.AddSingleton(s => Refit.RestService.For<IApplicationService>(config["ExternalServices:ApplicationService"] ?? ""));
            services.AddSingleton(s => Refit.RestService.For<IEnterpriseService>(config["ExternalServices:EnterpriseService"] ?? ""));
            services.AddSingleton(s => Refit.RestService.For<ISearchService>(config["ExternalServices:SearchService"] ?? ""));
            services.AddSingleton(s => Refit.RestService.For<ISeoService>(config["ExternalServices:SeoService"] ?? ""));
            services.AddSingleton(s => Refit.RestService.For<IAIService>(config["ExternalServices:AIService"] ?? ""));
            services.AddScoped<IApiUtils, ApiUtils>();
            #region MAPPING REPOSITORIES

            services.AddScoped<IAimwelErrorsRepository, AimwelErrorsRepository>();
            services.AddScoped<IZoneUrl, UrlZoneRepository>();
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<IJobOfferRepository, JobOfferRepository>();
            services.AddScoped<IUnitsRepository, UnitsRepository>();
            services.AddScoped<IContractProductRepository, ContractProductRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductLineRepository, ProductLineRepository>();
            services.AddScoped<IAreaRepository, AreaRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IDegreeRepository, DegreeRepository>();
            services.AddScoped<IJobContractTypeRepository, JobContractTypeRepository>();
            services.AddScoped<IJobExpYearsRepository, JobExpYearsRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<IResidenceTypeRepository, ResidenceRepository>();
            services.AddScoped<IJobCategoryRepository, JobCategoryRepository>();
            services.AddScoped<IJobContractTypeRepository, JobContractTypeRepository>();
            services.AddScoped<IJobTypeRepository, JobTypeRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IContractPublicationRegionRepository, ContractPublicationRegionRepository>();
            services.AddScoped<ISalaryTypeRepository, SalaryTypeRepository>();
            services.AddScoped<IEnterpriseRepository, EnterpriseRepository>();
            services.AddScoped<ICountryIsoRepository, CountryIsoRepository>();
            services.AddScoped<IZipCodeRepository, ZipCodeRepository>();
            services.AddScoped<IRegJobVacMatchingRepository, RegJobVacMatchingRepository>();
            services.AddScoped<IRegEnterpriseContractRepository, RegEnterpriseContractRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEnterpriseUserRepository, EnterpriseUserRepository>();
            services.AddScoped<IEQuestDegreeEquivalentRepository, EQuestDegreeEquivalentRepository>();
            services.AddScoped<ICountryStateEQRepository, CountryStateEQRepository>();
            services.AddScoped<IindustryEQRepository, IndustryEQRepository>();
            services.AddScoped<ISalaryRepository, SalaryRepository>();
            services.AddScoped<IJobVacTypeRepository, JobVacTypeRepository>();
            services.AddScoped<ISiteRepository, SiteRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();            
            services.AddScoped<IlogoRepository, LogoRepository>();
            services.AddScoped<IEnterpriseBlindRepository, EnterpriseBlindRepository>();
            services.AddScoped<IJobVacancyLanguageRepository, JobVacancyLanguageRepository>();
            services.AddScoped<IRegJobVacWorkPermitRepository, RegJobVacWorkPermitRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IATSManagerAdminRepository, ATSManagerAdminRepository>();
            services.AddScoped<ITitleRepository, TitleRepository>();
            services.AddScoped<ICampaignsManagementRepository, CampaignsManagementRepository>();
            services.AddScoped<IApplicationServiceLocal, ApplicationLocalService>();
            services.AddScoped<IinternalService, InternalService>();
            services.AddScoped<ISafetyModeration, SafetyModeration>();
            services.AddScoped<IQuestService, QuestService>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IFieldRepository, FieldRepository>();
            services.AddScoped<IContractPaymentRepository, ContractPaymentRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRegEnterpriseConsumsRepository, RegEnterpriseConsumsRepository>();
            services.AddScoped<IEnterpriseUserJobVacRepository, EnterpriseUserJobVacRepository>();
            services.AddScoped<ISalesforceTransactionRepository, SalesforceTransactionRepository>();
            services.AddScoped<IWP_CategoryOfferRelationRepository, WP_CategoryOfferRelationRepository>();
            services.AddScoped<IInternationalDiffusionCountryRepository, InternationalDiffusionCountryRepository>();
            services.AddScoped<IJobTitleDenominationsRepository, JobTitleDenominationsRepository>();

            #endregion MAPPING REPOSITORIES

            services.AddScoped<IGeoNamesConector, GeoNamesConector>();
            return services;
        }
    }
}
