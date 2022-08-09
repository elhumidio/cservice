using Application.DTO.GeoNames;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

namespace Infraestructure.Integrations
{
    public class GeoNamesConector : IGeoNamesConector
    {
        private IConfigurationSection _geoNameConfiguration;
        private string _geoNameUrl;

        public GeoNamesConector()
        {

            _geoNameConfiguration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("GeoNames");
            _geoNameUrl = _geoNameConfiguration.GetValue<string>("BaseUrl");
        }
        public GeoNamesDTO GetPostalCodesCollection(string postalCode, string country)
        {
            var geoNames = new GeoNamesDTO();
            string url = $"{_geoNameUrl}?postalcode={postalCode}&country={country}&maxRows=100&username=turijobs";

            using (var httpClient = new HttpClient())
            {
                geoNames = JsonConvert.DeserializeObject<GeoNamesDTO>(httpClient.GetStringAsync(new Uri(url)).Result);

            }

            return geoNames;
        }


    }

}
