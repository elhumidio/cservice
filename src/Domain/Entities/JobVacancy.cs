using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TJobVacancy")]
    public partial class JobVacancy
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
        public int? IdworkPermit { get; set; }
        public int IdjobContractType { get; set; }
        public int? IdworkDayType { get; set; }
        public int IdsalaryType { get; set; }
        public int IdresidenceType { get; set; }
        public int Iddegree { get; set; }
        public int IdjobExpYears { get; set; }
        public int? IdquestRegistState { get; set; }
        public string Title { get; set; } = null!;
        public string? ShortDescription { get; set; }
        public string? City { get; set; }
        public int VacancyNumber { get; set; }
        public int VisitorNumber { get; set; }
        public string Description { get; set; } = null!;
        public string? Requirements { get; set; }
        public string ScheduleTime { get; set; } = null!;
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public DateTime UpdatingDate { get; set; }
        public DateTime FinishDate { get; set; }
        public DateTime? FilledDate { get; set; }
        public bool ChkPack { get; set; }
        public bool ChkBlindVac { get; set; }
        public bool ChkFilled { get; set; }
        public bool ChkDeleted { get; set; }
        public bool? ChkEnterpriseVisible { get; set; }
        public int? IdjobVacancyOld { get; set; }
        public bool ChkColor { get; set; }
        public bool ChkUpdateDate { get; set; }
        public int? ShowOrder { get; set; }
        public string? ForeignZipCode { get; set; }
        public bool? ChkDestacarAreaProv { get; set; }
        public bool? ChkSynerquia { get; set; }
        public string? ExternalUrl { get; set; }
        public int? SalaryCurrency { get; set; }
        public bool? ChkDisability { get; set; }
        public int? Idstatus { get; set; }
        public int? Idcity { get; set; }
        public string? CityOld { get; set; }
        public string? JobLocation { get; set; }
        public DateTime? DailyJobFinishDate { get; set; }
        public bool? ChkBlindSalary { get; set; }
        public int? ExtensionDays { get; set; }
        public int IdjobRegType { get; set; }
        public int Idsite { get; set; }
        public int? OldIdPort { get; set; }
        public int? OldIdMx { get; set; }
        public int? IdClosingReason { get; set; }
        public DateTime? LastVisitorDate { get; set; }
        public string? IntegrationId { get; set; }
        public int? Isco { get; set; }
        public int? TitleId { get; set; }
        public int? FieldId { get; set; }
        public bool? AllowChat { get; set; }
    }
}
