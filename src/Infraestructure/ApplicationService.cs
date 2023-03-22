using Application.Interfaces;
using Application.Utils;
using Domain.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure
{
    public class ApplicationService : IApplicationService
    {
        private IConfiguration _config;
        public ApplicationService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<GenericOfferCounter> CountApplicantsByOffers(ListOffersRequest jobIds)
        {
   
            var serviceURL = _config["ApplicationService:CandidateApplication"];
            Uri serviceUri = GetURL(serviceURL, $"CountApplicantsByOffers");
            var result = await RestClient.Post<ListOffersRequest,GenericOfferCounter>(serviceUri.AbsoluteUri,jobIds);
            return result;
        }

        public async Task<GenericOfferCounter> CountRedirectsByOffer(ListOffersRequest jobIds)
        {
            var serviceURL = _config["ApplicationService:JobOfferRedirect"];
            Uri serviceUri = GetURL(serviceURL, $"CountRedirectsByOffers");
            var result = await RestClient.Post<ListOffersRequest, GenericOfferCounter>(serviceUri.AbsoluteUri, jobIds);
            return result;
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
