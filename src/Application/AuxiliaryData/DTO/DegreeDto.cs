namespace Application.AuxiliaryData.DTO
{
    public partial class DegreeDTO
    {
        public int Iddegree { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public string BaseName { get; set; } = null!;
        public string? ShortName { get; set; }
        public byte? Ordre { get; set; }
        public int? Weight { get; set; }
    }
}
