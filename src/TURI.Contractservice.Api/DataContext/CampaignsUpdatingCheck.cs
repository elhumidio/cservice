using System;
using System.Collections.Generic;

namespace TURI.Contractservice.DataContext
{
    public partial class CampaignsUpdatingCheck
    {
        public int Id { get; set; }
        public string? Campaign { get; set; }
        public bool? NeedsRefresh { get; set; }
    }
}
