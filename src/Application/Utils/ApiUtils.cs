using Application.JobOffer.Commands;
using Domain.DTO.ManageJobs;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using System.Text;
using System.Text.RegularExpressions;
using TURI.Seoservice.Contracts.Services;

namespace Application.Utils
{
    public interface IApiUtils
    {
        public string BuildURLJobvacancy(OfferModel _offer);

        public string GetRegTypeBySiteAndLanguage(int lang, int type);

        public int GetCVExpiredDays(IsOldOfferArgs pParameters);

        public void UpdateGoogleIndexingURL(OfferModel offer);

        public void DeleteGoogleIndexingURL(OfferModel offer);

        public OfferModel GetOfferModel(CreateOfferCommand offer);

        public OfferModel GetOfferModel(UpdateOfferCommand offer);

        public OfferModel GetOfferModel(JobVacancy offer);
    }

    public class ApiUtils : IApiUtils
    {
        private const int ONBOARD = 226;
        private readonly IRegionRepository _regionRepo;
        private readonly IZoneUrl _zoneUrl;
        private readonly ISeoService _seoService;
        private const string k_UNO_MARZO_DIECIOCHO = "01-03-2018";
        private const int k_ONE_YEAR = 1;
        private const int k_TEN_DAYS = 10;
        private const int k_JOBVACYPE_STANDARD_SIXTY = 6;
        private const int k_THIRTY_DAYS = 30;
        private const int k_NINETY_DAYS = 90;

        public ApiUtils(IRegionRepository regionRepository, IZoneUrl zoneUrl, ISeoService seoService)
        {
            _regionRepo = regionRepository;
            _zoneUrl = zoneUrl;
            _seoService = seoService;
        }

        public int GetCVExpiredDays(IsOldOfferArgs pParameters)
        {
            int CVExpiredDays = 0;
            if (pParameters.ContractStartDate < Convert.ToDateTime(k_UNO_MARZO_DIECIOCHO).Date)
            {
                CVExpiredDays = Convert.ToInt32((System.DateTime.Today - pParameters.OfferPublicationDate.AddDays(pParameters.ExtensionDays).AddYears(k_ONE_YEAR)).TotalDays);
            }
            else
            {
                if (pParameters.OfferIDJobVacType == k_JOBVACYPE_STANDARD_SIXTY)
                {
                    CVExpiredDays = Convert.ToInt32((System.DateTime.Today - pParameters.OfferPublicationDate.AddDays(pParameters.ExtensionDays + pParameters.ProductDuration + k_THIRTY_DAYS)).TotalDays);
                }
                else
                {
                    if (pParameters.OfferCheckPack)
                    {
                        CVExpiredDays = Convert.ToInt32((System.DateTime.Today - pParameters.ContractFinishDate.AddDays(k_THIRTY_DAYS + pParameters.ExtensionDays)).TotalDays);
                    }
                    else
                    {
                        CVExpiredDays = Convert.ToInt32((System.DateTime.Today - pParameters.OfferPublicationDate.AddDays(pParameters.ExtensionDays + k_NINETY_DAYS)).TotalDays);
                    }
                }
            }
            return CVExpiredDays;
        }

