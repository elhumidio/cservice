namespace TURI.ContractService.Contracts.Contract.Models.ManageJobs
{
    public class ManageJobsResponse
    {
        public OfferModelResponse[]? Offers { get; set; }
    }

    public class OfferModelResponse
    {
        public string Name { get; set; }
        public int IdjobVacancy { get; set; }
        public int Idcontract { get; set; }
        public int IdjobVacType { get; set; }
        public int IdjobRegType { get; set; }
        public int Idregion { get; set; }
        public int Idsite { get; set; }
        public DateTime PublicationDate { get; set; }
        public int IdenterpriseUserG { get; set; }
        public string Title { get; set; }
        public int Identerprise { get; set; }
        public bool ChkPack { get; set; }
        public string CityUrl { get; set; }
        public int Idcountry { get; set; }
        public bool ChkBlindVac { get; set; }
        public bool ChkFilled { get; set; }
        public bool ChkDeleted { get; set; }
        public bool ChkUpdateDate { get; set; }
        public bool ChkColor { get; set; }
        public bool ChkEnterpriseVisible { get; set; }
        public int Idcity { get; set; }
        public string City { get; set; }
        public int Idbrand { get; set; }
        public int Idstatus { get; set; }
        public int ExtensionDays { get; set; }
        public DateTime UpdatingDate { get; set; }
        public DateTime FinishDate { get; set; }
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
        public bool? CanSeeFilters { get; set; }
        public string? JobRegType { get; set; }
    }
}
