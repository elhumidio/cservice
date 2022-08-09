using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("ContractPublicationRegion")]
public partial class ContractPublicationRegion
{
    public int Idcontract { get; set; }
    public int Idproduct { get; set; }
    public int Idsite { get; set; }
    public int Idregion { get; set; }
    public bool ChkActive { get; set; }
    public DateTime CreationDate { get; set; }
    public string CreationBouserId { get; set; } = null!;
    public DateTime? DeactivationDate { get; set; }
    public string? DeactivationBouserId { get; set; }
}

