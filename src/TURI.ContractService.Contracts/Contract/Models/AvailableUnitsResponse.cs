namespace TURI.ContractService.Contract.Models;

public class AvailableUnitsResponse
{
    public int Units { get; set; }
    public int ContractId { get; set; }
    public bool IsPack { get; set; }
    public int type { get; set; }
    public int OwnerId { get; set; }
}
