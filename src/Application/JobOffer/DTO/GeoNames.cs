using Newtonsoft.Json;

namespace Application.JobOffer.DTO;

public class GeoNames
{
    public List<PostalCode> postalCodes { get; set; }
}

public class PostalCode
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string adminCode2 { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string adminCode1 { get; set; }
    public string adminName2 { get; set; }
    public double lng { get; set; }
    public string countryCode { get; set; }
    public string postalCode { get; set; }
    public string adminName1 { get; set; }

    [JsonProperty("ISO3166-2")]
    public string ISO31662 { get; set; }

    public string placeName { get; set; }
    public double lat { get; set; }
    public string adminCode3 { get; set; }
    public string adminName3 { get; set; }
}
