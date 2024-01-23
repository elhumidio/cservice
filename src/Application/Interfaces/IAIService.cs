using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAIService
    {
        public string DoGPTRequest(string prompt, string data);
    }
}
