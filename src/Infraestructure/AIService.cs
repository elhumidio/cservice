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

        public string DoGPTRequest(string prompt, string data)
        {
            var serviceURL = _config["ExternalServices:AIService"];
            Uri serviceUri = GetURL(serviceURL, $"api/ChatGPT/DoGPTRequest");
            var rawPrompt = new StreamReader(File.OpenRead("rawPrompt.txt")).ReadToEnd();
            var args = new List<string> { prompt };

            var body = string.Format(rawPrompt, args.ToArray());
            return RestClient.Post<SendGPTRequest, string>(serviceUri.AbsoluteUri, new SendGPTRequest(body, data)).Result;
            throw new NotImplementedException();
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
