using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

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
            Uri serviceUri = GetURL(serviceURL, $"api/ChatGPT/DoGPTJobTitleFromAreaRequest");
            try
            {
                using (var reader = new StreamReader(File.OpenRead("rawPrompt.txt")))
                {
                    var rawPrompt = reader.ReadToEnd();
                    var modifiedPrompt = rawPrompt.Replace("|0|", '"' + title + '"').Replace("|1|", "\"" + denominationList + "\"");

                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var response = client.PostAsync(serviceUri.AbsoluteUri, new StringContent(modifiedPrompt, Encoding.UTF8, "application/json")).Result;
                        string strResponse = response.Content.ReadAsStringAsync().Result;
                        var responseObj = JsonConvert.DeserializeObject<ChatGPTResponseObject>(strResponse);
                        return responseObj?.results[0] ?? "-1";
                    }
                };
            }
            catch
            {
                return "-1";
            }            
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

    public class ChatGPTResponseObject
    {
        public string generationId { get; set; }
        public string prompt { get; set; }
        public string[] results { get; set; }
    }
}
