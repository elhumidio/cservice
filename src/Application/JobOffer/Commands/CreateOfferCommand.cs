using ApisClient.DTO;
using Application.JobOffer.DTO;
using Domain.Enums;
using MediatR;
using System.Runtime.Serialization;

namespace Application.JobOffer.Commands
{
    [DataContract]
    public class CreateOfferCommand : IRequest<OfferModificationResult>
    {
        [DataMember]
        public int IdjobVacancy { get; set; }

        [DataMember]
        public int Idcontract { get; set; }

        [DataMember]
        public int Identerprise { get; set; }

        [DataMember]
        public int Idbrand { get; set; } //table

        [DataMember]
        public int? IdzipCode { get; set; } //verify against other service

        [DataMember]
        public int? IdenterpriseUserG { get; set; } //calculate

        [DataMember]
        public int IdenterpriseUserLastMod { get; set; } //same as IdenterpriseUserG

        [DataMember]
        public int? Idquest { get; set; }   //null

        [DataMember]
        public int Idcountry { get; set; } //table

        [DataMember]
        public int Idregion { get; set; } //table

        [DataMember]
        public int? IdjobVacState { get;    set; } //always 1

        [DataMember]
        public int IdjobVacType { get; set; } // if empty 0 else incoming one

        [DataMember]
        public int? IdjobCategory { get; set; } //table

        [DataMember]
        public int Idarea { get; set; } //table

        [DataMember]
        public int? IdsubArea { get; set; } //table

        [DataMember]
        public List<int>? IdworkPermit { get; set; }//table

        [DataMember]
        public int IdjobContractType { get; set; } //table

        [DataMember]
        public int? IdworkDayType { get; set; } //table

        [DataMember]
        public int IdsalaryType { get; set; } //table

        [DataMember]
        public int IdresidenceType { get; set; } //table

        [DataMember]
        public int Iddegree { get; set; } //table

        [DataMember]
        public int IdjobExpYears { get; set; } //table

        [DataMember]
        public int? IdquestRegistState { get; set; }

        [DataMember]
        public string Title { get; set; } // not empty

        [DataMember]
        public string? ShortDescription { get; set; } //not empty

        [DataMember]
        public string? City { get; set; }

        [DataMember]
        public int VacancyNumber { get; set; } // done ??

        [DataMember]
        public int VisitorNumber { get; set; } = 0;

        [DataMember]
        public string Description { get; set; } // not empty

        [DataMember]
        public string? Requirements { get; set; } //not empty

        [DataMember]
        public string ScheduleTime { get; set; }

        [DataMember]
        public decimal? SalaryMin { get; set; } //to study

        [DataMember]
        public decimal? SalaryMax { get; set; } //to study

        [DataMember]
        public DateTime? PublicationDate { get; set; } //getdate

        [DataMember]
        public DateTime? ModificationDate { get; set; } //getdate

        [DataMember]
        public DateTime? UpdatingDate { get; set; } //getdate

        [DataMember]
        public DateTime? FinishDate { get; set; } //not empty > getdate()

        [DataMember]
        public DateTime? FilledDate { get; set; } = null; //null

        [DataMember]
        public bool ChkPack { get; set; }

        [DataMember]
        public bool ChkBlindVac { get; set; }

        [DataMember]
        public bool ChkFilled { get; set; }

        [DataMember]
        public bool ChkDeleted { get; set; }

        [DataMember]
        public bool? ChkEnterpriseVisible { get; set; }

        [DataMember]
        public int? IdjobVacancyOld { get; set; } = null;

        [DataMember]
        public bool ChkColor { get; set; } = false;

        [DataMember]
        public bool ChkUpdateDate { get; set; }

        [DataMember]
        public int? ShowOrder { get; set; }

        [DataMember]
        public string? ForeignZipCode { get; set; }

        [DataMember]
        public bool? ChkDestacarAreaProv { get; set; }

        [DataMember]
        public bool? ChkSynerquia { get; set; }

        [DataMember]
        public string? ExternalUrl { get; set; }

        [DataMember]
        public int? SalaryCurrency { get; set; }

        [DataMember]
        public bool? ChkDisability { get; set; }

        [DataMember]
        public int? Idstatus { get; set; }

        [DataMember]
        public int? Idcity { get; set; }

        //  [DataMember]
        //   public string? CityOld { get; set; }
        [DataMember]
        public string? JobLocation { get; set; }

        [DataMember]
        public DateTime? DailyJobFinishDate { get; set; }

        [DataMember]
        public bool? ChkBlindSalary { get; set; }

        [DataMember]
        public int? ExtensionDays { get; set; }

        [DataMember]
        public int IdjobRegType { get; set; }

        [DataMember]
        public int Idsite { get; set; }

        [DataMember]
        public DateTime? LastVisitorDate { get; set; }

        [DataMember]
        public string? ZipCode { get; set; }

        [DataMember]
        public IntegrationData IntegrationData { get; set; }

        [DataMember]
        public List<JobLanguages>? JobLanguages { get; set; }

        [DataMember]
        public QuestDTO? QuestDTO { get; set; }

        [DataMember]
        public int TitleId { get; set; }

        [DataMember]
        public int TitleDenominationId { get; set; }

        [DataMember]
        public int FieldId { get; set; }
        [DataMember]
        public decimal? Latitude { get; set; }
        [DataMember]
        public decimal? Longitude { get; set; }
        [DataMember]
        public string? Address { get; set; }


        public CreateOfferCommand()
        {
            IntegrationData = new IntegrationData();
            JobLanguages = new List<JobLanguages>();
        }
    }

    public class JobLanguages
    {
        public int IdLanguage { get; set; }
        public int IdLangLevel { get; set; }
    }

    public class IntegrationData
    {
        public string? IDIntegration { get; set; }
        public string? ApplicationEmail { get; set; }
        public string? ApplicationUrl { get; set; }
        public string? ApplicationReference { get; set; }
    }
}
