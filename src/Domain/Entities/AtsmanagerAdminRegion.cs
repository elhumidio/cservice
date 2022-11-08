using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class AtsmanagerAdminRegion
    {
        public int ManagerId { get; set; }
        public int? RegionId { get; set; }
        public int? CountryId { get; set; }

        public virtual AtsmanagerAdmin Manager { get; set; } = null!;
    }
}
