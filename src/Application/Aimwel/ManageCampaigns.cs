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

namespace Application.Aimwel
{
    public class ManageCampaigns : IAimwelCampaign
    {
        private readonly IConfiguration _config;
        private readonly IGeoNamesConector _geoNamesConector;
        private readonly IEnterpriseRepository _enterpriseRepo;
        private readonly ILogger _logger;
        private readonly IlogoRepository _logoRepo;
        private readonly IZipCodeRepository _zipCodeRepo;
        private readonly ICountryIsoRepository _countryIsoRepo;
        private readonly IJobOfferRepository _jobOfferRepo;

        public ManageCampaigns(IConfiguration config,
            IGeoNamesConector geoNamesConector,
            IEnterpriseRepository enterpriseRepo,
            ILogger<ManageCampaigns> logger,
            IlogoRepository logoRepo,
            IZipCodeRepository zipCodeRepo,
            ICountryIsoRepository countryIsoRepo,
            IJobOfferRepository jobOfferRepo)
        {
            _config = config;
            _geoNamesConector = geoNamesConector;
            _enterpriseRepo = enterpriseRepo;
            _logger = logger;
            _logoRepo = logoRepo;
            _zipCodeRepo = zipCodeRepo;
            _countryIsoRepo = countryIsoRepo;
            _jobOfferRepo = jobOfferRepo;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task<GetCampaignResponse> GetCampaign(GetCampaignRequest request)
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
            using var channel = GrpcChannel.ForAddress(addressChannel, grpcOptions);
            var client = new CampaignManagement.CampaignManagementClient(channel);
            var ans = await client.GetCampaignAsync(request);

            return ans;
        }

        public async Task<bool> StopAimwelCampaign(int jobId) {
            
            GrpcChannel channel;
            var campaignId = _jobOfferRepo.AimwelIdByJobId(jobId);
            var client = GetClient(out channel);
            var request = new EndCampaignRequest
            {
                CampaignId = campaignId                 
            };
            var ret = await client.EndCampaignAsync(request);   


            return true;
        }

        /// <summary>
        /// Creates Aimwel campaign
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public async Task<CreateCampaignResponse> CreateCampaing(CreateOfferCommand job)
        {
            GrpcChannel channel;        
            var client= GetClient(out channel);
            
            string code = _zipCodeRepo.GetZipById((int)job.IdzipCode).Zip;
            var urlLogo = $"{_config["Aimwel:Portal.urlRootStatics"]}" +
                        $"{"/img/"}" +
                        $"{ApiUtils.GetShortCountryBySite((Sites)job.Idsite)}" +
                        $"{"/logos/"}" +
                        $"{_logoRepo.GetLogoByBrand(job.Idbrand).UrlImgBig}";
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
                        Name = _enterpriseRepo.GetCompanyName(job.Identerprise),
                        LogoUrl = urlLogo,
                    },
                    Location = new Geolocation
                    {
                        CountryIso = Country.Gb,
                        Latitude = geolocation.postalCodes.FirstOrDefault().lat,
                        Longitude = geolocation.postalCodes.FirstOrDefault().lng,
                    },
                    Address = new Address
                    {
                        CountryIso = Country.Nl,
                        State = geolocation.postalCodes.FirstOrDefault().adminName2,
                        City = geolocation.postalCodes.FirstOrDefault().placeName,
                        Street = "",
                        Region = geolocation.postalCodes.FirstOrDefault().adminName1,
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
            var ans = await CreateCampaign(client, request, new Metadata());
            if (!string.IsNullOrEmpty(ans.CampaignId))
            {
                var offer = _jobOfferRepo.GetOfferById(job.IdjobVacancy);
                if (offer != null)
                {
                    offer.AimwelCampaignId = ans.CampaignId;
                    await _jobOfferRepo.UpdateOffer(offer);
                }
            }

            return ans;
        }

        /// <summary>
        /// Gets client
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="client"></param>
        private CampaignManagement.CampaignManagementClient GetClient(out GrpcChannel channel)
        {
            CampaignManagement.CampaignManagementClient client;
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
            return client;
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
