using Domain.Enums;

namespace Application.Contracts.DTO
{
    public class AvailableUnitsDto
    {
        public int Units { get; set; }
        public int ContractId { get; set; }
        public bool IsPack { get; set; }
        public VacancyType type { get; set; }
        public int OwnerId { get; set; }
    }

    public class AvailableUnitsResult
    {
        public List<AvailableUnitsDto> Units { get; set; }
    }
}
