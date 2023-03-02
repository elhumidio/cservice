namespace Application.Utils
{
    public  class CacheKeys
    {        public static string ActiveJobsByActiveDays => "_ActiveJobsByActiveDays";

        private static string _titles;
        public static string Brands => "_brands";
        public static string Titles {
            get { return _titles; }
            set { _titles = $"_titles{value}"; }
        }
    }
}
