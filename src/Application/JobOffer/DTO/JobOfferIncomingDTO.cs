namespace Application.JobOffer.DTO
{
    public class JobOfferIncomingDTO
    {

        // public string token { get; set; }
        public int ownerId { get; set; }

        public ExternalOffersData? externalData { get; set; }
        public SalaryInfo SalaryInfo { get; set; }
        public string JobTitle { get; set; }
        public string JobIndustryId { get; set; }
        public int numberVacancies { get; set; }
        public string Description { get; set; }
        public string ContractTypeId { get; set; }
        public string City { get; set; }
        public int CountryId { get; set; }
        public int RegionId { get; set; }
        public string ZipCode { get; set; }
        public int CandidateResidenceId { get; set; }
        public int DegreeId { get; set; }
        public int YearsExperienceId { get; set; }
        public string Requirements { get; set; }
        public int BrandId { get; set; }
        public int JobCategoryId { get; set; }
        public string OfferType { get; set; }
        public string RegistryType { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class ExternalOffersData
    {
        public string ApplicationEmail { get; set; }
        public string ApplicationUrl { get; set; }
        public string applicationReference { get; set; }
    }
    public class SalaryInfo
    {
        public int salaryType { get; set; } // TODO endpoints con datos tabulados
        public int minSalary { get; set; }
        public int maxSalary { get; set; }

    }
}
