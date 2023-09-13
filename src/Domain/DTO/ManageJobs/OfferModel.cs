using Domain.Entities;

namespace Domain.DTO.ManageJobs
{
    public class OfferModel : JobVacancy
    {
        public string Name { get; set; }
        public bool IsWelcome { get; set; }
        public int IDEnterpriseUserG { get; set; }
        public int IDJobVacType { get; set; }
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
        public bool chkPack { get; set; }
        public bool chkBlindVac { get; set; }
        public bool chkFilled { get; set; }
        public bool chkDeleted { get; set; }
        public bool chkUpdateDate { get; set; }
        public bool chkColor { get; set; }
        public bool chkEnterpriseVisible { get; set; }
        public bool IsOldOffer { get; set; }
        public string? FormData { get; set; }
        public bool isPending { get; set; }
        public bool isCancel { get; set; }
        public bool chkAllCountry { get; set; }
        public string? JobVacType { get; set; }
        public int IDJobRegType { get; set; }
        public string? CCAA { get; set; }

        public OfferModel()
        {
            IdjobVacancy = -1;
            IDJobRegType = -1;
            Idcontract = -1;
            IDJobVacType = -1;
            IDProduct = -1;
            IDEnterpriseUserG = -1;
            Title = string.Empty;
            City = string.Empty;
            ForeignZipCode = string.Empty;
            ExpYears = string.Empty;
            ZipCode = string.Empty;
            ZipCodeCity = string.Empty;
            DegreeName = string.Empty;
            SalaryType = string.Empty;
            SalaryMin1 = -1;
            AreaName = string.Empty;
            SubdomainName = string.Empty;
            FieldName = string.Empty;
            EnterpriseName = string.Empty;
            Identerprise = -1;
            RegionName = string.Empty;
            Idregion = -1;
            Caducity = -1;
            CaducityShow = -1;
            RegNumber = 0;
            NNuevos = 0;
            NPendientes = 0;
            NEvaluating = 0;
            NFinalistas = 0;
            NDescartados = 0;
            RegPercent = string.Empty;
            chkPack = false;
            chkBlindVac = false;
            chkFilled = false;
            chkDeleted = false;
            chkUpdateDate = false;
            chkColor = false;
            chkEnterpriseVisible = false;
            IsOldOffer = false;
            FormData = string.Empty;
            isPending = false;
            isCancel = false;
            chkAllCountry = false;
            JobVacType = string.Empty;
        }
    }
}
