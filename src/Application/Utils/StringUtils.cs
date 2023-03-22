using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public static class StringUtils
    {
        public static string EmptyIfNull(string s)
        {
            string r = string.Empty;
            if (!string.IsNullOrEmpty(s))
            {
                r = s;
            }
            return r;
        }

        public static int EmptyIfNull(int? i)
        {
            int r = int.MinValue;
            if (i != null)
            {
                r = (int)i;
            }
            return r;
        }

        public static string FormatString(string inputString)
        {
            return FormatString(inputString, "-");
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
                .Replace("\u009B", "")
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
                .Replace("  "," ")
                .Replace(" ", separator);

            sb.Replace("--", "-")
              .Replace("--", "-");

            return sb.ToString();
        }


        // Recoge una fecha en formato DateTime y devuelve un string en formato YYYYMMDD
        public static string GetDateYYYYMMDD(DateTime _date)
        {
            return _date.ToString("s").Replace("-", "").Substring(0, 8);
        }

        /// <summary>
        /// Transform a string (eg: city name, company name) in a url friendly value
        /// </summary>
        /// <param name="_field"></param>
        /// <returns></returns>
        public static string GenerateUrlFriendlyFromName(string _field)
        {
            char[] original_character_a = { 'á', 'à', 'ä', 'â', 'ã', 'å', 'æ' };
            char[] original_character_e = { 'æ', 'é', 'è', 'ë', 'ê', 'ě' };
            char[] original_character_i = { 'í', 'ì', 'ï', 'î', 'ı' };
            char[] original_character_o = { 'ó', 'ò', 'ö', 'ô', 'õ', 'ø' };
            char[] original_character_u = { 'ú', 'ù', 'ü', 'û', 'ů' };
            char[] original_character_c = { 'ç', 'ć', 'č' };
            char[] original_character_s = { 'ş', 'š' };
            char[] original_character_d = { 'ð', 'ď' };
            char[] original_character_g = { 'ğ' };
            char[] original_character_z = { 'ž' };
            char[] original_character_y = { 'ý' };
            char[] original_character_th = { 'þ' };

            _field = ReplaceFromArray(original_character_a, 'a'.ToString(), _field);
            _field = ReplaceFromArray(original_character_e, 'e'.ToString(), _field);
            _field = ReplaceFromArray(original_character_i, 'i'.ToString(), _field);
            _field = ReplaceFromArray(original_character_o, 'o'.ToString(), _field);
            _field = ReplaceFromArray(original_character_u, 'u'.ToString(), _field);
            _field = ReplaceFromArray(original_character_c, 'c'.ToString(), _field);
            _field = ReplaceFromArray(original_character_s, 's'.ToString(), _field);
            _field = ReplaceFromArray(original_character_d, 'd'.ToString(), _field);
            _field = ReplaceFromArray(original_character_g, 'g'.ToString(), _field);
            _field = ReplaceFromArray(original_character_z, 'z'.ToString(), _field);
            _field = ReplaceFromArray(original_character_y, 'y'.ToString(), _field);
            _field = ReplaceFromArray(original_character_th, "th".ToString(), _field);

            string cleaned = _field.Replace(' ', '-')
                .Replace('/', '-')
                .Replace("'", "")
                .Replace("-&-", "-")
                .Replace('[', '-')
                .Replace(']', '-')
                .Replace('–', '-')
                .Replace("\"", string.Empty)
                .Replace("$", string.Empty)
                .Replace("¨", string.Empty)
                .Replace("*", string.Empty)
                .Replace("#", string.Empty)
                .Replace('ñ', 'n')
                .Replace('ç', 'c')
                .Replace('.', '-')
                .Replace("%", string.Empty)
                .Replace("?", string.Empty)
                .Replace("¿", string.Empty)
                .Replace("&", string.Empty)
                .Replace(":", string.Empty)
                .Replace("+", string.Empty)
                .Replace(",", string.Empty)
                .Replace('^', '-')
                .Replace("“", string.Empty)
                .Replace("”", string.Empty)
                .Replace("\\", "-")
                .Replace(", ", "-")
                .Replace(')', '-')
                .Replace('(', '-')
                .Replace("¡", string.Empty)
                .Replace("!", string.Empty)
                .Replace("|", string.Empty)
                .Replace("---", "-")
                .Replace("--", "-")
                .Replace("ª", string.Empty)
                .Replace("º", string.Empty)
                .Replace("´", string.Empty)
                .Replace("`", string.Empty);
            return cleaned;
        }

        private static string ReplaceFromArray(char[] _wrongs, string replacecharacter, string _word)
        {
            string cleanedWord = string.Empty;

            foreach (var character in _word)
            {
                if (_wrongs.Any(a => a == character))
                {
                    cleanedWord = _word.Replace(character.ToString(), replacecharacter.ToString());
                }
            }
            return string.IsNullOrEmpty(cleanedWord) ? _word : cleanedWord;
        }

        public static string Combine(this string uri1, string uri2) => $"{uri1.TrimEnd('/')}/{uri2.TrimStart('/')}";
    }
}
