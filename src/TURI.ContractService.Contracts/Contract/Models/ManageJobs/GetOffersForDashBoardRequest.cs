namespace TURI.ContractService.Contracts.Contract.Models.ManageJobs
{
    public class GetOffersForDashBoardRequest
    {
        public int CompanyId { get; set; }

        public int Site { get; set; }

        public int LangId { get; set; }

        public bool Actives { get; set; }

        public bool Filed { get; set; }

        public bool All { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? Location { get; set; }

        public int BrandId { get; set; }

        public string? Title { get; set; }
    }
}
