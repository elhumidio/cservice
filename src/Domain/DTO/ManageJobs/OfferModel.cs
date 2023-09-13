using Domain.Entities;

namespace Domain.DTO.ManageJobs
{
    public class OfferModel : JobVacancy
    {
        public string Name { get; set; }
        public bool IsWelcome { get; set; }
        public int IDProduct { get; set; }
        public int Caducity { get; set; }
        public int CaducityShow { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractFinishDate { get; set; }
        public string ExpYears { get; set; }
        public string? ZipCode { get; set; }
        public string? ZipCodeCity { get; set; }
        public string? DegreeName { get; set; }
        public string? SalaryType { get; set; }
        public int SalaryMin1 { get; set; }
        public string? AreaName { get; set; }
        public string? SubdomainName { get; set; }
        public string? FieldName { get; set; }
        public string? EnterpriseName { get; set; }
        public string? RegionName { get; set; }
        public int RegNumber { get; set; }
        public int NNuevos { get; set; }
        public int NPendientes { get; set; }
        public int NEvaluating { get; set; }
        public int NFinalistas { get; set; }
        public int NDescartados { get; set; }
        public string? RegPercent { get; set; }                
        public bool IsOldOffer { get; set; }
        public string? FormData { get; set; }
        public bool isPending { get; set; }
        public bool isCancel { get; set; }
        public bool chkAllCountry { get; set; }
        public string? JobVacType { get; set; }        
        public string? CCAA { get; set; }
        public string? OfferUrl { get; set; }
        public string? CityUrl { get; set; }


    }
}
