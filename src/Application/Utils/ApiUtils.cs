using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using DPGRecruitmentCampaignClient;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Text;
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
 
    }
}
