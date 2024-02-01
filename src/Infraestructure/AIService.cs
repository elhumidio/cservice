using ApisClient.DTO;
using Application.Core;
using Application.Interfaces;
using Application.Utils;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Infraestructure
{
    public class AIService : IAIService
    {

        private IConfiguration _config;

        public AIService(IConfiguration config)
        {
            _config = config;
        }

        public string DoGPTRequestDynamic(string denominationList, string title)
        {
            var serviceURL = _config["ExternalServices:AIService"];
            Uri serviceUri = GetURL(serviceURL, $"api/ChatGPT/DoGPTDynamicRequest");
            using (var reader = new StreamReader(File.OpenRead("rawPrompt.txt")))
            {
                var rawPrompt = reader.ReadToEnd();
                var modifiedPrompt = rawPrompt.Replace("|0|", '"' + title + '"').Replace("|1|", "\"" + denominationList + "\"");
                
                return RestClient.Post<string, string>(serviceUri.AbsoluteUri, modifiedPrompt).Result;
            };
        }

        private Uri GetURL(string serviceUrl, string methodName)
        {
            if (serviceUrl == null)
                throw new ArgumentException("AIService app setting is empty");

            Uri baseUri = new Uri(serviceUrl);
            Uri methodUri = new Uri(baseUri, methodName);
            return methodUri;
        }
    }


    public class SendGPTRequest : IRequest<Result<string>>
    {
        public string Prompt { get; set; }
        public string Data { get; set; }

        public SendGPTRequest(string prompt, string data)
        {
            Prompt = prompt;
            Data = data;
        }
    }
}
