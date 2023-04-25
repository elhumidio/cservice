using Application.Aimwel.Interfaces;
using Application.DTO.GeoNames;
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
using Newtonsoft.Json;
using System;
using System.Text;

namespace Application.Aimwel
{
    /// <summary>
    /// This class provides methods to manage campaigns in the Aimwel system.
    /// </summary>
    public class ManageCampaigns : IAimwelCampaign
    {
        private readonly IZoneUrl _zoneUrl;
        private readonly IConfiguration _config;
        private readonly IGeoNamesConector _geoNamesConector;
        private readonly IEnterpriseRepository _enterpriseRepo;
        private readonly ILogger _logger;
        private readonly IlogoRepository _logoRepo;
        private readonly IZipCodeRepository _zipCodeRepo;
        private readonly ICountryIsoRepository _countryIsoRepo;
        private readonly ICampaignsManagementRepository _campaignsManagementRepo;
        private readonly IAreaRepository _areaRepository;
        private readonly IRegionRepository _regionRepo;
        private readonly IAimwelErrorsRepository _aimwelErrorsRepository;
        private readonly ICampaignsManagementRepository _campaignsManagementRepository;

        private readonly ICountryRepository _countryRepo;

        public ManageCampaigns(IConfiguration config,
            IGeoNamesConector geoNamesConector,
            IEnterpriseRepository enterpriseRepo,
            ILogger<ManageCampaigns> logger,
            IlogoRepository logoRepo,
            IZipCodeRepository zipCodeRepo,
            ICountryIsoRepository countryIsoRepo,
            ICampaignsManagementRepository campaignsManagementRepo,
            IAreaRepository areaRepository,
            IRegionRepository regionRepository,
            ICountryRepository countryRepository,
            IZoneUrl zoneUrl,
            IAimwelErrorsRepository aimwelErrorsRepository,
            ICampaignsManagementRepository campaignsManagementRepository)
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
            _regionRepo = regionRepository;
            _countryRepo = countryRepository;
            _zoneUrl = zoneUrl;
            _aimwelErrorsRepository = aimwelErrorsRepository;
            _campaignsManagementRepository = campaignsManagementRepository;
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
            var campaigns = await _campaignsManagementRepo.GetAllCampaignManagement(jobId);
            foreach (var campaign in campaigns)
            {
                if (campaign.Status != (int)AimwelStatus.CANCELED)
                {
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

                        try
                        {
                            var ret = await client.EndCampaignAsync(request);
                        }
                        catch (Exception ex)
                        {
                            var a = ex.Message;
                        }
                    }
                }
            }
            return true;
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
                try {
                    var ret = await client.GetCampaignAsync(request);
                    return ret;
                } catch {
                    return null;
                }   
                
            }
        }

        /// <summary>
        /// Gets the campaign by Aimwel ID.
        /// </summary>
        /// <param name="aimwelId">The Aimwel ID.</param>
        /// <returns>The campaign response.</returns>
        public async Task<GetCampaignResponse> GetCampaignByAimwelId(string aimwelId)
        {
            GrpcChannel channel;

            var client = GetClient(out channel);
            var request = new GetCampaignRequest
            {
                CampaignId = aimwelId
            };

            var ret = await client.GetCampaignAsync(request);
            return ret;
        }

        /// <summary>
        /// Gets the campaign needs update.
        /// </summary>
        /// <param name="campaignName">Name of the campaign.</param>
        /// <returns>The campaign needs update.</returns>
        public async Task<bool> GetCampaignNeedsUpdate(string campaignName)
        {
            return await _campaignsManagementRepo.GetCampaignNeedsUpdate(campaignName);
        }

        /// <summary>
        /// Marks a campaign as updated in the database.
        /// </summary>
        /// <param name="campaignName">The name of the campaign to mark as updated.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task<bool> MarkUpdateCampaign(string campaignName)
        {
            return await _campaignsManagementRepo.MarkCampaignUpdated(campaignName);
        }

        /// <summary>
        /// Creates Aimwel campaign
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public async Task<CreateCampaignResponse> CreateCampaing(JobVacancy job)
        {
            if (job.Idsite == (int)Sites.ITALY || job.Idsite == (int)Sites.MEXICO)
            {
                return new CreateCampaignResponse() { CampaignId = String.Empty };
            }
            CampaignSetting settings;
            try
            {
                settings = await _campaignsManagementRepo.GetCampaignSetting(job);
                if (settings == null)
                {
                    settings = new CampaignSetting();
                    settings.Goal = 100;
                    settings.Budget = 0.000m;
                }
                job.Isco ??= _areaRepository.GetIscoDefaultFromArea(job.Idarea);
                GrpcChannel channel;
                string logo = string.Empty;
                string urlLogo = string.Empty;
                var client = GetClient(out channel);
                double latitude = 0d;
                double longitude = 0d;
                string PostalCode = string.Empty;
                string countryISO = string.Empty;
                var regionId = _enterpriseRepo.GetCompanyRegion(job.Identerprise);
                var companyRegion = _regionRepo.Get(regionId);
                GeoNamesDto geolocation = null;
                var region = _regionRepo.Get(job.Idregion);
                long units = Convert.ToInt64(decimal.Truncate(settings.Budget));
                var inputValue = Math.Round(settings.Budget, 2, MidpointRounding.AwayFromZero);
                int hundredths = settings.Budget == 0.00m ? 0 : ReminderDigits(inputValue, 2);
                var countryName = _countryRepo.GetCountryById(job.Idcountry).BaseName;
                if (job.IdzipCode == null || job.IdzipCode == -1 || job.IdzipCode == 0)
                {
                    if (job.Idcountry == 226)
                    {
                        latitude = Convert.ToDouble("40.60704563565361");
                        longitude = Convert.ToDouble("-4.049663543701172");
                        PostalCode = "28292";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(job.City))
                        {
                            countryISO = _countryIsoRepo.GetIsobyCountryId(job.Idcountry);
                            geolocation = _geoNamesConector.GetPostalCodesCollectionByPlaceName(job.City, countryISO);

                            //if geolocation not GOOGLE will be
                            if (geolocation != null && geolocation.postalCodes.Any())
                            {
                                latitude = geolocation.postalCodes.FirstOrDefault().lat;
                                longitude = geolocation.postalCodes.FirstOrDefault().lng;
                                PostalCode = geolocation.postalCodes.FirstOrDefault().postalCode;
                            }
                            else
                            {
                                countryISO = _countryIsoRepo.GetIsobyCountryId(job.Idcountry);
                                geolocation = _geoNamesConector.GetPostalCodesCollectionByPlaceName(companyRegion.BaseName.ToLower(), countryISO);
                                //if geolocation not GOOGLE will be
                                if (geolocation != null && geolocation.postalCodes.Any())
                                {
                                    latitude = geolocation.postalCodes.FirstOrDefault().lat;
                                    longitude = geolocation.postalCodes.FirstOrDefault().lng;
                                    PostalCode = geolocation.postalCodes.FirstOrDefault().postalCode;
                                }
                            }
                        }
                        else
                        {
                            if (job.Idregion == 60)
                            {
                                if (regionId != 61)
                                {
                                    //TODO get name
                                    countryISO = _countryIsoRepo.GetIsobyCountryId(job.Idcountry);
                                    geolocation = _geoNamesConector.GetPostalCodesCollectionByPlaceName(companyRegion.BaseName.ToLower(), countryISO);
                                    //if geolocation not GOOGLE will be
                                    if (geolocation != null && geolocation.postalCodes.Any())
                                    {
                                        latitude = geolocation.postalCodes.FirstOrDefault().lat;
                                        longitude = geolocation.postalCodes.FirstOrDefault().lng;
                                        PostalCode = geolocation.postalCodes.FirstOrDefault().postalCode;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    var code = _zipCodeRepo.GetZipById((int)job.IdzipCode);
                    if (code != null)
                    {
                        latitude = Convert.ToDouble(code.Latitude);
                        longitude = Convert.ToDouble((code.Longitude));
                        PostalCode = code.Zip;
                    }
                }
                var logoEntity = _logoRepo.GetLogoByBrand(job.Idbrand);
                if (logoEntity == null)
                {
                    urlLogo = "https://www.turijobs.com/static/img/global/nologo.png";
                }
                else
                {
                    logo = logoEntity.UrlImgBig;
                    urlLogo = $"{_config["Aimwel:Portal.urlRootStatics"]}" +
                  $"{"/img/"}" +
                  $"{ApiUtils.GetShortCountryBySite((Sites)job.Idsite)}" +
                  $"{"/logos/"}" +
                  $"{logo}";
                }

                var address = new Address
                {
                    CountryAlpha2 = _countryIsoRepo.GetIsobyCountryId(job.Idcountry),
                    State = region == null ? companyRegion.Ccaa : region.Ccaa == null ? region.BaseName : region.Ccaa,
                    City = job.City ?? geolocation.postalCodes.First().adminName3,
                    Street = "",
                    Region = region == null ? companyRegion.BaseName : region.BaseName,
                    PostalCode = PostalCode
                };

                var request = new CreateCampaignRequest
                {
                    JobId = job.IdjobVacancy.ToString(),

                    Advertisement = new Advertisement
                    {
                        Branding = ApiUtils.GetBrandBySite(job.Idsite),
                        Uri = BuildURLJobvacancy(job)
                    },

                    JobContent = new JobContent
                    {
                        JobTitle = job.Title,
                        JobDescription = $"{job.Description} \n {job.Requirements}",
                        Language = ApiUtils.GetLanguageBySite(job.Idsite),
                        PublicationTime = Timestamp.FromDateTime(DateTime.UtcNow),

                        HiringOrganization = new HiringOrganization
                        {
                            Name = _enterpriseRepo.GetCompanyName(job.Identerprise),
                            LogoUrl = urlLogo,
                        },
                        Location = new Geolocation
                        {
                            CountryAlpha2 = _countryIsoRepo.GetIsobyCountryId(job.Idcountry), //verificar
                            Latitude = latitude,
                            Longitude = longitude,
                        },
                        Address = address,
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
                var management = await _campaignsManagementRepository.GetCampaignManagement(job.IdjobVacancy);
                var IsUpdate = management?.Status != null && !string.IsNullOrEmpty(ans.CampaignId);
                
                if (IsUpdate)
                {
                    //update
                    management.ExternalCampaignId = ans.CampaignId;
                    management.Status = (int)AimwelStatus.ACTIVE;
                    management.LastModificationDate = DateTime.Now;
                    management.Budget = settings.Budget;
                    management.Goal = settings.Goal;
                    management.ModificationReason = (int)CampaignModificationReason.MANUAL;
                    await _campaignsManagementRepo.Update(management);
                }
                else
                {
                    Guid guidOutput = new Guid();
                    bool isValid = Guid.TryParse(ans.CampaignId, out guidOutput);
                    if (isValid)
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
                }

                return ans;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return new CreateCampaignResponse { CampaignId = msg };
            }
        }

        public async Task<CampaignsManagement> CreateCampaingUpdater(JobVacancy job)
        {
            if (job.Idsite == (int)Sites.ITALY || job.Idsite == (int)Sites.MEXICO)
            {
                return new CampaignsManagement() { };
            }
            CampaignSetting settings;
            try
            {
                settings = await _campaignsManagementRepo.GetCampaignSetting(job);
                if (settings == null)
                {
                    settings = new CampaignSetting();
                    settings.Goal = 100;
                    settings.Budget = 0.000m;
                }
                job.Isco ??= _areaRepository.GetIscoDefaultFromArea(job.Idarea);
                GrpcChannel channel;
                string logo = string.Empty;
                string urlLogo = string.Empty;
                var client = GetClient(out channel);
                double latitude = 0d;
                double longitude = 0d;
                string PostalCode = string.Empty;
                string countryISO = string.Empty;
                var regionId = _enterpriseRepo.GetCompanyRegion(job.Identerprise);
                var companyRegion = _regionRepo.Get(regionId);
                GeoNamesDto geolocation = null;
                var region = _regionRepo.Get(job.Idregion);
                long units = Convert.ToInt64(decimal.Truncate(settings.Budget));
                var inputValue = Math.Round(settings.Budget, 2, MidpointRounding.AwayFromZero);
                int hundredths = settings.Budget == 0.00m ? 0 : ReminderDigits(inputValue, 2);
                var countryName = _countryRepo.GetCountryById(job.Idcountry).BaseName;
                if (job.IdzipCode == null || job.IdzipCode == -1 || job.IdzipCode == 0)
                {
                    if (job.Idcountry == 226)
                    {
                        latitude = Convert.ToDouble("40.60704563565361");
                        longitude = Convert.ToDouble("-4.049663543701172");
                        PostalCode = "28292";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(job.City))
                        {
                            countryISO = _countryIsoRepo.GetIsobyCountryId(job.Idcountry);
                            geolocation = _geoNamesConector.GetPostalCodesCollectionByPlaceName(job.City, countryISO);

                            //if geolocation not GOOGLE will be
                            if (geolocation != null && geolocation.postalCodes.Any())
                            {
                                latitude = geolocation.postalCodes.FirstOrDefault().lat;
                                longitude = geolocation.postalCodes.FirstOrDefault().lng;
                                PostalCode = geolocation.postalCodes.FirstOrDefault().postalCode;
                            }
                            else
                            {
                                countryISO = _countryIsoRepo.GetIsobyCountryId(job.Idcountry);
                                geolocation = _geoNamesConector.GetPostalCodesCollectionByPlaceName(companyRegion.BaseName.ToLower(), countryISO);
                                //if geolocation not GOOGLE will be
                                if (geolocation != null && geolocation.postalCodes.Any())
                                {
                                    latitude = geolocation.postalCodes.FirstOrDefault().lat;
                                    longitude = geolocation.postalCodes.FirstOrDefault().lng;
                                    PostalCode = geolocation.postalCodes.FirstOrDefault().postalCode;
                                }
                            }
                        }
                        else
                        {
                            if (job.Idregion == 60)
                            {
                                if (regionId != 61)
                                {
                                    //TODO get name
                                    countryISO = _countryIsoRepo.GetIsobyCountryId(job.Idcountry);
                                    geolocation = _geoNamesConector.GetPostalCodesCollectionByPlaceName(companyRegion.BaseName.ToLower(), countryISO);
                                    //if geolocation not GOOGLE will be
                                    if (geolocation != null && geolocation.postalCodes.Any())
                                    {
                                        latitude = geolocation.postalCodes.FirstOrDefault().lat;
                                        longitude = geolocation.postalCodes.FirstOrDefault().lng;
                                        PostalCode = geolocation.postalCodes.FirstOrDefault().postalCode;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    var code = _zipCodeRepo.GetZipById((int)job.IdzipCode);
                    if (code != null)
                    {
                        latitude = Convert.ToDouble(code.Latitude);
                        longitude = Convert.ToDouble((code.Longitude));
                        PostalCode = code.Zip;
                    }
                }
                var logoEntity = _logoRepo.GetLogoByBrand(job.Idbrand);
                if (logoEntity == null)
                {
                    urlLogo = "https://www.turijobs.com/static/img/global/nologo.png";
                }
                else
                {
                    logo = logoEntity.UrlImgBig;
                    urlLogo = $"{_config["Aimwel:Portal.urlRootStatics"]}" +
                  $"{"/img/"}" +
                  $"{ApiUtils.GetShortCountryBySite((Sites)job.Idsite)}" +
                  $"{"/logos/"}" +
                  $"{logo}";
                }

                var address = new Address
                {
                    CountryAlpha2 = _countryIsoRepo.GetIsobyCountryId(job.Idcountry),
                    State = region == null ? companyRegion.Ccaa : region.Ccaa == null ? region.BaseName : region.Ccaa,
                    City = job.City ?? geolocation.postalCodes.First().adminName3,
                    Street = "",
                    Region = region == null ? companyRegion.BaseName : region.BaseName,
                    PostalCode = PostalCode
                };

                var request = new CreateCampaignRequest
                {
                    JobId = job.IdjobVacancy.ToString(),

                    Advertisement = new Advertisement
                    {
                        Branding = ApiUtils.GetBrandBySite(job.Idsite),
                        Uri = BuildURLJobvacancy(job)
                    },

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
                            CountryAlpha2 = _countryIsoRepo.GetIsobyCountryId(job.Idcountry), //verificar
                            Latitude = latitude,
                            Longitude = longitude,
                        },
                        Address = address,
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
                    Guid guidOutput = new Guid();
                    bool isValid = Guid.TryParse(ans.CampaignId, out guidOutput);
                    if (isValid)
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

                        return campaign;
                    }
                    else
                    {
                        AimwelCreationError err = new()
                        {
                            Date = DateTime.Now,
                            IdJobVacancy = job.IdjobVacancy,
                            FailedCampaign = JsonConvert.SerializeObject(ans)
                        };
                        _aimwelErrorsRepository.Add(err);
                    }
                }
                else
                {
                    return new CampaignsManagement();
                }

                return new CampaignsManagement();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return new CampaignsManagement();
            }
        }

        public int ReminderDigits(decimal number, int count)
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

            try
            {
                var reply = await client.CreateCampaignAsync(request, metadata);
                _logger.LogInformation("Successfully created campaign: " + reply);
                return reply;
            }
            catch (Exception e)
            {
                CreateCampaignResponse r = new CreateCampaignResponse();
                r.CampaignId = e.Message;
                return r;
            }
        }

        public string BuildURLJobvacancy(JobVacancy _offer)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder tmpSb = new StringBuilder();

            tmpSb.Clear();
            string title = ApiUtils.FormatString(_offer.Title.Trim());
            title = title.EndsWith("-") ? title.Remove(title.Length - 1, 1) : title;

            tmpSb.Append(ApiUtils.GetUriBySite(_offer.Idsite)); //http://www.turijobs.com
            tmpSb.Append(ApiUtils.GetSearchbySite(_offer.Idsite));
            if (_offer.Idregion == 61 || _offer.Idcountry == 226)
                tmpSb.Append(ApiUtils.GetAbroadTerm(_offer.Idsite));
            if (_offer.Idcity != null && _offer.Idcity > 0)
            {
                var cityUrl = _zoneUrl.GetCityUrlByCityId((int)_offer.Idcity);
                if (!string.IsNullOrEmpty(cityUrl))
                {
                    tmpSb.Append(string.Format("-{0}", ApiUtils.FormatString(cityUrl).Trim())); //http://www.turijobs.com/ofertas-trabajo-calella
                }
                else
                {
                    if (_offer.Idsite != (int)Sites.MEXICO)
                    {
                        var regionName = _regionRepo.GetRegionNameByID(_offer.Idregion, true);
                        var ccaa = _regionRepo.GetCCAAByID(_offer.Idregion, true);
                        if (_offer.Idcity == 0)
                        {
                            tmpSb.Append(string.Format("-{0}", StringUtils.FormatString(regionName).Trim())); //http://www.turijobs.com/ofertas-trabajo-cadiz
                            if ((ccaa != "- Todo País" && ccaa != "- Todo Portugal") && ccaa != regionName)
                                tmpSb.Append(string.Format("-{0}", StringUtils.FormatString(ccaa).Trim())); //http://www.turijobs.com/ofertas-trabajo-cadiz-andalucia
                        }
                        else
                            tmpSb.Append(string.Format("-{0}", StringUtils.FormatString(regionName).Trim()));
                    }
                    else // méxico
                    {
                        tmpSb.Append(string.Format("-{0}", StringUtils.FormatString(_offer.City).Trim()));
                    }
                }
            }

            tmpSb.Append(string.Format("/{0}", StringUtils.FormatString(title.ToLower()).Trim())); //http://www.turijobs.com/ofertas-trabajo-cadiz/recepcionista
            tmpSb.Append(string.Format("{0}", StringUtils.FormatString("-of").Trim())); //http://www.turijobs.com/ofertas-trabajo-cadiz/recepcionista-of
            tmpSb.Append(string.Format("{0}", StringUtils.FormatString(_offer.IdjobVacancy.ToString().Trim()))); //http://www.turijobs.com/ofertas-trabajo-cadiz/recepcionista-of76008
            sb.Append(ApiUtils.SanitizeURL(tmpSb).ToString());

            return sb.ToString();
        }
    }
}
