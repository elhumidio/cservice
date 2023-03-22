using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CampaignsManagement
    {
        public int Id { get; set; }
        public string? ExternalCampaignId { get; set; }
        public int? IdjobVacancy { get; set; }
        public int? Status { get; set; }
        public DateTime? LastModificationDate { get; set; }
        public int? ModificationReason { get; set; }
        public decimal? Budget { get; set; }
        public int? Goal { get; set; }
        public string? Provider { get; set; }
        
    }
}
