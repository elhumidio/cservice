using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class AimwelCreationError
    {
        public int Id { get; set; }
        public int? IdJobVacancy { get; set; }
        public DateTime? Date { get; set; }
        public string? FailedCampaign { get; set; }
    }
}
