using Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infraestructure
{
    public class InternalService : IinternalService
    {
        private IConfiguration _config;

        public InternalService(IConfiguration configuration) {

            _config = configuration;
        }
        /*public async Task<GoogleLocation> GetGooglelocationByPlace(string place, string country)
        {
            var serviceURL = _config["InternalService:GetLocationByPlace"];
            Uri serviceUri = GetURL(serviceURL, $"GetGoogleLocationByPlace");
            var result = await RestClient.Post<ListOffersRequest, GenericOfferCounter>(serviceUri.AbsoluteUri, jobIds);
            throw new NotImplementedException();
        }*/


        private Uri GetURL(string serviceUrl, string methodName)
        {
            if (serviceUrl == null)
                throw new ArgumentException("ApplicationService app setting is empty");

            Uri baseUri = new Uri(serviceUrl);
            Uri methodUri = new Uri(baseUri, methodName);
            return methodUri;
        }
    }
}
