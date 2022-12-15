using Application.Aimwel.Interfaces;
using Application.Interfaces;
using Application.Utils;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using DPGRecruitmentCampaignClient;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Country = DPGRecruitmentCampaignClient.Country;

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
        private readonly ICampaignsManagementRepository _campaignsManagementRepo;
        private readonly IAreaRepository _areaRepository;

        public ManageCampaigns(IConfiguration config,
            IGeoNamesConector geoNamesConector,
            IEnterpriseRepository enterpriseRepo,
            ILogger<ManageCampaigns> logger,
            IlogoRepository logoRepo,
            IZipCodeRepository zipCodeRepo,
            ICountryIsoRepository countryIsoRepo,
            ICampaignsManagementRepository campaignsManagementRepo,
            IAreaRepository areaRepository)
        {
            _config = config;
            _geoNamesConector = geoNamesConector;
            _enterpriseRepo = enterpriseRepo;
            _logger = logger;
            _logoRepo = logoRepo;
            _zipCodeRepo = zipCodeRepo;
            _countryIsoRepo = countryIsoRepo;
            _campaignsManagementRepo = campaignsManagementRepo;
            _areaRepository = areaRepository;
        }

        /// <summary>
        /// It retrieves campaign
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

        /// <summary>
        /// It ends a campaign
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public async Task<bool> StopCampaign(int jobId)
        {
            GrpcChannel channel;
            var campaign = await _campaignsManagementRepo.GetCampaignManagement(jobId);
            campaign.Status = (int)AimwelStatus.CANCELED;
            campaign.LastModificationDate = DateTime.UtcNow;
            campaign.ModificationReason = (int)CampaignModificationReason.FILED;
            await _campaignsManagementRepo.Update(campaign);

            if (string.IsNullOrEmpty(campaign.ExternalCampaignId))
            {
                return false;
            }
            else
            {
                var client = GetClient(out channel);
                var request = new EndCampaignRequest
                {
                    CampaignId = campaign.ExternalCampaignId
                };

                var ret = await client.EndCampaignAsync(request);
                return true;
            }
        }

        /// <summary>
        /// It pause a campaign
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public async Task<bool> PauseCampaign(int jobId)
        {
            GrpcChannel channel;
            var campaign = await _campaignsManagementRepo.GetCampaignManagement(jobId);
            campaign.Status = (int)AimwelStatus.PAUSED;
            campaign.LastModificationDate = DateTime.UtcNow;
            campaign.ModificationReason = (int)CampaignModificationReason.FILED;
            await _campaignsManagementRepo.Update(campaign);
            if (string.IsNullOrEmpty(campaign.ExternalCampaignId))
            {
                return false;
            }
            else
            {
                var client = GetClient(out channel);
                var request = new PauseCampaignRequest
                {
                    CampaignId = campaign.ExternalCampaignId
                };

                var ret = await client.PauseCampaignAsync(request);
                return true;
            }
        }

        /// <summary>
        /// It resume a campaign
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public async Task<bool> ResumeCampaign(int jobId)
        {
            GrpcChannel channel;
            var campaign = await _campaignsManagementRepo.GetCampaignManagement(jobId);
            campaign.Status = (int)AimwelStatus.ACTIVE;
            campaign.LastModificationDate = DateTime.UtcNow;
            campaign.ModificationReason = (int)CampaignModificationReason.MANUAL;
            await _campaignsManagementRepo.Update(campaign);
            if (string.IsNullOrEmpty(campaign.ExternalCampaignId))
            {
                return false;
            }
            else
            {
                var client = GetClient(out channel);
                var request = new ResumeCampaignRequest
                {
                    CampaignId = campaign.ExternalCampaignId
                };

                var ret = await client.ResumeCampaignAsync(request);
                return true;
            }
        }

        /// <summary>
        /// It Gets Campaign state
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public async Task<GetCampaignResponse> GetCampaignState(int jobId)
        {
            GrpcChannel channel;
            var campaignId = await _campaignsManagementRepo.GetAimwellIdByJobId(jobId);
            if (string.IsNullOrEmpty(campaignId))
            {
                return new GetCampaignResponse() { Status = CampaignStatus.Ended };
            }
            else
            {
                var client = GetClient(out channel);
                var request = new GetCampaignRequest
                {
                    CampaignId = campaignId
                };

                var ret = await client.GetCampaignAsync(request);
                return ret;
            }
        }

        /// <summary>
        /// Creates Aimwel campaign
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public async Task<CreateCampaignResponse> CreateCampaing(JobVacancy job)
        {
            CampaignSetting settings;
            settings = await _campaignsManagementRepo.GetCampaignSetting(job);
            if (settings == null)
            {
                settings = new CampaignSetting();
                settings.Goal = 100;
                settings.Budget = 0.000m;
                if (job.Isco == null)
                {
                    job.Isco = _areaRepository.GetIscoDefaultFromArea(job.Idarea);
                }
            }
            GrpcChannel channel;
            var client = GetClient(out channel);
            long units = Convert.ToInt64(decimal.Truncate(settings.Budget));
            int hundredths = settings.Budget == 0.00m ? 0 : ReminderDigits(Convert.ToDouble(settings.Budget), 2);
            string code = _zipCodeRepo.GetZipById((int)job.IdzipCode).Zip;
            var urlLogo = $"{_config["Aimwel:Portal.urlRootStatics"]}" +
                        $"{"/img/"}" +
                        $"{ApiUtils.GetShortCountryBySite((Sites)job.Idsite)}" +
                        $"{"/logos/"}" +
                        $"{_logoRepo.GetLogoByBrand(job.Idbrand).UrlImgBig}";
            var geolocation = _geoNamesConector.GetPostalCodesCollection(code, _countryIsoRepo.GetIsobyCountryId(job.Idcountry));

            try
            {
                var request = new CreateCampaignRequest
                {
                    JobId = job.IdjobVacancy.ToString(),

                    Advertisement = new Advertisement { Branding = ApiUtils.GetBrandBySite(job.Idsite), Uri = ApiUtils.GetUriBySite(job.Idsite).AbsoluteUri },
                    JobContent = new JobContent
                    {
                        JobTitle = job.Title,
                        JobDescription = job.Description,
                        Language = ApiUtils.GetLanguageBySite(job.Idsite),
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
                                JobClassificationType = JobClassificationType.Isco,
                                JobClassificationValue = job.Isco.ToString() //TODO determine which Isco code put here
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
                            Units = units,
                            Hundredths = hundredths
                        }
                    }
                };
                var ans = await CreateCampaign(client, request, new Metadata());
                if (!string.IsNullOrEmpty(ans.CampaignId))
                {
                    CampaignsManagement campaign = new()
                    {
                        Status = (int)AimwelStatus.ACTIVE,
                        Goal = settings.Goal,
                        IdjobVacancy = job.IdjobVacancy,
                        Budget = settings.Budget,
                        ExternalCampaignId = ans.CampaignId,
                        LastModificationDate = DateTime.Now,
                        ModificationReason = (int)CampaignModificationReason.CREATED,
                        Provider = "Aimwell"
                    };

                    await _campaignsManagementRepo.Add(campaign);
                }

                return ans;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return new CreateCampaignResponse { CampaignId = msg };
            }
        }

        public int ReminderDigits(Double number, int count)
        {
            return Convert.ToInt32((number - Math.Truncate(number)).ToString().Substring(2, count));
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
