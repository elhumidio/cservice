using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CampaignsUpdatingCheck
    {
        public int Id { get; set; }
        public string? Campaign { get; set; }
        public bool? NeedsRefresh { get; set; }
    }
}
