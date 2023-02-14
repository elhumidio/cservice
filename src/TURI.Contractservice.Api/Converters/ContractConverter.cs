using Amazon.Auth.AccessControlPolicy;
using Application.Contracts.DTO;
using AutoMapper;
using TURI.ContractService.Contract.Models;

namespace API.Converters
{
    public static class ContractConverter
    {
        public static List<AvailableUnitsResponse> ToResponseModel(this List<AvailableUnitsDto> list)
        {
            //no funciona, hay que hacerlo de otra forma
            List<AvailableUnitsResponse> targetList = new List<AvailableUnitsResponse>(list.Cast<AvailableUnitsResponse>());
            return targetList;
        }
    }
}
