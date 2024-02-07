using Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Application.ContractProducts.DTO
{
    public class AssignedUnitsByUserAndProduct
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public List<ProductUnitsDistribution>? UnitsInfoByProduct { get; set; } = new List<ProductUnitsDistribution>();
    }

    public enum ProductType
    {
        Basic,
        Superior,
        Premium,
        PremiumInternational,
        Internship
    }

    public class ProductUnitsDistribution
    {
        public string? ProductName { get; set; }
        public VacancyTypesCredits Type { get; set; }
        public int UnitsPurchasedAndValid { get; set; }
        public int AssignedUnits { get; set; }
        public int ConsumedUnits { get; set; }
        public int AvailableUnits { get; set; }
    }

    public class UnitsContainer
    {
        public int AssignedUnitsBasic { get; set; }
        public int AsssignedUnitsSuperior { get; set; }
        public int AssignedUnitsPremium { get; set; }
        public int AssignedUnitsSuperior { get; set;}
        public int AssignedUnitsPremiumInternational { get; set; }
        public int AssignedUnitsInternship { get; set; }
        public int PurchasedUnitsBasic { get; set; }
        public int PurchasedUnitsSuperior { get; set; }
        public int PurchasedUnitsPremium { get; set; }
        public int PurchasedUnitsPremiumInternational { get; set; }
        public int PurchasedUnitsInternship { get; set; }
        public int ConsumedUnitsBasic { get; set; }
        public int ConsumedUnitsSuperior { get; set; }
        public int ConsumedUnitsPremium { get; set; }
        public int ConsumedUnitsPremiumInternational { get; set; }
        public int ConsumedUnitsInternship { get; set; }

        public int AvailableUnitsBasic { get; set; }
        public int AvailableUnitsSuperior { get; set; }
        public int AvailableUnitsPremium { get; set; }
        public int AvailableUnitsPremiumInternational { get; set; }
        public int AvailableUnitsInternship { get; set; }

        public int AvailableToAssignBasic { get; set; }
        public int AvailableToAssignSuperior { get; set; }
        public int AvailableToAssignPremium { get; set; }
        public int AvailableToAssignPremiumInternational { get; set; }
        public int AvailableToAssignInternship { get; set; }




        public List<AssignedUnitsByUserAndProduct>? UnitsInfoByUser { get; set; } = new List<AssignedUnitsByUserAndProduct>();

        
    }
}
