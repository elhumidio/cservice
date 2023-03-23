using System;
using System.Collections.Generic;

namespace TURI.Contractservice.DataContext
{
    public partial class AimwelCreationError
    {
        public int Id { get; set; }
        public int? IdJobVacancy { get; set; }
        public DateTime? Date { get; set; }
        public string? FailedCampaign { get; set; }
    }
}
