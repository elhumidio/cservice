namespace Domain.DTO
{
    public class OfferInfoMin
    {
        public int? JobId { get; set; }
        public int? CvId { get; set; }
        public int? RegistrationId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? CompanyName { get; set; }
        public string? LogoUrl { get; set; }
        public int? CityId { get; set; }
        public string? CityName { get; set; }
        public int? RegionId { get; set; }
        public string? RegionName { get; set; } = string.Empty;
        public int? CountryId { get; set; }
        public string? CountryName { get; set; } = string.Empty;
        public int? NumApplies { get; set; }

    }
}
}
