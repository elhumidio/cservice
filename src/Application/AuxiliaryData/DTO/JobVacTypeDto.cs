namespace Application.AuxiliaryData.DTO
{
    public partial class JobVacTypeDTO
    {
        public int IdjobVacType { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public string BaseName { get; set; } = null!;
        public int DaysOutstanding { get; set; }
    }
}
