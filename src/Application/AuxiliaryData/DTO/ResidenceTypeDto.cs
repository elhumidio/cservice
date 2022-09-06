namespace Application.AuxiliaryData.DTO
{
    public partial class ResidenceTypeDTO
    {
        public int IdresidenceType { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public string BaseName { get; set; } = null!;
    }
}
