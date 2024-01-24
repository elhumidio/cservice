namespace Domain.Entities
{
    public partial class Benefit
    {
        public Benefit()
        {
            CompanyBenefits = new HashSet<CompanyBenefit>();
        }

        public int Id { get; set; }
        public string? Description { get; set; }
        public int? TranslationId { get; set; }

        public virtual TranslationsWeb? Translation { get; set; }
        public virtual ICollection<CompanyBenefit> CompanyBenefits { get; set; }
    }    
}
