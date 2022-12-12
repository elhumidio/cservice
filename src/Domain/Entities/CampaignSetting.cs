using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CampaignSetting
    {
        public int Id { get; set; }
        public int AreaId { get; set; }
        public int RegionId { get; set; }
        public decimal Budget { get; set; }
        public int? Goal { get; set; }
        public int? Isco08 { get; set; }
        public int? Isco88 { get; set; }
    }
}
