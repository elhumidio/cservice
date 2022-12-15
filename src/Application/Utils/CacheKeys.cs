using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public  class CacheKeys
    {
        private static string _titles;
        public static string Brands => "_brands";
        public static string Titles {
            get { return _titles; }
            set { _titles = $"_titles{value}"; }
        }
    }
}
