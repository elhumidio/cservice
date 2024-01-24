
namespace Domain.Entities
{
    public partial class TranslationsWeb
    {
        public TranslationsWeb()
        {
            JobTitles = new HashSet<JobTitle>();
            JobTitlesDenominations = new HashSet<JobTitleDenomination>();
        }

        public string? TextName { get; set; }
        public string? TextScope { get; set; }
        public string? English { get; set; }
        public string? Spanish { get; set; }
        public string? Italian { get; set; }
        public string? Portuguese { get; set; }
        public int Id { get; set; }
        public virtual ICollection<Benefit> Benefits { get; set; }

        public virtual ICollection<JobTitle> JobTitles { get; set; }
        public virtual ICollection<JobTitleDenomination> JobTitlesDenominations { get; set; }
    }
}
