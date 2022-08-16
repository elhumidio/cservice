using Domain.Entities;

namespace Application.Product.DTO
{
    public partial class ProductDto
    {
        public int Idproduct { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public int? Idcountry { get; set; }
        public int? Idregion { get; set; }
        public int Idcurrency { get; set; }
        public string BaseName { get; set; } = null!;
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public bool? ChkAvailable { get; set; }
        public bool ChkPack { get; set; }
        public bool ChkShopOnline { get; set; }
        public bool ChkExtension { get; set; }
        public bool ChkService { get; set; }
        public int? IdgroupForShop { get; set; }
        public bool ChkMainForShop { get; set; }
        public bool ChkOfferForService { get; set; }
        public bool ChkPostAjob { get; set; }
        public bool ChkEmptyService { get; set; }
        public bool ChkShopCv { get; set; }
        public int? Discount { get; set; }
        public int? SpecialDiscount { get; set; }
        public bool? ChkWelcome { get; set; }
        public bool? Atsrestricted { get; set; }
        public bool? RegionRestricted { get; set; }

        public virtual TsturijobsLang Ids { get; set; } = null!;
    }
}
