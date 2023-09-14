using Application.JobOffer.DTO;
using Application.Utils;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Text;
using TURI.SearchService.Contracts.Search.Services;

namespace Application.JobOffer.Commands
{
    public class WP_GetRelatedOffersByCategory
    {
        private const int ONBOARD = 226;
        public class Command : IRequest<List<WP_Offer>>
        {
            public string CategoryId { get; set; }
            public int SiteId { get; set; }
            public int NumOffers { get; set; }
        }

        public class Handler : IRequestHandler<Command, List<WP_Offer>>
        {
            private readonly IConfiguration _config;
            private readonly ISearchService _searchService;
            private readonly IJobOfferRepository _offerRepo;
            private readonly IRegionRepository _regionRepo;
            private readonly ICountryRepository _countryRepo;
            private readonly IEnterpriseRepository _enterpriseRepo;
            private readonly IlogoRepository _logoRepo;
            private readonly IZoneUrl _zoneUrlRepo;
            private readonly IWP_CategoryOfferRelationRepository _relationsRepo;

            public Handler(IConfiguration config,
                           ISearchService searchService,
                           IJobOfferRepository offerRepo,
                           IRegionRepository regionRepo,
                           ICountryRepository countryRepo,
                           IEnterpriseRepository enterpriseRepo,
                           IlogoRepository logoRepo,
                           IZoneUrl zoneUrlRepo,
                           IWP_CategoryOfferRelationRepository relationsRepo)
            {
                _config = config;
                _searchService = searchService;
                _offerRepo = offerRepo;
                _regionRepo = regionRepo;
                _countryRepo = countryRepo;
                _enterpriseRepo = enterpriseRepo;
                _logoRepo = logoRepo;
                _zoneUrlRepo = zoneUrlRepo;
                _relationsRepo = relationsRepo;
            }

            public async Task<List<WP_Offer>> Handle(Command request, CancellationToken cancellationToken)
            {
                int areaId = GetRelatedAreaByCategory(request.CategoryId);
                var jobs = _offerRepo.WP_GetOffersRelatedByCategory(areaId, request.SiteId, request.NumOffers);
                if (jobs.Count == 0)
                {
                    return new List<WP_Offer>();
                }
                List<WP_Offer> response = new List<WP_Offer>();
                foreach (var job in jobs)
                {
                    WP_Offer offer = new WP_Offer();
                    offer.Id = job.IdjobVacancy;
                    int dayDiff = (DateTime.Now - job.PublicationDate).Days;
                    int hourDiff = (DateTime.Now - job.PublicationDate).Hours;
                    if (dayDiff < 1)
                    {
                        offer.DateDiff = $"Hace {hourDiff} horas";
                    }
                    else
                    {
                        offer.DateDiff = $"Hace {dayDiff} {(dayDiff == 1 ? "día" : "días")}";
                    }
                    string region = _regionRepo.GetRegionNameByID(job.Idregion, false);
                    string country = _countryRepo.GetCountryNameById(job.Idcountry);
                    offer.Location = $"{region}, {country}";
                    offer.Title = job.Title;
                    offer.EnterpriseName = _enterpriseRepo.GetCompanyNameCheckingBlind(job.Identerprise, job.ChkBlindVac);
                    string logo = _logoRepo.GetLogoURLByBrand(job.Idbrand);
                    if (job.ChkBlindVac || string.IsNullOrEmpty(logo))
                    {
                        offer.ImageURL = "https://www.turijobs.com/static/img/global/nologo.png";
                    }
                    else
                    {
                        offer.ImageURL = GetEnterpriseLogoURL(logo, request.SiteId);
                    }
                    if (string.IsNullOrEmpty(job.ExternalUrl))
                    {
                        offer.URL = BuildURLJobvacancy(job);
                    }
                    else
                    {
                        offer.URL = job.ExternalUrl;
                    }
                    List<string> tags = await _searchService.GetOfferTags(new TURI.SearchService.Contracts.Search.Models.SearchOfferTagsModel()
                    {
                        OfferId = job.IdjobVacancy,
                        LanguageId = 7,
                        SiteId = request.SiteId
                    });
                    if (tags.Any())
                    {
                        offer.Tags = tags.Select(x => x.Trim()).ToList();
                    }
                    response.Add(offer);
                }
                return response;
            }

            private int GetRelatedAreaByCategory(string categoryId)
            {
                return _relationsRepo.GetRelationIdByCategoryId(categoryId);
            }

            private string GetEnterpriseLogoURL(string logoURL, int siteId)
            {
                if (logoURL.Contains("static"))
                {
                    return logoURL;
                }
                return $"{_config["Aimwel:Portal.urlRootStatics"]}" +
                       $"{"/img/"}" +
                       $"{ApiUtils.GetShortCountryBySite((Sites)siteId)}" +
                       $"{"/logos/"}" +
                       $"{logoURL}";
            }

            private string BuildURLJobvacancy(JobVacancy _offer)
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder tmpSb = new StringBuilder();

                tmpSb.Clear();
                string title = ApiUtils.FormatString(_offer.Title.Trim());
                title = title.EndsWith("-") ? title.Remove(title.Length - 1, 1) : title;

                tmpSb.Append(ApiUtils.GetUriBySite(_offer.Idsite)); //http://www.turijobs.com
                tmpSb.Append(ApiUtils.GetSearchbySite(_offer.Idsite));
                if (_offer.Idregion == 61 || _offer.Idcountry == ONBOARD)
                    tmpSb.Append(ApiUtils.GetAbroadTerm(_offer.Idsite));
                if (_offer.Idcity != null && _offer.Idcity > 0)
                {
                    var cityUrl = _zoneUrlRepo.GetCityUrlByCityId((int)_offer.Idcity);
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
}
