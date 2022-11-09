using Domain.Enums;
using DPGRecruitmentCampaignClient;
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
    }
}
