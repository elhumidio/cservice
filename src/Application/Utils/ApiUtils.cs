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
    }
}
