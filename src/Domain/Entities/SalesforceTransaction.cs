namespace Domain.Entities
{
    public partial class SalesforceTransaction
    {
        public int IdsalesforceTransaction { get; set; }
        public string ObjectTypeName { get; set; } = null!;
        public int TurijobsId { get; set; }
        public bool? Success { get; set; }
        public string? ErrorText { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public int? Idsite { get; set; }
    }
}