        public static bool IsValidEmail(string _email)
        {
            var regex = @"^[a-z0-9!#$%&' * +/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";
            try
            {
                if (string.IsNullOrEmpty(_email))
                    return false;
                var isRightformatted = Regex.IsMatch(_email.ToLower(), regex, RegexOptions.IgnoreCase);
                return isRightformatted;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static Uri GetUriBySite(int siteId)
        {
            Uri Uri = new Uri("https://www.turijobs.com");

            switch (siteId)
            {
                case (int)Sites.SPAIN:
                    Uri = new Uri("https://www.turijobs.com");
                    break;

                case (int)Sites.PORTUGAL:
                    Uri = new Uri("https://www.turijobs.pt");
                    break;

                case (int)Sites.ITALY:
                    Uri = new Uri("https://www.turijobs.it");
                    break;

                case (int)Sites.MEXICO:
                    Uri = new Uri("https://www.turijobs.com.mx");
                    break;

                default:
                    Uri = new Uri("https://www.turijobs.com");
                    break;
            }
            return Uri;
        }

        public string GetRegTypeBySiteAndLanguage(int lang, int type)
        {
            Dictionary<(int, int), string> regTypeMap = new Dictionary<(int, int), string>
                {
                    {(1, 14), "Classic"},
                    {(1, 7), "Clásica"},
                    {(1, 17), "Clássica"},
                    {(1, 15), "Classico"},
                    {(2, 14), "Express"},
                    {(2, 7), "Express"},
                    {(2, 17), "Express"},
                    {(2, 15), "Express"}
                };

            if (regTypeMap.TryGetValue((type, lang), out string result))
            {
                return result;
            }

            return string.Empty;
        }

        public static string GetBrandBySite(int siteId)
        {
            string brand = string.Empty;
            switch (siteId)
            {
                case (int)Sites.SPAIN:
                    brand = "turijobs-spain";
                    break;

                case (int)Sites.PORTUGAL:
                    brand = "turijobs-portugal";
                    break;

                case (int)Sites.ITALY:
                    brand = "turijobs-italy";
                    break;

                case (int)Sites.MEXICO:
                    brand = "turijobs-mexico";
                    break;

                default:
                    brand = "turijobs-spain";
                    break;
            }
            return brand;
        }

        public static int GetTuriLanguageBySite(int siteId)
        {
            switch (siteId)
            {
                case (int)Sites.SPAIN:
                    return 7;

                case (int)Sites.PORTUGAL:
                    return 17;

                case (int)Sites.ITALY:
                    return 15;

                case (int)Sites.MEXICO:
                    return 7;

                default: return 7;
            }
        }

        public static string GetShortCountryBySite(Sites site)
        {
            string country = string.Empty;
            switch (site)
            {
                case Sites.SPAIN:
                    country = "sp";
                    break;

                case Sites.PORTUGAL:
                    country = "pt";
                    break;

                case Sites.ITALY:
                    country = "it";
                    break;

                case Sites.MEXICO:
                    country = "mx";
                    break;
            }
            return country;
        }

        public string BuildURLJobvacancy(OfferModel _offer)
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

        public static int GetCountryIdBySite(int site)
        {
            int country = 40;
            switch (site)
            {
                case 6:
                    country = 40;
                    break;

                case 8:
                    country = 118;
                    break;

                case 39:
                    country = 71;
                    break;

                case 11:
                    country = 98;
                    break;
            }
            return country;
        }

        public static string FormatString(string inputString)
        {
            return FormatString(inputString, "-");
        }

        /// <summary>
        /// Sanitiza una URL.
        /// </summary>
        /// <param name="_cadena">String con la cadena a sanitizar.</param>
        /// <returns>Devuelve un String con la URL generada.</returns>
        public static string SanitizeURL(StringBuilder _cadena)
        {
            _cadena.Replace("--", "-");
            _cadena.Replace("-/", "/");
            _cadena.Replace("//cursos/", "/cursos/");

            // elimina el "-" del final de la cadena, si lo tiene
            string strcad = _cadena.ToString();
            if (strcad.Substring(strcad.Length - 1, 1) == "-")
            {
                strcad = strcad.Substring(0, strcad.Length - 1);
            }

            _cadena.Clear();
            _cadena.Append(strcad);

            return _cadena.ToString();
        }

        public static string FormatString(string inputString, string separator)
        {
            if (string.IsNullOrEmpty(inputString)) return String.Empty;

            StringBuilder sb = new StringBuilder(inputString.Trim().ToLower(), inputString.Trim().Length);

            sb
                .Replace("á", "a")
                .Replace("à", "a")
                .Replace("ä", "a")
                .Replace("â", "a")
                .Replace("ã", "a")
                .Replace("å", "a")
                .Replace("æ", "a")

                .Replace("é", "e")
                .Replace("è", "e")
                .Replace("ë", "e")
                .Replace("ê", "e")
                .Replace("ě", "e")

                .Replace("í", "i")
                .Replace("ì", "i")
                .Replace("ï", "i")
                .Replace("î", "i")
                .Replace("ı", "i")

                .Replace("ó", "o")
                .Replace("ò", "o")
                .Replace("ö", "o")
                .Replace("ô", "o")
                .Replace("õ", "o")
                .Replace("ø", "o")

                .Replace("ú", "u")
                .Replace("ù", "u")
                .Replace("ü", "u")
                .Replace("û", "u")
                .Replace("ů", "u")

                .Replace("ç", "c")
                .Replace("ć", "c")
                .Replace("č", "c")

                .Replace("ğ", "g")

                .Replace("ş", "s")
                .Replace("š", "s")

                .Replace("ž", "z")

                .Replace("ý", "y")

                .Replace("ð", "d")
                .Replace("ď", "d")

                .Replace("þ", "th")

                .Replace("/", "-")
                .Replace("'", "")
                .Replace("-&-", "-")
                .Replace("[", "-")
                .Replace("]", "-")
                .Replace("–", "-")
                .Replace("\"", "")
                .Replace("$", "")
                .Replace("¨", "")
                .Replace("*", "")
                .Replace("#", "")
                .Replace("ñ", "n")
                .Replace("ç", "c")
                .Replace(".", "-")
                .Replace("%", "")
                .Replace("?", "")
                .Replace("¿", "")
                .Replace("&", "")
                .Replace(":", "")
                .Replace("+", "")
                .Replace(",", "")
                .Replace("^", "-")
                .Replace("“", "")
                .Replace("”", "")
                .Replace("\\", "-")
                .Replace("(", "-")
                .Replace(")", "-")
                .Replace("¡", "")
                .Replace("!", "")
                .Replace("|", "")
                .Replace("º", "")
                .Replace("ª", "")
                .Replace("´", "")
                .Replace("`", "")
                .Replace(" - ", "-")
                .Replace("---", "-")
                .Replace("--", "-")
                .Replace("’", "")
                .Replace(" ", separator);

            sb.Replace("--", "-")
              .Replace("--", "-");

            return sb.ToString();
        }

        public static string GetSearchbySite(int site)
        {
            string domain = string.Empty;

            switch (site)
            {
                case 6:
                    domain = "ofertas-trabajo";
                    break;

                case 8:
                    domain = "anuncios-emprego";
                    break;

                case 11:
                    domain = "ofertas-trabajo";
                    break;

                case 39:
                    domain = "offerte-lavoro";
                    break;
            }

            return domain;
        }

        public static string GetAbroadTerm(int _idsite)
        {
            string url = string.Empty;
            switch (_idsite)
            {
                case 8:
                    url = string.Format("/{0}", "anuncios-emprego-estrangeiro");
                    break;

                case 6:
                case 11:
                    url = string.Format("/{0}", "ofertas-trabajo-extranjero");
                    break;

                case 39:
                    url = string.Format("/{0}", "offerte-lavoro-allestero");
                    break;
            }
            return url;
        }

        public OfferModel GetOfferModel(CreateOfferCommand offer)
        {
            return new OfferModel()
            {
                IdjobVacancy = offer.IdjobVacancy,
                Title = offer.Title,
                Idsite = offer.Idsite,
                Idcountry = offer.Idcountry,
                Idregion = offer.Idregion,
                Idcity = offer.Idcity.GetValueOrDefault()
            };
        }

        public OfferModel GetOfferModel(UpdateOfferCommand offer)
        {
            return new OfferModel()
            {
                IdjobVacancy = offer.IdjobVacancy,
                Title = offer.Title,
                Idsite = offer.Idsite,
                Idcountry = offer.Idcountry,
                Idregion = offer.Idregion,
                Idcity = offer.Idcity.GetValueOrDefault()
            };
        }

        public OfferModel GetOfferModel(JobVacancy offer)
        {
            return new OfferModel()
            {
                IdjobVacancy = offer.IdjobVacancy,
                Title = offer.Title,
                Idsite = offer.Idsite,
                Idcountry = offer.Idcountry,
                Idregion = offer.Idregion,
                Idcity = offer.Idcity.GetValueOrDefault()
            };
        }

        /// <summary>
        /// Call Google API Indexing for create/update URL index.
        /// </summary>
        /// <param name="offer"></param>
        public void UpdateGoogleIndexingURL(OfferModel offer)
        {
            try
            {
                string offerURL = BuildURLJobvacancy(offer);

                if (!string.IsNullOrEmpty(offerURL))
                {
                    _seoService.UpdateGoogleIndexingURL(offerURL);
                }
            }
            catch { }
        }

        /// <summary>
        /// Call Google API Indexing for delete URL index.
        /// </summary>
        /// <param name="offer"></param>
        public void DeleteGoogleIndexingURL(OfferModel offer)
        {
            try
            {
                string offerURL = BuildURLJobvacancy(offer);

                if (!string.IsNullOrEmpty(offerURL))
                {
                    _seoService.DeleteGoogleIndexingURL(offerURL);
                }
            }
            catch { }
        }
    }
}
