namespace Domain.DTO.ManageJobs
{
    public class IsOldOfferArgs
    {
        #region Properties

        public DateTime OfferPublicationDate { get; set; }
        public bool OfferCheckPack { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractFinishDate { get; set; }
        public int OfferIDJobVacType { get; set; }
        public int ProductDuration { get; set; }
        public int ExtensionDays { get; set; }

        #endregion Properties
    }
}
