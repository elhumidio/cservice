using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.JobOffer.DTO
{
    public class OfferDescriptionDTO
    {
        // Offer
        public int OfferId { get; set; }
        public string Title { get; set; }
        public string JobLocation { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Logo { get; set; }
        public string Description { get; set; }
        public int Vacancies { get; set; }
        public int TotalCandidateApplied { get; set; }
        public string SalaryMin { get; set; }
        public string SalaryMax { get; set; }
        public string SalaryCurrency { get; set; }
        public string SalaryType { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Tasks { get;set; }
        public List<string> Requirements { get; set; }
        public List<string> Skills { get; set; }
        public List<string> Preferences { get; set; }
        public List<string> Benefits { get; set; }

        // Enterprise
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLocation { get; set; }
        public string CompanyDescription { get; set; }
        public int Employees { get; set; }
        public string CompanyField { get; set; }
        public string CompanyURL { get; set; }
    }
}
