using Application.Contracts.DTO;
using TURI.ContractService.Contract.Models;

namespace API.Converters
{
    public static class ContractConverter
    {
        public static AvailableUnitsResponse ToResponseModel(this AvailableUnitsDto item)
        {
            return new AvailableUnitsResponse
            {
                Units = item.Units,
                ContractId = item.ContractId,
                IsPack = item.IsPack,
                type = (int)item.type,
                OwnerId = item.OwnerId,
            };
        }
    }
}
