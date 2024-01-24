namespace Domain.Entities
{
    public partial class CompanyBenefit
    {
        public int CompanyId { get; set; }
        public int BenefitId { get; set; }
        

        public virtual Benefit Benefit { get; set; } = null!;
    }
}
