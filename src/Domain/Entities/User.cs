using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TSUser")]
    public partial class User
    {
        public int Idsuser { get; set; }
        public int IdstypeUser { get; set; }
        public int Idslanguage { get; set; }
        public string Email { get; set; } = null!;
        public byte[]? WordOfPass { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModification { get; set; }
        public DateTime LastLoggin { get; set; }
        public byte NumberRetry { get; set; }
        public bool ChkNewsLetterCand { get; set; }
        public bool ChkNewsLetterEnt { get; set; }
        public bool? ChkActive { get; set; }
        public string Password { get; set; } = null!;
        public string? Source { get; set; }
        public string? Medium { get; set; }
        public string? Campaign { get; set; }
        public string? Keyword { get; set; }
        public int? IdslanguagePref { get; set; }
        public bool? ChkEmailMarketing { get; set; }
        public short? IdexpiryReason { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? OptoutHash { get; set; }
        public int? SiteId { get; set; }
        public int? OldIdsuser { get; set; }
        public string? EncryptionAlgorithm { get; set; }
    }
}
