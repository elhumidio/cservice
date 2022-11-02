namespace Application.JobOffer.DTO
{
    public class JobOfferDto
    {
        public int IdjobVacancy { get; set; }
        public int Idcontract { get; set; }
        public int Identerprise { get; set; }
        public int Idbrand { get; set; }
        public int? IdzipCode { get; set; }
        public int? IdenterpriseUserG { get; set; }
        public int IdenterpriseUserLastMod { get; set; }
        public int? Idquest { get; set; }
        public int Idcountry { get; set; }
        public int Idregion { get; set; }
        public int? IdjobVacState { get; set; }
        public int IdjobVacType { get; set; }
        public int? IdjobCategory { get; set; }
        public int Idarea { get; set; }
        public int? IdsubArea { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? PublicationDate { get; set; }
        public DateTime? LastVisitorDate { get; set; }
        public bool chkFilled { get; set; }
        public string? AimwelCampaignId { get; set; }
    }
}
