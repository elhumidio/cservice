namespace Application.Utils
{
    public static class Extensions
    {
        public static string NumberString(this Enum enVal)
        {
            return enVal.ToString("D");
        }
        public static string CapitalizeStringUpperFirstLowerRest(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            char[] array = s.ToLower().ToCharArray();
            array[0] = char.ToUpper(array[0]);
            return new string(array);
        }
    }
}
