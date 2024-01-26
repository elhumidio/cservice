
namespace Domain.Entities
{
    public class JobTitleDenomination
    {
        public int Id { get; set; }
        public string Denomination { get; set; } = null!;
        public int FkJobTitle { get; set; }
        public int LanguageId { get; set; }
        public bool BaseName { get; set; }
        public bool SiteMap { get; set; }

        public virtual JobTitle FkJobTitleNavigation { get; set; } = null!;
    }
}
