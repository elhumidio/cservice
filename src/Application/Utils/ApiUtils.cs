using Domain.Enums;
using DPGRecruitmentCampaignClient;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;


namespace Application.Utils
{
    public class ApiUtils
    {
        
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
        public static Uri GetUriBySite(int siteId) {

            Uri Uri = new Uri("https://www.turijobs.com");
            
            switch (siteId)
            {
                case (int)Sites.SPAIN:
                    Uri = new Uri("https://www.turijobs.com");
                    break;
                case (int)Sites.PORTUGAL:
                    Uri = new Uri("https://www.turijobs.pt") ;
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

        public static string GetBrandBySite(int siteId)
        {
            string brand = string.Empty;
            switch (siteId)
            {
                case (int)Sites.SPAIN:
                    brand = "turijobs-spain";
                    break;
                case (int)Sites.PORTUGAL:
                    brand  = "turijobs-portugal";
                    break;
                case (int)Sites.ITALY:
                    brand =  "turijobs-italy";
                    break;  

                case (int)Sites.MEXICO:
                    brand =  "turijobs-mexico";
                    break;
                default:
                    brand = "turijobs-spain";
                    break;
            }
            return brand;

        }

        public static Language GetLanguageBySite(int siteId)
        {

            switch (siteId) {
                case (int)Sites.SPAIN:
                    return Language.EsEs;
                    
                    case (int)Sites.PORTUGAL:
                    return Language.PtPt;
                    
                case (int)Sites.ITALY:
                    return Language.ItCh;
                    
                case (int)Sites.MEXICO:
                    return Language.EsMx;
                    default:return Language.EsEs;
            }

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
    }
}
