using Newtonsoft.Json;

namespace Domain.Entities
{
    public partial class RegEnterpriseConsum
    {
        public int Identerprise { get; set; }
        public int Idcontract { get; set; }
        public int Idproduct { get; set; }
        public int Units { get; set; }
        public int UnitsUsed { get; set; }
        [JsonIgnore]
        public virtual Contract IdcontractNavigation { get; set; } = null!;
    }
}
