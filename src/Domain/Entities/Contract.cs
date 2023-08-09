using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TContract")]
    public partial class Contract
    {
        public Contract()
        {
            InverseIdcontractParentNavigation = new HashSet<Contract>();
            ContractProducts = new HashSet<ContractProduct>();
            RegEnterpriseContracts = new HashSet<RegEnterpriseContract>();
        }

        public int Idcontract { get; set; }
        public int Identerprise { get; set; }
        public int IdenterpriseUser { get; set; }
        public int? IdcontractParent { get; set; }
        public int IdpayMethod { get; set; }
        public int Idcurrency { get; set; }
        public DateTime ContractDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string? Comment { get; set; }
        public bool ChkApproved { get; set; }
        public string? Concept { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Price { get; set; }
        public string? CorporateNameInvoicing { get; set; }
        public string? ContactInvoicing { get; set; }
        public string? CompanyTaxCodeInvoicing { get; set; }
        public string? AddressInvoicing { get; set; }
        public string? PhoneNumberInvoicing { get; set; }
        public string? FaxNumberInvoicing { get; set; }
        public bool ShopOnline { get; set; }
        public int IdbackOfUser { get; set; }
        public decimal? FinalPrice { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? CancelDate { get; set; }
        public int? IdcancelReason { get; set; }
        public bool ChkCancel { get; set; }
        public bool ChkPayFract { get; set; }
        public bool ChkContratExtension { get; set; }
        public bool ChkPromotionalCode { get; set; }
        public int Idcode { get; set; }
        public decimal? DiscPromotion { get; set; }
        public string? SalesforceId { get; set; }
        public DateTime? Sftimestamp { get; set; }
        public int? SiteId { get; set; }
        public int? OldIdcontract { get; set; }

        public virtual Contract? IdcontractParentNavigation { get; set; }
        public virtual ICollection<Contract> InverseIdcontractParentNavigation { get; set; }
        public virtual ICollection<ContractProduct> ContractProducts { get; set; }
        public virtual ICollection<RegEnterpriseConsum> RegEnterpriseConsums { get; set; }
        public virtual ICollection<RegEnterpriseContract> RegEnterpriseContracts { get; set; }
    }
}
