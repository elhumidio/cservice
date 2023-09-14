namespace Domain.DTO.Requests
{
    public class ManageJobsArgs
    {
        public int CompanyId { get; set; }
        public int Site { get; set; }
        public int LangId { get; set; }
        public bool Actives { get; set; }
        public bool Filed { get; set; }
        public bool All { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
