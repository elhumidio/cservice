namespace Domain.DTO
{
    public class JobTitleDto
    {
        public int Id { get; set; }
        public string Isco08 { get; set; } = null!;
        public string Isco88 { get; set; } = null!;
        public string? Description { get; set; }
        public bool Active { get; set; }
        public int? FkTranslationDescription { get; set; }
        public List<JobTitleAreasDto> JobTitlesAreas { get; set; }
    }
}
