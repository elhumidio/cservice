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

    }
}
