using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("JobTitles")]
    public partial class JobTitle
    {
        public JobTitle()
        {
            JobTitlesAreas = new HashSet<JobTitleArea>();
            JobTitlesDenominations = new HashSet<JobTitleDenomination>();
        }

        public int Id { get; set; }
        public string Isco08 { get; set; } = null!;
        public string Isco88 { get; set; } = null!;
        public string? Description { get; set; }
        public bool Active { get; set; }
        public int? FkTranslationDescription { get; set; }

        public virtual ICollection<JobTitleArea> JobTitlesAreas { get; set; }
        public virtual ICollection<JobTitleDenomination> JobTitlesDenominations { get; set; }
    }
}
