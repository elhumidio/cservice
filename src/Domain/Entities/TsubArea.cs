namespace Domain.Entities
{
    public partial class TsubArea
    {
        public int IdsubArea { get; set; }
        public int Idarea { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public string BaseName { get; set; } = null!;

        public virtual Area Id { get; set; } = null!;
    }
}
