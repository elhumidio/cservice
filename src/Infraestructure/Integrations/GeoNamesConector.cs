using Application.DTO.GeoNames;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

namespace Infraestructure.Integrations
{
    public class GeoNamesConector : IGeoNamesConector
    {
        private string _geoNamesUrl;

        public GeoNamesConector(IConfiguration configuration)
        {
            _geoNamesUrl = configuration["GeoNames:BaseUrl"].ToString();
        }

        public GeoNamesDto GetPostalCodesCollection(string postalCode, string country)
        {
            GeoNamesDto geoNames = new();
            string url = $"{_geoNamesUrl}?postalcode={postalCode}&country={country}&maxRows=100&username=turijobs";
            try {
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromMilliseconds(1000);
                    var response = httpClient.GetStringAsync(new Uri(url));
                    var res = response.Result;
                    if (res != null)
                    {
                        geoNames = JsonConvert.DeserializeObject<GeoNamesDto>(response.Result);
                    }
                }
                return geoNames;
            }
            catch{
                return geoNames;
            }
        }

        public GeoNamesDto GetPostalCodesCollectionByPlaceName(string placename, string country)
        {
            GeoNamesDto geoNames = new();
            string url = $"{_geoNamesUrl}?placename={placename}&country={country}&maxRows=100&username=turijobs";

            using (var httpClient = new HttpClient())
            {
                geoNames = JsonConvert.DeserializeObject<GeoNamesDto>(httpClient.GetStringAsync(new Uri(url)).Result);
            }

            return geoNames;
        }
    }
}
