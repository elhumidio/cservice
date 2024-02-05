using Domain.DTO;
using TURI.ContractService.Contracts.Contract.Models.Response;

namespace API.Converters
{
    public static class JobAlertConverter
    {
        public static JobTitleDenominationsResponse ToModel(this JobTitleDenominationsDto item)
        {
            return new JobTitleDenominationsResponse
            {
                Id = item.Id,
                Denomination = item.Denomination,
                FkJobTitle = item.FkJobTitle
            };
        }
    }
}
