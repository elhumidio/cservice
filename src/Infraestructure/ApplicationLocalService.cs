using Application.Interfaces;
using Domain.DTO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Infraestructure
{
    public class ApplicationLocalService : IApplicationServiceLocal
    {
        private IConfiguration _config;
        public ApplicationLocalService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<GenericOfferCounter> CountApplicantsByOffers(ListOffersRequest jobIds)
        {
            var serviceURL = _config["ApplicationService:CandidateApplication"];
            Uri serviceUri = GetURL(serviceURL, $"CountApplicantsByOffers");
            var json = JsonConvert.SerializeObject(jobIds);
            using var httpClient = new HttpClient();
            using var request = new HttpRequestMessage(new HttpMethod("POST"), serviceUri.ToString());
            request.Headers.TryAddWithoutValidation("Content-Transfer-Encoding", "binary");
            request.Content = new StringContent(json);
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            try
            {
                HttpResponseMessage responseMessage = await httpClient.SendAsync(request);
                var response = await responseMessage.Content.ReadAsStringAsync();
                var responseDTO = JsonConvert.DeserializeObject<List<CountByOffer>>(response);
                GenericOfferCounter obj = new GenericOfferCounter();
                obj.results = responseDTO;
                return obj;
            }
            catch (Exception)
            {
                return new GenericOfferCounter();

            }
        }

        public async Task<GenericOfferCounter> CountRedirectsByOffer(ListOffersRequest jobIds)
        {
            CountOffersRequest obj = new CountOffersRequest();
            obj.JobOfferIds = jobIds.Offers.ToArray();
            var serviceURL = _config["ApplicationService:JobOfferRedirect"];
            Uri serviceUri = GetURL(serviceURL, $"CountRedirectsByOffers");
            var json = JsonConvert.SerializeObject(obj);
            using var httpClient = new HttpClient();
            using var request = new HttpRequestMessage(new HttpMethod("POST"), serviceUri.ToString());
            request.Headers.TryAddWithoutValidation("Content-Transfer-Encoding", "binary");
            request.Content = new StringContent(json);
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            HttpResponseMessage responseMessage = await httpClient.SendAsync(request);
            var response = await responseMessage.Content.ReadAsStringAsync();
            var responseDTO = JsonConvert.DeserializeObject<GenericOfferCounter>(response);
            return responseDTO;
        }

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
