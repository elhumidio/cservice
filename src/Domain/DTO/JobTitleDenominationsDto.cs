
namespace Domain.DTO
{
    public class JobTitleDenominationsDto
    {
        public int Id { get; set; }
        public string Denomination { get; set; } = null!;
        public int FkJobTitle { get; set; }
        public int LanguageId { get; set; }
        public bool BaseName { get; set; }
        public string Isco08 { get; set; } = null!;
        public string Isco88 { get; set; } = null!;

        public List<int>? JobTitlesAreas { get; set; }

    }
}
