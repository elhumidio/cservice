using System.ComponentModel.DataAnnotations.Schema;

namespace API.DataContext
{
    [Table("TEnterprise")]
    public partial class Enterprise
    {
        public int Identerprise { get; set; }
        public int IdlegalForm { get; set; }
        public int IdhowMeet { get; set; }
        public int Idcountry { get; set; }
        public int Idregion { get; set; }
        public int IdzipCode { get; set; }
        public int Idfield { get; set; }
        public int IdbackOfUser { get; set; }
        public int? IdenterpriseOld { get; set; }
        public int? Idcentral { get; set; }
        public int? IdcountryInvoicing { get; set; }
        public int? IdregionInvoicing { get; set; }
        public int? IdzipCodeInvoicing { get; set; }

        /// <summary>
        /// Employees Number
        /// </summary>
        public int Employees { get; set; }

        public int VisitsNumber { get; set; }
        public string City { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string CorporateName { get; set; } = null!;
        public string CompanyTaxCode { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string Description { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public DateTime LastModification { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? FaxNumber { get; set; }
        public string? UrlWeb { get; set; }
        public string? Urlturijobs { get; set; }
        public string? ContactInvoicing { get; set; }
        public string? CityInvoicing { get; set; }
        public string? AddressInvoicing { get; set; }
        public string? Iban { get; set; }
        public string? Ban { get; set; }
        public string? DescriptionMeet { get; set; }
        public string? ExpiryReason { get; set; }
        public bool? ChkActive { get; set; }
        public string? NameInvoicing { get; set; }
        public string? CorporateNameInvoicing { get; set; }
        public string? PhoneNumberInvoicing { get; set; }
        public string? FaxNumberInvoicing { get; set; }
        public int? IdlegalFormInvoicing { get; set; }
        public short? IdexpiryReason { get; set; }
        public string? CompanyTaxCodeInvoicing { get; set; }
        public int? IdsubField { get; set; }
        public int? Idstatus { get; set; }
        public string? ZipCode { get; set; }
        public string? ZipCodeInvoicing { get; set; }
        public int? Idcity { get; set; }
        public string? CityOld { get; set; }
        public int? IdcityInvoicing { get; set; }
        public string? Iderp { get; set; }
        public string? SalesforceId { get; set; }
        public DateTime? Sftimestamp { get; set; }
        public string? Region { get; set; }
        public string? RegionInvoicing { get; set; }
        public int? IdenterpriseType { get; set; }
        public int? SiteId { get; set; }
        public int? OldIdenterprise { get; set; }
        public int? ParentAccount { get; set; }
        public string? AccountStatus { get; set; }
        public int? ReplacedBy { get; set; }
        public int? DepartmentOf { get; set; }
        public int? MacroSector { get; set; }
        public bool? Ats { get; set; }
        public string? Atsname { get; set; }
        public string? PrefixPhoneNumber { get; set; }
        public string? PrefixPhoneNumberInvoicing { get; set; }
    }
}
