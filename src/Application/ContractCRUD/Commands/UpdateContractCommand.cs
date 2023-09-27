using Application.Core;
using MediatR;

namespace Application.ContractCRUD.Commands
{
    public class UpdateContractCommand : IRequest<Result<bool>>
    {
        public int IdUser { get; set; }
        public int IdContract { get; set; }
        public int IdEnterprise { get; set; }
        public int IdEnterpriseUser { get; set; }
        public int? IdContractParent { get; set; }
        public int IdPayMethod { get; set; }
        public int IdCurrency { get; set; }
        public DateTime? ContractDate { get; set; }
        public int IdSite { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string? Comment { get; set; }
        public bool chkApproved { get; set; }
        public string Concept { get; set; }
        public decimal Discount { get; set; }
        public decimal Price { get; set; }
        public decimal FinalPrice { get; set; }
        public string? CorporateNameInvoicing { get; set; }
        public string? ContactInvoicing { get; set; }
        public string? CompanyTaxCodeInvoicing { get; set; }
        public string? AddressInvoicing { get; set; }
        public string? PhoneNumberInvoicing { get; set; }
        public string? FaxNumberInvoicing { get; set; }
        public bool chkShopOnline { get; set; }
        public int IdLanguage { get; set; }
        public bool hasPack { get; set; }
        public int StandardUnits { get; set; }
        public int FeaturedUnits { get; set; }
        public int FiledUnits { get; set; }
        public bool hasGestors { get; set; }
        public bool chkPayFract { get; set; }
        public int IdBackOfUser { get; set; }
    }
}
