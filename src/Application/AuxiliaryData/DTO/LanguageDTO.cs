namespace Application.AuxiliaryData.DTO
{
    public partial class LanguageDTO
    {
        public int IdsTurijobsLang { get; set; }
        public int IdSite { get; set; }
        public string LangName { get; set; } = null!;
        public int? sysMsgLangId { get; set; }
    }
}
