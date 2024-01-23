
namespace Domain.Entities
{
    public class JobTitleDenomination
    {
        public int ID { get; set; }
        public string Denomination {  get; set; }
        public int FK_JobTitle { get; set; }
        public int LanguageId { get; set; }
        public bool BaseName { get; set; }
        public bool SiteMap { get; set; }
    }
}
