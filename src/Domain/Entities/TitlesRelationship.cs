using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class TitlesRelationship
    {
        public int Id { get; set; }
        public int? JobTitleId { get; set; }
        public int? JobTitleEquivalentId { get; set; }
        public int? Weight { get; set; }
    }
}
