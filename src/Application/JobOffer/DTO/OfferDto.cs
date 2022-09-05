using Application.JobOffer.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.JobOffer.DTO
{
    public class OfferResultDto
    {
        public int IdjobVacancy { get; set; }
        public int Idcontract { get; set; }
        public int Identerprise { get; set; }
        public int Idbrand { get; set; }
        public int? IdzipCode { get; set; }
        public int? IdenterpriseUserG { get; set; }
        public int Idcountry { get; set; }
        public int Idregion { get; set; }
        public int IdjobVacType { get; set; }
        public int Idarea { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? PublicationDate { get; set; }
        public bool chkFilled { get; set; }
        public IntegrationData IntegrationData { get; set; }

        public OfferResultDto()
        {
            IntegrationData = new IntegrationData();
        }
    }
}
