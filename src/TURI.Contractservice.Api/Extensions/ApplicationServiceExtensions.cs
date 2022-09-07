using Application.Behaviors;
using Application.Interfaces;
using Application.JobOffer.Commands;
using Application.JobOffer.Queries;
using Domain.Repositories;
using FluentValidation.AspNetCore;
using Infraestructure.Integrations;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;
using System.Reflection;

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
            services.AddFluentValidation(new[] { typeof(CreateOfferCommandHandler).GetTypeInfo().Assembly });
            services.AddFluentValidation(new[] { typeof(UpdateOfferCommandHandler).GetTypeInfo().Assembly });
            services.AddMediatR(typeof(CreateOfferCommand).Assembly);
            services.AddMediatR(typeof(ListActives.Handler).Assembly);
            services.AddAutoMapper(typeof(Application.Core.MappingProfiles).Assembly);
            services.AddControllers().AddNewtonsoftJson();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            #region MAPPING REPOSITORIES

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

            #endregion MAPPING REPOSITORIES

            services.AddScoped<IGeoNamesConector, GeoNamesConector>();
            return services;
        }
    }
}
