using ApisClient.DTO;
using Application.Interfaces;
using Application.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure
{
    public class QuestService : IQuestService
    {
        private IConfiguration _config;
        public QuestService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<int> CreateQuest(QuestDTO questDTO)
        {
            var serviceURL = _config["QuestService:BaseURL"];
            Uri serviceUri = GetURL(serviceURL, $"CreateCuestionnaire");
            return await RestClient.Post<QuestDTO, int>(serviceUri.AbsoluteUri, questDTO);
        }

        private Uri GetURL(string serviceUrl, string methodName)
        {
            if (serviceUrl == null)
                throw new ArgumentException("QuestService app setting is empty");

            Uri baseUri = new Uri(serviceUrl);
            Uri methodUri = new Uri(baseUri, methodName);
            return methodUri;
        }
    }
}
