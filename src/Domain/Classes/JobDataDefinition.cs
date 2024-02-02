namespace Domain.Classes
{
    public class JobDataDefinition
    {
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public int IDCountry { get; set; }
        public int IDRegion { get; set; }
        public int IDArea { get; set; }
        public int IDJobVacancy { get; set; }
        public int IDEnterprise { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime UpdatingDate { get; set; }
        public int IDCity { get; set; }
        public string Description { get; set; }
        public int IDSite { get; set; }
        public bool ChkBlindVac { get; set; }
        public string City { get; set; }
        public int TitleId { get; set; }
    }
}
