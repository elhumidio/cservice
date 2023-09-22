namespace Domain.Entities
{
    public partial class Culture
    {
        public int Id { get; set; }
        public string CultureName { get; set; } = null!;
        public string Language { get; set; } = null!;
        public string? CultureCode { get; set; }
        public string? Iso639xValue { get; set; }
        public string? ShortName { get; set; }
        public int? Idslanguage { get; set; }
        public int? Idcountry { get; set; }
        public string? Path { get; set; }
        public string? UrlBase { get; set; }
        public string? Cursos { get; set; }
        public string? Actualidad { get; set; }
        public string? JobVacancyDirName { get; set; }
    }
}
