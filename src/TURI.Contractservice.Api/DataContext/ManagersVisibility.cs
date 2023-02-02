using System;
using System.Collections.Generic;

namespace TURI.Contractservice.DataContext
{
    public partial class ManagersVisibility
    {
        public int EnterpriseUserId { get; set; }
        public int ContractId { get; set; }
        public bool? IsVisible { get; set; }
        public int Id { get; set; }
    }
}
