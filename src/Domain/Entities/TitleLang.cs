using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TitleLangs")]
    public partial class TitleLang
    {
        public int TitleId { get; set; }
        public int? LanguageId { get; set; }
        public string? Label { get; set; }
        public int Id { get; set; }

        public virtual Title Title { get; set; } = null!;
    }
}
