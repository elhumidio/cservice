using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TJobVacancyLanguage")]
    public partial class JobVacancyLanguage
    {
        public int IdjobVacancy { get; set; }
        public int Idlanguage { get; set; }
        public int IdlangLevel { get; set; }
    }
}
