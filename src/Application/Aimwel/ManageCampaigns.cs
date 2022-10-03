using Application.Aimwel.Interfaces;
using Application.Interfaces;
using Application.JobOffer.Commands;
using Application.Utils;
using Domain.Enums;
using Domain.Repositories;
using DPGRecruitmentCampaignClient;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Aimwel
{
    public class ManageCampaigns : IAimwelCampaign
    {
        private readonly IConfiguration _config;
        private readonly IGeoNamesConector _geoNamesConector;
        private readonly IEnterpriseRepository _enterpriseRepository;
        private readonly ILogger _logger;
        private readonly IlogoRepository _logoRepository;
        private readonly IZipCodeRepository _zipCodeRepository;
        private readonly ICountryIsoRepository _countryIsoRepo;

        public ManageCampaigns(IConfiguration config,
            IGeoNamesConector geoNamesConector,
            IEnterpriseRepository enterpriseRepository,
            ILogger<ManageCampaigns> logger,
            IlogoRepository logoRepository,
            IZipCodeRepository zipCodeRepository,
            ICountryIsoRepository countryIsoRepository)
        {
            _config = config;
            _geoNamesConector = geoNamesConector;
            _enterpriseRepository = enterpriseRepository;
            _logger = logger;
            _logoRepository = logoRepository;
            _zipCodeRepository = zipCodeRepository;
            _countryIsoRepo = countryIsoRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<GetCampaignResponse> GetCampaign(GetCampaignRequest request) {

            var token = _config["Aimwel:token"].ToString();
            var addressChannel = _config["Aimwel:AddressChannel"];
            var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (!string.IsNullOrEmpty(token))
                {
                    metadata.Add("dpgr-token", token);
                }
                return Task.CompletedTask;
            });
            var channelCredentials = ChannelCredentials.Create(new SslCredentials(), credentials);
            var grpcOptions = new GrpcChannelOptions
            {
                Credentials = channelCredentials
            };
            using var channel = GrpcChannel.ForAddress(addressChannel, grpcOptions);
            var client = new CampaignManagement.CampaignManagementClient(channel);            
            var ans = await client.GetCampaignAsync(request);

            return ans;
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public async Task<CreateCampaignResponse> CreateCampaing(CreateOfferCommand job)
        {
            GrpcChannel channel;
            CampaignManagement.CampaignManagementClient client;
            GetClient(out channel, out client);
            string code = _zipCodeRepository.GetZipById((int)job.IdzipCode).Zip;

            var geolocation = _geoNamesConector.GetPostalCodesCollection(code, _countryIsoRepo.GetIsobyCountryId(job.Idcountry));
            var request = new CreateCampaignRequest
            {
                JobId = job.IdjobVacancy.ToString(),
                Advertisement = new Advertisement { Branding = "turijobs", Uri = _config["Aimwel:UrlTurijobs"] },
                JobContent = new JobContent
                {
                    JobTitle = job.Title,
                    JobDescription = job.Description,
                    Language = Language.EnGb,
                    PublicationTime = Timestamp.FromDateTime(DateTime.UtcNow),
                    HiringOrganization = new HiringOrganization
                    {
                        Name = _enterpriseRepository.GetCompanyName(job.Identerprise),
                        LogoUrl = $"{_config["Aimwel:Portal.urlRootStatics"]}" +
                        $"{"/img/"}" +
                        $"{ApiUtils.GetShortCountryBySite((Sites)job.Idsite)}" +
                        $"{"/logos/"}" +
                        $"{_logoRepository.GetLogoByBrand(job.Idbrand).UrlImgBig}",
                    },
                    Location = new Geolocation
                    {
                        CountryIso = Country.Nl,
                        Latitude = geolocation.postalCodes.FirstOrDefault().lat,
                        Longitude = geolocation.postalCodes.FirstOrDefault().lng,
                    },
                    Address = new Address
                    {
                        CountryIso = Country.Nl,
                        State = geolocation.postalCodes.FirstOrDefault().adminName2, //TODO Get region name
                        City = geolocation.postalCodes.FirstOrDefault().placeName, //TODO Get
                        Street = "",
                        PostalCode = geolocation.postalCodes.FirstOrDefault().postalCode
                    },
                    JobClassification = {
                    new[] {
                      new JobClassificationEntry {
                        JobClassificationType =JobClassificationType.Isco,
                        JobClassificationValue = "1122"
                      },
                    }
                  }
                },
                EndTime = Timestamp.FromDateTime(DateTime.UtcNow.AddDays(14)),
                BudgetBestEffort = new BudgetBestEffort
                {
                    Budget = new Money
                    {
                        Currency = Currency.Eur,
                        Units = 300,
                        Hundredths = 82
                    }
                }
            };
            var serrequest = JsonConvert.SerializeObject(request);
            return await CreateCampaign(client, request, new Metadata());
        }


        /// <summary>
        /// Gets client
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="client"></param>
        private void GetClient(out GrpcChannel channel, out CampaignManagement.CampaignManagementClient client)
        {
            var token = _config["Aimwel:token"].ToString();
            var addressChannel = _config["Aimwel:AddressChannel"];
            var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (!string.IsNullOrEmpty(token))
                {
                    metadata.Add("dpgr-token", token);
                }
                return Task.CompletedTask;
            });
            var channelCredentials = ChannelCredentials.Create(new SslCredentials(), credentials);
            var grpcOptions = new GrpcChannelOptions
            {
                Credentials = channelCredentials
            };
            channel = GrpcChannel.ForAddress(addressChannel, grpcOptions);
            client = new CampaignManagement.CampaignManagementClient(channel);
        }

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="client"></param>
        ///// <param name="request"></param>
        ///// <param name="metadata"></param>
        ///// <returns></returns>
        private async Task<CreateCampaignResponse> CreateCampaign(CampaignManagement.CampaignManagementClient client, CreateCampaignRequest request, Grpc.Core.Metadata metadata)
        {
                // Note that there is also a client.CreateCampaignAsyn(request, metadata); available
                var reply = await client.CreateCampaignAsync(request, metadata);
                _logger.LogInformation("Successfully created campaign: " + reply);                
                return reply;                        
        }



    }
}
